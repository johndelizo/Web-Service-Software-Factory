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
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.Modeling;
using System.Reflection;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Validate that a wrapped message has only one body part.
    /// </summary>
    [ConfigurationElementType(typeof(CustomValidatorData))] 
    public class IsWrappedValidator : Validator<bool>
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public IsWrappedValidator(NameValueCollection attributes)
            :
            base(null, null)
        {

        }

        protected override void DoValidate(bool objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
             // Validate only if it's wrapped
            if (objectToValidate)
            {
                return;
            }
                
            ModelElement mel = GetModelElement(currentTarget);

            if (mel == null)
                return;

            string melName = string.Empty;
            DomainClassInfo.TryGetName(mel, out melName);

            if(CountMessageParts(mel) > 1)
            {
                this.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, melName), currentTarget, key);
            }
        }

        private ModelElement GetModelElement(object currentTarget)
        {
            ModelElement element = null;
            PropertyInfo prop = currentTarget.GetType().GetProperty("ModelElement");
            if (prop != null)
            {
                element = (ModelElement)prop.GetValue(currentTarget, null);
            }

            return element;
        }

        private int CountMessageParts(object mel)
        {
            PropertyInfo prop = mel.GetType().GetProperty("MessageParts");
            if (prop != null)
            {
                LinkedElementCollection<MessagePart> parts = (LinkedElementCollection<MessagePart>)prop.GetValue(mel, null);
                return parts.Count;
            }

            return 0;
        }

        protected override string DefaultMessageTemplate
        {
            get
            {
                return Resources.IsWrappedValidatorMessage;
            }
        }
    }
}
