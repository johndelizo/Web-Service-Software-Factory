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
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
    /// Tests for UniqueOrderPropertyCollectionValidatorFixture
	/// </summary>
	[TestClass]
    public class UniqueOrderPropertyCollectionValidatorFixture
	{
		[TestMethod]
		public void ReturnFailureForDuplicateOrders()
		{
            Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));

            using (Transaction t = store.TransactionManager.BeginTransaction())
            {
                DataContract dcElement = store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;

                PrimitiveDataType element1 = store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
                WCFDataElement extension = new WCFDataElement();
                extension.Order = 0;
                element1.ObjectExtender = extension;

                dcElement.DataMembers.Add(element1);

                PrimitiveDataType element2 = store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
                extension.Order = 0;
                element2.ObjectExtender = extension;

                dcElement.DataMembers.Add(element2);

                UniqueOrderPropertyCollectionValidator validator = new UniqueOrderPropertyCollectionValidator(null);
                ValidationResults validationResults = validator.Validate(dcElement.DataMembers);

                Assert.IsFalse(validationResults.IsValid);

                t.Rollback();
            }
		}

		[TestMethod]
		public void ReturnSuccessForValidCollection()
		{
            Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));

            using (Transaction t = store.TransactionManager.BeginTransaction())
            {
                DataContract dcElement = store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;

                PrimitiveDataType element1 = store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
                WCFDataElement extension = new WCFDataElement();
                extension.Order = 0;
                element1.ObjectExtender = extension;

                dcElement.DataMembers.Add(element1);

                PrimitiveDataType element2 = store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
                extension.Order = 2;
                element2.ObjectExtender = extension;

                dcElement.DataMembers.Add(element2);

                UniqueOrderPropertyCollectionValidator validator = new UniqueOrderPropertyCollectionValidator(null);
                ValidationResults validationResults = validator.Validate(dcElement.DataMembers);

                Assert.IsFalse(validationResults.IsValid);

                t.Rollback();
            }
		}
	}
}
