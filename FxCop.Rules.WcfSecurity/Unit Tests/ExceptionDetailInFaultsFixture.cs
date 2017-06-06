using System.Configuration;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class ExceptionDetailInFaultsFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\ExceptionDetailInFaults.config")]
        public void ShouldGetOneProblemWithRuleViolation()
        {
            ExceptionDetailInFaults rule = new ExceptionDetailInFaults();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("ExceptionDetailInFaults.config");
            ProblemCollection problems = rule.Check(configuration);

            Assert.IsNotNull(problems);
            Assert.AreEqual(2, problems.Count);
            Assert.AreEqual(configuration.FilePath, problems[0].SourceFile);
        }
    }
}
