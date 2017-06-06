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
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Concurrent;
using Microsoft.VisualStudio.Shell.Design.Serialization;
using Microsoft.Practices.Modeling.Common;
using System.Runtime.Serialization;

namespace Microsoft.Practices.Modeling.Serialization
{
	/// <summary>
	/// Helper class for type serialization
	/// </summary>
	/// <remarks>
	/// Uses <see cref="XmlSerializer"/> and fallback to <see cref="BinaryFormatter"/> 
	/// if a type does not support XML serialization.
	/// </remarks>
	public static class GenericSerializer
	{
		#region Private Fields

		private static XmlSerializerFactory serializerFactory = new XmlSerializerFactory();

		#endregion

		#region Public Implementation
		/// <summary>
		/// Deserializes the specified data.
		/// </summary>
		/// <param name="data">The string representation of the type.</param>
		/// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T Deserialize<T>(string data)
		{			
			Guard.ArgumentNotNullOrEmptyString(data, "data");

            return Deserialize<T>(data, null);
		}

        /// <summary>
        /// Deserializes the specified types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="types">The types.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T Deserialize<T>(string data, ICollection<Type> types)
        {
            return Deserialize<T>(data, types, Encoding.UTF8);
        }

        /// <summary>
        /// Deserializes the specified types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="types">The types.</param>
        /// <param name="data">The string representation of the type.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T Deserialize<T>(string data, ICollection<Type> types, Encoding encoding)
		{
            Guard.ArgumentNotNullOrEmptyString(data, "data");

            if (Uri.IsHexDigit(data[0]))
            {
                // we have a binary formatter base64 encoded
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(data)))
                {
                    return (T)formatter.Deserialize(stream);
                }
            }

