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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.Dsl.Integration.Helpers
{
    /// <summary>
    /// Represents the method that loads the domain model of type T from a serialized file.
    /// </summary>
    public delegate T ModelLoader<T>(string modelPath, Store store, SerializationResult result) where T : ModelElement;

	/// <summary>
	/// Domain model helper class.
	/// </summary>
	public static class DomainModelHelper
	{
		/// <summary>
		/// Gets the surface area.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static bool IsDiagramSelected(IServiceProvider provider)
		{
			DiagramDocView docView = DesignerHelper.GetDiagramDocView(provider);

			if(docView != null)
			{
				if(docView.SelectionCount == 1)
				{
					foreach(object component in docView.GetSelectedComponents())
					{
						return component is Diagram;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the selected concept.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static TConcept GetSelectedConcept<TConcept>(IServiceProvider provider) where TConcept : class
		{
			ModelElement element = GetSelectedElement(provider);

			if(element is TConcept)
			{
				return element as TConcept;
			}

			return default(TConcept);
		}

		/// <summary>
		/// Gets the selected element.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static ModelElement GetSelectedElement(IServiceProvider provider)
		{
			ModelingDocView docView = DesignerHelper.GetModelingDocView(provider);

			if (docView != null)
			{
				if (docView.SelectionCount == 1)
				{
					foreach (object component in docView.GetSelectedComponents())
					{
						ShapeElement selectionShape = component as ShapeElement;
						ModelElement selectionElement = component as ModelElement;
						
						if (selectionShape != null)
						{
							return selectionShape.ModelElement;
						}
						else if (selectionElement != null)
						{
							return selectionElement;
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the selected shape.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static ShapeElement GetSelectedShape(IServiceProvider provider)
		{
            Guard.ArgumentNotNull(provider, "provider");

			DiagramDocView docView = DesignerHelper.GetDiagramDocView(provider);

			if(docView != null)
			{
				if(docView.SelectionCount == 1)
				{
					foreach(object component in docView.GetSelectedComponents())
					{
						ShapeElement selectedShape = component as ShapeElement;
						if(selectedShape != null)
						{
							return selectedShape;
						}
					}
				}
			}

			return null;
		}
		 
		/// <summary>
		/// Gets the selected elements.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public static IList<object> GetSelectedElements(IServiceProvider provider)
		{
			ModelingDocView docView = DesignerHelper.GetModelingDocView(provider);

			if(docView != null)
			{
				if(docView.SelectionCount > 0)
				{
					IList<object> elements = new List<object>(docView.SelectionCount);

					foreach(object component in docView.GetSelectedComponents())
					{
						ShapeElement selectedShape = component as ShapeElement;
						ModelElement selectedElement = component as ModelElement;
						if (selectedShape != null)
						{
							elements.Add(selectedShape.ModelElement);
						}
						else if (selectedElement != null)
						{
							elements.Add(selectedElement);
						}
					}

					return elements;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the element.
		/// </summary>
		/// <param name="store">The store.</param>
		/// <returns></returns>
		public static T GetElement<T>(Store store) where T : ModelElement
		{
			T element = default(T);
			foreach (ModelElement mel in store.ElementDirectory.AllElements)
			{
				if (mel is T)
				{
					element = (T)mel;
					break;
				}
			}
			return element;
		}

		/// <summary>
		/// Gets the element.
		/// </summary>
		/// <param name="store">The store.</param>
		/// <param name="elementId">The element id.</param>
		/// <returns></returns>
		public static T GetElement<T>(Store store, Guid elementId) where T : ModelElement
		{
			return (T)store.ElementDirectory.GetElement(elementId);
		}

		/// <summary>
		/// Loads the domain model serialized in the specified path.
		/// </summary>
		/// <param name="modelPath">The model path.</param>
		/// <param name="loader">The model loader.</param>
		/// <returns></returns>
		public static TModel LoadModel<TDomainModel, TModel>(string modelPath, ModelLoader<TModel> loader)
			where TDomainModel : DomainModel
			where TModel : ModelElement
		{
			Store store = new Store(typeof(CoreDesignSurfaceDomainModel), typeof(TDomainModel));
			SerializationResult result = new SerializationResult();

			using (Transaction transaction = store.TransactionManager.BeginTransaction("Load", true))
			{
				TModel model = loader(modelPath, store, result);
				if (result.Failed)
				{
					throw new SerializationException(result);
				}
				transaction.Commit();
				return model;
			}
		}

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TElement GetElementFromShape<TElement>(PresentationElement element)
            where TElement : ModelElement
        {
            return PresentationViewsSubject.GetSubject(element) as TElement;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static TShape GetShapeFromElement<TShape>(ModelElement element)
            where TShape : ShapeElement
        {
            return PresentationViewsSubject.GetPresentation(element).FirstOrDefault() as TShape;
        }

        public static ModelElement GetModelElement(object target)
        {
            ModelElement element = null;
            PresentationElement presentation = target as PresentationElement;
            if (presentation != null)
            {
                if (presentation.Subject != null)
                {
                    element = presentation.Subject;
                }
            }
            else
            {
                element = target as ModelElement;
            }
            return element;
        }
	}
}
