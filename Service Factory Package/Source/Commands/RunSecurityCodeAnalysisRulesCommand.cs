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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.Modeling.Common.Logging;
using Vs = VSLangProj;
using Web = VsWebSite;

namespace Microsoft.Practices.ServiceFactory.Commands
{
    /// <summary>
    /// Run the Code Analysis rules in the selected project and all its references.
    /// </summary>
    public class RunSecurityCodeAnalysisRulesCommand : CodeAnalysisRulesCommand 
    {
        public RunSecurityCodeAnalysisRulesCommand(IServiceProvider provider)
            : base(provider)
        { }

        protected override string RuleCheckIdPrefix
        {
            get { return "WCFS"; }
        }
        
        protected override string RulesetFileName
        {
            get { return "WcfSecurityRules.ruleset"; }
        }
	}
}