            // we have an xml serialized string
            XmlSerializer serializer = GetXmlSerializer(typeof(T), types);
            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(data)))
            {
                return (T)serializer.Deserialize(stream);
            }
		}

		/// <summary>
		/// Deserializes the specified file info.
		/// </summary>
		/// <param name="fileInfo">The file info.</param>
		/// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T Deserialize<T>(FileInfo fileInfo)
		{
			Guard.ArgumentNotNull(fileInfo, "fileInfo");

			Type[] types = { };
            return Deserialize<T>(fileInfo, types);
		}

		/// <summary>
		/// Deserializes the specified types.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <param name="fileInfo">The file info.</param>
		/// <returns></returns>
		// FXCOP: FileInfo is more appropriate here as fileInfo refers to a file not a directory.
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), 
         SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static T Deserialize<T>( FileInfo fileInfo, ICollection<Type> types)
		{
			Guard.ArgumentNotNull(types, "types");
			Guard.ArgumentNotNull(fileInfo, "fileInfo");

			XmlSerializer serializer = GetXmlSerializer(typeof(T), types);

			if(File.Exists(fileInfo.FullName))
			{
				using(StreamReader reader = new StreamReader(fileInfo.FullName))
				{
					return (T)serializer.Deserialize(reader);
				}
			}

			return default(T);
		}

		/// <summary>
		/// Serializes the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), 
         SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
		public static string Serialize<T>(object obj)
		{
			Guard.ArgumentNotNull(obj, "obj");
			return Serialize<T>(obj, null);
		}

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), 
         SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        public static string Serialize<T>(object obj, ICollection<Type> types)
        {
            return Serialize(obj, typeof(T), types, Encoding.UTF8);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="types">The types.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), 
         SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        public static string Serialize<T>(object obj, ICollection<Type> types, Encoding encoding)
		{
			return Serialize(obj, typeof(T), types, encoding);
		}

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fileInfo">The file info.</param>
        // FXCOP: FileInfo is more appropriate here as fileInfo refers to a file not a directory.
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        public static void Serialize<T>(object obj, FileInfo fileInfo, IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(obj, "obj");
            Guard.ArgumentNotNull(fileInfo, "fileInfo");

            Type[] types = { };
            Serialize<T>(obj, types, fileInfo, serviceProvider);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="types">The types.</param>
        /// <param name="fileInfo">The file info.</param>
        // FXCOP: FileInfo is more appropriate here as fileInfo refers to a file not a directory.
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        public static void Serialize<T>(object obj, ICollection<Type> types, FileInfo fileInfo, IServiceProvider serviceProvider)
        {
            Guard.ArgumentNotNull(obj, "obj");
            Guard.ArgumentNotNull(types, "types");
            Guard.ArgumentNotNull(fileInfo, "fileInfo");

            string text = Serialize<T>(obj, types);

            if (serviceProvider != null)
            {
                TFSHelper.CheckOutFile(serviceProvider, fileInfo.FullName);
                File.WriteAllText(fileInfo.FullName, text);
            }
            else
            {
                File.WriteAllText(fileInfo.FullName, text);
            }
        }

		/// <summary>
		/// Serializes the specified object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="objType">The type of the object to serailize.</param>
		/// <param name="types">The types.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        public static string Serialize(object obj, Type objType, ICollection<Type> types, Encoding encoding)
		{
			Guard.ArgumentNotNull(obj, "obj");
			string text;

			try
			{
				XmlSerializer serializer = GetXmlSerializer(objType, types);                
                
				using (MemoryStream stream = new MemoryStream())
				{
					serializer.Serialize(stream, obj);
					text = encoding.GetString(stream.ToArray());
				}
			}
			catch (InvalidOperationException ioe)
			{
                Trace.TraceWarning(Microsoft.Practices.Modeling.Common.Properties.Resources.XmlSerializationFailedSwitchToBinary);
                Trace.TraceWarning(ioe.InnerException == null ? ioe.Message : ioe.InnerException.Message);
				// if we could not use XML serialization, fallback to binary serialization
				BinaryFormatter formatter = new BinaryFormatter();
				using (MemoryStream stream = new MemoryStream())
				{
					formatter.Serialize(stream, obj);
					text = Convert.ToBase64String(stream.ToArray());
				}
			}

			return text;
		}

		public static string XmlEncodeValue(object value)
		{
			if (value == null ||
				value.Equals(string.Empty))
			{
				return string.Empty;
			}

			Type convertType = value.GetType();
			bool isDateTime = convertType.Equals(typeof(DateTime));
			// get 'ToString' method according to input Type
			MethodInfo toStringMethod = isDateTime ?
				typeof(XmlConvert).GetMethod("ToString", new Type[] { convertType, typeof(XmlDateTimeSerializationMode) }) :
				typeof(XmlConvert).GetMethod("ToString", new Type[] { convertType });

			if (toStringMethod != null)
			{
				return (string)toStringMethod.Invoke(null, isDateTime ? 
					new object[] { value, XmlDateTimeSerializationMode.RoundtripKind } :
					new object[] { value });
			}
			
			// fallback to string conversion.
			return Convert.ToString(value, CultureInfo.InvariantCulture);
		}

		public static string XmlEncodeName(string name)
		{
			return XmlConvert.EncodeName(name);
		}

		public static XmlSerializer CreateSerializer(Type type)
		{
            // try using cache first, otherwise fallback to new instance.
            return serializerFactory.CreateSerializer(type) ?? new XmlSerializer(type);
		}

		#endregion

		#region Private Implementation

        private static XmlSerializer GetXmlSerializer(Type type, ICollection<Type> extraTypes)
		{
            if (extraTypes == null || 
				extraTypes.Count == 0)
            {
                return CreateSerializer(type);
            }

            List<Type> etypes = new List<Type>(extraTypes);
            StringBuilder b = new StringBuilder();
            etypes.ForEach(t => b.Append(t.FullName));
            return GlobalCache.AddOrGetExisting<XmlSerializer>(b.ToString(), k =>
			{   
                return new XmlSerializer(type, etypes.ToArray());
			});
		}

		#endregion
	}
}