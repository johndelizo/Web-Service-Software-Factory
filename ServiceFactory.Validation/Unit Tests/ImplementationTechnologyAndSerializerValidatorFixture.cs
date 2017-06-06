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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Reflection;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx;


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
	/// Tests for ImplementationTechnologyAndSerializerValidator
	/// </summary>
	[TestClass]
	public class ImplementationTechnologyAndSerializerValidatorFixture
	{
		[TestMethod]
		public void ReturnSuccessForValidSerializer1()
		{
			Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			Partition partition = new Partition(store);

			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
				ServiceContractModel serviceContractModel = new ServiceContractModel(store);

				serviceContractModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
				serviceContractModel.SerializerType = SerializerType.DataContractSerializer;

				ValidationResults validationResults = new ValidationResults();
				TestImplementationTechnologyAndSerializerValidator validator = new TestImplementationTechnologyAndSerializerValidator();
				validator.TestDoValidate(serviceContractModel.SerializerType, serviceContractModel, null, validationResults);

				Assert.IsTrue(validationResults.IsValid);

				t.Rollback();
			}
		}

		[TestMethod]
		public void ReturnSuccessForValidSerializer2()
		{
			Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			Partition partition = new Partition(store);

			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
				ServiceContractModel serviceContractModel = new ServiceContractModel(store);

				serviceContractModel.ImplementationTechnology = new ServiceContractWCFExtensionProvider();
				serviceContractModel.SerializerType = SerializerType.XmlSerializer;

				ValidationResults validationResults = new ValidationResults();
				TestImplementationTechnologyAndSerializerValidator validator = new TestImplementationTechnologyAndSerializerValidator();
				validator.TestDoValidate(serviceContractModel.SerializerType, serviceContractModel, null, validationResults);

				Assert.IsTrue(validationResults.IsValid);

				t.Rollback();
			}
		}

		[TestMethod]
		public void ReturnSuccessForValidSerializer3()
		{
			Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			Partition partition = new Partition(store);

			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
				ServiceContractModel serviceContractModel = new ServiceContractModel(store);

				serviceContractModel.ImplementationTechnology = new ServiceContractAsmxExtensionProvider();
				serviceContractModel.SerializerType = SerializerType.XmlSerializer;

				ValidationResults validationResults = new ValidationResults();
				TestImplementationTechnologyAndSerializerValidator validator = new TestImplementationTechnologyAndSerializerValidator();
				validator.TestDoValidate(serviceContractModel.SerializerType, serviceContractModel, null, validationResults);

				Assert.IsTrue(validationResults.IsValid);

				t.Rollback();
			}
		}

		[TestMethod]
		public void ReturnSuccessForInvalidValidSerializer()
		{
			Store store = new Store(new MockServiceProvider(), typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
			Partition partition = new Partition(store);

			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
				ServiceContractModel serviceContractModel = new ServiceContractModel(store);

				serviceContractModel.ImplementationTechnology = new ServiceContractAsmxExtensionProvider();
				serviceContractModel.SerializerType = SerializerType.DataContractSerializer;

				ValidationResults validationResults = new ValidationResults();
				TestImplementationTechnologyAndSerializerValidator validator = new TestImplementationTechnologyAndSerializerValidator();
				validator.TestDoValidate(serviceContractModel.SerializerType, serviceContractModel, null, validationResults);

				Assert.IsFalse(validationResults.IsValid);

				t.Rollback();
			}
		}

		public class TestImplementationTechnologyAndSerializerValidator : ImplementationTechnologyAndSerializerValidator
		{
			public void TestDoValidate(SerializerType objectToValidate, object currentTarget, string key, ValidationResults validationResults)
			{
				this.DoValidate(objectToValidate, currentTarget, key, validationResults);
			}
		}
	}
}
