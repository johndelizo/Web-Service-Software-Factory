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
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors.TypeBrowser;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests
{
	/// <summary>
	/// Summary description for PublicTypeFilterFixture
	/// </summary>
	[TestClass]
	public class PublicTypeFilterFixture
	{
		private PublicTypeFilter filter;

		[TestInitialize]
		public void TestInitialize()
		{
			filter = new PublicTypeFilter();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowOnNullType()
		{
			filter.CanFilterType(null, true);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowOnError()
		{
			filter.CanFilterType(typeof(PrivateType), true);
		}

		[TestMethod]
		public void ShouldReturnTrueOnPublicType()
		{
			Assert.IsTrue(filter.CanFilterType(typeof(string), false));
		}

		[TestMethod]
		public void ShouldReturnFalseOnPrivateType()
		{
			Assert.IsFalse(filter.CanFilterType(typeof(PrivateType), false));
		}

		class PrivateType
		{
		}
	}
}
