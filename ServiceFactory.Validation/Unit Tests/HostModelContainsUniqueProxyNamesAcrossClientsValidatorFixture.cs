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
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.ServiceFactory.HostDesigner;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections.Specialized;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
	public class HostModelContainsUniqueProxyNamesAcrossClientsValidatorFixture
	{
		[TestMethod]
		public void ValidationFailsWhenTwoProxiesHaveSameNameAndProject()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(HostDesignerDomainModel));
			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ClientApplication clientApp1 = new ClientApplication(store,
					new PropertyAssignment(ClientApplication.ImplementationProjectDomainPropertyId, "Project1"));
				ClientApplication clientApp2 = new ClientApplication(store,
					new PropertyAssignment(ClientApplication.ImplementationProjectDomainPropertyId, "Project1"));

				HostDesignerModel model = new HostDesignerModel(store);

				model.ClientApplications.Add(clientApp1);
				model.ClientApplications.Add(clientApp2);

				Proxy proxy1 = new Proxy(store,
					new PropertyAssignment(Proxy.NameDomainPropertyId, "Proxy1"));
				Proxy proxy2 = new Proxy(store,
					new PropertyAssignment(Proxy.NameDomainPropertyId, "Proxy1"));


				clientApp1.Proxies.Add(proxy1);
				clientApp2.Proxies.Add(proxy2);

				TestableHostModelContainsUniqueProxyNamesAcrossClientsValidator validator = new TestableHostModelContainsUniqueProxyNamesAcrossClientsValidator();

				Assert.IsFalse(validator.IsValid(model));


				t.Rollback();
			}
		}

		[TestMethod]
		public void ValidationPassedWhenProjectDiffersButNameIsSame()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(HostDesignerDomainModel));
			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ClientApplication clientApp1 = new ClientApplication(store,
									new PropertyAssignment(ClientApplication.ImplementationProjectDomainPropertyId, "Project1"));
				ClientApplication clientApp2 = new ClientApplication(store,
					new PropertyAssignment(ClientApplication.ImplementationProjectDomainPropertyId, "AnotherProject"));

				HostDesignerModel model = new HostDesignerModel(store);

				model.ClientApplications.Add(clientApp1);
				model.ClientApplications.Add(clientApp2);

				Proxy proxy1 = new Proxy(store,
					new PropertyAssignment(Proxy.NameDomainPropertyId, "Proxy1"));
				Proxy proxy2 = new Proxy(store,
					new PropertyAssignment(Proxy.NameDomainPropertyId, "Proxy1"));


				clientApp1.Proxies.Add(proxy1);
				clientApp2.Proxies.Add(proxy2);

				TestableHostModelContainsUniqueProxyNamesAcrossClientsValidator validator = new TestableHostModelContainsUniqueProxyNamesAcrossClientsValidator();


				t.Rollback();
			}
		}

		[TestMethod]
		public void ValidationOnlyAppliesToServicesReferencesInDifferentHosts()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(HostDesignerDomainModel));
			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ClientApplication clientApp = new ClientApplication(store,
					new PropertyAssignment(ClientApplication.ImplementationProjectDomainPropertyId, "Project1"));

				HostDesignerModel model = new HostDesignerModel(store);

				model.ClientApplications.Add(clientApp);

				Proxy proxy1 = new Proxy(store,
					new PropertyAssignment(Proxy.NameDomainPropertyId, "Proxy1"));
				Proxy proxy2 = new Proxy(store,
					new PropertyAssignment(Proxy.NameDomainPropertyId, "Proxy1"));


				clientApp.Proxies.Add(proxy1);
				clientApp.Proxies.Add(proxy2);

				TestableHostModelContainsUniqueProxyNamesAcrossClientsValidator validator = new TestableHostModelContainsUniqueProxyNamesAcrossClientsValidator();

				Assert.IsTrue(validator.IsValid(model));


				t.Rollback();
			}
		}

		private class TestableHostModelContainsUniqueProxyNamesAcrossClientsValidator : HostModelContainsUniqueProxyNamesAcrossClientsValidator
		{
			public TestableHostModelContainsUniqueProxyNamesAcrossClientsValidator()
				: base(new NameValueCollection())
			{
			}

			public bool IsValid(HostDesignerModel hostModel)
			{
				ValidationResults vrs = new ValidationResults();
				this.DoValidate(hostModel.ClientApplications, hostModel.ClientApplications, "", vrs);

				return new List<ValidationResult>(vrs).Count == 0;
			}

		}
	}
}
