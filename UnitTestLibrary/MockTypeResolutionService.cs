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
using System.Reflection;
using System.Text;
using System.ComponentModel.Design;

namespace Microsoft.Practices.UnitTestLibrary
{
	public class MockTypeResolutionService : ITypeResolutionService
	{
		public Assembly GetAssembly(AssemblyName name, bool throwOnError)
		{
			throw new NotImplementedException();
		}

		public Assembly GetAssembly(AssemblyName name)
		{
			throw new NotImplementedException();
		}

		public string GetPathOfAssembly(AssemblyName name)
		{
			throw new NotImplementedException();
		}

		public Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			return Type.GetType(name, throwOnError, ignoreCase);
		}

		public Type GetType(string name, bool throwOnError)
		{
			return Type.GetType(name, throwOnError);
		}

		public Type GetType(string name)
		{
			return Type.GetType(name);
		}

		public void ReferenceAssembly(AssemblyName name)
		{
			throw new NotImplementedException();
		}
	}
}
