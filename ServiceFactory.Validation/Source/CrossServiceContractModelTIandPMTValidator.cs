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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.ServiceFactory.HostDesigner;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.VisualStudio.Modeling.Integration;
using System.Collections.ObjectModel;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Validate that the service contract model reference has associated a Technology Information and PMT.
    /// </summary>
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class CrossServiceContractModelTIandPMTValidator : CrossModelReferenceValidator
    {
        private const string crossModelReferenceValidatorMessageKeyName = "crossModelReferenceValidatorMessage";
        private string currentMessageTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossDataContractModelTIandPMTValidator"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public CrossServiceContractModelTIandPMTValidator(NameValueCollection attributes)
            : base(attributes)
        {
            if (attributes == null)
            {
                return;
            }

            currentMessageTemplate = String.IsNullOrEmpty(attributes.Get(crossModelReferenceValidatorMessageKeyName)) ?
                Resources.CrossServiceContractModelTIandPMTValidator :
                attributes.Get(crossModelReferenceValidatorMessageKeyName);
        }

        /// <summary>
        /// Does the validate.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The current target.</param>
        /// <param name="key">The key.</param>
        /// <param name="validationResults">The validation results.</param>
        protected override void DoValidate(ModelBusReference objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            this.MessageTemplate = currentMessageTemplate;

            base.DoValidate(objectToValidate, currentTarget, key, validationResults);

            if (!validationResults.IsValid)
            {
                return;
            }

            ServiceReference serviceReference = currentTarget as ServiceReference;

            if (serviceReference == null)
            {
                return;
            }

            using (ModelBusReferenceResolver resolver = new ModelBusReferenceResolver())
            {
                ModelElement referencedElement = resolver.Resolve(objectToValidate);
                if (referencedElement != null)
                {
                    ServiceContractModel dcm = referencedElement.Store.ElementDirectory.FindElements<ServiceContractModel>()[0];
                    if (dcm.ImplementationTechnology == null ||
                        String.IsNullOrWhiteSpace(dcm.ProjectMappingTable) ||
                        !dcm.ImplementationTechnology.Name.Equals(GetItName(currentTarget), StringComparison.OrdinalIgnoreCase))
                    {
                        validationResults.AddResult(
                            new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, ValidatorUtility.GetTargetName(currentTarget)), currentTarget, key, String.Empty, this));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the default message template.
        /// </summary>
        /// <value>The default message template.</value>
        protected override string DefaultMessageTemplate
        {
            get { return Resources.CrossServiceContractModelTIandPMTValidator; }
        }

        private string GetItName(object element)
        {
            if (element == null)
                return string.Empty;

            ModelElement mel = element as ModelElement;
            if (mel != null)
            {
                if (!string.IsNullOrWhiteSpace(this.MessageTemplate))
                {
                    ServiceContractModel scModel = DomainModelHelper.GetElement<ServiceContractModel>(mel.Store);
                    if (scModel != null && scModel.ImplementationTechnology != null)
                    {
                        return scModel.ImplementationTechnology.Name;
                    }
                    // try with Host
                    HostApplication hApp = DomainModelHelper.GetElement<HostApplication>(mel.Store);
                    if (hApp != null && hApp.ImplementationTechnology != null)
                    {
                        return hApp.ImplementationTechnology.Name;
                    }
                }
            }
            return string.Empty;
        }
    }
}
