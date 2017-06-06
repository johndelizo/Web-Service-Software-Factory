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
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.Practices.ServiceFactory.DataContracts;
using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;

namespace Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx
{
	[Serializable]
	[ObjectExtender(typeof(FaultContract))]
	[CLSCompliant(false)]
	public class AsmxFaultContract : ObjectExtender<FaultContract>
	{
		#region Constructors

		public AsmxFaultContract()
		{
		}

		#endregion

        #region Properties

        [Category(DataContractAsmxExtensionProvider.ExtensionProviderPropertyCategory),
         Description("Specifies how the code will be generated."),
         DisplayName("Artifact Link"),
         ReadOnly(true),
         BrowsableAttribute(true)]
        [XmlIgnore]
        public AsmxFaultContractLink ArtifactLink
        {
            get
            {
                if (this.ModelElement != null &&
                    this.ModelElement.DataContractModel != null)
                {
                    return ArtifactLinkFactory.CreateInstance<AsmxFaultContractLink>(
                                    (ModelElement)this.ModelElement,
                                    this.ModelElement.DataContractModel.ProjectMappingTable);
                }
                return null;
            }
        }

		private bool orderParts;

		[Category(DataContractAsmxExtensionProvider.ExtensionProviderPropertyCategory),
        Description("Determines if the generated source code supports ordered message parts."),
         DisplayName("Order Parts"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("OrderParts")]
		public bool OrderParts
		{
			get { return orderParts; }
			set { orderParts = value; }
		}
        #endregion
	}
}