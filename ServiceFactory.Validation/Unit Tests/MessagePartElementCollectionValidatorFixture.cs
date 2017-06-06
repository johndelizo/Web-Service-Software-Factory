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

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
	public class MessagePartElementCollectionValidatorFixture
	{
		[TestMethod]
		public void DoValidateCollectionItemFailsForDuplicateNamedElements()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			ServiceContractModel model;

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				string name = "Duplicate Name";
				model = store.ElementFactory.CreateElement(ServiceContractModel.DomainClassId) as ServiceContractModel;

				Message contract = store.ElementFactory.CreateElement(Message.DomainClassId) as Message;
				contract.Name = name;

				MessagePart part = store.ElementFactory.CreateElement(PrimitiveMessagePart.DomainClassId) as PrimitiveMessagePart;
				part.Name = name;

				contract.MessageParts.Add(part);

				TestableMessagePartElementCollectionValidator target = new TestableMessagePartElementCollectionValidator();

				ValidationResults results = new ValidationResults();
				target.TestDoValidateCollectionItem(part, contract, String.Empty, results);

				Assert.IsFalse(results.IsValid);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void DoValidateCollectionItemSucceedsForUniqueNamedElements()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			ServiceContractModel model;

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				model = store.ElementFactory.CreateElement(ServiceContractModel.DomainClassId) as ServiceContractModel;

				Message contract = store.ElementFactory.CreateElement(Message.DomainClassId) as Message;
				contract.Name = "Contract Name";

				MessagePart part = store.ElementFactory.CreateElement(PrimitiveMessagePart.DomainClassId) as PrimitiveMessagePart;
				part.Name = "Part Name";

				contract.MessageParts.Add(part);

				TestableMessagePartElementCollectionValidator target = new TestableMessagePartElementCollectionValidator();

				ValidationResults results = new ValidationResults();
				target.TestDoValidateCollectionItem(part, contract, String.Empty, results);

				Assert.IsTrue(results.IsValid);

				transaction.Commit();
			}
		}
	}

	public class TestableMessagePartElementCollectionValidator : MessagePartElementCollectionValidator
	{
		public TestableMessagePartElementCollectionValidator()
			: base(null)
		{
		}

		public void TestDoValidateCollectionItem(MessagePart objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			this.DoValidateCollectionItem(objectToValidate, currentTarget, key, validationResults);
		}
	}
}
