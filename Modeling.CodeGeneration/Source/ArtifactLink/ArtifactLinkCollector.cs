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
using System.Collections.ObjectModel;

namespace Microsoft.Practices.Modeling.CodeGeneration.Artifacts
{
	public class ArtifactLinkCollector : IArtifactLinkContainer
	{
        HashSet<IArtifactLink> artifacts;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArtifactLinkCollector"/> class.
		/// </summary>
		public ArtifactLinkCollector()
		{
            artifacts = new HashSet<IArtifactLink>();
		}

		/// <summary>
		/// Collects the specified links.
		/// </summary>
		/// <param name="links">The links.</param>
		public void Collect(IArtifactLinkContainer links)
		{
			if (links != null && 
                links.ArtifactLinks != null)
			{
				foreach (IArtifactLink link in links.ArtifactLinks)
				{
                    // This "Add()" will only add if not present (returns true if added)
					artifacts.Add(link);
				}
			}
		}

		#region IArtifactLinkContainer Members

		/// <summary>
		/// Gets the artifact links.
		/// </summary>
		/// <value>The artifact links.</value>
		public ICollection<IArtifactLink> ArtifactLinks
		{
			get { return artifacts; }
		}

		#endregion
	}
}
