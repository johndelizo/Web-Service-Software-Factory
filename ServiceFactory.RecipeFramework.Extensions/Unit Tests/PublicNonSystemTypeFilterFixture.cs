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
	public class PublicNonSystemTypeFilterFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowOnNullType()
		{
			PublicNonSystemTypeFilter filter = new PublicNonSystemTypeFilter();
			filter.CanFilterType(null, true);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowOnError()
		{
			PublicNonSystemTypeFilter filter = new PublicNonSystemTypeFilter();
			filter.CanFilterType(typeof(PrivateType), true);
		}

		[TestMethod]
		public void ShouldReturnFalseOnSystemType()
		{
			PublicNonSystemTypeFilter filter = new PublicNonSystemTypeFilter();
			Assert.IsFalse(filter.CanFilterType(typeof(string), false));
		}

		[TestMethod]
		public void ShouldReturnFalseOnMicrosoftTypes()
		{
			PublicNonSystemTypeFilter filter = new PublicNonSystemTypeFilter();
			Assert.IsFalse(filter.CanFilterType(typeof(Microsoft.VisualStudio.ErrorHandler), false));
		}

		[TestMethod]
		public void FilterDescriptionReturnsValidText()
		{
			string expectedText = "solution types";
			PublicNonSystemTypeFilter filter = new PublicNonSystemTypeFilter();
			Assert.AreEqual<string>(expectedText, filter.FilterDescription);
		}

		[TestMethod]
		public void ShouldReturnFalseOnPrivateType()
		{
			PublicNonSystemTypeFilter filter = new PublicNonSystemTypeFilter();
			Assert.IsFalse(filter.CanFilterType(typeof(PrivateType), false));
		}

		[TestMethod]
		public void ShouldReturnTrueOnNonSystemType() 
		{
			PublicNonSystemTypeFilter filter = new PublicNonSystemTypeFilter();
			Assert.IsTrue(filter.CanFilterType(typeof(VSLangProj.ReferencesEventsClass), false));
		}

		[TestMethod]
		public void ShouldReturnFalseOnAbstractType()
		{
			PublicNonSystemTypeFilter filter = new PublicNonSystemTypeFilter();
			Assert.IsFalse(filter.CanFilterType(typeof(VSLangProj.PrjBrowseObjectCATID), false));
		}
		
		class PrivateType
		{
		}
	}
}
