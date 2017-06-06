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
using System.Drawing.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;

namespace Microsoft.Practices.Modeling.CodeGeneration.Artifacts.Design
{
	public sealed class ArtifactLinkConverter<TArtifactLink> : ExpandableObjectConverter
		where TArtifactLink : ArtifactLink, new()
	{
		#region Public Implementation
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
			if(value is TArtifactLink)
			{
				return value.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>
		/// Returns whether this converter can convert the object to the specified type, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
		{
			string itemPath = propertyValues["ItemPath"].ToString();
			TArtifactLink artifactLink = new TArtifactLink();
			artifactLink.ItemName = itemPath;
			return artifactLink;
		}

		/// <summary>
		/// Returns whether changing a value on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"></see> to create a new value, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <returns>
		/// true if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"></see> to create a new value; otherwise, false.
		/// </returns>
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// Returns whether the given value object is valid for this type and for the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="value">The <see cref="T:System.Object"></see> to test for validity.</param>
		/// <returns>
		/// true if the specified value is valid for this object; otherwise, false.
		/// </returns>
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			return base.IsValid(context, value);
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
			if(value is string)
			{
				string[] values = value.ToString().Split(',');
				TArtifactLink link = new TArtifactLink();
				if(values.Length == 1)
				{
					link.ItemName = values[0];
					return link;
				}
				return link;
			}
			return base.ConvertFrom(context, culture, value);
		}

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
		#endregion

		#region Private Implementation
		private bool IsValidProject(ITypeDescriptorContext context, Guid projectGuid)
		{
			IVsSolution vsSolution = GetService<IVsSolution, SVsSolution>(context);
			if(vsSolution != null)
			{
				IVsHierarchy projectHierarchy = null;
				int hResult = vsSolution.GetProjectOfGuid(ref projectGuid, out projectHierarchy);
				return ((hResult == 0) && (projectHierarchy != null));
			}
			return false;
		}

		private T GetService<T, TImpl>(ITypeDescriptorContext context)
		{
			if(context != null && context.Instance != null)
			{
				IServiceProvider serviceProvider = ((ShapeElement)context.Instance).Store as IServiceProvider;
				if(serviceProvider != null)
				{
					return (T)serviceProvider.GetService(typeof(TImpl));
				}
			}
			return default(T);
		}

		private T GetService<T>(ITypeDescriptorContext context)
		{
			return GetService<T, T>(context);
		} 
		#endregion
	}
}