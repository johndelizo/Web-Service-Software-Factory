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
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	public static class ModelCollector
	{
        public static IArtifactLinkContainer GetArtifacts(Store store)
        {
            return GetArtifacts(store.ElementDirectory.AllElements);
        }

		public static IArtifactLinkContainer GetArtifacts(ICollection<ModelElement> elements)
		{
            ArtifactLinkCollector collector = new ArtifactLinkCollector();

            foreach (ModelElement element in elements)
            {
                if (typeof(IExtensibleObject).IsAssignableFrom(element.GetType()))
                {
                    IArtifactLinkContainer links = GetArtifacts(element);
                    collector.Collect(links);
                }
            }
            return collector;
        }

		public static IArtifactLinkContainer GetArtifacts(ModelElement modelElement)
		{
			IArtifactLinkContainer links = null;
			if (modelElement is IArtifactLinkContainer)
			{
				links = (IArtifactLinkContainer)modelElement;
			}
			else if (modelElement is IExtensibleObject)
			{
				IExtensibleObject extendedObject = (IExtensibleObject)modelElement;
				links = extendedObject.ObjectExtender as IArtifactLinkContainer;
			}
			return links;
		}

		public static IArtifactLinkContainer GetArtifacts(IServiceProvider serviceProvider)
		{
            return GetArtifacts(serviceProvider, false);
		}

        /// <summary>
        /// Will walk until it finds the first valid artifact link with roles
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static bool HasValidArtifactsAndRoles(IServiceProvider serviceProvider)
        {
            IArtifactLinkContainer container = ModelCollector.GetArtifacts(serviceProvider, true);
            return HasValidArtifacts(container) && HasRoles(container);
        }

        /// <summary>
        /// Returns true if if finds any ArtifactLink
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static bool HasValidArtifacts(IArtifactLinkContainer container)
        {
            return container != null &&
                container.ArtifactLinks != null &&
                container.ArtifactLinks.Count > 0;
        }

        /// <summary>
        /// Returns true if it finds roles in an ArtifactLink
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static bool HasRoles(IArtifactLinkContainer container)
        {
            if (container != null)
            {
                foreach (IArtifactLink link in container.ArtifactLinks)
                {
                    if (link.Container != Guid.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static IArtifactLinkContainer GetArtifacts(IServiceProvider serviceProvider, bool firstValid)
        {
            ShapeElement selectedShape = DomainModelHelper.GetSelectedShape(serviceProvider);
            
            if (selectedShape != null &&
                selectedShape.ModelElement != null)
            {
                if (selectedShape is Diagram)
                {
                    if (firstValid)
                    {
                        foreach (ModelElement element in selectedShape.ModelElement.Store.ElementDirectory.AllElements)
                        {
                            if (typeof(IExtensibleObject).IsAssignableFrom(element.GetType()))
                            {
                                IArtifactLinkContainer links = GetArtifacts(element);
                                // trim walk on first valid links
                                return FilterValidLinks(selectedShape.ModelElement.Store, links);
                            }
                        }
                        return null;
                    }
                    return GetArtifacts(selectedShape.ModelElement.Store);
                }
                else
                {
                    IArtifactLinkContainer links = GetArtifacts(selectedShape.ModelElement);
                    return firstValid ? FilterValidLinks(serviceProvider, links) : links;
                }
            }

            ModelElement selectedElement = DomainModelHelper.GetSelectedElement(serviceProvider);
            if (selectedElement != null)
            {
                IArtifactLinkContainer links = GetArtifacts(selectedElement);
                return firstValid ? FilterValidLinks(serviceProvider, links) : links;
            }

            return null;
        }

        private static IArtifactLinkContainer FilterValidLinks(IServiceProvider serviceProvider, IArtifactLinkContainer links)
        {
            ICodeGenerationService codeGenerationService = serviceProvider.GetService(typeof(ICodeGenerationService)) as ICodeGenerationService;
            if (codeGenerationService == null ||
                links == null || !codeGenerationService.AreValid(links.ArtifactLinks))
            {
                return null;
            }
            return links;
        }
	}
}
