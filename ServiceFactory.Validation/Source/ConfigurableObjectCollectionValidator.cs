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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections;
using Microsoft.Practices.Modeling.CodeGeneration;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Performs validation on a collection of objects by applying the validation rules specified for a supplied type.
	/// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
    public class ConfigurableObjectCollectionValidator<T> : ConfigurableObjectValidator<T>
    {
        public ConfigurableObjectCollectionValidator(NameValueCollection configuration)
            : base(configuration)
        {
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults validationResults)
        {
            if (objectToValidate != null)
            {
                IEnumerable enumerable = objectToValidate as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object element in enumerable)
                    {
                        base.DoValidate(element, element, null, validationResults);
                    }
                }
            }
        }
    }
}
