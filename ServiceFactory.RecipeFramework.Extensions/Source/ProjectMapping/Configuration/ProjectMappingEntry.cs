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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Serialization;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration
{
	[Serializable]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/pag/project-mapping")]
	[XmlRoot(Namespace = "http://schemas.microsoft.com/pag/project-mapping", 
		ElementName="ProjectMapping", IsNullable = false)]
	public sealed class ProjectMappingEntry
	{
		private Collection<Role> roles;

		[XmlArrayItemAttribute(IsNullable = false)]
		public Collection<Role> Roles
		{
			get
			{
				return this.roles;
			}
			set
			{
				this.roles = value;
			}
		}

		private string projectId;

		[XmlAttribute("ProjectId")]
		public string ProjectId
		{
			get
			{
				return this.projectId;
			}
			set
			{
				this.projectId = value;
			}
		}

		private string projectPath;

		[XmlAttribute("ProjectPath")]
		public string ProjectPath
		{
			get
			{
				return this.projectPath;
			}
			set
			{
				this.projectPath = value;
			}
		}

        private string projectName;

        [XmlAttribute("ProjectName")]
        public string ProjectName
        {
            get
            {
                return this.projectName;
            }
            set
            {
                this.projectName = value;
            }
        }

		public ProjectMappingEntry()
			: this(Guid.Empty, string.Empty, new Collection<Role>(), string.Empty)
		{

		}

		public ProjectMappingEntry(Guid projectId, string projectPath, string projectName)
			: this(projectId, projectPath, new Collection<Role>(), projectName)
		{

		}

		public ProjectMappingEntry(Guid projectId, string projectPath, Collection<Role> roles, string projectName)
		{
			this.roles = roles;
            this.projectName = projectName;
			this.projectId = projectId.ToString("D", CultureInfo.InvariantCulture);

			if(string.IsNullOrEmpty(projectPath))
			{
				this.projectPath = @"\";
			}
			else
			{
				this.projectPath = projectPath;
			}
		}

		public Role FindRole(Predicate<Role> match)
		{
			Guard.ArgumentNotNull(match, "match");

			foreach(Role role in this.roles)
			{
				if(match(role))
				{
					return role;
				}
			}

			return null;
		}

		public Role FindRoleByName(string name)
		{
			Guard.ArgumentNotNullOrEmptyString(name, "name");

			return FindRole(delegate(Role role)
			{
				return role.Name == name;
			});
		}

		/// <summary>
		/// String representation of the project mapping.
		/// </summary>
		/// <remarks>
		/// Added to make debugging output more readable.
		/// </remarks>
		/// <returns></returns>
		public new string ToString()
		{
			Role[] roleArray = new Role[Roles.Count];

			Roles.CopyTo(roleArray, 0);
			string[] roleNames = Array.ConvertAll(roleArray, new Converter<Role, string>(Role.RoleToString));
			return String.Format(CultureInfo.CurrentUICulture, "{{{0}}} -> {1}", this.projectId, String.Join(",", roleNames));
		}
	}
}