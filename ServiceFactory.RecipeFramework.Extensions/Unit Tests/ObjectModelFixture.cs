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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration;
using Microsoft.Practices.Modeling.Serialization;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests
{
	[TestClass]
	public class ObjectModelFixture
	{
		ProjectMappingInformation info = null;
		Guid projectId;

		[TestInitialize]
		public void Setup()
		{
			projectId = Guid.NewGuid();
			Role role = new Role("FooRole");
			Role role1 = new Role("FooRole1");
			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				new ProjectMapping.Configuration.ProjectMappingEntry(
					projectId,
                    @"\", "FooName");
			projectMapping.Roles.Add(role);
			projectMapping.Roles.Add(role1);

			ProjectMappingTable mappingTable = new ProjectMappingTable("Foo");
			mappingTable.ProjectMappings.Add(projectMapping);

			info = new ProjectMappingInformation("Foo.rolemapping");
			info.ProjectMappingTables.Add(mappingTable);
		}

		[TestMethod]
		public void TestProjectMappingInformationSerialization()
		{
			string stringResentation =
				GenericSerializer.Serialize<ProjectMappingInformation>(info);

			ProjectMappingInformation deserializedInfo =
				GenericSerializer.Deserialize<ProjectMappingInformation>(stringResentation);

			Assert.AreEqual(info.FileName, deserializedInfo.FileName, "Not Equal");
			Assert.AreEqual(info.ProjectMappingTables.Count, deserializedInfo.ProjectMappingTables.Count, "Not Equal");
			Assert.AreEqual(info.ProjectMappingTables[0].Name, deserializedInfo.ProjectMappingTables[0].Name, "Not Equal");
			Assert.AreEqual(info.ProjectMappingTables[0].ProjectMappings.Count, deserializedInfo.ProjectMappingTables[0].ProjectMappings.Count, "Not Equal");
			Assert.AreEqual(info.ProjectMappingTables[0].ProjectMappings[0].ProjectId, deserializedInfo.ProjectMappingTables[0].ProjectMappings[0].ProjectId, "Not Equal");
			Assert.AreEqual(info.ProjectMappingTables[0].ProjectMappings[0].ProjectPath, deserializedInfo.ProjectMappingTables[0].ProjectMappings[0].ProjectPath, "Not Equal");
			Assert.AreEqual(info.ProjectMappingTables[0].ProjectMappings[0].Roles.Count, deserializedInfo.ProjectMappingTables[0].ProjectMappings[0].Roles.Count, "Not Equal");
			Assert.AreEqual(info.ProjectMappingTables[0].ProjectMappings[0].Roles[0].Name, deserializedInfo.ProjectMappingTables[0].ProjectMappings[0].Roles[0].Name, "Not Equal");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FindMappingTableWithNullArgument()
		{
			ProjectMappingTable projectMappingTable = info.FindProjectMappingTableByName(null);
		}

		[TestMethod]
		public void FindMappingTableShouldReturnNull()
		{
			ProjectMappingTable projectMappingTable = info.FindProjectMappingTableByName("DoesntExist");

			Assert.IsNull(projectMappingTable, "Not null");
		}

		[TestMethod]
		public void FindMappingTableShouldReturnValue()
		{
			ProjectMappingTable projectMappingTable = info.FindProjectMappingTableByName("Foo");

			Assert.IsNotNull(projectMappingTable, "Null");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FindProjectMappingWithNullArgument()
		{
			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				info.ProjectMappingTables[0].FindProjectMappingByProjectId(Guid.Empty);
		}

		[TestMethod]
		public void FindProjectMappingShouldReturnNull()
		{
			ProjectMapping.Configuration.ProjectMappingEntry projectMapping = 
				info.ProjectMappingTables[0].FindProjectMappingByProjectId(Guid.NewGuid());

			Assert.IsNull(projectMapping, "Not null");
		}

		[TestMethod]
		public void FindProjectMappingShouldReturnValue()
		{
			ProjectMapping.Configuration.ProjectMappingEntry projectMapping = 
				info.ProjectMappingTables[0].FindProjectMappingByProjectId(projectId);

			Assert.IsNotNull(projectMapping, "Null");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FindRoleWithNullArgument()
		{
			Role role =
				info.ProjectMappingTables[0].ProjectMappings[0].FindRoleByName(null);
		}

		[TestMethod]
		public void FindRoleReturnNull()
		{
			Role role =
				info.ProjectMappingTables[0].ProjectMappings[0].FindRoleByName("Dummy");

			Assert.IsNull(role, "Not null");
		}

		[TestMethod]
		public void FindRoleShouldReturnValue()
		{
			Role role =
				info.ProjectMappingTables[0].ProjectMappings[0].FindRoleByName("FooRole");

			Assert.IsNotNull(role, "Null");
		}
	}
}