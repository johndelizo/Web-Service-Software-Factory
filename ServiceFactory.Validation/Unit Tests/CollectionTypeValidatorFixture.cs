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
    /// Summary description for CollectionTypeValidatorFixture
    /// </summary>
    [TestClass]
    public class CollectionTypeValidatorFixture : DataContractModelFixture
    {
        [TestMethod]
        public void ReturnFailureForInvalidCollectionType()
        {
            PrimitiveDataType primitive = CreatePrimitiveDataType();
            primitive.ObjectExtender = new Extenders.DataContract.Asmx.AsmxDataElement();
            primitive.CollectionType = typeof(Dictionary<,>);

            TestCollectionTypeValidator validator = new TestCollectionTypeValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(primitive.CollectionType, primitive, "", validationResults);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnSuccessForValidCollectionType()
        {
            PrimitiveDataType primitive = CreatePrimitiveDataType();
            primitive.ObjectExtender = new Extenders.DataContract.Asmx.AsmxDataElement();
            primitive.CollectionType = typeof(List<>);

            TestCollectionTypeValidator validator = new TestCollectionTypeValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(primitive.CollectionType, primitive, "", validationResults);

            Assert.IsTrue(validationResults.IsValid);
        }

        private PrimitiveDataType CreatePrimitiveDataType()
        {
            PrimitiveDataType primitive = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
            primitive.Name = "primitive foo";
            return primitive;
        }
    }

    public class TestCollectionTypeValidator : CollectionTypeValidator
    {
        public TestCollectionTypeValidator() : base(null) { }

        public void TestDoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            DoValidate(objectToValidate, currentTarget, key, validationResults);
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            base.DoValidate(objectToValidate, currentTarget, key, validationResults);
        }
    }
}