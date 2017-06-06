//===============================================================================
// Microsoft patterns & practices
// Web Service Software Factory 2010
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.Description;
using System.Xml.Schema;
using System.Xml;
using System.Globalization;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using System.CodeDom;
using System.Runtime.Serialization;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class XsdElementValidator : Validator<string>
	{
		private const string defaultSchemaDirectory = "schemas";
		private string schemaDirectory;
		private string invalidFilePathMessage;
		private string notCompliantWithDataContractSerializerMessage;

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
		public XsdElementValidator(NameValueCollection attributes)
			: base(null, null)
		{
			if (attributes == null)
			{
				return;
			}

			invalidFilePathMessage = attributes.Get("invalidFilePathMessage") ?? Resources.XsdElementInvalidFilePathMessage;
			schemaDirectory = attributes.Get("schemaDirectory") ?? defaultSchemaDirectory;
			notCompliantWithDataContractSerializerMessage = attributes.Get("notCompliantWithDataContractSerializerMessage") ?? 
															Resources.NotCompliantWithDataContractSerializerMessage;
		}

		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (string.IsNullOrEmpty(objectToValidate))
            {
                return;
            }

			ModelElement mel = currentTarget as ModelElement;
            if (mel == null ||
				(!(mel is XsdMessage) && !(mel is XsdElementFault)))
            {
                return;
            }

            XmlSchemaElementMoniker elementUri = new XmlSchemaElementMoniker(objectToValidate);
            // It's a primite type
            if(string.IsNullOrEmpty(elementUri.ElementName))
            {
                return;
            }

            string fileName = Path.GetFileName(elementUri.XmlSchemaPath);
			string fullPath = GetXsdFullPath(mel, elementUri.XmlSchemaPath);
			
			
			string melName = string.Empty;
			DomainClassInfo.TryGetName(mel, out melName);
			
            if(string.IsNullOrEmpty(fullPath))
			{
                this.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentUICulture, this.invalidFilePathMessage, melName, fileName, schemaDirectory), currentTarget, key);
                return;
            }

            XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(IsXmlSerializer(mel));

			CodeCompileUnit unit = null;
			try
			{
				unit = generator.GenerateCodeCompileUnit(fullPath);
			}
			catch (InvalidDataContractException exception)
			{
				this.LogValidationResult(validationResults, exception.Message, currentTarget, key);
				return;
			}
			catch (InvalidSerializerException serializationException)
			{
				if (!IsXmlSerializer(mel))
				{
					this.LogValidationResult(validationResults, 
						string.Format(CultureInfo.CurrentUICulture, 
						this.notCompliantWithDataContractSerializerMessage + ". " + serializationException.Message, 
						fileName), currentTarget, key);
					return;
				}
			}
	
            foreach (CodeNamespace ns in unit.Namespaces)
            {
				foreach (CodeTypeDeclaration codeType in ns.Types)
				{
					if (codeType.Name.Equals(elementUri.ElementName, StringComparison.Ordinal))
					{
						return;
					}
				}
            }

			this.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, melName, elementUri.ElementName, fileName), currentTarget, key);
        }

		protected override string DefaultMessageTemplate
		{
			get { return Resources.XsdElementValidatorMessage; }
		}

		/// <summary>
		/// Gets the xsd full path.
		/// </summary>
		/// <param name="resourceItem">The resource item.</param>
		/// <returns></returns>
		private string GetXsdFullPath(ModelElement mel, string xsdFile)
		{
			IVsSolution solution = GetService<IVsSolution, SVsSolution>(mel);
			using (HierarchyNode rootNode = new HierarchyNode(solution))
			using (HierarchyNode file = rootNode.RecursiveFindByName(xsdFile))
			{
				if (file != null && File.Exists(file.Path))
				{
					return file.Path;
				}
				else
				{
					return string.Empty;
				}
			}
		}

		private TInterface GetService<TInterface, TImpl>(ModelElement mel)
		{
			return (TInterface)mel.Store.GetService(typeof(TImpl));
		}

		private bool IsXmlSerializer(ModelElement mel)
		{
			SerializerType serializer = mel is XsdMessage ? ((XsdMessage)mel).ServiceContractModel.SerializerType :
															((XsdElementFault)mel).Operation.ServiceContractModel.SerializerType;

			return serializer == SerializerType.XmlSerializer;
		}
	}
}
