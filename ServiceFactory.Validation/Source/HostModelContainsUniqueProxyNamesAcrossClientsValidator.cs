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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.ServiceFactory.HostDesigner;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Threading;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class HostModelContainsUniqueProxyNamesAcrossClientsValidator : ConfigurableObjectCollectionValidator<ClientApplication>
    {
        public HostModelContainsUniqueProxyNamesAcrossClientsValidator(NameValueCollection configuration)
            : base(configuration)
        {
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            IEnumerable<ClientApplication> clientApplications = objectToValidate as IEnumerable<ClientApplication>;
            if (clientApplications != null)
            {
                List<string> discriminators = new List<string>();
                foreach (ClientApplication clientApplication in clientApplications)
                {
                    string implementationProject = clientApplication.ImplementationProject;

                    //missing implementation projects is covered by another Validator.
                    if (string.IsNullOrEmpty(implementationProject)) continue;
                    string caseInsensitiveImplementationProject = implementationProject.ToUpperInvariant();

                    foreach (string proxyName in IterateDistinctProxyNames(clientApplication))
                    {
                        string discriminator = string.Concat(caseInsensitiveImplementationProject, "/", proxyName);
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

        private IEnumerable<string> IterateDistinctProxyNames(ClientApplication clientApplication)
        {
            List<string> proxyNames = new List<string>();
            foreach (Proxy proxy in clientApplication.Proxies)
            {
                //is covered by a different Validator
                if (string.IsNullOrEmpty(proxy.Name)) continue;

                string caseInsensitiveProxyName = proxy.Name.ToUpperInvariant();

                //uniqueness within a single host is covered by another Validator.
                if (!proxyNames.Contains(caseInsensitiveProxyName))
                {
                    proxyNames.Add(caseInsensitiveProxyName);
                }
            }

            return proxyNames;
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.NonUniqueProxyNameValidatorMessage; }
        }
    }
}
