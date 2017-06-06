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
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.UnitTestLibrary;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Tests
{
	/// <summary>
	/// Summary description for ExtensionProviderFixture
	/// </summary>
	[TestClass]
	public class ExtensionProviderBaseFixture
	{
		public ExtensionProviderBaseFixture()
		{
		}

		[TestMethod]
		public void ExtensionProviderShouldReturnReflectedProperties()
		{
			MockExtensionProvider provider = new MockExtensionProvider();

			Assert.AreEqual(provider.Id, new Guid("02568D23-44A9-4f09-925E-91A6FF7742F3"), "Not Equal");
			Assert.AreEqual(provider.Name, "ABC 123", "Not Equal");
			Assert.AreEqual(provider.Description, "TestableExtensionProvider", "Not Equal");
		}

		[TestMethod]
		public void ExtensionProviderShouldReturnOverrideToStringMethod()
		{
			MockExtensionProvider provider = new MockExtensionProvider();

			Assert.AreEqual(provider.ToString(), String.Format("{0}|{1}|{2}", provider.Id.ToString("b"), provider.Name, provider.Description), "Not Equal");
		}
	}
}