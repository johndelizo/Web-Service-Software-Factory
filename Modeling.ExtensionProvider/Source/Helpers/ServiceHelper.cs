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
using System.Globalization;
using Microsoft.Practices.Modeling.ExtensionProvider.Services;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Helpers
{
	internal static class ServiceHelper
	{
		#region Internal Implementation
		internal static IExtensionProviderService GetExtensionProviderService(IServiceProvider serviceProvider)
		{
			Guard.ArgumentNotNull(serviceProvider, "serviceProvider");

			IExtensionProviderService extensionProviderService =
				(IExtensionProviderService)serviceProvider.GetService(typeof(IExtensionProviderService));

			if(extensionProviderService == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.IExtensionProviderServiceError));
			}

			return extensionProviderService;
		} 
		#endregion
	}
}