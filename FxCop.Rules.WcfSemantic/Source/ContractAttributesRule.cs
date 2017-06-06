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
using Microsoft.FxCop.Sdk;
using System.Reflection;
using System.ServiceModel;

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
{
    /// <summary>
    /// Base class for rules that use all or any of the follwoing attributes:
    /// ServiceContractAttribute, OperationContractAttribute, OperationBehaviorAttribute, FaultContractAttribute
    /// </summary>
    public abstract class ContractAttributesRule : SemanticIntrospectionRule
    {
		protected Type serviceContractAttribute;
        protected Type operationContractAttribute;
        protected Type operationBehaviorAttribute;
        protected Type faultContractAttribute;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ContractAttributesRule"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		protected ContractAttributesRule(string name)
			: base(name)
		{
			serviceContractAttribute = typeof(ServiceContractAttribute);
			operationContractAttribute = typeof(OperationContractAttribute);
			OperationBehaviorAttribute = typeof(OperationBehaviorAttribute);
			faultContractAttribute = typeof(FaultContractAttribute);
		}

		protected Type ServiceContractAttribute
		{
			get { return serviceContractAttribute; }
			set { serviceContractAttribute = value; }
		}

		protected Type OperationContractAttribute
		{
			get { return operationContractAttribute; }
			set { operationContractAttribute = value; }
		}

		protected Type OperationBehaviorAttribute
		{
			get { return operationBehaviorAttribute; }
			set { operationBehaviorAttribute = value; }
		}

		protected Type FaultContractAttribute
		{
			get { return faultContractAttribute; }
			set { faultContractAttribute = value; }
		}
     }
}
