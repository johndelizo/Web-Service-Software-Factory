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

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class AssemblyReferenceAttribute : Attribute
	{
		private string assemblyName;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyReferenceAttribute"/> class.
		/// </summary>
		/// <param name="assemblyName">The long form of the assembly name.</param>
		public AssemblyReferenceAttribute(string assemblyName)
		{
			this.assemblyName = assemblyName;
		}

		/// <summary>
		/// Gets or sets the the long form of the assembly name.
		/// </summary>
		/// <value>The long form of the assembly name.</value>
		public string AssemblyName
		{
			get { return assemblyName; }
		}
	}
}