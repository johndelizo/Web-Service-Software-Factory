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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.Globalization;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Metadata
{
	/// <summary>
	/// Attribute used to configure a ExtensionProviderBase class.
	/// </summary>
	/// <remarks>
	/// Use in conjunction with the ExtensionProviderBase class which reads this attribute's properties.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	// FXCOP: False positive
	[SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
	public sealed class ExtensionProviderAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionProviderAttribute"/> class.
		/// </summary>
		/// <param name="extensionProviderId">The extension provider id.</param>
		/// <param name="extensionProviderName">Name of the extension provider.</param>
		/// <param name="extensionProviderDescription">The extension provider description.</param>

		public ExtensionProviderAttribute(string extensionProviderId, string extensionProviderName, string extensionProviderDescription, Type modelToExtend)
		{
			this.extensionProviderId = new Guid(extensionProviderId);
			this.extensionProviderName = extensionProviderName;
			this.extensionProviderDescription = extensionProviderDescription;
			this.modelToExtend = modelToExtend;
		} 

		#endregion

		#region Properties

		Guid extensionProviderId;

		/// <summary>
		/// Gets the extension provider id.
		/// </summary>
		/// <value>The extension provider id.</value>		
		public Guid ExtensionProviderId
		{
			get { return extensionProviderId; }
		}

		string extensionProviderName;

		/// <summary>
		/// Gets the name of the extension provider.
		/// </summary>
		/// <value>The name of the extension provider.</value>		
		public string ExtensionProviderName
		{
			get { return extensionProviderName; }
		}

		string extensionProviderDescription;

		/// <summary>
		/// Gets the extension provider description.
		/// </summary>
		/// <value>The extension provider description.</value>
		public string ExtensionProviderDescription
		{
			get { return extensionProviderDescription; }
		}

		private Type modelToExtend;

		/// <summary>
		/// Gets or sets the model to extend.
		/// </summary>
		/// <value>The model to extend.</value>
		public Type ModelToExtend
		{
			get { return modelToExtend; }
			set { modelToExtend = value; }
		}

		#endregion
	}
}