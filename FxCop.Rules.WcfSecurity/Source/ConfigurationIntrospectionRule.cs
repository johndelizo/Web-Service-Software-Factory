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
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Microsoft.FxCop.Sdk;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Base configuration introspection rule class.
    /// </summary>
    public abstract class ConfigurationIntrospectionRule : SecurityIntrospectionRule
    {
        private string sourceFile;
        private const string ConfigFileSearchPattern = "*.config";
        private bool isConfigInTempPath = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConfigurationIntrospectionRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected ConfigurationIntrospectionRule(string name) : base(name) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConfigurationIntrospectionRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAssembly">The resource assembly.</param>
        protected ConfigurationIntrospectionRule(string name, string resourceName, Assembly resourceAssembly)
            : base(name, resourceName, resourceAssembly)
        {
        }

        /// <summary>
        /// Gets the source file.
        /// </summary>
        /// <value>The source file.</value>
        public string SourceFile
        {
            get { return sourceFile; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                // update the sourceFile in case it's not in the system temp folder.
                if (!value.StartsWith(Path.GetTempPath(), StringComparison.OrdinalIgnoreCase))
                {
                    sourceFile = value;
                }
                else
                {
                    isConfigInTempPath = true;
                }
            }
        }

        /// <summary>
        /// Checks the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public abstract Microsoft.FxCop.Sdk.ProblemCollection Check(Configuration configuration);

        /// <summary/>
        /// // this override will get a config file (if any) from the same location as the current inspected asm.
        public override ProblemCollection Check(ModuleNode module)
        {
            Configuration configuration = ConfigurationProbing(module);
            if (configuration != null)
            {
                this.SourceFile = configuration.FilePath;
                return Check(configuration);
            }
            return base.Check(module);
        }

        /// <summary>
        /// Determines whether the config file is in a temp path.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if config is in temp path; otherwise, <c>false</c>.
        /// </returns>
        public bool IsConfigInTempPath()
        {
            return isConfigInTempPath;
        }

        // FxCop: False positive
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private Configuration ConfigurationProbing(ModuleNode module)
        {
            if (module == null ||
                string.IsNullOrEmpty(module.Directory))
            {
                return null;
            }

            try
            {
                string[] configs = Directory.GetFiles(module.Directory, ConfigFileSearchPattern, SearchOption.TopDirectoryOnly);
                if (configs == null ||
                    configs.Length == 0)
                {
                    // try the parent folder (this scenario is for web apps, asms in Bin folder and config in parent folder)
                    configs = Directory.GetFiles(Path.GetDirectoryName(module.Directory), ConfigFileSearchPattern, SearchOption.TopDirectoryOnly);
                }
                string configPath = SelectConfigFile(configs, module.Location);
                if (string.IsNullOrEmpty(configPath))
                {
                    return null;
                }
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = configPath;
                return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            }
            catch (Exception e)
            {
                TraceManager.Exception(e);
                return null;
            }
        }

        private static string SelectConfigFile(string[] configs, string moduleLocation)
        {
            string selected = null;

            if (configs != null &&
                configs.Length != 0 &&
                !string.IsNullOrEmpty(moduleLocation))
            {
                List<string> lookup = new List<string>(configs);
                // probe for App config file (config name = exe name + [.config] extension) 
                selected = moduleLocation + ".config";
                if (!lookup.Contains(selected))
                {
                    // probe for web config file
                    selected = lookup.Find(delegate(string configPath)
                    {
                        return Path.GetFileName(configPath).Equals("web.config", StringComparison.OrdinalIgnoreCase);
                    });
                }
            }
            return selected;
        }
    }
}
