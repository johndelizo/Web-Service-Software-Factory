//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.Modeling.Common
{
    /// <summary>
    /// Helper methods to deal with DTE to VSIP project model conversions.
    /// </summary>
    public static class VsHelper
    {
        /// <summary>
        /// Toes the hierarchy.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public static IVsHierarchy ToHierarchy(EnvDTE.Project project)
        {
            if (project == null) throw new ArgumentNullException("project");

            // DTE does not expose the project GUID that exists at in the msbuild project file.
            Microsoft.Build.Evaluation.Project msproject = new Microsoft.Build.Evaluation.Project();
            msproject.FullPath = project.FileName;
            
            string guid = msproject.GetPropertyValue("ProjectGuid");

            IServiceProvider serviceProvider = new ServiceProvider(project.DTE as
                Microsoft.VisualStudio.OLE.Interop.IServiceProvider);

            return VsShellUtilities.GetHierarchy(serviceProvider, new Guid(guid));
        }

        /// <summary>
        /// Toes the vs project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public static IVsProject3 ToVSProject(EnvDTE.Project project)
        {
            if (project == null) throw new ArgumentNullException("project");

            IVsProject3 vsProject = ToHierarchy(project) as IVsProject3;

            if (vsProject == null)
            {
                throw new ArgumentException(Properties.Resources.ProjectIsNotVSProject);
            }

            return vsProject;
        }

        /// <summary>
        /// Toes the DTE project.
        /// </summary>
        /// <param name="hierarchy">The hierarchy.</param>
        /// <returns></returns>
        public static EnvDTE.Project ToDteProject(IVsHierarchy hierarchy)
        {
            if (hierarchy == null) throw new ArgumentNullException("hierarchy");

            object prjObject = null;
            if (hierarchy.GetProperty(0xfffffffe, -2027, out prjObject) >= 0)
            {
                return (EnvDTE.Project)prjObject;
            }
            else
            {
                throw new ArgumentException(Properties.Resources.HierarchyIsNotProjectException, "hierarchy");
            }
        }

        /// <summary>
        /// Toes the DTE project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public static EnvDTE.Project ToDteProject(IVsProject project)
        {
            if (project == null) throw new ArgumentNullException("project");

            return ToDteProject(project as IVsHierarchy);
        }

        /// <summary>
        /// Gets the project service provider.
        /// </summary>
        /// <param name="hierarchy">The hierarchy.</param>
        /// <returns></returns>
        public static IServiceProvider GetProjectServiceProvider(IVsHierarchy hierarchy)
        {
            if (hierarchy == null)
            {
                throw new ArgumentNullException("hierarchy");
            }

            Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleProvider;
            if (hierarchy.GetSite(out oleProvider) >= 0)
            {
                return new ServiceProvider(oleProvider);
            }
            else
            {
                throw new ArgumentException(Properties.Resources.HierarchyNotSitedException, "hierarchy");
            }
        }

        /// <summary>
        /// Gets the project service provider.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public static IServiceProvider GetProjectServiceProvider(IVsProject project)
        {
            if (project == null) throw new ArgumentNullException("project");

            return GetProjectServiceProvider(project as IVsHierarchy);
        }

        /// <summary>
        /// Gets the project service provider.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public static IServiceProvider GetProjectServiceProvider(EnvDTE.Project project)
        {
            return GetProjectServiceProvider(ToHierarchy(project));
        }
    }
}
