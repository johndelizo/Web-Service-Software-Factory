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
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.CodeGeneration.Properties;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions;
using EnvDTE;
using Microsoft.Practices.VisualStudio.Helper;
using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.Modeling.CodeGeneration.Artifacts
{
	[Serializable]
	[DesignTimeVisible(true)]
	[DefaultValue("Empty")]
	[CLSCompliant(false)]
	public abstract class ArtifactLink : IArtifactLink, IModelReference
	{
		protected ArtifactLink()
		{
		}

		#region Properties

		[NonSerialized]
		EnvDTE.Project projectField;

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual EnvDTE.Project Project
		{
			get
			{
				return projectField;
			}
			set
			{
				projectField = value;
			}
		}

		private string itemNameField;
		/// <summary>
		/// Gets or sets the item name.
		/// </summary>
		/// <value>The item path.</value>
		[Browsable(true)]
		[ReadOnly(true)]
		public virtual string ItemName
		{
			get { return itemNameField; }
			set { itemNameField = value; }
		}

		[NonSerialized]
		string namespaceField;

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
		public virtual string Namespace
		{
			get
			{
				if (string.IsNullOrEmpty(namespaceField))
				{
					namespaceField = ProjectNode.GetEvaluatedProperty(this.Project, "RootNamespace");
				}
				return namespaceField;
			}
		}

		[NonSerialized]
		string defaultExtensionField = string.Empty;

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual string DefaultExtension
		{
			get
			{
				if (string.IsNullOrEmpty(defaultExtensionField))
				{
					if (Project != null)
					{						
						defaultExtensionField = "." + 
							CodeDomProvider.CreateProvider(GetLanguageName()).FileExtension;
					}
				}
				return defaultExtensionField;
			}
		}

		private string pathField = string.Empty;

		public virtual string Path
		{
			get { return pathField; }
			set { pathField = value; }
		}

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual string FullItemName
		{
			get	{return Namespace + "." + ItemName;	}
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			IArtifactLink other = obj as IArtifactLink;
			if (other == null)
			{
				return false;
			}
			return this.Container == other.Container &&
				   this.ItemPath == other.ItemPath &&
                   this.GetType().FullName == other.GetType().FullName;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			if (Container == Guid.Empty)
			{
				return Resources.UnmappedRole;
			}
			else
			{
				return Container.ToString("b", CultureInfo.InvariantCulture) + "," + ItemName;
			}
		}

		#endregion

		#region IArtifactLink Members

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual string ItemPath
		{
			get
			{
				return "." + System.IO.Path.Combine(
					System.IO.Path.DirectorySeparatorChar.ToString(),
					System.IO.Path.Combine(this.Path, System.IO.Path.ChangeExtension(ItemName, DefaultExtension)));
			}
		}

		[NonSerialized]
		Guid containerField = Guid.Empty;

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual Guid Container
		{
			get	{ return containerField; }
			set	{ containerField = value; }
		}

		[NonSerialized]
		IDictionary<string, object> data = null;

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual IDictionary<string, object> Data
		{
			get 
			{
				if (data == null)
				{
					data = new Dictionary<string, object>();
				}
				return data;
			}
		}

		#endregion

		#region IModelReference Members

		[NonSerialized]
		private ModelElement modelElement;

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual ModelElement ModelElement
		{
			get { return modelElement; }
			set { modelElement = value; }
		}

		#endregion

		#region Private Implementation

		private string GetLanguageName()
		{
			string name = string.Empty;
			string langId = ProjectNode.GetLanguageFromProject(this.Project);
			switch (langId)
			{
				case CodeModelLanguageConstants.vsCMLanguageCSharp:
					name = "cs";
					break;

				case CodeModelLanguageConstants.vsCMLanguageVB:
					name = "vb";
					break;

				case CodeModelLanguageConstants.vsCMLanguageVC:
					name = "cpp";
					break;
			}

			return name;
		}

		#endregion
	}
}