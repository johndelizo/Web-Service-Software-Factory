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
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using System.Reflection;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums;
using Microsoft.Practices.Modeling.CodeGeneration.Metadata;
using System.Collections.ObjectModel;
using Microsoft.Practices.VisualStudio.Helper;
using System.Globalization;
using EnvDTE;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Artifacts
{
	public static class ArtifactLinkFactory
	{
		private static Guid ZeroProjectWithRoleGuid = Guid.Empty;
		private static Guid MoreThanOneProjectWithSameRoleGuid =
			new Guid(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });

        //static ArtifactLinkFactory()
        //{
        //    ProjectMappingManager.Instance.Reloaded += new EventHandler<EventArgs>(OnProjectMappingTableReloaded);
        //}

 		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <typeparam name="TArtifactLink">The type of the artifact link.</typeparam>
		/// <param name="modelElement">The model element.</param>
		/// <param name="mappingTable">Unique name of the project.</param>
		/// <returns></returns>
		public static TArtifactLink CreateInstance<TArtifactLink>(ModelElement modelElement, string mappingTable)
			where TArtifactLink : ArtifactLink, new()
		{
			Guard.ArgumentNotNull(modelElement, "modelElement");

			return (TArtifactLink)CreateInstance(typeof(TArtifactLink), modelElement, mappingTable, modelElement.GetType());
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <typeparam name="TArtifactLink">The type of the artifact link.</typeparam>
		/// <param name="modelElement">The model element.</param>
		/// <param name="mappingTable">Unique name of the project.</param>
		/// <param name="attributeProvider">The attribute provider.</param>
		/// <returns></returns>
		public static TArtifactLink CreateInstance<TArtifactLink>(ModelElement modelElement, string mappingTable, 
			ICustomAttributeProvider attributeProvider)
			where TArtifactLink : ArtifactLink, new()
		{
			return (TArtifactLink)CreateInstance(typeof(TArtifactLink), modelElement, mappingTable, attributeProvider);
		}

		/// <summary>
		/// Creates an instance
		/// </summary>
		/// <param name="artifactLinkType"></param>
		/// <param name="modelElement"></param>
		/// <param name="mappingTable"></param>
		/// <returns></returns>
        public static ArtifactLink CreateInstance(Type artifactLinkType, ModelElement modelElement, string mappingTable,
            ICustomAttributeProvider attributeProvider)
        {
            Guard.ArgumentNotNull(artifactLinkType, "artifactLinkType");
            Guard.ArgumentNotNull(modelElement, "modelElement");
            Guard.ArgumentNotNull(attributeProvider, "attributeProvider");

            Tuple<Type, Guid , string> key = new Tuple<Type, Guid, string>(artifactLinkType, modelElement.Id, mappingTable);
            return GlobalCache.AddOrGetExisting<ArtifactLink>(key.ToString(), k =>
                {
                    ArtifactLink link = CreateLink(artifactLinkType, modelElement);
                    if (!String.IsNullOrEmpty(mappingTable))
                    {
                        try
                        {
                            link.Container = GetContainer(mappingTable, attributeProvider);
                            link.Path = GetProjectPath(mappingTable, link.Container);
                            link.Project = GetProject(modelElement, link.Container);
                        }
                        catch (Exception e)
                        {
                            Logger.Write(e);
                        }
                    }
                    return link;
                });
        }

        #region Host Designer overloads

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="TArtifactLink">The type of the artifact link.</typeparam>
        /// <param name="modelElement">The model element.</param>
        /// <param name="projectUniqueName">Name of the project unique.</param>
        /// <param name="itemPath">The item path.</param>
        /// <returns></returns>
        public static TArtifactLink CreateInstance<TArtifactLink>(ModelElement modelElement, string projectUniqueName, string itemPath)
            where TArtifactLink : ArtifactLink, new()
        {
            return (TArtifactLink)CreateInstance(typeof(TArtifactLink), modelElement, projectUniqueName, itemPath);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="modelElement">The model element.</param>
        /// <param name="projectUniqueName">Name of the project unique.</param>
        /// <param name="itemPath">The item path.</param>
        /// <returns></returns>
        public static ArtifactLink CreateInstance(Type linkType, ModelElement modelElement, string projectUniqueName, string itemPath)
        {
            Guard.ArgumentNotNull(linkType, "linkType");
            Guard.ArgumentNotNull(modelElement, "modelElement");
            Guard.ArgumentNotNullOrEmptyString(projectUniqueName, "projectUniqueName");
            Guard.ArgumentNotNull(itemPath, "itemPath");

            Tuple<Type, Guid, string> key = new Tuple<Type, Guid, string>(linkType, modelElement.Id, projectUniqueName);
            return GlobalCache.AddOrGetExisting<ArtifactLink>(key.ToString(), k =>
                {
                    ArtifactLink link = CreateLink(linkType, modelElement);
                    try
                    {
                        link.Path = itemPath;
                        SetLinkContainerProperties(modelElement, projectUniqueName, link);
                    }
                    catch (Exception e)
                    {
                        Logger.Write(e);
                    }
                    return link;
                });
        }

        private static void SetLinkContainerProperties(ModelElement element, string projectUniqueName, ArtifactLink link)
        {
            IVsSolution vsSolution = GetService<IVsSolution, SVsSolution>(element);
            using (HierarchyNode hNode = new HierarchyNode(vsSolution, projectUniqueName))
            {
                link.Project = GetProjectFromHierarchyNode(hNode);
                link.Container = hNode.ProjectGuid;
            }
        }

        #endregion

        #region Private Implementation

        private static ArtifactLink CreateLink(Type artifactLinkType, ModelElement modelElement)
		{
			ArtifactLink link = (ArtifactLink)Activator.CreateInstance(artifactLinkType);
			link.ModelElement = modelElement;
			link.ItemName = GetName(modelElement);
			return link;
		}

		private static string GetProjectPath(string mappingTable, Guid guid)
		{
			if (!IsValidProjectId(guid))
			{
				return string.Empty;
			}
			return ProjectMappingManager.Instance.GetProjectPath(mappingTable, guid);
		}

		private static TInterface GetService<TInterface, TImpl>(ModelElement modelElement)
		{
			if (modelElement == null)
			{
				return default(TInterface);
			}
			return (TInterface)modelElement.Store.GetService(typeof(TImpl));
		}

		private static string GetName(ModelElement modelElement)
		{
			if (modelElement == null)
			{
				return string.Empty;
			}
			string modelElementName = string.Empty;
			if (DomainClassInfo.TryGetName(modelElement, out modelElementName))
			{
				return modelElementName;
			}
			return string.Empty;
		}

		private static Guid GetContainer(string mappingTable, ICustomAttributeProvider attributeProvider)
		{
			ProjectMappingRoleAttribute roleAttrib =
				ReflectionHelper.GetAttribute<ProjectMappingRoleAttribute>(attributeProvider, true);

			ServiceFactoryRoleType roleType = roleAttrib.RoleType;

			List<Role> roles = new List<Role>();
			roles.Add(new Role(roleType.ToString()));

			ReadOnlyCollection<Guid> projectGuids = ProjectMappingManager.Instance.GetProjectsInRoles(
				mappingTable,
				roles);

			if (projectGuids.Count == 0)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture,
					Properties.Resources.ZeroProjectWithRole, roleType.ToString()));
			}
			else if (projectGuids.Count > 1)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture,
					Properties.Resources.MoreThanOneProjectWithSameRole, roleType.ToString()));
			}

			return projectGuids[0];
		}

		private static Project GetProject(ModelElement element, Guid projectId)
		{
			IVsSolution vsSolution = GetService<IVsSolution, SVsSolution>(element);
			if (!IsValidProjectId(projectId))
			{
				return null;
			}
			using (HierarchyNode hNode = new HierarchyNode(vsSolution, projectId))
			{
				return GetProjectFromHierarchyNode(hNode);
			}
		}

		private static bool IsValidProjectId(Guid projectId)
		{
			return projectId != ZeroProjectWithRoleGuid &&
				   projectId != MoreThanOneProjectWithSameRoleGuid;
		}

        private static Project GetProjectFromHierarchyNode(HierarchyNode hNode)
        {
            if (hNode != null)
            {
                Project project = hNode.ExtObject as Project;
                return project;
            }
            return null;
        }

        //private static void OnProjectMappingTableReloaded(object sender, EventArgs e)
        //{
        //    // Clean cache to force reloading artifacts. 
        //    GlobalCache.Reset();
        //}

        #endregion
    }
}