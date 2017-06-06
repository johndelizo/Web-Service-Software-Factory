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
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.Modeling.Common;

namespace DataContractDsl.Tests.ASMX
{
	/// <summary>
	/// Summary description for AsmxDataContractCollectionFixture
	/// </summary>
	[TestClass]
	public class AsmxDataContractCollectionTTFixture : DataContractModelFixture
	{
		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		public void ShouldReturnAsmxDataContractCollectionArtifactLink()
		{
			DataContractCollection dcCollection = new DataContractCollection(Store);
			dcCollection.DataContractModel = new DataContractModel(Store);
			AsmxDataContractCollection asmxDCCollection = new AsmxDataContractCollection();
			asmxDCCollection.ModelElement = dcCollection;

			Assert.IsInstanceOfType(asmxDCCollection.ArtifactLink, typeof(AsmxDataContractCollectionLink));
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContractCollection collectionElement = 
				CreateDefaultDataContractCollectionElement(typeof(Collection<>));
			string content = RunTemplate(collectionElement);

			this.EnsureType(ref content, PrimitiveDataElementName1);
			Type generatedType = CompileAndGetType(content);

			Assert.IsTrue(generatedType.IsClass);
			Assert.AreEqual<string>(ElementName, generatedType.Name);
			XmlRootAttribute xmlRootAttr = TypeAsserter.AssertAttribute<XmlRootAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlRootAttr.Namespace);
			Assert.IsFalse(xmlRootAttr.IsNullable);
			XmlTypeAttribute xmlTypeAttr = TypeAsserter.AssertAttribute<XmlTypeAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlTypeAttr.Namespace);
			
			Assert.AreEqual<string>(((AsmxDataContractCollection)collectionElement.ObjectExtender).CollectionType.Name, generatedType.BaseType.Name);
			Assert.IsTrue(generatedType.BaseType.FullName.Contains(PrimitiveDataElementName1));
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestPrimitiveHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			PrimitiveDataTypeCollection collectionElement =
				CreateDefaultPrimitiveDataTypeCollection(typeof(Collection<>), PrimitiveDataElementType1);
			string content = RunTemplate(collectionElement);

			Type generatedType = CompileAndGetType(content);

			Assert.IsTrue(generatedType.IsClass);
			Assert.AreEqual<Type>(typeof(Collection<int>), generatedType.BaseType);
			XmlRootAttribute xmlRootAttr = TypeAsserter.AssertAttribute<XmlRootAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlRootAttr.Namespace);
			Assert.IsFalse(xmlRootAttr.IsNullable);
			XmlTypeAttribute xmlTypeAttr = TypeAsserter.AssertAttribute<XmlTypeAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlTypeAttr.Namespace);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestPrimitiveHeaderGenerationWithArrayCollectionType()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			PrimitiveDataTypeCollection collectionElement = 
				CreateDefaultPrimitiveDataTypeCollection(typeof(Array), PrimitiveDataElementType1);
			TemplateResult result = RunTemplateWithErrors(collectionElement);

			// should have a warning
			Assert.AreEqual<int>(1, result.Errors.Length);

			string content = result.ContentResults;

			Type generatedType = CompileAndGetType(content);

			Assert.IsTrue(generatedType.IsClass);
			Assert.AreEqual<Type>(typeof(Collection<int>), generatedType.BaseType);
			XmlRootAttribute xmlRootAttr = TypeAsserter.AssertAttribute<XmlRootAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlRootAttr.Namespace);
			Assert.IsFalse(xmlRootAttr.IsNullable);
			XmlTypeAttribute xmlTypeAttr = TypeAsserter.AssertAttribute<XmlTypeAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlTypeAttr.Namespace);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestPrimitiveHeaderGenerationWithDictionaryCollectionType()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			PrimitiveDataTypeCollection collectionElement =
				CreateDefaultPrimitiveDataTypeCollection(typeof(Dictionary<,>), PrimitiveDataElementType1);
			TemplateResult result = RunTemplateWithErrors(collectionElement);

			// should have a warning
			Assert.AreEqual<int>(1, result.Errors.Length);

			string content = result.ContentResults;

			Type generatedType = CompileAndGetType(content);

			Assert.IsTrue(generatedType.IsClass);
			Assert.AreEqual<Type>(typeof(Collection<int>), generatedType.BaseType);
			XmlRootAttribute xmlRootAttr = TypeAsserter.AssertAttribute<XmlRootAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlRootAttr.Namespace);
			Assert.IsFalse(xmlRootAttr.IsNullable);
			XmlTypeAttribute xmlTypeAttr = TypeAsserter.AssertAttribute<XmlTypeAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlTypeAttr.Namespace);
		}

		protected override string Template
		{
			get
			{
				AsmxDataContractCollectionLink link = new AsmxDataContractCollectionLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(AsmxDataContractCollection); }
		}

		private DataContractCollection CreateDefaultDataContractCollectionElement(Type collectionType)
		{
			DataContractCollection rootElement = new DataContractCollection(Store);
			rootElement.DataContractModel = new DataContractModel(Store);
			rootElement.DataContractModel.ProjectMappingTable = "ASMX";
			rootElement.Name = ElementName;
			rootElement.Namespace = ElementNamespace;
			rootElement.ObjectExtender = AttachDataContractCollectionExtender(rootElement, collectionType); 

			DataContract dce = new DataContract(Store);
			dce.Name = PrimitiveDataElementName1;
			rootElement.DataContract = dce;
			return rootElement;
		}

		private PrimitiveDataTypeCollection CreateDefaultPrimitiveDataTypeCollection(Type collectionType,
			string itemType)
		{
			PrimitiveDataTypeCollection rootElement = new PrimitiveDataTypeCollection(Store);
			rootElement.DataContractModel = new DataContractModel(Store);
			rootElement.DataContractModel.ProjectMappingTable = "ASMX";
			rootElement.Name = ElementName;
			rootElement.Namespace = ElementNamespace;
			rootElement.ObjectExtender = AttachDataContractCollectionExtender(rootElement, collectionType);
			rootElement.ItemType = itemType;
			return rootElement;
		}

		private static object AttachDataContractCollectionExtender(DataContractCollectionBase element, Type collectionType)
		{
			AsmxDataContractCollection extender = new AsmxDataContractCollection();
			extender.CollectionType = collectionType;
			extender.ModelElement = element;
			return extender;
		}
	}
}
