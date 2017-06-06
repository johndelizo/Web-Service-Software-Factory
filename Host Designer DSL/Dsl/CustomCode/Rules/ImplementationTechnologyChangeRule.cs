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
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.Helpers;

namespace Microsoft.Practices.ServiceFactory.HostDesigner
{
	[RuleOn(typeof(HostApplication), FireTime = TimeToFire.TopLevelCommit)]
	public partial class ImplementationTechnologyChangeRule : ChangeRule
	{
		public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
		{
			if (e.NewValue is IExtensionProvider)
			{
				HostApplication hostApp = e.ModelElement as HostApplication;
				foreach (ServiceDescription sd in hostApp.ServiceDescriptions)
				{
					ExtensionProviderHelper.AttachObjectExtender((IExtensibleObject)sd, (IExtensionProvider)e.NewValue);
					foreach (Endpoint endpoint in sd.Endpoints)
					{
						ExtensionProviderHelper.AttachObjectExtender((IExtensibleObject)endpoint, (IExtensionProvider)e.NewValue);
					}
				}
			}
		}
	}
}
