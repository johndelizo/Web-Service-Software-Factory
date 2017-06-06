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
using System.Collections.Generic;
using Microsoft.Build;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Build.Evaluation;
using System.Collections.Concurrent;

namespace Microsoft.Practices.UnitTestLibrary
{
	public class MockVSHierarchy : IVsHierarchy, IVsProject, IVsUIHierarchy, VSLangProj.VSProject, VSLangProj.References, IVsProject2
	{
		Guid guid;
		string fileName;
		List<string> children;
		Project project;
		List<MockVSHierarchy> subProjects;
		MockVSHierarchy parent;
		object externalObject = null ;
        static ConcurrentDictionary<Guid, string> visitedFiles = new ConcurrentDictionary<Guid, string>();

		public List<MockVSHierarchy> SubProjects
		{
			get { return subProjects; }
		}

		public List<string> Children
		{
			get { return children; }
		}

		public string FileName
		{
			get { return fileName; }
		}

		public string Name
		{
			get
			{
				if (fileName.StartsWith("<"))
				{
					return fileName;
				}
				else
				{
					return new FileInfo(fileName).Name;
				}
			}
		}

		public Guid GUID
		{
			get { return guid; }
		}

		public void AddChild(string child)
		{
			children.Add(child);
		}

		private Guid typeGuid;
		public Guid TypeGuid
		{
			get { return typeGuid; }
			set { typeGuid = value; }
		}
	
		public void AddChildren(int childrenSize)
		{
			for (int i = 0; i < childrenSize; i++)
			{
				children.Add(string.Format("Child{0}", i));
			}
		}

		public object ExtObject
		{
			get { return externalObject;  }
			set { externalObject = value; }
		}

		public void AddProject(MockVSHierarchy project)
		{
			Assert.AreNotSame(this, project);
			this.subProjects.Add(project);
			MockVsSolution.Solution.RegisterProjectInSolution(project);
		}

		public void RemoveProject(MockVSHierarchy project)
		{
			this.subProjects.Remove(project);
			MockVsSolution.Solution.UnregisterProjectInSolution(project);
		}

		public MockVSHierarchy()
			: this(0, "<Solution>", Guid.Empty, null)
		{
		}

		public MockVSHierarchy(string name)
			: this(0, name, Guid.NewGuid(),null)
		{
		}

		public MockVSHierarchy(string name,MockVSHierarchy parent)
			: this(0, name, Guid.NewGuid(), parent)
		{
		}

		public MockVSHierarchy(string name, Guid guid)
			: this(0, name, guid, null)
		{
		}

		public MockVSHierarchy(int children)
			: this(children, "Project.Project", Guid.NewGuid(),null)
		{
		}

		public MockVSHierarchy(int childrenSize, string name, Guid guid, MockVSHierarchy parent)
		{
			if ( parent==null && MockVsSolution.Solution != null && MockVsSolution.Solution.Root != null)
			{
				this.parent = MockVsSolution.Solution.Root;
			}
			else
			{
				this.parent = parent;
			}
			this.guid = guid;
			this.subProjects = new List<MockVSHierarchy>();
			this.fileName = name;
			this.children = new List<string>();
			AddChildren(childrenSize);
            if (guid != Guid.Empty &&
                !Directory.Exists(fileName))
            {
                this.project = new Microsoft.Build.Evaluation.Project();  //Microsoft.Build.BuildEngine.Engine.GlobalEngine.CreateNewProject(); 
                string fullPath;
                if (!visitedFiles.TryGetValue(guid, out fullPath))
                {
                    try
                    {
                        this.project.Save(fileName);
                    }
                    catch(InvalidOperationException)
                    {
                        // rename to a random value
                        fileName = Path.ChangeExtension(Path.GetRandomFileName(), ".project");
                        this.project = new Microsoft.Build.Evaluation.Project();
                        this.project.Save(fileName);
                    }
                    fullPath = this.project.FullPath;
                    visitedFiles.GetOrAdd(guid, fullPath);
                 }
                fileName = fullPath;
            }

			externalObject = new MockEnvDTEProject(this);
		}

