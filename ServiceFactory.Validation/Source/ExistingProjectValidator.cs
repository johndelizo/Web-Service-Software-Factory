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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that a project exist ont the solution
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// 
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class ExistingProjectValidator : Validator<string>
	{
		private string elementNameProperty;
		private string projectProperty;

		public ExistingProjectValidator(NameValueCollection attributes)
			: base(null, null)
		{
			elementNameProperty = attributes.Get("elementNameProperty") ?? "Name";
			projectProperty = attributes.Get("projectProperty") ?? "ImplementationProject";
		}

        [SuppressMessage("Microsoft.Design","CA1031:DoNotCatchGeneralExceptionTypes")]
		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			string elementNamePropertyValue = ValidationEngine.GetUniquePropertyValue(currentTarget, elementNameProperty);
			string projectPropertyValue = ValidationEngine.GetUniquePropertyValue(currentTarget, projectProperty);
			ModelElement element = currentTarget as ModelElement;
			IServiceProvider serviceProvider = element.Store as IServiceProvider;

			if(string.IsNullOrEmpty(projectPropertyValue))
			{
				//The Microsoft.Practices.EnterpriseLibrary.Validation.Validators.StringLengthValidator is fired
				return;
			}

			try
			{
				IVsSolution vsSolution = GetService<IVsSolution, SVsSolution>(serviceProvider);
				using (HierarchyNode hNode = new HierarchyNode(vsSolution, projectPropertyValue))
				{
					if (hNode == null)
					{
						validationResults.AddResult(
							new ValidationResult(string.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, elementNamePropertyValue), objectToValidate, key, String.Empty, this)
							);
					}
				}
			}
			catch(Exception)
			{
				//Thrown if Project doesn't exist on solution
				validationResults.AddResult(
					new ValidationResult(string.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, elementNamePropertyValue), objectToValidate, key, String.Empty, this)
					);
			}
		}

		protected override string DefaultMessageTemplate
		{
			get { return Resources.ProjectDoesntExistMessage; }
		}

		private static TInterface GetService<TInterface, TImpl>(IServiceProvider serviceProvider)
		{
			if(serviceProvider == null)
			{
				return default(TInterface);
			}

			return (TInterface)serviceProvider.GetService(typeof(TImpl));
		}
	}
}
