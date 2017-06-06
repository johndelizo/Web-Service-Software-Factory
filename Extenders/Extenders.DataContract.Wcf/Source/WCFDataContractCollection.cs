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
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using System.Drawing.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf.Design.Editors;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf
{
	[Serializable]
	[CLSCompliant(false)]
	[ObjectExtender(typeof(DataContractCollectionBase))]
	// FxCop: Our use of Enum refers to a MEL here.
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public class WCFDataContractCollection : ObjectExtender<DataContractCollectionBase>
	{
		#region fields

		private Type collectionType;
		
		#endregion

		#region Constructors

		public WCFDataContractCollection()
		{
		}

		#endregion

		#region Properties

		[Category(DataContractWcfExtensionProvider.ExtensionProviderPropertyCategory),
        Description("This value determines what kind of collection is generated in code."),
         DisplayName("Collection Type"),
         ReadOnly(false),
		 Browsable(true)]
		[System.ComponentModel.Editor(typeof(WCFCollectionTypesEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.TypeConverter))]
		[XmlIgnore]
		public Type CollectionType
		{
			get { return collectionType; }
			set { collectionType = value; }
		}

		// Property for XMLSerialization usage
		[Browsable(false)]
		[XmlElement("CollectionType")]
		public string CollectionTypeName
		{
			get
			{
				if (this.CollectionType != null)
				{
					return this.CollectionType.AssemblyQualifiedName;
				}
				return null;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.CollectionType = Type.GetType(value);
				}
			}
		}

		[Category(DataContractWcfExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies how the code will be generated."),
         DisplayName("Artifact Link"),
         ReadOnly(true),
		 Browsable(true)]
		[XmlIgnore]
		public DataContractCollectionLink ArtifactLink
		{
			get
			{
				if (this.ModelElement != null &&
					this.ModelElement.DataContractModel != null)
				{
					return ArtifactLinkFactory.CreateInstance<DataContractCollectionLink>(
							(ModelElement)this.ModelElement,
							this.ModelElement.DataContractModel.ProjectMappingTable);
				}
				return null;
			}
		}

		#endregion
	}
}