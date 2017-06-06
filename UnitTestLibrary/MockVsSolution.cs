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
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.UnitTestLibrary
{
	public sealed class MockVsSolution : IVsSolution
	{
		private MockVSHierarchy root;
		private Dictionary<Guid, MockVSHierarchy> projects;

		static MockVsSolution solution;

		public static MockVsSolution Solution
		{
			get
			{
				return solution;
			}
		}

		public MockVsSolution(MockVSHierarchy root)
		{
			solution = this;
			this.root = root;
			this.projects = new Dictionary<Guid, MockVSHierarchy>();
		}

		public MockVSHierarchy Root
		{
			get
			{
				return root;
			}
		}

		internal void RegisterProjectInSolution(MockVSHierarchy project)
		{
            if (!projects.ContainsKey(project.GUID))
            {
                projects.Add(project.GUID, project);
            }
		}

		internal void UnregisterProjectInSolution(MockVSHierarchy project)
		{
			projects.Remove(project.GUID);
		}

		#region IVsSolution Members

		public int AddVirtualProject(IVsHierarchy pHierarchy, uint grfAddVPFlags)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int AddVirtualProjectEx(IVsHierarchy pHierarchy, uint grfAddVPFlags, ref Guid rguidProjectID)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int AdviseSolutionEvents(IVsSolutionEvents pSink, out uint pdwCookie)
		{
			pdwCookie = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int CanCreateNewProjectAtLocation(int fCreateNewSolution, string pszFullProjectFilePath, out int pfCanCreate)
		{
			pfCanCreate = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int CloseSolutionElement(uint grfCloseOpts, IVsHierarchy pHier, uint docCookie)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int CreateNewProjectViaDlg(string pszExpand, string pszSelect, uint dwReserved)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int CreateProject(ref Guid rguidProjectType, string lpszMoniker, string lpszLocation, string lpszName, uint grfCreateFlags, ref Guid iidProject, out IntPtr ppProject)
		{
			MockVSHierarchy newProject = new MockVSHierarchy(lpszName);
			MockVsSolution.Solution.Root.AddProject(newProject);
			ppProject = Marshal.GetIUnknownForObject(newProject);
			return VSConstants.S_OK;
		}

		public int CreateSolution(string lpszLocation, string lpszName, uint grfCreateFlags)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int GenerateNextDefaultProjectName(string pszBaseName, string pszLocation, out string pbstrProjectName)
		{
			pbstrProjectName = string.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GenerateUniqueProjectName(string lpszRoot, out string pbstrProjectName)
		{
			pbstrProjectName = string.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GetGuidOfProject(IVsHierarchy pHierarchy, out Guid pguidProjectID)
		{
			pguidProjectID = Guid.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GetItemInfoOfProjref(string pszProjref, int propid, out object pvar)
		{
			pvar = null;
			return VSConstants.E_NOTIMPL;
		}

		public int GetItemOfProjref(string pszProjref, out IVsHierarchy ppHierarchy, out uint pitemid, out string pbstrUpdatedProjref, VSUPDATEPROJREFREASON[] puprUpdateReason)
		{
			ppHierarchy = null;
			pitemid = 0;
			pbstrUpdatedProjref = string.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProjectEnum(uint grfEnumFlags, ref Guid rguidEnumOnlyThisType, out IEnumHierarchies ppenum)
		{
			ppenum = null;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProjectFactory(uint dwReserved, Guid[] pguidProjectType, string pszMkProject, out IVsProjectFactory ppProjectFactory)
		{
			ppProjectFactory = null;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProjectFilesInSolution(uint grfGetOpts, uint cProjects, string[] rgbstrProjectNames, out uint pcProjectsFetched)
		{
			pcProjectsFetched = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProjectInfoOfProjref(string pszProjref, int propid, out object pvar)
		{
			pvar = null;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProjectOfGuid(ref Guid rguidProjectID, out IVsHierarchy ppHierarchy)
		{
			if (rguidProjectID == Guid.Empty)
			{
				ppHierarchy = this.root;
				return VSConstants.S_OK;
			}
			else if (projects.ContainsKey(rguidProjectID))
			{
				ppHierarchy = projects[rguidProjectID];
				return VSConstants.S_OK;
			}
			else
			{
				ppHierarchy = null;
				return VSConstants.E_NOINTERFACE;
			}
		}

		public int GetProjectOfProjref(string pszProjref, out IVsHierarchy ppHierarchy, out string pbstrUpdatedProjref, VSUPDATEPROJREFREASON[] puprUpdateReason)
		{
			ppHierarchy = null;
			pbstrUpdatedProjref = null;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProjectOfUniqueName(string pszUniqueName, out IVsHierarchy ppHierarchy)
		{
			foreach (MockVSHierarchy project in projects.Values)
			{
				if (project.Name == pszUniqueName)
				{
					ppHierarchy = project;
					return VSConstants.S_OK;
				}
			}
			ppHierarchy = null;
			return VSConstants.E_FAIL;
		}

		public int GetProjectTypeGuid(uint dwReserved, string pszMkProject, out Guid pguidProjectType)
		{
			pguidProjectType = Guid.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProjrefOfItem(IVsHierarchy pHierarchy, uint itemid, out string pbstrProjref)
		{
			pbstrProjref = string.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProjrefOfProject(IVsHierarchy pHierarchy, out string pbstrProjref)
		{
			pbstrProjref = string.Empty;
			return VSConstants.S_OK;
		}

		public int GetProperty(int propid, out object pvar)
		{
			pvar = null;
			return VSConstants.E_NOTIMPL;
		}

		public int GetSolutionInfo(out string pbstrSolutionDirectory, out string pbstrSolutionFile, out string pbstrUserOptsFile)
		{
			pbstrSolutionDirectory = string.Empty;
			pbstrSolutionFile = string.Empty;
			pbstrUserOptsFile = string.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GetUniqueNameOfProject(IVsHierarchy pHierarchy, out string pbstrUniqueName)
		{
			pbstrUniqueName = ((MockVSHierarchy)pHierarchy).Name;
			return VSConstants.S_OK;
		}

		public int GetVirtualProjectFlags(IVsHierarchy pHierarchy, out uint pgrfAddVPFlags)
		{
			pgrfAddVPFlags = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int OnAfterRenameProject(IVsProject pProject, string pszMkOldName, string pszMkNewName, uint dwReserved)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int OpenSolutionFile(uint grfOpenOpts, string pszFilename)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int OpenSolutionViaDlg(string pszStartDirectory, int fDefaultToAllProjectsFilter)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int QueryEditSolutionFile(out uint pdwEditResult)
		{
			pdwEditResult = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int QueryRenameProject(IVsProject pProject, string pszMkOldName, string pszMkNewName, uint dwReserved, out int pfRenameCanContinue)
		{
			pfRenameCanContinue = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int RemoveVirtualProject(IVsHierarchy pHierarchy, uint grfRemoveVPFlags)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int SaveSolutionElement(uint grfSaveOpts, IVsHierarchy pHier, uint docCookie)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int SetProperty(int propid, object var)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int UnadviseSolutionEvents(uint dwCookie)
		{
			return VSConstants.E_NOTIMPL;
		}

		#endregion
	}
}
