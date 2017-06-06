using System.Configuration;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class CreditTakingAttackFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\CreditTakingAttack.config")]
        public void ShouldGetTwoProblemsWithRuleViolation()
        {
            CreditTakingAttack rule = new CreditTakingAttack();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("CreditTakingAttack.config");
            ProblemCollection problems = rule.Check(configuration);

            Assert.IsNotNull(problems);
            Assert.AreEqual(2, problems.Count);
        }
    }
}
