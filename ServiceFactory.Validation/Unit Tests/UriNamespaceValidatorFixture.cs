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
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;


namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	/// <summary>
	/// Tests for UriNamespaceValidator
	/// </summary>
	[TestClass]
	public class UriNamespaceValidatorFixture
	{
		[TestMethod]
		public void ReturnSuccessForNull()
		{
			UriNamespaceValidator validator = new UriNamespaceValidator();
			ValidationResults validationResults = validator.Validate(null);

			Assert.IsTrue(validationResults.IsValid);
		}

		[TestMethod]
		public void ReturnSuccessForValidAction()
		{
			UriNamespaceValidator validator = new UriNamespaceValidator();

			ValidationResults validationResults = validator.Validate("Foo");
			Assert.IsTrue(validationResults.IsValid);

			validationResults = validator.Validate("http://Foo/Action");
			Assert.IsTrue(validationResults.IsValid);

			validationResults = validator.Validate("urn:foo.action");
			Assert.IsTrue(validationResults.IsValid);
		}
	}
}
