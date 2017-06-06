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
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.Modeling.ExtensionProvider.Helpers;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx;

namespace DataContractDsl.Functional.Tests
{
	/// <summary>
	/// Tests DataContract Model Element
	/// </summary>
	[TestClass]
	public class DataContractFixture
	{
		[TestMethod]
		public void DataContractAddRuleAddsObjectExtenderForWcfProvider()
		{			
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(Microsoft.Practices.ServiceFactory.DataContracts.DataContractDslDomainModel));
			DataContractModel dcModel;
			DataContract dcElement;

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				dcModel = store.ElementFactory.CreateElement(DataContractModel.DomainClassId) as DataContractModel;
				dcElement = store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;

				dcModel.Contracts.Add(dcElement);
				dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();
				
				Assert.IsNotNull(dcElement, "DataContract is null");
				Assert.IsNotNull(dcElement.DataContractModel.ImplementationTechnology, "ImplementationTechnology is null");

				transaction.Commit();
			}

			Assert.IsNotNull(dcElement.ObjectExtenderContainer, "ObjectExtenderContainer is null");
			Assert.IsFalse(dcElement.ObjectExtenderContainer.ObjectExtenders.Count == 0, "Extender count is zero");
		}

		[TestMethod]
		public void DataContractAddRuleAddsObjectExtenderForAsmxProvider()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(Microsoft.Practices.ServiceFactory.DataContracts.DataContractDslDomainModel));
			DataContractModel dcModel;
			DataContract dcElement;

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				dcModel = store.ElementFactory.CreateElement(DataContractModel.DomainClassId) as DataContractModel;
				dcElement = store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;

				dcModel.Contracts.Add(dcElement);
				dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();

				Assert.IsNotNull(dcElement, "DataContract is null");
				Assert.IsNotNull(dcElement.DataContractModel.ImplementationTechnology, "ImplementationTechnology is null");

				transaction.Commit();
			}

			Assert.IsNotNull(dcElement.ObjectExtenderContainer, "ObjectExtenderContainer is null");
			Assert.IsFalse(dcElement.ObjectExtenderContainer.ObjectExtenders.Count == 0, "Extender count is zero");
		}

		[TestMethod]
		public void ShouldPersistValuesOnImplementationTechnologyChangeRule()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(Microsoft.Practices.ServiceFactory.DataContracts.DataContractDslDomainModel));
			DataContractModel dcModel;
			DataContract dcElement;

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				dcModel = store.ElementFactory.CreateElement(DataContractModel.DomainClassId) as DataContractModel;
				dcElement = store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;

				dcModel.Contracts.Add(dcElement);
				dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();

				Assert.IsNotNull(dcElement, "DataContract is null");
				Assert.IsNotNull(dcElement.DataContractModel.ImplementationTechnology, "ImplementationTechnology is null");

				// fire rules on commit
				transaction.Commit();
			}

			AsmxDataContract asmxExtender = dcElement.ObjectExtender as AsmxDataContract;
			Assert.IsNotNull(asmxExtender, "ObjectExtender is null");
			// store some value to compare later
			asmxExtender.OrderParts = true;

			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();
				// fire rules on commit
				transaction.Commit();
			}

			// now we should have the WCF extender
			WCFDataContract wcfExtender = dcElement.ObjectExtender as WCFDataContract;
			Assert.IsNotNull(wcfExtender, "ObjectExtender is null");

			// get back the ASMX extender and check values
			using (Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();
				// fire rules on commit
				transaction.Commit();
			}

			asmxExtender = dcElement.ObjectExtender as AsmxDataContract;
			Assert.IsNotNull(asmxExtender, "ObjectExtender is null");
			Assert.IsTrue(asmxExtender.OrderParts, "OrderParts in ASMX extender was not persisted");

			// check for ObjectExtenderContainer final state
			Assert.IsNotNull(dcElement.ObjectExtenderContainer, "ObjectExtenderContainer is null");
			Assert.AreEqual<int>(2, dcElement.ObjectExtenderContainer.ObjectExtenders.Count, "Extender count is not 2");
		}
	}
}