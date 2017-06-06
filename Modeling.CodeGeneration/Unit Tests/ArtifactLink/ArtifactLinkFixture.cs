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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Microsoft.Practices.UnitTestLibrary;

namespace Microsoft.Practices.Modeling.CodeGeneration.Artifacts.Tests
{
	/// <summary>
	/// Summary description for ArtifactLinkFixture
	/// </summary>
	[TestClass]
	public class ArtifactLinkFixture
	{
		public ArtifactLinkFixture()
		{
		}

		[TestMethod]
		public void ShouldReturnUnmappedRoleIfNoContainer()
		{
			TestableArtifactLink artifactLink = new TestableArtifactLink();

			Assert.AreEqual<string>("There is no role associated with this artifact link.", artifactLink.ToString());
		}

		[TestMethod]
		public void ShouldReturnContainerGuidAndItemNameFromToString()
		{
			TestableArtifactLink artifactLink = new TestableArtifactLink();
			Guid containerGuid = Guid.NewGuid();
			artifactLink.Container = containerGuid;
			artifactLink.ItemName = "TestName";

			StringAssert.Contains(artifactLink.ToString(), containerGuid.ToString("b"));
			StringAssert.Contains(artifactLink.ToString(), artifactLink.ItemName);
		}

		[TestMethod]
		public void ShouldReturnProjectExtension()
		{
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			MockVSHierarchy project = new MockVSHierarchy("Project1.project");
			root.AddProject(project);

			Assert.IsInstanceOfType(project.ExtObject, typeof(EnvDTE.Project));

			TestableArtifactLink artifactLink = new TestableArtifactLink();

			artifactLink.Container = project.GUID;
			artifactLink.ItemName = "TestItem";
			artifactLink.Project = project.ExtObject as EnvDTE.Project;

			Assert.AreEqual<string>(".cs", artifactLink.DefaultExtension);
		}

		[TestMethod]
		public void ShouldEmptyDataDictionary()
		{
			TestableArtifactLink artifactLink = new TestableArtifactLink();

			Assert.IsNotNull(artifactLink.Data);
			Assert.AreEqual<int>(0, artifactLink.Data.Count);
		}

		class TestableArtifactLink : ArtifactLink
		{
			
		}
	}
}
