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
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.ServiceModel;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Net.Security;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace ServiceContractDsl.Tests.WCF
{
	[TestClass]
	public class ServiceContractTTFixture : ServiceContractModelFixture
	{
		const string ServiceContractElementName = "Name1";
		const string ServiceContractElementNamespace = "http://mynamespace";
		private bool processFault;

		[TestMethod]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			Assert.AreEqual<string>("I" + ServiceContractElementName, generatedType.Name);
			Assert.IsTrue(generatedType.IsInterface);
			ServiceContractAttribute serviceContract = TypeAsserter.AssertAttribute<ServiceContractAttribute>(generatedType);
			Assert.AreEqual<string>(ServiceContractElementName, serviceContract.Name);
			Assert.AreEqual<string>(ServiceContractElementNamespace, serviceContract.Namespace);
			Assert.AreEqual(ProtectionLevel.None, serviceContract.ProtectionLevel);
		}

		[TestMethod]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestWFCHeaderGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			WCFServiceContract extender = new WCFServiceContract();
			extender.SessionMode = SessionMode.Required;
			extender.ModelElement = rootElement;
			rootElement.ObjectExtender = extender;
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			ServiceContractAttribute serviceContract = TypeAsserter.AssertAttribute<ServiceContractAttribute>(generatedType);
			Assert.AreEqual<string>(ServiceContractElementName, serviceContract.Name);
			Assert.AreEqual<string>(ServiceContractElementNamespace, serviceContract.Namespace);
			Assert.AreEqual<SessionMode>(SessionMode.Required, serviceContract.SessionMode);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestFaultMessagesGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			DataContractFault fault1 = new DataContractFault(Store);
			fault1.Name = "fault1";
			//fault1.Type = @"[DSLNAMESPACE]\[MODELELEMENTTYPE]\fault1Type@[PROJECT]\[MODELFILE]";
			fault1.Operation = op1;
			DataContractFault fault2 = new DataContractFault(Store);
			fault2.Name = "fault2";
			//fault2.Type = @"[DSLNAMESPACE]\[MODELELEMENTTYPE]\fault2Type@[PROJECT]\[MODELFILE]"; ;
			fault2.Operation = op1;
			processFault = true;
			ResolveModelElement(fault1.Name);
			ResolveModelElement(fault2.Name);

			string content = RunTemplateWithDIS(rootElement);

            StringAssert.Contains(content, "I" + ServiceContractElementName);
            StringAssert.Contains(content, ServiceContractElementNamespace);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestOperationGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			Assert.AreEqual<int>(0, method.GetParameters().Length);
			Assert.AreEqual<string>("Void", method.ReturnType.Name);
			OperationContractAttribute operation = TypeAsserter.AssertAttribute<OperationContractAttribute>(method);
			Assert.AreEqual<string>(op1.Action, operation.Action);
			Assert.IsNull(operation.Name);
			Assert.AreEqual<bool>(op1.IsOneWay, operation.IsOneWay);
			Assert.AreEqual<bool>(((WCFOperationContract)op1.ObjectExtender).IsTerminating, operation.IsTerminating);
			Assert.AreEqual<bool>(((WCFOperationContract)op1.ObjectExtender).IsInitiating, operation.IsInitiating);
            Assert.AreEqual<ProtectionLevel>(((WCFOperationContract)op1.ObjectExtender).ProtectionLevel, operation.ProtectionLevel);
			Assert.IsNull(operation.ReplyAction);
			Assert.IsTrue(operation.HasProtectionLevel);
		}

		[TestMethod]
		public void ShouldGetDefaultActionValue()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);

			Operation op1 = new Operation(Store);
			op1.ServiceContract = rootElement;
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";

			// commit the tx and trigger the OperationAddRule so the action value will be filled with the default value
			this.transaction.Commit();
			this.transaction = null;

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			OperationContractAttribute operation = TypeAsserter.AssertAttribute<OperationContractAttribute>(method);
			Assert.AreEqual<string>(rootElement.Namespace + "/" + rootElement.Name + "/" + op1.Name, operation.Action);
		}

		[TestMethod]
		public void ShouldGetUpdatedActionValueAfterChange()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);

			Operation op1 = new Operation(Store);
			op1.ServiceContract = rootElement;
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";

			// commit the tx and trigger the OperationAddRule so the action value will be filled with the default value
			this.transaction.Commit();
			this.transaction = Store.TransactionManager.BeginTransaction();
			op1.Name = "op2";
			this.transaction.Commit();
			this.transaction = null;

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			OperationContractAttribute operation = TypeAsserter.AssertAttribute<OperationContractAttribute>(method);
			Assert.AreEqual<string>(rootElement.Namespace + "/" + rootElement.Name + "/op2", operation.Action);
		}

		[TestMethod]
		public void ShouldGetActionValueWithDefaultUriSlashEnded()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace + "/");

			Operation op1 = new Operation(Store);
			op1.ServiceContract = rootElement;
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";

			// commit the tx and trigger the OperationAddRule so the action value will be filled with the default value
			this.transaction.Commit();
			this.transaction = Store.TransactionManager.BeginTransaction();
			op1.Name = "op2";
			this.transaction.Commit();
			this.transaction = null;

			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			OperationContractAttribute operation = TypeAsserter.AssertAttribute<OperationContractAttribute>(method);
			Assert.AreEqual<string>(rootElement.Namespace + rootElement.Name + "/op2", operation.Action);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestAsyncOperationGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			Operation op1 = new Operation(Store);
			WCFOperationContract wfcOperationContract = new WCFOperationContract();
			wfcOperationContract.AsyncPattern = true;
			op1.ObjectExtender = wfcOperationContract;
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			MethodInfo beginMethod = TypeAsserter.AssertMethod("Begin"+op1.Name, generatedType);
			Assert.AreEqual<int>(2, beginMethod.GetParameters().Length);
			Assert.AreEqual<string>("IAsyncResult", beginMethod.ReturnType.Name);
			MethodInfo endMethod = TypeAsserter.AssertMethod("End"+op1.Name, generatedType);
			Assert.AreEqual<int>(1, endMethod.GetParameters().Length);
			Assert.AreEqual<string>("Void", endMethod.ReturnType.Name);
		}


		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
		public void TestOperationWithoutObjectExtender()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			Operation op1 = new Operation(Store);
			op1.ObjectExtender = null;
			op1.Request = new Message(Store);

			Assert.AreEqual(0, op1.ArtifactLinks.Count);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestResponseGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			Message response = new Message(Store);
			response.Name = "Response1";
			op1.Response = response;
			string content = RunTemplate(rootElement);

			EnsureType(ref content, "Response1");
			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			Assert.AreEqual<string>("Response1", method.ReturnType.Name);
			Assert.AreEqual<int>(0, method.GetParameters().Length);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestRequestGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			Message request = new Message(Store);
			request.Name = "Request1";
			op1.Request = request;
			string content = RunTemplate(rootElement);

			EnsureType(ref content, "Request1");
			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			Assert.AreEqual<int>(1, method.GetParameters().Length);
			Assert.AreEqual<string>("Request1", ((ParameterInfo)method.GetParameters().GetValue(0)).ParameterType.Name);
			Assert.AreEqual<string>("Void", method.ReturnType.Name);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestMultipleOperationsGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			Operation op2 = new Operation(Store);
			op2.ObjectExtender = new WCFOperationContract();
			op2.Name = "op2";
			op2.Action = "op2";
			op2.ServiceContract = rootElement;
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertMethod(op1.Name, generatedType);
			TypeAsserter.AssertMethod(op2.Name, generatedType);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestReplyActionGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			Operation op1 = new Operation(Store);
			WCFOperationContract oc = new WCFOperationContract();
			oc.ReplyAction = "foo";
			op1.ObjectExtender = oc;
			op1.Name = "op1";
			op1.ServiceContract = rootElement;
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			OperationContractAttribute operation = TypeAsserter.AssertAttribute<OperationContractAttribute>(method);
			Assert.AreEqual<string>(oc.ReplyAction, operation.ReplyAction);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\SimpleSchema1.xsd", "SampleData")]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestXsdMessageUnwrappedRequestGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			rootElement.ServiceContractModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
 
			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			XsdMessage request = new XsdMessage(Store);
			request.Name = "Request1";
			request.Element = @"xsd://SampleData/SimpleSchema1.xsd?element1";
			request.IsWrapped = false;
			request.ServiceContractModel = rootElement.ServiceContractModel;
			WCFXsdMessageContract wcfXsdMc = new WCFXsdMessageContract();
			wcfXsdMc.ModelElement = request;
			request.ObjectExtender = wcfXsdMc;

			op1.Request = request;
			string content = RunTemplate(rootElement);

			EnsureType(ref content, "Request1");
			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			Assert.AreEqual<int>(1, method.GetParameters().Length);
			Assert.AreEqual<string>("Request1", ((ParameterInfo)method.GetParameters().GetValue(0)).ParameterType.Name);
			Assert.AreEqual<string>("Void", method.ReturnType.Name);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\SimpleSchema1.xsd", "SampleData")]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void TestXsdMessageWrappedRequestGeneration()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			rootElement.ServiceContractModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();

			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			XsdMessage request = new XsdMessage(Store);
			request.Name = "Request1";
			request.Element = @"xsd://SampleData/SimpleSchema1.xsd?element1";
			request.IsWrapped = true;
			request.ServiceContractModel = rootElement.ServiceContractModel;
			WCFXsdMessageContract wcfXsdMc = new WCFXsdMessageContract();
			wcfXsdMc.ModelElement = request;
			request.ObjectExtender = wcfXsdMc;

			op1.Request = request;
			string content = RunTemplate(rootElement);

			EnsureType(ref content, "Request1");
			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			Assert.AreEqual<int>(1, method.GetParameters().Length);
			Assert.AreEqual<string>("Request1", ((ParameterInfo)method.GetParameters().GetValue(0)).ParameterType.Name);
			Assert.AreEqual<string>("Void", method.ReturnType.Name);
		}

		[TestMethod]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateXmlSerializerFormatAttributeWithXmlSerialize()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");
			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			rootElement.ServiceContractModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer;
			string content = RunTemplate(rootElement);

			Type generatedType = CompileAndGetType(content);
			TypeAsserter.AssertAttribute<XmlSerializerFormatAttribute>(generatedType);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
		[DeploymentItem(@"SampleData\BaseTypes.xsd", "SampleData")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateKnownTypeAttributeWithXsdExtendedTypes()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			rootElement.ServiceContractModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
			rootElement.ServiceContractModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.DataContractSerializer;
			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;
			XsdMessage request = new XsdMessage(Store);
			request.Name = "Request1";
			request.Element = @"xsd://SampleData/BaseTypes.xsd?LandmarkPoint";
			request.ServiceContractModel = rootElement.ServiceContractModel;
			WCFXsdMessageContract wcfXsdMc = new WCFXsdMessageContract();
			wcfXsdMc.ModelElement = request;
			request.ObjectExtender = wcfXsdMc;

			op1.Request = request;
			string content = RunTemplate(rootElement);

			EnsureType(ref content, "Request1");
			EnsureType(ref content, "LandmarkBase");
			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			ServiceKnownTypeAttribute attribute = TypeAsserter.AssertAttribute<ServiceKnownTypeAttribute>(method);
			Assert.AreEqual<string>("LandmarkBase", attribute.Type.Name);
		}

		[TestMethod]
		[DeploymentItem("ProjectMapping.ServiceContractDsl.Tests.xml")]
		[DeploymentItem(@"SampleData\BaseTypes.xsd", "SampleData")]
        [DeploymentItem(@"TextTemplates\WCF\CS\ServiceContractWcfCommon.tt", @"TextTemplates\WCF\CS")]
		[DeploymentItem(@"TextTemplates\WCF\CS\ServiceContract.tt", @"TextTemplates\WCF\CS")]
		public void ShouldGenerateKnownTypeAttributeWithXsdExtendedTypesNotDuplicated()
		{
			ProjectMappingManagerSetup.InitializeManager(ServiceProvider, "ProjectMapping.ServiceContractDsl.Tests.xml");

			ServiceContract rootElement = CreateRoot(ServiceContractElementName, ServiceContractElementNamespace);
			rootElement.ServiceContractModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
			rootElement.ServiceContractModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.DataContractSerializer;
			Operation op1 = new Operation(Store);
			op1.ObjectExtender = new WCFOperationContract();
			op1.Name = "op1";
			op1.Action = "op1";
			op1.ServiceContract = rootElement;

			XsdMessage request = new XsdMessage(Store);
			request.Name = "Request1";
			request.Element = @"xsd://SampleData/BaseTypes.xsd?LandmarkPoint";
			request.ServiceContractModel = rootElement.ServiceContractModel;
			WCFXsdMessageContract wcfXsdMc = new WCFXsdMessageContract();
			wcfXsdMc.ModelElement = request;
			request.ObjectExtender = wcfXsdMc;

			XsdMessage response = new XsdMessage(Store);
			response.Name = "Response1";
			response.Element = @"xsd://SampleData/BaseTypes.xsd?LandmarkPoint";
			response.ServiceContractModel = rootElement.ServiceContractModel;
			WCFXsdMessageContract wcfXsdMc2 = new WCFXsdMessageContract();
			wcfXsdMc2.ModelElement = request;
			response.ObjectExtender = wcfXsdMc2;

			op1.Request = request;
			op1.Response = response;
			string content = RunTemplate(rootElement);

			EnsureType(ref content, "Request1");
			EnsureType(ref content, "Response1");
			EnsureType(ref content, "LandmarkBase");
			Type generatedType = CompileAndGetType(content);
			MethodInfo method = TypeAsserter.AssertMethod(op1.Name, generatedType);
			ServiceKnownTypeAttribute attribute = TypeAsserter.AssertAttribute<ServiceKnownTypeAttribute>(method);
			Assert.AreEqual<string>("LandmarkBase", attribute.Type.Name);
		}

		#region Private Implementation

		private Type CompileAndGetType(string content)
		{
			EnsureNamespace(ref content);
			string typeName = DefaultNamespace + ".I" + ServiceContractElementName;
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(content);

			Type generatedType = results.CompiledAssembly.GetType(typeName, false);
			
            Assert.IsNotNull(generatedType, "Invalid type: " + typeName);
			return generatedType;
		}

		private ServiceContract CreateRoot(string name, string ns)
		{
			ServiceContract rootElement = new ServiceContract(Store);
			rootElement.ServiceContractModel = new ServiceContractModel(Store);
			rootElement.Name = name;
			rootElement.Namespace = ns;
			rootElement.ServiceContractModel.ProjectMappingTable = "WCF";
			dataContractStore = new Store(ServiceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));
			ResolveModelElement("foo");
			return rootElement;
		}

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
			if (this.processFault)
			{
				return ResolveFaultElement(instanceNamespace);
			}

			using (Transaction transaction = DataContractStore.TransactionManager.BeginTransaction())
			{
				DataContract dc = new DataContract(DataContractStore);
				dc.DataContractModel = new DataContractModel(DataContractStore);
				dc.DataContractModel.ProjectMappingTable = "WCF";
				dc.Name = instanceNamespace;
				WCFDataContract extender = new WCFDataContract();
				dc.ObjectExtender = extender;
				extender.ModelElement = dc;
				transaction.Commit();
				return dc;
			}
		}

		protected ModelElement ResolveFaultElement(string instanceNamespace)
		{
			using (Transaction transaction = DataContractStore.TransactionManager.BeginTransaction())
			{
				FaultContract fc = new FaultContract(DataContractStore);
				fc.DataContractModel = new DataContractModel(DataContractStore);
				fc.DataContractModel.ProjectMappingTable = "WCF";
				fc.Name = instanceNamespace;
				WCFFaultContract extender = new WCFFaultContract();
				fc.ObjectExtender = extender;
				extender.ModelElement = fc;
				transaction.Commit();
				return fc;
			}
		}

		protected override string Template
		{
			get
			{
				ServiceContractLink link = new ServiceContractLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(WCFServiceContract); }
		}

		#endregion
	}
}
