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
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.Practices.VisualStudio.Helper;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace Microsoft.Practices.VisualStudio.Helper.Tests
{
	/// <summary>
	/// Summary description for HierarchyNodeFixture
	/// </summary>
	[TestClass]
	public class HierarchyNodeFixture
	{
		[TestMethod]
		public void EnumerationWalking()
		{
			int children = 3;
			MockVSHierarchy hierarchy = new MockVSHierarchy(children);
			MockVsSolution solution = new MockVsSolution(hierarchy);
			HierarchyNode node = new HierarchyNode(solution);
			int i = 0;
			foreach (HierarchyNode child in node.Children)
			{
				++i;
			}
			Assert.AreEqual<int>(children, i, "Invalid number of children");
		}

		[TestMethod]
		public void NamePropertySetCorrectly()
		{
			string name = "MyName";
			MockVSHierarchy hierarchy = new MockVSHierarchy(name, Guid.NewGuid());
			MockVsSolution solution = new MockVsSolution(hierarchy);
			HierarchyNode node = new HierarchyNode(solution);
			Assert.AreEqual<string>(name, node.Name);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ConstructorThrowsIfProjectIsInvalid()
		{
			new HierarchyNode(new MockVsSolution(new MockVSHierarchy(0)), Guid.NewGuid());
		}

		[TestMethod]
		public void ForEachWalksAllChildren()
		{
			int children = 3;
			MockVSHierarchy hierarchy = new MockVSHierarchy(children);
			MockVsSolution solution = new MockVsSolution(hierarchy);
			HierarchyNode node = new HierarchyNode(solution);
			int i = 0;
			
			node.ForEach(delegate(HierarchyNode child) { i++; });

			Assert.AreEqual(children, i, "Incorrect number of nodes walked");
		}

		[TestMethod]
		public void RecursiveForEachWalksAllChildrenAndParent()
		{
			int children = 3;
			MockVSHierarchy hierarchy = new MockVSHierarchy(children);
			MockVsSolution solution = new MockVsSolution(hierarchy);
			HierarchyNode node = new HierarchyNode(solution);

			int i = 0;

			node.RecursiveForEach(delegate(HierarchyNode child)
			{
				Trace.WriteLine(child.Name);
				i++;
			});

			Assert.AreEqual((children + 1), i, "Incorrect number of nodes walked");
		}

		[TestMethod]
		public void FindByName()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			string child1 = "Child1";
			string child2 = "Child2";
			string child3 = "Child3";
			string child4 = "Child4";
			hierarchy.AddChild(child1);
			hierarchy.AddChild(child2);
			hierarchy.AddChild(child3);
			HierarchyNode node = new HierarchyNode(solution);
			Assert.IsNotNull(node.FindByName(child1));
			Assert.IsNotNull(node.FindByName(child2));
			Assert.IsNotNull(node.FindByName(child3));
			Assert.IsNull(node.FindByName(child4));
		}

		[TestMethod]
		public void RecursiveFindByName()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			MockVSHierarchy project1 = new MockVSHierarchy("Project1.project");
			hierarchy.AddProject(project1);
			string child1 = "Child1";
			project1.AddChild(child1);
			MockVSHierarchy project2 = new MockVSHierarchy("Project2.project");
			hierarchy.AddProject(project2);
			string child2 = "Child2";
			project2.AddChild(child2);
			string child3 = "Child3";
			project2.AddChild(child3);
			string child4 = "Child4";

			HierarchyNode node = new HierarchyNode(solution);
			Assert.IsNull(node.FindByName(child1));
			Assert.IsNull(node.FindByName(child2));
			Assert.IsNull(node.FindByName(child3));
			Assert.IsNull(node.FindByName(child4));
			Assert.IsNotNull(node.RecursiveFindByName(child1));
			Assert.IsNotNull(node.RecursiveFindByName(child2));
			Assert.IsNotNull(node.RecursiveFindByName(child3));
			Assert.IsNull(node.RecursiveFindByName(child4));
		}

		[TestMethod]
		public void RecursiveFindByNameIgnoreCase()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			MockVSHierarchy project1 = new MockVSHierarchy("Project1.project");
			hierarchy.AddProject(project1);
			string child1 = "Child1";
			project1.AddChild(child1);
			MockVSHierarchy project2 = new MockVSHierarchy("Project2.project");
			hierarchy.AddProject(project2);
			string child2 = "ChIlD2.cd";
			project2.AddChild(child2);
			string child3 = "ChildThree3";
			project2.AddChild(child3);
			string child4 = "Child4NotAdded";

			HierarchyNode node = new HierarchyNode(solution);
			Assert.IsNull(node.FindByName(child1));
			Assert.IsNull(node.FindByName(child2));
			Assert.IsNull(node.FindByName(child3));
			Assert.IsNull(node.FindByName(child4));
			Assert.IsNotNull(node.RecursiveFindByName(child1.ToLowerInvariant()));
			Assert.IsNotNull(node.RecursiveFindByName(child2.ToUpperInvariant()));
			Assert.IsNotNull(node.RecursiveFindByName(CodeIdentifier.MakeCamel(child3)));
			Assert.IsNull(node.RecursiveFindByName(child4));
		}

		[TestMethod]
		public void FindOrCreateSolutionFolder()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			HierarchyNode node = new HierarchyNode(solution);
			string folderName = "SlnItems";
			HierarchyNode folder = node.FindOrCreateSolutionFolder(folderName);
			Assert.IsNotNull(folder);
			Assert.AreEqual<string>(folderName, folder.Name);
		}

		[TestMethod]
		public void TestRelativePath()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			MockVSHierarchy folder1 = new MockVSHierarchy("folder1");
			hierarchy.AddProject(folder1);
			string child1 = "subFolder1";
			folder1.AddChild(child1);
			HierarchyNode rootNode = new HierarchyNode(solution);
			HierarchyNode folder1Node = rootNode.FindByName("folder1");
			HierarchyNode child1Node = folder1Node.FindByName(child1);
			Assert.IsNotNull(child1Node.RelativePath);
			Assert.AreEqual<string>(Path.Combine(Directory.GetCurrentDirectory(), child1), child1Node.RelativePath);
		}

		[TestMethod]
		public void RemoveItem()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			MockVSHierarchy project = new MockVSHierarchy("Project3.project");
			hierarchy.AddProject(project);
			ProjectNode projectNode = new ProjectNode(solution, project.GUID);
			string itemName = "item1";
			HierarchyNode node = projectNode.AddItem(itemName);
			Assert.IsNotNull(projectNode.FindByName(itemName));
			node.Remove();
			Assert.IsNull(projectNode.FindByName(itemName));
		}

		[TestMethod]
		public void TestFileHasIcon()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			string docName = "Doc1.doc";
			hierarchy.AddChild(docName);
			HierarchyNode slnNode = new HierarchyNode(solution);
			HierarchyNode node = slnNode.FindByName(docName);
			Assert.IsNotNull(node.Icon);
		}

		[TestMethod]
		public void TestHasChildrenChanges()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			HierarchyNode slnNode = new HierarchyNode(solution);
			Assert.IsFalse(slnNode.HasChildren);
			string docName = "Doc1.doc";
			hierarchy.AddChild(docName);
			Assert.IsTrue(slnNode.HasChildren);
		}

		[TestMethod]
		public void TestHasProperty()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			HierarchyNode slnNode = new HierarchyNode(solution);
			Assert.IsFalse(slnNode.HasIconIndex);
		}

		[TestMethod]
		public void TestGetObject()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			HierarchyNode slnNode = new HierarchyNode(solution);
			Assert.AreSame(hierarchy, slnNode.GetObject<MockVSHierarchy>());
		}

		[TestMethod]
		public void ShouldReturnTypeGuidForSolutionFile()
		{
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			string childName = "Child1.txt";
			root.AddChild(childName);

			HierarchyNode slnNode = new HierarchyNode(solution);
			HierarchyNode childNode = slnNode.FindByName(childName);

			Assert.AreEqual<Guid>(VSConstants.GUID_ItemType_PhysicalFile, childNode.TypeGuid);
		}

		[TestMethod]
		public void ShouldReturnTypeGuidForSolutionFolder()
		{
			MockVSHierarchy hierarchy = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(hierarchy);
			MockVSHierarchy project = new MockVSHierarchy("Project4.project");
			project.TypeGuid = VSConstants.GUID_ItemType_VirtualFolder;
			hierarchy.AddProject(project);

			HierarchyNode slnNode = new HierarchyNode(solution);
			HierarchyNode prjNode = slnNode.FindByName("Project4.project");

			Assert.AreEqual<Guid>(VSConstants.GUID_ItemType_VirtualFolder, prjNode.TypeGuid);
		}
	}
}
