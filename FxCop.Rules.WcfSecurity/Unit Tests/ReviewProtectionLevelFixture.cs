using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FxCop.Sdk;
using Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests.Utilities;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [TestClass]
    public class ReviewProtectionLevelFixture
    {
        [TestMethod]
        public void ShouldGetOneProblemWithRuleViolationOnServiceContract()
        {
            ReviewProtectionLevel rule = new ReviewProtectionLevel();
            TypeNode typeNode = RuleHelper.GetTypeNodeFromType(typeof(IHelloWorld));
            rule.VisitAttributes(typeNode.Attributes);
            Assert.AreEqual(1, rule.Problems.Count);
        }

        [TestMethod]
        public void ShouldGetOneProblemWithRuleViolationOnOperationContract()
        {
            ReviewProtectionLevel rule = new ReviewProtectionLevel();
            TypeNode typeNode = RuleHelper.GetTypeNodeFromType(typeof(IHelloWorld));
            rule.VisitAttributes(typeNode.GetMembersNamed(Identifier.For("HelloWorld"))[0].Attributes);
            Assert.AreEqual(1, rule.Problems.Count);
        }

        [TestMethod]
        public void ShouldGetNoProblemWithNoRuleViolationOnServiceContract()
        {
            ReviewProtectionLevel rule = new ReviewProtectionLevel();
            TypeNode typeNode = RuleHelper.GetTypeNodeFromType(typeof(IHelloWorld2));
            rule.VisitAttributes(typeNode.Attributes);
            Assert.AreEqual(0, rule.Problems.Count);
        }

        [TestMethod]
        public void ShouldGetNoProblemWithNoRuleViolationOnOperationContract()
        {
            ReviewProtectionLevel rule = new ReviewProtectionLevel();
            TypeNode typeNode = RuleHelper.GetTypeNodeFromType(typeof(IHelloWorld2));
            rule.VisitAttributes(typeNode.GetMembersNamed(Identifier.For("HelloWorld"))[0].Attributes);
            Assert.AreEqual(0, rule.Problems.Count);
        }
    }
}
