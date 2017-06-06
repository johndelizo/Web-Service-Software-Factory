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
using System.Xml.Serialization;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Helpers;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration
{
	[Serializable]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/pag/project-mapping")]
	[XmlRoot(Namespace = "http://schemas.microsoft.com/pag/project-mapping", IsNullable = false)]
	public sealed class ProjectMappingTable
	{
		private string name;

		[XmlAttribute("Name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		private Collection<ProjectMappingEntry> projectMappings;

		[XmlArrayItemAttribute(IsNullable = false, ElementName="ProjectMapping")]
		public Collection<ProjectMappingEntry> ProjectMappings
		{
			get
			{
				return this.projectMappings;
			}
			set
			{
				this.projectMappings = value;
			}
		}

		public ProjectMappingTable()
			: this(string.Empty, new Collection<ProjectMappingEntry>())
		{

		}

		public ProjectMappingTable(string name)
			: this(name, new Collection<ProjectMappingEntry>())
		{

		}

		public ProjectMappingTable(string name, Collection<ProjectMappingEntry> projectMappings)
		{
			this.name = name;
			this.projectMappings = projectMappings;
		}

		public ProjectMappingEntry FindProjectMapping(Predicate<ProjectMappingEntry> match)
		{
			Guard.ArgumentNotNull(match, "match");

			foreach(ProjectMappingEntry projectMapping in this.projectMappings)
			{
				if (match(projectMapping))
				{
					return projectMapping;
				}
			}

			return null;
		}

		public ProjectMappingEntry FindProjectMappingByProjectId(Guid projectId)
		{
			GuidGuard.GuidNotEmpty(projectId, "projectId");

			return FindProjectMapping(delegate(ProjectMappingEntry projectMapping)
			{
				Guid mappingProjectId = new Guid(projectMapping.ProjectId);
				return mappingProjectId.Equals(projectId);
			});
		}

		/// <summary>
		/// String representation of the project mapping table.
		/// </summary>
		/// <remarks>
		/// Added to make debugging output more readable.
		/// </remarks>
		/// <returns></returns>
		public new string ToString()
		{
			return this.name;
		}
	}
}