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
using Microsoft.Practices.Modeling.ExtensionProvider.TypeDescription;
using System.ComponentModel;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.Practices.ServiceFactory.Validation;
using System.Globalization;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	[TypeDescriptionProvider(typeof(ExtendedTypeDescriptionProvider))]
	[ValidationState(ValidationState.Enabled)]
	public partial class Operation : IExtensibleObject, IArtifactLinkContainer, IValidatableElement
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

		#region IArtifactLinkContainer Members

		public ICollection<IArtifactLink> ArtifactLinks
		{
			get
			{
				List<IArtifactLink> links = new List<IArtifactLink>();
				if (this.Request != null &&
					this.Request.ObjectExtender is IArtifactLinkContainer)
				{
					IArtifactLinkContainer requestLinks = (IArtifactLinkContainer)this.Request.ObjectExtender;
					if (requestLinks.ArtifactLinks != null)
					{
						links.AddRange(requestLinks.ArtifactLinks);
					}
				}

				if (this.Response != null &&
					this.Response.ObjectExtender is IArtifactLinkContainer)
				{
					IArtifactLinkContainer responseLinks = (IArtifactLinkContainer)this.Response.ObjectExtender;
					if (responseLinks.ArtifactLinks != null)
					{
						links.AddRange(responseLinks.ArtifactLinks);
					}
				}

				foreach (Fault fault in this.Faults)
				{
					if (fault.ObjectExtender is IArtifactLinkContainer)
					{
						IArtifactLinkContainer container = (IArtifactLinkContainer)fault.ObjectExtender;
						links.AddRange(container.ArtifactLinks);
					}
				} 
				
				return links;
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

		#region Helpers

		public static string BuildDefaultAction(Operation operation)
		{
			if (operation == null)
			{
				return string.Empty;
			}

			if (operation.ServiceContract != null)
			{
				// get uri namespace
				string ns = operation.ServiceContractModel != null ? operation.ServiceContractModel.Namespace :
																	 operation.ServiceContract.Namespace;
				string format = ns.Contains("/") ? 
					"{0}" + (ns.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? string.Empty : "/") + "{1}/{2}" 
					: "{0}.{1}.{2}";
				return string.Format(CultureInfo.InvariantCulture, format, ns, operation.ServiceContract.Name, operation.Name);
			}
			return operation.Name;
		}

		#endregion
	}
}