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
using Microsoft.VisualStudio.Shell;
using System.Globalization;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Metadata
{
	/// <summary>
	/// Attribute used to configure a ObjectExtender class
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class ObjectExtenderAttribute : Attribute
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectExtenderAttribute"/> class.
		/// </summary>
		/// <param name="objectToExtend">The object to extend.</param>
		public ObjectExtenderAttribute(Type objectToExtend)
		{
			this.objectToExtend = objectToExtend;
		} 
		#endregion

		#region Properties
		Type objectToExtend;

		/// <summary>
		/// Gets the object to extend.
		/// </summary>
		/// <value>The object to extend.</value>
		public Type ObjectToExtend
		{
			get { return objectToExtend; }
		} 
		#endregion
	}
}