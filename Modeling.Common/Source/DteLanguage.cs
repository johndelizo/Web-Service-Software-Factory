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
using System.Diagnostics;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Shell;

namespace Microsoft.Practices.Modeling.Common
{
    public static class DteLanguage
    {
        public static bool IsCSharpOrVbProject(IServiceProvider serviceProvider)
        {
            return IsCSharpOrVbProject(serviceProvider, false);
        }

        public static bool IsCSharpOrVbProject(IServiceProvider serviceProvider, bool excludeWebProjects)
        {
            EnvDTE.DTE dte = serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            return IsCSharpOrVbProject(DteHelper2.GetSelectedProject(dte), excludeWebProjects);
        }

        public static bool IsCSharpOrVbProject(EnvDTE.Project project)
        {
            return IsCSharpOrVbProject(project, false);
        }

        public static bool IsCSharpOrVbProject(EnvDTE.Project project, bool excludeWebProjects)
        {
            if (project == null) return false;

            return ReferenceUtil.IsCSharpProject(project) ||
                   (!excludeWebProjects && DteHelperEx.IsWebCSharpProject(project)) ||
                   IsVBProject(project) ||
                   (!excludeWebProjects && IsWebVBProject(project));
        }

        private static bool IsVBProject(EnvDTE.Project project)
        {
            return (project != null && 
                    project.Kind == ProvideRelatedFileAttribute.VisualBasicProjectGuid);
        }

        private static bool IsWebVBProject(EnvDTE.Project project)
        {
            if (project != null &&
                DteHelper2.IsWebProject(project) &&
                project.Properties != null)
            {
                try
                {
                    Property property = project.Properties.Item("CurrentWebsiteLanguage");
                    return ((property.Value != null) && property.Value.ToString().Equals("Visual Basic", StringComparison.InvariantCultureIgnoreCase));
                }
                catch (Exception exception)
                {
                    Trace.TraceError(exception.ToString());
                    return false;
                }
            }

            return false;
        }
    }
}
