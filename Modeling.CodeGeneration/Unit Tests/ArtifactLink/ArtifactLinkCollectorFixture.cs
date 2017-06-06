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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Modeling.CodeGeneration.Artifacts.Tests
{
	[TestClass]
	public class ArtifactLinkCollectorFixture
	{
		internal class MyArtifactLink : IArtifactLink
		{
			#region IArtifactLink Members

			public Guid Container
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public string ItemPath
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			public IDictionary<string, object> Data
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			#endregion
		}

		internal class MyContainer : IArtifactLinkContainer
		{
			List<IArtifactLink> links;

			public MyContainer(int instances)
			{
				links = new List<IArtifactLink>();
				IArtifactLink link = new MyArtifactLink();
				for (int i = 0; i < instances; i++)
				{
					links.Add(link);
				}
			}

			#region IArtifactLinkContainer Members

			public ICollection<IArtifactLink> ArtifactLinks
			{
				get
				{
					return links;
				}
			}

			#endregion
		}

		[TestMethod]
		public void TestCollect()
		{
			int repeated = 2;
			MyContainer container = new MyContainer(repeated);
			Assert.AreEqual<int>(repeated,container.ArtifactLinks.Count);
			ArtifactLinkCollector target = new ArtifactLinkCollector();
			target.Collect(container);
			Assert.AreEqual<int>(1, target.ArtifactLinks.Count);
		}
	}
}
