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

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public sealed class XmlSchemaCodeGenerationAttribute: Attribute
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSchemaCodeGenerationAttribute"/> class.
		/// </summary>
		/// <param name="useXmlSerializerImporter">The use XML serializer importer.</param>
		public XmlSchemaCodeGenerationAttribute(bool useXmlSerializerImporter)
		{
			this.useXmlSerializerImporter = useXmlSerializerImporter;
		} 
		#endregion

		#region Properties

		private bool useXmlSerializerImporter;

		public bool UseXmlSerializerImporter
		{
			get { return useXmlSerializerImporter; }
		}
	
		#endregion
	}
}