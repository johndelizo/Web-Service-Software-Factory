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
using System.IO;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies
{
	public class XmlSchemaResourceResolver : IResourceResolver
	{
		private ModelElement modelElement;

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSchemaResourceResolver"/> class.
		/// </summary>
		/// <param name="link">The link.</param>
		public XmlSchemaResourceResolver(IArtifactLink link)
		{
			Guard.ArgumentNotNull(link, "link");
			ArtifactLink alink = link as ArtifactLink;
			Guard.ArgumentNotNull(alink, "ArtifactLink");

			this.modelElement = alink.ModelElement;
		}

		#region IResourceResolver Members

		/// <summary>
		/// Gets the resource path.
		/// </summary>
		/// <param name="resourceItem">The resource item.</param>
		/// <returns></returns>
		public string GetResourcePath(string resourceItem)
		{
			Guard.ArgumentNotNullOrEmptyString(resourceItem, "resourceItem");

			IVsSolution solution = GetService<IVsSolution, SVsSolution>();
			HierarchyNode rootNode = new HierarchyNode(solution);
			HierarchyNode file = rootNode.RecursiveFindByName(resourceItem);
			return file != null ? file.Path : GetFallbackResourcePath(resourceItem);
		}

		/// <summary>
		/// Gets the resource.
		/// </summary>
		/// <param name="resourceItem">The resource item.</param>
		/// <returns></returns>
		public string GetResource(string resourceItem)
		{
			throw new NotImplementedException();
		}

		private TInterface GetService<TInterface, TImpl>()
		{
			return (TInterface)this.modelElement.Store.GetService(typeof(TImpl));
		}

		private string GetFallbackResourcePath(string resourceItem)
		{
			string path = Path.GetDirectoryName(GetType().Assembly.Location);
			return Path.Combine(path, resourceItem);
		}

		#endregion
	}
}
