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


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
	/// Summary description for FileNameValidatorFixture
	/// </summary>
	[TestClass]
	public class FileNameValidatorFixture
	{
		[TestMethod]
		public void ReturnFailureForNullFile()
		{
			Validator validator = new FileNameValidator();
			ValidationResults validationResults = validator.Validate(null);

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(2, NumberOfErrors(validationResults));
		}

		[TestMethod]
		public void ReturnFailureForEmptyFile()
		{
			Validator validator = new FileNameValidator();
			ValidationResults validationResults = validator.Validate(string.Empty);

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(2, NumberOfErrors(validationResults));
		}

		[TestMethod]
		public void ReturnFailureForFileTooLong()
		{
			Validator validator = new FileNameValidator();
			ValidationResults validationResults = validator.Validate(new String('a',111));

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, NumberOfErrors(validationResults));
		}

		[TestMethod]
		public void ReturnFailureForFileWithReservedSystemWords()
		{
			Validator validator = new FileNameValidator();
			ValidationResults validationResults = validator.Validate("PRN");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, NumberOfErrors(validationResults));
		}

		[TestMethod]
		public void ReturnFailureForFileWithInvalidCharacters()
		{
			Validator validator = new FileNameValidator();
			ValidationResults validationResults = validator.Validate("?class.cs");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, NumberOfErrors(validationResults));
		}

		[TestMethod]
		public void ReturnSuccessForValidFile()
		{
			Validator validator = new FileNameValidator();
			ValidationResults validationResults = validator.Validate("class.cs");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, NumberOfErrors(validationResults));
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
	}
}
