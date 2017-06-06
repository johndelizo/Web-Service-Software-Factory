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
using System.Globalization;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.VisualStudio.Modeling;
using System.Diagnostics;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.ServiceFactory.HostDesigner;
using Microsoft.VisualStudio.Modeling.Integration;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Validate that a Service Description and the Service implementation both have the same techonology.
    /// </summary>
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class ServiceAndServiceImplementationTechnologyCrossModelValidator : CrossModelReferenceValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrossDataContractModelTIandPMTValidator"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public ServiceAndServiceImplementationTechnologyCrossModelValidator(NameValueCollection attributes)
            : base(attributes)
        {
            if (attributes == null)
            {
                return;
            }
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
                    ServiceContractModel scm = DomainModelHelper.GetElement<ServiceContractModel>(referencedElement.Store);
                    if (scm != null && scm.ImplementationTechnology != null)
                    {
                        if (serviceReference.HostApplication.ImplementationTechnology != null &&
                           !serviceReference.HostApplication.ImplementationTechnology.Name.Equals(
                           scm.ImplementationTechnology.Name))
                        {
                            this.LogValidationResult(
                                validationResults,
                                string.Format(CultureInfo.CurrentCulture, this.MessageTemplate, serviceReference.Name),
                                currentTarget,
                                key);
                        }
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
            get { return Resources.ServiceAndServiceImplementationTechnologyCrossModelValidator; }
        }
    }
}
