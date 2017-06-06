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
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Class that implements the NoProtection rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if a customBinding or a basicHttpBinding if the element 'security' has the attribute
    /// 'mode' with a value of 'None' in case of BasicHttpBinding only, with a value 'message' and the 
    /// attribute 'clientCredentialType' in the message element with a value of 'UserName'.
    /// </remarks>
    public sealed class NoProtection : ServiceModelConfigurationRule
    {
        private const string CustomBindingAttributeValue = "customBinding";
        private const string BasicHttpBindingAttributeValue = "basicHttpBinding";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NoProtection"/> class.
        /// </summary>
        public NoProtection()
            : base("NoProtection")
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
                    if (endpointElement.Binding.Equals(BasicHttpBindingAttributeValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckRuleForBasicBinding(
                            configurationManager.GetStandardBinding<BasicHttpBinding, BasicHttpBindingElement>(endpointElement.BindingConfiguration));
                    }
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
                    if (clientEndpoint.Binding.Equals(BasicHttpBindingAttributeValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckRuleForBasicBinding(
                            configurationManager.GetStandardBinding<BasicHttpBinding, BasicHttpBindingElement>(clientEndpoint.BindingConfiguration));
                    }
                    if (clientEndpoint.Binding.Equals(CustomBindingAttributeValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CheckRuleForCustomBinding(configurationManager.GetCustomBinding(clientEndpoint.BindingConfiguration));
                    }
                }
            }
            return base.Problems;
        }

        private void CheckRuleForBasicBinding(BasicHttpBindingElement basicHttpBinding)
        {
            if (basicHttpBinding != null &&
                (basicHttpBinding.Security.Mode == BasicHttpSecurityMode.None ||
                 (basicHttpBinding.Security.Mode == BasicHttpSecurityMode.Message &&
                  basicHttpBinding.Security.Message.ClientCredentialType == BasicHttpMessageCredentialType.UserName)
                 )
                )
            {
                AddProblem(basicHttpBinding.Name);
            }
        }

        private void CheckRuleForCustomBinding(CustomBindingElement customBinding)
        {
            if (customBinding != null)
            {
                SecurityElement securityElement = (SecurityElement)customBinding[typeof(SecurityElement)];
                if (securityElement == null)
                {
                    AddProblem(customBinding.Name);
                }
            }
        }

        private void AddProblem(string bindingName)
        {
            Resolution resolution = base.GetResolution(bindingName);
            Problem problem = new Problem(resolution);
            problem.SourceFile = base.SourceFile;
            base.Problems.Add(problem);
        }
    }
}
