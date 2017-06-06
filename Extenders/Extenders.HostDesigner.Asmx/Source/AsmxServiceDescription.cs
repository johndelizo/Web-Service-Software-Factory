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
using Microsoft.Practices.ServiceFactory.HostDesigner;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.Practices.ServiceFactory.Extenders.HostDesigner.Asmx.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;

namespace Microsoft.Practices.ServiceFactory.Extenders.HostDesigner.Asmx
{
	[Serializable]
	[CLSCompliant(false)]
	[ObjectExtender(typeof(ServiceDescription))]
	public class AsmxServiceDescription : ObjectExtender<ServiceDescription>
	{
		public AsmxServiceDescription()
		{
		}

		[ReadOnly(true),
			Browsable(false)]
		[XmlIgnore]
		public AsmxServiceArtifactLink ServiceArtifactLink
		{
			get
			{
				ServiceDescription serviceDescriptionModelElement = this.ModelElement as ServiceDescription;

				if(serviceDescriptionModelElement == null ||
					serviceDescriptionModelElement.HostApplication == null ||
					string.IsNullOrEmpty(serviceDescriptionModelElement.HostApplication.ImplementationProject))
				{
					return null;
				}

				return ArtifactLinkFactory.CreateInstance<AsmxServiceArtifactLink>(
											serviceDescriptionModelElement,
											serviceDescriptionModelElement.HostApplication.ImplementationProject,
											string.Empty);
			}
		}
	}
}
