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
using System.ComponentModel;
using Microsoft.Practices.Modeling.ExtensionProvider.TypeDescription;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DataContractDsl.Functional.Tests
{
	/// <summary>
	/// Summary description for ExtendedTypeDescriptorProviderFixture
	/// </summary>
	[TestClass]
	public class ExtendedTypeDescriptorProviderFixture
	{
		[TestMethod]
		public void ExtendeeObjectShouldContainExtendedProperties()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));
			
			using(Transaction transaction = store.TransactionManager.BeginTransaction())
			{
				DataContract dcElement = store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;

				ExtendedObject obj = new ExtendedObject();
				obj.Field2 = 1;

				dcElement.ObjectExtender = obj;

				Assert.AreEqual(1, TypeDescriptor.GetProperties(dcElement.ObjectExtender).Count, "Properties not injected");
				Assert.AreEqual("Field2", TypeDescriptor.GetProperties(dcElement.ObjectExtender)[0].Name, "Properties not injected");

				transaction.Rollback();
			}
		}

		internal class ExtendedObject
		{
			private int field2;

			public int Field2
			{
				get { return field2; }
				set { field2 = value; }
			}
		}
	}
}
