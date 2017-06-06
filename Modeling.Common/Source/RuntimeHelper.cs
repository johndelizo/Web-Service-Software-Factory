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
using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.ExtensionManager;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.Modeling.Common
{
	public static class RuntimeHelper
	{
        static string packagePath = null;
        static IServiceProvider provider = null;
        const string ServiceFactoryPackageExtensionId = "19bb3318-cdf8-44bc-b02b-5788104f8cf8";

        /// <summary>
        /// Returns the service provider instance.
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get { return provider ?? SetDefaultProvider(); }
            set { provider = value; }
        }

        /// <summary>
        /// Returns the file path for the current package.
        /// </summary>
        public static string PackagePath
        {
            get { return packagePath ?? SetDefaultPath(); }
        }

        /// <summary>
        /// Returns the default execution path of the installed main package.
        /// </summary>
        /// <returns></returns>
        public static string GetExecutionPath()
        {
            return GetExecutionPath(null);
        }

        public static string GetExecutionPath(string relativePath)
        {
            string fullPath = Path.Combine(PackagePath, relativePath ?? string.Empty);

            return File.Exists(fullPath) ||
                   Directory.Exists(fullPath) ? fullPath : null;
        }

        #region Private members

        private static IServiceProvider SetDefaultProvider()
        {
            EnvDTE.DTE dte = (EnvDTE.DTE)DteLocator.GetCurrentInstance();
            if (dte == null)
            {
                // This will be used for testing
                dte = Marshal.GetActiveObject("VisualStudio.DTE.11.0") as EnvDTE.DTE;
            }

            provider = new ServiceProvider(dte as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            return provider;
        }

        private static string SetDefaultPath()
        {
            IVsExtensionManager manager = ServiceProvider.GetService(typeof(SVsExtensionManager)) as IVsExtensionManager;
            if (manager == null)
            {
                // This will be used for testing
                packagePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return packagePath;
            }

            IInstalledExtension extension = manager.GetInstalledExtension(ServiceFactoryPackageExtensionId);
            Debug.Assert(extension != null, "We could not find " + ServiceFactoryPackageExtensionId + " or package not installed.");

            packagePath = extension.InstallPath;
            return packagePath;
        }

        #endregion
    }
}
