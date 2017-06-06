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
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.Modeling.Common
{
    public class VSStatusBar
    {
        private IVsStatusbar statusBar;
		private IServiceProvider serviceProvider;

        public VSStatusBar() : 
            this(RuntimeHelper.ServiceProvider)
        {            
        }

        public VSStatusBar(IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(serviceProvider, "serviceProvider");
            this.statusBar = serviceProvider.GetService(typeof(SVsStatusbar)) as IVsStatusbar;
			this.serviceProvider = serviceProvider;
            Guard.ArgumentNotNull(this.statusBar, "SVsStatusbar service");
        }

        public void ShowMessage(string message)
        {
            if(!IsFrozen())
            {
				VSShellHelper.SetWaitCursor(this.serviceProvider);
                ErrorHandler.ThrowOnFailure(statusBar.SetText(message));
            }
        }

        public bool IsFrozen()
        {
            int frozen;
            ErrorHandler.ThrowOnFailure(statusBar.IsFrozen(out frozen));
            return Convert.ToBoolean(frozen);
        }

		[SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Microsoft.VisualStudio.Shell.Interop.IVsStatusbar.Animation(System.Int32,System.Object@)")]
		public void Clear()
        {
            if (!IsFrozen())
            {
                ErrorHandler.ThrowOnFailure(statusBar.SetText(Properties.Resources.StatusBarDefault));
                ErrorHandler.ThrowOnFailure(statusBar.Clear());
            }
        }
    }
}
