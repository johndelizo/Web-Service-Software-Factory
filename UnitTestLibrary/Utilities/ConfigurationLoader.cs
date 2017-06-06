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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.UnitTestLibrary.Utilities
{
    public static class ConfigurationLoader
    {
		public static System.Configuration.Configuration LoadConfiguration(string configurationFileName)
		{
			return LoadConfiguration(configurationFileName, null);
		}

        public static System.Configuration.Configuration LoadConfiguration(string configurationFileName, string optionalFolder)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
			map.ExeConfigFilename = GetConfigurationFilePath(configurationFileName, optionalFolder);
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

		public static string GetConfigurationFilePath(string configurationFileName)
		{
			return GetConfigurationFilePath(configurationFileName, null);
		}

		public static string GetConfigurationFilePath(string configurationFileName, string optionalFolder)
		{
			return GetConfigurationFilePath(configurationFileName, optionalFolder, false);
		}

		public static string GetConfigurationFilePath(string configurationFileName, string optionalFolder, bool forceCopy)
        {

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(assemblyPath,
				Path.Combine(optionalFolder ?? string.Empty, Path.GetFileName(configurationFileName)));

			if (!File.Exists(path) ||
				forceCopy)
            {
                string basePath = Path.Combine(assemblyPath, configurationFileName);
				if (File.Exists(basePath) &&
				   !string.IsNullOrEmpty(optionalFolder))
				{
					if (!Directory.Exists(Path.GetDirectoryName(path)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(path));
					}
					if(forceCopy && File.Exists(path))
					{
						File.Delete(path);
					}
					File.Copy(basePath, path);
				}
				else if(File.Exists(basePath))
				{
					path = basePath;
				}
			}

			Assert.IsTrue(File.Exists(path), string.Format("The file {0} does not exist", path));			
				
            return path;
        }
    }
}
