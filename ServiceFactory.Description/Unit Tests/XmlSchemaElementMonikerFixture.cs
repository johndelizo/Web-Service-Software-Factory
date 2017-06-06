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

namespace Microsoft.Practices.ServiceFactory.Description.Tests
{
	/// <summary>
	/// Summary description for XmlSchemaUriBuilderFixture
	/// </summary>
	[TestClass]
	public class XmlSchemaElementMonikerFixture
	{
		const string XmlSchemaUriPath = "schemas/subfolder/MySchema.xsd";
		const string XmlSchemaFilePath = "schemas\\subfolder\\MySchema.xsd";
		const string XmlSchemaFileRootPath = "schemas\\MySchema.xsd";
		const string XmlSchemaFileNoPath = "myschema.xsd";
		const string ElementName = "element";
		readonly string XmlSchemaElementMonikerFileFormat = "xsd://root\\" + XmlSchemaFilePath + "?" + ElementName;

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowOnEmptyUri()
		{
			new XmlSchemaElementMoniker(String.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowOnNullXmlSchemaPath()
		{
			new XmlSchemaElementMoniker(null, "foo");
		}

		[TestMethod]
		public void ShouldGetValidUriFromUriString()
		{
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker(XmlSchemaElementMonikerFileFormat);
									
			Assert.AreEqual<string>(XmlSchemaFilePath, uri.XmlSchemaPath);
			Assert.AreEqual<string>(ElementName, uri.ElementName);
			Assert.AreEqual<string>(XmlSchemaElementMonikerFileFormat, uri.ToString());
		}

		[TestMethod]
		public void ShouldGetValidUriFromParts()
		{
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker("r o o t\\" + XmlSchemaFilePath, ElementName);

			Assert.AreEqual<string>("r o o t\\" + XmlSchemaFilePath, uri.XmlSchemaPath);
			Assert.AreEqual<string>(ElementName, uri.ElementName);
		}

		[TestMethod]
		public void ShouldGetValidUriFromPartsWithSpaces()
		{
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker(XmlSchemaFilePath, ElementName);

			Assert.AreEqual<string>(XmlSchemaFilePath, uri.XmlSchemaPath);
			Assert.AreEqual<string>(ElementName, uri.ElementName);
			Assert.AreEqual<string>(XmlSchemaElementMonikerFileFormat, uri.ToString());
		}

		[TestMethod]
		public void ShouldGetValidUriFromPartsWithNoFilePath()
		{
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker(XmlSchemaFileNoPath, ElementName);

			Assert.AreEqual<string>(XmlSchemaFileNoPath, uri.XmlSchemaPath);
			Assert.AreEqual<string>(ElementName, uri.ElementName);
		}

		[TestMethod]
		public void ShouldGetValidElementFromUriTypeFormat()
		{
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker("xsd:String");

			Assert.IsNull(uri.ElementName);
			Assert.AreEqual<string>("String", uri.XmlSchemaPath);
		}

		[TestMethod]
		public void ShouldGetValidUriFromElementType()
		{
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker("String", null);

			Assert.IsNull(uri.ElementName);
			Assert.AreEqual<string>("xsd:String", uri.ToString());
		}

		[TestMethod]
		public void ShouldGetValidElementFromUriSpecialTypeFormat()
		{
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker("xsd:Nullable<int>");

			Assert.IsNull(uri.ElementName);
			Assert.AreEqual<string>("Nullable<int>", uri.XmlSchemaPath);
		}

		[TestMethod]
		public void ShouldGetValidUriFromElementSpecialType()
		{
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker("Nullable<int>", null);

			Assert.IsNull(uri.ElementName);
			Assert.AreEqual<string>("xsd:Nullable<int>", uri.ToString());
		}
	}
}
