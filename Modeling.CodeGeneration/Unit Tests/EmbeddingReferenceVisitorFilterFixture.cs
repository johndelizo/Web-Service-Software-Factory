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
using Microsoft.VisualStudio.Modeling;

namespace Microsoft.Practices.Modeling.CodeGeneration.Tests
{
	/// <summary>
	/// Tests for EmbeddingReferenceVisitorFilter
	/// </summary>
	[TestClass]
	public class EmbeddingReferenceVisitorFilterFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestVisitRelationshipWithNullParameters()
		{
			EmbeddingReferenceVisitorFilter filter =
				new EmbeddingReferenceVisitorFilter();

			filter.ShouldVisitRelationship(null, null, null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestVisitRolePlayerWithNullParameters()
		{
			EmbeddingReferenceVisitorFilter filter =
				new EmbeddingReferenceVisitorFilter();

			filter.ShouldVisitRolePlayer(null, null, null, null, null);
		}
	}
}