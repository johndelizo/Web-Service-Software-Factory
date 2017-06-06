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
using System.ServiceModel;
using Microsoft.FxCop.Sdk;
using System.ServiceModel.Configuration;
using System.Diagnostics;
using System.ServiceModel.Description;

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
{
    /// <summary>
    /// Class that implements the ContractBindingNotSupportedSession rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if the a service contract requires Session, 
    /// but the configured binding for that contract doesn't support it or 
    /// isn't configured properly to support it
    /// Note: This rule will not support 'offending line code navigation'. 
    /// It's a 'configuration aware' rule.
    /// </remarks>
    public sealed class ContractBindingNotSupportedSession : ContractBindingRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ContractBindingNotSupportedSession"/> class.
        /// </summary>
        public ContractBindingNotSupportedSession()
            : base("ContractBindingNotSupportedSession")
        {
        }

        /// <summary>
        /// Evaluates the rule.
        /// </summary>
        /// <param name="type">The type.</param>
		/// <param name="attribute">The attribute.</param>
		/// <param name="binding">The binding.</param>
        public override void EvaluateRule(TypeNode type, 
            AttributeNode attribute, string binding)
        {
			// for further info on Sessions and bindings, 
			// read: http://msdn2.microsoft.com/en-us/library/ms730879.aspx
			if ((SemanticRulesUtilities.GetAttributeValue<SessionMode>(attribute, "SessionMode") == SessionMode.Required &&
					IsSessionlessBinding(binding)) ||
				(SemanticRulesUtilities.GetAttributeValue<SessionMode>(attribute, "SessionMode") == SessionMode.NotAllowed &&
					IsSessionfullBinding(binding)))
			{
				Resolution resolution = base.GetResolution(type.FullName, binding);
				Problem problem = new Problem(resolution);
				base.Problems.Add(problem);
			}
        }
 
        private bool IsSessionlessBinding(string binding)
        {
            // check for any other binding or configuratuion that does not support sessions
            return binding.Equals("basicHttpBinding", StringComparison.OrdinalIgnoreCase) ||
                   binding.Equals("NetMsmqBinding", StringComparison.OrdinalIgnoreCase) ||
                   binding.Equals("NetPeerTcpBinding", StringComparison.OrdinalIgnoreCase) ||
                   binding.Equals("MsmqIntegrationBinding", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsSessionfullBinding(string binding)
        {
            return binding.Equals("wsDualHttpBinding", StringComparison.OrdinalIgnoreCase) ||
                   binding.Equals("netTcpBinding", StringComparison.OrdinalIgnoreCase);
        }
    }
}
