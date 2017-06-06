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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.Practices.ServiceFactory.DataContracts;

namespace ServiceContractDsl.Tests.Validation
{
	/// <summary>
	/// Tests for FullDepthElementWalker
	/// </summary>
	[TestClass]
	public class FullDepthElementWalkerFixture
	{
		private MockServiceProvider serviceProvider = new MockMappingServiceProvider();
		protected MockServiceProvider ServiceProvider
		{
			get { return serviceProvider; }
			set { serviceProvider = value; }
		}

		private Store serviceContractStore = null;
		protected Store ServiceContractStore
		{
			get
			{
				if(serviceContractStore == null)
				{
					serviceContractStore = new Store(ServiceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
				}
				return serviceContractStore;
			}
		}

		private Store dataContractStore = null;
		protected Store DataContractStore
		{
			get
			{
				if(dataContractStore == null)
				{
					dataContractStore = new Store(ServiceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));
				}
				return dataContractStore;
			}
		}

		[TestMethod]
		public void TestWalkerFromDomainModel()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(root);

				Assert.AreEqual(1, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromDomainModelWithServiceContract()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");

				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(root);

				Assert.AreEqual(2, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromDomainModelWithServiceContracts()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract1 = CreateServiceContract("Foo1", "Foo1");
				ServiceContract serviceContract2 = CreateServiceContract("Foo2", "Foo2");

				root.ServiceContracts.Add(serviceContract1);
				root.ServiceContracts.Add(serviceContract2);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(root);

				Assert.AreEqual(3, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContract()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");

				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(1, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperation()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");

				serviceContract.Operations.Add(CreateOperationContract("FooOP"));
				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(2, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperations()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");

				serviceContract.Operations.Add(CreateOperationContract("FooOP1"));
				serviceContract.Operations.Add(CreateOperationContract("FooOP2"));
				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(3, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperationWithResponse()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");
				Operation operation = CreateOperationContract("FooOP");

				operation.Request = CreateMessageContract("FooMC", "FooMC");
				serviceContract.Operations.Add(operation);
				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(3, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperationWithRequestAndResponse()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");
				Operation operation = CreateOperationContract("FooOP");

				operation.Request = CreateMessageContract("FooMCReq", "FooMCReq");
				operation.Response = CreateMessageContract("FooMCRes", "FooMCRes");
				serviceContract.Operations.Add(operation);
				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(4, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperationsWithEqualRequestAndResponse()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");
				Operation operation1 = CreateOperationContract("FooOP1");
				Operation operation2 = CreateOperationContract("FooOP2");
				Message request = CreateMessageContract("FooMCReq1", "FooMCReq1");
				Message response = CreateMessageContract("FooMCRes1", "FooMCRes1");

				operation1.Request = request;
				operation1.Response = response;

				serviceContract.Operations.Add(operation1);
				serviceContract.Operations.Add(operation2);
				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(5, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperationsWithDifferentRequestAndResponse()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");
				Operation operation1 = CreateOperationContract("FooOP1");
				Operation operation2 = CreateOperationContract("FooOP2");

				operation1.Request = CreateMessageContract("FooMCReq1", "FooMCReq1");
				operation1.Response = CreateMessageContract("FooMCRes1", "FooMCRes1");
				operation2.Request = CreateMessageContract("FooMCReq2", "FooMCReq2");
				operation2.Response = CreateMessageContract("FooMCRes2", "FooMCRes2");

				serviceContract.Operations.Add(operation1);
				serviceContract.Operations.Add(operation2);
				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(7, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperationWithRequestAndResponseWithPrimitiveMessagePart()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");
				Operation operation = CreateOperationContract("FooOP");
				Message request = CreateMessageContract("FooMCReq", "FooMCReq");
				Message response = CreateMessageContract("FooMCRes", "FooMCRes");
				PrimitiveMessagePart part1 = CreatePrimitiveMessagePart("FooPart1");
				PrimitiveMessagePart part2 = CreatePrimitiveMessagePart("FooPart2");

				request.MessageParts.Add(part1);
				response.MessageParts.Add(part2);
				operation.Request = request;
				operation.Response = response;
				serviceContract.Operations.Add(operation);
				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(6, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperationWithRequestAndResponseWithPrimitiveMessageParts()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");
				Operation operation = CreateOperationContract("FooOP");
				Message request = CreateMessageContract("FooMCReq", "FooMCReq");
				Message response = CreateMessageContract("FooMCRes", "FooMCRes");
				PrimitiveMessagePart part1 = CreatePrimitiveMessagePart("FooPart1");
				PrimitiveMessagePart part2 = CreatePrimitiveMessagePart("FooPart2");
				PrimitiveMessagePart part3 = CreatePrimitiveMessagePart("FooPart3");
				PrimitiveMessagePart part4 = CreatePrimitiveMessagePart("FooPart4");

				request.MessageParts.Add(part1);
				request.MessageParts.Add(part2);
				request.MessageParts.Add(part3);
				response.MessageParts.Add(part4);
				operation.Request = request;
				operation.Response = response;
				serviceContract.Operations.Add(operation);
				root.ServiceContracts.Add(serviceContract);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(serviceContract);

				Assert.AreEqual(8, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromServiceContractWithOperationWithRequestAndResponseWithDataContractMessagePart()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = DataContractStore.TransactionManager.BeginTransaction())
			{
				using(Transaction t1 = ServiceContractStore.TransactionManager.BeginTransaction())
				{
					ServiceContractModel root = CreateServiceContractRoot();
					ServiceContract serviceContract = CreateServiceContract("Foo", "Foo");
					Operation operation = CreateOperationContract("FooOP");
					Message request = CreateMessageContract("FooMCReq", "FooMCReq");
					Message response = CreateMessageContract("FooMCRes", "FooMCRes");
					DataContract dataContract = CreateDataContract("FooDc");
					DataContract dataContract1 = CreateDataContract("FooDc1");
					DataContractMessagePart part1 =
						CreateDataContractMessagePart(
							"FooPart1",
							GetMockMoniker(dataContract));
					DataContractMessagePart part2 =
						CreateDataContractMessagePart(
							"FooPart2",
							GetMockMoniker(dataContract1));

					request.MessageParts.Add(part1);
					response.MessageParts.Add(part2);
					operation.Request = request;
					operation.Response = response;
					serviceContract.Operations.Add(operation);
					root.ServiceContracts.Add(serviceContract);

					FullDepthElementWalker elementWalker =
						new FullDepthElementWalker(
							new ModelElementVisitor(elementList),
							new EmbeddingReferenceVisitorFilter(),
							false);

					elementWalker.DoTraverse(serviceContract);

					Assert.AreEqual(6, elementList.Count);  // 8 elemns with Resolver

					t1.Rollback();
				}

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromMessageContractWithPrimitiveMessageParts()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = ServiceContractStore.TransactionManager.BeginTransaction())
			{
				ServiceContractModel root = CreateServiceContractRoot();
				Message request = CreateMessageContract("FooMCReq", "FooMCReq");
				PrimitiveMessagePart part1 = CreatePrimitiveMessagePart("FooPart1");
				PrimitiveMessagePart part2 = CreatePrimitiveMessagePart("FooPart2");
				PrimitiveMessagePart part3 = CreatePrimitiveMessagePart("FooPart3");

				request.MessageParts.Add(part1);
				request.MessageParts.Add(part2);
				request.MessageParts.Add(part3);
				root.Messages.Add(request);

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(request);

				Assert.AreEqual(4, elementList.Count);

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromMessageContractWithDataContractMessagePart()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = DataContractStore.TransactionManager.BeginTransaction())
			{
				DataContractModel dcRoot = CreateDataContractRoot();
				DataContract dataContract = CreateDataContract("FooDc");

				dcRoot.Contracts.Add(dataContract);

				using(Transaction t1 = ServiceContractStore.TransactionManager.BeginTransaction())
				{
					ServiceContractModel scRoot = CreateServiceContractRoot();
					Message request = CreateMessageContract("FooMCReq", "FooMCReq");
					DataContractMessagePart part1 =
						CreateDataContractMessagePart(
							"FooPart1",
							GetMockMoniker(dataContract));

					request.MessageParts.Add(part1);
					scRoot.Messages.Add(request);

					FullDepthElementWalker elementWalker =
						new FullDepthElementWalker(
							new ModelElementVisitor(elementList),
							new EmbeddingReferenceVisitorFilter(),
							false);

					elementWalker.DoTraverse(request);

					Assert.AreEqual(2, elementList.Count); // 3 elems with Resolver

					t1.Rollback();
				}

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromMessageContractWithDataContractMessagePartWithDataElement()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = DataContractStore.TransactionManager.BeginTransaction())
			{
				DataContractModel dcRoot = CreateDataContractRoot();
				DataContract dataContract = CreateDataContract("FooDc");
				PrimitiveDataType element = CreatePrimitiveDataElement("FooElement");

				dataContract.DataMembers.Add(element);
				dcRoot.Contracts.Add(dataContract);

				using(Transaction t1 = ServiceContractStore.TransactionManager.BeginTransaction())
				{
					ServiceContractModel scRoot = CreateServiceContractRoot();
					Message request = CreateMessageContract("FooMCReq", "FooMCReq");
					DataContractMessagePart part1 =
						CreateDataContractMessagePart(
							"FooPart1",
							GetMockMoniker(dataContract));

					request.MessageParts.Add(part1);
					scRoot.Messages.Add(request);

					FullDepthElementWalker elementWalker =
						new FullDepthElementWalker(
							new ModelElementVisitor(elementList),
							new EmbeddingReferenceVisitorFilter(),
							false);

					elementWalker.DoTraverse(request);

					Assert.AreEqual(2, elementList.Count); // 4 elems with Resolver

					t1.Rollback();
				}

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromMessageContractWithDataContractMessagePartWithDataElements()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = DataContractStore.TransactionManager.BeginTransaction())
			{
				DataContractModel dcRoot = CreateDataContractRoot();
				DataContract dataContract = CreateDataContract("FooDc");
				PrimitiveDataType element1 = CreatePrimitiveDataElement("FooElement1");
				PrimitiveDataType element2 = CreatePrimitiveDataElement("FooElement2");

				dataContract.DataMembers.Add(element1);
				dataContract.DataMembers.Add(element2);
				dcRoot.Contracts.Add(dataContract);

				using(Transaction t1 = ServiceContractStore.TransactionManager.BeginTransaction())
				{
					ServiceContractModel scRoot = CreateServiceContractRoot();
					Message request = CreateMessageContract("FooMCReq", "FooMCReq");
					DataContractMessagePart part1 =
						CreateDataContractMessagePart(
							"FooPart1",
							GetMockMoniker(dataContract));

					request.MessageParts.Add(part1);
					scRoot.Messages.Add(request);

					FullDepthElementWalker elementWalker =
						new FullDepthElementWalker(
							new ModelElementVisitor(elementList),
							new EmbeddingReferenceVisitorFilter(),
							false);

					elementWalker.DoTraverse(request);

					Assert.AreEqual(2, elementList.Count); // 5 elems with Resolver

					t1.Rollback();
				}

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromMessageContractWithDataContractMessagePartsWithDataElements()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = DataContractStore.TransactionManager.BeginTransaction())
			{
				DataContractModel dcRoot = CreateDataContractRoot();
				DataContract dataContract1 = CreateDataContract("FooDc");
				PrimitiveDataType element1 = CreatePrimitiveDataElement("FooElement1");
				PrimitiveDataType element2 = CreatePrimitiveDataElement("FooElement2");
				DataContract dataContract2 = CreateDataContract("FooDc1");
				PrimitiveDataType element3 = CreatePrimitiveDataElement("FooElement1");
				PrimitiveDataType element4 = CreatePrimitiveDataElement("FooElement2");

				dataContract1.DataMembers.Add(element1);
				dataContract1.DataMembers.Add(element2);
				dataContract2.DataMembers.Add(element3);
				dataContract2.DataMembers.Add(element4);
				dcRoot.Contracts.Add(dataContract1);
				dcRoot.Contracts.Add(dataContract2);

				using(Transaction t1 = ServiceContractStore.TransactionManager.BeginTransaction())
				{
					ServiceContractModel scRoot = CreateServiceContractRoot();
					Message request = CreateMessageContract("FooMCReq", "FooMCReq");
					DataContractMessagePart part1 =
						CreateDataContractMessagePart(
							"FooPart1",
							GetMockMoniker(dataContract1));
					DataContractMessagePart part2 =
						CreateDataContractMessagePart(
							"FooPart2",
							GetMockMoniker(dataContract2));


					request.MessageParts.Add(part1);
					request.MessageParts.Add(part2);
					scRoot.Messages.Add(request);

					FullDepthElementWalker elementWalker =
						new FullDepthElementWalker(
							new ModelElementVisitor(elementList),
							new EmbeddingReferenceVisitorFilter(),
							false);

					elementWalker.DoTraverse(request);

					Assert.AreEqual(3, elementList.Count); // 9 elemsn with Resolver

					t1.Rollback();
				}

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestWalkerFromMessageContractWithDataContractCollectionMessagePart()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			using(Transaction t = DataContractStore.TransactionManager.BeginTransaction())
			{
				DataContractModel dcRoot = CreateDataContractRoot();
				DataContract dataContract = CreateDataContract("FooDc");
				DataContractCollection dataContractCollection = CreateDataContractCollection("FooDcc");
				dataContractCollection.DataContract = dataContract;

				dcRoot.Contracts.Add(dataContract);
				dcRoot.Contracts.Add(dataContractCollection);

				using(Transaction t1 = ServiceContractStore.TransactionManager.BeginTransaction())
				{
					ServiceContractModel scRoot = CreateServiceContractRoot();
					Message request = CreateMessageContract("FooMCReq", "FooMCReq");
					DataContractMessagePart part1 =
						CreateDataContractMessagePart(
							"FooPart1",
							GetMockMoniker(dataContract));

					request.MessageParts.Add(part1);
					scRoot.Messages.Add(request);

					FullDepthElementWalker elementWalker =
						new FullDepthElementWalker(
							new ModelElementVisitor(elementList),
							new EmbeddingReferenceVisitorFilter(),
							false);

					elementWalker.DoTraverse(request);

					Assert.AreEqual(2, elementList.Count); // 4 elemsn with Resolver

					t1.Rollback();
				}

				t.Rollback();
			}
		}

		private ServiceContractModel CreateServiceContractRoot()
		{
			ServiceContractModel serviceContractModel =
				new ServiceContractModel(ServiceContractStore);

			serviceContractModel.ProjectMappingTable = "WCF";

			return serviceContractModel;
		}

		private DataContractModel CreateDataContractRoot()
		{
			DataContractModel dataContractModel =
				new DataContractModel(DataContractStore);

			dataContractModel.ProjectMappingTable = "WCF";

			return dataContractModel;
		}

		private ServiceContract CreateServiceContract(string name, string ns)
		{
			ServiceContract serviceContract = new ServiceContract(ServiceContractStore);
			serviceContract.Name = name;
			serviceContract.Namespace = ns;
			return serviceContract;
		}

		private Operation CreateOperationContract(string name)
		{
			Operation operation = new Operation(ServiceContractStore);
			operation.Name = name;
			return operation;
		}

		private Message CreateMessageContract(string name, string ns)
		{
			Message messageContract = new Message(ServiceContractStore);
			messageContract.Name = name;
			messageContract.Namespace = ns;
			return messageContract;
		}

		private DataContractMessagePart CreateDataContractMessagePart(string name, string type)
		{
			DataContractMessagePart messagePart = new DataContractMessagePart(ServiceContractStore);
			messagePart.Name = name;
			//messagePart.Type = type;
			return messagePart;
		}

		private PrimitiveMessagePart CreatePrimitiveMessagePart(string name)
		{
			PrimitiveMessagePart messagePart = new PrimitiveMessagePart(ServiceContractStore);
			messagePart.Name = name;
			messagePart.Type = "System.String";
			return messagePart;
		}

		private DataContract CreateDataContract(string name)
		{
			DataContract dataContract = new DataContract(DataContractStore);
			dataContract.Name = name;
			return dataContract;
		}

		private DataContractCollection CreateDataContractCollection(string name)
		{
			DataContractCollection dataContract = new DataContractCollection(DataContractStore);
			dataContract.Name = name;
			return dataContract;
		}

		private PrimitiveDataType CreatePrimitiveDataElement(string name)
		{
			PrimitiveDataType dataElement = new PrimitiveDataType(DataContractStore);
			dataElement.Name = name;
			dataElement.Type = "System.String";
			return dataElement;
		}

		private string GetMockMoniker(ModelElement element)
		{
			string moniker =
				string.Format(@"//mel://[DSLNAMESPACE]\[MODELELEMENTTYPE]\{0}@[PROJECT]\[MODELFILE]", element.Id.ToString());

			return moniker;
		}
	}
}