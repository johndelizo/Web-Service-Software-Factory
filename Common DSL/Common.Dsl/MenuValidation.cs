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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.Modeling;
using System.Collections;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
    public static class MenuValidation 
    {
        public static void ValidateFromElement(
            ModelingDocData docData,
            ICollection currentSelection,
            Func<Object, ModelElement> validationTargetProvider)
        {
            ThrowOnInvalidController(docData);
            HashSet<ModelElement> elementList = new HashSet<ModelElement>();
            FullDepthElementWalker elementWalker = new FullDepthElementWalker(
                new ModelElementVisitor(elementList), new EmbeddingReferenceVisitorFilter(), false);
            
            foreach (object selectedObject in currentSelection)
            {
                // Build list of elements embedded beneath the selected root.
                ModelElement element = validationTargetProvider(selectedObject);
                if (element != null && !elementList.Contains(element))
                {
                    elementWalker.DoTraverse(element);
                }
            }

            // Clear the previous messages
            IValidationControllerAccesor accesor = docData as IValidationControllerAccesor;
            accesor.Controller.ClearMessages();
            if (elementList.Count > 0)
            {
                accesor.Controller.Validate(elementList, ValidationCategories.Menu);
            }
            elementList.Clear();
        }

        public static void ValidateFromModel<T>(ModelingDocData docData) where T : ModelElement
        {
            ThrowOnInvalidController(docData);
            HashSet<ModelElement> elementList = new HashSet<ModelElement>();
            FullDepthElementWalker elementWalker = new FullDepthElementWalker(
                new ModelElementVisitor(elementList), new EmbeddingReferenceVisitorFilter(), false);

            T model = DomainModelHelper.GetElement<T>(docData.Store);
            if (model != null)
            {
                elementWalker.DoTraverse(model);
            }
            // Clear the previous messages
            IValidationControllerAccesor accesor = docData as IValidationControllerAccesor;
            accesor.Controller.ClearMessages();
            if (elementList.Count > 0)
            {
                accesor.Controller.Validate(elementList, ValidationCategories.Menu);
            }
            elementList.Clear();
        }

        private static void ThrowOnInvalidController(ModelingDocData docData)
        {
            if (docData == null ||
                docData.Store == null ||
                !typeof(IValidationControllerAccesor).IsAssignableFrom(docData.GetType()))
            {
                throw new InvalidOperationException(Properties.Resources.InvalidValidationController);
            }
        }
    }
}
