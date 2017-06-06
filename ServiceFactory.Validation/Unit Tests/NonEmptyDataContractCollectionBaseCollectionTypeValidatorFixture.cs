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
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
    /// Tests for NonEmptyDataContractCollectionBaseCollectionTypeValidator
	/// </summary>
	[TestClass]
    public class NonEmptyDataContractCollectionBaseCollectionTypeValidatorFixture
	{
		[TestMethod]
		public void ReturnSuccessForValidCollection()
		{
			Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));

			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
                DataContractCollection collection = store.ElementFactory.CreateElement(DataContractCollection.DomainClassId) as DataContractCollection;

                WCFDataContractCollection extensionProvider  = new WCFDataContractCollection();

                extensionProvider.CollectionType = typeof(List<>);

                collection.ObjectExtender = extensionProvider;

                NonEmptyDataContractCollectionBaseCollectionTypeValidator validator = new NonEmptyDataContractCollectionBaseCollectionTypeValidator(null);

                ValidationResults validationResults = validator.Validate(collection);

				Assert.IsTrue(validationResults.IsValid);

				t.Rollback();
			}
		}

        [TestMethod]
        public void ReturnFailureForInvalidValidCollection()
        {
            Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));

            using (Transaction t = store.TransactionManager.BeginTransaction())
            {
                DataContractCollection collection = store.ElementFactory.CreateElement(DataContractCollection.DomainClassId) as DataContractCollection;

                collection.ObjectExtender = new WCFDataContractCollection();

                NonEmptyDataContractCollectionBaseCollectionTypeValidator validator = new NonEmptyDataContractCollectionBaseCollectionTypeValidator(null);

                ValidationResults validationResults = validator.Validate(collection);

                Assert.IsFalse(validationResults.IsValid);

                t.Rollback();
            }
        }
	}
}
