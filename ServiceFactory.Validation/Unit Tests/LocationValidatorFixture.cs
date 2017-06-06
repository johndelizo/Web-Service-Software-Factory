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
	/// Summary description for LocationValidatorFixture
	/// </summary>
	[TestClass]
	public class LocationValidatorFixture
	{
		[TestMethod]
		public void ReturnFailureForNullFile()
		{
			LocationValidator validator = new LocationValidator(".foo");
			ValidationResults validationResults = validator.Validate(null);

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnFailureForFileNotFound()
		{
			LocationValidator validator = new LocationValidator(".foo");
			ValidationResults validationResults = validator.Validate(@"C:\SomeFile");

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnFailureForInvalidFile()
		{
			LocationValidator validator = new LocationValidator(".foo");
			ValidationResults validationResults = validator.Validate(@"C:\SomeFile>");

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnFailureForInvalidUri()
		{
			LocationValidator validator = new LocationValidator(".foo");
			ValidationResults validationResults = validator.Validate("http://");

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnFailureForNotWellFormedUri()
		{
			LocationValidator validator = new LocationValidator(".foo");
			ValidationResults validationResults = validator.Validate("http: //host/foo");

			Assert.IsFalse(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnSuccessForValidFile()
		{
			LocationValidator validator = new LocationValidator(".dll");
			ValidationResults validationResults = validator.Validate(Assembly.GetExecutingAssembly().Location);

			Assert.IsTrue(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnSuccessForValidUri()
		{
			LocationValidator validator = new LocationValidator(".foo");
			ValidationResults validationResults = validator.Validate("http://foo");

			Assert.IsTrue(validationResults.IsValid);
		}
	}
}
