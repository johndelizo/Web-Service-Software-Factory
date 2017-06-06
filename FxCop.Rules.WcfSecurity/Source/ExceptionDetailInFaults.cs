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
using System.ServiceModel.Configuration;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Class that implements the ExceptionDetailInFaults rule.
    /// </summary>
    /// <remarks>
    /// This rule will check if the serviceDebug element in a behavior section will have the
    /// 'includeExceptionDetailInFaults' attribute with a value of 'false'.
    /// </remarks>
    public sealed class ExceptionDetailInFaults : ServiceModelConfigurationRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExceptionDetailInFaults"/> class.
        /// </summary>
        public ExceptionDetailInFaults()
            : base("ExceptionDetailInFaults")
        {
        }

        /// <summary>
        /// Checks the specified configuration manager.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <returns></returns>
        public override ProblemCollection Check(ServiceModelConfigurationManager configurationManager)
        {
            foreach (ServiceBehaviorElement behaviorElement in configurationManager.ServiceModelSection.Behaviors.ServiceBehaviors)
            {
                ServiceDebugElement serviceDebug =
                    ServiceModelConfigurationManager.GetBehaviorExtensionElement<ServiceDebugElement>(behaviorElement);

                if (serviceDebug.IncludeExceptionDetailInFaults)
                {
                    Resolution resolution = base.GetResolution(behaviorElement.Name);
                    Problem problem = new Problem(resolution);
                    problem.SourceFile = base.SourceFile;
                    base.Problems.Add(problem);
                }
            }
            return base.Problems;
        }
    }
}
