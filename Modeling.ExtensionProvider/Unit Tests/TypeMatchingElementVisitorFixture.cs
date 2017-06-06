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
using Microsoft.Practices.Modeling.ExtensionProvider.Helpers;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Tests
{
	/// <summary>
	/// Test fixture for TypeMatchingElementVisitorFixture
	/// </summary>
	[TestClass]
	public class TypeMatchingElementVisitorFixture
	{

		[TestMethod]
		public void ShouldAddElementThatMatchType()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			Store store = new Store(new Type[] { typeof(MockDomainModel) });
			Partition partition = new Partition(store);
			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, "TestMessage");

				DepthFirstElementWalker elementWalker = new DepthFirstElementWalker(
					new TypeMatchingElementVisitor<IExtensibleObject>(elementList),
					new EmbeddingVisitorFilter()
					);

				elementWalker.DoTraverse(myModelElement);
				t.Rollback();

				Assert.AreEqual<int>(1, elementList.Count);
			}
		}


		[TestMethod]
		public void ShouldReturnEmptyListIfNoMatches()
		{
			List<ModelElement> elementList = new List<ModelElement>();

			Store store = new Store(new Type[] { typeof(MockDomainModel) });
			Partition partition = new Partition(store);
			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				UnextendedMockModelElement unextendedModelElement = new UnextendedMockModelElement(partition);

				DepthFirstElementWalker elementWalker = new DepthFirstElementWalker(
					new TypeMatchingElementVisitor<IExtensibleObject>(elementList),
					new EmbeddingVisitorFilter()
					);

				elementWalker.DoTraverse(unextendedModelElement);
				t.Rollback();

				Assert.AreEqual<int>(0, elementList.Count);
			}
		}
	
	}
}
