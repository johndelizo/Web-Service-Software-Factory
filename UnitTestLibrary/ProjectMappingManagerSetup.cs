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
using System.IO;
using System.Text;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;

namespace Microsoft.Practices.UnitTestLibrary
{
    public static class ProjectMappingManagerSetup
    {
        public static void InitializeManager(string fileName)
        {
            InitializeManager(new MockMappingServiceProvider(), fileName);
        }

        public static void InitializeManager(IServiceProvider serviceProvider, string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            Assert.IsTrue(File.Exists(filePath), "Mapping file not found: " + filePath);
            ProjectMappingManager.Instance.SetMappingFile(filePath);
         }

        public static IProjectMappingManager CreateManager(string fileName)
        {
            return CreateManager(new MockMappingServiceProvider(), fileName);
        }

        public static IProjectMappingManager CreateManager(IServiceProvider serviceProvider, string fileName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            Assert.IsTrue(File.Exists(filePath), "Mapping file not found: " + filePath);
            // create a temp copy to avoid locks
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(fileName)));
            // Force mapping file to reload from disk since there is a singleton reference.
            File.Copy(filePath, fileName);
            IProjectMappingManager manager = new ProjectMappingManager(serviceProvider);
            manager.SetMappingFile(fileName);
            return manager;
        }
    }
}
