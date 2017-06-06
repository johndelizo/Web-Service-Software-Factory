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
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	/// <summary>
	/// Class that validates a model element transversing the full hierarchy
	/// </summary>
	public static class ModelValidator
	{
		/// <summary>
		/// Validates the model element.
		/// </summary>
		/// <param name="modelElement">The model element.</param>
		/// <param name="validationController">The validation controller.</param>
		/// <returns></returns>
        public static bool ValidateModelElement(ModelElement modelElement, ValidationController validationController)
        {
            Guard.ArgumentNotNull(modelElement, "modelElement");
            Guard.ArgumentNotNull(validationController, "validationController");

            bool isValid = true;
            HashSet<ModelElement> elementList = new HashSet<ModelElement>();
            FullDepthElementWalker elementWalker =
                new FullDepthElementWalker(new ModelElementVisitor(elementList), new EmbeddingReferenceVisitorFilter(), false);

            elementWalker.DoTraverse(modelElement);
            validationController.ClearMessages();
            isValid = validationController.Validate(elementList, ValidationCategories.Menu);
            elementList.Clear();
            
            return isValid;
        }
	}
}