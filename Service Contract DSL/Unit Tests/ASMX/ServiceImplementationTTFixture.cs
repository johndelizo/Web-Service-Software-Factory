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
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Web.Services;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx.CodeGeneration;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace ServiceContractDsl.Tests.ASMX
{
	[TestClass]
	public class ServiceImplementationTTFixture : ServiceContractDsl.Tests.ServiceImplementationTTFixture
	{
		const string ServiceName = "MyService";
		const string ServiceNamespace = "http://mynamespace";
		const string ServiceContractName = "MyServiceContract";
		const string OperationName = "OperationName";
		const string RequestName = "RequestName";
		const string ResponseName = "ResponseName";
		const string FaultName = "FaultName";
		readonly string ServiceContractInterfaceName = "I" + ServiceContractName;

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.ServiceContractDsl.Tests.xml")]
		[DeploymentItem("TextTemplates", "TextTemplates")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\ServiceImplementation.tt", @"TextTemplates\ASMX\CS")]
		public void TestHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			rootElement.Namespace = ServiceNamespace;
			AsmxService extender = new AsmxService();
			extender.ModelElement = rootElement;
			rootElement.ObjectExtender = extender;
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			Assert.AreEqual<Type>(typeof(System.Web.Services.WebService), generatedType.BaseType.BaseType);
			WebServiceAttribute serviceBehavior = TypeAsserter.AssertAttribute<WebServiceAttribute>(generatedType);
			Assert.AreEqual<string>(ServiceName, serviceBehavior.Name);
			Assert.AreEqual<string>(ServiceNamespace, serviceBehavior.Namespace);
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\ServiceImplementation.tt", @"TextTemplates\ASMX\CS")]
		public void NullServiceContractGeneratesServiceContract()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			rootElement.Namespace = ServiceNamespace;
			AsmxService extender = new AsmxService();
			extender.ModelElement = rootElement;
			rootElement.ObjectExtender = extender;
			rootElement.ServiceContract = null;

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertInterface(ServiceContractInterfaceName, generatedType, 0);
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\ServiceImplementation.tt", @"TextTemplates\ASMX\CS")]
		public void TestOneServiceContractGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			rootElement.ServiceContract = CreateServiceContract(ServiceContractName, OperationName);
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
		[DeploymentItem(@"ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\ServiceImplementation.tt", @"TextTemplates\ASMX\CS")]
		public void TestServiceContractWithFaultGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			ServiceContract contract = CreateServiceContract(ServiceContractName, OperationName);
			contract.Operations[0].Faults.Add(CreateFault(FaultName));
			rootElement.ServiceContract = contract;
			string content = RunTemplate(rootElement);

			EnsureType(ref content, ServiceContractInterfaceName);
			EnsureType(ref content, RequestName);
			EnsureType(ref content, ResponseName);
			EnsureType(ref content, FaultName);
			Type generatedType = CompileAndGetType(content);

			Assert.IsNotNull(generatedType);
			Assert.IsTrue(content.Contains("// MyFault fault = new MyFault();"));
			Assert.IsTrue(content.Contains("// throw GenerateSoapException("));
		}

		[TestMethod]
		[DeploymentItem(@"ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\ASMX\CS\ServiceContractAsmcCommon.tt", @"TextTemplates\ASMX\CS")]
		[DeploymentItem(@"TextTemplates\ASMX\CS\ServiceImplementation.tt", @"TextTemplates\ASMX\CS")]
		public void TestServiceContractWithFaultGenerationAndNoResponse()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Service rootElement = CreateDefaultService();
			ServiceContract contract = CreateServiceContract(ServiceContractName, OperationName, null, null);
			contract.Operations[0].Faults.Add(CreateFault(FaultName));
			rootElement.ServiceContract = contract;
			string content = RunTemplate(rootElement);

			EnsureType(ref content, ServiceContractInterfaceName);
			EnsureType(ref content, FaultName);
			Type generatedType = CompileAndGetType(content);

			Assert.IsNotNull(generatedType);
			Assert.IsTrue(content.Contains("// MyFault fault = new MyFault();"));
			Assert.IsTrue(content.Contains("// throw GenerateSoapException("));
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
				ASMXServiceImplementationLink link = new ASMXServiceImplementationLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(AsmxService); }
		}

		private Service CreateDefaultService()
		{
			Service rootElement = new Service(Store);
			rootElement.Name = ServiceName;
			rootElement.ServiceContractModel = new ServiceContractModel(Store);
			rootElement.ServiceContractModel.ProjectMappingTable = "ASMX";
			return rootElement;
		}

		private ServiceContract CreateServiceContract(
			string serviceContractName, string operationName)
		{
			return CreateServiceContract(serviceContractName, operationName, RequestName, ResponseName);
		}

		private ServiceContract CreateServiceContract(
			string serviceContractName, string operationName, string requestName, string responseName)
		{
			ServiceContract serviceContract = new ServiceContract(Store);
			serviceContract.Name = serviceContractName;
			Operation op1 = new Operation(Store);
			op1.Name = operationName;
			op1.Action = operationName;
			op1.Request = string.IsNullOrEmpty(requestName) ? null : CreateMessage(requestName);
			op1.Response = string.IsNullOrEmpty(responseName) ? null : CreateMessage(responseName);
			op1.ServiceContract = serviceContract;

			return serviceContract;
		}

		private Message CreateMessage(string name)
		{
			Message message = new Message(Store);
			message.Name = name;
			return message;
		}

		private Fault CreateFault(string name)
		{
			DataContractFault fault = new DataContractFault(Store);
			fault.Name = name;
            //fault.Type = @"mel://Microsoft.Practices.ServiceFactory.DataContracts\FaultContract\String@Project1.model\dc.datacontract";
			return fault;
		}

		#endregion
	}
}
