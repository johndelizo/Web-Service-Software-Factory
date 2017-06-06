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
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{	
    [ValidationState(ValidationState.Enabled)]
	public partial class DataContractBase : IArtifactLinkContainer
	{
		#region IArtifactLinkContainer Members

		private bool visited;

		/// <summary>
		/// Gets the artifact links.
		/// </summary>
		/// <value>The artifact links.</value>
		[Browsable(false)]
		public ICollection<IArtifactLink> ArtifactLinks
		{
			get
			{
				List<IArtifactLink> links = new List<IArtifactLink>();

				// if we've been here then yield
				if (visited)
				{
					return links;
				}

				if (ObjectExtender is IArtifactLinkContainer)
				{
					IArtifactLinkContainer container = (IArtifactLinkContainer)ObjectExtender;
					links.AddRange(container.ArtifactLinks);
				}

				try
				{
					// mark this class as visited to avoid an infinite loop while gathering internal ArtifactLinks
					visited = true;
					foreach (DataContractBase child in DataContractElements)
					{
						links.AddRange(child.ArtifactLinks);
					}
				}
				finally
				{
					// we are done here so clean up the flag
					visited = false;
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

        #endregion
	}
}
