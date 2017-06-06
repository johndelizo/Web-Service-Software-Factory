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
    /// Class that implements the MultipleUnmatchedMessageHandlers rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if the more than one operations has the unmatched 
    /// message handler parameter specified. (See Action="*" value).
    /// </remarks>
    public sealed class MultipleUnmatchedMessageHandlers : ContractAttributesRule
    {
        private bool hasUnmatchedMessageHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MultipleUnmatchedMessageHandlers"/> class.
        /// </summary>
        public MultipleUnmatchedMessageHandlers()
            : base("MultipleUnmatchedMessageHandlers")
        {
        }

        /// <summary>
        /// Checks the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override ProblemCollection Check(TypeNode type)
        {
            AttributeNode attribute = 
                SemanticRulesUtilities.GetAttribute(type, ServiceContractAttribute) ??
                SemanticRulesUtilities.GetAttribute(type, OperationContractAttribute);

            return CheckForProblems(attribute);
        }

        /// <summary>
        /// Checks the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public override ProblemCollection Check(Member member)
        {
            AttributeNode attribute =
                SemanticRulesUtilities.GetAttribute(member, ServiceContractAttribute) ??
                SemanticRulesUtilities.GetAttribute(member, OperationContractAttribute);
            return CheckForProblems(attribute);
        }

        private ProblemCollection CheckForProblems(AttributeNode attribute)
        {
			if (SemanticRulesUtilities.HasAttribute<ServiceContractAttribute>(attribute))
			{
				// reset the hasUnmatchedMessageHandler flag for each new type
				hasUnmatchedMessageHandler = false;
				return base.Problems;
			}

			string action = SemanticRulesUtilities.GetAttributeValue<String>(attribute, "Action");
			if (SemanticRulesUtilities.HasAttribute<OperationContractAttribute>(attribute) &&
				!string.IsNullOrEmpty(action) &&
				 action.Equals("*", StringComparison.OrdinalIgnoreCase))
			{
				// check if we already inspected another operation with unmatched message handler
				if (hasUnmatchedMessageHandler)
				{
					Resolution resolution = base.GetResolution();
					Problem problem = new Problem(resolution, attribute.SourceContext);
					base.Problems.Add(problem);
					return base.Problems;
				}
				hasUnmatchedMessageHandler = true;
			}
            return base.Problems;
        }
    }
}
