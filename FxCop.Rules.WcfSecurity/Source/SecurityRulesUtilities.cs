//===============================================================================
// Microsoft patterns & practices
// Web Service Software Factory 2010
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// Class that reports if any problem was raised by all other rules in this library.
    /// </summary>
    static class SecurityRulesUtilities
    {
        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public static string ResourceName
        {
            get { return "Microsoft.Practices.FxCop.Rules.WcfSecurity.WcfSecurityRules"; }
        }

        /// <summary>
        /// Gets the current assembly.
        /// </summary>
        /// <value>The current assembly.</value>
        public static Assembly CurrentAssembly
        {
            get { return typeof(SecurityRulesUtilities).Assembly; }
        }
    }
}
