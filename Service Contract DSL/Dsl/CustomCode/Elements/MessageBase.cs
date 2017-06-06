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
using System.Collections.Generic;
using Microsoft.Practices.Modeling.ExtensionProvider.TypeDescription;
using System.ComponentModel;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.ServiceFactory.Validation;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	[TypeDescriptionProvider(typeof(ExtendedTypeDescriptionProvider))]
	[ValidationState(ValidationState.Enabled)]
	public partial class MessageBase : IExtensibleObject, IValidatableElement, IArtifactLinkContainer
	{
		#region IExtensibleObject Members

		private object objectExtender;

		[Browsable(false)]
		public object ObjectExtender
		{
			get { return objectExtender; }
			set { objectExtender = value; }
		}

		[Browsable(false)]
		public ModelElement ModelElement
		{
			get { return this; }
		}

		[Browsable(false)]
		public IExtensionProvider ExtensionProvider
		{
			get
			{
				if (this.ServiceContractModel != null)
				{
					return this.ServiceContractModel.ImplementationTechnology;
				}
				return null;
			}
		}

		#endregion

		#region Validation support

		[ValidationMethod(ValidationCategories.Menu)]
		private void OnValidate(ValidationContext context)
		{
			ValidationEngine.Validate(ValidationElementState.FirstElement, context, this);
		}

		public void ContinueValidation(ValidationContext context)
		{
			ValidationEngine.Validate(ValidationElementState.LinkedElement, context, this);
		}

		#endregion

		#region IArtifactLinkContainer Members

		[Browsable(false)]
		public ICollection<IArtifactLink> ArtifactLinks
		{
			get
			{
				List<IArtifactLink> links = new List<IArtifactLink>();

				if (ObjectExtender is IArtifactLinkContainer)
				{
					IArtifactLinkContainer container = (IArtifactLinkContainer)ObjectExtender;
					links.AddRange(container.ArtifactLinks);
				}

				foreach (MessagePart part in this.MessageParts)
				{
					if (part.ObjectExtender is IArtifactLinkContainer)
					{
						IArtifactLinkContainer container = (IArtifactLinkContainer)part.ObjectExtender;
						links.AddRange(container.ArtifactLinks);
					}
				}

				return links;
			}
		}

		#endregion
	}
}