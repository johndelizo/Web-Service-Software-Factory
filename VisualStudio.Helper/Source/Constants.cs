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

namespace Microsoft.Practices.VisualStudio.Helper
{
	public static class Constants
	{
		public const string SolutionFolderType = "2150E333-8FDC-42a3-9474-1A3956D46DE8";
		public static readonly Guid SolutionFolderGuid = new Guid("{6BB5F8F0-4483-11D3-8BCF-00C04F8EC28C}"); // EnvDTE.Constants.vsProjectItemKindVirtualFolder);
	}
}