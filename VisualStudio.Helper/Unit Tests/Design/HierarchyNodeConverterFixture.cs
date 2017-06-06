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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.VisualStudio.Helper.Design;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.Practices.UnitTestLibrary;

namespace Microsoft.Practices.VisualStudio.Helper.Tests
{
	[TestClass]
	public class HierarchyNodeConverterFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCanConvertFromThrows()
		{
			HierarchyNodeConverter target = new HierarchyNodeConverter();
			Assert.IsTrue(target.CanConvertFrom(typeof(string)));
		}

		[TestMethod]
		public void TestCanConvertFrom()
		{
			MockTypeDescriptorContext context = new MockTypeDescriptorContext(null);
			HierarchyNodeConverter target = new HierarchyNodeConverter();
			Assert.IsTrue(target.CanConvertFrom(context, typeof(string)));
			Assert.IsTrue(target.CanConvertFrom(context, typeof(HierarchyNode)));
			Assert.IsFalse(target.CanConvertFrom(context, typeof(object)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCanConvertToThrows()
		{
			HierarchyNodeConverter target = new HierarchyNodeConverter();
			Assert.IsTrue(target.CanConvertTo(typeof(string)));
		}

		[TestMethod]
		public void TestCanConvertTo()
		{
			MockTypeDescriptorContext context = new MockTypeDescriptorContext(null);
			HierarchyNodeConverter target = new HierarchyNodeConverter();
			Assert.IsTrue(target.CanConvertTo(context,typeof(string)));
			Assert.IsTrue(target.CanConvertTo(context, typeof(HierarchyNode)));
			Assert.IsFalse(target.CanConvertTo(context, typeof(object)));
		}

	}


}
