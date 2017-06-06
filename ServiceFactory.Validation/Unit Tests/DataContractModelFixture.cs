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
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	public abstract class DataContractModelFixture : ModelFixture
	{
		private Store store = null;
		private DataContractDslDomainModel dm = null;

		protected override Store Store
		{
			get
			{
				if (store == null)
				{
					store = new Store(ServiceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));
				}
				return store;
			}
		}

		protected override DomainModel DomainModel
		{
			get
			{
				if (dm == null)
				{
					dm = Store.GetDomainModel<DataContractDslDomainModel>();
				}
				return dm;
			}
		}

		protected override Type ContractType
		{
			get { throw new NotImplementedException(); }
		}

		protected override string Template
		{
			get { throw new NotImplementedException(); }
		}
	}
}
