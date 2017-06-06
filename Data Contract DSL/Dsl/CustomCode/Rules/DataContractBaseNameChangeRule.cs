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

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	[RuleOn(typeof(DataContractBase), FireTime = TimeToFire.TopLevelCommit)]
	public partial class DataContractBaseNameChangeRule : ChangeRule
	{
		public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
		{
			if(e.DomainProperty.Id == DataContractBase.NameDomainPropertyId)
			{
				DataContractBase dataContract = e.ModelElement as DataContractBase;
				string oldValue = e.OldValue.ToString();
				foreach(Contract element in dataContract.Contracts)
				{
					if(element is DataContract)
					{
						((DataContract)element).DataMembers.ForEach(delegate(DataMember dataElement)
						{
							UpdatedElement(dataElement as ModelElementReference, e);
						});
					}
					else if(element is FaultContract)
					{
						((FaultContract)element).DataMembers.ForEach(delegate(DataMember dataElement)
						{
							UpdatedElement(dataElement as ModelElementReference, e);
						});					
					}
				}
			}
		}

		private void UpdatedElement(ModelElementReference element, ElementPropertyChangedEventArgs e)
		{
			if (element != null)
			{
				if (element.Name.Equals(e.OldValue))
				{
					element.Name = e.NewValue.ToString();
				}
				if (element.Type.Equals(e.OldValue))
				{
					element.Type = e.NewValue.ToString();
				}
			}
		}
	}
}