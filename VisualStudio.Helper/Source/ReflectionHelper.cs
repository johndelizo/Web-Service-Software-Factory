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
using System.Reflection;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.VisualStudio.Helper
{
	public static class ReflectionHelper
	{
		/// <summary>
		/// Gets the attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="provider">The provider.</param>
		/// <param name="inherit">if set to <c>true</c> [inherit].</param>
		/// <returns></returns>
		public static T GetAttribute<T>(ICustomAttributeProvider provider, bool inherit)
		{
			T[] attribs = GetAttributes<T>(provider, inherit);
			if (attribs != null && attribs.Length == 1)
			{
				return attribs[0];
			}
			return default(T);
		}

		/// <summary>
		/// Gets the attributes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="provider">The provider.</param>
		/// <param name="inherit">if set to <c>true</c> [inherit].</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static T[] GetAttributes<T>(ICustomAttributeProvider provider, bool inherit)
		{
			object[] attribs = null;
			attribs = provider.GetCustomAttributes(typeof(T), inherit);
			if (attribs == null)
			{
				return new T[0];
			}
			return Array.ConvertAll<Object, T>(attribs, delegate(object attrib)
				{
					return (T)attrib;
				});
		}

		/// <summary>
		/// Gets the types by interface.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns></returns>
		public static IList<Type> GetTypesByInterface(IList<Type> types, Type interfaceType)
		{
			List<Type> matches = new List<Type>();

			List<Type> typeList = new List<Type>(types);

			matches.AddRange(typeList.FindAll(delegate(Type type)
				{
					try
					{
						return type.GetInterface(interfaceType.FullName) != null &&
                               !type.IsAbstract;
					}
					catch (ReflectionTypeLoadException) // Bad .NET DLL with missing dependencies
					{
						return false;
					}

				}));

			return matches;
		}

		/// <summary>
		/// Gets the provider.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static ICustomAttributeProvider GetProvider<T>(string name)
		{
			Guard.ArgumentNotNullOrEmptyString(name, "name");

			foreach (MemberInfo member in typeof(T).GetMembers())
			{
				if (member.Name.Equals(name, StringComparison.Ordinal))
				{
					return member as ICustomAttributeProvider;
				}
			}
			return null;
		}
	}
}
