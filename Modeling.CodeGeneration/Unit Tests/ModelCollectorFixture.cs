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
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;

namespace Microsoft.Practices.Modeling.CodeGeneration.Tests
{
	[TestClass]
	public class ModelCollectorFixture
	{
		[TestMethod]
		public void GetArtifactsReturnsNull()
		{
			MockServiceProvider mockServiceProvider = new MockServiceProvider();
			IArtifactLinkContainer links = ModelCollector.GetArtifacts(mockServiceProvider);
			Assert.IsNull(links);
		}
	}
}
