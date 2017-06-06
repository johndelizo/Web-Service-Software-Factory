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
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;
using System.Collections.Specialized;
using Microsoft.VisualStudio.Modeling.Integration;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
    [TestClass]
    public class CrossDCModelTIandPTMValidatorFixture
    {
        private Store scStore;
        private Store dcStore;
        private DomainModel scDomainModel;
        private DomainModel dcDomainModel;
        private Transaction scTransaction;
        private Transaction dcTransaction;
        private ServiceContractModel scModel;
        private DataContractModel dcModel;
        private DataContract dc;
        private ServiceContract sc;
        private PrimitiveDataType primitiveDataElement;
        private NameValueCollection attributes;
		private DataContractFault fault;
        private Operation operation;
        private MockServiceProvider serviceProvider;

        #region Constants

        const string dataContractModelProjectName = "Project1.model";
        const string dataContractModelFileName = "dc.datacontract";
        const string primitiveDataElementName = "PrimitiveDataElement1";
        const string faultName = "Fault1";
        const string operationContractName = "Operation1";
        const string projectMappingTableName = "pmt.config";

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            serviceProvider = new MockMappingServiceProvider();

            #region Data Contract
            dcStore = new Store(serviceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));
            dcDomainModel = dcStore.GetDomainModel<DataContractDslDomainModel>();
            dcTransaction = dcStore.TransactionManager.BeginTransaction();
            dcModel = (DataContractModel)dcDomainModel.CreateElement(new Partition(dcStore), typeof(DataContractModel), null);
            
            // Specify the Implementation Technology and PMT
            dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();
            dcModel.ProjectMappingTable = projectMappingTableName;
            dc = dcStore.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;
            primitiveDataElement = dcStore.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
            primitiveDataElement.Name = primitiveDataElementName;
            #endregion

            #region Service Contract
            scStore = new Store(serviceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
            scDomainModel = scStore.GetDomainModel<ServiceContractDslDomainModel>();
            scTransaction = scStore.TransactionManager.BeginTransaction();
            scModel = (ServiceContractModel)scDomainModel.CreateElement(new Partition(scStore), typeof(ServiceContractModel), null);
            scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
            scModel.ProjectMappingTable = projectMappingTableName;
            sc = scStore.ElementFactory.CreateElement(ServiceContract.DomainClassId) as ServiceContract;
            #endregion

            #region Validator
            // Initialize validator's config
            attributes = new NameValueCollection();
            attributes.Add("elementNameProperty", "Name");
            #endregion

            #region Simulate Model
            //Create the moniker
            //mel://[DSLNAMESPACE]\[MODELELEMENTTYPE]\[MODELELEMENT.GUID]@[PROJECT]\[MODELFILE]
            string requestMoniker = string.Format(@"mel://{0}\{1}\{2}@{3}\{4}",
                primitiveDataElement.GetType().Namespace,
                primitiveDataElement.GetType().Name,
                primitiveDataElement.Id.ToString(),
                dataContractModelProjectName, dataContractModelFileName);

            // Add a DC to the model
            dc.DataMembers.Add(primitiveDataElement);
            dcModel.Contracts.Add(dc);

            // Create a Fault that references the Data Contract
			fault = scStore.ElementFactory.CreateElement(DataContractFault.DomainClassId) as DataContractFault;
            fault.Name = faultName;
            fault.Type = new MockModelBusReference(primitiveDataElement);

            // Create an Operation
            operation = scStore.ElementFactory.CreateElement(Operation.DomainClassId) as Operation;
            operation.Name = operationContractName;
            operation.Faults.Add(fault);
            sc.Operations.Add(operation);

            #endregion
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scTransaction.Rollback();
            dcTransaction.Rollback();
        }

        [TestMethod]
        public void DoValidateFailsForEmptyPMT()
        {
            dcModel.ProjectMappingTable = string.Empty;
            ValidationResults results = new ValidationResults();
            TestCrossDataContractModelTIandPMTValidator target = new TestCrossDataContractModelTIandPMTValidator(attributes);
            target.TestDoValidate(fault.Type, fault, string.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void DoValidateFailsForEmptyImplementationTechnology()
        {
            dcModel.ImplementationTechnology = null;

            ValidationResults results = new ValidationResults();
            TestCrossDataContractModelTIandPMTValidator target = new TestCrossDataContractModelTIandPMTValidator(attributes);
            target.TestDoValidate(fault.Type, fault, string.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void DoValidateSucceedsForNonEmptyImplementationTechnology()
        {
            ValidationResults results = new ValidationResults();
            TestCrossDataContractModelTIandPMTValidator target = new TestCrossDataContractModelTIandPMTValidator(attributes);
            target.TestDoValidate(fault.Type, fault, string.Empty, results);

            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void DoValidateSucceedsForNonEmptyPMT()
        {
            ValidationResults results = new ValidationResults();
            TestCrossDataContractModelTIandPMTValidator target = new TestCrossDataContractModelTIandPMTValidator(attributes);
            target.TestDoValidate(fault.Type, fault, string.Empty, results);

            Assert.IsTrue(results.IsValid);
        }

        #region Validator
        public class TestCrossDataContractModelTIandPMTValidator : CrossDataContractModelTIandPMTValidator
        {
           public TestCrossDataContractModelTIandPMTValidator(System.Collections.Specialized.NameValueCollection attributes) : base(attributes)
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
