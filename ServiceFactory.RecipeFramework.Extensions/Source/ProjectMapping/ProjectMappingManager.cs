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
using System.IO;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Helpers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Practices.VisualStudio.Helper;
using Helpers = Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Helpers;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Globalization;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.Modeling.Serialization;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping
{
	public class ProjectMappingManager : IProjectMappingManager
    {
        #region Events

        /// <summary>
        /// Occurs when the mapping table file is created.
        /// </summary>
        public event EventHandler<EventArgs> Created;

        /// <summary>
        /// Occurs when the mapping table file is reloaded.
        /// </summary>
        public event EventHandler<EventArgs> Reloaded;

        #endregion

        #region Fields
        
        // thread safe singleton pattern (along with static constructor)
        private static readonly ProjectMappingManager instance = new ProjectMappingManager();

		private ProjectMappingInformation projectMappingInformation = null;
		private FileInfo mappingFileInfo = null;
		private IVsSolution vsSolution = null;
        private bool suspendedUpdates = false;
        private IServiceProvider serviceProvider;

        private IServiceProvider ServiceProvider
        {
            get { return serviceProvider ?? RuntimeHelper.ServiceProvider; } 
        }

        #endregion

        #region Constructors

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ProjectMappingManager()
        {
        }

        /// <summary>
        /// Default initializer.
        /// </summary>
        public ProjectMappingManager()
        {
            // Init with empty delagate to avoid checking for null
            Created += delegate { };
            Reloaded += delegate { };
        }

               /// <summary>
        /// Default initializer.
        /// </summary>
        public ProjectMappingManager(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion

		#region Singleton Implementation

		/// <summary>
		/// Gets the singleton for <see cref="ProjectMappingManager"/>.
		/// </summary>
		/// <value>The instance.</value>
		public static IProjectMappingManager Instance
		{
			get { return instance; }
		}

		#endregion

		#region IProjectMapper Members

		/// <summary>
		/// Gets the mapping table names.
		/// </summary>
		/// <returns></returns>
		public ReadOnlyCollection<string> GetMappingTableNames()
		{
			List<string> mappingTableNames = new List<string>();

			foreach (ProjectMappingTable projectMappingTable in this.ProjectMappingInformation.ProjectMappingTables)
			{
				mappingTableNames.Add(projectMappingTable.Name);
			}

			return new ReadOnlyCollection<string>(mappingTableNames);
		}

		/// <summary>
		/// Gets the mapping table.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <returns><see cref="ProjectMappingTable"/> found, otherwise null.</returns>
		public ProjectMappingTable GetMappingTable(string projectMappingTableName)
		{
			Guard.ArgumentNotNullOrEmptyString(projectMappingTableName, "projectMappingTableName");

			return this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTableName);
		}

		/// <summary>
		/// Gets the project roles.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		public ReadOnlyCollection<Role> GetProjectRoles(string projectMappingTableName, Guid projectId)
		{
			Guard.ArgumentNotNullOrEmptyString(projectMappingTableName, "projectMappingTableName");
			GuidGuard.GuidNotEmpty(projectId, "projectId");

			ProjectMappingTable projectMappingTable =
				this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTableName);

			if (projectMappingTable == null)
			{
				throw new ProjectMappingTableNotFoundException(Properties.Resources.ProjectMappingTableNotFound);
			}

			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				projectMappingTable.FindProjectMappingByProjectId(projectId);

			if (projectMapping == null)
			{
				throw new ProjectMappingNotFoundException(Properties.Resources.ProjectNotFound);
			}

			return new ReadOnlyCollection<Role>(projectMapping.Roles);
		}

		/// <summary>
		/// Gets the projects in roles.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="roles">The roles.</param>
		/// <returns></returns>
		public ReadOnlyCollection<Guid> GetProjectsInRoles(string projectMappingTableName, IList<Role> roles)
		{
			Guard.ArgumentNotNullOrEmptyString(projectMappingTableName, "projectMappingTableName");
			Guard.ArgumentNotNull(roles, "roles");

			List<Guid> projectIds = new List<Guid>();

			ProjectMappingTable projectMappingTable =
				this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTableName);

			if (projectMappingTable == null)
			{
				throw new ProjectMappingTableNotFoundException(Properties.Resources.ProjectMappingTableNotFound);
			}

			foreach (ProjectMapping.Configuration.ProjectMappingEntry projectMapping in projectMappingTable.ProjectMappings)
			{
				foreach (Role role in roles)
				{
					if (projectMapping.FindRoleByName(role.Name) != null)
					{
						projectIds.Add(new Guid(projectMapping.ProjectId));
						break;
					}
				}
			}

			return new ReadOnlyCollection<Guid>(projectIds);
		}

		/// <summary>
		/// Gets the project path.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		public string GetProjectPath(string projectMappingTableName, Guid projectId)
		{
			Guard.ArgumentNotNullOrEmptyString(projectMappingTableName, "projectMappingTableName");
			GuidGuard.GuidNotEmpty(projectId, "projectId");

			ProjectMappingTable projectMappingTable =
				this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTableName);

			if (projectMappingTable == null)
			{
				throw new ProjectMappingTableNotFoundException(Properties.Resources.ProjectMappingTableNotFound);
			}

			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				projectMappingTable.FindProjectMappingByProjectId(projectId);

			if (projectMapping == null)
			{
				throw new ProjectMappingNotFoundException(Properties.Resources.ProjectNotFound);
			}

			return projectMapping.ProjectPath;
		}

		/// <summary>
		/// Gets or create an empty mapping file.
		/// </summary>
		public HierarchyNode CreateMappingFile()
		{
			HierarchyNode item = InternalGetOrCreateMappingFile();
			if (item == null)
			{
				string pmtFilePath = GetMappingFileName();
				if (!File.Exists(pmtFilePath))
				{
					File.WriteAllText(pmtFilePath, Helpers.Constants.MappingFileContent);
				}
                item = AddFile(pmtFilePath, Helpers.Constants.SolutionItems);

                string xsdFilePath = Path.ChangeExtension(pmtFilePath, ".xsd");
                if (!File.Exists(xsdFilePath))
                {
                    File.WriteAllText(xsdFilePath, Helpers.Constants.MappingSchemaFileContent);
                }
                AddFile(xsdFilePath, Helpers.Constants.SolutionItems);
                Created(this, new EventArgs());
			}
			return item;
		}

        /// <summary>
        /// Gets an existing the mapping file or null otherwise.
        /// </summary>
        public HierarchyNode GetMappingFile()
        {
            using (HierarchyNode root = new HierarchyNode(VSSolution))
            using (HierarchyNode folder = root.FindByName(Helpers.Constants.SolutionItems))
            {
                if (folder != null)
                {
                    return folder.FindByName(Helpers.Constants.MappingFile);
                }
                return null;
            }
        }

        /// <summary>
        /// Sets the mapping file location.
        /// </summary>
        /// <param name="file"></param>
        public void SetMappingFile(string file)
        {
            Guard.ArgumentNotNullOrWhiteSpaceString(file, "file");
            if (!File.Exists(file)) throw new FileNotFoundException();

            // Reset internal vars
            ReloadMappingFile();
            this.mappingFileInfo = new FileInfo(file);
        }

		/// <summary>
		/// Deletes the mapping file.
		/// </summary>
		public void DeleteMappingFile()
		{
			using (HierarchyNode item = InternalGetOrCreateMappingFile())
			{
				if (item != null)
				{
					item.Remove();
					File.Delete(GetMappingFileName());
					ReloadMappingFile();
				}
			}
		}

		/// <summary>
		/// Reloads the mapping file.
		/// </summary>
		public void ReloadMappingFile()
		{
			// Force a reload by setting the mapping information to null
			this.projectMappingInformation = null;
			this.mappingFileInfo = null;
            this.Reloaded(this, new EventArgs());
		}

		/// <summary>
		/// Gets the project mapping entry.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		public ProjectMapping.Configuration.ProjectMappingEntry GetProjectMappingEntry(
			string projectMappingTableName, Guid projectId)
		{
			Guard.ArgumentNotNullOrEmptyString(projectMappingTableName, "projectMappingTableName");
			GuidGuard.GuidNotEmpty(projectId, "projectId");

			ProjectMappingTable projectMappingTable =
				  this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTableName);

			if (projectMappingTable == null)
			{
				throw new ProjectMappingTableNotFoundException(Properties.Resources.ProjectMappingTableNotFound);
			}

			return projectMappingTable.FindProjectMappingByProjectId(projectId);
		}

		/// <summary>
		/// Adds the project mapping entry.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectMapping">The project mapping.</param>
		public void AddProjectMappingEntry(string projectMappingTableName, ProjectMapping.Configuration.ProjectMappingEntry projectMapping)
		{
			Guard.ArgumentNotNullOrEmptyString(projectMappingTableName, "projectMappingTableName");
			Guard.ArgumentNotNull(projectMapping, "projectMapping");

			ProjectMappingTable projectMappingTable =
				this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTableName);

			if (projectMappingTable == null)
			{
				throw new ProjectMappingTableNotFoundException(Properties.Resources.ProjectMappingTableNotFound);
			}

			if (projectMappingTable.FindProjectMappingByProjectId(new Guid(projectMapping.ProjectId)) != null)
			{
				throw new ProjectMappingAlreadyExistsException(Properties.Resources.ProjectMappingAlreadyExists);
			}

			//Add ProjectMapping
			projectMappingTable.ProjectMappings.Add(projectMapping);

			UpdateMappingFile();
		}

		/// <summary>
		/// Deletes the project mapping entry.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		/// <param name="projectId">The project id.</param>
		public void DeleteProjectMappingEntry(string projectMappingTableName, Guid projectId)
		{
			Guard.ArgumentNotNullOrEmptyString(projectMappingTableName, "projectMappingTableName");
			GuidGuard.GuidNotEmpty(projectId, "projectId");

			ProjectMappingTable projectMappingTable =
				this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTableName);

			if (projectMappingTable == null)
			{
				throw new ProjectMappingTableNotFoundException(Properties.Resources.ProjectMappingTableNotFound);
			}

			ProjectMapping.Configuration.ProjectMappingEntry projectMapping =
				projectMappingTable.FindProjectMappingByProjectId(projectId);

			if (projectMapping == null)
			{
				throw new ProjectMappingNotFoundException(Properties.Resources.ProjectNotFound);
			}

			//Delete ProjectMapping
			projectMappingTable.ProjectMappings.Remove(projectMapping);

			UpdateMappingFile();
		}

		/// <summary>
		/// Adds the project mapping table entry.
		/// </summary>
		/// <param name="projectMappingTable">The project mapping table.</param>
		public void AddProjectMappingTableEntry(ProjectMappingTable projectMappingTable)
		{
			Guard.ArgumentNotNull(projectMappingTable, "projectMappingTable");

			if (this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTable.Name) != null)
			{
				throw new ProjectMappingTableAlreadyExistsException(Properties.Resources.ProjectMappingTableAlreadyExists);
			}

			//Add ProjectMappingTable
			this.ProjectMappingInformation.ProjectMappingTables.Add(projectMappingTable);

			UpdateMappingFile();
		}

		/// <summary>
		/// Deletes the project mapping table entry.
		/// </summary>
		/// <param name="projectMappingTableName">Name of the project mapping table.</param>
		public void DeleteProjectMappingTableEntry(string projectMappingTableName)
		{
			Guard.ArgumentNotNullOrEmptyString(projectMappingTableName, "projectMappingTableName");

			ProjectMappingTable projectMappingTable =
				this.ProjectMappingInformation.FindProjectMappingTableByName(projectMappingTableName);

			if (projectMappingTable == null)
			{
				throw new ProjectMappingTableNotFoundException(Properties.Resources.ProjectMappingTableNotFound);
			}

			//Delete ProjectMappingTable
			this.ProjectMappingInformation.ProjectMappingTables.Remove(projectMappingTable);

			UpdateMappingFile();
		}

		/// <summary>
		/// Gets the project.
		/// </summary>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		public EnvDTE.Project GetProject(Guid projectId)
		{
			GuidGuard.GuidNotEmpty(projectId, "projectId");
			return new HierarchyNode(vsSolution, projectId).ExtObject as EnvDTE.Project;
		}

        /// <summary>
        /// Gets/Sets the state for suspending or resuming file updates
        /// </summary>
        public bool SuspendUpdates
        {
            get { return this.suspendedUpdates; }
            set
            {
                this.suspendedUpdates = value;
                if (false == value)
                {
                    // Write pending updates to file
                    UpdateMappingFile();
                }
            }
        }

		#endregion

		#region Private Implementation

		private string GetMappingFileName()
		{
			string solutionDirectory;

			using (HierarchyNode node = new HierarchyNode(VSSolution))
			{
				if (node.ProjectDir == null)
				{
					string solutionFile;
					string optsFile;
					VSSolution.GetSolutionInfo(out solutionDirectory, out solutionFile, out optsFile);
				}
				else
				{
					solutionDirectory = node.ProjectDir;
				}
			}
			return Path.Combine(solutionDirectory, Helpers.Constants.MappingFile);
		}

		private void UpdateMappingFile()
		{
            if (false == suspendedUpdates)
            {
                GenericSerializer.Serialize<ProjectMappingInformation>
                    (this.ProjectMappingInformation, this.MappingFileInfo, ServiceProvider);
            }
		}

		private Microsoft.VisualStudio.Shell.Interop.IVsSolution VSSolution
		{
			get
			{
				if (vsSolution == null)
				{
					vsSolution = GetService<Microsoft.VisualStudio.Shell.Interop.IVsSolution, Microsoft.VisualStudio.Shell.Interop.SVsSolution>();
				}
				return vsSolution;
			}
		}

		private FileInfo MappingFileInfo
		{
			get
			{
				if (mappingFileInfo == null)
				{
					using (HierarchyNode item = CreateMappingFile())
					{
						mappingFileInfo = new FileInfo(item.Path);
					}
				}
				return mappingFileInfo;
			}
		}

		private ProjectMappingInformation ProjectMappingInformation
		{
			get
			{
				if (projectMappingInformation == null)
				{
					projectMappingInformation =
                        GenericSerializer.Deserialize<ProjectMappingInformation>(this.MappingFileInfo);

					if (projectMappingInformation == null)
					{
						projectMappingInformation = new ProjectMappingInformation();
					}
				}

				return projectMappingInformation;
			}
		}

		private TService GetService<TService, SService>()
		{
			return (TService)ServiceProvider.GetService(typeof(SService));
		}

		private HierarchyNode InternalGetOrCreateMappingFile()
		{
            using (HierarchyNode root = new HierarchyNode(VSSolution))
            {
                HierarchyNode found = root.RecursiveFindByName(Helpers.Constants.MappingFile);
                if (found != null)
                {
                    return found;
                }
                using (found = root.FindOrCreateSolutionFolder(Helpers.Constants.SolutionItems))
                {
                    return found != null ? found.FindByName(Helpers.Constants.MappingFile) : null;
                }
            }
		}

        private HierarchyNode AddFile(string path, string location)
        {
            using (HierarchyNode folder = new HierarchyNode(VSSolution).FindOrCreateSolutionFolder(location))
            if(folder != null)
            {
                using (ProjectNode folderProject = new ProjectNode(VSSolution, folder.ProjectGuid))
                {
                    // Add xsd
                    HierarchyNode item = folderProject.AddItem(path);
                    Debug.Assert(item != null, "Upps the PMT item could not be added");
                    IVsWindowFrame wnd = folderProject.OpenItem(item);
                    Debug.Assert(wnd != null, "Upps the PMT.xsd item could not be opened");
                    wnd.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_SaveIfDirty);
                    return item;
                }
            }
            return null;
        }

		#endregion
    }
}