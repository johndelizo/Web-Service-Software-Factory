using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Cci;
using Microsoft.Fugue;
using Microsoft.FxCop.Sdk.Introspection;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Microsoft.Fugue.Checker;
using CCIWrapper;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    /// <summary>
    /// Summary description for CertificateValidationModeFixture
    /// </summary>
    [TestClass]
    public class CertificateValidationRevocationModeFixture
    {
        [TestMethod]
        public void ShouldGetOneProblemWithServiceCertificateValidationModeNoneRuleViolation()
        {
            CertificateValidationMode rule = new CertificateValidationMode();
            Method callee = Method.GetMethod(typeof(X509ServiceCertificateAuthentication).GetMethod("set_CertificateValidationMode"));
            ExpressionList arguments = new ExpressionList(new Variable(NodeType.VariableDeclaration));
            rule.VisitCall(
                null, null, 
                callee, 
                arguments,
                false, new MockProgramContext(),
                new MockStateBeforeInstruction((int)X509CertificateValidationMode.None), null);

            Assert.IsNotNull(rule.Problems);
            Assert.AreEqual(1, rule.Problems.Count);
        }

        [TestMethod]
        public void ShouldGetOneProblemWithClientCertificateValidationModeNoneRuleViolation()
        {
            CertificateValidationMode rule = new CertificateValidationMode();
            Method callee = Method.GetMethod(typeof(X509ClientCertificateAuthentication).GetMethod("set_CertificateValidationMode"));
            ExpressionList arguments = new ExpressionList(new Variable(NodeType.VariableDeclaration));
            rule.VisitCall(
                null, null,
                callee,
                arguments,
                false, new MockProgramContext(),
                new MockStateBeforeInstruction((int)X509CertificateValidationMode.None), null);

            Assert.IsNotNull(rule.Problems);
            Assert.AreEqual(1, rule.Problems.Count);
        }

        [TestMethod]
        public void ShouldGetOneProblemWithServiceCertificateValidationModePeerOrChainTrustRuleViolation()
        {
            CertificateValidationMode rule = new CertificateValidationMode();
            Method callee = Method.GetMethod(typeof(X509ServiceCertificateAuthentication).GetMethod("set_CertificateValidationMode"));
            ExpressionList arguments = new ExpressionList(new Variable(NodeType.VariableDeclaration));
            rule.VisitCall(
                null, null,
                callee,
                arguments,
                false, new MockProgramContext(),
                new MockStateBeforeInstruction((int)X509CertificateValidationMode.PeerOrChainTrust), null);

            Assert.IsNotNull(rule.Problems);
            Assert.AreEqual(1, rule.Problems.Count);
        }

        [TestMethod]
        public void ShouldGetOneProblemWithClientCertificateValidationModePeerOrChainTrustRuleViolation()
        {
            CertificateValidationMode rule = new CertificateValidationMode();
            Method callee = Method.GetMethod(typeof(X509ClientCertificateAuthentication).GetMethod("set_CertificateValidationMode"));
            ExpressionList arguments = new ExpressionList(new Variable(NodeType.VariableDeclaration));
            rule.VisitCall(
                null, null,
                callee,
                arguments,
                false, new MockProgramContext(),
                new MockStateBeforeInstruction((int)X509CertificateValidationMode.PeerOrChainTrust), null);

            Assert.IsNotNull(rule.Problems);
            Assert.AreEqual(1, rule.Problems.Count);
        }

        [TestMethod]
        public void ShouldGetOneProblemWithClientCertificateRevocationModeRuleViolation()
        {
            CertificateRevocationMode rule = new CertificateRevocationMode();
            Method callee = Method.GetMethod(typeof(X509ClientCertificateAuthentication).GetMethod("set_RevocationMode"));
            ExpressionList arguments = new ExpressionList(new Variable(NodeType.VariableDeclaration));
            rule.VisitCall(
                null, null,
                callee,
                arguments,
                false, new MockProgramContext(),
                new MockStateBeforeInstruction((int)X509RevocationMode.NoCheck), null);

            Assert.IsNotNull(rule.Problems);
            Assert.AreEqual(1, rule.Problems.Count);
        }

        [TestMethod]
        public void ShouldGetOneProblemWithServiceCertificateRevocationModeRuleViolation()
        {
            CertificateRevocationMode rule = new CertificateRevocationMode();
            Method callee = Method.GetMethod(typeof(X509ServiceCertificateAuthentication).GetMethod("set_RevocationMode"));
            ExpressionList arguments = new ExpressionList(new Variable(NodeType.VariableDeclaration));
            rule.VisitCall(
                null, null,
                callee,
                arguments,
                false, new MockProgramContext(),
                new MockStateBeforeInstruction((int)X509RevocationMode.NoCheck), null);

            Assert.IsNotNull(rule.Problems);
            Assert.AreEqual(1, rule.Problems.Count);
        }

        #region Mocks

        class MockProgramContext : IProgramContext
        {
            public Microsoft.Fugue.MILA.ISourceContext SourceContext
            {
                get { return SourceContextWrapper.For(new SourceContext()); }
            }

            #region Not implemented IProgramContext Members

            public IAnalysisContext AnalysisContext
            {
                get { throw new Exception("The method or operation AnalysisContext is not implemented."); }
            }

            public Microsoft.Fugue.ProtocolManagement.AttributeSet AttributesOnMethod(Method method)
            {
                throw new Exception("The method or operation AttributesOnMethod is not implemented.");
            }

            public Microsoft.Fugue.ProtocolManagement.AttributeSet AttributesOnParameter(Method method, Parameter parameter)
            {
                throw new Exception("The method or operation AttributesOnParameter is not implemented.");
            }

            public Microsoft.Fugue.ProtocolManagement.AttributeSet AttributesOnReturnValue(Method method)
            {
                throw new Exception("The method or operation AttributesOnReturnValue is not implemented.");
            }

            public Microsoft.Fugue.MILA.IBlock Block
            {
                get { throw new Exception("The method or operation Block is not implemented."); }
            }

            public Microsoft.Fugue.MILA.IControlFlowGraph ControlFlowGraph
            {
                get { throw new Exception("The method or operation ControlFlowGraph is not implemented."); }
            }

            public IVarDef DefAtCurrentStatement(Variable var)
            {
                throw new Exception("The method or operation DefAtCurrentStatement is not implemented.");
            }

            public bool FlowsTo(IVarDef def, IVarUse use)
            {
                throw new Exception("The method or operation FlowsTo is not implemented.");
            }

            public System.Collections.IEnumerable GetVisitorState(Variable var)
            {
                throw new Exception("The method or operation GetVisitorState is not implemented.");
            }

            public Method Method
            {
                get { throw new Exception("The method or operation Method is not implemented."); }
            }

            public void SetVisitorState(Variable var, object state)
            {
                throw new Exception("The method or operation SetVisitorState is not implemented.");
            }

            public Statement Statement
            {
                get { throw new Exception("The method or operation Statement is not implemented."); }
            }

            public IVarUse UseAtCurrentStatement(Variable var)
            {
                throw new Exception("The method or operation UseAtCurrentStatement is not implemented.");
            }

            #endregion
        }

        class MockStateBeforeInstruction : IExecutionState
        {
            private int value;

            public MockStateBeforeInstruction(int value)
            {
                this.value = value;
            }

            public IAbstractValue Lookup(Variable variable)
            {
                return new MockAbstractValue(value);
            }

            #region Not implemented IExecutionState Members

            public IAbstractValue GetContents(IAbstractValue value)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IAbstractValue GetField(IAbstractValue value, Field field)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public System.Collections.IEnumerable ReachableValues
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public string VariableName(Variable v, Method currentMethod)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public System.Collections.IEnumerable Variables
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            #endregion
        }

        class MockAbstractValue : IAbstractValue
        {
            private int value;

            public MockAbstractValue(int value)
            {
                this.value = value;
            }

            public IIntValue IntValue(IExecutionState executionState)
            {
                return new MockIntValue(this.value);
            }

            #region Not Implemented IAbstractValue Members

            public IArrayValue ArrayValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public TypeNode BestType(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IIndirectValue IndirectValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool IsLive(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool IsMayBeNull(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool IsNotNull(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool IsNull(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IStringValue StringValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public ITypeValue TypeValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
        }

        class MockIntValue : IIntValue
        {
            private int value;

            public MockIntValue(int value)
            {
                this.value = value;
            }

            #region IIntValue Members

            public int Value
            {
                get { return value; }
            }

            #endregion

            #region IAbstractValue Members

            public IArrayValue ArrayValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public TypeNode BestType(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IIndirectValue IndirectValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IIntValue IntValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool IsLive(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool IsMayBeNull(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool IsNotNull(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool IsNull(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IStringValue StringValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public ITypeValue TypeValue(IExecutionState executionState)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
        }

        #endregion
    }
}
