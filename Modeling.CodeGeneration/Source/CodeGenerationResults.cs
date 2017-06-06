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
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	/// <summary>
	/// Represents a collection of code generated files paths and its contents.
	/// </summary>
	// Simply use the name, without the 'Dictionary' suffix (simplicity and avoid verbose long names).
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	[Serializable]
	[CLSCompliant(false)]
	public class CodeGenerationResults : Dictionary<string, string>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodeGenerationResults"/> class.
		/// </summary>
		public CodeGenerationResults() : base()
		{ }

		/// <summary>
		/// Adds the specified file path.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="content">The content.</param>
		public new void Add(string file, string content)
		{
			// avoid adding duplicate files or with empty content
			if (!this.ContainsKey(file) && 
				!string.IsNullOrEmpty(content))
			{
				base.Add(file, content);
			}
		}

		/// <summary>
		/// Determines whether the specified file is part of the collection.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns>
		/// 	<c>true</c> if the specified file contains file; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsFile(string file)
		{
			return base.ContainsKey(file);
		}

		/// <summary>
		/// Gets the generated file paths.
		/// </summary>
		/// <value>The files.</value>
		public KeyCollection Files
		{
			get { return this.Keys; }
		}

		/// <summary>
		/// Gets or sets the content in the specified file.
		/// </summary>
		/// <value></value>
		public new string this[string file]
		{
			get { return base[file]; }
			set { base[file] = value; }
		}

		#region ISerializable Members

		protected CodeGenerationResults(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion ISerializable Members
	}
}
