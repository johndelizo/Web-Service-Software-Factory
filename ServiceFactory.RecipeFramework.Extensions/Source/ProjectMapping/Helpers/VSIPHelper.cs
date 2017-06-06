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
using System.Runtime.InteropServices;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Helpers
{
	public static class VSIPHelper
	{
		public static void AddSolutionItem(IVsSolution solution, string fileName)
		{
			uint itemId = DteHelper2.FindItemByName(
				solution as IVsHierarchy, "Solution Items");

			IntPtr ptr = IntPtr.Zero;
			Guid solutionFolderGuid = new Guid("2150E333-8FDC-42a3-9474-1A3956D46DE8");
			Guid iidProject = typeof(IVsHierarchy).GUID;

			int res = solution.CreateProject(
				ref solutionFolderGuid,
				null,
				null,
				"Solution Items",
				0,
				ref iidProject,
				out ptr);

			if(ptr != IntPtr.Zero)
			{
				IVsHierarchy hierarchy = (IVsHierarchy)Marshal.GetObjectForIUnknown(ptr);

				Guid projGuid;

				hierarchy.GetGuidProperty(
					VSConstants.VSITEMID_ROOT,
					(int)__VSHPROPID.VSHPROPID_ProjectIDGuid,
					out projGuid);

				ProjectNode node = new ProjectNode(solution, projGuid);

				node.AddItem(fileName);
			}
		}
	}
}