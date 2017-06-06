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
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors.TypeBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests
{
	[TestClass]
	public class PublicTypeWithDefaultConstructorFilterFixture
	{
		[TestMethod]
		public void AcceptPublicClassWithDefaultConstructor()
		{
			TypeFilterProvider filter = new PublicTypeWithDefaultConstructorFilter();
			Assert.IsTrue(filter.CanFilterType(typeof(PublicTestClass), false));
		}

		[TestMethod]
		public void DontAcceptInternalClass()
		{
			TypeFilterProvider filter = new PublicTypeWithDefaultConstructorFilter();
			Assert.IsFalse(filter.CanFilterType(typeof(PrivateTestClass), false));
		}

		[TestMethod]
		public void DontAcceptPublicClassWitoutDefaultConstructor()
		{
			TypeFilterProvider filter = new PublicTypeWithDefaultConstructorFilter();
			Assert.IsFalse(filter.CanFilterType(typeof(NoDefaultConstructor), false));
		}
	}

	public class PublicTestClass
	{
	}

	class PrivateTestClass
	{
	}

	public class NoDefaultConstructor
	{
		public NoDefaultConstructor(int arg)
		{
		}
	}
}
