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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.VisualStudio.Helper.Design;
using System.Windows.Forms;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.Practices.UnitTestLibrary;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.VisualStudio.Helper.Tests
{
	internal class MockTypeDescriptorContext : ITypeDescriptorContext
	{
		IServiceProvider serviceProvider;

		public MockTypeDescriptorContext(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		#region ITypeDescriptorContext Members

		IContainer ITypeDescriptorContext.Container
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		object ITypeDescriptorContext.Instance
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		void ITypeDescriptorContext.OnComponentChanged()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		bool ITypeDescriptorContext.OnComponentChanging()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion

		#region IServiceProvider Members

		object IServiceProvider.GetService(Type serviceType)
		{
			return serviceProvider.GetService(serviceType);
		}

		#endregion
	}
}
