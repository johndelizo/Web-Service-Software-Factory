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
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.ServiceModel;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf.CodeGeneration;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;
using System.Xml.Serialization;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace ServiceContractDsl.Tests.WCF
{
	[TestClass]
	public class MessageContractTTFixture : MessageContractTTBaseFixture
	{
		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\MessageContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateCorrectElementNameInMessageContract()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			MessageContractAttribute messageContract = TypeAsserter.AssertAttribute<MessageContractAttribute>(generatedType);
			Assert.AreEqual<string>(MessageContractElementName, generatedType.Name);
			Assert.AreEqual<string>(DefaultNamespace, generatedType.Namespace);
			Assert.IsFalse(messageContract.HasProtectionLevel);
			Assert.IsNull(messageContract.WrapperName);
			Assert.IsNull(messageContract.WrapperNamespace);
			Assert.IsFalse(messageContract.IsWrapped);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\MessageContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateStringPrimitiveMessagePart()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
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
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
        [DeploymentItem(@"TextTemplates\WCF\CS\MessageContract.tt", @"TextTemplates\WCF\CS")]
        public void ShouldGenerateStringCollectionPrimitiveMessagePart()
        {
            ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
            rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
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
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\MessageContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateAllPrimitiveMessageParts()
		{
			Type[] candidateTypes = new Type[]{ 
										typeof(System.String), typeof(System.Int16), typeof(System.Int32),
										typeof(System.Int64), typeof(System.Double), typeof(System.Single)
												};
			string propertyNameFormat = "TestProperty{0}";
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
			foreach (Type partType in candidateTypes)
			{
				PrimitiveMessagePart primitivePart = new PrimitiveMessagePart(Store);
				primitivePart.Name = string.Format(propertyNameFormat, partType.ToString().Replace('.','_'));
				primitivePart.Type = partType.ToString();
				rootElement.MessageParts.Add(primitivePart);
			}

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			foreach (Type partType in candidateTypes)
			{
				string propertyName = string.Format(propertyNameFormat, partType.ToString().Replace('.','_'));
				PropertyInfo propInfo = generatedType.GetProperty(propertyName);
				Assert.IsNotNull(propInfo, string.Format("Could not locate {0} property in Message Contract", propertyName));
				Assert.AreEqual<string>(partType.ToString(), propInfo.PropertyType.ToString());
			}
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\MessageContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldErrorWhenPrimitivePartHasEmptyType()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
			PrimitiveMessagePart primitivePart = new PrimitiveMessagePart(Store);
			primitivePart.Name = "TestProperty";			
			primitivePart.Type = string.Empty;
			rootElement.MessageParts.Add(primitivePart);
			
			TemplateResult result = RunTemplateWithErrors(rootElement);

			Assert.AreEqual<int>(1, result.Errors.Length);
			//StringAssert.Contains(result.Errors[0], "Cannot generate Message Contract due to invalid type");
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\MessageContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateCorrectXmlSerializerAttributes()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			Message rootElement = CreateRoot<Message>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
			PrimitiveMessagePart primitivePart = new PrimitiveMessagePart(Store);
			primitivePart.Name = "TestProperty";
			primitivePart.Type = typeof(System.String).ToString();
			rootElement.MessageParts.Add(primitivePart);
			WCFMessageContract wcfMc = new WCFMessageContract(true);
			wcfMc.ModelElement = rootElement;
			rootElement.ObjectExtender = wcfMc;
			
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertAttribute<XmlSerializerFormatAttribute>(generatedType);
			PropertyInfo property = generatedType.GetProperty(primitivePart.Name);
			TypeAsserter.AssertAttribute<XmlElementAttribute>(property);
		}

		protected override string Template
		{
			get
			{
				MessageContractLink link = new MessageContractLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(WCFMessageContract); }
		}		
	}	
}
