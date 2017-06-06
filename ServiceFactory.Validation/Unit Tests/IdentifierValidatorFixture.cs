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
using System.Collections.Specialized;


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
	/// Summary description for IdentifierValidatorFixture
	/// </summary>
	[TestClass]
	public class IdentifierValidatorFixture
	{
		[TestMethod]
		public void ReturnFailureForNullIdentifier()
		{
			Validator<string> validator = new IdentifierValidator();
			ValidationResults validationResults = validator.Validate(null);

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForEmptyIdentifier()
		{
			Validator<string> validator = new IdentifierValidator();
			ValidationResults validationResults = validator.Validate(string.Empty);

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForInvalidIdentifier()
		{
			Validator<string> validator = new IdentifierValidator();
			ValidationResults validationResults = validator.Validate("?asd");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForInvalidDefaultLengthIdentifier()
		{
			Validator<string> validator = new IdentifierValidator();
			ValidationResults validationResults = validator.Validate("asdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnFailureForInvalidConfigurableLengthIdentifier()
		{
			NameValueCollection atts = new NameValueCollection();
			atts.Add("length", "4");

			Validator<string> validator = new IdentifierValidator(atts);
			ValidationResults validationResults = validator.Validate("ABCDE");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ReturnErrorForUnknownLanguage()
		{
			Validator<string> validator = new IdentifierValidator("UnknownLanguage", null);
			ValidationResults validationResults = validator.Validate("valid");
		}

		[TestMethod]
		public void ReturnFailureForInvalidIdentifierByLanguage()
		{
			Validator<string> validator = new IdentifierValidator("VB", null);
			ValidationResults validationResults = validator.Validate("?asd");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnSuccessForValidIdentifier()
		{
			Validator<string> validator = new IdentifierValidator();
			ValidationResults validationResults = validator.Validate("test");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void ReturnSuccessForValidIdentifierByLanguage()
		{
			Validator<string> validator = new IdentifierValidator("VB", null);
			ValidationResults validationResults = validator.Validate("test");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void AttributeBasedConstructorWithEmptyAttributeCollection()
		{
			Validator<string> validator = new IdentifierValidator((NameValueCollection)null);

			ValidationResults validationResults = validator.Validate("test");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void AttributeBasedConstructor()
		{
			NameValueCollection attributes = new NameValueCollection();
			attributes.Add("language", "VB");

			Validator<string> validator = new IdentifierValidator(attributes);

			ValidationResults validationResults = validator.Validate("test");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void FailOnEmptyValuesIfOptionalFlagNotTrue()
		{
			NameValueCollection attributes = new NameValueCollection();
			attributes.Add("optionalValue", "false");

			Validator<string> validator = new IdentifierValidator(attributes);

			ValidationResults validationResults = validator.Validate("");

			Assert.IsFalse(validationResults.IsValid);
			Assert.AreEqual(1, (new List<ValidationResult>(validationResults)).Count);
		}

		[TestMethod]
		public void IgnoreEmptyValuesIfOptionalFlagTrue()
		{
			NameValueCollection attributes = new NameValueCollection();
			attributes.Add("optionalValue", "True");

			Validator<string> validator = new IdentifierValidator(attributes);

			ValidationResults validationResults = validator.Validate("");

			Assert.IsTrue(validationResults.IsValid);
			Assert.AreEqual(0, (new List<ValidationResult>(validationResults)).Count);
		}

	}
}
