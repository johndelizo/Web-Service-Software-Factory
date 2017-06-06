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
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;

namespace Microsoft.Practices.Modeling.ExtensionProvider.TypeDescription
{
	public sealed class ExtendedTypeDescriptionProvider : ElementTypeDescriptionProvider
	{
		#region Protected Implementation
		/// <summary>
		/// Creates the type descriptor.
		/// </summary>
		/// <param name="parent">The parent type descriptor.</param>
		/// <param name="element">The model element.</param>
		/// <returns></returns>
		protected override ElementTypeDescriptor CreateTypeDescriptor(ICustomTypeDescriptor parent, ModelElement element)
		{
			if(element is IExtensibleObject)
			{
				return new ExtendedTypeDescriptor(element as IExtensibleObject);
			}

			return null;
		} 
		#endregion
	}
}