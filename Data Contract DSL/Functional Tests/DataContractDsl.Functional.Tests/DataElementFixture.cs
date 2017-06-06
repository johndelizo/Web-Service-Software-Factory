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

namespace DataContractDsl.Functional.Tests
{
	/// <summary>
	/// Summary description for DataElementFixture
	/// </summary>
	[TestClass]
	public class DataElementFixture
	{
		[TestMethod]
		public void DataElementAddRuleAddsObjectExtender()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(Microsoft.Practices.ServiceFactory.DataContracts.DataContractDslDomainModel));

			DataContractModel dcModel;
			DataContract dataContract;
			DataMember dataElement;

			using(Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				dcModel = store.ElementFactory.CreateElement(DataContractModel.DomainClassId) as DataContractModel;
				dataContract = store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;
				dataElement = store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as DataMember;

				dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();
				dcModel.Contracts.Add(dataContract);
				dataContract.DataMembers.Add(dataElement);
				
				Assert.IsNotNull(dataElement, "DataContract is null");
				Assert.IsNotNull(dataElement.DataContract.DataContractModel.ImplementationTechnology, "ImplementationTechnology is null");

				transaction.Commit();
			}

			Assert.IsNotNull(dataElement.ObjectExtenderContainer, "ObjectExtenderContainer is null");
			Assert.IsFalse(dataElement.ObjectExtenderContainer.ObjectExtenders.Count == 0, "Extender count is zero");
		}
	}
}
