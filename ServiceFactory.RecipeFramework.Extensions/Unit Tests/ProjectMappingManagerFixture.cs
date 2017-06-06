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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration;
using System.Collections.ObjectModel;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.Practices.UnitTestLibrary;
using System.IO;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests
{
	[TestClass]
	public class ProjectMappingManagerFixture
	{
		private const string WcfMappingTableName = "WCF";
		private Guid ProjectId1 = new Guid("4A216B22-B2B2-4851-AFFA-B7A5AF147645");
        IProjectMappingManager manager;

        [TestInitialize]
        public void Initialize()
        {
            manager = ProjectMappingManagerSetup.CreateManager("ProjectMapping.RecipeFramework.Extensions.Tests.xml");
        }

        [TestCleanup]
        public void Cleanup()
        {
            manager.DeleteMappingFile();
        }

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectsInRolesWithFirstArgumentNull()
		{
            manager.GetProjectsInRoles(null, new List<Role>());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectsInRolesWithSecondArgumentNull()
		{
            manager.GetProjectsInRoles("Foo", null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectsInRolesWithBothArgumentNull()
		{
            manager.GetProjectsInRoles(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableNotFoundException))]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldNotReturnProjectInRolesIfMappingTableDoesntExist()
		{
            manager.GetProjectsInRoles("Foo", new List<Role>());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectRolesWithFirstArgumentNull()
		{
            manager.GetProjectRoles(null, Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectRolesWithSecondArgumentNull()
		{
            manager.GetProjectRoles("Foo", Guid.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectRolesWithBothArgumentNull()
		{
            manager.GetProjectRoles(null, Guid.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableNotFoundException))]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldNotReturnRolesIfMappingTableDoesntExist()
		{
            manager.GetProjectRoles("Foo", Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingNotFoundException))]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldNotReturnRolesIfProjectDoesntExist()
		{
            manager.GetProjectRoles(WcfMappingTableName, Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectPathWithFirstArgumentNull()
		{
			manager.GetProjectPath(null, Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectPathWithSecondArgumentNull()
		{			
			manager.GetProjectPath("Foo", Guid.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectPathWithBothArgumentNull()
		{			
			manager.GetProjectPath(null, Guid.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableNotFoundException))]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldNotReturnProjectPathIfMappingTableDoesntExist()
		{			
			manager.GetProjectPath("Foo", Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingNotFoundException))]
		public void ShouldNotReturnProjectPathIfProjectDoesntExist()
		{			
			manager.GetProjectPath(WcfMappingTableName, Guid.NewGuid());
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldReturnProjectInRolesOneProject()
		{
			List<Role> roles = new List<Role>();
			roles.Add(new Role("MessageContractRole"));

			ReadOnlyCollection<Guid> projectIds =
				manager.GetProjectsInRoles(WcfMappingTableName, roles);

			Assert.IsNotNull(projectIds, "Null");
			Assert.AreEqual(projectIds.Count, 1, "Not Equal");
			Assert.AreEqual(projectIds[0], new Guid("BC9E7634-206C-43f4-81F3-5CA0D6DDBA99"), "Not Equal");
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldReturnProjectInRolesMultipleProjects()
		{
			List<Role> roles = new List<Role>();
			roles.Add(new Role("DataContractRole"));
			roles.Add(new Role("ServiceContractRole"));

			ReadOnlyCollection<Guid> projectIds =
				manager.GetProjectsInRoles(WcfMappingTableName, roles);

			Assert.IsNotNull(projectIds, "Null");
			Assert.AreEqual(projectIds.Count, 2, "Not Equal");
			Assert.AreEqual(projectIds[0], new Guid("4A216B22-B2B2-4851-AFFA-B7A5AF147645"), "Not Equal");
			Assert.AreEqual(projectIds[1], new Guid("DE91F8D4-0BB1-4768-ACF3-204ABB481AFD"), "Not Equal");
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldReturnRoles()
		{
			ReadOnlyCollection<Role> roles =
				manager.GetProjectRoles(WcfMappingTableName, new Guid("4A216B22-B2B2-4851-AFFA-B7A5AF147645"));

			Assert.IsNotNull(roles, "Null");
			Assert.AreEqual(roles.Count, 2, "Not Equal");
			Assert.AreEqual(roles[0].Name, "DataContractRole", "Not Equal");
			Assert.AreEqual(roles[1].Name, "ServiceContractRole", "Not Equal");
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldReturnProjectPath()
		{
			string projectPath = manager.GetProjectPath(WcfMappingTableName, new Guid("4A216B22-B2B2-4851-AFFA-B7A5AF147645"));

			Assert.AreEqual(projectPath, @"\", "Not Equal");
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldReturnProjectMappingTableNames()
		{
			ReadOnlyCollection<string> mappingTableNames =
				manager.GetMappingTableNames();

			Assert.IsNotNull(mappingTableNames, "Null");
			Assert.AreEqual(mappingTableNames.Count, 2, "Not Equal");
			Assert.AreEqual(mappingTableNames[0], WcfMappingTableName, "Not Equal");
			Assert.AreEqual(mappingTableNames[1], "ASMX", "Not Equal");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotAddAProjectMappingEntryWithNullFirstParameter()
		{			
			manager.AddProjectMappingEntry(null, new ProjectMapping.Configuration.ProjectMappingEntry());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotAddAProjectMappingEntryWithNullSecondParameter()
		{			
			manager.AddProjectMappingEntry("Foo", null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotAddAProjectMappingEntryWithWithBothParameterNull()
		{			
			manager.AddProjectMappingEntry(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableNotFoundException))]
		public void ShouldNotAddAProjectMappingEntryIfMappingTableThatDoesntExist()
		{			
			manager.AddProjectMappingEntry("Foo", new ProjectMapping.Configuration.ProjectMappingEntry());
		}
		
		[TestMethod]
		[ExpectedException(typeof(ProjectMappingAlreadyExistsException))]
		public void ShouldNotAddAProjectMappingEntryThatAlreadyExists()
		{
			Guid projectGuid = new Guid("4A216B22-B2B2-4851-AFFA-B7A5AF147645");

			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				new ProjectMapping.Configuration.ProjectMappingEntry(
					projectGuid,
					@"\Foo", "FooName");

			projectMapping.Roles.Add(new Role("FooRole"));
			
			manager.AddProjectMappingEntry(WcfMappingTableName, projectMapping);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotDeleteAProjectMappingEntryWithNullFirstParameter()
		{			
			manager.DeleteProjectMappingEntry(null, Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotDeleteAProjectMappingEntryWithNullSecondParameter()
		{			
			manager.DeleteProjectMappingEntry("Foo", Guid.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotDeleteAProjectMappingEntryWithBothParameterNull()
		{			
			manager.DeleteProjectMappingEntry(null, Guid.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableNotFoundException))]
		public void ShouldNotDeleteAProjectMappingEntryIfMappingTableThatDoesntExist()
		{
			manager.DeleteProjectMappingEntry("Foo", Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingNotFoundException))]
		public void ShouldNotDeleteAProjectMappingEntryIfProjectThatDoesntExist()
		{			
			manager.DeleteProjectMappingEntry(WcfMappingTableName, Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotDeleteAProjectMappingTableEntryWithParameterNull()
		{			
			manager.DeleteProjectMappingTableEntry(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableNotFoundException))]
		public void ShouldNotDeleteAProjectMappingTableEntryIfMappingTableThatDoesntExist()
		{			
			manager.DeleteProjectMappingTableEntry("Foo");
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableAlreadyExistsException))]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldNotAddAProjectMappingTableEntryThatAlreadyExists()
		{
			ProjectMappingTable projectMappingTable = new ProjectMappingTable(WcfMappingTableName);

			manager.AddProjectMappingTableEntry(projectMappingTable);
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableAlreadyExistsException))]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldAddAProjectMappingTableEntry()
		{
			ProjectMappingTable projectMappingTable = new ProjectMappingTable("FOOEntry");

			manager.AddProjectMappingTableEntry(projectMappingTable);
			manager.AddProjectMappingTableEntry(projectMappingTable);
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		[ExpectedException(typeof(ProjectMappingTableNotFoundException))]
		public void ShouldDeleteAProjectMappingTableEntry()
		{
			ProjectMappingTable projectMappingTable = new ProjectMappingTable("FOOEntry1");
			manager.AddProjectMappingTableEntry(projectMappingTable);

			manager.DeleteProjectMappingTableEntry("FOOEntry1");
			manager.DeleteProjectMappingTableEntry("FOOEntry1");
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldAddAProjectMappingEntry()
		{
			Guid projectGuid = new Guid("0D0429A4-2E7B-413b-92DB-2F5E048667C8");

			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				new ProjectMapping.Configuration.ProjectMappingEntry(
					projectGuid,
                    @"\Foo", "FooName");

			projectMapping.Roles.Add(new Role("FooRole"));

			manager.AddProjectMappingEntry(WcfMappingTableName, projectMapping);

			string projectPath = manager.GetProjectPath(WcfMappingTableName, projectGuid);
			ReadOnlyCollection<Role> roles = manager.GetProjectRoles(WcfMappingTableName, projectGuid);

			Assert.AreEqual(roles.Count, 1, "Not Equal");
			Assert.AreEqual(roles[0].Name, "FooRole", "Not Equal");
			Assert.AreEqual(projectPath, @"\Foo", "Not Equal");
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldDeleteAProjectMappingEntry()
		{
			Guid projectGuid = new Guid("A168E8C3-8CCD-47cd-AE2B-BE0F85F66782");

			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				new ProjectMapping.Configuration.ProjectMappingEntry(
					projectGuid,
                    @"\Foo", "FooName");

			ProjectMappingTable projectMappingTable = new ProjectMappingTable("FooEntry2");
			manager.AddProjectMappingTableEntry(projectMappingTable);

			manager.AddProjectMappingEntry("FooEntry2", projectMapping);

			manager.DeleteProjectMappingEntry("FooEntry2", projectGuid);

			Assert.IsNull(manager.GetProjectMappingEntry("FooEntry2", projectGuid));
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingNotFoundException))]
		public void ShouldNotDeleteAProjectMappingEntryTwice()
		{
			Guid projectGuid = new Guid("A168E8C3-8CCD-47cd-AE2B-BE0F85F66782");

			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				new ProjectMapping.Configuration.ProjectMappingEntry(
					projectGuid,
                    @"\Foo", "FooName");

			ProjectMappingTable projectMappingTable = new ProjectMappingTable("FooEntry2");
			manager.AddProjectMappingTableEntry(projectMappingTable);

			manager.AddProjectMappingEntry("FooEntry2", projectMapping);

			manager.DeleteProjectMappingEntry("FooEntry2", projectGuid);
			manager.DeleteProjectMappingEntry("FooEntry2", projectGuid);
		}

		[TestMethod]
		public void ShouldReadSampleFileCorrectly()
		{
            Assert.AreEqual(2, manager.GetMappingTableNames().Count);

            ProjectMappingTable table = manager.GetMappingTable("WCF");
			Assert.AreEqual(4, table.ProjectMappings.Count);

			ProjectMappingEntry entry = table.ProjectMappings[0];
			Assert.AreEqual("4A216B22-B2B2-4851-AFFA-B7A5AF147645", entry.ProjectId);
			Assert.AreEqual(@"\", entry.ProjectPath);
			Assert.AreEqual(2, entry.Roles.Count);
			Assert.AreEqual("DataContractRole", entry.Roles[0].Name);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetMappingTableWithArgumentNull()
		{
            manager.GetMappingTable(null);
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldReturnNullIfMappingTableDoesntExist()
		{
 			Assert.IsNull(manager.GetMappingTable("Foo"));
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldReturnMappingTableIfExist()
		{
 			ProjectMappingTable table = manager.GetMappingTable(WcfMappingTableName);

			Assert.IsNotNull(table);
			Assert.AreEqual<string>(WcfMappingTableName, table.Name);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectMappingEntryWithFirstArgumentNull()
		{
 			manager.GetProjectMappingEntry(null, Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectMappingEntryWithSecondArgumentEmpty()
		{
			manager.GetProjectMappingEntry("Foo", Guid.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetProjectMappingEntryWithBothArgumentNull()
		{
			manager.GetProjectMappingEntry(null, Guid.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ProjectMappingTableNotFoundException))]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldNotReturnProjectMappingIfMappingTableDoesntExist()
		{
			manager.GetProjectMappingEntry("Foo", Guid.NewGuid());
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.RecipeFramework.Extensions.Tests.xml")]
		public void ShouldReturnProjectMappingIfMappingTableExist()
		{			
			ProjectMapping.Configuration.ProjectMappingEntry mapping = manager.GetProjectMappingEntry(WcfMappingTableName, ProjectId1);

			Assert.IsNotNull(mapping);
			Assert.AreEqual<Guid>(ProjectId1, new Guid(mapping.ProjectId));
		}
	}
}