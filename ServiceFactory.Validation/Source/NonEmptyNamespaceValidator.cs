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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Globalization;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.Modeling;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Validate that a namespace is not empty.
    /// </summary>
    [ConfigurationElementType(typeof(CustomValidatorData))] 
    public class NonEmptyNamespaceValidator : Validator<string>
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public NonEmptyNamespaceValidator(NameValueCollection attributes)
            :
            base(null, null)
        {

        }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            ModelElement mel = currentTarget as ModelElement;

            if (mel == null)
                return;

            string melName = string.Empty;
            DomainClassInfo.TryGetName(mel, out melName);

            if (String.IsNullOrEmpty(objectToValidate))
            {
                this.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, melName), currentTarget, key);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get
            {
                return Resources.NonEmptyNamespaceValidator;
            }
        }
    }
}
