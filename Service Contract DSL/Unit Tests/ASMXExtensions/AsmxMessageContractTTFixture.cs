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
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx.CodeGeneration;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace ServiceContractDsl.Tests.AsmxExtensions
{

	[TestClass]
	public class AsmxMessageContractTTFixture : MessageContractTTBaseFixture
	{
		public AsmxMessageContractTTFixture()
		{
		}

		[TestMethod]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\MessageContract.tt", @"TextTemplates\ASMX\CS")]
		public void ShouldGeneratePartialClass()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);

			rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";

			string content = RunTemplate(rootElement);
			Assert.IsTrue(content.Contains("public partial class " + MessageContractElementName));		
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\MessageContract.tt", @"TextTemplates\ASMX\CS")]
		public void ShouldCompileWithValidXmlAttribute()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);

			rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			XmlRootAttribute xmlRootAttribute = TypeAsserter.AssertAttribute<XmlRootAttribute>(generatedType);
			Assert.AreEqual<string>(MessageContractElementName, xmlRootAttribute.ElementName);
			Assert.AreEqual<string>(MessageContractElementNamespace, xmlRootAttribute.Namespace);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\MessageContract.tt", @"TextTemplates\ASMX\CS")]
		public void ShouldCompileWithValidClassName()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);

			rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			Assert.AreEqual<string>(MessageContractElementName, generatedType.Name);
			Assert.AreEqual<string>(DefaultNamespace, generatedType.Namespace);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\MessageContract.tt", @"TextTemplates\ASMX\CS")]
		public void ShouldGenerateStringPrimitiveMessagePart()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";

			PrimitiveMessagePart primitivePart = new PrimitiveMessagePart(Store);
			primitivePart.Name = "TestProperty";
			primitivePart.Type = typeof(System.String).ToString();

			rootElement.MessageParts.Add(primitivePart);

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			PropertyInfo propInfo = generatedType.GetProperty("TestProperty");
			Assert.IsNotNull(propInfo);
			Assert.AreEqual<string>(propInfo.PropertyType.ToString(), primitivePart.Type);
		}

        [TestMethod]
        [DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\MessageContract.tt", @"TextTemplates\ASMX\CS")]
        public void ShouldGenerateStringCollectionPrimitiveMessagePart()
        {
            ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
            rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";

            PrimitiveMessagePart primitivePart = new PrimitiveMessagePart(Store);
            primitivePart.Name = "TestProperty";
            primitivePart.Type = typeof(System.String).ToString();
            primitivePart.IsCollection = true;

            rootElement.MessageParts.Add(primitivePart);

            string content = RunTemplate(rootElement);

            Type generatedType = CompileAndGetType(content);
            PropertyInfo propInfo = generatedType.GetProperty("TestProperty");
            Assert.IsNotNull(propInfo);
            Type expectedType = typeof(List<String>);
            Assert.AreEqual<Type>(propInfo.PropertyType, expectedType);
        }

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\MessageContract.tt", @"TextTemplates\ASMX\CS")]
		public void ShouldGenerateDataContractMessagePart()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";

			DataContractMessagePart part = new DataContractMessagePart(Store);
			part.Name = "TestProperty";
			//part.Type = @"mel://Microsoft.Practices.ServiceFactory.DataContracts\DataContract\String@Project1.model\dc.datacontract";

			rootElement.MessageParts.Add(part);

			string content = RunTemplateWithDIS(rootElement);

			//StringAssert.Contains(content, "FooDC TestProperty");
            StringAssert.Contains(content, MessageContractElementName);
            StringAssert.Contains(content, MessageContractElementNamespace);
		}

		#region private
		private Store dataContractStore = null;
		private Store DataContractStore
		{
			get
			{
				if (dataContractStore == null)
				{
					dataContractStore = new Store(ServiceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));
				}
				return dataContractStore;
			}
		}

		protected override ModelElement ResolveModelElement(string instanceNamespace)
		{
			using (Transaction transaction = DataContractStore.TransactionManager.BeginTransaction())
			{
				DataContract dc = new DataContract(DataContractStore);
				dc.DataContractModel = new DataContractModel(DataContractStore);
				dc.DataContractModel.ProjectMappingTable = "ASMX";
				dc.Name = "FooDC";
				AsmxDataContract extender = new AsmxDataContract();
				dc.ObjectExtender = extender;
				extender.ModelElement = dc;
				transaction.Commit();
				return dc;
			}
		}
	

		protected override string Template
		{
			get
			{
				AsmxMessageContractLink link = new AsmxMessageContractLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(AsmxMessageContract); }
		}

		#endregion

	}
}
