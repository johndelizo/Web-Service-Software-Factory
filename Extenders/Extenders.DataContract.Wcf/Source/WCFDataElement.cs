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
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;

namespace Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf
{
	[Serializable]
	[CLSCompliant(false)]
	[ObjectExtender(typeof(DataMember))]
	public class WCFDataElement : ObjectExtender<DataMember>
	{
		#region Constructors

		public WCFDataElement()
		{
		}

		#endregion

		#region Properties

		private bool isRequired = false;

		[Category(DataContractWcfExtensionProvider.ExtensionProviderPropertyCategory),
        Description("Indicates whether this field is required or not."),
         DisplayName("Is Required"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("IsRequired")]
		public bool IsRequired
		{
			get { return isRequired; }
			set { isRequired = value; }
		}

		private int order = 0;
		[Category(DataContractWcfExtensionProvider.ExtensionProviderPropertyCategory),
        Description("The unique order number of this member within its data contract. Ordering should start with 1."),
         DisplayName("Order"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("Order")]
		public int Order
		{
			get { return order; }
			set { order = value; }
		}

		#endregion
	}
}
