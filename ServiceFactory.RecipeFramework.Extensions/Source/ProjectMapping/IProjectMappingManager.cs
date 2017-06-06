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
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration;
using System.Collections.ObjectModel;
using EnvDTE;
using Microsoft.Practices.VisualStudio.Helper;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping
{
	public interface IProjectMappingManager
	{
		/// <summary>
		/// Gets the mapping table names.
		/// </summary>
		/// <returns></returns>
		ReadOnlyCollection<string> GetMappingTableNames();

		/// <summary>
		/// Gets the mapping table.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <returns></returns>
		ProjectMappingTable GetMappingTable(string projectMappingTableName);

		/// <summary>
		/// Gets the project roles.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		ReadOnlyCollection<Role> GetProjectRoles(string projectMappingTableName, Guid projectId);

		/// <summary>
		/// Gets the projects in roles.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="roles">The roles.</param>
		/// <returns></returns>
		ReadOnlyCollection<Guid> GetProjectsInRoles(string projectMappingTableName, IList<Role> roles);

		/// <summary>
		/// Gets the project path.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		string GetProjectPath(string projectMappingTableName, Guid projectId);

		/// <summary>
		/// Creates or gets the mapping file.
		/// </summary>
		HierarchyNode CreateMappingFile();

        /// <summary>
        /// Gets an existing mapping file or null otherwise.
        /// </summary>
        HierarchyNode GetMappingFile();

        /// <summary>
        /// Sets an existing mapping file.
        /// </summary>
        void SetMappingFile(string file);

		/// <summary>
		/// Deletes the mapping file.
		/// </summary>
		void DeleteMappingFile();

		/// <summary>
		/// Reloads the mapping file.
		/// </summary>
		void ReloadMappingFile();

		/// <summary>
		/// Gets the project mapping entry.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		ProjectMapping.Configuration.ProjectMappingEntry GetProjectMappingEntry(
			string projectMappingTableName, Guid projectId);

		/// <summary>
		/// Adds the project mapping table entry.
		/// </summary>
		/// <param name="projectMappingTable">The project mapping table.</param>
		void AddProjectMappingTableEntry(ProjectMappingTable projectMappingTable);

		/// <summary>
		/// Deletes the project mapping table entry.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		void DeleteProjectMappingTableEntry(string projectMappingTableName);

		/// <summary>
		/// Adds the project mapping entry.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectMapping">The project mapping.</param>
		void AddProjectMappingEntry(string projectMappingTableName, 
			ProjectMapping.Configuration.ProjectMappingEntry projectMapping);

		/// <summary>
		/// Deletes the project mapping entry.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectId">The project id.</param>
		void DeleteProjectMappingEntry(string projectMappingTableName, Guid projectId);

		/// <summary>
		/// Gets the project.
		/// </summary>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		Project GetProject(Guid projectId);

        /// <summary>
        /// Occurs when the mapping table file is created.
        /// </summary>
        event EventHandler<EventArgs> Created;

        /// <summary>
        /// Occurs when the mapping table file is reloaded.
        /// </summary>
        event EventHandler<EventArgs> Reloaded;

        /// <summary>
        ///  Gets/Sets the state for suspending or resuming file updates.
        /// </summary>
        bool SuspendUpdates { get; set; }
	}
}