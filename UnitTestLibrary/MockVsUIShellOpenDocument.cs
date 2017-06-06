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
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.UnitTestLibrary
{
	public sealed class MockVsUIShellOpenDocument : IVsUIShellOpenDocument
	{
		#region IVsUIShellOpenDocument Members

		int IVsUIShellOpenDocument.AddStandardPreviewer(string pszExePath, string pszDisplayName, int fUseDDE, string pszDDEService, string pszDDETopicOpenURL, string pszDDEItemOpenURL, string pszDDETopicActivate, string pszDDEItemActivate, uint aspAddPreviewerFlags)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.GetFirstDefaultPreviewer(out string pbstrDefBrowserPath, out int pfIsInternalBrowser, out int pfIsSystemBrowser)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.GetStandardEditorFactory(uint dwReserved, ref Guid pguidEditorType, string pszMkDocument, ref Guid rguidLogicalView, out string pbstrPhysicalView, out IVsEditorFactory ppEF)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.InitializeEditorInstance(uint grfIEI, IntPtr punkDocView, IntPtr punkDocData, string pszMkDocument, ref Guid rguidEditorType, string pszPhysicalView, ref Guid rguidLogicalView, string pszOwnerCaption, string pszEditorCaption, IVsUIHierarchy pHier, uint itemid, IntPtr punkDocDataExisting, Microsoft.VisualStudio.OLE.Interop.IServiceProvider pSPHierContext, ref Guid rguidCmdUI, out IVsWindowFrame ppWindowFrame)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.IsDocumentInAProject(string pszMkDocument, out IVsUIHierarchy ppUIH, out uint pitemid, out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP, out int pDocInProj)
		{
			MockVSHierarchy doc=new MockVSHierarchy(pszMkDocument);
			ppUIH = doc;
			pitemid = VSConstants.VSITEMID_ROOT;
			pDocInProj = 1;
			ppSP = null;
			return VSConstants.S_OK;
		}

		int IVsUIShellOpenDocument.IsDocumentOpen(IVsUIHierarchy pHierCaller, uint itemidCaller, string pszMkDocument, ref Guid rguidLogicalView, uint grfIDO, out IVsUIHierarchy ppHierOpen, uint[] pitemidOpen, out IVsWindowFrame ppWindowFrame, out int pfOpen)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.IsSpecificDocumentViewOpen(IVsUIHierarchy pHierCaller, uint itemidCaller, string pszMkDocument, ref Guid rguidEditorType, string pszPhysicalView, uint grfIDO, out IVsUIHierarchy ppHierOpen, out uint pitemidOpen, out IVsWindowFrame ppWindowFrame, out int pfOpen)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.MapLogicalView(ref Guid rguidEditorType, ref Guid rguidLogicalView, out string pbstrPhysicalView)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.OpenCopyOfStandardEditor(IVsWindowFrame pWindowFrame, ref Guid rguidLogicalView, out IVsWindowFrame ppNewWindowFrame)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.OpenDocumentViaProject(string pszMkDocument, ref Guid rguidLogicalView, out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP, out IVsUIHierarchy ppHier, out uint pitemid, out IVsWindowFrame ppWindowFrame)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.OpenDocumentViaProjectWithSpecific(string pszMkDocument, uint grfEditorFlags, ref Guid rguidEditorType, string pszPhysicalView, ref Guid rguidLogicalView, out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP, out IVsUIHierarchy ppHier, out uint pitemid, out IVsWindowFrame ppWindowFrame)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.OpenSpecificEditor(uint grfOpenSpecific, string pszMkDocument, ref Guid rguidEditorType, string pszPhysicalView, ref Guid rguidLogicalView, string pszOwnerCaption, IVsUIHierarchy pHier, uint itemid, IntPtr punkDocDataExisting, Microsoft.VisualStudio.OLE.Interop.IServiceProvider pSPHierContext, out IVsWindowFrame ppWindowFrame)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.OpenStandardEditor(uint grfOpenStandard, string pszMkDocument, ref Guid rguidLogicalView, string pszOwnerCaption, IVsUIHierarchy pHier, uint itemid, IntPtr punkDocDataExisting, Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp, out IVsWindowFrame ppWindowFrame)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.OpenStandardPreviewer(uint ospOpenDocPreviewer, string pszURL, VSPREVIEWRESOLUTION resolution, uint dwReserved)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIShellOpenDocument.SearchProjectsForRelativePath(uint grfRPS, string pszRelPath, string[] pbstrAbsPath)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
