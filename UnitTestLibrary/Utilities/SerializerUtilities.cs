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
using System.Xml.Serialization;
using System.IO;

namespace Microsoft.Practices.UnitTestLibrary.Utilities
{
	public sealed class  SerializerUtilities
	{
		public static string Serialize<TypeToSerialize>(TypeToSerialize obj)
		{
			Type[] types = { };
			return Serialize<TypeToSerialize>(obj, types);
		}

		public static string Serialize<TypeToSerialize>(TypeToSerialize obj, Type[] types)
		{
			string text;
			XmlSerializer serializer = new XmlSerializer(typeof(TypeToSerialize), types);

			using (MemoryStream stream = new MemoryStream())
			{
				serializer.Serialize(stream, obj);
				stream.Position = 0;
				text = new StreamReader(stream).ReadToEnd();
			}

			return text;
		}

		public static TypeToSerialize DeSerialize<TypeToSerialize>(string representation)
		{
			Type[] types = { };
			return DeSerialize<TypeToSerialize>(representation, types);
		}

		public static TypeToSerialize DeSerialize<TypeToSerialize>(string representation, Type[] types)
		{
			TypeToSerialize obj;

			XmlSerializer serializer = new XmlSerializer(typeof(TypeToSerialize), types);

			using (MemoryStream stream = new MemoryStream())
			{
				StreamWriter writer = new StreamWriter(stream);
				writer.Write(representation);
				writer.Flush();
				stream.Position = 0;
				obj = (TypeToSerialize)serializer.Deserialize(stream);
			}

			return obj;
		}
	}
}
