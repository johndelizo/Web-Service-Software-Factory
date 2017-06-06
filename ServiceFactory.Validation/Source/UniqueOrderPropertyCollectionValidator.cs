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
using System.Reflection;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.ServiceFactory.DataContracts;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Modeling;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class UniqueOrderPropertyCollectionValidator : Validator<IEnumerable<DataMember>>
	{
		private HybridDictionary elements = new HybridDictionary();
		private const string propertyName = "Order";

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
		public UniqueOrderPropertyCollectionValidator(NameValueCollection attributes)
			: base(null, null)
		{
		}

		protected override void DoValidate(IEnumerable<DataMember> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			bool validate = true;

			if(currentTarget is Contract)
			{
				Contract contract = currentTarget as Contract;

				if(contract.ObjectExtender != null)
				{
					PropertyInfo property = contract.ObjectExtender.GetType().GetProperty("OrderParts");

					if(property != null)
					{
						validate = (bool)property.GetValue(contract.ObjectExtender, null);
					}
				}
			}

			if(validate)
			{
				// Clear elements
				elements.Clear();

				string melName = GetObjectName(currentTarget);

				foreach(DataMember dcElement in objectToValidate)
				{
					object orderValue = GetOrderValue(dcElement);

					if(orderValue == null)
					{
						return;
					}

					if(elements.Contains(orderValue))
					{
						validationResults.AddResult(
							new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, melName, propertyName), currentTarget, key, String.Empty, this));
					}
					else
					{
						elements.Add(orderValue, true);
					}
				}
			}
		}

		protected override string DefaultMessageTemplate
		{
			get { return Resources.UniqueOrderPropertyCollectionValidator; }
		}

		private object GetOrderValue(DataMember element)
		{
			if(element.ObjectExtender != null)
			{
				PropertyInfo property = element.ObjectExtender.GetType().GetProperty(propertyName);
				if(property != null)
				{
					object value = property.GetValue(element.ObjectExtender, null);
					return value;
				}
			}

			return null;
		}

		/// <summary>
		/// Return the name of the object.
		/// </summary>
		/// <remarks>
		/// This returns the name of the model element.
		/// </remarks>
		/// <param name="namedObject">The object.</param>
		/// <returns>The name of the object.</returns>
		private string GetObjectName(object named)
		{
			ModelElement modelElement = named as ModelElement;

			if(modelElement == null)
				return named.ToString();
				
			string modelElementName = string.Empty;
			if (!DomainClassInfo.TryGetName(modelElement, out modelElementName))
			{
				//if model element doesnt have a name, we return the class' displayname
				DomainClassInfo classInfo = modelElement.GetDomainClass();
				modelElementName = classInfo.DisplayName;
			}

			return modelElementName;
		}
	}
}
