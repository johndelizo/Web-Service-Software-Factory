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
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.UnitTestLibrary;


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
    /// <summary>
    /// Summary description for XsdElementFaultCollectionValidatorFixture
    /// </summary>
    [TestClass]
    public class XsdElementFaultCollectionValidatorFixture : ServiceContractModelFixture
    {
        [TestMethod]
        public void ReturnFailureForInvalidSerializer()
        {
            Operation operation = CreateOperation();
            XsdElementFault fault = CreateXsdElementFault();

            operation.ServiceContractModel.SerializerType = SerializerType.XmlSerializer;
            operation.Faults.Add(fault);

            TestXsdElementFaultCollectionValidator validator = new TestXsdElementFaultCollectionValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(operation.Faults, operation, "", validationResults);

            Assert.IsFalse(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnSuccessForValidSerializer()
        {
            Operation operation = CreateOperation();
            XsdElementFault fault = CreateXsdElementFault();

            operation.ServiceContractModel.SerializerType = SerializerType.DataContractSerializer;
            operation.Faults.Add(fault);

            TestXsdElementFaultCollectionValidator validator = new TestXsdElementFaultCollectionValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(operation.Faults, operation, "", validationResults);

            Assert.IsTrue(validationResults.IsValid);
        }

        private XsdElementFault CreateXsdElementFault()
        {
            XsdElementFault fault = Store.ElementFactory.CreateElement(XsdElementFault.DomainClassId) as XsdElementFault;
            fault.Name = "foo";
            return fault;
        }

        private Operation CreateOperation()
        {
            Operation op = Store.ElementFactory.CreateElement(Operation.DomainClassId) as Operation;
            op.Name = "Operation Foo";

            op.ServiceContractModel = new ServiceContractModel(Store, null);

            return op;
        }

        protected override Type ContractType
        {
            get { return typeof(ServiceContract); }
        }

        protected override string Template
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
    }

    public class TestXsdElementFaultCollectionValidator : XsdElementFaultCollectionValidator
    {
        public TestXsdElementFaultCollectionValidator() : base(null) { }

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