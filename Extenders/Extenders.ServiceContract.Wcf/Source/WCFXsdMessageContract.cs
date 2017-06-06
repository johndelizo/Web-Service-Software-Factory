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
using Microsoft.Practices.VisualStudio.Helper;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf
{
	[Serializable]
	[ObjectExtender(typeof(XsdMessage))]
	[CLSCompliant(false)]
	public class WCFXsdMessageContract: ObjectExtender<XsdMessage>
	{
		#region Constructors

		public WCFXsdMessageContract()
		{
		}

		#endregion

		#region Properties

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies how the code will be generated."),
         DisplayName("Artifact Link"),
         ReadOnly(true),
		 Browsable(true)]
		[XmlIgnore]
		public XsdMessageContractLink ArtifactLink
		{
			get
			{
				if(IsValidModelElement())
				{
					XsdMessageContractLink link = ArtifactLinkFactory.CreateInstance<XsdMessageContractLink>(
						(ModelElement)this.ModelElement,
						this.ModelElement.ServiceContractModel.ProjectMappingTable);
					return link;
				}
				return null;
			}
		}

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies how the code will be generated."),
         DisplayName("XSD Message Artifact Link"),
         ReadOnly(true),
		 Browsable(true)]
		[XmlIgnore]
		public XsdMessageContractElementLink XsdMessageContractElementArtifactLink
		{
			get
			{
				if (IsValidModelElement())
				{
					XsdMessageContractElementLink link = ArtifactLinkFactory.CreateInstance<XsdMessageContractElementLink>(
						(ModelElement)this.ModelElement,
						this.ModelElement.ServiceContractModel.ProjectMappingTable,
						ReflectionHelper.GetProvider <XsdMessage>("Element"));
					SetDataValues(link);
					return link;
				}
				return null;
			}
		}

		private bool IsValidModelElement()
		{
			return this.ModelElement != null &&
				   this.ModelElement.ServiceContractModel != null;
		}

		private void SetDataValues(IArtifactLink link)
		{
			Utility.SetData(link, this.ModelElement.ServiceContractModel.SerializerType == SerializerType.XmlSerializer, "UseXmlSerializer");
			Utility.SetData(link, ((XsdMessage)this.ModelElement).Element, "Element");
			Utility.SetData(link, ((XsdMessage)this.ModelElement).Namespace, "Namespace");
		}

		#endregion
	}
}
