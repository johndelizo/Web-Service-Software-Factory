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
using Microsoft.Practices.ServiceFactory.HostDesigner;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Validate that only one Address is ""
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class EmptyAddressCollectionValidator : Validator<IEnumerable<Endpoint>>
    {
        int emptyAddressCount;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public EmptyAddressCollectionValidator(NameValueCollection attributes)
            : base(null, null)
        {
        }

        protected override void DoValidate(IEnumerable<Endpoint> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            emptyAddressCount = 0;

            foreach (Endpoint endpoint in objectToValidate)
            {
                if (string.IsNullOrEmpty(endpoint.Address))
                {
                    emptyAddressCount++;
                }
            }

            if (emptyAddressCount > 1)
            {
                validationResults.AddResult(
                    new ValidationResult(string.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, (currentTarget as ServiceDescription).Name), objectToValidate, key, String.Empty, this)
                    );
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.OnlyOneEmptyAddressMessage; }
        }
    }
}
