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

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	[RuleOn(typeof(MessageBase), FireTime = TimeToFire.TopLevelCommit)]
	public partial class MessageContractBaseAddRule : AddRule
	{
		public override void ElementAdded(ElementAddedEventArgs e)
		{
			MessageBase contract = e.ModelElement as MessageBase;
			ServiceContractModel root = contract.ServiceContractModel;

			if(root != null)
			{
				if(String.IsNullOrEmpty(contract.Namespace))
				{
					contract.Namespace = root.Namespace;
				}
			}
		}
	}
}
