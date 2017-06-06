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
using System.Collections;
using System.Runtime.Serialization;
using Microsoft.Practices.Modeling.Serialization;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Tests
{
	/// <summary>
	/// Summary description for GenericSerializerFixture
	/// </summary>
	[TestClass]
	public class GenericSerializerFixture
	{
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GlobalCache.Off();
        }

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotSerializeWithNullFirstParameter()
		{
			GenericSerializer.Serialize<SerializableType>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotSerializeWithBothParameterNull()
		{
			GenericSerializer.Serialize<SerializableType>(null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotSerializeWithFirstParameterNullAndSecondParameterValid()
		{
			Type[] types = { };
			GenericSerializer.Serialize<SerializableType>(null, types);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotDeSerializeWithNullFirstParameter()
		{
			string dummy = null;
			GenericSerializer.Deserialize<SerializableType>(dummy);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotDeSerializeWithBothParameterNull()
		{
			string dummy = null;
			Type[] types = null;
            GenericSerializer.Deserialize<SerializableType>(dummy, types);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldNotDeSerializeWithFirstParameterNullAndSecondParameterValid()
		{
			string dummy = null;
			Type[] types = { };
            GenericSerializer.Deserialize<SerializableType>(dummy, types);
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void ShouldNotDeSerializeWithInvalidParameters4()
		{
			string dummy = "dummy";
			Type[] types = null;
            GenericSerializer.Deserialize<SerializableType>(dummy, types);
		}

		[TestMethod]
		public void ShouldSerializeASerializableType()
		{
			SerializableType type = new SerializableType();
			type.Field = 1;

			string typeRepresentation = GenericSerializer.Serialize<SerializableType>(type);

			Assert.IsTrue(!String.IsNullOrEmpty(typeRepresentation), "Serialization failed");
		}

		[TestMethod]
		[ExpectedException(typeof(SerializationException))]
		public void ShouldNotSerializeANotSerializableType()
		{
			NotSerializableType1 type = new NotSerializableType1();
			type.Field = new Hashtable();

			GenericSerializer.Serialize<NotSerializableType1>(type);
		}

		[TestMethod]
		public void ShouldSerializeANotSerializableTypeAsBase64()
		{
			NotSerializableType2 type = new NotSerializableType2();
			type.Field = new Hashtable();

			string typeRepresentation = GenericSerializer.Serialize<NotSerializableType2>(type);
			Assert.IsTrue(!String.IsNullOrEmpty(typeRepresentation), "Serialization failed");
		}

		[TestMethod]
		public void ShouldSerializeASerializableTypeWithOtherReferences()
		{
			Type[] types = { typeof(SerializableType1) };
			SerializableType2 type = new SerializableType2();
			SerializableType1 type1 = new SerializableType1();
			type1.Field1 = 1;
			type.Field = type1;

			string typeRepresentation = GenericSerializer.Serialize<SerializableType2>(type, types);
			Assert.IsTrue(!String.IsNullOrEmpty(typeRepresentation), "Invalid serialization");
		}

		[TestMethod]
		public void ShouldDeSerializeASerializableType()
		{
			SerializableType type = new SerializableType();
			type.Field = 1;
			string typeRepresentation = GenericSerializer.Serialize<SerializableType>(type);

			SerializableType type1 = GenericSerializer.Deserialize<SerializableType>(typeRepresentation);

			Assert.AreEqual(type.Field, type1.Field, "Not Equal");
		}

		[TestMethod]
		public void ShouldDeSerializeASerializableTypeWithOtherReferences()
		{
			Type[] types = { typeof(SerializableType1) };
			SerializableType2 type = new SerializableType2();
			SerializableType1 type1 = new SerializableType1();
			type1.Field1 = 1;
			type.Field = type1;
			string typeRepresentation = GenericSerializer.Serialize<SerializableType2>(type, types);

            SerializableType2 type3 = GenericSerializer.Deserialize<SerializableType2>(typeRepresentation, types);

			Assert.AreEqual(type.Field.GetType(), type3.Field.GetType(), "Not Equal");
		}

		[TestMethod]
		public void ShouldDeSerializeANotSerializableType()
		{
			NotSerializableType2 type = new NotSerializableType2();
			type.Field = new Hashtable();
			string typeRepresentation = GenericSerializer.Serialize<NotSerializableType2>(type);

			NotSerializableType2 type1 = GenericSerializer.Deserialize<NotSerializableType2>(typeRepresentation);

			Assert.IsNotNull(type1);
			Assert.AreEqual(type.Field.ToString(), type1.Field.ToString(), "Not Equal");
		}

		#region Test types

		[Serializable]
		public class SerializableType
		{
			private int field;

			public int Field
			{
				get { return field; }
				set { field = value; }
			}
		}

		[Serializable]
		public class SerializableType2
		{
			private object field;

			public object Field
			{
				get { return field; }
				set { field = value; }
			}
		}

		[Serializable]
		public class SerializableType1
		{
			private int field1;

			public int Field1
			{
				get { return field1; }
				set { field1 = value; }
			}
		}

		public class NotSerializableType1
		{
			private Hashtable field;

			public Hashtable Field
			{
				get { return field; }
				set { field = value; }
			}
		}

		[Serializable]
		public class NotSerializableType2
		{
			private Hashtable field;

			public Hashtable Field
			{
				get { return field; }
				set { field = value; }
			}
		}

		#endregion
	}
}