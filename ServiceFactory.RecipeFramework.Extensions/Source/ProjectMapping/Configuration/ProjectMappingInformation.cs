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
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration
{
	[Serializable]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/pag/project-mapping")]
	[XmlRoot(Namespace = "http://schemas.microsoft.com/pag/project-mapping", IsNullable = false)]
	public sealed class ProjectMappingInformation
	{
		private Collection<ProjectMappingTable> projectMappingTables;

		[XmlArrayItemAttribute(IsNullable = false)]
		public Collection<ProjectMappingTable> ProjectMappingTables
		{
			get
			{
				return this.projectMappingTables;
			}
			set
			{
				this.projectMappingTables = value;
			}
		}

		private string fileName;

		[XmlAttribute("FileName")]
		public string FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;
			}
		}

		public ProjectMappingInformation()
			: this(string.Empty, new Collection<ProjectMappingTable>())
		{

		}

		public ProjectMappingInformation(string fileName)
			: this(fileName, new Collection<ProjectMappingTable>())
		{

		}

		public ProjectMappingInformation(string fileName, Collection<ProjectMappingTable> projectMappingTables)
		{
			this.projectMappingTables = projectMappingTables;
			this.fileName = fileName;
		}

		public ProjectMappingTable FindProjectMappingTable(Predicate<ProjectMappingTable> match)
		{
			Guard.ArgumentNotNull(match, "match");

			foreach(ProjectMappingTable projectMappingTable in this.projectMappingTables)
			{
				if(match(projectMappingTable))
				{
					return projectMappingTable;
				}
			}

			return null;
		}

		public ProjectMappingTable FindProjectMappingTableByName(string name)
		{
			Guard.ArgumentNotNullOrEmptyString(name, "name");

			return FindProjectMappingTable(delegate(ProjectMappingTable projectMappingTable)
			{
				return projectMappingTable.Name == name;
			});
		}
	}
}