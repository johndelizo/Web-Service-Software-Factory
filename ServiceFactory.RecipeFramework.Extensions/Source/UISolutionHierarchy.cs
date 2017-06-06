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
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions
{
	public static class UISolutionHierarchy
	{
		public static TreeNode CreateHierarchy(Solution solution, Predicate<Project> projectMatch, Predicate<ProjectItem> projectItemMatch)
		{
			return CreateHierarchy(solution, projectMatch, projectItemMatch, delegate(TreeNode node) { });
		}

		public static TreeNode CreateHierarchy(Solution solution, Predicate<Project> projectMatch, Predicate<ProjectItem> projectItemMatch, Action<TreeNode> nodeCreation)
		{
			TreeNode solutionNode = CreateHierarchyNode(solution, nodeCreation);

			foreach(Project project in solution.Projects)
			{
				CreateHierarchy(project, projectMatch, projectItemMatch, solutionNode, nodeCreation);
			}

			return solutionNode;
		}

		private static void CreateHierarchy(Project project, Predicate<Project> projectMatch, Predicate<ProjectItem> projectItemMatch, TreeNode parentNode, Action<TreeNode> nodeCreation)
		{
			if(projectMatch(project))
			{
				TreeNode node = CreateHierarchyNode(project, nodeCreation);
				parentNode.Nodes.Add(node);

                if (project.ProjectItems != null)
                {
                    if (project.Object is SolutionFolder)
                    {
                        foreach (ProjectItem projectItem in project.ProjectItems)
                        {
                            Project subProject = projectItem.Object as Project;

                            if (subProject != null)
                            {
                                CreateHierarchy(subProject, projectMatch, projectItemMatch, node, nodeCreation);
                            }
                            else
                            {
                                CreateHierarchy(projectItem, projectItemMatch, node, nodeCreation);
                            }
                        }
                    }
                    else
                    {
                        foreach (ProjectItem projectItem in project.ProjectItems)
                        {
                            CreateHierarchy(projectItem, projectItemMatch, node, nodeCreation);
                        }
                    }
                }
			}
		}

		private static void CreateHierarchy(ProjectItem projectItem, Predicate<ProjectItem> projectItemMatch, TreeNode parentNode, Action<TreeNode> nodeCreation)
		{
			if(projectItemMatch(projectItem))
			{
				TreeNode node = CreateHierarchyNode(projectItem, nodeCreation);
				parentNode.Nodes.Add(node);

				if(projectItem.ProjectItems != null)
				{
					foreach(ProjectItem subProjectItem in projectItem.ProjectItems)
					{
						CreateHierarchy(subProjectItem, projectItemMatch, node, nodeCreation);
					}
				}
			}
		}

		private static TreeNode CreateHierarchyNode(Solution solution, Action<TreeNode> nodeCreation)
		{
			return CreateHierarchyNode("Solution", solution, nodeCreation);
		}

		private static TreeNode CreateHierarchyNode(Project project, Action<TreeNode> nodeCreation)
		{
			return CreateHierarchyNode(project.Name, project, nodeCreation);
		}

		private static TreeNode CreateHierarchyNode(ProjectItem projectItem, Action<TreeNode> nodeCreation)
		{
			return CreateHierarchyNode(projectItem.Name, projectItem, nodeCreation);
		}

		private static TreeNode CreateHierarchyNode(string text, object tag, Action<TreeNode> nodeCreation)
		{
			TreeNode node = new TreeNode();
			node.Tag = tag;
			node.Text = text;
			nodeCreation(node);

			return node;
		}
	}
}