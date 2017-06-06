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

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	public partial class DataContractDslDomainModel
	{
		protected override Type[] GetCustomDomainModelTypes()
		{
			return new System.Type[] 
				{
					typeof(ContractAddRule),
					typeof(DataContractAddRule),
					typeof(DataContractEnumAddRule),
					typeof(PrimitiveDataTypeCollectionAddRule),
					typeof(DataContractCollectionAddRule),
					typeof(FaultContractAddRule),
					typeof(DataElementAddRule),
					typeof(DataContractBaseNameChangeRule),
					typeof(DataContractBaseCanBeContainedOnContractsDeleteRule),
					typeof(ImplementationTechnologyChangeRule),
                    typeof(ReferenceDataTypeAddRule),
                    typeof(ReferenceDataTypeDeletingRule)
			};
		}
	}
}