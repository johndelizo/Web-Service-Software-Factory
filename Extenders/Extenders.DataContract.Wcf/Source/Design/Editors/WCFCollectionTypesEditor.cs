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
using Microsoft.Practices.ServiceFactory.DataContracts;

namespace Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf.Design.Editors
{
	[CLSCompliant(false)]
	public class WCFCollectionTypesEditor : CollectionTypesEditor
	{
		private string[] KeyFilter = { CollectionTypes.ListKey, CollectionTypes.CollectionKey, CollectionTypes.DictionaryKey };

		protected override void FillValues()
		{
			Dictionary<string, Type> filteredCollectionTypes = new Dictionary<string, Type>();

			foreach (string key in KeyFilter)
			{
				filteredCollectionTypes.Add(key, CollectionTypes.Values[key]);
			}

			base.AddKeyValueItems(filteredCollectionTypes);
		}
	}	
}
