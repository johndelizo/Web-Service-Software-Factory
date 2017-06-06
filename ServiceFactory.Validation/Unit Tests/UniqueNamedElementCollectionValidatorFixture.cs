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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
	public class UniqueNamedElementCollectionValidatorFixture
	{
		[TestMethod]
		public void GetObjectNameReturnsCorrectNameForNamedModelElement()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			ServiceContractModel model;
			ServiceContract element;

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				string expected = "My Name";
				model = store.ElementFactory.CreateElement(ServiceContractModel.DomainClassId) as ServiceContractModel;
				element = store.ElementFactory.CreateElement(ServiceContract.DomainClassId) as ServiceContract;

				element.Name = expected;

				UniqueNamedElementCollectionValidator<ServiceContract> target = new UniqueNamedElementCollectionValidator<ServiceContract>(null);

				string actual = target.GetObjectName(element);
				Assert.AreEqual(expected, actual);

				transaction.Commit();
			}
		}

		[TestMethod]
		public void GetObjectNameReturnsReturnsTypeNameForUnnamedModelElement()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			ServiceContractModel model;

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				model = store.ElementFactory.CreateElement(ServiceContractModel.DomainClassId) as ServiceContractModel;

				UniqueNamedElementCollectionValidator<ServiceContractModel> target = new UniqueNamedElementCollectionValidator<ServiceContractModel>(null);

				string actual = target.GetObjectName(model);
				Assert.AreEqual("Service Contract Model", actual);

				transaction.Commit();
			}
		}
	}
}
