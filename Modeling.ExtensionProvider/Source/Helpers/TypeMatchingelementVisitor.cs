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
using Microsoft.VisualStudio.Modeling;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Helpers
{

	/// <summary>
	/// Adds visited elements to an element list if it matches specified type.
	/// </summary>
	/// <typeparam name="T">Type to match</typeparam>
	public sealed class TypeMatchingElementVisitor<T> : IElementVisitor
	{
		IList<ModelElement> elementList = null;

		public TypeMatchingElementVisitor(IList<ModelElement> elementList)
		{
			this.elementList = elementList;
		}
		#region IElementVisitor Members

		public void EndTraverse(ElementWalker walker)
		{
		}

		public void StartTraverse(ElementWalker walker)
		{
		}

		public bool Visit(ElementWalker walker, ModelElement element)
		{
			if (element is T)
			{
				elementList.Add(element);
			}
			return true;
		}

		#endregion
	}
}
