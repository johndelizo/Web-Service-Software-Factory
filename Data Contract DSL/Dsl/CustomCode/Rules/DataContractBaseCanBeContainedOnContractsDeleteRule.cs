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
using System.Diagnostics;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	/// <summary>
	/// Synchronizes deletion changes between <see cref="DataContractDataElement"/> and <see cref="DataContract"/>
	/// </summary>
	[RuleOn(typeof(DataContractBaseCanBeContainedOnContracts), FireTime = TimeToFire.TopLevelCommit)]
	public partial class DataContractBaseCanBeContainedOnContractsDeleteRule : DeleteRule
	{
		public override void ElementDeleted(ElementDeletedEventArgs e)
		{
			DataContractBaseCanBeContainedOnContracts link = e.ModelElement as DataContractBaseCanBeContainedOnContracts;
			Debug.Assert(link != null, "link != null");

			DataContractBase sourceDataContractBase = link.DataContractBase;
			Contract targetContractElement = link.Contract;

			if(targetContractElement is DataContract)
			{
				DataContract dataContractElement = targetContractElement as DataContract;

				RemoveDataElement(
					dataContractElement.Store,
					dataContractElement.DataMembers,
					GetDataElement(dataContractElement.DataMembers, link.Id));
			}
			else if(targetContractElement is FaultContract)
			{
				FaultContract faultContractElement = targetContractElement as FaultContract;

				RemoveDataElement(
					faultContractElement.Store,
					faultContractElement.DataMembers,
					GetDataElement(faultContractElement.DataMembers, link.Id));			
			}

			base.ElementDeleted(e);
		}

		private void RemoveDataElement(Store store, LinkedElementCollection<DataMember> dataElements, DataMember dataElement)
		{
			if(dataElement != null)
			{
				using(Transaction transaction = store.TransactionManager.BeginTransaction())
				{
					dataElements.Remove(dataElement);
					transaction.Commit();
				}
			}		
		}

		private ModelElementReference GetDataElement(LinkedElementCollection<DataMember> dataElements, Guid linkId)
		{
			ModelElementReference dataElementFound = null;

			foreach(DataMember dataElement in dataElements)
		    {
				ModelElementReference dataContractDataElement = dataElement as ModelElementReference;

		        if(dataContractDataElement != null && dataContractDataElement.ModelElementGuid == linkId)
		        {
		            dataElementFound = dataContractDataElement;
		            break;
		        }
		    }

		    return dataElementFound;
		}
	}
}