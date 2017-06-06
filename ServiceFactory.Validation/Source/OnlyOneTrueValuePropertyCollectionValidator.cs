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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.CodeGeneration;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
    /// Validate that only one element in a collection of Operation has its value set to True.
	/// </summary>
	/// <typeparam name="T"></typeparam>
    /// 
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class OnlyOneTrueValuePropertyCollectionValidator : Validator<IEnumerable<Operation>>
	{
        private string collectionElementUniqueIdProperty;
        private const string defaultUniqueIdProperty = "IsTerminating";
        public OnlyOneTrueValuePropertyCollectionValidator(NameValueCollection attributes)
			: base(null, null)
		{
            collectionElementUniqueIdProperty = defaultUniqueIdProperty;
            if (attributes != null)
            {
                collectionElementUniqueIdProperty = attributes.Get("collectionElementUniqueIdProperty") ?? defaultUniqueIdProperty;
            }
		}

		protected override void DoValidate(IEnumerable<Operation> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
            bool hasTruePropertyValue = false;
			foreach (Operation item in objectToValidate)
			{
                if (item.ObjectExtender != null)
                {
                    string propertyValue = GetPropertyValue(item, collectionElementUniqueIdProperty);
                    if (String.IsNullOrEmpty(propertyValue))
                    {
                        return;
                    }

                    bool result = bool.Parse(propertyValue);

                    // Only check for properties set to True
                    if (result)
                    {
                        if (hasTruePropertyValue)
                        {
                            validationResults.AddResult(
                                new ValidationResult(String.Format(CultureInfo.CurrentUICulture,this.MessageTemplate,collectionElementUniqueIdProperty), objectToValidate, key, String.Empty, this)
                                );
                        }
                        else
                        {
                            hasTruePropertyValue = true;
                        }
                    }
                }
			}
		}

        private string GetPropertyValue(Operation operation, string propertyName)
        {
            PropertyInfo property = operation.ObjectExtender.GetType().GetProperty(propertyName);
            if (property != null)
            {
                return property.GetValue(operation.ObjectExtender, null).ToString();
            }

            return null;
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.OnlyOneTrueValuePropertyMessage; }
        }
    }
}
