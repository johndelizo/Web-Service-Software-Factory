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
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Net.Security;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf.CodeGeneration;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf
{
	[Serializable]
	[ObjectExtender(typeof(Message))]
	[CLSCompliant(false)]
	public class WCFMessageContract: ObjectExtender<Message>
	{
		#region Constructors

		// ctor with default values.
		// defaults for WCF: 
		// - SerializerType.DataContract
		// - IsWrapped = true
		public WCFMessageContract()
			: this(false)
		{
		}

		public WCFMessageContract(bool isWrapped)
		{
			this.isWrapped = isWrapped;
		} 

		#endregion

		#region Properties

		private bool isWrapped;

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
         Description("When IsWrapped is set to false, the data contracts used by a service contract will not be wrapped. When this value is set to true, the data contracts will be wrapped. NOTE: The same value should be set for Request and Response to avoid duplicate proxy message classes."),
         DisplayName("Is Wrapped"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("IsWrapped")]
		public bool IsWrapped
		{
			get { return isWrapped; }
			set { isWrapped = value; }
		}

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
	     Description("Specifies how the code will be generated."),
         DisplayName("Artifact Link"),
         ReadOnly(true),
         Browsable(true)]
		[XmlIgnore]
		public MessageContractLink ArtifactLink
		{
			get
			{
				if (this.ModelElement != null &&
					this.ModelElement.ServiceContractModel != null)
				{
					return ArtifactLinkFactory.CreateInstance<MessageContractLink>(
						(ModelElement)this.ModelElement,
						this.ModelElement.ServiceContractModel.ProjectMappingTable);
				}
				return null;
			}
		}

		#endregion
	}
}