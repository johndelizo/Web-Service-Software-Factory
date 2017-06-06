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
    /// Class that implements the NonEmptyServiceContractName rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if the ServiceContract attribute has the Name property defined.
    /// </remarks>
    public sealed class NonEmptyServiceContractName : ContractAttributesRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NonEmptyServiceContractName"/> class.
        /// </summary>
        public NonEmptyServiceContractName()
            : base("NonEmptyServiceContractName")
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
				string.IsNullOrEmpty(SemanticRulesUtilities.GetAttributeValue<string>(attribute, "Name")))
			{
				Resolution resolution = base.GetResolution();
				Problem problem = new Problem(resolution, attribute.SourceContext);
				base.Problems.Add(problem);
				return base.Problems;
			}
            return base.Problems;
        }
    }
}
