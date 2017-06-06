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
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.TextManager.Interop;
using System.IO;
using Microsoft.VisualStudio;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks
{
	public class MockMappingServiceProvider : Microsoft.Practices.UnitTestLibrary.MockServiceProvider
	{
		private Guid guid1 = new Guid("4A216B22-B2B2-4851-AFFA-B7A5AF147645");
		private Guid guid2 = new Guid("DE91F8D4-0BB1-4768-ACF3-204ABB481AFD");
		private Guid guid3 = new Guid("BC9E7634-206C-43f4-81F3-5CA0D6DDBA99");
		private Guid guid4 = new Guid("A168E8C3-8CCD-47cd-AE2B-BE0F85F66781");
        private Guid guid5 = new Guid("4CEF011A-43A9-40CE-964B-1D03D28A1281");
        
        public MockMappingServiceProvider()
		{
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			root.AddProject(new MockVSHierarchy("Project1.project", guid1));
			root.AddProject(new MockVSHierarchy("Project2.project", guid2));
			root.AddProject(new MockVSHierarchy("Project3.project", guid3));
			root.AddProject(new MockVSHierarchy("Project4.project", guid4));
			AddService(typeof(SVsSolution), solution);
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ProjectMapping.xml");
			MockVSHierarchy mappingFileHier = new MockVSHierarchy(filePath, guid5);
			AddService(typeof(IVsRunningDocumentTable), new MockVsRDT(filePath,mappingFileHier,VSConstants.VSITEMID_ROOT));
			AddService(typeof(VsTextManagerClass), new MockVsTextManager());
			AddService(typeof(IVsUIShellOpenDocument), new MockVsUIShellOpenDocument());
			AddService(typeof(ILocalRegistry), new MockLocalRegistry());
			AddService(typeof(IVsFileChangeEx), new MockFileChangeEx());
			AddService(typeof(SVsFileChangeEx), new MockFileChangeEx());
		}
	}
}