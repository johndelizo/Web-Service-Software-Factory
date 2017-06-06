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
using System.Configuration;
using System.Reflection;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Base class for handling configuration files with service model sections.
    /// </summary>
    public abstract class ServiceModelConfigurationRule : ConfigurationIntrospectionRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceModelConfigurationRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected ServiceModelConfigurationRule(string name) : base(name) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceModelConfigurationRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAssembly">The resource assembly.</param>
        protected ServiceModelConfigurationRule(string name, string resourceName, Assembly resourceAssembly)
            : base(name, resourceName, resourceAssembly)
        {
        }

        /// <summary>
        /// Checks the specified configuration manager.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <returns></returns>
        public abstract ProblemCollection Check(ServiceModelConfigurationManager configurationManager);

        /// <summary>
        /// Checks the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public override ProblemCollection Check(Configuration configuration)
        {
            if (configuration == null)
            {
                return base.Problems;
            }

            // update SourceFile in case we are loding the base class from a config file (not from a resource).
            if (base.SourceFile == null)
            {
                base.SourceFile = configuration.FilePath;
            }

            return this.Check(new ServiceModelConfigurationManager(configuration));
        }
    }
}
