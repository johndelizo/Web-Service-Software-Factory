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
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.Modeling.ExtensionProvider.Helpers;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Design.Converters
{
	/// <summary>
	/// TypeConverter used on the DSL Serialization process for the IExtensionProvider type.
	/// </summary>
	public sealed class ExtensionProviderConverter : ExpandableObjectConverter
	{
		#region Fields

		IServiceProvider serviceProvider; 

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionProviderConverter"/> class.
		/// </summary>
		public ExtensionProviderConverter()
		{
            this.serviceProvider = RuntimeHelper.ServiceProvider;
		}

        
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionProviderConverter"/> class.
		/// </summary>
        public ExtensionProviderConverter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

		#endregion

		#region Public Implementation
		/// <summary>
		/// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you want to convert from.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if(sourceType == typeof(string))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.</param>
		/// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type"></see> to convert the value parameter to.</param>
		/// <returns>
		/// An <see cref="T:System.Object"></see> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		/// <exception cref="T:System.ArgumentNullException">The destinationType parameter is null. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			// context may be null
			Guard.ArgumentNotNull(culture, "culture");
			// value may be null
			Guard.ArgumentNotNull(destinationType, "destinationType");

			if (value is IExtensionProvider)
			{
				return value.ToString();
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"></see> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
		/// <returns>
		/// An <see cref="T:System.Object"></see> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Guard.ArgumentNotNull(culture, "culture");
			Guard.ArgumentNotNull(value, "value");

			if (value is string)
			{
				string[] values = value.ToString().Split('|');

				IExtensionProvider extensionProvider = null;

				if(values.Length == 3)
				{
					if(context == null)
					{
                        if(this.serviceProvider == null) this.serviceProvider = RuntimeHelper.ServiceProvider;
						extensionProvider = ServiceHelper.GetExtensionProviderService(this.serviceProvider).GetExtensionProvider(new Guid(values[0]));
					}
					else
					{
						extensionProvider = ServiceHelper.GetExtensionProviderService(context as IServiceProvider).GetExtensionProvider(new Guid(values[0]));
					}
				}

				return extensionProvider;
			}

			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Returns whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exclusive list of possible values, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <returns>
		/// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exhaustive list of possible values; false if other values are possible.
		/// </returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <returns>
		/// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> should be called to find a common set of values the object supports; otherwise, false.
		/// </returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return false;
		} 

		#endregion
	}
}