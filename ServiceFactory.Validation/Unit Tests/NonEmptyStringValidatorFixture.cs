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
	/// Summary description for NonEmptyStringValidatorFixture
	/// </summary>
	[TestClass]
	public class NonEmptyStringValidatorFixture
	{
		[TestMethod]
		public void ReturnFailureForNullString()
		{
			Validator<string> validator = new NonEmptyStringValidator();
			ValidationResults validationResults = validator.Validate(null);

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnFailureForEmptyString()
		{
			Validator<string> validator = new NonEmptyStringValidator();
			ValidationResults validationResults = validator.Validate(string.Empty);

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnSuccessForValidString()
		{
			Validator<string> validator = new NonEmptyStringValidator();
			ValidationResults validationResults = validator.Validate("test");

			Assert.IsTrue(validationResults.IsValid);
		}
	}
}
