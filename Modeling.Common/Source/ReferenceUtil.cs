//===============================================================================
// Microsoft patterns & practices
// Microsoft.Practices.RecipeFramework.Library, Version=2.0.0.0
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
using System.Linq;
using System.Text;
using EnvDTE;

namespace Microsoft.Practices.Modeling.Common
{
    public sealed class ReferenceUtil
    {
        // Methods
        public static bool HaveAClass(object target, out CodeClass codeClass)
        {
            ProjectItem projectItem = null;
            if (target is SelectedItems)
            {
                SelectedItems items = (SelectedItems)target;
                if ((items.Count > 1) && (items.Item(1).ProjectItem != null))
                {
                    projectItem = items.Item(1).ProjectItem;
                }
            }
            else if (target is ProjectItem)
            {
                projectItem = (ProjectItem)target;
            }
            if ((projectItem != null) && (projectItem.FileCodeModel != null))
            {
                foreach (CodeElement element in projectItem.FileCodeModel.CodeElements)
                {
                    if (element is CodeNamespace)
                    {
                        CodeNamespace namespace2 = (CodeNamespace)element;
                        if ((namespace2.Members.Count > 0) && (namespace2.Members.Item(1) is CodeClass))
                        {
                            codeClass = (CodeClass)namespace2.Members.Item(1);
                            return true;
                        }
                    }
                }
            }
            codeClass = null;
            return false;
        }

        public static bool IsCSharpProject(object target)
        {
            Project containingProject = null;
            if (target is Project)
            {
                containingProject = (Project)target;
            }
            else if (target is ProjectItem)
            {
                containingProject = ((ProjectItem)target).ContainingProject;
            }
            return ((containingProject != null) && (containingProject.Kind == "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"));
        }

        public static bool IsUnderFolder(object target, string folderName)
        {
            if (target is ProjectItem)
            {
                ProjectItem item = (ProjectItem)target;
                if (((item != null) && item.Kind.Equals("{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}")) && item.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsUnderFolderNested(object target, string folderName)
        {
            if (target is ProjectItem)
            {
                for (ProjectItem item = (ProjectItem)target; (item != null) && item.Kind.Equals("{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}"); item = (ProjectItem)item.Collection.Parent)
                {
                    if (item.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
