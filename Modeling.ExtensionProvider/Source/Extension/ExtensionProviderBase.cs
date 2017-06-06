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
using System.Reflection;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using System.ComponentModel;
using Microsoft.Practices.Modeling.ExtensionProvider.Design.Converters;
using System.Globalization;
using Microsoft.Practices.Modeling.ExtensionProvider.Helpers;
using Microsoft.Practices.Modeling.ExtensionProvider.Properties;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Extension
{
	/// <summary>
	/// Class that provides an extension mechanism for objects
	/// </summary>
	/// <remarks>
	/// This provides some base functionality for classes that need to implement IExtensionProvider. It should
	/// be used in conjunction with the ExtensionProviderAttribute, from which it reads values.
	/// </remarks>
	[CLSCompliant(true)]
	public abstract class ExtensionProviderBase : IExtensionProvider
	{
		private IList<Type> objectExtenderList;
		private ExtensionProviderAttribute extensionProviderAttribute = null;

		private ExtensionProviderAttribute Attribute
		{
			get 
			{
				if (extensionProviderAttribute == null)
					extensionProviderAttribute = ReflectionHelper.GetAttribute<ExtensionProviderAttribute>(this.GetType());
				if (extensionProviderAttribute == null)
					throw new InvalidOperationException(Resources.ExtensionProviderAttributeMissingMessage);
				return extensionProviderAttribute; 
			}
		}

		#region Properties

		/// <summary>
		/// Gets the id.
		/// </summary>
		/// <value>The id.</value>
		[Browsable(true)]
		public Guid Id
		{
			get { return Attribute.ExtensionProviderId; }
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		[Browsable(true)]
		public string Name
		{
			get { return Attribute.ExtensionProviderName; }
		}

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value>The description.</value>
		[Browsable(true)]
		public string Description
		{
			get { return Attribute.ExtensionProviderDescription; }
		}

		/// <summary>
		/// Gets the object extenders.
		/// </summary>
		/// <value>The object extenders.</value>
		[Browsable(false)]
		public abstract IList<Type> ObjectExtenders
		{
			get;
		}

		protected IList<Type> ObjectExtenderList
		{
			get { return objectExtenderList; }
			set { objectExtenderList = value; }
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentUICulture, "{0}|{1}|{2}",
					this.Id.ToString("b", CultureInfo.InvariantCulture),
					this.Name,
					this.Description);
		} 
	}
}