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
using Microsoft.Practices.ServiceFactory.HostDesigner;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
	public class HostModelContainsUniqueServiceReferencesAcrossHostsValidatorFixture
	{
	
		[TestMethod]
		public void ValidationFailsWhenTwoServiceReferencesHasSameNameAndProject()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(HostDesignerDomainModel));
			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
				HostApplication hostApp1 = new HostApplication(store, 
					new PropertyAssignment(HostApplication.ImplementationProjectDomainPropertyId, "Project1"));
				HostApplication hostApp2 = new HostApplication(store,
					new PropertyAssignment(HostApplication.ImplementationProjectDomainPropertyId, "Project1"));
				
				HostDesignerModel model = new HostDesignerModel(store);
				
				model.HostApplications.Add(hostApp1);
				model.HostApplications.Add(hostApp2);
				
				ServiceReference serviceReference1 = new ServiceReference(store, 
					new PropertyAssignment(ServiceReference.NameDomainPropertyId, "ServiceRef1"));
				ServiceReference serviceReference2 = new ServiceReference(store,
					new PropertyAssignment(ServiceReference.NameDomainPropertyId, "ServiceRef1"));
				
				
				hostApp1.ServiceDescriptions.Add(serviceReference1);
				hostApp2.ServiceDescriptions.Add(serviceReference2);

				TestableHostModelContainsUniqueServiceReferencesAcrossHostsValidator validator = new TestableHostModelContainsUniqueServiceReferencesAcrossHostsValidator();

				Assert.IsFalse(validator.IsValid(model));
				
				
				t.Rollback();
			}
		}

		[TestMethod]
		public void ValidationPassedWhenProjectDiffersButNameIsSame()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(HostDesignerDomainModel));
			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
				HostApplication hostApp1 = new HostApplication(store, 
					new PropertyAssignment(HostApplication.ImplementationProjectDomainPropertyId, "Project1"));
				HostApplication hostApp2 = new HostApplication(store,
					new PropertyAssignment(HostApplication.ImplementationProjectDomainPropertyId, "SomeOtherProject"));
				
				HostDesignerModel model = new HostDesignerModel(store);
				
				model.HostApplications.Add(hostApp1);
				model.HostApplications.Add(hostApp2);
				
				ServiceReference serviceReference1 = new ServiceReference(store, 
					new PropertyAssignment(ServiceReference.NameDomainPropertyId, "ServiceRef1"));
				ServiceReference serviceReference2 = new ServiceReference(store,
					new PropertyAssignment(ServiceReference.NameDomainPropertyId, "ServiceRef1"));
				
				
				hostApp1.ServiceDescriptions.Add(serviceReference1);
				hostApp2.ServiceDescriptions.Add(serviceReference2);

				TestableHostModelContainsUniqueServiceReferencesAcrossHostsValidator validator = new TestableHostModelContainsUniqueServiceReferencesAcrossHostsValidator();

				Assert.IsTrue(validator.IsValid(model));
				
				
				t.Rollback();
			}
		}

		[TestMethod]
		public void ValidationOnlyAppliesToServicesReferencesInDifferentHosts()
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(HostDesignerDomainModel));
			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				HostApplication hostApp1 = new HostApplication(store,
					new PropertyAssignment(HostApplication.ImplementationProjectDomainPropertyId, "Project1"));

				HostDesignerModel model = new HostDesignerModel(store);

				model.HostApplications.Add(hostApp1);

				ServiceReference serviceReference1 = new ServiceReference(store,
					new PropertyAssignment(ServiceReference.NameDomainPropertyId, "ServiceRef1"));
				ServiceReference serviceReference2 = new ServiceReference(store,
					new PropertyAssignment(ServiceReference.NameDomainPropertyId, "ServiceRef1"));


				hostApp1.ServiceDescriptions.Add(serviceReference1);
				hostApp1.ServiceDescriptions.Add(serviceReference2);

				TestableHostModelContainsUniqueServiceReferencesAcrossHostsValidator validator = new TestableHostModelContainsUniqueServiceReferencesAcrossHostsValidator();

				Assert.IsTrue(validator.IsValid(model));


				t.Rollback();
			}
		}
		
		private class TestableHostModelContainsUniqueServiceReferencesAcrossHostsValidator :HostModelContainsUniqueServiceReferencesAcrossHostsValidator
		{
			public TestableHostModelContainsUniqueServiceReferencesAcrossHostsValidator()
			:base(new NameValueCollection())
			{
			}
			
			public bool IsValid(HostDesignerModel hostModel)
			{
				ValidationResults vrs = new ValidationResults();
				this.DoValidate(hostModel.HostApplications, hostModel.HostApplications, "", vrs);
				
				return new List<ValidationResult>(vrs).Count == 0;
			}
			
		}
	}
}
