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
using System.Globalization;
using Microsoft.Practices.Modeling.CodeGeneration;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf
{
	[Serializable]
	[ObjectExtender(typeof(XsdElementFault))]
	[CLSCompliant(false)]
	public class WCFXsdElementFault : ObjectExtender<XsdElementFault>
	{
		public WCFXsdElementFault()
		{
		}

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies how the code will be generated."),
         DisplayName("Artifact Link"),
         ReadOnly(true),
		 Browsable(true)]
		[XmlIgnore]
		public XsdElementFaultLink ArtifactLink
		{
			get
			{
				if(this.ModelElement != null &&
				   this.ModelElement.Operation != null &&
				   this.ModelElement.Operation.ServiceContractModel != null)
				{
					XsdElementFaultLink alink = ArtifactLinkFactory.CreateInstance<XsdElementFaultLink>(
						(ModelElement)this.ModelElement,
						this.ModelElement.Operation.ServiceContractModel.ProjectMappingTable);
					alink.ItemName = this.ModelElement.Name;
                    SetDataValues(alink);
					return alink;
				}
				return null;
			}
		}

        private void SetDataValues(IArtifactLink link)
        {
            Utility.SetData(link, this.ModelElement.Operation.ServiceContractModel.SerializerType == SerializerType.XmlSerializer, "UseXmlSerializer");
            Utility.SetData(link, ((XsdElementFault)this.ModelElement).Element, "Element");
        }
	}
}
