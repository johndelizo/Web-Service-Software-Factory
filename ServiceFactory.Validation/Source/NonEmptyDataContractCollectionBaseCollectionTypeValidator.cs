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

namespace Microsoft.Practices.ServiceFactory.Validation
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class NonEmptyDataContractCollectionBaseCollectionTypeValidator : Validator
    {
        private string propertyName = "CollectionType";
        public NonEmptyDataContractCollectionBaseCollectionTypeValidator(NameValueCollection attributes) 
            : base(null, null) 
        {
            if (attributes == null)
                return;

            if(!String.IsNullOrEmpty(attributes.Get("propertyName")))
                propertyName = attributes.Get("propertyName");
       }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            DataContractCollectionBase dc = currentTarget as DataContractCollectionBase;

            if (dc == null)
                return;

            if(PropertyIsEmpty(dc))
                validationResults.AddResult(
                    new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, dc.Name, propertyName), currentTarget, key, String.Empty, this));
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.NonEmptyDataContractCollectionBaseCollectionTypeValidator; }
        }

        private bool PropertyIsEmpty(DataContractCollectionBase collection)
        {
            if (collection.ObjectExtender != null)
            {
                PropertyInfo property = collection.ObjectExtender.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    return property.GetValue(collection.ObjectExtender, null) == null;
                }
            }
            return true;
        }
    }
}
