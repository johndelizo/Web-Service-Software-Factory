using System.Configuration;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class UnTrustedClientCertificateRevocationFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\UnTrustedClientCertificateR.config")]
        public void ShouldGetOneProblemWithRuleViolation()
        {            
            UnTrustedClientCertificateRevocation rule = new UnTrustedClientCertificateRevocation();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("UnTrustedClientCertificateR.config");
            ProblemCollection problems = rule.Check(configuration);
           
            Assert.IsNotNull(problems);
            Assert.AreEqual(1, problems.Count);
            Assert.AreEqual(configuration.FilePath, problems[0].SourceFile);
        }
    }
}
