//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EnvDTE;
using VSLangProj;
using VsWebSite;

namespace Microsoft.Practices.Modeling.Common
{
    /// <summary>
    /// Provides extended utility methods for working with the DTE (see <see cref="DteHelper"/> class).
    /// </summary>
    public static class DteHelperEx
    {
        #region Fields & Constants

        /// <summary>
        /// Key to access the CurrentWebsiteLanguage property of a project.
        /// </summary>
        private const string CurrentWebsiteLanguagePropertyItem = "CurrentWebsiteLanguage";

        /// <summary>
        /// Value of the CurrentWebsiteLanguage property of a project.
        /// </summary>
        private const string CurrentWebsiteLanguagePropertyValue = "Visual C#";

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the first <see cref="Project"/> in the solution that matches the specified criteria
        /// bypassing all Solution Folders.
        /// </summary>
        /// <param name="vs">The VS instance.</param>
        /// <param name="match">The predicate condition.</param>
        /// <returns>The <see cref="Project"/> found or <see langword="null" />.</returns>
        public static Project FindProject(_DTE vs, Predicate<Project> match)
        {
            Guard.ArgumentNotNull(vs, "vs");
            Guard.ArgumentNotNull(match, "match");

            foreach (Project project in vs.Solution.Projects)
            {
                if (match(project))
                {
                    return project;
                }
                Project found = FindProjectInternal(project.ProjectItems, match);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the name of the project by.
        /// </summary>
        /// <param name="dte">The DTE.</param>
        /// <param name="name">The name.</param>
        /// <param name="isWeb">if set to <c>true</c> [is web].</param>
        /// <returns></returns>
        public static Project FindProjectByName(DTE dte, string name, bool isWeb)
        {
            Project project = null;

            if (!isWeb)
            {
                project = FindProject(dte, delegate(Project internalProject)
                {
                    return internalProject.Name == name;
                });
            }
            else
            {
                foreach (Project projectTemp in dte.Solution.Projects)
                {
                    if (projectTemp.Name.Contains(name))
                    {
                        project = projectTemp;
                        break;
                    }

                    if (projectTemp.ProjectItems != null)
                    {
                        Project projectTemp1 = FindProjectByName(projectTemp.ProjectItems, name);
                        if (projectTemp1 != null)
                        {
                            project = projectTemp1;
                            break;
                        }
                    }
                }
            }

            return project;
        }

        /// <summary>
        /// Retrieves the first <see cref="ProjectItem"/> in the solution that matches the specified criteria
        /// bypassing all Solution Folders.
        /// </summary>
        /// <param name="vs">The VS instance.</param>
        /// <param name="match">The predicate condition.</param>
        /// <returns>The <see cref="ProjectItem"/> found or <see langword="null" />.</returns>
        public static ProjectItem FindItem(_DTE vs, Predicate<Project> match)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Projects the exists.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static bool ProjectExists(Solution solution, string projectName, LanguageType language)
        {
            bool exists = false;

            string solutionPath = Path.GetDirectoryName(
                            solution.Properties.Item("Path").Value.ToString());

            if (Directory.Exists(solutionPath))
            {
                string projectFile = string.Concat(projectName, GetProjectExtension(language));

                exists = (Directory.GetFiles(
                            solutionPath,
                            projectFile,
                            SearchOption.AllDirectories).Length > 0);
            }

            return exists;
        }

        /// <summary>
        /// Finds a <see cref="ProjectItem"/> by name. 
        /// The comparison is not case sensitive.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="name">The name.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public static ProjectItem FindItemByName(ProjectItems collection, string name, bool recursive)
        {
            Guard.ArgumentNotNull(name, "name");

            if (collection != null)
            {
                foreach (ProjectItem item1 in collection)
                {
                    if (item1.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return item1;
                    }
                    if (recursive)
                    {
                        ProjectItem item2 = DteHelperEx.FindItemByName(item1.ProjectItems, name, recursive);
                        if (item2 != null)
                        {
                            return item2;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a ProjectItem that contains a specific CodeType
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ProjectItem FindContainingProjectItem(_DTE dte, CodeType type)
        {
            foreach (Project project in new ProjectIterator(dte))
            {
                foreach (ProjectItem item in project.ProjectItems)
                {
                    if (item.FileCodeModel != null)
                    {
                        foreach (CodeElement element in item.FileCodeModel.CodeElements)
                        {
                            if (element.Kind == vsCMElement.vsCMElementNamespace)
                            {
                                foreach (CodeElement member in ((CodeNamespace)element).Members)
                                {
                                    if (member.Kind == type.Kind &&
                                        member.FullName.Equals(type.FullName, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        return item;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Performs the validation of the item passed as target
        /// Returns true if the reference is allowed to be executed in the target
        /// that is if the target is a web project and C# project
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>
        /// 	<c>true</c> if [is web C sharp project] [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWebCSharpProject(object target)
        {
            Project project = null;
            if (target is Project)
            {
                project = (Project)target;
            }
            else if (target is ProjectItem)
            {
                project = ((ProjectItem)target).ContainingProject;
            }

            if (project != null &&
                DteHelper2.IsWebProject(project) &&
                project.Properties != null)
            {
                try
                {
                    Property property = project.Properties.Item(CurrentWebsiteLanguagePropertyItem);
                    return (property.Value != null &&
                        property.Value.ToString().Equals(CurrentWebsiteLanguagePropertyValue, StringComparison.InvariantCultureIgnoreCase));
                }
                catch (Exception exception)
                {
                    Trace.TraceError(exception.ToString());
                    // Some Project implementations throws this excpetion (i.e.: Analysis services project)
                    // or the CurrentWebsiteLanguagePropertyItem property does not exists.
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Shows the message in output window.
        /// </summary>
        /// <param name="dte">The DTE instance.</param>
        /// <param name="message">The message.</param>
        public static void ShowMessageInOutputWindow(DTE dte, string message)
        {
            ShowMessageInOutputWindow(dte, message, null);
        }

        /// <summary>
        /// Shows the message in output window.
        /// </summary>
        /// <param name="dte">The DTE instance.</param>
        /// <param name="message">The message.</param>
        /// <param name="paneName">Name of the pane.</param>
        public static void ShowMessageInOutputWindow(DTE dte, string message, string paneName)
        {
            Guard.ArgumentNotNull(dte, "dte");

            OutputWindow outputWindow = ((EnvDTE80.DTE2)dte).ToolWindows.OutputWindow;
            OutputWindowPane pane = GetPane(outputWindow, paneName);
            pane.OutputString(message);
            pane.OutputString(Environment.NewLine);
            pane.Activate();
            outputWindow.Parent.Activate();
        }

        /// <summary>
        /// Gets the code DOM provider from the language of the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public static CodeDomProvider GetCodeDomProvider(Project project)
        {
            if (project != null)
            {
                return CodeDomProvider.CreateProvider(
                    CodeDomProvider.GetLanguageFromExtension(GetDefaultExtension(project)));
            }

            return CodeDomProvider.CreateProvider("C#");
        }

        /// <summary>
        /// Retrieves the code file extension for the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public static string GetDefaultExtension(Project project)
        {
            if (DteHelper2.IsWebProject(project))
            {
                return IsWebCSharpProject(project) ? ".cs" : ".vb";
            }
            return DteHelper2.GetDefaultExtension(project);
        }

        #endregion

        #region Private Implementation

        private static OutputWindowPane GetPane(OutputWindow outputWindow, string panelName)
        {
            if (!string.IsNullOrEmpty(panelName))
            {
                foreach (OutputWindowPane pane in outputWindow.OutputWindowPanes)
                {
                    if (pane.Name.Equals(panelName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return pane;
                    }
                }
                return outputWindow.OutputWindowPanes.Add(panelName);
            }
            return outputWindow.ActivePane;
        }

        private static string GetProjectExtension(LanguageType language)
        {
            switch (language)
            {
            case LanguageType.cs:
                return ".csproj";
            case LanguageType.vb:
                return ".vbproj";
            default:
                return ".csproj";
            }
        }

        private static Project FindProjectInternal(ProjectItems items, Predicate<Project> match)
        {
            if (items == null)
            {
                return null;
            }

            foreach (ProjectItem item in items)
            {
                Project project = item.SubProject ?? item.Object as Project; ;
                if (project != null)
                {
                    if (match(project))
                    {
                        return project;
                    }
                    project = FindProjectInternal(project.ProjectItems, match);
                    if (project != null)
                    {
                        return project;
                    }
                }
            }
            return null;
        }

        private static Project FindProjectByName(ProjectItems items, string name)
        {
            foreach (ProjectItem item1 in items)
            {
                if ((item1.Object is Project) && (((Project)item1.Object).Name.Contains(name)))
                {
                    return (item1.Object as Project);
                }

                if (item1.ProjectItems != null)
                {
                    Project project1 = FindProjectByName(item1.ProjectItems, name);
                    if (project1 != null)
                    {
                        return project1;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Iterator classes

        #region ProjectIterator class

        /// <summary>
        /// Project iterator that returns all projects in a solution recursivelly
        /// </summary>
        public class ProjectIterator : IEnumerable<Project>
        {
            private _DTE dte;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:ProjectIterator"/> class.
            /// </summary>
            /// <param name="dte">The DTE.</param>
            public ProjectIterator(_DTE dte)
            {
                Guard.ArgumentNotNull(dte, "dte");
                this.dte = dte;
            }

            #region IEnumerable<Project> Members

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<Project> GetEnumerator()
            {
                if (dte.Solution.Projects == null)
                {
                    yield break;
                }

                foreach (Project project in dte.Solution.Projects)
                {
                    yield return project;

                    foreach (Project subProject in GetSubprojects(project))
                    {
                        yield return subProject;
                    }
                }
            }

            private IEnumerable<Project> GetSubprojects(Project project)
            {
                if (project.ProjectItems == null)
                {
                    yield break;
                }

                foreach (ProjectItem item in project.ProjectItems)
                {
                    Project subProject = item.SubProject ?? item.Object as Project;
                    if (subProject != null)
                    {
                        yield return subProject;
                        foreach (Project subSubProject in GetSubprojects(subProject))
                        {
                            yield return subSubProject;
                        }
                    }
                }
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region ProjectItemIterator class

        /// <summary>
        /// Iterator that returns all project items recursivelly
        /// </summary>
        public class ProjectItemIterator : IEnumerable<ProjectItem>
        {
            private Project project;
            private ProjectItem projectItem;

            /// <summary>
            /// Initializates the iterator from a project. All the project items
            /// will be returned by the iterator.
            /// </summary>
            /// <param name="project"></param>
            public ProjectItemIterator(Project project)
            {
                Guard.ArgumentNotNull(project, "project");
                this.project = project;
            }

            /// <summary>
            /// Initializates the iterator from a project item. All the project
            /// items below the given one will be returned.
            /// </summary>
            /// <remarks>
            /// The project item given as an argument will not be returned
            /// by the iterator
            /// </remarks>
            /// <param name="projectItem"></param>
            public ProjectItemIterator(ProjectItem projectItem)
            {
                Guard.ArgumentNotNull(projectItem, "projectItem");
                this.projectItem = projectItem;
            }

            #region IEnumerable<ProjectItem> Members

            /// <summary>
            /// Returns an enumerator that iterates across all project items recursivelly
            /// </summary>
            /// <returns></returns>
            public IEnumerator<ProjectItem> GetEnumerator()
            {
                if (project != null)
                {
                    foreach (ProjectItem item in project.ProjectItems)
                    {
                        yield return item;
                        foreach (ProjectItem subItem in EnumerateProjectItem(item))
                        {
                            yield return subItem;
                        }
                    }
                }
                else if (projectItem != null)
                {
                    foreach (ProjectItem subItem in EnumerateProjectItem(projectItem))
                    {
                        yield return subItem;
                    }
                }
                else
                {
                    yield break;
                }
            }

            /// <summary>
            /// Iterates sub project items
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            private IEnumerable<ProjectItem> EnumerateProjectItem(ProjectItem item)
            {
                if (item.ProjectItems == null)
                {
                    yield break;
                }

                foreach (ProjectItem subItem in item.ProjectItems)
                {
                    yield return subItem;
                    foreach (ProjectItem subSubItem in EnumerateProjectItem(subItem))
                    {
                        yield return subSubItem;
                    }
                }
            }

            #endregion

            #region IEnumerable Members

            /// <summary>
            /// Returns an enumerator that run across all project items recursivelly
            /// </summary>
            /// <returns></returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        #region ProjectReferencesIterator class

        /// <summary>
        /// Iterator that returns all the referenced projects of a project
        /// </summary>
        public class ProjectReferencesIterator : IEnumerable<Project>
        {
            private Project project;

            /// <summary>
            /// Initializes the iterator with a project
            /// </summary>
            /// <param name="project"></param>
            public ProjectReferencesIterator(Project project)
            {
                Guard.ArgumentNotNull(project, "project");
                this.project = project;
            }

            #region IEnumerable<Project> Members

            /// <summary>
            /// Returns the enumerator
            /// </summary>
            /// <returns></returns>
            public IEnumerator<Project> GetEnumerator()
            {
                if (DteHelper2.IsWebProject(project))
                {
                    return IterateWebProjectReferences((VSWebSite)project.Object);
                }
                else
                {
                    return IterateProjectReferences((VSProject)project.Object);
                }
            }

            private IEnumerator<Project> IterateProjectReferences(VSProject vsProject)
            {
                if (vsProject.References == null)
                {
                    yield break;
                }

                foreach (Reference reference in vsProject.References)
                {
                    if (reference.SourceProject != null)
                    {
                        yield return reference.SourceProject;
                    }
                }
            }

            private IEnumerator<Project> IterateWebProjectReferences(VSWebSite vsWebSite)
            {
                if (vsWebSite.References == null)
                {
                    yield break;
                }

                foreach (AssemblyReference reference in vsWebSite.References)
                {
                    if (reference.ReferencedProject != null)
                    {
                        yield return reference.ReferencedProject;
                    }
                }
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        #endregion

        #region CodeElementsIterator class

        /// <summary>
        /// This iterator returns all the code elements inside a project item
        /// or below a specified code element recursively
        /// </summary>
        public class CodeElementsIterator : IEnumerable<CodeElement>
        {
            private CodeElements elements;

            /// <summary>
            /// Initializes the iterator from a project item
            /// </summary>
            /// <param name="projectItem"></param>
            public CodeElementsIterator(ProjectItem projectItem)
            {
                Guard.ArgumentNotNull(projectItem, "projectItem");

                if (projectItem.FileCodeModel != null)
                {
                    elements = projectItem.FileCodeModel.CodeElements;
                }
            }

            /// <summary>
            /// Initializes the iterator from a code element
            /// </summary>
            /// <param name="codeElement"></param>
            /// <remarks>
            /// The specified code element is not returned by the iterator
            /// </remarks>
            public CodeElementsIterator(CodeElement codeElement)
            {
                Guard.ArgumentNotNull(codeElement, "codeElement");
                elements = codeElement.Children;
            }

            #region IEnumerable<CodeElement> Members

            /// <summary>
            /// Returns the enumerator
            /// </summary>
            /// <returns></returns>
            public IEnumerator<CodeElement> GetEnumerator()
            {
                if (elements == null)
                {
                    yield break;
                }
                foreach (CodeElement element in EnumerateCodeElements(elements))
                {
                    yield return element;
                }
            }

            private IEnumerable<CodeElement> EnumerateCodeElements(CodeElements elements)
            {
                foreach (CodeElement element in elements)
                {
                    yield return element;
                    foreach (CodeElement subElement in EnumerateCodeElements(element.Children))
                    {
                        yield return subElement;
                    }
                }
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion

        #endregion
    }
}
