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
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
    /// Validate that only XsdElementFaults are added when using XmlSerializer
	/// </summary>
	/// <typeparam name="T"></typeparam>
    /// 
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class XsdElementFaultCollectionValidator : Validator<IEnumerable<Fault>>
	{
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public XsdElementFaultCollectionValidator(NameValueCollection attributes)
			: base(null, null)
		{
		}

		protected override void DoValidate(IEnumerable<Fault> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
            foreach (Fault item in objectToValidate)
            {
                XsdElementFault xsdElementFault = item as XsdElementFault;

                if (xsdElementFault != null)
                {
                    if (xsdElementFault.Operation.ServiceContractModel.SerializerType == SerializerType.XmlSerializer)
                    {
                        validationResults.AddResult(
                        new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, xsdElementFault.Operation.Name, xsdElementFault.Name), objectToValidate, key, String.Empty, this)
                        );
                    }
                }
            }
		}

        protected override string DefaultMessageTemplate
        {
            get { return Resources.XsdElementFaultCollectionValidatoryMessage; }
        }
    }
}
