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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Design.Serialization;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	public class CodeGenerationService : ICodeGenerationService
	{
		IServiceProvider serviceProvider;
		// store the files we alredy generated in order to avoid generating moer than once.
		// we use a StringDictionary (hashtable) in order to speed up searchs
		HashSet<string> visitedFiles;

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeGenerationService"/> class.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		public CodeGenerationService(IServiceProvider serviceProvider)
		{
			Guard.ArgumentNotNull(serviceProvider, "serviceProvider");
			this.serviceProvider = serviceProvider;
            this.visitedFiles = new HashSet<string>();
		}

        /// <summary>
		/// Generates the artifacts.
		/// </summary>
		/// <param name="artifactLinks">The artifact links.</param>
		/// <returns></returns>
        public int GenerateArtifacts(ICollection<IArtifactLink> artifactLinks)
        {
            return GenerateArtifacts(artifactLinks, null);
        }

		/// <summary>
		/// Generates the artifacts.
		/// </summary>
		/// <param name="artifactLinks">The artifact links.</param>
		/// <returns></returns>
		public int GenerateArtifacts(ICollection<IArtifactLink> artifactLinks, Action<IArtifactLink> linkUpdated)
		{
			Guard.ArgumentNotNull(artifactLinks, "artifactLinks");
			
			// reset the visited files list
            this.visitedFiles = new HashSet<string>();

			// list to avoid duplicate code gen of an already visited link.
			HashSet<IArtifactLink> generatedLinks = new HashSet<IArtifactLink>();
			int generatedObjects = 0;

			foreach (IArtifactLink link in artifactLinks)
			{
				if(!generatedLinks.Contains(link))					
				{
					int generated = GenerateArtifact(link);
					if (generated > 0)
					{
						generatedLinks.Add(link);
						generatedObjects += generated;
                        if (linkUpdated != null) linkUpdated(link);
					}
				}
			}
			return generatedObjects;
		}

		/// <summary>
		/// Generates the artifact.
		/// </summary>
		/// <param name="artifactLink">The artifact link.</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public int GenerateArtifact(IArtifactLink artifactLink)
		{
			Guard.ArgumentNotNull(artifactLink, "artifactLink");
			int generatedObjects = 0;

			try
			{
				IVsSolution vsSolution = GetService<IVsSolution, SVsSolution>();
				ProjectNode projectNode = new ProjectNode(vsSolution, artifactLink.Container);

                try
                {
                    ICodeGenerationStrategy strategy = CreateStrategy(artifactLink, projectNode);
                    CodeGenerationResults result = strategy.Generate(artifactLink);

                    bool hasErrors = CheckErrors(strategy);

                    foreach (string file in result.Files)
                    {
                        string fullPath = Path.Combine(projectNode.ProjectDir, file);
                        if (!this.visitedFiles.Contains(fullPath) &&
                            AddFile(projectNode, file, result[file]))
                        {
                            generatedObjects++;
                            this.visitedFiles.Add(fullPath);
                        }
                    }

                    if (!hasErrors && result.Count > 0)
                    {
                        AddProjectReferences(projectNode, strategy.ProjectReferences);
                        AddAssemblyReferences(projectNode, strategy.AssemblyReferences);
                    }
                }
                catch (Exception e)
                {
                    // Report all errors to user as generated code output.
                    LogException(e);
                    if (!IsConfigFile(artifactLink.ItemPath))
                    {
                        // Add the offending file with a warning in it.
                        AddFile(projectNode, artifactLink.ItemPath, Properties.Resources.Generation_Exception);
                    }
                }
                finally
                {
                    if (projectNode != null) projectNode.Dispose();
                }
			}
			catch (Exception e) 
			{
				// Report all errors to user as generated code output.
				LogException(e);
			}

			return generatedObjects;
		}

		/// <summary>
		/// Determines whether the specified artifact link is valid.
		/// </summary>
		/// <param name="artifactLink">The artifact link.</param>
		/// <returns>
		/// 	<c>true</c> if the specified artifact link is valid; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValid(IArtifactLink artifactLink)
		{
            if (artifactLink == null || 
                string.IsNullOrWhiteSpace(artifactLink.ItemPath))
            {
                return false;
            }
			
			IVsSolution vsSolution = GetService<IVsSolution, SVsSolution>();
            IVsHierarchy projectHierarchy = null;
			Guid container = artifactLink.Container;
			int hr = vsSolution.GetProjectOfGuid(ref container, out projectHierarchy);
			if (hr == VSConstants.S_OK && projectHierarchy is IVsProject)
			{
                using (ProjectNode projectNode = new ProjectNode(vsSolution, artifactLink.Container))
                {
                    return projectNode.CanAddItem(artifactLink.ItemPath);
                }
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified artifact link collection is valid.
		/// </summary>
		/// <param name="artifactLinks">The artifact links.</param>
		/// <returns></returns>
		public bool AreValid(ICollection<IArtifactLink> artifactLinks)
		{
            if (artifactLinks == null) return false;
			
			bool result = true;
			foreach (IArtifactLink link in artifactLinks)
			{
				if (!IsValid(link))
				{
					result = false;
                    break;
				}
			}
			return result;
		}

		/// <summary>
		/// Gets the generated item from artifact.
		/// </summary>
		/// <param name="artifactLink">The artifact link.</param>
		/// <param name="oldName">The old name.</param>
		/// <returns></returns>
		public HierarchyNode GetGeneratedItemFromArtifact(IArtifactLink artifactLink, string oldName)
		{		
			if (IsValid(artifactLink))
			{
				IVsSolution vsSolution = GetService<IVsSolution, SVsSolution>();
                using (ProjectNode projectNode = new ProjectNode(vsSolution, artifactLink.Container))
                {
                    FileInfo fi = new FileInfo(artifactLink.ItemPath);                    
                    string itemPath = oldName + fi.Extension;
                    HierarchyNode item = projectNode.FindByName(itemPath);
                    return item;
                }
			}
			return null;
		}

		/// <summary>
		/// Gets the generated item from artifact.
		/// </summary>
		/// <param name="artifactLink">The artifact link.</param>
		/// <returns></returns>
		public HierarchyNode GetGeneratedItemFromArtifact(IArtifactLink artifactLink)
		{
			Guard.ArgumentNotNull(artifactLink, "artifactLink");
			
			string oldName = Path.GetFileNameWithoutExtension(artifactLink.ItemPath);
			return GetGeneratedItemFromArtifact(artifactLink, oldName);
		}

		/// <summary>
		/// Determines whether [is artifact already generated] [the specified artifact link].
		/// </summary>
		/// <param name="artifactLink">The artifact link.</param>
		/// <returns>
		/// 	<c>true</c> if [is artifact already generated] [the specified artifact link]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsArtifactAlreadyGenerated(IArtifactLink artifactLink)
		{
			Guard.ArgumentNotNull(artifactLink, "artifactLink");
			
			return (GetGeneratedItemFromArtifact(artifactLink)!=null);
		}

		/// <summary>
		/// Validates the delete.
		/// </summary>
		/// <param name="artifactLink">The artifact link.</param>
		/// <returns></returns>
		public HierarchyNode ValidateDelete(IArtifactLink artifactLink)
		{
			Guard.ArgumentNotNull(artifactLink, "artifactLink");
			
			HierarchyNode node = GetGeneratedItemFromArtifact(artifactLink);
			if (node!=null)
			{
				LogEntry(new LogEntry(
					String.Format(
						CultureInfo.CurrentUICulture,
						Properties.Resources.ArtifactDeleted,
						artifactLink.ItemPath),
					Properties.Resources.Generation_Error_Title,
					TraceEventType.Warning,
					(int)Errors.ArtifactShouldBeDeleted));
			}
			return node;
		}

		/// <summary>
		/// Validates the rename.
		/// </summary>
		/// <param name="artifactLink">The artifact link.</param>
		/// <param name="newName">The new name.</param>
		/// <param name="oldName">The old name.</param>
		/// <returns></returns>
		public HierarchyNode ValidateRename(IArtifactLink artifactLink, string newName, string oldName)
		{
			Guard.ArgumentNotNull(artifactLink, "artifactLink");
			Guard.ArgumentNotNull(newName, "newName");
			
			HierarchyNode node = GetGeneratedItemFromArtifact(artifactLink,oldName);
			if (node!=null && string.Compare(oldName, newName, StringComparison.OrdinalIgnoreCase) != 0)
			{
				LogEntry(new LogEntry(
					String.Format(
						CultureInfo.CurrentUICulture,
						Properties.Resources.ArtifactRenamed,
						new FileInfo(node.Path).Name,
						artifactLink.ItemPath),
					Properties.Resources.Generation_Error_Title,
					TraceEventType.Warning,
					(int)Errors.ArtifactShouldBeRenamed));
				return node;
			}
			return null;
		}

		/// <summary>
		/// Validates the delete from collection.
		/// </summary>
		/// <param name="artifactLinks">The artifact links.</param>
		/// <returns></returns>
		public ICollection<HierarchyNode> ValidateDeleteFromCollection(ICollection<IArtifactLink> artifactLinks)
		{
			Guard.ArgumentNotNull(artifactLinks, "artifactLinks");
			
			Logger.Clear();
			List<HierarchyNode> nodes = new List<HierarchyNode>();
			foreach (IArtifactLink link in artifactLinks)
			{
				HierarchyNode node = ValidateDelete(link);
				if (node != null)
				{
					nodes.Add(node);
				}
			}
			return nodes;
		}

		/// <summary>
		/// Validates the rename from collection.
		/// </summary>
		/// <param name="artifactLinks">The artifact links.</param>
		/// <param name="newName">The new name.</param>
		/// <param name="oldName">The old name.</param>
		/// <returns></returns>
		public ICollection<HierarchyNode> ValidateRenameFromCollection(
			ICollection<IArtifactLink> artifactLinks, string newName, string oldName)
		{
			Guard.ArgumentNotNull(artifactLinks, "artifactLinks");
			Guard.ArgumentNotNull(newName, "newName");
			
			Logger.Clear();
			List<HierarchyNode> nodes = new List<HierarchyNode>();
			foreach (IArtifactLink link in artifactLinks)
			{
				HierarchyNode node = ValidateRename(link, newName, oldName);
				if (node != null)
				{
					nodes.Add(node);
				}
			}
			return nodes;
		}

		#region Private Implementation

		private bool IsConfigFile(string file)
		{
			return Path.GetExtension(file).Equals(".config", StringComparison.OrdinalIgnoreCase);
		}

		private bool AddFile(ProjectNode projectNode, string file, string content)
		{
			// add the file to the project
			HierarchyNode node = projectNode.AddItem(file);
			if (node == null)
			{
				return false;
			}

			using (node)
			{
				// Hide the file already added
				IVsWindowFrame frame = projectNode.OpenItem(node);
				frame.Hide();
				using (DocData docData = new DocData(serviceProvider, node.Path))
				{
					docData.CheckoutFile(serviceProvider);
					using (DocDataTextWriter writer = new DocDataTextWriter(docData))
					{
						writer.Write(content);
					}
				}
				frame.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_SaveIfDirty);
				return true;
			}
		}

		private bool CheckErrors(ICodeGenerationStrategy strategy)
		{
			foreach (LogEntry error in strategy.Errors)
			{
				LogEntry(error);
			}

			return (strategy.Errors.Count > 0);
		}

		internal enum Errors
		{
			CodeGeneration = 1000,
			ArtifactShouldBeDeleted = 1001,
			ArtifactShouldBeRenamed = 1002
		}

		private object GetService(Type serviceType)
		{
			return serviceProvider.GetService(serviceType);
		}

		private T GetService<T, TImpl>()
		{
			return (T)GetService(typeof(TImpl));
		}

		protected ICodeGenerationStrategy CreateStrategy(IArtifactLink artifactLink, ProjectNode projectNode)
		{
			Debug.Assert(artifactLink != null);
			CodeGenerationStrategyAttribute strategyAttrib =
				ReflectionHelper.GetAttribute<CodeGenerationStrategyAttribute>(artifactLink.GetType(), true);

			if (strategyAttrib != null)
			{
				// add data
				Utility.SetData<IServiceProvider>(artifactLink, this.serviceProvider);
				Utility.SetData<ProjectNode>(artifactLink, projectNode);
				// return stategy instance
				return Activator.CreateInstance(strategyAttrib.CodeGeneratorType) as ICodeGenerationStrategy;
			}
			throw new InvalidOperationException(Properties.Resources.NoCodeGenerationStrategyAttribute);
		}

		protected virtual void LogEntry(LogEntry entry)
		{
			Logger.Write(entry);
		}

		private void LogException(Exception e)
		{
			LogEntry entry = new LogEntry(
				e.Message,
				Properties.Resources.Generation_Error_Title,
				TraceEventType.Error,
				(int)Errors.CodeGeneration);
			LogEntry(entry);
		}

		private void AddProjectReferences(ProjectNode projectNode, IList<Guid> references)
		{
			foreach (Guid projectId in references)
			{
				//Avoid Circular references
				if(projectId != projectNode.ProjectGuid)
				{
					projectNode.AddProjectReference(projectId);
				}
			}
		}

		private void AddAssemblyReferences(ProjectNode projectNode, IList<string> references)
		{
			foreach (string reference in references)
			{
				projectNode.AddAssemblyReference(reference);
			}
		}

		#endregion
	}
}
