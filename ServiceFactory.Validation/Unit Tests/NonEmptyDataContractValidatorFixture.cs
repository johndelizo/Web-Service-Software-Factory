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
	/// Summary description for NonEmptyDataContractValidatorFixture
    /// </summary>
    [TestClass]
	public class NonEmptyDataContractValidatorFixture : DataContractModelFixture
    {
        [TestMethod]
		public void ReturnFailureForEmptyDataContract()
        {
			DataContractCollection dc = CreateDataContractCollection();

            TestNonEmptyDataContractValidator validator = new TestNonEmptyDataContractValidator();
            ValidationResults validationResults = new ValidationResults();

			validator.TestDoValidate(dc.DataContract, dc, "DataContract", validationResults);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
		public void ReturnSuccessForValidDataContract()
        {
			DataContractCollection dc = CreateDataContractCollection();
			dc.DataContract = CreateDataContract();

            TestNonEmptyDataContractValidator validator = new TestNonEmptyDataContractValidator();
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

		private DataContractBase CreateDataContract()
		{
			DataContract dataContract = Store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;
			dataContract.Name = "fooDataContract";
			return dataContract as DataContractBase;
		}

		class TestNonEmptyDataContractValidator : NonEmptyDataContractValidator
		{
			public TestNonEmptyDataContractValidator() : base(null) { }

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