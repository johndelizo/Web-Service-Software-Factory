using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Microsoft.FxCop.Sdk.Introspection;
using Microsoft.FxCop.Common;
using Microsoft.FxCop.Sdk;
using System.ServiceModel.Security;
using Microsoft.Cci;
using Microsoft.Fugue;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Class that implements the CertificateRevocationMode rule.
    /// </summary>
    /// <remarks>
    /// This rule checks if there is a code that use the System.ServiceModel.Description.ClientCredentials class 
    /// or the System.ServiceModel.Description.ServiceCredentials class 
    /// and the property in that class 'ServiceCertificate.Authentication.RevocationMode' has a value of 'NoCheck'.
    /// Notice that this classes me be used from a client proxy class with the 'ClientCredentials' or 'ServiceCredentials' property.
    /// </remarks>
    public sealed class CertificateRevocationMode : SecurityControlFlowRule 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CertificateRevocationMode"/> class.
        /// </summary>
        public CertificateRevocationMode() : base("CertificateRevocationMode")
        {
        }
 
        /// <summary>
        /// Gets the visitor.
        /// </summary>
        /// <value>The visitor.</value>
        public override Microsoft.Fugue.ExecutionVisitor Visitor
        {
            get { return this; }
        }

        /// <summary>
        /// Visits the call.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="callee">The callee.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="isVirtualCall">if set to <c>true</c> [is virtual call].</param>
        /// <param name="programContext">The program context.</param>
        /// <param name="stateBeforeInstruction">The state before instruction.</param>
        /// <param name="stateAfterInstruction">The state after instruction.</param>
        public override void VisitCall(
            Variable destination, 
            Variable receiver, 
            Method callee, 
            ExpressionList arguments, 
            bool isVirtualCall, 
            Microsoft.Fugue.IProgramContext programContext, 
            Microsoft.Fugue.IExecutionState stateBeforeInstruction, 
            Microsoft.Fugue.IExecutionState stateAfterInstruction)
        {
            if ((callee.DeclaringType.GetRuntimeType() == typeof(X509ServiceCertificateAuthentication) ||
                 callee.DeclaringType.GetRuntimeType() == typeof(X509ClientCertificateAuthentication)) &&
                 (callee.Name.Name.Equals("set_RevocationMode", StringComparison.InvariantCultureIgnoreCase)))
            {
                IAbstractValue value = stateBeforeInstruction.Lookup((Variable)arguments[0]);
                IIntValue intValue = value.IntValue(stateBeforeInstruction);

                if (intValue != null)
                {
                    X509RevocationMode mode = (X509RevocationMode)intValue.Value;
                    if (mode == X509RevocationMode.NoCheck)
                    {
                        Resolution resolution = base.GetResolution();
                        Problem problem = new Problem(resolution, programContext);
                        base.Problems.Add(problem);
                    }
                }
            }

            base.VisitCall(destination, receiver, callee, arguments, isVirtualCall, programContext, stateBeforeInstruction, stateAfterInstruction);
        }
    }
}
