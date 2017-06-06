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
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.ServiceModel;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace ServiceContractDsl.Tests.WCF
{
	[TestClass]
	public class ServiceImplementationTTFixture : ServiceContractDsl.Tests.ServiceImplementationTTFixture
	{
		const string ServiceName = "MyService";
		const string ServiceNamespace = "http://mynamespace";
		const string ServiceContractName = "MyServiceContract";
		const string DefaultInstanceContextMode = "WCF::InstanceContextMode.PerSession";
		const string DefaultConcurrencyMode = "WCF::ConcurrencyMode.Single";
		const string OperationName = "OperationName";
		const string RequestName = "RequestName";
		const string ResponseName = "ResponseName";
		readonly string ServiceContractInterfaceName = "I" + ServiceContractName;

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceImplementation.tt", @"TextTemplates\WCF\CS")]
		public void TestHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
            
			Service rootElement = CreateDefaultService();
			rootElement.Namespace = ServiceNamespace;
			WCFService extender = new WCFService();
			extender.ModelElement = rootElement;
			rootElement.ObjectExtender = extender;
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			ServiceBehaviorAttribute serviceBehavior = TypeAsserter.AssertAttribute<ServiceBehaviorAttribute>(generatedType);
			Assert.AreEqual<string>(ServiceName, serviceBehavior.Name);
			Assert.AreEqual<string>(ServiceNamespace, serviceBehavior.Namespace);
			Assert.AreEqual<InstanceContextMode>(InstanceContextMode.PerSession, serviceBehavior.InstanceContextMode);
			Assert.AreEqual<ConcurrencyMode>(ConcurrencyMode.Single, serviceBehavior.ConcurrencyMode);
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceImplementation.tt", @"TextTemplates\WCF\CS")]
		public void NullServiceContractGeneratesServiceContract()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			rootElement.Namespace = ServiceNamespace;
			WCFService extender = new WCFService();
			extender.ModelElement = rootElement;
			rootElement.ObjectExtender = extender;
			rootElement.ServiceContract = null;

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertInterface(ServiceContractInterfaceName, generatedType, 0);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceImplementation.tt", @"TextTemplates\WCF\CS")]
		public void TestOneServiceContractGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			rootElement.ServiceContract = CreateServiceContract(rootElement.ServiceContractModel,ServiceContractName, OperationName);
			string content = RunTemplate(rootElement);

			EnsureType(ref content, ServiceContractInterfaceName);
			EnsureType(ref content, RequestName);
			EnsureType(ref content, ResponseName);
			string implementationContent = this.GetImplementationContent(ImplementationKind.RequestResponse);
			Type generatedType = CompileAndGetType(content + implementationContent);
			TypeAsserter.AssertInterface(GetFullServiceContractInterfaceName(), generatedType);
			MethodInfo method = TypeAsserter.AssertMethod(OperationName, generatedType);
			Assert.AreEqual<string>(ResponseName, method.ReturnType.Name);
			Assert.AreEqual<string>(RequestName, ((ParameterInfo)method.GetParameters().GetValue(0)).ParameterType.Name);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceImplementation.tt", @"TextTemplates\WCF\CS")]
		public void TestOperationWithoutRequest()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			rootElement.ServiceContract = CreateServiceContract(rootElement.ServiceContractModel,ServiceContractName, OperationName, null, ResponseName);
			string content = RunTemplate(rootElement);

			EnsureType(ref content, ServiceContractInterfaceName);
			EnsureType(ref content, ResponseName);
			string implementationContent = this.GetImplementationContent(ImplementationKind.Response);
			Type generatedType = CompileAndGetType(content + implementationContent);
			TypeAsserter.AssertInterface(GetFullServiceContractInterfaceName(), generatedType);
			MethodInfo method = TypeAsserter.AssertMethod(OperationName, generatedType);
			Assert.AreEqual<string>(ResponseName, method.ReturnType.Name);
			Assert.AreEqual<int>(0, method.GetParameters().Length);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceImplementation.tt", @"TextTemplates\WCF\CS")]
		public void TestOperationWithoutResponse()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			rootElement.ServiceContract = CreateServiceContract(rootElement.ServiceContractModel, ServiceContractName, OperationName, RequestName, null);
			string content = RunTemplate(rootElement);

			EnsureType(ref content, ServiceContractInterfaceName);
			EnsureType(ref content, RequestName);
			string implementationContent = this.GetImplementationContent(ImplementationKind.Request);
			Type generatedType = CompileAndGetType(content + implementationContent);
			TypeAsserter.AssertInterface(GetFullServiceContractInterfaceName(), generatedType);
			MethodInfo method = TypeAsserter.AssertMethod(OperationName, generatedType);
			Assert.AreEqual<string>(RequestName, ((ParameterInfo)method.GetParameters().GetValue(0)).ParameterType.Name);
			Assert.AreEqual<string>("Void", method.ReturnType.Name);
		}

		#region Private Implementation

		private string GetFullServiceContractInterfaceName()
		{
			return DefaultNamespace + "." + ServiceContractInterfaceName;
		}
		private Type CompileAndGetType(string content)
		{
			EnsureNamespace(ref content);
			string typeName = DefaultNamespace + "." + ServiceName;
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(content);

			Type generatedType = results.CompiledAssembly.GetType(typeName, false);
			
            Assert.IsNotNull(generatedType, "Invalid type: " + typeName);
			return generatedType;
		}

		protected override string Template
		{
			get
			{
				ServiceLink link = new ServiceLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(WCFService); }
		}

		private Service CreateDefaultService()
		{
			Service rootElement = new Service(Store);
			WCFService wcfService = new WCFService();
			wcfService.ModelElement = rootElement;
			rootElement.ObjectExtender = wcfService;
			rootElement.Name = ServiceName;
			rootElement.ServiceContractModel = new ServiceContractModel(Store);
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
			return rootElement;
		}

		private ServiceContract CreateServiceContract(
			ServiceContractModel model, string serviceContractName, string operationName)
		{
			return CreateServiceContract(model,serviceContractName, operationName, RequestName, ResponseName);
		}

		private ServiceContract CreateServiceContract(
			ServiceContractModel model,string serviceContractName, string operationName, string requestName, string responseName)
		{
			ServiceContract serviceContract = new ServiceContract(Store);
			serviceContract.ServiceContractModel = model;
			serviceContract.Name = serviceContractName;
			WCFServiceContract serviceContractExtender = new WCFServiceContract();
			serviceContractExtender.ModelElement = serviceContract;
			serviceContract.ObjectExtender = serviceContractExtender;

			Operation op1 = new Operation(Store);
			WCFOperationContract wcfOp1 = new WCFOperationContract();
			op1.ObjectExtender = wcfOp1;
			op1.Name = operationName;
			op1.Action = operationName;
			op1.Request = string.IsNullOrEmpty(requestName) ? null : CreateMessageContract(requestName);
			op1.Response = string.IsNullOrEmpty(responseName) ? null : CreateMessageContract(responseName);
			op1.ServiceContract = serviceContract;

			return serviceContract;
		}

		private Message CreateMessageContract(string name)
		{
			Message messageContract = new Message(Store);
			messageContract.Name = name;
			return messageContract;
		}

		#endregion
	}
}
