using System.Configuration;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class UnTrustedServiceCertificateValidationFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\UnTrustedServiceCertificateV.config")]
        public void ShouldGetOneProblemWithRuleViolation()
        {
            UnTrustedServiceCertificateValidation rule = new UnTrustedServiceCertificateValidation();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("UnTrustedServiceCertificateV.config");
            ProblemCollection problems = rule.Check(configuration);

            Assert.IsNotNull(problems);
            Assert.AreEqual(1, problems.Count);
        }
    }
}
