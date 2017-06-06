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
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Globalization;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.Modeling;
using System.Reflection;
using System.ServiceModel;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Validate that a wrapped message has only one body part.
    /// </summary>
    [ConfigurationElementType(typeof(CustomValidatorData))] 
    public class SessionModeValidator : Validator<SessionMode>
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public SessionModeValidator(NameValueCollection attributes)
            : base(null, null)
        {
        }

        protected override void DoValidate(SessionMode objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
             // Validate only if it's wrapped
            if (objectToValidate == SessionMode.Required)
            {
                return;
            }
                
            ServiceContract contract = GetModelElement(currentTarget) as ServiceContract;            
            if (contract == null)            
                return;
             
            if (!HasValidSessionProperties(contract))
            {
                string melName = string.Empty;
                DomainClassInfo.TryGetName(contract, out melName);
                this.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, melName), currentTarget, key);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.SessionModeValidatorMessage; }
        }

        private bool HasValidSessionProperties(ServiceContract contract)
        {
            foreach (Operation operation in contract.Operations)
            {
                if (false == GetExtenderProperty<bool>(operation.ObjectExtender, "IsInitiating") ||
                    true == GetExtenderProperty<bool>(operation.ObjectExtender, "IsTerminating"))
                {
                    return false;
                }
            }
            return true;
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

        private T GetExtenderProperty<T>(object extender, string propertyName)
        {
            if (extender != null)
            {
                PropertyInfo property = extender.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    return (T)property.GetValue(extender, null);
                }
            }
            return default(T);
        }      
    }
}
