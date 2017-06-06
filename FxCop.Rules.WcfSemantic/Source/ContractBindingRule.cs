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
using System.ServiceModel.MsmqIntegration;

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
{
    /// <summary>
    /// Base class for handling service contract types and binding settings in configuration files.
    /// </summary>
    public abstract class ContractBindingRule : ServiceModelConfigurationRule
    {
		private ServiceModelConfigurationManager configurationManager;

		protected ServiceModelConfigurationManager ConfigurationManager
		{
			get { return configurationManager; }
			set { configurationManager = value; }
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ContractBindingRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected ContractBindingRule(string name) : base(name)
        {
            configurationManager = null;
        }

        /// <summary>
        /// Checks the specified configuration manager.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <returns></returns>
        public override ProblemCollection Check(ServiceModelConfigurationManager configurationManager)
        {
            //store the config manager for later use
            this.configurationManager = configurationManager;
            return base.Problems;
        }

        public override ProblemCollection Check(ModuleNode module)
        {
			if (configurationManager == null)
			{
				base.Check(module);
			}

			SemanticRulesUtilities.EvaluateContracts(
				configurationManager,
				module,
				delegate(TypeNode node, ServiceEndpointElement endpoint)
				{
					this.EvaluateTypeNode(node, endpoint.Binding);
				});

			return base.Problems;
        }

        /// <summary>
        /// Evaluates the rule.
        /// </summary>
        /// <param name="type">The type.</param>
		/// <param name="attribute">The attribute.</param>
        /// <param name="binding">The binding.</param>
		public abstract void EvaluateRule(TypeNode type,
			AttributeNode attribute, string binding);
		
        protected bool IsWSDualHttpBinding(string binding)
        {
			if (string.IsNullOrEmpty(binding))
			{
				return false;
			}
			return binding.Equals(typeof(WSDualHttpBinding).Name, StringComparison.OrdinalIgnoreCase);
        }

        protected bool IsMsmqIntegrationBinding(string binding)
        {
			if (string.IsNullOrEmpty(binding))
			{
				return false;
			}
            return binding.Equals(typeof(MsmqIntegrationBinding).Name, StringComparison.OrdinalIgnoreCase);
        }

        protected bool IsNetMsmqBinding(string binding)
        {
			if (string.IsNullOrEmpty(binding))
			{
				return false;
			}
			return binding.Equals(typeof(NetMsmqBinding).Name, StringComparison.OrdinalIgnoreCase);
        }

        protected virtual void EvaluateTypeNode(TypeNode type, string binding)
        {
			if (type == null)
				throw new ArgumentNullException("type");
			if (binding == null)
				throw new ArgumentNullException("binding");

			AttributeNode attribute = SemanticRulesUtilities.GetAttribute(type, typeof(ServiceContractAttribute));

			if (configurationManager != null)
			{
				if (SemanticRulesUtilities.HasAttribute<ServiceContractAttribute>(attribute))
				{
					EvaluateRule(type, attribute, binding);
				}
			}
        }
    }
}
