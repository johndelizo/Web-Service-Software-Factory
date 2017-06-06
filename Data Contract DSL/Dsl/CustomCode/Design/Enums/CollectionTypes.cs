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
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	/// <summary>
	/// Visual Studio CollectionTypes Enum
	/// </summary>	
	public static class CollectionTypes
	{
		public const string NoneKey = "None";
		public const string ArrayKey = "T[]";
		public const string CollectionKey = "Collection<T>";
		public const string ListKey = "List<T>";
		public const string DictionaryKey = "Dictionary<string,T>";

		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
		static CollectionTypes()
		{
			values = new Dictionary<string, Type>();
			values.Add(NoneKey, null);
			values.Add(ArrayKey, typeof(Collection<>).GetGenericArguments()[0].MakeArrayType());
			values.Add(CollectionKey, typeof(Collection<>));
			values.Add(ListKey, typeof(List<>));
			values.Add(DictionaryKey, typeof(Dictionary<,>));
		}

		#region Properties

		private static Dictionary<string, Type> values;

		/// <summary>
		/// Gets the values.
		/// </summary>
		/// <value>The values.</value>
		public static Dictionary<string, Type> Values
		{
			get { return values; }
		}

		#endregion
	}
}