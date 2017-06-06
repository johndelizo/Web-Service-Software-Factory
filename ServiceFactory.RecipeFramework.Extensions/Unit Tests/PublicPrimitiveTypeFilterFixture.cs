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
using System.Xml;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests
{
	/// <summary>
	/// Summary description for PublicPrimitiveTypeFilterFixture
	/// </summary>
	[TestClass]
	public class PublicPrimitiveTypeFilterFixture
	{
		private PublicPrimitiveTypeFilter filter;

		[TestInitialize]
		public void TestInitialize()
		{
			filter = new PublicPrimitiveTypeFilter();
		}

		[TestMethod]
		public void ShouldReturnFalseOnEnum()
		{
			Assert.IsFalse( filter.CanFilterType(typeof(EnvironmentVariableTarget), false));
		}

		[TestMethod]
		public void ShouldReturnTrueOnSupportedTypes()
		{
			Assert.IsTrue(filter.CanFilterType(typeof(object), false));
			Assert.IsTrue(filter.CanFilterType(typeof(Guid), false));
			Assert.IsTrue(filter.CanFilterType(typeof(string), false));
			Assert.IsTrue(filter.CanFilterType(typeof(Uri), false));
			Assert.IsTrue(filter.CanFilterType(typeof(DateTime), false));
			Assert.IsTrue(filter.CanFilterType(typeof(TimeSpan), false));
			Assert.IsTrue(filter.CanFilterType(typeof(float), false));
			Assert.IsTrue(filter.CanFilterType(typeof(decimal), false));
			Assert.IsTrue(filter.CanFilterType(typeof(XmlQualifiedName), false));
		}

		[TestMethod]
		public void ShouldReturnTrueOnString()
		{
			Assert.IsTrue(filter.CanFilterType(typeof(string), false));
		}

		[TestMethod]
		public void ShouldReturnFalseOnPtr()
		{
			Assert.IsFalse(filter.CanFilterType(typeof(IntPtr), false));
			Assert.IsFalse(filter.CanFilterType(typeof(UIntPtr), false));
		}

		[TestMethod]
		public void ShouldReturnTrueOnPrimitive()
		{
			Assert.IsTrue(filter.CanFilterType(typeof(int), false));
			Assert.IsTrue(filter.CanFilterType(typeof(uint), false));
			Assert.IsTrue(filter.CanFilterType(typeof(long), false));
			Assert.IsTrue(filter.CanFilterType(typeof(ulong), false));
			Assert.IsTrue(filter.CanFilterType(typeof(byte), false));
			Assert.IsTrue(filter.CanFilterType(typeof(sbyte), false));
			Assert.IsTrue(filter.CanFilterType(typeof(short), false));
			Assert.IsTrue(filter.CanFilterType(typeof(ushort), false));
			Assert.IsTrue(filter.CanFilterType(typeof(double), false));
		}
	}
}
