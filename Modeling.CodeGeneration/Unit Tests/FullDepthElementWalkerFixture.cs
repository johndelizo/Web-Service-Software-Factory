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
using Microsoft.Practices.UnitTestLibrary;

namespace Microsoft.Practices.Modeling.CodeGeneration.Tests
{
	/// <summary>
	/// Tests for FullDepthElementWalker
	/// </summary>
	[TestClass]
	public class FullDepthElementWalkerFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestWithNullParameters()
		{
			FullDepthElementWalker walker = new FullDepthElementWalker(null, null);

			walker.DoTraverse(null);
		}

		[TestMethod]
		public void TestWalkingWithOneMel()
		{
			List<ModelElement> elementList = new List<ModelElement>();
			Store store = new Store(new Type[] { typeof(MockDomainModel) });
			Partition partition = new Partition(store);

			using(Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement mockModelElement = new ExtensibleMockModelElement(partition, "Foo");

				FullDepthElementWalker elementWalker =
					new FullDepthElementWalker(
						new ModelElementVisitor(elementList),
						new EmbeddingReferenceVisitorFilter(),
						false);

				elementWalker.DoTraverse(mockModelElement);

				Assert.AreEqual(1, elementList.Count);

				t.Rollback();
			}
		}
	}
}