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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.VisualStudio.Helper
{
	[TestClass]
	public class ReflectionHelperFixture
	{
		[TestMethod]
		public void GetAttributeHandlesNull()
		{
			bool inherit = false;
			MockAttributeProvider provider = new MockAttributeProvider(null,inherit);
			object result = ReflectionHelper.GetAttribute<object>(provider, inherit);
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetAttributeReturnsNullIfMultipleFound()
		{
			bool inherit = false;
			MyAttribute attrib = new MyAttribute();
			MockAttributeProvider provider = new MockAttributeProvider(new object[]{ attrib, attrib }, inherit);
			MyAttribute result = ReflectionHelper.GetAttribute<MyAttribute>(provider, inherit);
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetAttributeReturnsOnlyIfOneIsPresent()
		{
			bool inherit = false;
			MyAttribute attrib = new MyAttribute();
			MockAttributeProvider provider = new MockAttributeProvider(new object[] { attrib }, inherit);
			MyAttribute result = ReflectionHelper.GetAttribute<MyAttribute>(provider, inherit);
			Assert.AreSame(attrib,result);
		}

		[TestMethod]
		public void GetAttributesHandlesNull()
		{
			bool inherit = false;
			MockAttributeProvider provider = new MockAttributeProvider(null, inherit);
			object[] result = ReflectionHelper.GetAttributes<object>(provider, inherit);
			Assert.IsNotNull(result);
			Assert.AreEqual<int>(0,result.Length);
		}

		[TestMethod]
		public void GetAttributesReturnsNotEmpty()
		{
			bool inherit = false;
			MyAttribute attrib = new MyAttribute();
			object[] attribs = new object[] { attrib, attrib };
			MockAttributeProvider provider = new MockAttributeProvider(attribs, inherit);
			MyAttribute[] result = ReflectionHelper.GetAttributes<MyAttribute>(provider, inherit);
			Assert.AreEqual<int>(attribs.Length,result.Length);
		}

		[TestMethod]
		public void GetAttributesReturnsOnlyOne()
		{
			bool inherit = false;
			MyAttribute attrib = new MyAttribute();
			object[] attribs = new object[] { attrib };
			MockAttributeProvider provider = new MockAttributeProvider(attribs, inherit);
			MyAttribute[] result = ReflectionHelper.GetAttributes<MyAttribute>(provider, inherit);
			Assert.AreEqual<int>(attribs.Length, result.Length);
		}

		[TestMethod]
		public void GetTypeByInterfaceReturns()
		{
			List<Type> seedTypeList = new List<Type>();
			seedTypeList.Add(typeof(MockAttributeProvider));

			IList<Type> foundTypes = ReflectionHelper.GetTypesByInterface(
				seedTypeList, 
				typeof(ICustomAttributeProvider) );

			Assert.AreEqual<int>(1, foundTypes.Count);
			Assert.AreEqual<Type>(typeof(MockAttributeProvider), foundTypes[0]);
		}

		[TestMethod]
		public void CanGetProviderFromPropertyMember()
		{
			ICustomAttributeProvider provider = ReflectionHelper.GetProvider<MyType>("Element");

			Assert.IsNotNull(provider);
		}

		#region Internal classes 

		internal class MockAttributeProvider : ICustomAttributeProvider
		{
			object[] attribs;
			bool inherit;

			public MockAttributeProvider(object[] attribs, bool inherit)
			{
				this.attribs = attribs;
				this.inherit = inherit;
			}

			#region ICustomAttributeProvider Members

			object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit)
			{
				Assert.AreEqual<bool>(this.inherit, inherit);
				return attribs;
			}

			object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit)
			{
				Assert.AreEqual<bool>(this.inherit, inherit);
				return attribs;
			}

			bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

		internal class MyAttribute
		{
		}

		internal class MyType
		{
			string element = "";

			public string Element
			{
				get { return element; }
			}
		}

		#endregion
	}
}
