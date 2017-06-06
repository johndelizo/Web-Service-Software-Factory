using System.Configuration;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class ReplayDetectionFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\ReplayDetection.config")]
        public void ShouldGetTwoProblemsWithRuleViolation()
        {
            ReplayDetection rule = new ReplayDetection();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("ReplayDetection.config");
            ProblemCollection problems = rule.Check(configuration);

            Assert.IsNotNull(problems);
            Assert.AreEqual(2, problems.Count);
        }
    }
}
