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
using System.ComponentModel;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.TypeDescription;
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	[TypeDescriptionProvider(typeof(ExtendedTypeDescriptionProvider))]
	public abstract partial class ExtensibleModelElement : IExtensibleObject
	{
		#region IExtensibleObject Members

		private object objectExtender;

		[Browsable(false)]
		public object ObjectExtender
		{
			get { return objectExtender; }
			set { objectExtender = value; }
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public ModelElement ModelElement
		{
			get { return this; }
		}

		[Browsable(false)]
		public virtual IExtensionProvider ExtensionProvider
		{
			get 
			{
				ReadOnlyCollection<ModelElement> elements =
				   this.Store.ElementDirectory.FindElements(DataContractModel.DomainClassId);
				if (elements.Count == 0)
				{
					return null;
				}
				return ((DataContractModel)elements[0]).ImplementationTechnology;
			}
		}

		#endregion
	}
}
