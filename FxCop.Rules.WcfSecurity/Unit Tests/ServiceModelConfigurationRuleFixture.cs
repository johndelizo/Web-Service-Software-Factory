using System.Configuration;
using System.IO;
using System.Reflection;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    /// <summary>
    /// Summary description for ServiceModelConfigurationRuleFixture
    /// </summary>
    [TestClass]
    public class ServiceModelConfigurationRuleFixture
    {
        [TestMethod]
        [DeploymentItem(@"TestConfigs\EmptyConfigFile.config")]
        public void ShouldReturnNoProblemsWithNonWcfConfigurationFile()
        {
            MockServiceModelConfigurationRule rule = new MockServiceModelConfigurationRule();
            Configuration configuration = ConfigurationLoader.LoadConfiguration("EmptyConfigFile.config");
            ProblemCollection problems = rule.Check(configuration);
            Assert.IsNotNull(problems);
            Assert.AreEqual(0, problems.Count);
        }

        [TestMethod]
        [DeploymentItem(@"TestConfigs\EmptyConfigFile.config")]
        public void ShouldReturnNullProblemsWithBadWcfConfigurationFile()
        {
            MockServiceModelConfigurationRule rule = new MockServiceModelConfigurationRule();
            using (var module = ModuleNode.GetModule(GetAsmLocation("BadConfigFile")))
            {
                ProblemCollection problems = rule.Check(module);
                Assert.IsNull(problems);
            }
        }

        private string GetAsmLocation(string asmPostfix)
        {
            string location = Assembly.GetExecutingAssembly().Location + "." + asmPostfix + ".dll";
            if (!File.Exists(location))
            {
                File.Copy(Assembly.GetExecutingAssembly().Location, location);
            }
            return location;
        }

        #region MockServiceModelConfigurationRule class

        class MockServiceModelConfigurationRule : ServiceModelConfigurationRule
        {
            public MockServiceModelConfigurationRule()
                : base("MockServiceModelConfigurationRule",
                "Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Common.Rules",
                typeof(MockServiceModelConfigurationRule).Assembly)
            { }

            public override ProblemCollection Check(ServiceModelConfigurationManager configurationManager)
            {
                return base.Problems;
            }
        }

        #endregion
    }
}
