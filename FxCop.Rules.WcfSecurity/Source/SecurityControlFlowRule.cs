using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.FxCop.Sdk.Introspection;
using Microsoft.FxCop.Sdk;
using System.Reflection;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// base control flow rule class.
    /// </summary>
    public abstract class SecurityControlFlowRule : BaseControlFlowRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SecurityControlFlowRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected SecurityControlFlowRule(string name)
            : base(name, SecurityRulesUtilities.ResourceName, SecurityRulesUtilities.CurrentAssembly)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SecurityControlFlowRule"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceAssembly">The resource assembly.</param>
        protected SecurityControlFlowRule(string name, string resourceName, Assembly resourceAssembly)
            : base(name, resourceName, resourceAssembly)
        {
        }

        /// <summary>
        /// Gets the target visibility.
        /// </summary>
        /// <value>The target visibility.</value>
        public override TargetVisibilities TargetVisibility
        {
            get { return (TargetVisibilities.All); }
        }

        /// <summary>
        /// Gets the visitor.
        /// </summary>
        /// <value>The visitor.</value>
        public override Microsoft.Fugue.ExecutionVisitor Visitor
        {
            get { return this; }
        }
    }
}
