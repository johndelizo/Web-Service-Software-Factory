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
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using EnvDTE;

namespace Microsoft.Practices.VisualStudio.Helper.Tests
{
	/// <summary>
	/// Summary description for HierarchyNodeFixture
	/// </summary>
	[TestClass]
	public class ProjectNodeFixture
	{
		private MockVSHierarchy root = null;
		private MockVsSolution vsSolution = null;
		private MockVSHierarchy project = null;

		[TestInitialize]
		public void CreateVsSolution()
		{
			root = new MockVSHierarchy();
			vsSolution = new MockVsSolution(root);
			project = new MockVSHierarchy("Project.project");
			root.AddProject(project);
		}

		[TestCleanup]
		public void DeleteVsSolution()
		{
			project = null;
			vsSolution = null;
			root = null;
		}

				[TestMethod]
		public void TestAcceptsProjectReferenceToItSelf()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			int before = project.Children.Count;
			projectNode.AddProjectReference(project.GUID);
			Assert.AreEqual<int>(before,project.Children.Count);
		}

		[TestMethod]
		public void TestAddItem()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			string itemName = "item1.cs";
			projectNode.AddItem(itemName);
			string fullItemName = new FileInfo(itemName).FullName;
			Assert.IsTrue(project.Children.Contains(fullItemName));
		}

		[TestMethod]
		public void TestCanAddItem()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			string itemName = "item1.cs";
			Assert.IsTrue(projectNode.CanAddItem(itemName));
			string invalidItemName = "<item1>.cs";
			Assert.IsFalse(projectNode.CanAddItem(invalidItemName));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AddItemWithEmptyNameThrows()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			string itemName = ".cs";
			string fullItemName = new FileInfo(itemName).FullName;
			projectNode.AddItem(itemName);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AddItemWithInvalidNameThrows()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			string itemName = "Invalid<Name>";
			projectNode.AddItem(itemName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddItemWithNullNameThrows()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			projectNode.AddItem(null);
		}

		[TestMethod]
		public void TestOpenItem()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			string itemName = "item1.cs";
			HierarchyNode item = projectNode.AddItem(itemName);
			IVsWindowFrame wnd = projectNode.OpenItem(item);
			Assert.IsNotNull(wnd);
		}

		[TestMethod]
		public void TestGetMSBuildItem()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			string itemName = "item1.cs";
			Assert.IsNotNull(projectNode.AddItem(itemName));
			Assert.IsNotNull(projectNode.GetBuildItem(itemName));
			string itemName2 = ".\\item2.cs";
			Assert.IsNotNull(projectNode.AddItem(itemName2));
			Assert.IsNotNull(projectNode.GetBuildItem(itemName2));
			string itemName3 = ".\\FolderForItem3\\item3.cs";
			Assert.IsNotNull(projectNode.AddItem(itemName3));
			Assert.IsNotNull(projectNode.GetBuildItem(itemName3));
			string itemName4 = "FolderForItem4\\item4.cs";
			Assert.IsNotNull(projectNode.AddItem(itemName4));
			Assert.IsNotNull(projectNode.GetBuildItem(itemName4));
		}

		[TestMethod]
		public void TestFindSubFolder()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			string parentFolder = "Folder" + Guid.NewGuid().ToString();
			string subFolderName = "subFolder1";
			Assert.IsNotNull(projectNode.FindSubFolder("\\" + parentFolder + "\\" + subFolderName + "\\" + subFolderName + "\\"));
			HierarchyNode parentFolderNode = projectNode.FindByName(parentFolder);
			Assert.IsNotNull(parentFolderNode);
			HierarchyNode subFolder1Node = parentFolderNode.FindByName(subFolderName);
			Assert.IsNotNull(subFolder1Node);
			HierarchyNode subFolder2Node = subFolder1Node.FindByName(subFolderName);
			Assert.IsNotNull(subFolder2Node);
		}

		[TestMethod]
		public void TestCreateOrFindFolder()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			Assert.AreSame(projectNode, projectNode.FindOrCreateFolder("."));
			Assert.AreSame(projectNode, projectNode.FindSubFolder("."));
			Assert.AreSame(projectNode, projectNode.FindSubFolder(".\\."));
			Assert.AreSame(projectNode, projectNode.FindSubFolder(".\\.\\"));
			Assert.AreSame(projectNode, projectNode.FindSubFolder("\\"));
			Assert.AreSame(projectNode, projectNode.FindSubFolder("\\."));
			Assert.AreSame(projectNode, projectNode.FindSubFolder("\\.\\"));
			Assert.AreSame(projectNode, projectNode.FindSubFolder("\\.\\."));
		}

		[TestMethod]
		public void ShouldReturnCodeModelForNormalProject()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);

			string language = projectNode.Language;
			Assert.AreEqual<string>(CodeModelLanguageConstants.vsCMLanguageCSharp, language);
		}

		[TestMethod]
		public void ShouldReturnCodeModelForWebProject()
		{
			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			CreateWebsiteProject();
		
			string language = projectNode.Language;
			Assert.AreEqual<string>(CodeModelLanguageConstants.vsCMLanguageCSharp, language);
		}

		

		[TestMethod]
		public void ShouldAddReferencesToVSProject()
		{
			string assemblyPath = @"c:\blah\some.dll";

			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			projectNode.AddAssemblyReference(assemblyPath);

			string pathFromMock = ((VSLangProj.VSProject)((MockEnvDTEProject)project.ExtObject).Object).References.Item(0).Path;

			Assert.AreEqual<string>(assemblyPath, pathFromMock);
		}

		[TestMethod]
		public void ShouldAddProjectReferencestoVSProject()
		{
			MockVSHierarchy refProjHier = new MockVSHierarchy("refedproj.proj");
			root.AddProject(refProjHier);
			Project projToRef = refProjHier.ExtObject as Project;
			Assert.IsNotNull(projToRef);

			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			projectNode.AddProjectReference(refProjHier.GUID);

			Project projectFromMock = ((VSLangProj.VSProject)((MockEnvDTEProject)project.ExtObject).Object).References.Item(0).SourceProject;

			Assert.AreSame(projToRef, projectFromMock);
					
		}

		[TestMethod]
		public void ShouldAddReferenceToWebSiteProject()
		{
			string assemblyPath = @"c:\blah\some.dll";

			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			CreateWebsiteProject();
			
			projectNode.AddAssemblyReference(assemblyPath);

			string pathFromMock = ((VsWebSite.VSWebSite)((MockEnvDTEProject)project.ExtObject).Object).References.Item(0).FullPath;

			Assert.AreEqual<string>(assemblyPath, pathFromMock);

		}

		[TestMethod]
		public void ShouldAddProjectReferencesToWebSiteProject()
		{
			MockVSHierarchy refProjHier = new MockVSHierarchy("refedproj.proj");
			root.AddProject(refProjHier);
			Project projToRef = refProjHier.ExtObject as Project;
			Assert.IsNotNull(projToRef);

			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);
			CreateWebsiteProject();

			projectNode.AddProjectReference(refProjHier.GUID);

			Project projectFromMock = ((VsWebSite.VSWebSite)((MockEnvDTEProject)project.ExtObject).Object).References.Item(0).ReferencedProject;

			Assert.AreSame(projToRef, projectFromMock);

		}

		private void CreateWebsiteProject()
		{
			MockEnvDTEWebSite webSiteProj = new MockEnvDTEWebSite();
			((MockEnvDTEProject)project.ExtObject).Object = webSiteProj;
			webSiteProj.Project = (Project)project.ExtObject;
		}
	}
}
