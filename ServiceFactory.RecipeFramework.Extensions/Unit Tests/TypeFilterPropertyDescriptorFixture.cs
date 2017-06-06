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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors.TypeBrowser;
using System.ComponentModel;
using System.Workflow.ComponentModel.Design;

namespace RecipeFramework.Extensions.Tests
{
	/// <summary>
	/// Tests TypeFilterPropertyDescriptor
	/// </summary>
	[TestClass]
	public class TypeFilterPropertyDescriptorFixture
	{
		public TypeFilterPropertyDescriptorFixture()
		{
		}

		PropertyDescriptor testPropertyDescriptor = TypeDescriptor.GetProperties(String.Empty)[0];


		[TestMethod]
		public void CanFilterDelegates()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			bool canFilter = descriptor.CanFilterType(typeof(System.String), true);

			Assert.IsTrue(filterProvider.CanFilterTypeInvoked, "CanFilterType not invoked");
		}

		[TestMethod]
		public void FilterDescriptionDelegates()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			string description = descriptor.FilterDescription;

			Assert.IsTrue(filterProvider.FilterDescriptionInvoked, "FilterDescription not invoked");
		}

		
		[TestMethod()]
		public void CanResetValueDelegates()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			descriptor.CanResetValue(null);
			Assert.IsTrue(propertyDescriptor.canResetValueInvoked, "ResetValue not delegated");
		}

		[TestMethod()]
		public void ComponentTypeDelegates()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			Type type = descriptor.ComponentType;

			Assert.IsTrue(propertyDescriptor.getComponentTypeInvoked, "ComponentType not delegated");
		}

		[TestMethod()]
		public void IsReadOnlyDelegated()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			bool isReadOnly = descriptor.IsReadOnly;
			Assert.IsTrue(propertyDescriptor.isReadOnlyInvoked, "IsReadOnly not delegated");
		}

		[TestMethod()]
		public void GetValueDelegated()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			descriptor.GetValue(null);
			Assert.IsTrue(propertyDescriptor.getValueInvoked, "GetValue not delegated");
		}

		[TestMethod()]
		public void PropertyTypeDelegated()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			Type type = descriptor.PropertyType;
			Assert.IsTrue(propertyDescriptor.propertyTypeInvoked, "PropertyType not delegated");
		}

		[TestMethod()]
		public void ResetValueDelegated()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			descriptor.ResetValue(null);
			Assert.IsTrue(propertyDescriptor.resetValueInvoked, "ResetValue not delegated");
		}

		[TestMethod()]
		public void SetValueDelegated()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			descriptor.SetValue(null, null);
			Assert.IsTrue(propertyDescriptor.setValueInvoked, "SetValue not delegated");
		}
		
		[TestMethod()]
		public void ShouldSerializeValueDelegated()
		{
			MockPropertyDescriptor propertyDescriptor = new MockPropertyDescriptor(testPropertyDescriptor);
			MockTypeFilterProvider filterProvider = new MockTypeFilterProvider();

			TypeFilterPropertyDescriptor descriptor = new TypeFilterPropertyDescriptor(propertyDescriptor, filterProvider);

			descriptor.ShouldSerializeValue(null);

			Assert.IsTrue(propertyDescriptor.shouldSerializeValueInvoked, "ShouldSerializeValue not delegated");
		}



		#region mocks
		private class MockTypeFilterProvider : ITypeFilterProvider
		{
			private bool canFilterTypeInvoked = false;

			public bool CanFilterTypeInvoked
			{
				get { return canFilterTypeInvoked; }
				set { canFilterTypeInvoked = value; }
			}
			private bool filterDescriptionInvoked = false;

			public bool FilterDescriptionInvoked
			{
				get { return filterDescriptionInvoked; }
				set { filterDescriptionInvoked = value; }
			}

			#region ITypeFilterProvider Members

			public bool CanFilterType(Type type, bool throwOnError)
			{
				canFilterTypeInvoked = true;
				return true;
			}

			public string FilterDescription
			{
				get
				{
					filterDescriptionInvoked = true;
					return string.Empty;
				}
			}

			#endregion
		}

		private class MockPropertyDescriptor : System.ComponentModel.PropertyDescriptor
		{
			public MockPropertyDescriptor(MemberDescriptor descr)
				: base(descr)
			{
			}

			public bool canResetValueInvoked = false;
			
			public override bool CanResetValue(object component)
			{
				canResetValueInvoked = true;
				return true;
			}


			public bool getComponentTypeInvoked = false;
			public override Type ComponentType
			{
				get { getComponentTypeInvoked = true; return null; }
			}

			public bool getValueInvoked = false;
			public override object GetValue(object component)
			{
				getValueInvoked = true;
				return null;
			}

			public bool isReadOnlyInvoked = false;
			public override bool IsReadOnly
			{
				get { return isReadOnlyInvoked = true;  }
			}

			public bool propertyTypeInvoked = false;
			public override Type PropertyType
			{
				get 
				{ 
					propertyTypeInvoked = true; 
					return null; 
				}
			}

			public bool resetValueInvoked = false;
			public override void ResetValue(object component)
			{
				resetValueInvoked = true;
			}

			public bool setValueInvoked = false;
			public override void SetValue(object component, object value)
			{
				setValueInvoked = true;
			}

			public bool shouldSerializeValueInvoked = false;
			public override bool ShouldSerializeValue(object component)
			{
				return shouldSerializeValueInvoked = true;
			}
		}

		#endregion

	}
}
