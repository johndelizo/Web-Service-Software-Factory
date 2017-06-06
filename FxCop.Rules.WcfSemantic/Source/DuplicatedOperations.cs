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
    /// Class that implements the DuplicatedOperations rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if more then one operation has the same signature name.
    /// </remarks>
    public sealed class DuplicatedOperations : ContractAttributesRule
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DuplicatedOperations"/> class.
        /// </summary>
        public DuplicatedOperations()
            : base("DuplicatedOperations")
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

			if (SemanticRulesUtilities.HasAttribute<ServiceContractAttribute>(attribute))
			{
				List<string> duplicated = new List<string>();
				foreach (Member member in type.Members)
				{
					if (SemanticRulesUtilities.GetAttribute(member, OperationContractAttribute) != null)
					{
						if (duplicated.Contains(member.Name.Name))
						{
							Resolution resolution = base.GetResolution(member.FullName);
							Problem problem = new Problem(resolution, type.SourceContext);
							base.Problems.Add(problem);
						}
						else
						{
							duplicated.Add(member.Name.Name);
						}
					}
				}
			}
            return base.Problems;
        }
    }
}
