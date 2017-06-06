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
	internal class ExtendedPropertyDescriptor : PropertyDescriptor
	{
		#region Fields
		object instance;
		PropertyDescriptor descriptor;
		ModelElement element;
		#endregion

		#region Constructors
		public ExtendedPropertyDescriptor(PropertyDescriptor descriptor, object component)
			: base(descriptor)
		{
			IObjectExtender objectExtender = component as IObjectExtender;

			if(objectExtender != null)
			{
				element = objectExtender.ModelElement;
			}

			this.descriptor = descriptor;
			this.instance = component;
		} 
		#endregion

		#region Public Implementation
		public override bool CanResetValue(object component)
		{
			return this.descriptor.CanResetValue(instance);
		}

		public override Type ComponentType
		{
			get { return this.descriptor.ComponentType; }
		}

		public override object GetValue(object component)
		{
			return this.descriptor.GetValue(instance);
		}

		public override bool IsReadOnly
		{
			get { return this.descriptor.IsReadOnly; }
		}

		public override Type PropertyType
		{
			get { return this.descriptor.PropertyType; }
		}

		public override void ResetValue(object component)
		{
			this.descriptor.ResetValue(instance);
			MarkAsDirty();
		}

		private void MarkAsDirty()
		{
			if(element != null)
			{
				using(Transaction tx = element.Store.TransactionManager.BeginTransaction())
				{
					tx.Commit();
				}
			}
		}

		public override void SetValue(object component, object value)
		{
			this.descriptor.SetValue(instance, value);
			MarkAsDirty();
		}

		public override bool ShouldSerializeValue(object component)
		{
			return this.descriptor.ShouldSerializeValue(instance);
		} 
		#endregion
	}
}