using System.Configuration;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class UnTrustedClientCertificateValidationFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\UnTrustedClientCertificateV.config")]
        public void ShouldGetOneProblemWithRuleViolation()
        {
            UnTrustedClientCertificateValidation rule = new UnTrustedClientCertificateValidation();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("UnTrustedClientCertificateV.config");
            ProblemCollection problems = rule.Check(configuration);

            Assert.IsNotNull(problems);
            Assert.AreEqual(1, problems.Count);
            Assert.AreEqual(configuration.FilePath, problems[0].SourceFile);
        }
    }
}
