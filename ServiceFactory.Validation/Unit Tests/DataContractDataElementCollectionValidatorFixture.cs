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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
    [TestClass]
	public class DataContractDataElementCollectionValidatorFixture : DataContractModelFixture
    {
        [TestMethod]
        public void DoValidateCollectionDataElementFailsForEmptyNamedElements()
        {
			DataContract dataContract = CreateDataContract();

			PrimitiveDataType part = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
            part.Name = string.Empty;
            dataContract.DataMembers.Add(part);

            TestableDataElementCollectionValidator target = new TestableDataElementCollectionValidator();

            ValidationResults results = new ValidationResults();
            target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

            Assert.IsFalse(results.IsValid);
        }

		[TestMethod]
		public void DoValidateCollectionDataElementFailsForSameDataContractName()
		{
			DataContract dataContract = CreateDataContract();

			PrimitiveDataType part = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
			part.Name = dataContract.Name;
			dataContract.DataMembers.Add(part);

			TestableDataElementCollectionValidator target = new TestableDataElementCollectionValidator();

			ValidationResults results = new ValidationResults();
			target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

			Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(1, NumberOfErrors(results));
		}

        [TestMethod]
        public void DoValidateCollectionDataElementFailsForDuplicateNamedElements()
        {
			DataContract dataContract = CreateDataContract();

			PrimitiveDataType part = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
            part.Name = "foopart";

            dataContract.DataMembers.Add(part);

            PrimitiveDataType part2 = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
			part2.Name = part.Name;

            dataContract.DataMembers.Add(part2);

            TestableDataElementCollectionValidator target = new TestableDataElementCollectionValidator();

            ValidationResults results = new ValidationResults();
            target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

            Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(1, NumberOfErrors(results));
		}

        [TestMethod]
        public void DoValidateCollectionDataElementSucceedsForUniqueNamedElements()
        {
			DataContract dataContract = CreateDataContract();

			PrimitiveDataType part = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
			part.Name = "Part Name 1";

            dataContract.DataMembers.Add(part);

            PrimitiveDataType part2 = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
            part2.Name = "Part Name 2";

            dataContract.DataMembers.Add(part2);

            TestableDataElementCollectionValidator target = new TestableDataElementCollectionValidator();

            ValidationResults results = new ValidationResults();
            target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

            Assert.IsTrue(results.IsValid);
		}

		[TestMethod]
		public void DoValidateCollectionDataElementFailsForDuplicateNamedElementsAndSameDataContractName()
		{
			DataContract dataContract = CreateDataContract();

			PrimitiveDataType part = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
			part.Name = dataContract.Name;

			dataContract.DataMembers.Add(part);

			PrimitiveDataType part2 = Store.ElementFactory.CreateElement(PrimitiveDataType.DomainClassId) as PrimitiveDataType;
			part2.Name = part.Name;

			dataContract.DataMembers.Add(part2);

			TestableDataElementCollectionValidator target = new TestableDataElementCollectionValidator();

			ValidationResults results = new ValidationResults();
			target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

			Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(3, NumberOfErrors(results));
		}

		private DataContract CreateDataContract()
		{
			DataContract dataContract = Store.ElementFactory.CreateElement(DataContract.DomainClassId) as DataContract;
			dataContract.Name = "foo";
			return dataContract;
		}

		private int NumberOfErrors(ValidationResults validationResults)
		{
			int count = 0;

			foreach (ValidationResult result in validationResults)
			{
				count++;
			}

			return count;
		}

		#region TestableDataElementCollectionValidator class

		class TestableDataElementCollectionValidator : DataContractDataElementCollectionValidator
        {
            public TestableDataElementCollectionValidator()
                : base(null)
            {
            }

            public void TestDoValidateCollectionItem(DataMember objectToValidate, object currentTarget, string key, ValidationResults validationResults)
            {
                DataContract contract = currentTarget as DataContract;
                foreach (DataMember element in contract.DataMembers)
                {
                    this.DoValidateCollectionItem(element, currentTarget, key, validationResults);
                }
            }
		}

		#endregion
	}
}
