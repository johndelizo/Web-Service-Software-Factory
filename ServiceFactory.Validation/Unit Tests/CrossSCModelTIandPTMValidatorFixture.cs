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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.ServiceFactory.Extenders.HostDesigner.Wcf;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;
using Microsoft.Practices.ServiceFactory.HostDesigner;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using Microsoft.VisualStudio.Modeling.Integration;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
    [TestClass]
    public class CrossSCModelTIandPTMValidatorFixture
    {
        private Store scStore;
        private Store hdStore;
        private DomainModel scDomainModel;
        private DomainModel hdDomainModel;
        private Transaction scTransaction;
        private Transaction hdTransaction;
        private ServiceContractModel scModel;
        private HostDesignerModel hdModel;
        private NameValueCollection attributes;
        private MockServiceProvider serviceProvider;
		private ServiceReference reference;

        #region Constants

        const string serviceContractModelProjectName = "Project1.model";
        const string serviceContractModelFileName = "sc.servicecontract";
        const string projectMappingTableName = "pmt.config";
		const string serviceContractName = "ServiceContract1";
		const string serviceMelReferenceName = "ServiceMelReference1";

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            serviceProvider = new MockMappingServiceProvider();

			scStore = new Store(serviceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));			
			scDomainModel = scStore.GetDomainModel<ServiceContractDslDomainModel>();
			scTransaction = scStore.TransactionManager.BeginTransaction();

			scModel = (ServiceContractModel)scDomainModel.CreateElement(new Partition(scStore), typeof(ServiceContractModel), null);
			scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
			scModel.ProjectMappingTable = projectMappingTableName;

			ServiceContract sc = scStore.ElementFactory.CreateElement(ServiceContract.DomainClassId) as ServiceContract;
			sc.Name = serviceContractName;

			scModel.ServiceContracts.Add(sc);

			hdStore = new Store(serviceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(HostDesignerDomainModel));
			hdDomainModel = hdStore.GetDomainModel<HostDesignerDomainModel>();
            hdTransaction = hdStore.TransactionManager.BeginTransaction();
			hdModel = (HostDesignerModel)hdDomainModel.CreateElement(new Partition(hdStore), typeof(HostDesignerModel), null);

			HostApplication app = (HostApplication)hdStore.ElementFactory.CreateElement(HostApplication.DomainClassId);

			app.ImplementationTechnology = new HostDesignerWcfExtensionProvider();

			reference = (ServiceReference)hdStore.ElementFactory.CreateElement(ServiceReference.DomainClassId);

			//mel://[DSLNAMESPACE]\[MODELELEMENTTYPE]\[MODELELEMENT.GUID]@[PROJECT]\[MODELFILE]
			string serviceMoniker = string.Format(@"mel://{0}\{1}\{2}@{3}\{4}",
				sc.GetType().Namespace,
				serviceContractName,
				sc.Id.ToString(),
				serviceContractModelProjectName, serviceContractModelFileName);

			reference.Name = serviceMelReferenceName;
            reference.ServiceImplementationType = new MockModelBusReference(sc);

			app.ServiceDescriptions.Add(reference);

			// Initialize validator's config
            attributes = new NameValueCollection();
            attributes.Add("elementNameProperty", "Name");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scTransaction.Rollback();
            hdTransaction.Rollback();
        }

        [TestMethod]
        public void DoValidateFailsForEmptyPMT()
        {
            scModel.ProjectMappingTable = string.Empty;
            ValidationResults results = new ValidationResults();
            TestCrossServiceContractModelTIandPMTValidator target = new TestCrossServiceContractModelTIandPMTValidator(attributes);
			target.TestDoValidate(reference.ServiceImplementationType, reference, string.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void DoValidateFailsForEmptyImplementationTechnology()
        {
			scModel.ImplementationTechnology = null;

            ValidationResults results = new ValidationResults();
            TestCrossServiceContractModelTIandPMTValidator target = new TestCrossServiceContractModelTIandPMTValidator(attributes);
			target.TestDoValidate(reference.ServiceImplementationType, reference, string.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void DoValidateSucceedsForNonEmptyImplementationTechnology()
        {
            ValidationResults results = new ValidationResults();
            TestCrossServiceContractModelTIandPMTValidator target = new TestCrossServiceContractModelTIandPMTValidator(attributes);
			target.TestDoValidate(reference.ServiceImplementationType, reference, string.Empty, results);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void DoValidateSucceedsForNonEmptyPMT()
        {
            ValidationResults results = new ValidationResults();
            TestCrossServiceContractModelTIandPMTValidator target = new TestCrossServiceContractModelTIandPMTValidator(attributes);
			target.TestDoValidate(reference.ServiceImplementationType, reference, string.Empty, results);

            Assert.IsTrue(results.IsValid);
        }

        #region Validator
        public class TestCrossServiceContractModelTIandPMTValidator : CrossServiceContractModelTIandPMTValidator
        {
           public TestCrossServiceContractModelTIandPMTValidator(System.Collections.Specialized.NameValueCollection attributes) : base(attributes)
           {
           }

           public void TestDoValidate(ModelBusReference objectToValidate, object currentTarget, string key, ValidationResults validationResults)
           {
               this.DoValidate(objectToValidate, currentTarget, key, validationResults);
           }
        }
        #endregion
    }
}
