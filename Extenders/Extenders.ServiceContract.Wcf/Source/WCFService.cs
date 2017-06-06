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
using System.Globalization;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf.CodeGeneration;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf
{
	[Serializable]
	[CLSCompliant(false)]
	[ObjectExtender(typeof(Service))]
	public class WCFService : ObjectExtender<Service>
	{
		private ConcurrencyMode concurrencyMode;
		private InstanceContextMode instanceContextMode;

		#region Constructors

		public WCFService()
			: this(
			ConcurrencyMode.Single,
			InstanceContextMode.PerSession)
		{
		}

		public WCFService(
			ConcurrencyMode concurrencyMode,
			InstanceContextMode instanceContextMode)
		{
			this.concurrencyMode = concurrencyMode;
			this.instanceContextMode = instanceContextMode;			
		}

		#endregion

		#region Properties

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies whether a service supports single-threaded or multi-threaded modes of operation."),
         DisplayName("Concurrency Mode"),
         ReadOnly(false),
		 Browsable(true)]
		[XmlElement("ConcurrencyMode")]
		public ConcurrencyMode ConcurrencyMode
		{
			get { return concurrencyMode; }
			set { concurrencyMode = value; }
		}

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies the number of service instances available for handling calls that are contained in incoming messages."),
         DisplayName("Instance Context Mode"),
         ReadOnly(false),
		 Browsable(true)]
		[XmlElement("InstanceContextMode")]
		public InstanceContextMode InstanceContextMode
		{
			get { return instanceContextMode; }
			set { instanceContextMode = value; }
		}

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies how the code will be generated."),
         DisplayName("Artifact Link"),
         ReadOnly(true),
		 Browsable(true)]
		[XmlIgnore]
		public ServiceLink ArtifactLink
		{
			get
			{
				if (this.ModelElement != null &&
					this.ModelElement.ServiceContractModel != null)
				{
					return ArtifactLinkFactory.CreateInstance<ServiceLink>(
						(ModelElement)this.ModelElement,
						this.ModelElement.ServiceContractModel.ProjectMappingTable);
				}
				return null;
			}
		}

		#endregion
	}
}