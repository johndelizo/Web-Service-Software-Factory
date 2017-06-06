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
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies
{

	/// <summary>
	/// Wraps a TextTemplateAttribute decorated ArtifactLink to provide
	/// convenience access to Template information.
	/// </summary>
	public class TextTemplateArtifactLinkWrapper : IArtifactLink
	{
		IArtifactLink link = null;
		IResourceResolver resolver = new DefaultResourceResolver();
		TextTemplateAttribute[] textTemplateAttributes = null;

		public TextTemplateArtifactLinkWrapper(IArtifactLink artifactLink)
		{
			Guard.ArgumentNotNull(artifactLink, "artifactLink");
			object[] attributes = artifactLink.GetType().GetCustomAttributes(typeof(TextTemplateAttribute), true);

			if (attributes.Length == 0)
			{
				throw new InvalidOperationException(Properties.Resources.TextTemplateAttributeNotFound);
			}

			textTemplateAttributes = (TextTemplateAttribute[])attributes;
			link = artifactLink;
		}

		[CLSCompliant(false)]
		public IResourceResolver ResourceResolver
		{
			get { return resolver; }
			set { resolver = value; }
		}
	
		#region IArtifactLink Members

		public Guid Container
		{
			get { return link.Container; }
		}

		public string ItemPath
		{
			get { return link.ItemPath; }
		}

		public IDictionary<string, object> Data
		{
			get { return link.Data; }
		}

		#endregion

		public string GetTemplateRef(TextTemplateTargetLanguage textTemplateTargetLanguage)
		{
			foreach (TextTemplateAttribute attrib in textTemplateAttributes)
			{
				if (attrib.TargetLanguage == textTemplateTargetLanguage)
				{
					return ResourceResolver.GetResourcePath(attrib.TemplateName);
				}
			}

			return null;
		}

		public string GetTemplate(TextTemplateTargetLanguage textTemplateTargetLanguage)
		{
			string templateRef = GetTemplateRef(textTemplateTargetLanguage);

			if (templateRef == null)
			{
				return null;
			}
			else
			{
				return ResourceResolver.GetResource(templateRef);
			}
		}
	}
}