		#region IVsHierarchy Members

		public int AdviseHierarchyEvents(IVsHierarchyEvents pEventSink, out uint pdwCookie)
		{
			pdwCookie = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int Close()
		{
			return VSConstants.E_NOTIMPL;
		}

		public int GetCanonicalName(uint itemid, out string pbstrName)
		{
			pbstrName = string.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GetGuidProperty(uint itemid, int propid, out Guid pguid)
		{
			__VSHPROPID vshPropId = (__VSHPROPID)propid;
			switch (vshPropId)
			{
				case __VSHPROPID.VSHPROPID_ProjectIDGuid:
					if (itemid == VSConstants.VSITEMID_ROOT)
					{
						pguid = guid;
						return VSConstants.S_OK;
					}
					break;

				case __VSHPROPID.VSHPROPID_TypeGuid:
					if (itemid == VSConstants.VSITEMID_ROOT)
					{
						pguid = this.TypeGuid;
					}
					else if (IsProject(itemid)) {
						pguid = GetProject(itemid).TypeGuid;
					}
					else if (IsChild(itemid))
					{
						pguid = VSConstants.GUID_ItemType_PhysicalFile;
					}
					else
					{
						pguid = Guid.Empty;
						return VSConstants.S_FALSE;
					}

					return VSConstants.S_OK;
			}


			pguid = Guid.Empty;
			return VSConstants.DISP_E_MEMBERNOTFOUND;
		}

		public int GetNestedHierarchy(uint itemid, ref Guid iidHierarchyNested, out IntPtr ppHierarchyNested, out uint pitemidNested)
		{
			if (itemid == VSConstants.VSITEMID_ROOT || itemid == VSConstants.VSITEMID_NIL)
			{
				ppHierarchyNested = IntPtr.Zero;
				pitemidNested = VSConstants.VSITEMID_NIL;
			}
			else if (IsProject(itemid))
			{
				itemid = itemid - (uint)this.children.Count;
				ppHierarchyNested = Marshal.GetIUnknownForObject(SubProjects[(int)itemid]);
				pitemidNested = VSConstants.VSITEMID_ROOT;
			}
			else
			{
				ppHierarchyNested = IntPtr.Zero;
				pitemidNested = VSConstants.VSITEMID_NIL;
			}
			return VSConstants.S_OK;
		}

		private bool IsChild(uint itemId)
		{
			return (itemId != VSConstants.VSITEMID_ROOT &&
					itemId < HierarchyChildrenCount &&
					itemId < this.children.Count);
		}

		private bool IsProject(uint itemId)
		{
			return (itemId != VSConstants.VSITEMID_ROOT &&
					itemId < HierarchyChildrenCount &&
					itemId >= this.children.Count);
		}

		private int HierarchyChildrenCount
		{
			get { return this.children.Count + this.subProjects.Count; }
		}

		public int GetProperty(uint itemid, int _propid, out object pvar)
		{
			pvar = null;
			__VSHPROPID propId = (__VSHPROPID)_propid;
			switch (propId)
			{
				case __VSHPROPID.VSHPROPID_ExtObject:
					{
						if (itemid == VSConstants.VSITEMID_ROOT)
						{
							pvar = ExtObject;
							return VSConstants.S_OK;
						}
						break;
					}
				case __VSHPROPID.VSHPROPID_SaveName:
					{
						if (itemid == VSConstants.VSITEMID_ROOT)
						{
							pvar = this.fileName;
						}
						else if (IsChild(itemid))
						{
							pvar = new FileInfo(GetChild(itemid)).FullName;
						}
						else if (IsProject(itemid))
						{
							pvar = GetProject(itemid).FileName;						
						}

						return VSConstants.S_OK;
					}
				case __VSHPROPID.VSHPROPID_Name:
					{
						if (itemid == VSConstants.VSITEMID_ROOT)
						{
							pvar = Name;
						}
						else if (IsChild(itemid))
						{
							pvar =  new FileInfo(GetChild(itemid)).Name;
						}
						else if (IsProject(itemid))
						{
							pvar = GetProject(itemid).Name;
						}
						return VSConstants.S_OK;
					}
				case __VSHPROPID.VSHPROPID_FirstVisibleChild:
				case __VSHPROPID.VSHPROPID_FirstChild:
					{
						if (itemid == VSConstants.VSITEMID_ROOT && (HierarchyChildrenCount) > 0)
						{
							pvar = 0;
							return VSConstants.S_OK;
						}
						else
						{
							pvar = VSConstants.VSITEMID_NIL;
							return VSConstants.S_OK;
						}
					}
				case __VSHPROPID.VSHPROPID_NextSibling:
				case __VSHPROPID.VSHPROPID_NextVisibleSibling:
					{
						if (itemid >= 0 && (itemid + 1) < HierarchyChildrenCount)
						{
							pvar = itemid + 1;
							return VSConstants.S_OK;
						}
						else
						{
							pvar = VSConstants.VSITEMID_NIL;
							return VSConstants.S_OK;
						}
					}
				case __VSHPROPID.VSHPROPID_ProjectDir:
					{
						pvar = System.IO.Directory.GetCurrentDirectory();
						return VSConstants.S_OK;
					}
				case __VSHPROPID.VSHPROPID_ParentHierarchy:
					{
						pvar = this.parent;
						return VSConstants.S_OK;
					}
			}
			return VSConstants.DISP_E_MEMBERNOTFOUND;
		}

		private MockVSHierarchy GetProject(uint itemid)
		{
			uint projectItemId = itemid - (uint)this.children.Count;
			return this.subProjects[(int)projectItemId];
		}

		private string GetChild(uint itemid)
		{
			return this.children[(int)itemid];
		}

		public int GetSite(out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
		{
			ppSP = null;
			return VSConstants.E_NOTIMPL;
		}

		public int ParseCanonicalName(string pszName, out uint pitemid)
		{
			pitemid = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int QueryClose(out int pfCanClose)
		{
			pfCanClose = 0;
			return VSConstants.E_NOTIMPL;
		}

		public int SetGuidProperty(uint itemid, int propid, ref Guid rguid)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int SetProperty(uint itemid, int propid, object var)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int UnadviseHierarchyEvents(uint dwCookie)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int Unused0()
		{
			return VSConstants.E_NOTIMPL;
		}

		public int Unused1()
		{
			return VSConstants.E_NOTIMPL;
		}

		public int Unused2()
		{
			return VSConstants.E_NOTIMPL;
		}

		public int Unused3()
		{
			return VSConstants.E_NOTIMPL;
		}

		public int Unused4()
		{
			return VSConstants.E_NOTIMPL;
		}

		#endregion

		#region IVsProject Members

		int IVsProject.AddItem(uint itemidLoc, VSADDITEMOPERATION dwAddItemOperation, string pszItemName, uint cFilesToOpen, string[] rgpszFilesToOpen, IntPtr hwndDlgOwner, VSADDRESULT[] pResult)
		{
			if (Directory.Exists(rgpszFilesToOpen[0]))
			{
				AddProject(new MockVSHierarchy(rgpszFilesToOpen[0],this));
			}
			else
			{
				children.Add(rgpszFilesToOpen[0]);
				if (project != null)
				{
					FileInfo itemFileInfo = new FileInfo(rgpszFilesToOpen[0]);
					project.Save(fileName);
					FileInfo projectFileInfo = new FileInfo(project.FullPath);
					string itemName = itemFileInfo.FullName.Substring(projectFileInfo.Directory.FullName.Length + 1);
					project.AddItem("Compile", itemName);
					project.Save(fileName);
				}
			}
			return VSConstants.S_OK;
		}

		int IVsProject.GenerateUniqueItemName(uint itemidLoc, string pszExt, string pszSuggestedRoot, out string pbstrItemName)
		{
			pbstrItemName = string.Empty;
			return VSConstants.E_NOTIMPL;
		}

		int IVsProject.GetItemContext(uint itemid, out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
		{
			ppSP = null;
			return VSConstants.E_NOTIMPL;
		}

		int IVsProject.GetMkDocument(uint itemid, out string pbstrMkDocument)
		{
			if (itemid == VSConstants.VSITEMID_ROOT)
			{
				pbstrMkDocument = this.fileName;
			}
			else
			{
				pbstrMkDocument = children[(int)itemid];
			}
			return VSConstants.S_OK;
		}

		int IVsProject.IsDocumentInProject(string pszMkDocument, out int pfFound, VSDOCUMENTPRIORITY[] pdwPriority, out uint pitemid)
		{
			uint i = 0;
			foreach (string doc in children)
			{
				if (doc == pszMkDocument)
				{
					pfFound = 1;
					pitemid = i;
					return VSConstants.S_OK;
				}
				i++;
			}
			pitemid = VSConstants.VSITEMID_NIL;
			pfFound = 0;
			return VSConstants.S_OK;
		}

		int IVsProject.OpenItem(uint itemid, ref Guid rguidLogicalView, IntPtr punkDocDataExisting, out IVsWindowFrame ppWindowFrame)
		{
			ppWindowFrame = new MockVsWindowFrame();
			return VSConstants.S_OK;
		}

		#endregion

		#region IVsUIHierarchy Members

		int IVsUIHierarchy.AdviseHierarchyEvents(IVsHierarchyEvents pEventSink, out uint pdwCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.Close()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.ExecCommand(uint itemid, ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.GetCanonicalName(uint itemid, out string pbstrName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.GetGuidProperty(uint itemid, int propid, out Guid pguid)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.GetNestedHierarchy(uint itemid, ref Guid iidHierarchyNested, out IntPtr ppHierarchyNested, out uint pitemidNested)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.GetProperty(uint itemid, int propid, out object pvar)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.GetSite(out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.ParseCanonicalName(string pszName, out uint pitemid)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.QueryClose(out int pfCanClose)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.QueryStatusCommand(uint itemid, ref Guid pguidCmdGroup, uint cCmds, Microsoft.VisualStudio.OLE.Interop.OLECMD[] prgCmds, IntPtr pCmdText)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.SetGuidProperty(uint itemid, int propid, ref Guid rguid)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.SetProperty(uint itemid, int propid, object var)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.UnadviseHierarchyEvents(uint dwCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.Unused0()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.Unused1()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.Unused2()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.Unused3()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int IVsUIHierarchy.Unused4()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		#region VSProject Members

		EnvDTE.ProjectItem VSLangProj.VSProject.AddWebReference(string bstrUrl)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		VSLangProj.BuildManager VSLangProj.VSProject.BuildManager
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		void VSLangProj.VSProject.CopyProject(string bstrDestFolder, string bstrDestUNCPath, VSLangProj.prjCopyProjectOption copyProjectOption, string bstrUsername, string bstrPassword)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		EnvDTE.ProjectItem VSLangProj.VSProject.CreateWebReferencesFolder()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		EnvDTE.DTE VSLangProj.VSProject.DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		VSLangProj.VSProjectEvents VSLangProj.VSProject.Events
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		void VSLangProj.VSProject.Exec(VSLangProj.prjExecCommand command, int bSuppressUI, object varIn, out object pVarOut)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		void VSLangProj.VSProject.GenerateKeyPairFiles(string strPublicPrivateFile, string strPublicOnlyFile)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		string VSLangProj.VSProject.GetUniqueFilename(object pDispatch, string bstrRoot, string bstrDesiredExt)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		VSLangProj.Imports VSLangProj.VSProject.Imports
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		EnvDTE.Project VSLangProj.VSProject.Project
		{
			get
			{
				return ExtObject as EnvDTE.Project;
			}
		}

		VSLangProj.References VSLangProj.VSProject.References
		{
			get { return this; }
		}

		void VSLangProj.VSProject.Refresh()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		string VSLangProj.VSProject.TemplatePath
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		EnvDTE.ProjectItem VSLangProj.VSProject.WebReferencesFolder
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		bool VSLangProj.VSProject.WorkOffline
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		#endregion

		#region References Members

		VSLangProj.Reference VSLangProj.References.Add(string bstrPath)
		{
			if (!children.Contains(bstrPath))
			{
				children.Add(bstrPath);
			}
			return null;
		}

		VSLangProj.Reference VSLangProj.References.AddActiveX(string bstrTypeLibGuid, int lMajorVer, int lMinorVer, int lLocaleId, string bstrWrapperTool)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		VSLangProj.Reference VSLangProj.References.AddProject(EnvDTE.Project pProject)
		{
			MockEnvDTEProject project = (MockEnvDTEProject)pProject;
			if (!children.Contains(project.Hierarchy.FileName))
			{
				children.Add(project.Hierarchy.FileName);
			}
			return null;
		}

		EnvDTE.Project VSLangProj.References.ContainingProject
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		int VSLangProj.References.Count
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		EnvDTE.DTE VSLangProj.References.DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		VSLangProj.Reference VSLangProj.References.Find(string bstrIdentity)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		System.Collections.IEnumerator VSLangProj.References.GetEnumerator()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		VSLangProj.Reference VSLangProj.References.Item(object index)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		object VSLangProj.References.Parent
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion


		#region IVsProject2 Members

		int IVsProject2.AddItem(uint itemidLoc, VSADDITEMOPERATION dwAddItemOperation, string pszItemName, uint cFilesToOpen, string[] rgpszFilesToOpen, IntPtr hwndDlgOwner, VSADDRESULT[] pResult)
		{
			return ((IVsProject)this).AddItem(itemidLoc, dwAddItemOperation, pszItemName, cFilesToOpen, rgpszFilesToOpen, hwndDlgOwner, pResult);
		}

		int IVsProject2.GenerateUniqueItemName(uint itemidLoc, string pszExt, string pszSuggestedRoot, out string pbstrItemName)
		{
			return ((IVsProject)this).GenerateUniqueItemName(itemidLoc, pszExt, pszSuggestedRoot, out pbstrItemName);
		}

		int IVsProject2.GetItemContext(uint itemid, out Microsoft.VisualStudio.OLE.Interop.IServiceProvider ppSP)
		{
			return ((IVsProject)this).GetItemContext(itemid, out ppSP);
		}

		int IVsProject2.GetMkDocument(uint itemid, out string pbstrMkDocument)
		{
			return ((IVsProject)this).GetMkDocument(itemid, out pbstrMkDocument);
		}

		int IVsProject2.IsDocumentInProject(string pszMkDocument, out int pfFound, VSDOCUMENTPRIORITY[] pdwPriority, out uint pitemid)
		{
			return ((IVsProject)this).IsDocumentInProject(pszMkDocument, out pfFound, pdwPriority, out pitemid);
		}

		int IVsProject2.OpenItem(uint itemid, ref Guid rguidLogicalView, IntPtr punkDocDataExisting, out IVsWindowFrame ppWindowFrame)
		{
			return ((IVsProject)this).OpenItem(itemid, ref rguidLogicalView, punkDocDataExisting, out ppWindowFrame);
		}

		int IVsProject2.RemoveItem(uint dwReserved, uint itemid, out int pfResult)
		{
			if (itemid < HierarchyChildrenCount)
			{
				if (IsChild(itemid))
				{
					this.children.RemoveAt((int)itemid);
				}
				else
				{
					MockVSHierarchy project = GetProject(itemid);
					RemoveProject(project);
				}
				pfResult = 1;
				return VSConstants.S_OK;
			}
			pfResult = 0;
			return VSConstants.E_FAIL;
		}

		int IVsProject2.ReopenItem(uint itemid, ref Guid rguidEditorType, string pszPhysicalView, ref Guid rguidLogicalView, IntPtr punkDocDataExisting, out IVsWindowFrame ppWindowFrame)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
