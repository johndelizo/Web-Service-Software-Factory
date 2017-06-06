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
using System.Xml.Serialization;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Configuration
{
	[Serializable]
	[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/pag/project-mapping")]
	[XmlRoot(Namespace = "http://schemas.microsoft.com/pag/project-mapping", IsNullable = false)]
	public sealed class Role
	{
		private string name;

		[XmlAttribute("Name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public Role()
			: this(string.Empty)
		{

		}

		public Role(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// String representation of the role.
		/// </summary>
		/// <remarks>
		/// Added to make debugging output more readable.
		/// </remarks>
		/// <returns></returns>
		public new string ToString()
		{
			return RoleToString(this);
		}

		public static string RoleToString(Role role)
		{
			Guard.ArgumentNotNull(role, "role");

			return role.Name;
		}
	}
}