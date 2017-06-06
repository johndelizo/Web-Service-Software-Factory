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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Modeling.ExtensionProvider.Design.Converters;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.Modeling.ExtensionProvider.Services;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.Serialization;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Tests
{
	/// <summary>
	/// Summary description for ObjectExtenderContainerFixture
	/// </summary>
	[TestClass]
	public class ObjectExtenderContainerConverterFixture
	{
		public ObjectExtenderContainerConverterFixture()
		{
		}

		[TestMethod]
		public void ObjectExtenderContainerConverterCanConvertFromString()
		{
			MockServiceProvider mockServiceProvider = new MockServiceProvider();
			MockServiceProviderService mockService = new MockServiceProviderService();

			mockServiceProvider.AddService(typeof(IExtensionProviderService), mockService);

			ObjectExtenderContainerConverter converter = new ObjectExtenderContainerConverter(mockServiceProvider);

			Assert.IsTrue(converter.CanConvertFrom(typeof(string)), "Cannot convert from a string");
		}

		[TestMethod]
		public void ObjectExtenderContainerConverterConvertFromTypeCompliantWithXmlSerializer()
		{
			MockServiceProvider mockServiceProvider = new MockServiceProvider();
			Type[] types = { typeof(TestSerializableObject) };
			MockServiceProviderService mockService = new MockServiceProviderService();
			mockService.ObjectExtenderTypes.Add(types[0]);
			mockServiceProvider.AddService(typeof(IExtensionProviderService), mockService);

			ObjectExtenderContainerConverter converter = new ObjectExtenderContainerConverter(mockServiceProvider);

			TestSerializableObject testObject1 = new TestSerializableObject();
			testObject1.ValueOne = "TestDataOne";
			testObject1.ValueTwo = 33;
			string stringRepresentation;
			ObjectExtenderContainer container1 = new ObjectExtenderContainer();
			container1.ObjectExtenders.Add(testObject1);

			stringRepresentation = GenericSerializer.Serialize<ObjectExtenderContainer>(container1, types);

			ObjectExtenderContainer container2 = converter.ConvertFrom(stringRepresentation) as ObjectExtenderContainer;

			Assert.IsNotNull(container2, "container is null");
			Assert.IsNotNull(container2.ObjectExtenders, "ObjectExtenders is null");
			Assert.AreEqual(1, container2.ObjectExtenders.Count, "container.ObjectExtenders.Count != 1");

			TestSerializableObject testObject2 = container2.ObjectExtenders[0] as TestSerializableObject;
			Assert.AreEqual(testObject1.ValueOne, testObject2.ValueOne, "Not equal");
			Assert.AreEqual(testObject1.ValueTwo, testObject2.ValueTwo, "Not equal");
		}

		[TestMethod] 
		public void ObjectExtenderContainerConverterConvertToTypeCompliantWithXmlSerializer()
		{
			MockServiceProvider mockServiceProvider = new MockServiceProvider();
			MockServiceProviderService mockService = new MockServiceProviderService();

			mockServiceProvider.AddService(typeof(IExtensionProviderService), mockService);

			ObjectExtenderContainerConverter converter = new ObjectExtenderContainerConverter(mockServiceProvider);

			TestSerializableObject testObject1 = new TestSerializableObject();
			testObject1.ValueOne = "TestDataOne";
			testObject1.ValueTwo = 33;
			string stringRepresentation;
			Type[] types = { typeof(TestSerializableObject) };
			ObjectExtenderContainer container1 = new ObjectExtenderContainer();
			container1.ObjectExtenders.Add(testObject1);

			stringRepresentation = GenericSerializer.Serialize<ObjectExtenderContainer>(container1, types);

			string stringRepresentation2 = converter.ConvertTo(stringRepresentation, typeof(string)) as string;

			Assert.AreEqual(stringRepresentation, stringRepresentation2, "Not equal");
		}
		
		#region TestSerializableObject class
		
		[Serializable]
		public class TestSerializableObject
		{
			public string ValueOne;
			public int ValueTwo;
		}

		#endregion
	}
}