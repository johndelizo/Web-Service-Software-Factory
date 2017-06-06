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
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Config=Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests
{
	[TestClass]
	public class ProjectMappingFixture
	{
		[TestMethod]
		public void ToStringFormatsOutputCorrectly()
		{
			Guid projectGuid = Guid.NewGuid();
			Collection<Role> roles = new Collection<Role>();
			roles.Add(new Role("foo"));
			roles.Add(new Role("bar"));

            Config.ProjectMappingEntry mapping = new Config.ProjectMappingEntry(projectGuid, "\\", roles, "FooName");

			Assert.AreEqual("{" + projectGuid.ToString() + "} -> foo,bar", mapping.ToString());
		}

		[TestMethod]
		public void FindRoleByNameReturnsRoleWhenPresent()
		{
			string roleToFind = "foo";
			Guid projectGuid = Guid.NewGuid();
			Collection<Role> roles = new Collection<Role>();
			roles.Add(new Role(roleToFind));
			roles.Add(new Role("bar"));
            Config.ProjectMappingEntry mapping = new Config.ProjectMappingEntry(projectGuid, "\\", roles, "FooName");

			Role result = mapping.FindRoleByName(roleToFind);

			Assert.IsNotNull(result, "No role returned when one should exist");
			Assert.AreEqual(roleToFind, result.Name, "Role name does not match role searched for");
		}

		[TestMethod]
		public void FindRoleByNameReturnsNullWhenRollNotPresent()
		{
			Guid projectGuid = Guid.NewGuid();
			Collection<Role> roles = new Collection<Role>();
			roles.Add(new Role("foo"));
			roles.Add(new Role("bar"));

            Config.ProjectMappingEntry mapping = new Config.ProjectMappingEntry(projectGuid, "\\", roles, "FooName");

			Role result = mapping.FindRoleByName("baz");

			Assert.IsNull(result, "No role should be returned");
		}
	}
}
