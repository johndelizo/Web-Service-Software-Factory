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
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;

namespace Microsoft.Practices.Modeling.ExtensionProvider.TypeDescription
{
	internal class ExtendedTypeDescriptor : ElementTypeDescriptor
	{
		#region Fields
		IExtensibleObject instance; 
		#endregion

		#region Constructors
		internal ExtendedTypeDescriptor(IExtensibleObject extender)
			: base(extender.ModelElement)
		{
			this.instance = extender;
		} 
		#endregion

		#region Public Implementation
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection originalProperties = base.GetProperties(attributes);

			List<PropertyDescriptor> newProperties = new List<PropertyDescriptor>();

			foreach(PropertyDescriptor propertyDescriptor in originalProperties)
			{
				newProperties.Add(propertyDescriptor);
			}

			//Inject custom properties
			foreach(PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(instance.ObjectExtender))
			{
				newProperties.Add(new ExtendedPropertyDescriptor(propertyDescriptor, instance.ObjectExtender));
			}

			return new PropertyDescriptorCollection(newProperties.ToArray(), true);
		} 
		#endregion
	}
}