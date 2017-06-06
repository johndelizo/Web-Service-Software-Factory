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

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	[RuleOn(typeof(Service), FireTime = TimeToFire.TopLevelCommit)]
	public partial class ServiceAddRule : AddRule
	{
		public override void ElementAdded(ElementAddedEventArgs e)
		{
			Service service = e.ModelElement as Service;
			ServiceContractModel root = service.ServiceContractModel;

			if(root != null)
			{
                if (root.ImplementationTechnology != null)
                {
                    ExtensionProviderHelper.AttachObjectExtender(service, root.ImplementationTechnology);
                }

				if (String.IsNullOrEmpty(service.Namespace))
				{
					service.Namespace = root.Namespace;
				}
			}
		}
	}
}
