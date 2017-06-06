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
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
    [TestClass]
    public class FaultCollectionCrossModelValidatorFixture
    {
        private Store scStore;
        private Store dcStore;
        private DomainModel scDomainModel;
        private DomainModel dcDomainModel;
        private Transaction scTransaction;
        private Transaction dcTransaction;
        private ServiceContractModel scModel;
        private DataContractModel dcModel;
        private Operation operation;
        private FaultContract fc;
		private DataContractFault dcfault;
        private MockServiceProvider serviceProvider;
		private NameValueCollection attributes;

        #region Constants
        const string dataContractModelProjectName = "Project1.model";
        const string dataContractModelFileName = "fc.datacontract";
        const string dcfaultName = "DataContractFault1";
        const string projectMappingTableName = "pmt.config";
        const string operationName = "Operation1";
        const string faultContractName = "FaultContract1";
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            serviceProvider = new MockMappingServiceProvider();

			attributes = new NameValueCollection();
			attributes.Add("elementNameProperty", "Name");

            #region Data Contract
            dcStore = new Store(serviceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));
            dcDomainModel = dcStore.GetDomainModel<DataContractDslDomainModel>();
            dcTransaction = dcStore.TransactionManager.BeginTransaction();
            dcModel = (DataContractModel)dcDomainModel.CreateElement(new Partition(dcStore), typeof(DataContractModel), null);
            dcModel.ProjectMappingTable = projectMappingTableName;
            fc = dcStore.ElementFactory.CreateElement(FaultContract.DomainClassId) as FaultContract;
            fc.Name = faultContractName;
			dcModel.Contracts.Add(fc);
            #endregion

			#region Service Contract
			scStore = new Store(serviceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			scDomainModel = scStore.GetDomainModel<ServiceContractDslDomainModel>();
			scTransaction = scStore.TransactionManager.BeginTransaction();
			scModel = (ServiceContractModel)scDomainModel.CreateElement(new Partition(scStore), typeof(ServiceContractModel), null);
			scModel.ProjectMappingTable = projectMappingTableName;
            operation = scStore.ElementFactory.CreateElement(Operation.DomainClassId) as Operation;
            operation.Name = operationName;

            //Create the moniker
            //mel://[DSLNAMESPACE]\[MODELELEMENTTYPE]\[MODELELEMENT.GUID]@[PROJECT]\[MODELFILE]
            string requestMoniker = string.Format(@"mel://{0}\{1}\{2}@{3}\{4}",
                fc.GetType().Namespace,
                fc.GetType().Name,
                fc.Id.ToString(),
                dataContractModelProjectName, dataContractModelFileName);

            dcfault = scStore.ElementFactory.CreateElement(DataContractFault.DomainClassId) as DataContractFault;
			dcfault.Name = dcfaultName;
            dcfault.Type = new MockModelBusReference(fc);
            
            operation.Faults.Add(dcfault);
			scModel.Operations.Add(operation);
            #endregion
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scTransaction.Rollback();
            dcTransaction.Rollback();
        }

        [TestMethod]
        public void TestIvValidFaultSerializer()
        {
			scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
			scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer;
			dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();

			ValidationResults results = new ValidationResults();
            TestFaultCollectionCrossModelValidator validator = new TestFaultCollectionCrossModelValidator(null);
			validator.TestDoValidate(operation.Faults, operation, string.Empty, results);

			Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void TestvnValidScenarioForWCF1()
        {
            scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
            scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.DataContractSerializer;
            dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();

            ValidationResults results = new ValidationResults();
            TestFaultCollectionCrossModelValidator validator = new TestFaultCollectionCrossModelValidator(null);
            validator.TestDoValidate(operation.Faults, operation, string.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void TestValidScenarioForWCF2()
        {
            scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
            scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.DataContractSerializer;
            dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();

            ValidationResults results = new ValidationResults();
            TestFaultCollectionCrossModelValidator validator = new TestFaultCollectionCrossModelValidator(null);
            validator.TestDoValidate(operation.Faults, operation, string.Empty, results);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestValidScenarioForASMX()
        {
            scModel.ImplementationTechnology = new ServiceContractAsmxExtensionProvider();
            scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer;
            dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();

            ValidationResults results = new ValidationResults();
            TestFaultCollectionCrossModelValidator validator = new TestFaultCollectionCrossModelValidator(null);
            validator.TestDoValidate(operation.Faults, operation, string.Empty, results);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestInvalidScenarioForASMX()
        {
            scModel.ImplementationTechnology = new ServiceContractAsmxExtensionProvider();
            scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer;
            dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();

            ValidationResults results = new ValidationResults();
            TestFaultCollectionCrossModelValidator validator = new TestFaultCollectionCrossModelValidator(null);
            validator.TestDoValidate(operation.Faults, operation, string.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

		[TestMethod]
		public void ValidatorShouldHandleNoImplementationTechnologyGracefully()
		{
			ValidationResults results = new ValidationResults();
			TestFaultCollectionCrossModelValidator validator = new TestFaultCollectionCrossModelValidator(null);
			validator.TestDoValidate(operation.Faults, operation, string.Empty, results);

			Assert.IsTrue(results.IsValid);
		}

        [TestMethod]
        public void TestInvalidScenarioForASMXAndDataContractSerializer()
        {
            scModel.ImplementationTechnology = new ServiceContractAsmxExtensionProvider();
            scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.DataContractSerializer;
            dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();

            ValidationResults results = new ValidationResults();
            TestFaultCollectionCrossModelValidator validator = new TestFaultCollectionCrossModelValidator(null);
            validator.TestDoValidate(operation.Faults, operation, string.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

		#region Validator
		public class TestFaultCollectionCrossModelValidator : FaultCollectionCrossModelValidator
        {
            public TestFaultCollectionCrossModelValidator(System.Collections.Specialized.NameValueCollection attributes)
				: base(attributes)
           {
           }

           public void TestDoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
           {
               this.DoValidate(objectToValidate, currentTarget, key, validationResults);
           }
        }
        #endregion
    }
}
