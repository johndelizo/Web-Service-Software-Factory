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
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx;
using Microsoft.Practices.UnitTestLibrary.Utilities;

namespace ServiceContractDsl.Tests.ASMX
{
	[TestClass]
	public class XsdMessageContractTTFixture : MessageContractTTBaseFixture
	{
		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\XsdMessageContract.tt", @"TextTemplates\ASMX\CS")]
		public void ShouldGenerateCorrectElementNameInMessageContract()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			XsdMessage rootElement = CreateRoot<XsdMessage>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.IsWrapped = true;
			rootElement.Element = "xsd:\\file.xsd?MyType";
			rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";

			string content = RunTemplate(rootElement);

			EnsureType(ref content, "MyType");
			Type generatedType = CompileAndGetType(content);
			Assert.AreEqual<string>(MessageContractElementName, generatedType.Name);
			Assert.AreEqual<string>(DefaultNamespace, generatedType.Namespace);
			Assert.AreEqual<int>(2, generatedType.GetConstructors().Length);
			XmlRootAttribute xmlRoot = TypeAsserter.AssertAttribute<XmlRootAttribute>(generatedType);
			Assert.AreEqual<string>(this.MessageContractElementNamespace, xmlRoot.Namespace);
			Assert.IsFalse(xmlRoot.IsNullable);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\XsdMessageContract.tt", @"TextTemplates\ASMX\CS")]
		public void ShouldNotGenerateWithUnwrappedMessage()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			XsdMessage rootElement = CreateRoot<XsdMessage>(MessageContractElementName, MessageContractElementNamespace);
			rootElement.IsWrapped = false;
			rootElement.Element = "xsd:\\file.xsd?MyType";
			rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";

			string content = RunTemplate(rootElement);

			Assert.AreEqual<string>(string.Empty, content);
		}

		protected override string Template
		{
			get
			{
				AsmxXsdMessageContractLink link = new AsmxXsdMessageContractLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(AsmxXsdMessageContract); }
		}
	}
}
