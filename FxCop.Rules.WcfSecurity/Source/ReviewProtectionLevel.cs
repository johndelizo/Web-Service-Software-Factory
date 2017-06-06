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
using System.Net.Security;
using System.ServiceModel;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Class that implements the ReviewProtectionLevel rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if the attribute ServiceContractAttribute or OperationContractAttribute
    /// have the property 'ProtectionLevel' with a value of None.
    /// </remarks>
    public sealed class ReviewProtectionLevel : SecurityIntrospectionRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReviewProtectionLevel"/> class.
        /// </summary>
        public ReviewProtectionLevel() : base("ReviewProtectionLevel")
        {
        }        

        public override void VisitAttributeNode(AttributeNode attribute)
        {
            if (Utilities.HasAttribute<ServiceContractAttribute>(attribute) ||
                Utilities.HasAttribute<OperationContractAttribute>(attribute))
            {
                ProtectionLevel protectionLevel;
                if (Utilities.TryGetAttributeValue<ProtectionLevel>(attribute, "ProtectionLevel", out protectionLevel))
                {
                    if (protectionLevel == ProtectionLevel.None)
                    {
                        Resolution resolution = base.GetResolution(attribute.Type.Name.Name);
                        Problem problem = new Problem(resolution, attribute.SourceContext);
                        base.Problems.Add(problem);
                    }
                }
            }
        }
    }
}
