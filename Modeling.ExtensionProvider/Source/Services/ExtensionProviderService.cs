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
using Microsoft.Practices.Modeling.ExtensionProvider.Services;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions;
using Microsoft.Practices.VisualStudio.Helper;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Services
{
	public class ExtensionProviderService : IExtensionProviderService
	{
		IList<Type> extensionProviderCache = null;
		List<IExtensionProvider> extensionProviders = null;
		List<Type> objectExtenderTypes = null;

		public ExtensionProviderService() 
		{
			extensionProviderCache = AssemblyLoader.GetTypesByInterface(typeof(IExtensionProvider));
		}

		/// <summary>
		/// Construct an ExtensionProviderService providing the type cache.
		/// </summary>
		/// <param name="typeCache"></param>
		/// <remarks>
		/// Primarily for unit testing.
		/// </remarks>
		public ExtensionProviderService(IList<Type> typeCache)
		{
			extensionProviderCache = ReflectionHelper.GetTypesByInterface(typeCache, typeof(IExtensionProvider));
		}

		#region IExtensionProviderService Members

		public IList<IExtensionProvider> ExtensionProviders
		{
			get
			{
				if (extensionProviders == null)
				{
					extensionProviders = GetLoadedExtensionProviders();
				}

				return extensionProviders;
			}
		}

		private List<IExtensionProvider> GetLoadedExtensionProviders()
		{
			List<IExtensionProvider> extensionProviders = new List<IExtensionProvider>();

			foreach (Type loadedType in extensionProviderCache)
			{
				extensionProviders.Add((IExtensionProvider)Activator.CreateInstance(loadedType));
			}

			return extensionProviders;
		}

		public IList<Type> ObjectExtenderTypes
		{
			get
			{
				if (objectExtenderTypes == null)
				{
					objectExtenderTypes = new List<Type>();

					foreach (IExtensionProvider extensionProvider in this.ExtensionProviders)
					{
						foreach (Type type in extensionProvider.ObjectExtenders)
						{
							objectExtenderTypes.Add(type);
						}
					}
				}

				return objectExtenderTypes;
			}
		}

		public IExtensionProvider GetExtensionProvider(Guid extensionProviderId)
		{
			foreach (IExtensionProvider extensionProvider in this.ExtensionProviders)
			{
				if (extensionProvider.Id == extensionProviderId)
				{
					return extensionProvider;
				}
			}

			return null;
		}

		#endregion
	}
}
