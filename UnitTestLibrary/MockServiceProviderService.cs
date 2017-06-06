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
using Microsoft.Practices.Modeling.ExtensionProvider.Services;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;

namespace Microsoft.Practices.UnitTestLibrary
{
	public class MockServiceProviderService : IExtensionProviderService
	{
		IList<IExtensionProvider> list = new List<IExtensionProvider>();
		List<Type> typeList = new List<Type>();

		public MockServiceProviderService() :
			this(new MockExtensionProvider(), null)
		{
		}

		public MockServiceProviderService(IExtensionProvider extensionProvider, Type serializableObjectType)
		{
			list.Add(extensionProvider);
			if (serializableObjectType != null)
			{
				typeList = new List<Type>();
				typeList.Add(serializableObjectType);
			}
		}

		public IList<IExtensionProvider> ExtensionProviders
		{
			get
			{
				return list;
			}
		}

		public IList<Type> ObjectExtenderTypes
		{
			get
			{
				return typeList;
			}
		}

		public IExtensionProvider GetExtensionProvider(Guid extensionProviderId)
		{
			if (extensionProviderId == new Guid("02568D23-44A9-4f09-925E-91A6FF7742F3"))
			{
				return new MockExtensionProvider();
			}

			return null;
		}
	}
}
