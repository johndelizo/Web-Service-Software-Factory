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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.WCFReference.Interop;
using Microsoft.Practices.Modeling.Common;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.VisualStudio.Helper.Design
{
    public static class VsShellDialogs
	{
        private static string strLocation = String.Empty;

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool AddProjectDialog(
            IServiceProvider provider,
            string title,
            string templateName)
        {
            Guard.ArgumentNotNull(provider, "provider");
            Guard.ArgumentNotNullOrEmptyString(title, "title");
            Guard.ArgumentNotNullOrEmptyString(templateName, "templateName");

            try
            {
                IVsSolution solution = provider.GetService(typeof(IVsSolution)) as IVsSolution;
                uint pitemid = 0;
                using (HierarchyNode hierarchy = new HierarchyNode(solution, DteHelper2.GetCurrentSelection(provider, out pitemid)))
                {
                    if (hierarchy.IsSolution)
                    {
                        string templatePath = null;// (hierarchy.Hierarchy as EnvDTE.Solution).TemplatePath;
                        IVsSolution3 sln = (IVsSolution3)hierarchy.Hierarchy;
                        ErrorHandler.ThrowOnFailure(sln.CreateNewProjectViaDlgEx(
                            title, templatePath, "Visual C#", templateName, null, (uint)__VSCREATENEWPROJVIADLGEXFLAGS.VNPVDE_ALWAYSADDTOSOLUTION, null));
                    }
                }
                return true;
            }
            catch (COMException comEx)
            {
                if (comEx.ErrorCode != VSConstants.OLE_E_PROMPTSAVECANCELLED) 
                    Logger.Write(comEx);
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
            return false;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool AddProjectItemDialog(
            IServiceProvider provider, 
            string title, 
            string templateName, bool addExisting = false)
        {
            Guard.ArgumentNotNull(provider, "provider");
            Guard.ArgumentNotNullOrEmptyString(title, "title");
            Guard.ArgumentNotNullOrEmptyString(templateName, "templateName");

            try
            {
                IVsSolution solution = provider.GetService(typeof(IVsSolution)) as IVsSolution;
                uint pitemid = 0;
                using(HierarchyNode hierarchy = new HierarchyNode(solution, DteHelper2.GetCurrentSelection(provider, out pitemid)))
                using(HierarchyNode child = new HierarchyNode(hierarchy, pitemid))
                {
                    Guid rguidProject = hierarchy.TypeGuid;
                    IVsProject project = child.IsSolution ? null : (IVsProject)child.Hierarchy;
                    
                    string strFilter = String.Empty;                   
                    string templatePath = GetTemplatePathFromProject(hierarchy);
                    int iDontShowAgain;

                    IVsAddProjectItemDlg2 addProjectItemDlg2 = (IVsAddProjectItemDlg2)provider.GetService(typeof(SVsAddProjectItemDlg));

                    uint uiFlags;
                    if (addExisting)
                    {
                        uiFlags = (uint)(__VSADDITEMFLAGS.VSADDITEM_AddExistingItems |
                                         __VSADDITEMFLAGS.VSADDITEM_ProjectHandlesLinks |
                                         __VSADDITEMFLAGS.VSADDITEM_AllowStickyFilter | 
                                         __VSADDITEMFLAGS.VSADDITEM_ShowLocationField);
                        //strFilter = "Data Contract (*.datacontract)";
                    }
                    else
                    {
                        uiFlags = (uint)(__VSADDITEMFLAGS.VSADDITEM_AddNewItems |
                                         __VSADDITEMFLAGS.VSADDITEM_SuggestTemplateName |
                                         __VSADDITEMFLAGS.VSADDITEM_AllowHiddenTreeView | 
                                         __VSADDITEMFLAGS.VSADDITEM_AllowStickyFilter |
                                         __VSADDITEMFLAGS.VSADDITEM_ShowLocationField);
                    }
 
                    ErrorHandler.ThrowOnFailure(
                        addProjectItemDlg2.AddProjectItemDlgTitled(
                            child.IsRoot ? VSConstants.VSITEMID_ROOT : pitemid,
                            ref rguidProject,
                            project,
                            uiFlags,
                            string.Format(CultureInfo.CurrentCulture, title, hierarchy.Name),
                            templatePath,
                            templateName,
                            ref strLocation,
                            ref strFilter,
                            out iDontShowAgain),
                        VSConstants.OLE_E_PROMPTSAVECANCELLED);
                    return true;
                }
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
            return false;
        }

        // Add a service reference to the given project. 
        public static IVsAddWebReferenceResult AddWebReferenceDialog(
            IServiceProvider serviceProvider,
            string title,
            HierarchyNode proxyProject)
        {
            Guard.ArgumentNotNull(serviceProvider, "serviceProvider");
            Guard.ArgumentNotNull(proxyProject, "proxyProject");

            if (!proxyProject.GetProperty<Boolean>((int)__VSHPROPID3.VSHPROPID_ServiceReferenceSupported))
            {
                throw new NotSupportedException(Properties.Resources.ServiceReferenceSupported);
            }

            IVsAddWebReferenceDlg3 awrdlg = serviceProvider.GetService(typeof(SVsAddWebReferenceDlg3)) as IVsAddWebReferenceDlg3;
            IVsDiscoveryService discoveryService = serviceProvider.GetService(typeof(SVsDiscoveryService)) as IVsDiscoveryService;
            IDiscoverySession discoverySession = null;
            if (discoveryService != null) discoveryService.CreateDiscoverySession(out discoverySession);

            IVsAddWebReferenceResult addWebReferenceResult = null;
 
            int cancelled = 1;

            if (awrdlg != null)
            {
                awrdlg.ShowAddWebReferenceDialog(
                    proxyProject.Hierarchy,
                    discoverySession,
                    ServiceReferenceType.SRT_WCFReference | ServiceReferenceType.SRT_ASMXReference,
                    title,
                    null,
                    null,
                    out addWebReferenceResult,
                    out cancelled);
            }

            if (addWebReferenceResult != null && cancelled == 0)
            {
                return addWebReferenceResult;
            }
            else
            {
                return null;
            }
        }

        private static string GetTemplatePathFromProject(HierarchyNode hierarchy)
        {
            string language = ProjectNode.GetLanguageFromProject(hierarchy.ExtObject as EnvDTE.Project);
            switch (language)
            {
                case EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp:
                    return "Visual C# Items";
                case EnvDTE.CodeModelLanguageConstants.vsCMLanguageVB:
                    return "Common Items";
                default:
                    return null;
            }
        }
	}
}
