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
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
	public class DataContractEnumDataElementCollectionValidatorFixture : DataContractModelFixture
	{
		[TestMethod]
		public void DoValidateCollectionDataElementFailsForEmptyNamedElements()
		{
			DataContractEnum dataContract = CreateDataContract();

			EnumNamedValue part = Store.ElementFactory.CreateElement(EnumNamedValue.DomainClassId) as EnumNamedValue;
			part.Name = string.Empty;
			dataContract.EnumNamedValues.Add(part);

			TestableDataContractEnumDataElementCollectionValidator target = new TestableDataContractEnumDataElementCollectionValidator();

			ValidationResults results = new ValidationResults();
			target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

			Assert.IsFalse(results.IsValid);
		}

		[TestMethod]
		public void DoValidateCollectionDataElementFailsForSameDataContractName()
		{
			DataContractEnum dataContract = CreateDataContract();

			EnumNamedValue part = Store.ElementFactory.CreateElement(EnumNamedValue.DomainClassId) as EnumNamedValue;
			part.Name = dataContract.Name;
			dataContract.EnumNamedValues.Add(part);

			TestableDataContractEnumDataElementCollectionValidator target = new TestableDataContractEnumDataElementCollectionValidator();

			ValidationResults results = new ValidationResults();
			target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

			Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(1, NumberOfErrors(results));
		}

		[TestMethod]
		public void DoValidateCollectionDataElementFailsForDuplicateNamedElements()
		{
			DataContractEnum dataContract = CreateDataContract();

			EnumNamedValue part = Store.ElementFactory.CreateElement(EnumNamedValue.DomainClassId) as EnumNamedValue;
			part.Name = "foo";
			dataContract.EnumNamedValues.Add(part);

			EnumNamedValue part2 = Store.ElementFactory.CreateElement(EnumNamedValue.DomainClassId) as EnumNamedValue;
			part2.Name = part.Name;

			dataContract.EnumNamedValues.Add(part2);

			TestableDataContractEnumDataElementCollectionValidator target = new TestableDataContractEnumDataElementCollectionValidator();

			ValidationResults results = new ValidationResults();
			target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

			Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(1, NumberOfErrors(results));
		}

		[TestMethod]
		public void DoValidateCollectionDataElementSucceedsForUniqueNamedElements()
		{
			DataContractEnum dataContract = CreateDataContract();

			EnumNamedValue part = Store.ElementFactory.CreateElement(EnumNamedValue.DomainClassId) as EnumNamedValue;
			part.Name = "foo1";
			dataContract.EnumNamedValues.Add(part);

			EnumNamedValue part2 = Store.ElementFactory.CreateElement(EnumNamedValue.DomainClassId) as EnumNamedValue;
			part2.Name = "foo2";

			dataContract.EnumNamedValues.Add(part2);

			TestableDataContractEnumDataElementCollectionValidator target = new TestableDataContractEnumDataElementCollectionValidator();

			ValidationResults results = new ValidationResults();
			target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

			Assert.IsTrue(results.IsValid);
		}

		[TestMethod]
		public void DoValidateCollectionDataElementFailsForDuplicateNamedElementsAndSameDataContractName()
		{
			DataContractEnum dataContract = CreateDataContract();

			EnumNamedValue part = Store.ElementFactory.CreateElement(EnumNamedValue.DomainClassId) as EnumNamedValue;
			part.Name = dataContract.Name;
			dataContract.EnumNamedValues.Add(part);

			EnumNamedValue part2 = Store.ElementFactory.CreateElement(EnumNamedValue.DomainClassId) as EnumNamedValue;
			part2.Name = part.Name;

			dataContract.EnumNamedValues.Add(part2);

			TestableDataContractEnumDataElementCollectionValidator target = new TestableDataContractEnumDataElementCollectionValidator();

			ValidationResults results = new ValidationResults();
			target.TestDoValidateCollectionItem(part, dataContract, String.Empty, results);

			Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(3, NumberOfErrors(results));
		}

		private DataContractEnum CreateDataContract()
		{
			DataContractEnum dataContractEnum = Store.ElementFactory.CreateElement(DataContractEnum.DomainClassId) as DataContractEnum;
			dataContractEnum.Name = "fooEnum";
			return dataContractEnum;
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

		class TestableDataContractEnumDataElementCollectionValidator : DataContractEnumDataElementCollectionValidator
		{
			public TestableDataContractEnumDataElementCollectionValidator()
				: base(null)
			{
			}

			public void TestDoValidateCollectionItem(EnumNamedValue objectToValidate, object currentTarget, string key, ValidationResults validationResults)
			{
				DataContractEnum contract = currentTarget as DataContractEnum;
				foreach (EnumNamedValue element in contract.EnumNamedValues)
				{
					this.DoValidateCollectionItem(element, currentTarget, key, validationResults);
				}
			}
		}

		#endregion
	}
}
