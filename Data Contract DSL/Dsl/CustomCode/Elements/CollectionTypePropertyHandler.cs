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
using System.Collections.ObjectModel;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	public abstract partial class DataMember
	{
		internal sealed partial class CollectionTypePropertyHandler
		{
			protected override void OnValueChanged(DataMember element, Type oldValue, Type newValue)
			{
				base.OnValueChanged(element, oldValue, newValue);
				Type type = typeof(Collection<>).GetGenericArguments()[0];
				if (element.collectionTypePropertyStorage == type)
				{
					element.collectionTypePropertyStorage = element.collectionTypePropertyStorage.MakeArrayType();
				}
			}
		}
	}
}
