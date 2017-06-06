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

namespace Microsoft.Practices.VisualStudio.Helper.Design
{
	/// <summary>
	/// Inferface that must be implemented by filters of the <see cref="SolutionPickerEditor"/>
	/// </summary>
	public interface ISolutionPickerFilter
	{
		/// <summary>
		/// Filter method
		/// </summary>
		/// <param name="node">Wheather or not this node should be filter out</param>
		/// <returns>True if the node must be filter out, False if not</returns>
		bool Filter(HierarchyNode node);
	}
}
