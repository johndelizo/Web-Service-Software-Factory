using System.Configuration;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class NoProtectionFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\NoProtection.config")]
        public void ShouldGetThreeProblemsWithRuleViolation()
        {
            NoProtection rule = new NoProtection();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("NoProtection.config");
            ProblemCollection problems = rule.Check(configuration);

            Assert.IsNotNull(problems);
            Assert.AreEqual(3, problems.Count);
        }
    }
}
