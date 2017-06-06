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
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.UnitTestLibrary;

namespace ServiceContractDsl.Tests
{
	public abstract class ServiceContractModelFixture: ModelFixture
	{	
		private Store store = null;
		protected override Store Store
		{
			get
			{
				if ( store==null )
				{
					store = new Store(ServiceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
				}
				return store;
			}
		}

		private ServiceContractDslDomainModel dm = null;
		protected override DomainModel DomainModel
		{
			get
			{
				if ( dm==null )
				{
					dm = Store.GetDomainModel<ServiceContractDslDomainModel>();
				}
				return dm;
			}
		}
	}
}
