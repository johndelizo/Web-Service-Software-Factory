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
using System.Xml.Serialization;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace DataContractDsl.Tests.ASMX
{
	/// <summary>
	/// Summary description for AsmxDataContractEnumTTFixture
	/// </summary>
	[TestClass]
	public class AsmxDataContractEnumTTFixture : DataContractModelFixture
	{
		private const string EnumElement1Name = "EnumElement1";
		private const string EnumElement1Value = "EnumElement1Value";

		private const string EnumElement2Name = "EnumElement2";
		private const string EnumElement2Value = "EnumElement2Value";


		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		public void ShouldReturnAsmxDataContractEnumLink()
		{
			DataContractEnum enumElement = new DataContractEnum(Store);
			enumElement.DataContractModel = new DataContractModel(Store);
			AsmxDataContractEnum asmxDCEnum = new AsmxDataContractEnum();
			asmxDCEnum.ModelElement = enumElement;

			Assert.IsInstanceOfType(asmxDCEnum.ArtifactLink, typeof(AsmxDataContractEnumLink));
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContractEnum rootElement = CreateDefaultDataContractEnum();
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			Assert.IsTrue(generatedType.IsEnum);
			Assert.AreEqual<string>(ElementName, generatedType.Name);
			XmlRootAttribute xmlRootAttr = TypeAsserter.AssertAttribute<XmlRootAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlRootAttr.Namespace);
			Assert.IsFalse(xmlRootAttr.IsNullable);
			XmlTypeAttribute xmlTypeAttr = TypeAsserter.AssertAttribute<XmlTypeAttribute>(generatedType);
			Assert.AreEqual<string>(ElementNamespace, xmlTypeAttr.Namespace);
		}

		[TestMethod]
		[DeploymentItem("TextTemplates", "TextTemplates")]
		[DeploymentItem("ProjectMapping.DataContractDsl.Tests.xml")]
		public void TestEnumDataElementsGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.DataContractDsl.Tests.xml");

			DataContractEnum rootElement = CreateDefaultDataContractEnum();
			rootElement.EnumNamedValues.AddRange(LoadEnumDataElements());
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertExistPublicField(EnumElement1Name, generatedType);

            XmlEnumAttribute attrib = TypeAsserter.AssertAttribute<XmlEnumAttribute>(generatedType.GetField(EnumElement1Name));
			Assert.AreEqual<string>(EnumElement1Value, attrib.Name);

			TypeAsserter.AssertExistPublicField(EnumElement2Name, generatedType);
            attrib = TypeAsserter.AssertAttribute<XmlEnumAttribute>(generatedType.GetField(EnumElement2Name));
			Assert.AreEqual<string>(EnumElement2Value, attrib.Name);
		}

		protected override string Template
		{
			get
			{
				AsmxDataContractEnumLink link = new AsmxDataContractEnumLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(AsmxDataContractEnum); }
		}

		private DataContractEnum CreateDefaultDataContractEnum()
		{
			DataContractEnum rootElement = new DataContractEnum(Store);
			rootElement.DataContractModel = new DataContractModel(Store);
			rootElement.DataContractModel.ProjectMappingTable = "ASMX";
			rootElement.Name = ElementName;
			rootElement.Namespace = ElementNamespace;
			return rootElement;
		}

		private List<EnumNamedValue> LoadEnumDataElements()
		{
			List<EnumNamedValue> elements = new List<EnumNamedValue>();

			EnumNamedValue element1 = new EnumNamedValue(Store);
			element1.Name = EnumElement1Name;
			element1.Value = EnumElement1Value;
			elements.Add(element1);

			EnumNamedValue element2 = new EnumNamedValue(Store);
			element2.Name = EnumElement2Name;
			element2.Value = EnumElement2Value;
			elements.Add(element2);

			return elements;
		}
	}
}
