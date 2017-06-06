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
using System.Net.Security;

namespace ServiceContractDsl.Tests.WCF
{
	[TestClass]
	public class XsdMessageContractTTFixture : MessageContractTTBaseFixture
	{
		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\XsdMessageContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateCorrectElementNameInMessageContract()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			XsdMessage rootElement = CreateRoot<XsdMessage>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.IsWrapped = true;
			rootElement.Element = "xsd:\\file.xsd?MyType";
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
			rootElement.ServiceContractModel.SerializerType = SerializerType.DataContractSerializer;

			string content = RunTemplate(rootElement);
			
			EnsureType(ref content, "MyType");
			Type generatedType = CompileAndGetType(content);
			Assert.AreEqual<string>(MessageContractElementName, generatedType.Name);
			Assert.AreEqual<string>(DefaultNamespace, generatedType.Namespace);
			Assert.AreEqual<int>(2, generatedType.GetConstructors().Length);
			MessageContractAttribute messageContract = TypeAsserter.AssertAttribute<MessageContractAttribute>(generatedType);
			Assert.AreEqual<string>(MessageContractElementName, messageContract.WrapperName);
			Assert.IsNotNull(messageContract.WrapperNamespace);
			Assert.IsTrue(messageContract.IsWrapped);
			PropertyInfo property = generatedType.GetProperty("MyType");
			Assert.IsNotNull(property);
			MessageBodyMemberAttribute bodyAttr = TypeAsserter.AssertAttribute<MessageBodyMemberAttribute>(property);
			Assert.AreEqual<string>(messageContract.WrapperNamespace, bodyAttr.Namespace);
			Assert.AreEqual<int>(0, bodyAttr.Order);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\XsdMessageContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateCorrectXmlSerializerAttributes()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			XsdMessage rootElement = CreateRoot<XsdMessage>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.IsWrapped = true;
			rootElement.Element = "xsd:\\file.xsd?MyType";
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
			rootElement.ServiceContractModel.SerializerType = SerializerType.XmlSerializer;
			
			string content = RunTemplate(rootElement);

			EnsureType(ref content, "MyType");
			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertAttribute<XmlSerializerFormatAttribute>(generatedType);
			PropertyInfo property = generatedType.GetProperty("MyType");
			Assert.IsNotNull(property);
			TypeAsserter.AssertAttribute<XmlElementAttribute>(property);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\XsdMessageContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldNotGenerateWithUnwrappedMessage()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			XsdMessage rootElement = CreateRoot<XsdMessage>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.IsWrapped = false;
			rootElement.Element = "xsd:\\file.xsd?MyType";
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
			rootElement.ServiceContractModel.SerializerType = SerializerType.DataContractSerializer;

			string content = RunTemplate(rootElement);

			EnsureType(ref content, "MyType");
			Type generatedType = CompileAndGetType(content);
			MessageContractAttribute messageContract = TypeAsserter.AssertAttribute<MessageContractAttribute>(generatedType);
			Assert.IsFalse(messageContract.IsWrapped);
		}

		protected override string Template
		{
			get
			{
				XsdMessageContractLink link = new XsdMessageContractLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(WCFXsdMessageContract); }
		}		
	}	
}