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
using Microsoft.VisualStudio.Modeling.Integration;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
    [TestClass]
	public class ImplementationTechnologyAndSerializerCrossModelValidatorFixture
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
        private Message msg;
        private PrimitiveDataType primitiveDataElement;
		private DataContractMessagePart part;
        private MockServiceProvider serviceProvider;
		private NameValueCollection attributes;

        #region Constants
        const string dataContractModelProjectName = "Project1.model";
        const string dataContractModelFileName = "dc.datacontract";
        const string primitiveDataElementName = "PrimitiveDataElement1";
        const string partName = "Part1";
        const string projectMappingTableName = "pmt.config";
		const string messageName = "Message1";
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
            dc = dcStore.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;
			primitiveDataElement = dcStore.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
            primitiveDataElement.Name = primitiveDataElementName;
			dc.DataMembers.Add(primitiveDataElement);
			dcModel.Contracts.Add(dc);
            #endregion

			#region Service Contract
			scStore = new Store(serviceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			scDomainModel = scStore.GetDomainModel<ServiceContractDslDomainModel>();
			scTransaction = scStore.TransactionManager.BeginTransaction();
			scModel = (ServiceContractModel)scDomainModel.CreateElement(new Partition(scStore), typeof(ServiceContractModel), null);
			scModel.ProjectMappingTable = projectMappingTableName;
			msg = scStore.ElementFactory.CreateElement(Message.DomainClassId) as Message;
			msg.Name = messageName;

            //Create the moniker
            //mel://[DSLNAMESPACE]\[MODELELEMENTTYPE]\[MODELELEMENT.GUID]@[PROJECT]\[MODELFILE]
            string requestMoniker = string.Format(@"mel://{0}\{1}\{2}@{3}\{4}",
                primitiveDataElement.GetType().Namespace,
                primitiveDataElement.GetType().Name,
                primitiveDataElement.Id.ToString(),
                dataContractModelProjectName, dataContractModelFileName);

			part = scStore.ElementFactory.CreateElement(DataContractMessagePart.DomainClassId) as DataContractMessagePart;
			part.Name = partName;
            part.Type = new MockModelBusReference(primitiveDataElement);
			
            msg.MessageParts.Add(part);
			scModel.Messages.Add(msg);
            #endregion
        }

        [TestCleanup]
        public void TestCleanup()
        {
            scTransaction.Rollback();
            dcTransaction.Rollback();
        }

		[TestMethod]
		public void TestValidScenario2()
		{
			scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
			scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.DataContractSerializer;
			dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();

			ValidationResults results = new ValidationResults();
			TestImplementationTechnologyAndSerializerCrossModelValidator validator = new TestImplementationTechnologyAndSerializerCrossModelValidator(attributes);
			validator.TestDoValidate(part.Type, part, string.Empty, results);

			Assert.IsTrue(results.IsValid);
		}

		[TestMethod]
		public void TestValidScenario3()
		{
			scModel.ImplementationTechnology = new ServiceContractAsmxExtensionProvider();
			scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer;
			dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();

			ValidationResults results = new ValidationResults();
			TestImplementationTechnologyAndSerializerCrossModelValidator validator = new TestImplementationTechnologyAndSerializerCrossModelValidator(attributes);
			validator.TestDoValidate(part.Type, part, string.Empty, results);

			Assert.IsTrue(results.IsValid);
		}

		[TestMethod]
		public void TestValidScenario4()
		{
			scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
			scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer;
			dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();

			ValidationResults results = new ValidationResults();
			TestImplementationTechnologyAndSerializerCrossModelValidator validator = new TestImplementationTechnologyAndSerializerCrossModelValidator(attributes);
			validator.TestDoValidate(part.Type, part, string.Empty, results);

			Assert.IsTrue(results.IsValid);
		}

		[TestMethod]
		public void TestInvalidScenario1()
		{
			scModel.ImplementationTechnology = new ServiceContractAsmxExtensionProvider();
			scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer;
			dcModel.ImplementationTechnology = new DataContractWcfExtensionProvider();

			ValidationResults results = new ValidationResults();
			TestImplementationTechnologyAndSerializerCrossModelValidator validator = new TestImplementationTechnologyAndSerializerCrossModelValidator(attributes);
			validator.TestDoValidate(part.Type, part, string.Empty, results);

			Assert.IsFalse(results.IsValid);
		}

		[TestMethod]
		public void TestInvalidScenario2()
		{
			scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
			scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.DataContractSerializer;
			dcModel.ImplementationTechnology = new DataContractAsmxExtensionProvider();

			ValidationResults results = new ValidationResults();
			TestImplementationTechnologyAndSerializerCrossModelValidator validator = new TestImplementationTechnologyAndSerializerCrossModelValidator(attributes);
			validator.TestDoValidate(part.Type, part, string.Empty, results);

			Assert.IsFalse(results.IsValid);
		}

        [TestMethod]
        public void TestInvalidExtensionAndSerializer()
        {
            scModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
            scModel.SerializerType = Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer;
            dcModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();

            ValidationResults results = new ValidationResults();
            TestImplementationTechnologyAndSerializerCrossModelValidator validator = new TestImplementationTechnologyAndSerializerCrossModelValidator(attributes);
            validator.TestDoValidate(part.Type, part, string.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

		#region Validator
		public class TestImplementationTechnologyAndSerializerCrossModelValidator : ImplementationTechnologyAndSerializerCrossModelValidator
        {
			public TestImplementationTechnologyAndSerializerCrossModelValidator(System.Collections.Specialized.NameValueCollection attributes)
				: base(attributes)
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
