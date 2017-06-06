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
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using System.Collections;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Extension
{
	/// <summary>
	/// Interface that needs to be implemented on a object to be extended
	/// </summary>
	[CLSCompliant(true)]
	public interface IExtensibleObject
	{
		/// <summary>
		/// Gets or sets the object extender.
		/// </summary>
		/// <value>The object extender.</value>
		object ObjectExtender { get; set; }

		/// <summary>
		/// Gets or sets the object extender container.
		/// </summary>
		/// <value>The object extender container.</value>
		ObjectExtenderContainer ObjectExtenderContainer { get;set; }

		/// <summary>
		/// Gets the model element.
		/// </summary>
		/// <value>The model element.</value>
		ModelElement ModelElement { get; }

		/// <summary>
		/// Gets the extension provider.
		/// </summary>
		/// <value>The extension provider.</value>
		IExtensionProvider ExtensionProvider { get;}
	}
}