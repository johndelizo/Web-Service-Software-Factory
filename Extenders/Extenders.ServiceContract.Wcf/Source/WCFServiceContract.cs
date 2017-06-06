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
using System.Net.Security;
using System.ServiceModel;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using System.Drawing.Design;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;
using System.Diagnostics;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf.CodeGeneration;
using System.Globalization;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf
{
	[Serializable]
	[CLSCompliant(false)]
	[ObjectExtender(typeof(Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract))]
	public class WCFServiceContract : ObjectExtender<Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract>
	{
		#region Constructors

		public WCFServiceContract()
			: this(ProtectionLevel.None, SessionMode.Allowed)
		{
		}

		public WCFServiceContract(ProtectionLevel protectionLevel, SessionMode sessionMode)
		{
			this.protectionLevel = protectionLevel;
			this.sessionMode = sessionMode;
		}
		#endregion

		#region Properties

		private ProtectionLevel protectionLevel;

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
         Description("Indicates the protection level of all messages supporting this contract."),
         DisplayName("Protection Level"),
		 ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("ProtectionLevel")]
		public ProtectionLevel ProtectionLevel
		{
			get { return protectionLevel; }
			set { protectionLevel = value; }
		}

		private SessionMode sessionMode;

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
         Description("Specifies the session support for this contract. The binding on the endpoint must also support this session choice."),
         DisplayName("Session Mode"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("SessionMode")]
		public SessionMode SessionMode
		{
			get { return sessionMode; }
			set { sessionMode = value; }
		}

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
	     Description("The name of the service contract interface that will be generated."),
         DisplayName("Service Contract Name"),
         ReadOnly(true),
		 BrowsableAttribute(true)]
		[XmlIgnore]
		public string ServiceContractName
		{
			get
			{
				return string.Format(CultureInfo.CurrentCulture, Properties.Resources.ServiceContractNameFormat, ModelElement.Name);
			}
		}

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies how the code will be generated."),
         DisplayName("Artifact Link"),
         ReadOnly(true),
		 Browsable(true)]
		[XmlIgnore]
		public ServiceContractLink ArtifactLink
		{
			get
			{
				if (this.ModelElement != null && 
					this.ModelElement.ServiceContractModel != null)
				{
					ServiceContractLink link = ArtifactLinkFactory.CreateInstance<ServiceContractLink>(
						(ModelElement)this.ModelElement,
						this.ModelElement.ServiceContractModel.ProjectMappingTable);

					link.ItemName = ServiceContractName;

					return link;
				}
				return null;
			}
		}

		#endregion
	}
}