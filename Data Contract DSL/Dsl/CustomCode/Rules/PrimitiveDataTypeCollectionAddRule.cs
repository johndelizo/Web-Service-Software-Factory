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
using System.Reflection;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.Helpers;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	[RuleOn(typeof(PrimitiveDataTypeCollection), FireTime = TimeToFire.TopLevelCommit)]
	public partial class PrimitiveDataTypeCollectionAddRule : AddRule
	{
		public override void ElementAdded(ElementAddedEventArgs e)
		{
			PrimitiveDataTypeCollection dataContractPrimitiveCollection = e.ModelElement as PrimitiveDataTypeCollection;
			if (dataContractPrimitiveCollection == null)
			{
				return;
			} 

			DataContractModel root = dataContractPrimitiveCollection.DataContractModel;
			if(root != null &&
			   root.ImplementationTechnology != null)
			{
				ExtensionProviderHelper.AttachObjectExtender(dataContractPrimitiveCollection, root.ImplementationTechnology);
			}

			if(string.IsNullOrEmpty(dataContractPrimitiveCollection.ItemType))
			{
				dataContractPrimitiveCollection.ItemType = typeof(string).FullName;
			}

			if (String.IsNullOrEmpty(dataContractPrimitiveCollection.Namespace))
			{
				dataContractPrimitiveCollection.Namespace = ArtifactLinkHelper.DefaultNamespace(e.ModelElement);
			}

			UpdateDataContractCollectionType(dataContractPrimitiveCollection, CollectionTypes.Values[CollectionTypes.ListKey]);
		}


		// We do not currently have a way to add rules for object extenders, so this is looking
		// for an appropriate property for an object extender.
		private void UpdateDataContractCollectionType(DataContractCollectionBase dcElement, Type collectionType)
		{
			if (dcElement != null && dcElement.ObjectExtender != null)
			{
				PropertyInfo property = dcElement.ObjectExtender.GetType().GetProperty("CollectionType");

				if (property != null)
				{
                    object value = property.GetValue(dcElement.ObjectExtender, null);

                    if (value == null)
					    property.SetValue(dcElement.ObjectExtender, collectionType, null);
				}
			}
		}
	}
}