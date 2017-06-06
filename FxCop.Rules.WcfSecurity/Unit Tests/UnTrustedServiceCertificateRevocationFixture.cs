using System.Configuration;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class UnTrustedServiceCertificateRevocationFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\UnTrustedServiceCertificateR.config")]
        public void ShouldGetOneProblemWithRuleViolation()
        {
            UnTrustedServiceCertificateRevocation rule = new UnTrustedServiceCertificateRevocation();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("UnTrustedServiceCertificateR.config");
            ProblemCollection problems = rule.Check(configuration);

            Assert.IsNotNull(problems);
            Assert.AreEqual(1, problems.Count);
        }
    }
}
