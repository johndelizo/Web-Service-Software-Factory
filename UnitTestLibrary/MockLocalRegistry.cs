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
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.UnitTestLibrary
{
	public sealed class MockLocalRegistry : ILocalRegistry, ILocalRegistry3
	{
		private string registryRoot;

		public MockLocalRegistry()
		{
		}

		public MockLocalRegistry(string registryRoot)
		{
			this.registryRoot = registryRoot;
		}

		#region ILocalRegistry Members

		public int CreateInstance(Guid clsid, object punkOuter, ref Guid riid, uint dwFlags, out IntPtr ppvObj)
		{
			ppvObj = Marshal.GetIUnknownForObject(new object());
			return VSConstants.S_OK;
		}

		public int GetClassObjectOfClsid(ref Guid clsid, uint dwFlags, IntPtr lpReserved, ref Guid riid, out IntPtr ppvClassObject)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetTypeLibOfClsid(Guid clsid, out Microsoft.VisualStudio.OLE.Interop.ITypeLib pptLib)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		#region ILocalRegistry3 Members

		public int CreateManagedInstance(string codeBase, string assemblyName, string typeName, ref Guid riid, out IntPtr ppvObj)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetClassObjectOfClsid(ref Guid clsid, uint dwFlags, IntPtr lpReserved, ref Guid riid, IntPtr ppvClassObject)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetClassObjectOfManagedClass(string codeBase, string assemblyName, string typeName, ref Guid riid, out IntPtr ppvClassObject)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetLocalRegistryRoot(out string pbstrRoot)
		{
			pbstrRoot = this.registryRoot;
			return VSConstants.S_OK;
		}

		#endregion
	}
}
