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
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace DataContractDsl.Tests.ASMX
{
	/// <summary>
	/// Summary description for DataContractTTFixture
	/// </summary>
	[TestClass]
	public class DataContractTTFixture : DataContractModelFixture
	{
		const string SerializableAttribute = "[Serializable]";
		const string DataContractLinkedElementName = "MyLinkedDataContractElement";
		const string DataContractLinkedElementType = "DataContractLinkedElementType";

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContract rootElement = CreateDefaultDataContract(); 
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			Assert.AreEqual<string>(ElementName, generatedType.Name);
			SerializableAttribute serializableAttr = TypeAsserter.AssertAttribute<System.SerializableAttribute>(generatedType);
			Assert.IsNotNull(serializableAttr);
			XmlRootAttribute xmlRootAttr = TypeAsserter.AssertAttribute<XmlRootAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlRootAttr.Namespace);
			Assert.IsFalse(xmlRootAttr.IsNullable);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveTypesGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.AddRange(LoadPrimitiveDataElements(null, true, false));
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertField(PrimitiveDataElementType1, PrimitiveDataElementName1, generatedType);
			TypeAsserter.AssertField(PrimitiveDataElementType2, PrimitiveDataElementName2, generatedType);
			XmlElementAttribute xmlElementAttr = TypeAsserter.AssertAttribute<XmlElementAttribute>(generatedType.GetProperty(PrimitiveDataElementName1));
			Assert.AreEqual<string>(PrimitiveDataElementName1, xmlElementAttr.ElementName);
			Assert.IsFalse(xmlElementAttr.IsNullable);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveNoDataMembersGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.AddRange(LoadPrimitiveDataElements(null, false, false));
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertField(PrimitiveDataElementType1, PrimitiveDataElementName1, generatedType);
			TypeAsserter.AssertField(PrimitiveDataElementType2, PrimitiveDataElementName2, generatedType);
			XmlIgnoreAttribute xmlIgnoreAttr = TypeAsserter.AssertAttribute<XmlIgnoreAttribute>(generatedType.GetProperty(PrimitiveDataElementName1));
			Assert.IsNotNull(xmlIgnoreAttr);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveNullableTypeGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.Add(LoadPrimitiveDataElement(PrimitiveDataElementName1, PrimitiveDataElementType1, null, true, true));
			string content = RunTemplate(rootElement);
		
			EnsureType(ref content, DataContractLinkedElementType);
			Type generatedType = CompileAndGetType(content);
			XmlElementAttribute xmlElementAttr = TypeAsserter.AssertAttribute<XmlElementAttribute>(generatedType.GetProperty(PrimitiveDataElementName1));
			Assert.IsTrue(xmlElementAttr.IsNullable);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveCollectionTypesGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.AddRange(LoadPrimitiveDataElements(typeof(List<>), true, false));
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertField(typeof(List<int>), PrimitiveDataElementName1, generatedType);
			TypeAsserter.AssertField(typeof(List<string>), PrimitiveDataElementName2, generatedType);
			XmlArrayItemAttribute xmlArrayAttr = TypeAsserter.AssertAttribute<XmlArrayItemAttribute>(generatedType.GetProperty(PrimitiveDataElementName1));
			Assert.AreEqual<string>(PrimitiveDataElementName1, xmlArrayAttr.ElementName);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveInvalidCollectionTypesGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.AddRange(LoadPrimitiveDataElements(typeof(Dictionary<,>), true, false));
			TemplateResult result = RunTemplateWithErrors(rootElement);

			// should have a warning
			Assert.AreEqual<int>(4, result.Errors.Length);

			string content = result.ContentResults;

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertField(typeof(Collection<int>), PrimitiveDataElementName1, generatedType);
			TypeAsserter.AssertField(typeof(Collection<string>), PrimitiveDataElementName2, generatedType);
			XmlArrayItemAttribute xmlArrayAttr = TypeAsserter.AssertAttribute<XmlArrayItemAttribute>(generatedType.GetProperty(PrimitiveDataElementName1));
			Assert.AreEqual<string>(PrimitiveDataElementName1, xmlArrayAttr.ElementName);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveCollectionNullableTypesGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.AddRange(LoadPrimitiveDataElements(typeof(List<>), true, true));
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertField(typeof(List<Nullable<int>>), PrimitiveDataElementName1, generatedType);
			TypeAsserter.AssertField(typeof(List<string>), PrimitiveDataElementName2, generatedType);
			XmlArrayItemAttribute xmlArrayAttr = TypeAsserter.AssertAttribute<XmlArrayItemAttribute>(generatedType.GetProperty(PrimitiveDataElementName1));
			Assert.AreEqual<string>(PrimitiveDataElementName1, xmlArrayAttr.ElementName);
			Assert.IsTrue(xmlArrayAttr.IsNullable);
		}

		protected override string Template
		{
			get
			{
				AsmxDataContractLink link = new AsmxDataContractLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(AsmxDataContract); }
		}

		#region Private methods

		private DataContract CreateDefaultDataContract()
		{
			DataContract rootElement = new DataContract(Store);
			rootElement.DataContractModel = new DataContractModel(Store);
			rootElement.DataContractModel.ProjectMappingTable = "ASMX";
			rootElement.Name = ElementName;
			rootElement.Namespace = ElementNamespace;
			return rootElement;
		}

		private List<DataMember> LoadPrimitiveDataElements(Type collectionType, bool isDataMember, bool isNullable)
		{
			List<DataMember> dataElements = new List<DataMember>();
			dataElements.Add(
				LoadPrimitiveDataElement(PrimitiveDataElementName1, PrimitiveDataElementType1, collectionType, isDataMember, isNullable));

			dataElements.Add(
				LoadPrimitiveDataElement(PrimitiveDataElementName2, PrimitiveDataElementType2, collectionType, isDataMember, isNullable));

			return dataElements;
		}

		private List<DataMember> LoadDataContractDataElements(DataContract sourceElement, Multiplicity multiplicity)
		{
			DataContract targetElement = new DataContract(Store);
			targetElement.Name = DataContractLinkedElementType;
			DataContractBaseCanBeContainedOnContracts link = new DataContractBaseCanBeContainedOnContracts(sourceElement, targetElement);

			List<DataMember> dataElements = new List<DataMember>();
			ModelElementReference element1 = new ModelElementReference(Store);
			element1.Name = DataContractLinkedElementName;
			element1.Type = DataContractLinkedElementType;
			element1.SetLinkedElement(link.Id);

			dataElements.Add(element1);
			return dataElements;
		}

		private PrimitiveDataType LoadDefaultPrimitiveDataElement()
		{
			return LoadPrimitiveDataElement(
				PrimitiveDataElementName1, PrimitiveDataElementType1, null, true, false);
		}

		private PrimitiveDataType LoadPrimitiveDataElement(
			string name, string type, Type collectionType, bool isDataMember, bool isNullable)
		{
			PrimitiveDataType element = new PrimitiveDataType(Store);
			element.Name = name;
			element.Type = type;
			element.CollectionType = collectionType;
			element.IsNullable = isNullable;
			element.IsDataMember = isDataMember;
			return element;
		}

		#endregion
	}
}
