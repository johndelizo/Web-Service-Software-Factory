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
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Reflection;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace DataContractDsl.Tests.WCF
{
	/// <summary>
	/// Summary description for DataContractTTFixture
	/// </summary>
	[TestClass]
	public class DataContractTTFixture : DataContractModelFixture
	{
		const string DataMemberAttribute = "[WcfSerialization::DataMember(";
		const string DataContractLinkedElementName = "MyLinkedDataContractElement";
		const string DataContractLinkedElementType = "DataContractLinkedElementType";

		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");
			DataContract rootElement = CreateDefaultDataContract(); 
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);

			Assert.AreEqual<string>(ElementName, generatedType.Name);
			DataContractAttribute dataContract = TypeAsserter.AssertAttribute<DataContractAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, dataContract.Namespace);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveTypesGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");
			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.AddRange(LoadPrimitiveDataElements(false, true));
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertField(PrimitiveDataElementType1, PrimitiveDataElementName1, generatedType);
			TypeAsserter.AssertField(PrimitiveDataElementType2, PrimitiveDataElementName2, generatedType);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveArrayTypesGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");			
			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.AddRange(LoadPrimitiveDataElements(true, true));
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertField(typeof(List<int>), PrimitiveDataElementName1, generatedType);
			TypeAsserter.AssertField(typeof(List<string>), PrimitiveDataElementName2, generatedType);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveNoDataMembersGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");
			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.AddRange(LoadPrimitiveDataElements(false, false));
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertField(PrimitiveDataElementType1, PrimitiveDataElementName1, generatedType);
			TypeAsserter.AssertField(PrimitiveDataElementType2, PrimitiveDataElementName2, generatedType);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestWCFDataElementPropertiesGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");
			DataContract rootElement = CreateDefaultDataContract();
			WCFDataElement dataElement = new WCFDataElement();
			dataElement.IsRequired = true;
			dataElement.Order = 1;
			PrimitiveDataType element = LoadDefaultPrimitiveDataElement();
			element.ObjectExtender = dataElement;
			rootElement.DataMembers.Add(element);
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			DataMemberAttribute dataMember = TypeAsserter.AssertAttribute<DataMemberAttribute>(generatedType.GetProperty(element.Name));
			Assert.IsTrue(dataMember.IsRequired);
			Assert.AreEqual<int>(1, dataMember.Order);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveNullableTypeGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");
			DataContract rootElement = CreateDefaultDataContract();
			rootElement.DataMembers.Add(LoadPrimitiveDataElement(PrimitiveDataElementName1, PrimitiveDataElementType1, false, true, true));
			string content = RunTemplate(rootElement);

			Assert.IsTrue(content.Contains("private System.Nullable<int>"));
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestMembersWithPrimitiveEnumTypeGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");
			DataContract rootElement = CreateDefaultDataContract();
			DataContractEnum enumElement = new DataContractEnum(Store);
			enumElement.Name = PrimitiveDataElementName1;
			rootElement.DataContractModel.Contracts.Add(enumElement);
			rootElement.DataMembers.AddRange(LoadLinkedElements(enumElement));
			string content = RunTemplate(rootElement);
			
			this.EnsureType(ref content, PrimitiveDataElementName1);
			Type generatedType = CompileAndGetType(content);
			KnownTypeAttribute knownTypeAttr = TypeAsserter.AssertAttribute<KnownTypeAttribute>(generatedType);
			
			Assert.AreEqual<string>(PrimitiveDataElementName1, knownTypeAttr.Type.Name);
		}

		protected override string Template
		{
			get
			{
				DataContractLink link = new DataContractLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(WCFDataContract); }
		}

		#region Private methods

		private DataContract CreateDefaultDataContract()
		{
			DataContract rootElement = new DataContract(Store);
			rootElement.DataContractModel = new DataContractModel(Store);
			rootElement.DataContractModel.ProjectMappingTable = "WCF";
			rootElement.Name = ElementName;
			rootElement.Namespace = ElementNamespace;
			return rootElement;
		}

		private List<DataMember> LoadPrimitiveDataElements(bool isArray, bool isDataMember)
		{
			List<DataMember> dataElements = new List<DataMember>();
			dataElements.Add(
				LoadPrimitiveDataElement(PrimitiveDataElementName1, PrimitiveDataElementType1, isArray, isDataMember, false));

			dataElements.Add(
				LoadPrimitiveDataElement(PrimitiveDataElementName2, PrimitiveDataElementType2, isArray, isDataMember, false));

			return dataElements;
		}

		private List<DataMember> LoadLinkedElements(DataContractBase sourceElement)
		{
			DataContract targetElement = new DataContract(Store);
			targetElement.Name = DataContractLinkedElementType;
			DataContractBaseCanBeContainedOnContracts link = new DataContractBaseCanBeContainedOnContracts(sourceElement, targetElement);

			List<DataMember> dataElements = new List<DataMember>();
			ModelElementReference element1 = new ModelElementReference(Store);
			element1.Name = sourceElement.Name;
			element1.Type = sourceElement.Name;
			element1.SetLinkedElement(link.Id);

			dataElements.Add(element1);
			return dataElements;
		}

		private PrimitiveDataType LoadDefaultPrimitiveDataElement()
		{
			return LoadPrimitiveDataElement(
				PrimitiveDataElementName1, PrimitiveDataElementType1, false, true, false);
		}

		private PrimitiveDataType LoadPrimitiveDataElement(
			string name, string type, bool isArray, bool isDataMember, bool isNullable)
		{
			PrimitiveDataType element = new PrimitiveDataType(Store);
			element.Name = name;
			element.Type = type;
			element.CollectionType = (isArray ? typeof(List<>) : null);
			element.IsNullable = isNullable;
			element.IsDataMember = isDataMember;
			return element;
		}

		#endregion
	}
}
