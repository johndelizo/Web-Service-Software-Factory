using System;
using System.IO;
using System.Reflection;
using Microsoft.FxCop.Sdk;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    /// <summary>
    /// Summary description for ConfigurationIntrospectionRuleFixture
    /// </summary>
    [TestClass]
    public class ConfigurationIntrospectionRuleFixture
    {
        [TestMethod]
        public void ShouldReturnNoProblemsWithNullModule()
        {
            MockConfigurationIntrospectionRule rule = new MockConfigurationIntrospectionRule();
            ProblemCollection problems = rule.Check(null);

            Assert.IsNotNull(problems);
            Assert.AreEqual(0, problems.Count);
        }

        [TestMethod]
        public void ShouldReturnNullProblemsWithEmptyModule()
        {
            MockConfigurationIntrospectionRule rule = new MockConfigurationIntrospectionRule();
            ProblemCollection problems = rule.Check(ModuleNode.GetModule(Assembly.GetExecutingAssembly().Location));

            Assert.IsNull(problems);
        }

        [TestMethod]
        [DeploymentItem(@"TestConfigs\BadConfigFile.config")]
        public void ShouldReturnNullProblemsWithBadConfigurationFile()
        {
            MockConfigurationIntrospectionRule rule = new MockConfigurationIntrospectionRule();
            using (var module = ModuleNode.GetModule(GetAsmLocation("BadConfigFile", null)))
            {
                ProblemCollection problems = rule.Check(module);
                Assert.IsNull(problems);
                Assert.IsNull(rule.Configuration);
            }
        }

        [TestMethod]
        [DeploymentItem(@"TestConfigs\EmptyConfigFile.config")]
        public void ShouldGetTheSourceFile()
        {
            MockConfigurationIntrospectionRule rule = new MockConfigurationIntrospectionRule();
            string configPath = CreateConfigurationFile("EmptyConfigFile.config", null);
            using (var module = ModuleNode.GetModule(GetAsmLocation("EmptyConfigFile", null)))
            {
                rule.Check(module);
                if (!rule.IsConfigInTempPath())
                {
                    Assert.AreEqual(configPath, rule.SourceFile);
                }
                Assert.IsNotNull(rule.Configuration);
            }
        }

        [TestMethod]
        [DeploymentItem(@"TestConfigs\EmptyConfigFile.config")]
        public void ShouldGetTheConfigurationFromWebConfigFile()
        {
            MockConfigurationIntrospectionRule rule = new MockConfigurationIntrospectionRule();
            // create a separate folder to test the web config file
            DirectoryInfo webDir = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebConfigTest"));
            // put there the web config file
            string configPath = CreateConfigurationFile("EmptyConfigFile.config", Path.Combine(webDir.FullName, "web.config"));
            // put the asm in a subdirectory of web config dir
            DirectoryInfo asmDir = webDir.CreateSubdirectory("Asm");
            using (var module =
                ModuleNode.GetModule(GetAsmLocation("EmptyConfigFile", Path.Combine(asmDir.FullName, Path.GetFileName(Assembly.GetExecutingAssembly().Location)))))
            {
                rule.Check(module);
                Assert.AreEqual(configPath, rule.SourceFile);
                Assert.IsNotNull(rule.Configuration);
            }
        }

        private string GetAsmLocation(string asmPostfix, string optionalPath)
        {
            string location = (optionalPath ?? Assembly.GetExecutingAssembly().Location) + "." + asmPostfix + ".dll";
            if(!File.Exists(location))
            {
                File.Copy(Assembly.GetExecutingAssembly().Location, location);
            }
            return location;
        }

        private string CreateConfigurationFile(string resourceName, string optionalPath)
        {
            string configurationFileName = optionalPath ?? (Assembly.GetExecutingAssembly().Location + "." + resourceName.Replace(".config",".dll.config"));
            ConfigurationLoader.CreateConfigurationFile(configurationFileName, resourceName);
            return configurationFileName;
        }

        #region MockConfigurationIntrospectionRule class

        class MockConfigurationIntrospectionRule : ConfigurationIntrospectionRule
        {            
            public MockConfigurationIntrospectionRule()
                : base("MockConfigurationIntrospectionRule",
                "Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Common.Rules", 
                typeof(MockConfigurationIntrospectionRule).Assembly)
            { }

            public System.Configuration.Configuration Configuration;

            public override ProblemCollection Check(System.Configuration.Configuration configuration)
            {
                this.Configuration = configuration;
                return base.Problems;
            }

            public override ProblemCollection Check(ModuleNode module)
            {
                return base.Check(module);
            }
        }

        #endregion
    }
}
