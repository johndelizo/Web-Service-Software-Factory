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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Microsoft.Practices.Modeling.CodeGeneration;

namespace Microsoft.Practices.UnitTestLibrary.Utilities
{
	public static class TypeAsserter
	{
		/// <summary>
		/// Asserts the field.
		/// </summary>
		/// <param name="fieldType">Type of the field.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="container">The container.</param>
		public static void AssertField(string fieldType, string fieldName, Type container)
		{
			AssertField(Type.GetType(fieldType), fieldName, container);
		}

		/// <summary>
		/// Asserts the field.
		/// </summary>
		/// <param name="fieldType">Type of the field.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="container">The container.</param>
		public static void AssertField(Type fieldType, string fieldName, Type container)
		{
			Assert.AreEqual<Type>(fieldType,
				container.GetField(Utility.ToCamelCase(fieldName),
				BindingFlags.Instance | BindingFlags.NonPublic).FieldType,
				"Field name {0} does not match with field type {1}", fieldName, fieldType.Name);
		}

		/// <summary>
		/// Asserts the field.
		/// </summary>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="container">The container.</param>
		public static void AssertExistPublicField(string fieldName, Type container)
		{
			Assert.IsNotNull(container.GetField(fieldName),
				"Field name {0} does not exists", fieldName);
		}

		/// <summary>
		/// Asserts the property.
		/// </summary>
		/// <param name="propertyType">Type of the property.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="container">The container.</param>
		public static void AssertProperty(string propertyType, string propertyName, Type container)
		{
			AssertProperty(Type.GetType(propertyType), propertyName, container);
		}

		/// <summary>
		/// Asserts the property.
		/// </summary>
		/// <param name="propertyType">Type of the property.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="container">The container.</param>
		public static void AssertProperty(Type propertyType, string propertyName, Type container)
		{
			Assert.AreEqual<Type>(propertyType,
				container.GetProperty(propertyName,
				BindingFlags.Instance | BindingFlags.Public).PropertyType,
				"Property {0} name does not match with property type {1}", propertyName, propertyType.Name);
		}

		/// <summary>
		/// Asserts the attribute.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		public static T AssertAttribute<T>(ICustomAttributeProvider attributeProvider)
			where T : Attribute
		{
			Assert.AreEqual<int>(1, attributeProvider.GetCustomAttributes(typeof(T), true).Length,
				"Attribute {0} not found in {1}", typeof(T).Name, attributeProvider.ToString());
			return attributeProvider.GetCustomAttributes(typeof(T), true).GetValue(0) as T;
		}

		/// <summary>
		/// Asserts the method.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static MethodInfo AssertMethod(string methodName, Type container)
		{
			MethodInfo method = container.GetMethod(methodName);
			Assert.IsNotNull(method, "Method not found: {0}", methodName);
			return method;
		}

		/// <summary>
		/// Asserts the interface.
		/// </summary>
		/// <param name="interfaceName">Name of the interface.</param>
		/// <param name="container">The container.</param>
		public static void AssertInterface(string interfaceName, Type container)
		{
			AssertInterface(interfaceName, container, 1);
		}

		/// <summary>
		/// Asserts the interface.
		/// </summary>
		/// <param name="interfaceName">Name of the interface.</param>
		/// <param name="container">The container.</param>
		/// <param name="occurs">The occurs.</param>
		public static void AssertInterface(string interfaceName, Type container, int occurs)
		{
			Type[] interfaces = container.FindInterfaces(delegate(Type typeObj, Object criteriaObj)
			{
				return typeObj.ToString() == criteriaObj.ToString();
			}, interfaceName);

			Assert.AreEqual<int>(occurs, interfaces.Length, "Interface {1} found: {0}", interfaceName, (occurs > 0 ? "not" : ""));
		}
	}
}
