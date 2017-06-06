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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
	public class PropertyCollectionValidatorFixture
	{
		[TestMethod]
		public void CollectionFailsValidation()
		{
			string badValue = "badvalue";
			TestablePropertyCollectionValidator target = new TestablePropertyCollectionValidator(badValue);
			string collName = "The Collection";

			ThePropertyCollectionClass coll = new ThePropertyCollectionClass(collName);
			coll.Add(new ThePropertyClass("value"));
			coll.Add(new ThePropertyClass(badValue));

			ValidationResults result = target.Validate(coll);
			List<ValidationResult> results = new List<ValidationResult>(result);

			Assert.IsFalse(result.IsValid);
			Assert.AreEqual(1, results.Count);
		}

		[TestMethod]
		public void CollectionPassesValidation()
		{
			string badValue = "badvalue";
			TestablePropertyCollectionValidator target = new TestablePropertyCollectionValidator(badValue);
			string collName = "The Collection";

			ThePropertyCollectionClass coll = new ThePropertyCollectionClass(collName);
			coll.Add(new ThePropertyClass("value"));
			coll.Add(new ThePropertyClass("value"));

			ValidationResults result = target.Validate(coll);
			List<ValidationResult> results = new List<ValidationResult>(result);

			Assert.IsTrue(result.IsValid);
			Assert.AreEqual(0, results.Count);
		}
	}

	public class TestablePropertyCollectionValidator : PropertyCollectionValidator<ThePropertyClass>
	{
		private string _badPropertyValue;

		public TestablePropertyCollectionValidator(string badPropertyValue)
		{
			_badPropertyValue = badPropertyValue;
		}

		protected override void DoValidateCollectionItem(ThePropertyClass objectToValidate, object currentTarget, string key, Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults validationResults)
		{
			if (String.Compare(objectToValidate.Property, _badPropertyValue) == 0)
				validationResults.AddResult(new ValidationResult("bad value", objectToValidate, String.Empty, String.Empty, this));
		}

		protected override string DefaultMessageTemplate
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}
	}
}
