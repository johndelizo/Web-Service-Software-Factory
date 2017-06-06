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
using EnvDTE;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.UnitTestLibrary
{
	public class MockEnvDTEProject : EnvDTE.Project
	{
        const string vsCMLanguageCSharp = "{B5E9BD34-6D3E-4B5D-925E-8A43B79820B4}";
		MockVSHierarchy hierarchy;
		CodeModel codeModel = new MockCodeModel(vsCMLanguageCSharp);
		MockProjectProperties projectProperties = new MockProjectProperties();
		object envDteObject = null;


		public MockEnvDTEProject(MockVSHierarchy hierarchy)
		{
			this.hierarchy = hierarchy;
			projectProperties.Add("RootNamespace", "Namespace1");

			envDteObject = new MockEnvDteVSProject(this);
		}

		public void SetCodeModel(CodeModel codeModel)
		{
			this.codeModel = codeModel;
		}


		public MockVSHierarchy Hierarchy
		{
			get { return hierarchy; }
		}

		#region Project Members

		EnvDTE.CodeModel EnvDTE.Project.CodeModel
		{
			get
			{
				return codeModel;
			}
		}

		EnvDTE.Projects EnvDTE.Project.Collection
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		EnvDTE.ConfigurationManager EnvDTE.Project.ConfigurationManager
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		EnvDTE.DTE EnvDTE.Project.DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		void EnvDTE.Project.Delete()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		string EnvDTE.Project.ExtenderCATID
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		object EnvDTE.Project.ExtenderNames
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		string EnvDTE.Project.FileName
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		string EnvDTE.Project.FullName
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		EnvDTE.Globals EnvDTE.Project.Globals
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		bool EnvDTE.Project.IsDirty
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

		string EnvDTE.Project.Kind
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		string EnvDTE.Project.Name
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

		public object Object
		{
			get { return envDteObject; }
			set { envDteObject = value; }
		}

		EnvDTE.ProjectItem EnvDTE.Project.ParentProjectItem
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		EnvDTE.ProjectItems EnvDTE.Project.ProjectItems
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		EnvDTE.Properties EnvDTE.Project.Properties
		{
			get { return this.projectProperties; }
		}

		void EnvDTE.Project.Save(string FileName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		void EnvDTE.Project.SaveAs(string NewFileName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		bool EnvDTE.Project.Saved
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

		string EnvDTE.Project.UniqueName
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		object EnvDTE.Project.get_Extender(string ExtenderName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}

	public class MockProjectProperties : EnvDTE.Properties
	{
		List<Property> propertiesList = new List<Property>();

		public MockProjectProperties()
		{
		}


		public Property Add(string name, string value)
		{
			MockProperty prop = new MockProperty();
			prop.Name = name;
			prop.Value = value;
			propertiesList.Add(prop);

			return prop;
		}

		#region Properties Members

		public object Application
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public int Count
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public DTE DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			return this.propertiesList.GetEnumerator();
		}

		public Property Item(object index)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public object Parent
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
	}

	public class MockProperty : EnvDTE.Property
	{

		string name;
		object value;

		#region Property Members

		public object Application
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public Properties Collection
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public DTE DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string Name
		{
			set { name = value; }
			get { return name; }
		}

		public short NumIndices
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public object Object
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

		public Properties Parent
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public object Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}

		public object get_IndexedValue(object Index1, object Index2, object Index3, object Index4)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void let_Value(object lppvReturn)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void set_IndexedValue(object Index1, object Index2, object Index3, object Index4, object Val)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}


	public class MockEnvDTEWebSite : VsWebSite.VSWebSite
	{
		private Project project;
		private MockVSWebSiteAssemblyReferences references = new MockVSWebSiteAssemblyReferences();

		#region VSWebSite Members

		public ProjectItem AddFromTemplate(string bstrRelFolderUrl, string bstrWizardName, string bstrLanguage, string bstrItemName, bool bUseCodeSeparation, string bstrMasterPage, string bstrDocType)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public VsWebSite.CodeFolders CodeFolders
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public DTE DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string EnsureServerRunning()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string GetUniqueFilename(string bstrFolder, string bstrRoot, string bstrDesiredExt)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool PreCompileWeb(string bstrCompilePath, bool bUpdateable)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public Project Project
		{
			get { return this.project; }
			set { this.project = value; }
		}

		public VsWebSite.AssemblyReferences References
		{
			get { return references; }
		}

		public void Refresh()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string TemplatePath
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string URL
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public string UserTemplatePath
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public VsWebSite.VSWebSiteEvents VSWebSiteEvents
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void WaitUntilReady()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public VsWebSite.WebReferences WebReferences
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public VsWebSite.WebServices WebServices
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
	}

	public class MockEnvDteVSProject : VSLangProj.VSProject
	{
		Project envDteProject = null;
		MockVSProjectReferences references = new MockVSProjectReferences();

		public MockEnvDteVSProject(Project envDteProject)
		{
			this.envDteProject = envDteProject;
		}
		#region VSProject Members

		public ProjectItem AddWebReference(string bstrUrl)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public VSLangProj.BuildManager BuildManager
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void CopyProject(string bstrDestFolder, string bstrDestUNCPath, VSLangProj.prjCopyProjectOption copyProjectOption, string bstrUsername, string bstrPassword)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public ProjectItem CreateWebReferencesFolder()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public DTE DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public VSLangProj.VSProjectEvents Events
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void Exec(VSLangProj.prjExecCommand command, int bSuppressUI, object varIn, out object pVarOut)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void GenerateKeyPairFiles(string strPublicPrivateFile, string strPublicOnlyFile)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string GetUniqueFilename(object pDispatch, string bstrRoot, string bstrDesiredExt)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public VSLangProj.Imports Imports
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public Project Project
		{
			get { return this.envDteProject; }
			set { this.envDteProject = value; }
		}

		public VSLangProj.References References
		{
			get { return references; }
		}

		public void Refresh()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string TemplatePath
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public ProjectItem WebReferencesFolder
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool WorkOffline
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
	}

	internal class MockVSWebSiteAssemblyReferences : VsWebSite.AssemblyReferences
	{

		List<MockVSWebSiteAssemblyReference> references = new List<MockVSWebSiteAssemblyReference>();

		#region AssemblyReferences Members

		public VsWebSite.AssemblyReference AddFromFile(string bstrPath)
		{
			MockVSWebSiteAssemblyReference reference = new MockVSWebSiteAssemblyReference();
			reference.FullPath = bstrPath;
			references.Add(reference);
			return reference;
		}

		public VsWebSite.AssemblyReference AddFromGAC(string bstrAssemblyName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void AddFromProject(Project pProj)
		{
			MockVSWebSiteAssemblyReference reference = new MockVSWebSiteAssemblyReference();
			reference.ReferencedProject = pProj;
			references.Add(reference);
		}

		public Project ContainingProject
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public int Count
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public DTE DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public VsWebSite.AssemblyReference Item(object index)
		{
			return references[(int)index];
		}

		#endregion

		internal class MockVSWebSiteAssemblyReference : VsWebSite.AssemblyReference
		{
			string path = string.Empty;
			Project referenceProject = null;

			#region AssemblyReference Members

			public Project ContainingProject
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public DTE DTE
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string FullPath
			{
				get { return path; }
				set { path = value; }
			}

			public string Name
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public VsWebSite.AssemblyReferenceType ReferenceKind
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public Project ReferencedProject
			{
				get { return referenceProject; }
				set { referenceProject = value; }
			}

			public void Remove()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public string StrongName
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			#endregion
		}
	}

	internal class MockVSProjectReferences : VSLangProj.References
	{
		List<VSLangProj.Reference> references = new List<VSLangProj.Reference>();

		#region References Members

		public VSLangProj.Reference Add(string bstrPath)
		{
			MockVSReference reference = new MockVSReference();
			reference.Path = bstrPath;
			references.Add(reference);
			return reference;
		}

		public VSLangProj.Reference AddActiveX(string bstrTypeLibGuid, int lMajorVer, int lMinorVer, int lLocaleId, string bstrWrapperTool)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public VSLangProj.Reference AddProject(Project pProject)
		{
			MockVSReference reference = new MockVSReference();
			reference.SourceProject = pProject;
			references.Add(reference);
			return reference;
		}

		public Project ContainingProject
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public int Count
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public DTE DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public VSLangProj.Reference Find(string bstrIdentity)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public VSLangProj.Reference Item(object index)
		{
			return references[(int)index];
		}

		public object Parent
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion

		internal class MockVSReference : VSLangProj.Reference
		{
			private string path;
			private Project project;

			#region Reference Members

			public int BuildNumber
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public VSLangProj.References Collection
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public Project ContainingProject
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public bool CopyLocal
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

			public string Culture
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public DTE DTE
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string Description
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string ExtenderCATID
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public object ExtenderNames
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string Identity
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public int MajorVersion
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public int MinorVersion
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string Name
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string Path
			{
				get { return path; }
				set { path = value; }
			}

			public string PublicKeyToken
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public void Remove()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			public int RevisionNumber
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public Project SourceProject
			{
				get { return this.project; }
				set { this.project = value; }
			}

			public bool StrongName
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public VSLangProj.prjReferenceType Type
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string Version
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public object get_Extender(string ExtenderName)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}
	}
}
