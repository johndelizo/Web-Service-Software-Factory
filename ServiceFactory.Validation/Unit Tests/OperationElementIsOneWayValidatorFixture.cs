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
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
	public class OperationElementIsOneWayValidatorFixture
	{
		[TestMethod]
		public void ValidateWithResponseAndIsOneWayFails()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				Operation operation = store.ElementFactory.CreateElement(Operation.DomainClassId) as Operation;
				Message contract = store.ElementFactory.CreateElement(Message.DomainClassId) as Message;

				operation.Response = contract;
				operation.IsOneWay = true;

				TestableOperationElementIsOneWayValidator target = new TestableOperationElementIsOneWayValidator();

				ValidationResults results = new ValidationResults();
				target.TestDoValidate(operation.IsOneWay, operation, "IsOneWay", results);

				Assert.IsFalse(results.IsValid);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void ValidateWithRequestAndIsOneWaySucceeds()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				Operation operation = store.ElementFactory.CreateElement(Operation.DomainClassId) as Operation;
				Message contract = store.ElementFactory.CreateElement(Message.DomainClassId) as Message;

				operation.Request = contract;
				operation.IsOneWay = true;

				TestableOperationElementIsOneWayValidator target = new TestableOperationElementIsOneWayValidator();

				ValidationResults results = new ValidationResults();
				target.TestDoValidate(operation.IsOneWay, operation, "IsOneWay", results);

				Assert.IsTrue(results.IsValid);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void ValidateWithRequestAndFaultsIsOneWayFails()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				Operation operation = store.ElementFactory.CreateElement(Operation.DomainClassId) as Operation;
				Message contract = store.ElementFactory.CreateElement(Message.DomainClassId) as Message;
				DataContractFault fault = store.ElementFactory.CreateElement(DataContractFault.DomainClassId) as DataContractFault;

				operation.Request = contract;
				operation.IsOneWay = true;
				operation.Faults.Add(fault);

				TestableOperationElementIsOneWayValidator target = new TestableOperationElementIsOneWayValidator();

				ValidationResults results = new ValidationResults();
				target.TestDoValidate(operation.IsOneWay, operation, "IsOneWay", results);

				Assert.IsFalse(results.IsValid);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void ValidateWithReplyActionAndIsOneWayFails()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				Operation operation = store.ElementFactory.CreateElement(Operation.DomainClassId) as Operation;
				Message contract = store.ElementFactory.CreateElement(Message.DomainClassId) as Message;

				operation.ObjectExtender = new WCFOperationContract(false, false, false, "test", System.Net.Security.ProtectionLevel.None);
				operation.IsOneWay = true;

				TestableOperationElementIsOneWayValidator target = new TestableOperationElementIsOneWayValidator();

				ValidationResults results = new ValidationResults();
				target.TestDoValidate(operation.IsOneWay, operation, "IsOneWay", results);

				Assert.IsFalse(results.IsValid);

				transaction.Commit();
			}
		}
	}

	#region TestableOperationElementIsOneWayValidator class

	public class TestableOperationElementIsOneWayValidator : OperationElementIsOneWayValidator
	{
		public TestableOperationElementIsOneWayValidator()
			: base(null)
		{
		}

		public void TestDoValidate(bool objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			this.DoValidate(objectToValidate, currentTarget, key, validationResults);
		}
	}

	#endregion
}
