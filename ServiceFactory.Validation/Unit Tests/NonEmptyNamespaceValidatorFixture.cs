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
    /// Summary description for NonEmptyNamespaceValidatorFixture
    /// </summary>
    [TestClass]
    public class NonEmptyNamespaceValidatorFixture : DataContractModelFixture
    {
        [TestMethod]
        public void ReturnFailureForNullNamespace()
        {
            DataContract dc = CreateDataContract();
            dc.Namespace = null;

            TestNonEmptyNamespaceValidator validator = new TestNonEmptyNamespaceValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(dc.Namespace, dc, "Namespace", validationResults);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnFailureForEmptyNamespace()
        {
            DataContract dc = CreateDataContract();

            TestNonEmptyNamespaceValidator validator = new TestNonEmptyNamespaceValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(dc.Namespace, dc, "Namespace", validationResults);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnSuccessForValidNamespace()
        {
            DataContract dc = CreateDataContract();
            dc.Namespace = "valid";

            TestNonEmptyNamespaceValidator validator = new TestNonEmptyNamespaceValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(dc.Namespace, dc, "Namespace", validationResults);

            Assert.IsTrue(validationResults.IsValid);
        }

        private DataContract CreateDataContract()
        {
            DataContract dataContract = Store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;
            dataContract.Name = "foo";
            return dataContract;
        }
    }

    public class TestNonEmptyNamespaceValidator : NonEmptyNamespaceValidator
    {
        public TestNonEmptyNamespaceValidator() : base(null) { }

        public void TestDoValidate(object objectToValidate, Contract currentTarget, string key, ValidationResults validationResults)
        {
            DoValidate(objectToValidate, currentTarget, key, validationResults);
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            base.DoValidate(objectToValidate, currentTarget, key, validationResults);
        }
    }
}