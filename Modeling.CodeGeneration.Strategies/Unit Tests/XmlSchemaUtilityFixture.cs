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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using Microsoft.Practices.ServiceFactory.Description;
using System.IO;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.Tests
{
	/// <summary>
	/// Summary description for XmlSchemaUtilityFixture
	/// </summary>
	[TestClass]
	public class XmlSchemaUtilityFixture
	{
		[TestMethod]
		[DeploymentItem(@"SampleData\SimpleSchema1.xsd", "SampleData")]
		public void ShouldGetGetXmlSchemaSource()
		{
			MyArtifactLink link = new MyArtifactLink("SampleData\\SimpleSchema1.xsd", "element1");
			string schemaSource = XmlSchemaUtility.GetXmlSchemaSource(@"xsd:\\SampleData\SimpleSchema1.xsd?element1", link);

			Assert.IsTrue(Path.IsPathRooted(schemaSource));
			Assert.IsTrue(File.Exists(schemaSource));
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\BaseTypes.xsd", "SampleData")]
		public void ShouldGetGetBaseTypesFromReferencedType()
		{
			MyArtifactLink link = new MyArtifactLink("SampleData\\BaseTypes.xsd", "LandmarkPoint");
			IList<string> types = XmlSchemaUtility.GetBaseTypesFromReferencedType(@"xsd:\\SampleData\BaseTypes.xsd?LandmarkPoint", link);

			Assert.IsTrue(types.Count > 0);
			Assert.AreEqual<string>(link.Namespace + ".LandmarkBase", types[0]);
		}

		#region MyArtifactLink class

		class MyArtifactLink : ArtifactLink, IResourceResolver
		{
			IResourceResolver resolver;
			string itemPath;

			public MyArtifactLink(string itemPath, string itemName)
			{
				this.resolver = new AssemblyResourceResolver();
				this.itemPath = itemPath;
				this.ItemName = itemName;
			}

			public override string DefaultExtension
			{
				get { return new CSharp.CSharpCodeProvider().FileExtension; }
			}

			public override string ItemPath
			{
				get { return itemPath; }
			}

			#region IResourceResolver Members

			public string GetResourcePath(string resourceItem)
			{
				return resolver.GetResourcePath(resourceItem);
			}

			public string GetResource(string resourceItem)
			{
				throw new NotImplementedException();
			}

			public override string Namespace
			{
				get
				{
					return "Namespace";
				}
			}

			#endregion
		}

		#endregion
	}
}
