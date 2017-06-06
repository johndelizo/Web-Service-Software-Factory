using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
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
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	public class FullDepthElementWalker : ElementWalker
	{
		#region Constructors
		/// <summary>
		/// Constructor that takes an ElementVisitor.
		/// This defaults to depth first traversal, pre-Order visitation of the graph with no element links.
		/// </summary>
		/// <param name="visitor">IElementVisitor to use when traversing</param>
		/// <param name="filter">IElementVisitorFilter to use when traversing</param>
		public FullDepthElementWalker(IElementVisitor visitor, IElementVisitorFilter filter)
			:
			base(visitor, filter)
		{
		}

		/// <summary>
		/// Constructor that takes an ElementVisitor.
		/// </summary>
		/// <param name="visitor">IElementVisitor to use when traversing</param>
		/// <param name="filter">IElementVisitorFilter to use when traversing</param>
		/// <param name="includeLinks">request element links be included in the visitation</param>
		public FullDepthElementWalker(IElementVisitor visitor, IElementVisitorFilter filter, bool includeLinks)
			:
			base(visitor, filter, includeLinks)
		{
		}
		#endregion

		#region public Methods
		/// <summary>
		/// Traverse the model starting at the specified starting element.
		/// </summary>
		/// <param name="rootElement">ModelElement from which to start traversing</param>
		/// <returns>false if the traversal was terminated prematurely, otherwise true</returns>
		public override sealed bool DoTraverse(ModelElement rootElement)
		{
			Guard.ArgumentNotNull(rootElement, "rootElement");

			bool keepTraversing = true;
			Visitor.StartTraverse(this);
			if(rootElement != null)
			{
				keepTraversing = DoVisitElement(rootElement);
				if(keepTraversing)
				{
					foreach(ModelElement element in this.InternalElementList)
					{
						if(!(keepTraversing = Visitor.Visit(this, element)))
							break;
					}
				}
			}
			Visitor.EndTraverse(this);
			return keepTraversing;
		}

		protected virtual IList<ModelElement> GetRelatedElements(ModelElement element)
		{
			List<ModelElement> elementList = new List<ModelElement>();
			List<ModelElement> elementLinks = IncludeLinks ? new List<ModelElement>() : null;

			// All the links to this element needs to be examined to determine whether we should continue copying process or not
			// Find all the links connect to this element (or elementLink since it's legal to have element link to be a roleplayer as well...)
			foreach(DomainRoleInfo domainRole in element.GetDomainClass().AllDomainRolesPlayed)
			{
				// this function supports demand loading
				ReadOnlyCollection<ElementLink> links = domainRole.GetElementLinks(element);

				foreach(ElementLink eachElemLink in links)
				{
					if(eachElemLink.GetType() != typeof(PresentationViewsSubject))
					{
						DomainRelationshipInfo domainRelInfo = eachElemLink.GetDomainRelationship();

						if(Filter.ShouldVisitRelationship(this, element, domainRole, domainRelInfo, eachElemLink) == VisitorFilterResult.Yes)
						{
							if(IncludeLinks)
							{
								elementLinks.Add(eachElemLink);
							}

							IList<DomainRoleInfo> domainRoles = domainRelInfo.DomainRoles;

							for(int i = 0; i < domainRoles.Count; i++)
							{
								DomainRoleInfo role = domainRoles[i];
								ModelElement rolePlayer = role.GetRolePlayer(eachElemLink);

								// Find each roleplayer and add them to the queue list
								if((rolePlayer != element) && !Visited(rolePlayer) && (Filter.ShouldVisitRolePlayer(this, element, eachElemLink, role, rolePlayer) == VisitorFilterResult.Yes))
								{
									elementList.Add(rolePlayer);
								}
							}
						}
					}
				}
			}

			if(IncludeLinks)
			{
				elementList.AddRange(elementLinks);
			}
			elementList.TrimExcess();

			return elementList;
		}

		protected virtual void BeginTraverseElement(ModelElement element)
		{
		}

		protected virtual void EndTraverseElement(ModelElement element)
		{
		}

		#endregion

		#region Private Mehtods
		private bool DoVisitElement(ModelElement e)
		{
			// Before we traverse the element. Call the virtual so the decendent class can be notified
			BeginTraverseElement(e);
			bool result = VisitElementAndLinks(e);
			// After we're done, call the EndTraverseElement so we know we're done with the element.
			EndTraverseElement(e);
			return result;
		}

		private bool VisitElement(ModelElement e)
		{
			if(Visited(e))
			{
				return true;
			}
			else
			{
				MarkVisited(e);
				InternalElementList.Add(e);
				return true;
			}
		}

		private bool VisitElementAndLinks(ModelElement e)
		{
			bool keepVisiting = VisitElement(e);
			if(keepVisiting)
			{
				ElementLink thisLink = e as ElementLink;
				if(thisLink != null)
				{
					keepVisiting = VisitRolePlayers(thisLink);
				}

				if(keepVisiting)
				{
					IList<ModelElement> elems = GetRelatedElements(e);

					foreach(ModelElement child in elems)
					{
						if(!Visited(child))
						{
							keepVisiting = DoVisitElement(child);
						}
					}

					ICrossModelingPropertyHolder holder = e as ICrossModelingPropertyHolder;

					if(holder != null)
					{
                        ModelElement refElement = ModelBusReferenceResolver.ResolveAndCache(holder.Type);
                        if (refElement != null)
                        {
                            DoVisitElement(refElement);
                        }
                    }
				}
			}
			return keepVisiting;
		}

		private bool VisitRolePlayers(ElementLink link)
		{
			bool keepVisiting = true;
			DomainRelationshipInfo domainRelInfo = link.GetDomainRelationship();

			IList<DomainRoleInfo> domainRoles = domainRelInfo.DomainRoles;

			for(int i = 0; i < domainRoles.Count && keepVisiting; i++)
			{
				DomainRoleInfo role = domainRoles[i];

				// Since GetRolePlayer will do demand-loading, we first need to test here whether
				// role supports demand loading and if it is, don't load if not asked to.
				if(this.BypassDemandLoading)
				{
					Moniker rolePlayerMoniker = role.GetRolePlayerMoniker(link);
					if(rolePlayerMoniker != null && rolePlayerMoniker.ModelElement == null)
					{
						// skip this role if it's not resolved yet
						continue;
					}
				}

				ModelElement rolePlayer = role.GetRolePlayer(link);

				// Find each roleplayer and add them to the queue list
				// RolePlayer might be null if it is an unresolved moniker
				if((rolePlayer != link) &&
					(rolePlayer != null) &&
					!Visited(rolePlayer) &&
					(Filter.ShouldVisitRolePlayer(this, link, link, role, rolePlayer) == VisitorFilterResult.Yes))
				{
					keepVisiting = DoVisitElement(rolePlayer);
				}
			}
			return keepVisiting;
		}

		#endregion
    }
}