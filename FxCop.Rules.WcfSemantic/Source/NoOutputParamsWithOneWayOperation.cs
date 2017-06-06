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
using System.Reflection;

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
{
    /// <summary>
    /// Class that implements the NoOutputParamsWithOneWayOperation rule.
    /// </summary>
    /// <remarks>
    /// This rule will check that all 'one way' opertions should not
    /// have output params.
    /// </remarks>
    public sealed class NoOutputParamsWithOneWayOperation : ContractAttributesRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NoOutputParamsWithOneWayOperation"/> class.
        /// </summary>
        public NoOutputParamsWithOneWayOperation()
            : base("NoOutputParamsWithOneWayOperation")
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
				SemanticRulesUtilities.GetAttributeValue<Boolean>(attribute, "IsOneWay") &&
				HasOutParams(((Method)member)))
			{
				Resolution resolution = base.GetResolution();
				Problem problem = new Problem(resolution, member.SourceContext);
				base.Problems.Add(problem);
			}
            return base.Problems;
        }

        private bool HasOutParams(Method method)
        {
			if (method != null)
			{
				foreach (Parameter parameter in method.Parameters)
				{
					if (parameter.IsOut)
					{
						return true;
					}
				}
			}
            return false;
        }
    }
}
