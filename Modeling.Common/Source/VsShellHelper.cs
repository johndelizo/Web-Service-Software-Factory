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
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.Practices.Modeling.Common.Logging;

namespace Microsoft.Practices.Modeling.Common
{
	public static class VSShellHelper
	{
		public static void ShowErrorDialog(IServiceProvider provider, string error)
		{
			Guard.ArgumentNotNull(provider, "provider");
			Guard.ArgumentNotNullOrWhiteSpaceString(error, "error");

			IUIService uiService = provider.GetService(typeof(IUIService)) as IUIService;
			if (uiService != null)
			{
				uiService.ShowError(error);
				return;
			}
			Logger.Write(error);
		}

		public static void SetWaitCursor(IServiceProvider provider)
		{
			Guard.ArgumentNotNull(provider, "provider");
			
			IVsUIShell shell = provider.GetService(typeof(SVsUIShell)) as IVsUIShell;
			if (shell != null)
			{
				ErrorHandler.ThrowOnFailure(shell.SetWaitCursor());
			}
		}
	}
}
