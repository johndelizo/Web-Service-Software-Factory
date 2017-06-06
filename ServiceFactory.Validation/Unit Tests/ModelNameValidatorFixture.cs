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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
	/// Summary description for ModelNameValidator
	/// </summary>
	[TestClass]
	public class ModelNameValidatorFixture
	{
		const int InvalidLengh = 260;

		[TestMethod]
		public void ShouldInvalidateOnNullName()
		{
			Validator validator = CreateValidator();
			ValidationResults results = validator.Validate(null);

			Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(3, results.Count);
		}
		
		[TestMethod]
		public void ShouldInvalidateOnEmptyName()
		{
			Validator validator = CreateValidator();
			ValidationResults results = validator.Validate(string.Empty);

			Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(3, results.Count);
		}

		[TestMethod]
		public void ShouldInvalidateOnLongName()
		{
			Validator validator = CreateValidator();
			ValidationResults results = validator.Validate(new String('a', InvalidLengh));

			Assert.IsFalse(results.IsValid);
			Assert.AreEqual<int>(1, results.Count);
		}

		[TestMethod]
		public void ShouldPassWithValidName()
		{
			Validator validator = CreateValidator();
			ValidationResults results = validator.Validate("name");

			Assert.IsTrue(results.IsValid);
			Assert.AreEqual<int>(0, results.Count);
		}

		private Validator CreateValidator()
		{
			Validator validator = new ModelNameValidator(new NameValueCollection());
			validator.MessageTemplate = "test {0}{1}";
			return validator;
		}
	}
}
