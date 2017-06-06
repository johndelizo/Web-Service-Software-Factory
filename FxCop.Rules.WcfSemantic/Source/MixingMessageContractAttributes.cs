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
    /// Class that implements the MixingMessageContractAttributes rule.
    /// </summary>
    /// <remarks>
    /// This rule will check that all 'one way' opertions should not
    /// have output params.
    /// </remarks>
    public sealed class MixingMessageContractAttributes : ContractAttributesRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MixingMessageContractAttributes"/> class.
        /// </summary>
        public MixingMessageContractAttributes()
            : base("MixingMessageContractAttributes")
        {
        }

        /// <summary>
        /// Checks the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override ProblemCollection Check(Member member)
        {
            AttributeNode attribute = SemanticRulesUtilities.GetAttribute(member, OperationContractAttribute);

			if (SemanticRulesUtilities.HasAttribute<OperationContractAttribute>(attribute) &&
				IsMixingMessageContractParams((Method)member))
			{
				Resolution resolution = base.GetResolution(member.Name.Name);
				Problem problem = new Problem(resolution, member.SourceContext);
				base.Problems.Add(problem);
			}
            return base.Problems;
        }

		private bool IsMixingMessageContractParams(Method method)
        {
            bool hasMessageContract = false;
            bool hasOtherType = false;

			foreach (Parameter parameter in method.Parameters)
            {
                if (HasMessageContractAttribute(parameter.Type.Attributes))
                {
                    hasMessageContract = true;
                }
                else
                {
                    hasOtherType = true;
                }
                if (hasMessageContract && hasOtherType)
                {
                    return true;
                }
            }

            // check for return type
			if (HasMessageContractAttribute(method.ReturnType.Attributes))
            {
                hasMessageContract = true;
            }
            else
            {
                hasOtherType = true; 
            }
            return hasOtherType && hasMessageContract;
        }

		private bool HasMessageContractAttribute(AttributeNodeCollection attributes)
        {
			return SemanticRulesUtilities.GetAttribute(attributes, typeof(MessageContractAttribute)) != null;
        }
    }
}
