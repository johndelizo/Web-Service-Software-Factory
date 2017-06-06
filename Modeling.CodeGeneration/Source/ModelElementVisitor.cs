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
using Microsoft.VisualStudio.Modeling;
using System.Collections.Generic;

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	public sealed class ModelElementVisitor : IElementVisitor
	{
		private ICollection<ModelElement> elementList;

		/// <summary>
		/// Construct a ValidateCommandVisitor that adds elements to be validated to the specified list.
		/// </summary>
        public ModelElementVisitor(ICollection<ModelElement> elementList)
		{
			this.elementList = elementList;
		}

		/// <summary>
		/// Called when traversal begins. 
		/// </summary>
		public void StartTraverse(ElementWalker walker) { }

		/// <summary>
		/// Called when traversal ends. 
		/// </summary>
		public void EndTraverse(ElementWalker walker) { }

		/// <summary>
		/// Called for each element in the traversal.
		/// </summary>
		public bool Visit(ElementWalker walker, ModelElement element)
		{
			this.elementList.Add(element);
			return true;
		}
	}
}