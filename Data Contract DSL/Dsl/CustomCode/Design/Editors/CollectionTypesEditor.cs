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
using System.Windows.Forms;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	/// <summary>
	/// UITypeEditor for Visual Studio CollectionTypes Enum
	/// </summary>
	[CLSCompliant(false)]
	public class CollectionTypesEditor : ListEditor
	{
		/// <summary>
		/// Fills the values.
		/// </summary>
		protected override void FillValues()
		{
			this.AddKeyValueItems(CollectionTypes.Values);
		}
	}
}
