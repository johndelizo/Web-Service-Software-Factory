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

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
{
    /// <summary>
    /// Class that implements the InvalidCallbackContractType rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if the CallbackContract type is an interface or a MarshalByRef derived class.
    /// </remarks>
    public sealed class InvalidCallbackContractType : ContractAttributesRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:InvalidCallbackContractType"/> class.
        /// </summary>
        public InvalidCallbackContractType()
            : base("InvalidCallbackContractType")
        {
        }

        /// <summary>
        /// Checks the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override ProblemCollection Check(TypeNode type)
        {
            AttributeNode attribute = SemanticRulesUtilities.GetAttribute(type, ServiceContractAttribute);

			if (SemanticRulesUtilities.HasAttribute<ServiceContractAttribute>(attribute) &&
				!IsValidCallbackContract(SemanticRulesUtilities.GetAttributeValue<TypeNode>(attribute, "CallbackContract")))
			{
				Resolution resolution = base.GetResolution();
				Problem problem = new Problem(resolution, attribute.SourceContext);
				base.Problems.Add(problem);
				return base.Problems;
			}
            return base.Problems;
        }

		private bool IsValidCallbackContract(TypeNode node)
        {
			if (node == null)
			{
				// If we did not find a CallbackContract attribute, bypass check  
				return true;
			}
			Type callbackContract = Type.GetType(
				node.FullName + "," + node.DeclaringModule.ContainingAssembly.StrongName);

            return callbackContract.IsInterface ||
                   (callbackContract.IsClass && callbackContract.IsMarshalByRef);
        }
    }
}
