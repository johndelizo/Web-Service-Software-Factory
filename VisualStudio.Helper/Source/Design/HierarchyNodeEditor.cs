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
using System.Security.Permissions;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.ComponentModel;

namespace Microsoft.Practices.VisualStudio.Helper.Design
{
	/// <summary>
	/// Editor for <see cref="HierarchyNode"/> objects
	/// </summary>
	public class HierarchyNodeEditor: SolutionPickerEditor
	{
		/// <summary>
		/// Edits an <see cref="HierarchyNode"/> object
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IVsSolution vsSolution = (IVsSolution)ServiceHelper.GetService(provider, typeof(IVsSolution), this);
			using (HierarchyNode root = new HierarchyNode(vsSolution))
			{
				return EditValue(provider, root, value as HierarchyNode);
			}
		}
	}
}
