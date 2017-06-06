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
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.IO;
using System.CodeDom.Compiler;
using System.Globalization;
using Microsoft.Practices.ServiceFactory.Description;
using Microsoft.Practices.VisualStudio.Helper;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.Tests
{
	/// <summary>
	/// Summary description for XmlSchemaCodeGenerationStrategyFixture
	/// </summary>
	[TestClass]
	public class XmlSchemaCodeGenerationStrategyFixture
	{
		[TestMethod]
		public void ShouldInitializeWithDefaultValues()
		{
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();

			Assert.AreEqual<int>(0, strategy.ProjectReferences.Count);
			Assert.AreEqual<int>(0, strategy.AssemblyReferences.Count);
			Assert.AreEqual<int>(0, strategy.Errors.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowOnNullArtifactLink()
		{
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();

			strategy.Generate(null);
		}

		[TestMethod]
		public void ShouldNotGenerateAndGetErrorWithNoSchema()
		{
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(new MyArtifactLink(null, null));

			Assert.AreEqual<int>(0, content.Count);
			Assert.AreEqual<int>(1, strategy.Errors.Count);
		}

		[TestMethod]
		public void ShouldNotGenerateAndGetErrorWithSchemaNotFound()
		{
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(new MyArtifactLink("foo.xsd", "SomeElement"));

			Assert.AreEqual<int>(0, content.Count);
			Assert.AreEqual<int>(1, strategy.Errors.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\InvalidSchema.xsd", "SampleData")]
		public void ShouldGenerateWithErrorsOnInvalidSchema()
		{
			MyArtifactLink link = new MyArtifactLink("SampleData\\InvalidSchema.xsd", "SomeElement");
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(link);

			Assert.AreEqual<int>(0, content.Count);
			Assert.AreEqual<int>(1, strategy.Errors.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\SimpleSchema1.xsd", "SampleData")]
		public void ShouldGenerateWithSimpleSchema()
		{
			MyArtifactLink link = new MyArtifactLink("SampleData\\SimpleSchema1.xsd", "element1");
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(link);

			Assert.AreEqual<int>(3, content.Count);
			Assert.AreEqual<int>(0, strategy.Errors.Count);

			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(JoinContent(content), ((List<string>)strategy.AssemblyReferences).ToArray());
			Assert.AreEqual<int>(3, results.CompiledAssembly.GetTypes().Length);
			TypeAsserter.AssertAttribute<DataContractAttribute>(results.CompiledAssembly.GetType("element1"));
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\SimpleSchema1.xsd", "SampleData")]
		public void ShouldGenerateWithXmlSerializerImporterFromAttribute()
		{
			MyArtifactLinkWithXmlImporterFromAttribute link = new MyArtifactLinkWithXmlImporterFromAttribute("SampleData\\SimpleSchema1.xsd", "element1");
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(link);

			Assert.AreEqual<int>(3, content.Count);
			Assert.AreEqual<int>(0, strategy.Errors.Count);
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(JoinContent(content), ((List<string>)strategy.AssemblyReferences).ToArray());
			TypeAsserter.AssertAttribute<XmlRootAttribute>(results.CompiledAssembly.GetType("element1"));
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\SimpleSchema1.xsd", "SampleData")]
		public void ShouldGenerateWithXmlSerializerImporterFromData()
		{
			MyArtifactLinkWithXmlImporterFromData link = new MyArtifactLinkWithXmlImporterFromData("SampleData\\SimpleSchema1.xsd", "element1");
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(link);

			Assert.AreEqual<int>(3, content.Count);
			Assert.AreEqual<int>(0, strategy.Errors.Count);
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(JoinContent(content), ((List<string>)strategy.AssemblyReferences).ToArray());
			TypeAsserter.AssertAttribute<XmlRootAttribute>(results.CompiledAssembly.GetType("element1"));
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\CircularRefSchema.xsd", "SampleData")]
		public void ShouldHandleCircularRefSchema()
		{
			MyArtifactLinkWithXmlImporterFromData link = new MyArtifactLinkWithXmlImporterFromData("SampleData\\CircularRefSchema.xsd", "element1");
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(link);

			Assert.AreEqual<int>(3, content.Count);
			Assert.AreEqual<int>(0, strategy.Errors.Count);
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(JoinContent(content), ((List<string>)strategy.AssemblyReferences).ToArray());
			TypeAsserter.AssertAttribute<XmlRootAttribute>(results.CompiledAssembly.GetType("element1"));
			TypeAsserter.AssertAttribute<XmlTypeAttribute>(results.CompiledAssembly.GetType("complexType1"));
			TypeAsserter.AssertAttribute<XmlTypeAttribute>(results.CompiledAssembly.GetType("complexType2"));
		}
		
		[TestMethod]
		[DeploymentItem(@"SampleData\TopElementNoType.xsd", "SampleData")]
		public void ShouldNotGenerateAndGetErrorWithTopElementsWithoutType()
		{
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(new MyArtifactLink("SampleData\\TopElementNoType.xsd", "otherElement"));

			Assert.AreEqual<int>(0, content.Count);
			Assert.AreEqual<int>(1, strategy.Errors.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\BaseTypes.xsd", "SampleData")]
		public void ShouldGenerateTypeWithBaseTypes()
		{
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(new MyArtifactLink("SampleData\\BaseTypes.xsd", "GetLandmarkPointsByRectResponse"));

			Assert.AreEqual<int>(15, content.Count);
			Assert.AreEqual<int>(0, strategy.Errors.Count);
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(JoinContent(content), ((List<string>)strategy.AssemblyReferences).ToArray());
			Assert.IsNotNull(results.CompiledAssembly.GetType("GetLandmarkPointsByRectResponse", false, false));
			Assert.IsNotNull(results.CompiledAssembly.GetType("LandmarkPoint", false, false));
			Assert.IsNotNull(results.CompiledAssembly.GetType("LandmarkBase", false, false));
			Assert.IsNotNull(results.CompiledAssembly.GetType("ShapeType", false, false));
			Assert.IsNotNull(results.CompiledAssembly.GetType("LonLatPt", false, false));
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\TypeRefsInAttributes.wsdl", "SampleData")]
		public void ShouldGenerateTypesRefsFromAttributes()
		{
			MyArtifactLink link = new MyArtifactLink("SampleData\\TypeRefsInAttributes.wsdl", "AttachmentAddRq");
			Utility.SetData(link, true, XmlSchemaCodeGenerationStrategy.UseXmlSerializerDataKey);
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(link);

			Assert.AreEqual<int>(69, content.Count);
			Assert.AreEqual<int>(0, strategy.Errors.Count);

			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(JoinContent(content), ((List<string>)strategy.AssemblyReferences).ToArray());
			Assert.AreEqual<int>(69, results.CompiledAssembly.GetTypes().Length);
			Assert.IsNotNull(results.CompiledAssembly.GetType("AddressType", false, false));
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\OneXmlRootAttribute.xsd", "SampleData")]
		public void ShouldGenerateOneXmlRootAttribute()
		{
			MyArtifactLink link = new MyArtifactLink("SampleData\\OneXmlRootAttribute.xsd", "ImplementsAbstractCT");
			Utility.SetData(link, true, XmlSchemaCodeGenerationStrategy.UseXmlSerializerDataKey);
			XmlSchemaCodeGenerationStrategy strategy = new XmlSchemaCodeGenerationStrategy();
			CodeGenerationResults content = strategy.Generate(link);

			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(JoinContent(content), ((List<string>)strategy.AssemblyReferences).ToArray());

			TypeAsserter.AssertAttribute<XmlRootAttribute>(results.CompiledAssembly.GetType("ImplementsAbstractCT"));
			Type type = results.CompiledAssembly.GetType("MyAbstractCT");
			Assert.AreEqual<int>(0, type.GetCustomAttributes(typeof(XmlRootAttribute), true).Length);
		}

		#region Private Functions

		private string[] JoinContent(CodeGenerationResults content)
		{
			string[] result = new string[content.Values.Count];
			content.Values.CopyTo(result, 0);
			return result;
		}

		private static ProjectNode GetTestProjectNode()
		{
			return GetTestProjectNode(EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp);
		}

		private static ProjectNode GetTestProjectNode(string language)
		{
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution vsSolution = new MockVsSolution(root);
			MockVSHierarchy project = new MockVSHierarchy("Project.project");

			MockEnvDTEProject mockEnvDteProject = project.ExtObject as MockEnvDTEProject;
			if (mockEnvDteProject != null)
			{
				mockEnvDteProject.SetCodeModel(new MockCodeModel(language));
			}
			root.AddProject(project);

			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);

			return projectNode;
		}

		#endregion

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
				if (!string.IsNullOrEmpty(itemPath) &&
					!string.IsNullOrEmpty(itemName))
				{
					Utility.SetData(this, new XmlSchemaElementMoniker(itemPath, this.ItemName).ToString(), XmlSchemaCodeGenerationStrategy.ElementDataKey);
				}
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

			#endregion
		}

		[XmlSchemaCodeGeneration(true)]
		class MyArtifactLinkWithXmlImporterFromAttribute : MyArtifactLink
		{
			public MyArtifactLinkWithXmlImporterFromAttribute(string itemPath, string itemName)
				: base(itemPath, itemName)
			{
			}
		}

		[XmlSchemaCodeGeneration(true)]
		class MyArtifactLinkWithXmlImporterFromData : MyArtifactLink
		{
			public MyArtifactLinkWithXmlImporterFromData(string itemPath, string itemName)
				: base(itemPath, itemName)
			{
				Utility.SetData(this, true, XmlSchemaCodeGenerationStrategy.UseXmlSerializerDataKey);
			}
		}

		#endregion
	}
}
