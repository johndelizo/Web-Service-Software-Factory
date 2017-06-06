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
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;
using Microsoft.Practices.FxCop.Rules.WcfSemantic.Tests.Utilities;
using Microsoft.Practices.FxCop.Rules.WcfSemantic.Tests.Common;

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic.Tests
{
    /// <summary>
    /// Summary description for ContractBindingNotSupportedSessionFixture
    /// </summary>
    [TestClass]
    public class ContractBindingNotSupportedSessionFixture
    {
        [TestMethod]
		[DeploymentItem(@"TestConfigs\BindingNotSupportedSession.config")]
        public void ShouldGetSixProblemsWithConfigFile()
        {
            Configuration configuration = ConfigurationLoader.LoadConfiguration("BindingNotSupportedSession.config", "TestConfigs");
            ContractBindingNotSupportedSession rule = new ContractBindingNotSupportedSession();
            rule.Check(configuration);
            using (ModuleNode module = ModuleNode.GetModule(
                typeof(ContractBindingNotSupportedSessionFixture).Assembly.Location, true, true, true))
            {
                rule.Check(module);
            }

            Assert.AreEqual(6, rule.Problems.Count);
        }
    }
}
