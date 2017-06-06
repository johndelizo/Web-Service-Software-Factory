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
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.UnitTestLibrary;


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
    /// <summary>
	/// Summary description for NonRecursiveDataContractValidatorFixture
    /// </summary>
    [TestClass]
	public class NonRecursiveDataContractValidatorFixture : DataContractModelFixture
    {
        [TestMethod]
		public void ReturnFailureForSelfReferencedDataContract()
        {
			DataContractCollection dc = CreateDataContractCollection();
			dc.DataContract = dc;
			TestNonRecursiveDataContractValidator validator = new TestNonRecursiveDataContractValidator();
            ValidationResults validationResults = new ValidationResults();

			validator.TestDoValidate(dc.DataContract, dc, "DataContract", validationResults);

            Assert.IsFalse(validationResults.IsValid);
        }

		[TestMethod]
		public void ReturnFailureForRecursiveDataContract()
		{
			DataContractCollection dc = CreateDataContractCollection();
			dc.Name = "dc";
			DataContractCollection dc2 = CreateDataContractCollection();
			dc2.Name = "dc2";
			dc2.DataContract = dc;
			dc.DataContract = dc2;
			TestNonRecursiveDataContractValidator validator = new TestNonRecursiveDataContractValidator();
			ValidationResults validationResults = new ValidationResults();

			validator.TestDoValidate(dc.DataContract, dc, "DataContract", validationResults);

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnSuccessForEmtpyDataContract()
		{
			DataContractCollection dc = CreateDataContractCollection();
			TestNonRecursiveDataContractValidator validator = new TestNonRecursiveDataContractValidator();
			ValidationResults validationResults = new ValidationResults();

			validator.TestDoValidate(dc.DataContract, dc, "DataContract", validationResults);

			Assert.IsTrue(validationResults.IsValid);
		}

        [TestMethod]
		public void ReturnSuccessForValidDataContract()
        {
			DataContractCollection dc = CreateDataContractCollection();
			dc.DataContract = CreateDataContract();

			TestNonRecursiveDataContractValidator validator = new TestNonRecursiveDataContractValidator();
            ValidationResults validationResults = new ValidationResults();

			validator.TestDoValidate(dc.DataContract, dc, "DataContract", validationResults);

            Assert.IsTrue(validationResults.IsValid);
		}

		#region Internal Implementation

		private DataContractCollection CreateDataContractCollection()
		{
			DataContractCollection dataContractCol = Store.ElementFactory.CreateElement(DataContractCollection.DomainClassId) as DataContractCollection;
			dataContractCol.Name = "fooDataContractCollection";
			return dataContractCol;
		}

		private DataContract CreateDataContract()
        {
            DataContract dataContract = Store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;
			dataContract.Name = "fooDataContract";
            return dataContract;
        }

		class TestNonRecursiveDataContractValidator : NonRecursiveDataContractValidator
		{
			public TestNonRecursiveDataContractValidator() : base(null) { }

			public void TestDoValidate(object objectToValidate, Contract currentTarget, string key, ValidationResults validationResults)
			{
				DoValidate(objectToValidate, currentTarget, key, validationResults);
			}

            public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
			{
				base.DoValidate(objectToValidate, currentTarget, key, validationResults);
			}
		}

		#endregion
	}
}