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
using System.ServiceModel.Configuration;
using System.ServiceModel.Security;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Class that implements the UnTrustedClientCertificateValidation rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if the attribute name 'certificateValidationMode' has a value other then 'ChainTrust'.
    /// This attribute is locate in the serviceCredentials/clientCertificate section 
    /// </remarks>
    public sealed class UnTrustedClientCertificateValidation : ServiceModelConfigurationRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:UnTrustedClientCertificateValidation"/> class.
        /// </summary>
        public UnTrustedClientCertificateValidation()
            : base("UnTrustedClientCertificateValidation")
        {
        }

        /// <summary>
        /// Checks the specified configuration manager.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <returns></returns>
        public override ProblemCollection Check(ServiceModelConfigurationManager configurationManager)
        {
            foreach (ServiceBehaviorElement behaviorElement in configurationManager.ServiceModelSection.Behaviors.ServiceBehaviors)
            {
                ServiceCredentialsElement serviceCredentials =
                    ServiceModelConfigurationManager.GetBehaviorExtensionElement<ServiceCredentialsElement>(behaviorElement);

                X509CertificateValidationMode validationMode = serviceCredentials.ClientCertificate.Authentication.CertificateValidationMode;

                if (validationMode != X509CertificateValidationMode.ChainTrust)
                {
                    Resolution resolution = base.GetResolution(validationMode.ToString(), 
                        X509CertificateValidationMode.ChainTrust.ToString());
                    Problem problem = new Problem(resolution);
                    problem.SourceFile = base.SourceFile;
                    base.Problems.Add(problem);
                }
            }
            return base.Problems;
        }
    }
}
