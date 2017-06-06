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
using System.ComponentModel;
using System.Collections;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.Helpers;
using Microsoft.Practices.Modeling.Serialization;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.Modeling.Common.Logging;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Design.Converters
{
	/// <summary>
	/// TypeConverter used on the DSL Serialization process for the ObjectExtenderContainer type.
	/// </summary>
	public sealed class ObjectExtenderContainerConverter : TypeConverter
	{
		#region Fields

		IServiceProvider serviceProvider;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectExtenderContainerConverter"/> class.
		/// </summary>
		public ObjectExtenderContainerConverter()
		{
            this.serviceProvider = RuntimeHelper.ServiceProvider;
		}

        /// <summary>
		/// Initializes a new instance of the <see cref="ObjectExtenderContainerConverter"/> class.
		/// </summary>
        public ObjectExtenderContainerConverter(IServiceProvider serviceProvider)
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
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"></see> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
		/// <returns>
		/// An <see cref="T:System.Object"></see> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
            ObjectExtenderContainer container = new ObjectExtenderContainer();
            string convertValue = value as string;
            if (!string.IsNullOrEmpty(convertValue))
            {
                try
                {
                    //We need to obtain all posible referenced types for the XMLSerializer (XMLInclude)
                    IList<Type> types = GetExtraTypesFromProviders(context);
                    container = GenericSerializer.Deserialize<ObjectExtenderContainer>(convertValue, types);
                }
                catch (Exception ex)
                {
                    Logger.Write(ex);
                }
            }
            return container;
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
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
            ObjectExtenderContainer objectExtenderContainer = value as ObjectExtenderContainer;
            if (destinationType == typeof(string)
                && objectExtenderContainer != null)
            {                
                //We need to obtain all posible referenced types for the XMLSerializer (XMLInclude)
                IList<Type> types = GetTypesFromContainer(objectExtenderContainer);
                return GenericSerializer.Serialize<ObjectExtenderContainer>(value, types);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        } 

		#endregion

		#region Private Implementation

        private IList<Type> GetTypesFromContainer(ObjectExtenderContainer objectExtenderContainer)
        {
            Guard.ArgumentNotNull(objectExtenderContainer, "objectExtenderContainer");

            IList<Type> types = new List<Type>() { typeof(System.ComponentModel.BindingList<string>) };

            foreach (object objectExtender in objectExtenderContainer.ObjectExtenders)
            {
                types.Add(objectExtender.GetType());
            }

            return types;
        }

		private IList<Type> GetExtraTypesFromProviders(ITypeDescriptorContext context)
		{
            IList<Type> types;

			if(context == null)
			{
				types = ServiceHelper.GetExtensionProviderService(this.serviceProvider).ObjectExtenderTypes;
			}
			else
			{
				types = ServiceHelper.GetExtensionProviderService(context as IServiceProvider).ObjectExtenderTypes;
			}

            if (!types.Contains(typeof(System.ComponentModel.BindingList<string>)))
            {
                types.Add(typeof(System.ComponentModel.BindingList<string>));
            }

            return types;
		} 

		#endregion
	}
}