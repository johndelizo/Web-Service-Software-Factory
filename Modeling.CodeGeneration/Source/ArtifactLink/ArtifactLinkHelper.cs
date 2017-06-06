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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Artifacts
{
	public static class ArtifactLinkHelper
	{
		[CLSCompliant(false)]
		public static ArtifactLink GetFirstArtifactLink(ModelElement modelElement)
		{
			Guard.ArgumentNotNull(modelElement, "modelElement");

			if(modelElement is IExtensibleObject)
			{
				object extender = ((IExtensibleObject)modelElement).ObjectExtender;

				if(extender != null)
				{
					foreach(PropertyInfo prop in extender.GetType().GetProperties())
					{
						if(typeof(IArtifactLink).IsAssignableFrom(prop.PropertyType))
						{
							return prop.GetValue(extender, null) as ArtifactLink;
						}
					}
				}
			}

			return null;
		}

		[CLSCompliant(false)]
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
		public static string DefaultNamespace(ModelElement element)
		{
			ArtifactLink link = GetFirstArtifactLink(element);
			if(link != null &&
				!string.IsNullOrEmpty(link.Namespace))
			{
				return string.Format(CultureInfo.InvariantCulture, "urn:{0}", link.Namespace.ToLowerInvariant());
			}
			return string.Empty;
		}
	}
}
