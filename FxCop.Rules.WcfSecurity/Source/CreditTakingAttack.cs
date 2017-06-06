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
using System.ServiceModel.Configuration;
using System.ServiceModel.Security;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Class that implements the CreditTakingAttack rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if a customBinding has the 'MessageProtectionOrder' attribute in the security element with
    /// a value of 'EncryptBeforeSign'.
    /// </remarks>
    public sealed class CreditTakingAttack : ServiceModelConfigurationRule
    {
        private const string CustomBindingAttributeValue = "customBinding";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CreditTakingAttack"/> class.
        /// </summary>
        public CreditTakingAttack()
            : base("CreditTakingAttack")
        {
        }

        /// <summary>
        /// Checks the specified configuration manager.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <returns></returns>
        public override ProblemCollection Check(ServiceModelConfigurationManager configurationManager)
        {
            // Check for service endpoints
            foreach (ServiceElement serviceElement in configurationManager.GetServices())
            {
                foreach(ServiceEndpointElement endpointElement in serviceElement.Endpoints)
                {
                    if(endpointElement.Binding.Equals(CustomBindingAttributeValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckRuleForCustomBinding(configurationManager.GetCustomBinding(endpointElement.BindingConfiguration));
                    }
                }
            }
            // Check for client endponts
            ClientSection client = configurationManager.ServiceModelSection.Client;
            if(client != null)
            {
                foreach(ChannelEndpointElement clientEndpoint in client.Endpoints)
                {
                    if (clientEndpoint.Binding.Equals(CustomBindingAttributeValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckRuleForCustomBinding(configurationManager.GetCustomBinding(clientEndpoint.BindingConfiguration));
                    }
                }
            }
            return base.Problems;
        }

        private void CheckRuleForCustomBinding(CustomBindingElement customBinding)
        {
            SecurityElement securityElement = (SecurityElement)customBinding[typeof(SecurityElement)];
            if (securityElement != null)
            {
                if (securityElement.MessageProtectionOrder == MessageProtectionOrder.EncryptBeforeSign ||
                    (securityElement.AuthenticationMode == AuthenticationMode.SecureConversation && 
                     securityElement.SecureConversationBootstrap.MessageProtectionOrder == MessageProtectionOrder.EncryptBeforeSign))
                {
                    Resolution resolution = base.GetResolution(
                        customBinding.Name, 
                        MessageProtectionOrder.SignBeforeEncrypt.ToString(), 
                        MessageProtectionOrder.SignBeforeEncryptAndEncryptSignature.ToString());
                    Problem problem = new Problem(resolution);
                    problem.SourceFile = base.SourceFile;
                    base.Problems.Add(problem);
                }
            }
        }
    }
}
