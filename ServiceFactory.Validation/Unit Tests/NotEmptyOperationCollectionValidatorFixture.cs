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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Reflection;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
	/// Tests for NotEmptyOperationCollectionValidator
	/// </summary>
	[TestClass]
	public class NotEmptyOperationCollectionValidatorFixture
	{
		[TestMethod]
		public void ReturnFailureForNull()
		{
			NotEmptyOperationCollectionValidator validator = new NotEmptyOperationCollectionValidator();
			ValidationResults validationResults = validator.Validate(null);

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnSuccessForValidCollection()
		{
			Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			Partition partition = new Partition(store);

			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
				ServiceContractModel serviceContractModel = new ServiceContractModel(store);
				ServiceContract sc = new ServiceContract(store);

				sc.Operations.Add(new Operation(store));
				serviceContractModel.ServiceContracts.Add(sc);

				NotEmptyOperationCollectionValidator validator = new NotEmptyOperationCollectionValidator();
				ValidationResults validationResults = validator.Validate(sc.Operations);

				Assert.IsTrue(validationResults.IsValid);

				t.Rollback();
			}
		}

		[TestMethod]
		public void ReturnFailureForInvalidValidCollection()
		{
			NotEmptyOperationCollectionValidator validator = new NotEmptyOperationCollectionValidator();
			ValidationResults validationResults = validator.Validate(new List<Operation>());

			Assert.IsFalse(validationResults.IsValid);
		}
	}
}
