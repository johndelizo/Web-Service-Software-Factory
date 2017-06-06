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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.VisualStudio.Helper.Design;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.Practices.UnitTestLibrary;

namespace Microsoft.Practices.VisualStudio.Helper.Tests
{
	[TestClass]
	public class OnlyProjectsFilterFixture
	{
		[TestMethod]
		public void ShouldFilterOutChildren()
		{
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			string childName = "Child1.txt";
			root.AddChild(childName);
			string projectName = "Project1.project";
			MockVSHierarchy project = new MockVSHierarchy(projectName);
			root.AddProject(project);

			HierarchyNode slnNode = new HierarchyNode(solution);
			HierarchyNode projectNode = new HierarchyNode(solution, project.GUID);
			HierarchyNode childNode = slnNode.FindByName(childName);
			
			OnlyProjectsFilter target = new OnlyProjectsFilter();
			Assert.IsFalse(target.Filter(slnNode));
			Assert.IsTrue(target.Filter(childNode));
			Assert.IsFalse(target.Filter(projectNode));
		}
	}


}
