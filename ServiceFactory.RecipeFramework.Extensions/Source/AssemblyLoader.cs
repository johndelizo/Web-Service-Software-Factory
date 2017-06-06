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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.VisualStudio.Helper;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions
{
    /// <summary>
    /// Load assemblies in the guidance package \Lib folder and use an AppDomain AssemblyResolve
    /// event handler to return assemblies loaded from this folder.
    /// </summary>
    /// <remarks>
    /// This AssemblyLoader must be initialized by calling LoadAll before the guidance package 
    /// attempts to load any of the assemblies managed by the assembly loader.
    /// </remarks>
    public static class AssemblyLoader
    {
        private static bool loaded;
        private static IList<Assembly> loadedAssemblies;
        private static object sync = new object();

        /// <summary>
        /// Load a list of assemblies and sink AssemblyResolve event.
        /// </summary>
        public static void LoadAll()
        {
            LoadAll(RuntimeHelper.GetExecutionPath());
        }

        /// <summary>
        /// Load a list of assemblies and sink AssemblyResolve event.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"),
         SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        public static void LoadAll(string basePath)
        {
            lock (sync)
            {
                if (loaded)
                {
                    return;
                }

                loadedAssemblies = new List<Assembly>();

                if (string.IsNullOrEmpty(basePath))
                {
                    Logger.Write(Properties.Resources.AssemblyFolderNotFound, TraceEventType.Critical);
                }
                else
                {
                    if (!Path.IsPathRooted(basePath))
                    {
                        basePath = RuntimeHelper.GetExecutionPath(basePath);
                    }
                    string[] assemblyFilenames = Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories);
                    foreach (string filename in assemblyFilenames)
                    {
                        try
                        {
                            loadedAssemblies.Add(Assembly.LoadFrom(filename));
                        }
                        catch (BadImageFormatException) { }
                        catch (Exception e)
                        {
                            Logger.Write(e, TraceEventType.Warning);
                        }
                    }
                    if (assemblyFilenames.Length == 0)
                    {
                        Logger.Write(string.Format(CultureInfo.CurrentCulture, Properties.Resources.AssembliesNotLoaded, basePath), TraceEventType.Critical);
                    }
                }
                loaded = true;
            }
        }

        public static IList<Type> GetTypesByInterface(Type interfaceType)
        {
            List<Type> matches = new List<Type>();
            foreach (Assembly assembly in LoadedAssemblies)
            {
                try
                {
                    List<Type> assemblyTypes = new List<Type>(assembly.GetTypes());
                    matches.AddRange(ReflectionHelper.GetTypesByInterface(assemblyTypes, interfaceType));
                }
                catch (ReflectionTypeLoadException e) // Bad .NET DLL with missing dependencies
                {
                    Logger.Write(e, TraceEventType.Warning);
                    continue;
                }
            }

            return matches;
        }

        public static IList<Assembly> LoadedAssemblies
        {
            get
            {
                if (!loaded)
                {
                    LoadAll();
                }
                return loadedAssemblies;
            }
        }

        /// <summary>
        /// Marks the cache as not-loaded, primarily for unit testing.    
        /// </summary>
        public static void ResetCache()
        {
            lock (sync)
            {
                loaded = false;
                loadedAssemblies = new List<Assembly>();
            }
        }
    }
}
