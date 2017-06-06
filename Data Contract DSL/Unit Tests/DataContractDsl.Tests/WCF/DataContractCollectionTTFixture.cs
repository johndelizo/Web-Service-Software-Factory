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
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace DataContractDsl.Tests.WCF
{
	/// <summary>
	/// Summary description for DataContractCollectionTTFixture
	/// </summary>
	[TestClass]
	public class DataContractCollectionTTFixture : DataContractModelFixture
	{
		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContractCollection collectionElement = CreateDefaultDataContractCollectionElement(typeof(Collection<>));
			string content = RunTemplate(collectionElement);
			
			this.EnsureType(ref content, PrimitiveDataElementName1);
			Type generatedType = CompileAndGetType(content);

			Assert.IsTrue(generatedType.IsClass);
			Assert.AreEqual<string>(((WCFDataContractCollection)collectionElement.ObjectExtender).CollectionType.Name, generatedType.BaseType.Name);
			Assert.IsTrue(generatedType.BaseType.FullName.Contains(PrimitiveDataElementName1));
			CollectionDataContractAttribute collectionAttr = TypeAsserter.AssertAttribute<CollectionDataContractAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, collectionAttr.Namespace);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestPrimitiveHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			PrimitiveDataTypeCollection collectionElement = CreateDefaultPrimitiveDataTypeCollection(typeof(Collection<>), PrimitiveDataElementType1);
			string content = RunTemplate(collectionElement);

			Type generatedType = CompileAndGetType(content);

			Assert.IsTrue(generatedType.IsClass);
			Assert.AreEqual<Type>(typeof(Collection<int>), generatedType.BaseType);
			CollectionDataContractAttribute collectionAttr = TypeAsserter.AssertAttribute<CollectionDataContractAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, collectionAttr.Namespace);
		}

		[TestMethod]
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
		}

		protected override string Template
		{
			get
			{
				DataContractCollectionLink link = new DataContractCollectionLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(WCFDataContractCollection); }
		}

		private DataContractCollection CreateDefaultDataContractCollectionElement(Type collectionType)
		{
			DataContractCollection rootElement = new DataContractCollection(Store);
			rootElement.DataContractModel = new DataContractModel(Store);
			rootElement.DataContractModel.ProjectMappingTable = "WCF";
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
			rootElement.DataContractModel.ProjectMappingTable = "WCF";
			rootElement.Name = ElementName;
			rootElement.Namespace = ElementNamespace;
			rootElement.ObjectExtender = AttachDataContractCollectionExtender(rootElement, collectionType);
			rootElement.ItemType = itemType;
			return rootElement;
		}

		private static object AttachDataContractCollectionExtender(DataContractCollectionBase element, Type collectionType)
		{
			WCFDataContractCollection extender = new WCFDataContractCollection();
			extender.CollectionType = collectionType;
			extender.ModelElement = element;
			return extender;
		}
	}
}
