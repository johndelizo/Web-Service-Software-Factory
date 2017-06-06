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
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Services
{
	/// <summary>
	/// VS Service used to retrieve ExtensionProviders information
	/// </summary>
	[Guid("4D13EFD4-853C-489c-860F-F87CCA709A27")]
	[ComVisible(true)]
	public interface IExtensionProviderService
	{
		/// <summary>
		/// Gets all extension providers.
		/// </summary>
		/// <value>The extension providers.</value>
		IList<IExtensionProvider> ExtensionProviders
		{
			get;
		}

		/// <summary>
		/// Gets all object extender types for all extension providers.
		/// </summary>
		/// <value>The object extender types.</value>
		IList<Type> ObjectExtenderTypes
		{
			get;
		}

		/// <summary>
		/// Gets a particular extension provider.
		/// </summary>
		/// <param name="extensionProviderId">The extension provider id.</param>
		/// <returns></returns>
		IExtensionProvider GetExtensionProvider(Guid extensionProviderId);
	}
}