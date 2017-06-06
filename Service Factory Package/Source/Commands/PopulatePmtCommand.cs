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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using EnvDTE;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.ServiceFactory.Commands
{
    public class PopulatePmtCommand : CommandBase
	{
		private readonly Guid SolutionFolderGuid = new Guid(EnvDTE.Constants.vsProjectItemKindVirtualFolder);
		private const string RolesEntry = "Roles";
		private readonly char[] RolesDelimiter = { '|' };
		private string solutionFolderName;
		private string mappingTableName;
        private const string ProjectPath = "\\GeneratedCode";
        IProjectMappingManager manager = ProjectMappingManager.Instance;

        public PopulatePmtCommand(IServiceProvider provider)
            : base(provider)
        { }

		protected override void OnExecute()
		{
            EnsureArguments();
            manager.SuspendUpdates = true;
            if (EnsureMappingTableExists())
            {
                IVsSolution solution = GetService<IVsSolution, SVsSolution>();
                using (HierarchyNode folder = new HierarchyNode(solution, solutionFolderName))
                {
                    TraverseHierarchyNode(folder);
                }
            }
            manager.SuspendUpdates = false;
		}

		#region Private Implementation

		private void EnsureArguments()
		{
            IVsSolution solution = this.GetService<IVsSolution>();
            uint pitemid = 0;
            using(HierarchyNode hierarchy = new HierarchyNode(solution, DteHelper2.GetCurrentSelection(this.Provider, out pitemid)))
            {
                solutionFolderName = hierarchy.UniqueName;
            }

            mappingTableName = solutionFolderName;

			// Ensure ProjectMapping.xml exists
            manager.CreateMappingFile().Dispose();
		}

		private bool EnsureMappingTableExists()
		{
            if (manager.GetMappingTable(mappingTableName) != null)
			{
				mappingTableName = CreateNewMappingTableName();
                // Ask to proceed with new name
                IUIService ui = this.GetService<IUIService>();
                if (ui.ShowMessage(
                    string.Format(CultureInfo.CurrentCulture, Resources.CreateNewMappingTable, mappingTableName), 
                    null, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
			}
			// Create new mapping table
			ProjectMappingTable projectMappingTable = new ProjectMappingTable(mappingTableName);
            manager.AddProjectMappingTableEntry(projectMappingTable);
            return true;
		}

		private string CreateNewMappingTableName()
		{
			string newName; 
			int suffix = 1;
            while (manager.GetMappingTable(
				newName = mappingTableName + suffix.ToString(CultureInfo.InvariantCulture)) != null)
			{
				suffix++;
			}
			return newName;
		}

		private void TraverseHierarchyNode(HierarchyNode node)
		{
			node.RecursiveForEach(delegate(HierarchyNode child)
			{
				// recurse if this node is a Solution Folder
				if (child.TypeGuid != SolutionFolderGuid)
				{
					// If this is a project, add the mapping
					if (child.ExtObject is EnvDTE.Project)
					{
						Collection<Role> roles = GetRoles(child.ExtObject as Project);
						if (child.ProjectGuid != Guid.Empty)
						{
							ProjectMappingEntry mapping = new ProjectMappingEntry(child.ProjectGuid, ProjectPath, roles, child.Name);
                            manager.AddProjectMappingEntry(mappingTableName, mapping);
                            this.Trace(Resources.MappingTableAddedMessage, TraceEventType.Information, child.Name, mappingTableName, roles.Count); 
						}
					}
				}
			});
		}

		private Collection<Role> GetRoles(Project project)
		{
			if (project.Globals != null && 
				project.Globals.get_VariableExists(RolesEntry))
			{
				string projectRolesEntry = project.Globals[RolesEntry].ToString();
				string[] projectRoles = projectRolesEntry.Split(RolesDelimiter);
				return BuildRolesCollection(projectRoles);
			}

			return InferRolesFromProjectName(project.Name);
		}

		private Collection<Role> BuildRolesCollection(string[] projectRoles)
		{
			List<Role> roles = new List<Role>();

			foreach (string role in projectRoles)
			{
				try
				{
					if (!String.IsNullOrEmpty(role))
					{
						ServiceFactoryRoleType roleType =
							(ServiceFactoryRoleType)Enum.Parse(typeof(ServiceFactoryRoleType), role, true);

						Role serviceFactoryRole = new Role(roleType.ToString());

						if (roles.Find(delegate(Role match)
						{
							if (match.Name == roleType.ToString())
							{
								return true;
							}

							return false;
						}
							) == null)
						{
							roles.Add(serviceFactoryRole);
						}
					}
				}
				catch (ArgumentException)
				{
					//Do Nothing Is not a valid role value
				}
			}

			return (new Collection<Role>(roles));
		}

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
		private Collection<Role> InferRolesFromProjectName(string projectName)
		{
			Collection<Role> roles = new Collection<Role>();
			projectName = projectName.ToLowerInvariant();

			foreach (string roleName in Enum.GetNames(typeof(ServiceFactoryRoleType)))
			{
				if (projectName.IndexOf(FormatRoleSuffix(roleName), StringComparison.OrdinalIgnoreCase) != -1)
				{
					roles.Add(new Role(roleName));
					break;
				}
			}

			return roles;
		}

        [SuppressMessage("Microsoft.Globalization","CA1308:NormalizeStringsToUppercase")]
		private string FormatRoleSuffix(string roleName)
		{
			// strip the "Role" suffix
			// case insensitive comparison (ToLowerInvariant) 
			string suffix = roleName.Replace("Role", string.Empty);

			// adjust suffix to v2 naming convention
			if (suffix.Equals("service", StringComparison.OrdinalIgnoreCase))
			{
				return "serviceimplementation";
			}

			return suffix.ToLowerInvariant();
		}

		#endregion
	}
}