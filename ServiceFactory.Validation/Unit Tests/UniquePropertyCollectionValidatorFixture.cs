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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
	public class UniquePropertyCollectionValidatorFixture
	{
		[TestMethod]
		public void CollectionWithUniquelyNamedElementsPasses()
		{
			UniquePropertyCollectionValidator<ThePropertyClass> target =
				new UniquePropertyCollectionValidator<ThePropertyClass>("Property");

			List<ThePropertyClass> coll = new List<ThePropertyClass>();
			coll.Add(new ThePropertyClass("a"));
			coll.Add(new ThePropertyClass("b"));

			ValidationResults results = target.Validate(coll);

			Assert.IsTrue(results.IsValid);
		}

		[TestMethod]
		public void CollectionWithEmptyPropertyElementsPasses()
		{
			UniquePropertyCollectionValidator<ThePropertyClass> target =
				new UniquePropertyCollectionValidator<ThePropertyClass>("Property");

			List<ThePropertyClass> coll = new List<ThePropertyClass>();
			coll.Add(new ThePropertyClass(null));
			coll.Add(new ThePropertyClass(""));

			ValidationResults results = target.Validate(coll);

			Assert.IsTrue(results.IsValid);
		}

		[TestMethod]
		public void CollectionWithDuplicateNamedElementsFails()
		{
			UniquePropertyCollectionValidator<ThePropertyClass> target =
				new UniquePropertyCollectionValidator<ThePropertyClass>("Property");

			List<ThePropertyClass> coll = new List<ThePropertyClass>();
			coll.Add(new ThePropertyClass("same"));
			coll.Add(new ThePropertyClass("same"));

			ValidationResults result = target.Validate(coll);
			List<ValidationResult> results = new List<ValidationResult>(result);

			Assert.IsFalse(result.IsValid);
			Assert.AreEqual(1, results.Count);
		}

		[TestMethod]
		public void CollectionWithDuplicateNamedElementsFailsWhenInitializedUsingAttributes()
		{
			NameValueCollection attributes = new NameValueCollection();
			attributes.Add("collectionElementUniqueIdProperty", "Property");
			UniquePropertyCollectionValidator<ThePropertyClass> target =
				new UniquePropertyCollectionValidator<ThePropertyClass>(attributes);

			List<ThePropertyClass> coll = new List<ThePropertyClass>();
			coll.Add(new ThePropertyClass("same"));
			coll.Add(new ThePropertyClass("same"));

			ValidationResults result = target.Validate(coll);
			List<ValidationResult> results = new List<ValidationResult>(result);

			Assert.IsFalse(result.IsValid);
			Assert.AreEqual(1, results.Count);
		}

		[TestMethod]
		public void ErrorsIncludeTheCollectionName()
		{
			TestableUniquePropertyCollectionValidator target = new TestableUniquePropertyCollectionValidator();
			string collName = "The Collection";
			ThePropertyCollectionClass coll = new ThePropertyCollectionClass(collName);
			coll.Add(new ThePropertyClass("same"));
			coll.Add(new ThePropertyClass("same"));

			ValidationResults result = target.Validate(coll);
			List<ValidationResult> results = new List<ValidationResult>(result);

			Assert.IsFalse(result.IsValid);
			Assert.AreEqual(1, results.Count);
			Assert.IsTrue(results[0].Message.Contains(collName), "Message should contain the collection's name");
		}

		[TestMethod]
		public void NullAttributesPassedToConstructorIsIgnored()
		{
			UniquePropertyCollectionValidator<ThePropertyClass> target =
				new UniquePropertyCollectionValidator<ThePropertyClass>((NameValueCollection)null);

			Assert.AreEqual("Name", target.UniquePropertyName);
		}

		[TestMethod]
		public void CollectionWithCaseInsensitiveDuplicateNamedElementsFails()
		{
			UniquePropertyCollectionValidator<ThePropertyClass> target =
				new UniquePropertyCollectionValidator<ThePropertyClass>("Property");

			List<ThePropertyClass> coll = new List<ThePropertyClass>();
			coll.Add(new ThePropertyClass("Same"));
			coll.Add(new ThePropertyClass("same"));

			ValidationResults result = target.Validate(coll);
			List<ValidationResult> results = new List<ValidationResult>(result);

			Assert.IsFalse(result.IsValid);
			Assert.AreEqual(1, results.Count);
		}

		[TestMethod]
		public void CollectionWithCaseSensitiveDuplicateNamedElementsPass()
		{
			NameValueCollection attributes = new NameValueCollection();
			attributes.Add("collectionElementUniqueIdProperty", "Property"); 
			attributes.Add("caseSensitive", "true");
			UniquePropertyCollectionValidator<ThePropertyClass> target =
				new UniquePropertyCollectionValidator<ThePropertyClass>(attributes);

			List<ThePropertyClass> coll = new List<ThePropertyClass>();
			coll.Add(new ThePropertyClass("Same"));
			coll.Add(new ThePropertyClass("same"));

			ValidationResults result = target.Validate(coll);
			List<ValidationResult> results = new List<ValidationResult>(result);

			Assert.IsTrue(result.IsValid);
			Assert.AreEqual(0, results.Count);
		}

	}

	#region Support classes

	public class TestableUniquePropertyCollectionValidator: UniquePropertyCollectionValidator<ThePropertyClass>
	{
		public TestableUniquePropertyCollectionValidator()
			: base("Property")
		{
		}

		public override string GetObjectName(object element)
		{
			ThePropertyCollectionClass s = element as ThePropertyCollectionClass;
			if (s == null)
				throw new ArgumentException();
			return s.Name;
		}
	}

	public class ThePropertyCollectionClass: List<ThePropertyClass>
	{
		private string _name;

		public string Name
		{ 
			get { return _name; }
		}

		public ThePropertyCollectionClass(string name)
		{
			_name = name;
		}
	}

	public class ThePropertyClass
	{
		private string _property;

		public ThePropertyClass(string value)
		{
			_property = value;
		}

		public string Property
		{
			get { return _property; }
			set { _property = value; }
		}
	}

	#endregion
}
