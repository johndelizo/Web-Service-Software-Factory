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
using System.Collections.ObjectModel;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;

namespace Microsoft.Practices.Modeling.CodeGeneration.Tests
{
	/// <summary>
	/// Summary description for UtilityFixture
	/// </summary>
	[TestClass]
	public class UtilityFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowOnNullToCamelCaseIdentifier()
		{
			Utility.ToCamelCase(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowOnNullToPascalCaseIdentifier()
		{
			Utility.ToPascalCase(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowOnNullGetCSharpTypeOutputTypeName()
		{
			Utility.GetCSharpTypeOutput(null);
		}


		[TestMethod]
		public void ShouldConvertToCamelCase()
		{
			string result = Utility.ToCamelCase("FooBar");
			Assert.AreEqual<string>("fooBar", result);
		}

		[TestMethod]
		public void ShouldAddFieldSuffix()
		{
			string result = Utility.ToCamelCase("fooBar");
			Assert.AreEqual<string>("fooBarField", result);
		}

        [TestMethod]
        public void ShouldConvertToPascalCaseAndNotAddFieldSuffix()
        {
            string result = Utility.ToPascalCase("fooBar","fooBar");
            Assert.AreNotEqual<string>("FooBarField", result);
        }

        [TestMethod]
        public void ShouldConvertToPascalCaseAndAddFieldSuffix()
        {
            string result = Utility.ToPascalCase("fooBar", "FooBar");
            Assert.AreEqual<string>("FooBarField", result);
        }

		[TestMethod]
		public void ShouldConvertToPascalCase()
		{
			string result = Utility.ToPascalCase("fooBar");
			Assert.AreEqual<string>("FooBar", result);
		}

		[TestMethod]
		public void ShouldNotConvertToPascalCase()
		{
			string result = Utility.ToPascalCase("FooBar");
			Assert.AreEqual<string>("FooBar", result);
		}

		[TestMethod]
		public void ShouldGetCSharpTypeOutput()
		{
			string output = Utility.GetCSharpTypeOutput("System.String");
			Assert.AreEqual<string>("string", output);
		}

		[TestMethod]
		public void ShouldNotGetCSharpTypeOutput()
		{
			string output = Utility.GetCSharpTypeOutput("string");
			Assert.AreEqual<string>("string", output);
		}

		[TestMethod]
		public void ShouldGetCustomCSharpTypeOutput()
		{
			string output = Utility.GetCSharpTypeOutput("Foo");
			Assert.AreEqual<string>("Foo", output);
		}

		[TestMethod]
		public void ShouldGetCSharpNullableTypeOutput()
		{
			string nullableInt = "System.Nullable`1[System.Int32]";
			string output = Utility.GetCSharpTypeOutput(nullableInt);
			Assert.AreEqual<string>("System.Nullable<int>", output);
		}

		[TestMethod]
		public void ShouldGetCSharpNullableTypeOutputWithParam()
		{
			string output = Utility.GetCSharpTypeOutput("System.Int32", true);
			Assert.AreEqual<string>("System.Nullable<int>", output);
		}

		[TestMethod]
		public void ShouldGetCSharpTypeOutputWithNotNullableType()
		{
			string output = Utility.GetCSharpTypeOutput("System.String", true);
			Assert.AreEqual<string>("string", output);
		}

		[TestMethod]
		public void ShouldGetCSharpTypeDeclarationWithNullCollectionType()
		{
			string expected = "string";
			string actual = Utility.GetCSharpTypeDeclaration(null, "System.String");

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldReturnListT()
		{
			string expected = "System.Collections.Generic.List<string>";
			string actual = Utility.GetCSharpTypeDeclaration(typeof(List<>), "System.String");

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldReturnCollectionT()
		{
			string expected = "System.Collections.ObjectModel.Collection<string>";
			string actual = Utility.GetCSharpTypeDeclaration(typeof(Collection<>), "System.String");

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldReturnDictionaryT()
		{
			string expected = "System.Collections.Generic.Dictionary<string, int>";
			string actual = Utility.GetCSharpTypeDeclaration(typeof(Dictionary<string, int>), "System.Int32");

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldReturnDictionaryTWithoutTypeArguments()
		{
			string expected = "System.Collections.Generic.Dictionary<string, int>";
			string actual = Utility.GetCSharpTypeDeclaration(typeof(Dictionary<,>), "System.Int32");

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldReturnDictionaryOfFoo()
		{
			string expected = "System.Collections.Generic.Dictionary<string, Microsoft.Practices.Modeling.CodeGeneration.Tests.UtilityFixture.Foo>";
			string actual = Utility.GetCSharpTypeDeclaration(typeof(Dictionary<string, Foo>), "Foo");

			Assert.AreEqual<string>(expected, actual);
		}

		// Aux struct for ShouldReturnCollectionOfFoo test
		struct Foo { }

		[TestMethod]
		public void ShouldReturnCollectionOfFoo()
		{
			string expected = "System.Collections.ObjectModel.Collection<Foo>";
			string actual = Utility.GetCSharpTypeDeclaration(typeof(Collection<>), "Foo");

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldReturnArrayOfString()
		{
			string expected = "string[]";
			Type arrayType = typeof(List<>).GetGenericArguments()[0].MakeArrayType();
			string actual = Utility.GetCSharpTypeDeclaration(arrayType, "System.String");

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldReturnArrayOfFoo()
		{
			string expected = "Foo[]";
			Type arrayType = typeof(List<>).GetGenericArguments()[0].MakeArrayType();
			string actual = Utility.GetCSharpTypeDeclaration(arrayType, "Foo");

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldReturnArray()
		{
			string expected = "System.Array";
			string actual = Utility.GetCSharpTypeDeclaration(typeof(Array));

			Assert.AreEqual<string>(expected, actual);
		}

		[TestMethod]
		public void ShouldUpdateArtifactLinkData()
		{
			MyArtifactLink link = new MyArtifactLink();
			Utility.SetData<string>(link, "foo");
			string data = Utility.GetData<string>(link);

			Assert.AreEqual<string>("foo", data);
		}

		#region MyArtifactLink class

		private class MyArtifactLink : ArtifactLink
		{
		}

		#endregion
	}
}
