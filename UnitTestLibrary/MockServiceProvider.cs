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

namespace Microsoft.Practices.UnitTestLibrary
{
	public class MockServiceProvider: IServiceProvider
	{
		Dictionary<Type, object> services;

		public MockServiceProvider()
		{
			services = new Dictionary<Type,object>();
		}

		public void AddService(Type serviceType,object serviceInstance)
		{
			services.Add(serviceType, serviceInstance);
		}

		#region IServiceProvider Members

		public object GetService(Type serviceType)
		{
			if (services.ContainsKey(serviceType))
			{
				return services[serviceType];
			}
			return null;
		}

		#endregion
	}
}
