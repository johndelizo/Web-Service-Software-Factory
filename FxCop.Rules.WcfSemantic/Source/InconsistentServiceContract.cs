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
    /// Class that implements the InconsistentServiceContract rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if the SessionMode parameter is missing from the ServiceContractAttribute
    /// or if the SessionMode value is consistent with the IsInitiating/IsTerminating arguments on each method.
    /// </remarks>
    public sealed class InconsistentServiceContract : ContractAttributesRule
    {
        private bool hasSessionMode;
        private SessionMode sessionMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:InconsistentServiceContract"/> class.
        /// </summary>
        public InconsistentServiceContract()
            : base("InconsistentServiceContract")
        {
        }

        /// <summary>
        /// Checks the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override ProblemCollection Check(TypeNode type)
        {
            // reset the class vars for each new type
            hasSessionMode = false;
            sessionMode = SessionMode.Allowed; // Set to default value

            AttributeNode attribute = SemanticRulesUtilities.GetAttribute(type, ServiceContractAttribute) ??
                                      SemanticRulesUtilities.GetAttribute(type, OperationContractAttribute);
            
            return CheckForProblems(attribute, null);
        }

        /// <summary>
        /// Checks the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns></returns>
        public override ProblemCollection Check(Member member)
        {
            AttributeNode attribute = SemanticRulesUtilities.GetAttribute(member, ServiceContractAttribute) ??
                                      SemanticRulesUtilities.GetAttribute(member, OperationContractAttribute);
            return CheckForProblems(attribute, member);
        }

        private ProblemCollection CheckForProblems(AttributeNode attribute, Member member)
        {
			if (SemanticRulesUtilities.HasAttribute<ServiceContractAttribute>(attribute))
			{
				// store state for check member analysis overload.
				hasSessionMode = SemanticRulesUtilities.HasAttribute<ServiceContractAttribute>(attribute, "SessionMode");
				if (hasSessionMode)
				{
					sessionMode = SemanticRulesUtilities.GetAttributeValue<SessionMode>(attribute, "SessionMode");
				}
				return base.Problems;
			}

			if (SemanticRulesUtilities.HasAttribute<OperationContractAttribute>(attribute) &&
				IsRuleViolated(attribute))
			{
				Resolution resolution = base.GetResolution(CustomizeDescription(attribute), member.Name.Name);
				Problem problem = new Problem(resolution, attribute.SourceContext);
				base.Problems.Add(problem);
				return base.Problems;
			}
            return base.Problems;
        }

        private bool IsRuleViolated(AttributeNode attribute)
        {
            bool hasInitTerm = HasInitiatingOrTerminatingValuesDeclared(attribute);

            return (!hasSessionMode && hasInitTerm) ||
                   (sessionMode == SessionMode.NotAllowed && hasInitTerm) ||
                   (sessionMode == SessionMode.Required && !hasInitTerm);
        }

        private string CustomizeDescription(AttributeNode attribute)
        {
            bool hasInitTerm = HasInitiatingOrTerminatingValuesDeclared(attribute);
            if (sessionMode == SessionMode.NotAllowed && hasInitTerm)
            {
                return Properties.Resources.InconsistentServiceContractRemoveMessage;
            }
            return Properties.Resources.InconsistentServiceContractUpdateMessage;
        }

        private bool HasInitiatingOrTerminatingValuesDeclared(AttributeNode attribute)
        {
			return SemanticRulesUtilities.HasAttribute<OperationContractAttribute>(attribute, "IsInitiating") ||
				   SemanticRulesUtilities.HasAttribute<OperationContractAttribute>(attribute, "IsTerminating");
        }
    }
}
