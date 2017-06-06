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
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf;
using Microsoft.Practices.ServiceFactory.ServiceContracts;


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
    /// <summary>
    /// Summary description for IsWrappedValidatorFixture
    /// </summary>
    [TestClass]
    public class IsWrappedValidatorFixture : ServiceContractModelFixture
    {
        [TestMethod]
        public void ReturnSucceedForIsWrappedWithOneBodyPart()
        {
            Message mc = CreateMessageContract();
            PrimitiveMessagePart part = CreatePrimitiveMessagePart();
            mc.MessageParts.Add(part);

            TestIsWrappedValidator validator = new TestIsWrappedValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(false, mc, "IsWrapped", validationResults);

            Assert.IsTrue(validationResults.IsValid);
        }

        [TestMethod]
        public void ReturnFailureForIsWrappedWithBodyParts()
        {
            Message mc = CreateMessageContract();
            PrimitiveMessagePart part = CreatePrimitiveMessagePart();
            PrimitiveMessagePart part2 = CreatePrimitiveMessagePart();
            part2.Name = "Part2";
            
            mc.MessageParts.Add(part);
            mc.MessageParts.Add(part2);

            TestIsWrappedValidator validator = new TestIsWrappedValidator();
            ValidationResults validationResults = new ValidationResults();

            validator.TestDoValidate(false, mc, "IsWrapped", validationResults);

            Assert.IsFalse(validationResults.IsValid);
        }

        private Message CreateMessageContract()
        {
            Message messageContract = Store.ElementFactory.CreateElement(Message.DomainClassId) as Message;
            WCFMessageContract wcfMessageContract = new WCFMessageContract();

            wcfMessageContract.IsWrapped = false;
            messageContract.Name = "foo";
            messageContract.ObjectExtender = wcfMessageContract;

            return messageContract;
        }

        private PrimitiveMessagePart CreatePrimitiveMessagePart()
        {
            PrimitiveMessagePart part = Store.ElementFactory.CreateElement(PrimitiveMessagePart.DomainClassId) as PrimitiveMessagePart;
            part.Name = "Part1";

            return part;
        }

        protected override Type ContractType
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        protected override string Template
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
    }

    public class TestIsWrappedValidator : IsWrappedValidator
    {
        public TestIsWrappedValidator() : base(null) { }

        public void TestDoValidate(object objectToValidate, Message currentTarget, string key, ValidationResults validationResults)
        {
            DoValidate(objectToValidate, currentTarget, key, validationResults);
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            base.DoValidate(objectToValidate, currentTarget, key, validationResults);
        }
    }

    public abstract class ServiceContractModelFixture : ModelFixture
    {
        private Store store = null;
        protected override Store Store
        {
            get
            {
                if (store == null)
                {
                    store = new Store(ServiceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(ServiceContractDslDomainModel));
                }
                return store;
            }
        }

        private ServiceContractDslDomainModel dm = null;
        protected override DomainModel DomainModel
        {
            get
            {
                if (dm == null)
                {
                    dm = Store.GetDomainModel<ServiceContractDslDomainModel>();
                }
                return dm;
            }
        }
    }
}