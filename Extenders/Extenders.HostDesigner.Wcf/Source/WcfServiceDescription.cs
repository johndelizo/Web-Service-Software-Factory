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
	[ObjectExtender(typeof(ServiceDescription))]
	public class WcfServiceDescription : ObjectExtender<ServiceDescription>
	{
		private const string WebProjectKind = "{E24C65DC-7377-472b-9ABA-BC803B73C61A}";
		private const string AppConfigFileName = "App";
		private const string WebConfigFileName = "Web";

		public WcfServiceDescription()
		{
		}

		#region Properties

		private bool enableMetadataPublishing = false;

		[Category(HostDesignerWcfExtensionProvider.ExtensionProviderPropertyCategory),
         Description("A value that indicates whether this service should expose meta-data about its contract and policies."),
         DisplayName("Enable Metadata Publishing"),
		 ReadOnly(false),
		 Browsable(true)]
		[XmlElement("IsRequired")]
		public bool EnableMetadataPublishing
		{
			get { return enableMetadataPublishing; }
			set { enableMetadataPublishing = value; }
		}

		#endregion

		[ReadOnly(true),
		Browsable(false)]
		[XmlIgnore]
		public WcfServiceArtifactLink ServiceArtifactLink
		{
			get
			{
				return CreateLink<WcfServiceArtifactLink>();
			}
		}

		[ReadOnly(true),
	     Browsable(false)]
		[XmlIgnore]
		public WcfServiceConfigurationArtifactLink ServiceConfigurationArtifactLink
		{
			get
			{
				WcfServiceConfigurationArtifactLink link = CreateLink<WcfServiceConfigurationArtifactLink>();

				if(link != null)
				{
					if(link.Project.Kind.Equals(WebProjectKind))
					{
						link.ItemName = WebConfigFileName;
					}
					else
					{
						link.ItemName = AppConfigFileName;
					}
				}

				return link;
			}
		}

		private TAlink CreateLink<TAlink>() where TAlink : ArtifactLink, new()
		{
			ServiceDescription serviceDescriptionModelElement = this.ModelElement as ServiceDescription;

			if(serviceDescriptionModelElement == null ||
				serviceDescriptionModelElement.HostApplication == null ||
				string.IsNullOrEmpty(serviceDescriptionModelElement.HostApplication.ImplementationProject))
			{
				return default(TAlink);
			}

			return ArtifactLinkFactory.CreateInstance<TAlink>(
										serviceDescriptionModelElement,
										serviceDescriptionModelElement.HostApplication.ImplementationProject,
										string.Empty);
		}
	}
}