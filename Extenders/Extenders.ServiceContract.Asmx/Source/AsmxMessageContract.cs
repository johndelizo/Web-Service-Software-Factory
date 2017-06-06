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
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using System.Drawing.Design;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx.CodeGeneration;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx
{
	[Serializable]
	[CLSCompliant(false)]
	[ObjectExtender(typeof(Message))]
	public class AsmxMessageContract : ObjectExtender<Message>
	{
		public AsmxMessageContract()
		{
		}

		[Category(ServiceContractAsmxExtensionProvider.ExtensionProviderPropertyCategory),
			 Description("Specifies how the code will be generated."),
			 ReadOnly(true),
            DisplayName("Artifact Link"),
            Browsable(true)]
		[XmlIgnore]
		public AsmxMessageContractLink ArtifactLink
		{
			get
			{
				if (this.ModelElement != null &&
					this.ModelElement.ServiceContractModel != null)
				{
					return ArtifactLinkFactory.CreateInstance<AsmxMessageContractLink>(
						(ModelElement)this.ModelElement,
						this.ModelElement.ServiceContractModel.ProjectMappingTable);
				}
				return null;
			}
		}
	}
}