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

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
{
    public sealed class MissingOperationContractAttribute : ContractAttributesRule
    {
        public MissingOperationContractAttribute()
            : base("MissingOperationContractAttribute")
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
            if (SemanticRulesUtilities.GetAttribute(type, serviceContractAttribute) != null)
            {
                // traverse each method and check if contains the OperationContractAttribute
                foreach (Member member in type.Members)
                {
                    if (member.NodeType == NodeType.Method &&
                        SemanticRulesUtilities.GetAttribute(member, operationContractAttribute) == null)
                    {
                        Resolution resolution = base.GetResolution(member.Name.Name);
                        Problem problem = new Problem(resolution, type.SourceContext);
                        base.Problems.Add(problem);
                    }
                }
            }
            return base.Problems;
        }
    }
}
