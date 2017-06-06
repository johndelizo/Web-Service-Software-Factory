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
using Microsoft.Practices.FxCop.Rules.WcfSemantic.Tests.Common;
using Microsoft.FxCop.Sdk;
using Microsoft.Practices.FxCop.Rules.WcfSemantic.Tests.Utilities;

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic.Tests
{
    /// <summary>
    /// Summary description for DuplicatedOperationsFixture
    /// </summary>
    [TestClass]
    public class DuplicatedOperationsFixture
    {       
        [TestMethod]
		public void ShouldGetTwoProblemsWithDoubleTwinsOperations()
        {
            DuplicatedOperations rule = new DuplicatedOperations();
			rule.Check(RuleHelper.GetTypeNodeFromType(typeof(IDuplicateOperations)));
            Assert.AreEqual(2, rule.Problems.Count);
        }
    }
}
