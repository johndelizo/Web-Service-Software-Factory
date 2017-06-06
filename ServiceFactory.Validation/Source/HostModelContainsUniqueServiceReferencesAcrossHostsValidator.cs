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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.ServiceFactory.HostDesigner;
using System.Threading;
using System.Collections.Specialized;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class HostModelContainsUniqueServiceReferencesAcrossHostsValidator : ConfigurableObjectCollectionValidator<HostApplication>
    {
        public HostModelContainsUniqueServiceReferencesAcrossHostsValidator(NameValueCollection configuration)
            : base(configuration)
        {
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            IEnumerable<HostApplication> hostApplications = objectToValidate as IEnumerable<HostApplication>;
            if (hostApplications != null)
            {
                List<string> discriminators = new List<string>();
                foreach (HostApplication hostApplication in hostApplications)
                {
                    string implementationProject = hostApplication.ImplementationProject;

                    //missing implementation projects is covered by another Validator.
                    if (string.IsNullOrEmpty(implementationProject)) continue;
                    string caseInsensitiveImplementationProject = implementationProject.ToUpperInvariant();

                    foreach (string serviceReferenceName in IterateDistinctServiceReferenceNames(hostApplication))
                    {
                        string discriminator = string.Concat(caseInsensitiveImplementationProject, "/", serviceReferenceName);
                        if (discriminators.Contains(discriminator))
                        {
                            string validationMessage = String.Format(Thread.CurrentThread.CurrentUICulture, this.MessageTemplate, discriminator);
                            this.LogValidationResult(validationResults, validationMessage, objectToValidate, key);
                        }
                        else
                        {
                            discriminators.Add(discriminator);
                        }
                    }
                }
            }
        }

        private IEnumerable<string> IterateDistinctServiceReferenceNames(HostApplication hostApplication)
        {
            List<string> serviceReferenceNames = new List<string>();
            foreach (ServiceReference serviceReference in hostApplication.ServiceDescriptions)
            {
                //is covered by a different Validator
                if (string.IsNullOrEmpty(serviceReference.Name)) continue;

                string caseInsensitiveReferenceName = serviceReference.Name.ToUpperInvariant();

                //uniqueness within a single host is covered by another Validator.
                if (!serviceReferenceNames.Contains(caseInsensitiveReferenceName))
                {
                    serviceReferenceNames.Add(caseInsensitiveReferenceName);
                }
            }

            return serviceReferenceNames;
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.NonUniqueServiceReferenceNameValidatorMessage; }
        }
    }
}
