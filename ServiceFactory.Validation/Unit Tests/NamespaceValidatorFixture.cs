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


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
	/// Summary description for NamespaceValidatorFixture
	/// </summary>
	[TestClass]
	public class NamespaceValidatorFixture
	{
		[TestMethod]
		public void ReturnFailureForNullNamespace()
		{
			Validator<string> validator = new NamespaceValidator();
			ValidationResults validationResults = validator.Validate(null);

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForEmptyNamespace()
		{
			Validator<string> validator = new NamespaceValidator();
			ValidationResults validationResults = validator.Validate(string.Empty);

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForInvalidNamespace()
		{
			Validator<string> validator = new NamespaceValidator();
			ValidationResults validationResults = validator.Validate("asd ee.asda");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForDuplicateNamespaceParts()
		{
			Validator<string> validator = new NamespaceValidator();
			ValidationResults validationResults = validator.Validate("Global.Foo.Foo");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForBadConstructedNamespace()
		{
			Validator<string> validator = new NamespaceValidator();
			ValidationResults validationResults = validator.Validate("Global..Foo");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForBadSimpleNamespace()
		{
			Validator<string> validator = new NamespaceValidator();
			ValidationResults validationResults = validator.Validate("Global$");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForInvalidIdentifierByLanguage()
		{
			Validator<string> validator = new NamespaceValidator("VB", null);
			ValidationResults validationResults = validator.Validate("asd ee.asda");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnSuccessForValidIdentifier()
		{
			Validator<string> validator = new NamespaceValidator();
			ValidationResults validationResults = validator.Validate("test.qweq.asda");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnSuccessForValidIdentifierByLanguage()
		{
			Validator<string> validator = new NamespaceValidator("VB", null);
			ValidationResults validationResults = validator.Validate("test.qwe.aasdwe");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnSuccessForValidDuplicateCaseInsensitiveIdentifier()
		{
			Validator<string> validator = new NamespaceValidator();
			ValidationResults validationResults = validator.Validate("Global.Foo.foo");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, (new List<ValidationResult>(validationResults)).Count);
		}
	}
}
