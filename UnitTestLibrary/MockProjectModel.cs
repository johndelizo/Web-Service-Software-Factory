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
using System.Globalization;
using System.Text;
using System.ComponentModel.Design;
using Microsoft.Practices.Modeling.Presentation.Models;

namespace ServicesGuidancePackage.Tests.Common
{
    /// <summary>
    /// Mock class for testing things that need the IProjectModel interface
    /// </summary>
	public class MockProjectModel : IProjectModel
    {
        private string containedFile;
		private ITypeResolutionService typeResolutionService;
        private object mockProject;
		private List<ITypeModel> types;
        private List<IProjectItemModel> projectItems;

		public MockProjectModel()
		{
		    types = new List<ITypeModel>();
            projectItems = new List<IProjectItemModel>();
            mockProject = new object();
        }

        public MockProjectModel(string containedFile) : this()
        {
            this.containedFile = containedFile;
        }
       
        public object Project
        {
            get { return mockProject; }
			set { mockProject = value; }
        }

        public string FullPath
        {
            get { return containedFile; }
        }

        public bool ProjectContainsFile(string filename)
        {
            if( string.Compare( filename, containedFile, true, CultureInfo.InvariantCulture) == 0 )
            {
                return true;
            }
            return false;
        }

		public ITypeResolutionService TypeResolutionService
		{
			get { return typeResolutionService; }
			set { typeResolutionService = value; }
		}

		public IList<ITypeModel> Types
		{
			get { return types; }
		}

        public IList<IProjectItemModel> ProjectItems
        {
            get { return projectItems; }
        }

		public void AddType(ITypeModel type)
		{
			types.Add(type);
		}

        public void AddProjectItem(IProjectItemModel item)
        {
            projectItems.Add(item);
        }

        public bool IsWebProject
        {
            get { return false; }
        }

        public string ContainedFile
        {
            get { return containedFile; }
            set { containedFile = value; }
        }
	}
}
