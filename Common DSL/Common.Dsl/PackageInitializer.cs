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
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.Modeling.Common.Logging;
using System.Diagnostics;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions;
using Microsoft.Practices.Modeling.Common;
using System.ComponentModel.Design;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.Practices.Modeling.ExtensionProvider.Services;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Reflection;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
    public static class PackageInitializer
    {
        private static bool initialized;
        private static readonly object syncLock = new object();
        private static ProjectMappingManagerMonitor monitor;
 
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            lock (syncLock)
            {
                try
                {
                    if (!initialized)
                    {
                        LogWriterFactory.AddVSListeners();
                        if (serviceProvider is Package)
                        {
                            RuntimeHelper.ServiceProvider = serviceProvider;
                            monitor = new ProjectMappingManagerMonitor(serviceProvider, ProjectMappingManager.Instance);
                            LoadAssemblies();
                            InitializeServices(serviceProvider);
                            initialized = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Write(e);
                }
            }
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        public static void Shutdown()
        {
            lock (syncLock)
            {
                try
                {
                    if (initialized && 
                        monitor != null)
                    {
                        monitor.Dispose();
                        monitor = null;
                    }
                    GlobalCache.Reset();
                    initialized = false;
                }
                catch (Exception e)
                {
                    Logger.Write(e);
                }
            }
        }

        #region Private implementation

        private static void LoadAssemblies()
        {
            //	Initialize the AssemblyLoader. 
            //  This covers the scenario where the user opens an existing solution.
            //	In this scenario the model project packages gets loaded before the guidance package. The
            //	AssemblyLoader must be initialized before anything attempts to resolve the assemblies it loads.
            AssemblyLoader.LoadAll("Lib");
        }

        private static void InitializeServices(IServiceProvider serviceProvider)
        {
            IServiceContainer container = (IServiceContainer)serviceProvider;
            ServiceCreatorCallback callback = new ServiceCreatorCallback(OnCreateService);
            if (GetService<ICodeGenerationService>(serviceProvider) == null)
            {
                container.AddService(typeof(ICodeGenerationService), callback, true);
            }
            if (GetService<IExtensionProviderService>(serviceProvider) == null)
            {
                container.AddService(typeof(IExtensionProviderService), callback, true);
            }
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns></returns>
        private static TInterface GetService<TInterface>(IServiceProvider provider)
        {
            return (TInterface)provider.GetService(typeof(TInterface));
        }

        private static object OnCreateService(IServiceContainer container, Type serviceType)
        {
            if (serviceType == typeof(ICodeGenerationService))
            {
                return new CodeGenerationService((IServiceProvider)container);
            }
            else if (serviceType == typeof(IExtensionProviderService))
            {
                return new ExtensionProviderService();
            }
            return Activator.CreateInstance(serviceType);
        }
 
        #endregion#
    }
}
