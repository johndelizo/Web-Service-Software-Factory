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
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.Practices.ServiceFactory.HostDesigner;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.ServiceFactory.Extenders.HostDesigner.Wcf.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Microsoft.Practices.ServiceFactory.Extenders.HostDesigner.Wcf
{
	[Serializable]
	[CLSCompliant(false)]
	[ObjectExtender(typeof(Endpoint))]
	public class WcfEndpoint : ObjectExtender<Endpoint>
	{
		#region Constructors
		public WcfEndpoint()
		{
		}
		#endregion

		#region Properties
		private BindingType bindingType;

		[Category(HostDesignerWcfExtensionProvider.ExtensionProviderPropertyCategory),
         Description("Determines the WCF binding type for this endpoint."),
         DisplayName("Binding Type"),
		 ReadOnly(false),
		 Browsable(true)]
		[XmlElement("IsRequired")]
		public BindingType BindingType
		{
			get { return bindingType; }
			set { bindingType = value; }
		}
		#endregion
	}
}