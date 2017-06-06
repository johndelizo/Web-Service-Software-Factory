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
using System.Resources;
using System.Reflection;
using System.IO;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Helpers
{
	internal static class Constants
	{
		public const string MappingFile = "ProjectMapping.xml";
        public const string MappingFileSchema = "ProjectMapping.xsd";
        public const string SolutionItems = "Solution Items";

        public static string MappingFileContent
        {
            get
            {
                using (StreamReader reader = new StreamReader(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.NewMappingFile.xml")))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string MappingSchemaFileContent
        {
            get
            {
                using (StreamReader reader = new StreamReader(
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.ProjectMapping.xsd")))
                {
                    return reader.ReadToEnd();
                }
            }
        }
	}
}