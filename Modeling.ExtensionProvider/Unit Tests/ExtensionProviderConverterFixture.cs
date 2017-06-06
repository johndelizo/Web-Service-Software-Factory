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
using System.ComponentModel;
using System.Text;
using Microsoft.Practices.Modeling.ExtensionProvider.Design.Converters;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.Modeling.ExtensionProvider.Services;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Tests
{
	[TestClass]
	public class ExtensionProviderConverterFixture
	{
		[TestMethod]
		public void ExtensionProviderContainerConverterCanConvertFromString()
		{
			ExtensionProviderConverter converter = new ExtensionProviderConverter(new MockServiceProvider());
			Assert.IsTrue(converter.CanConvertFrom(typeof(string)), "Cannot convert from a string");
		}

		[TestMethod]
		public void ExtensionProviderContainerConverterCanConvertToIExtensionProvider()
		{
			ExtensionProviderConverter converter = new ExtensionProviderConverter(new MockServiceProvider());

			Assert.IsNotNull(converter.CanConvertTo(null, (typeof(IExtensionProvider))), "Cannot convert to a IExtensionProvider");
		}

		[TestMethod]
		[DeploymentItem("Microsoft.Practices.RecipeFramework.Library.dll")]
		public void ExtensionProviderContainerConverterConvertToExtensionProvider()
		{
			MockExtensionProvider provider = new MockExtensionProvider();
			MockServiceProvider mockServiceProvider = new MockServiceProvider();
			MockServiceProviderService mockService = new MockServiceProviderService();

			mockServiceProvider.AddService(typeof(IExtensionProviderService), mockService);

			ExtensionProviderConverter converter = new ExtensionProviderConverter(mockServiceProvider);

			MockExtensionProvider provider1 = converter.ConvertFrom(provider.ToString()) as MockExtensionProvider;

			Assert.AreEqual(provider.Id, provider1.Id, "Not Equal");
			Assert.AreEqual(provider.Description, provider1.Description, "Not Equal");
			Assert.AreEqual(provider.Name, provider1.Name, "Not Equal");
		}
	}
}