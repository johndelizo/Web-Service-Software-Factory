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
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.UnitTestLibrary
{
	[ExtensionProvider("02568D23-44A9-4f09-925E-91A6FF7742F3", "ABC 123", "TestableExtensionProvider", null)]
	public class MockExtensionProvider : ExtensionProviderBase
	{
		IList<Type> list = null;

		public MockExtensionProvider()
		{
		}

		public MockExtensionProvider(Type serializableObjectType)
		{
			list = new Collection<Type>();
			list.Add(serializableObjectType);
		}

		public override IList<Type> ObjectExtenders
		{
			get
			{
				return list;
			}
		}
	}
}
