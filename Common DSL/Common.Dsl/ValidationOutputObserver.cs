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
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.Practices.Modeling.Common.Properties;
using Microsoft.Practices.Modeling.Common.Logging;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.Modeling.Common;
using System;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
	public class ValidationOutputObserver : ValidationMessageObserver
	{
        private VSStatusBar status;
        private IServiceProvider serviceProvider;

        public ValidationOutputObserver(IServiceProvider serviceProvider)
            : base()
        {
            this.serviceProvider = serviceProvider;
        }

		protected override void OnValidationBeginning(ValidationContext context)
		{
            if (context != null)
            {
                if (context.Categories != ValidationCategories.Load &&
                    context.Categories != ValidationCategories.Open)
                {
                    Logger.Clear();
                }
                Logger.SuspendRefresh();
            }
            status = new VSStatusBar(serviceProvider);
            status.ShowMessage(Properties.Resources.ValidationProgressMessage);
		}
  
		protected override void OnValidationEnded(ValidationContext context)
		{
            if (context != null)
            {

                if (context.Categories == ValidationCategories.Menu &&
                    context.CurrentViolations.Count == 0)
                {
                    Logger.Write(
                        string.Format(CultureInfo.CurrentCulture, Properties.Resources.ValidationEndedMessage,
                            Logger.Messages[TraceEventType.Error],
                            Logger.Messages[TraceEventType.Warning],
                            Logger.Messages[TraceEventType.Information]),
                        TraceEventType.Information);
                }
                Logger.ResumeRefresh();
                ValidationEngine.Reset();
            }
            status.Clear();
		}
    }
}

