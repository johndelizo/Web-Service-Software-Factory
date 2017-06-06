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
	public class EmbeddingReferenceVisitorFilter : IElementVisitorFilter
	{
		public virtual VisitorFilterResult ShouldVisitRelationship(
			ElementWalker walker, 
			ModelElement sourceElement, 
			DomainRoleInfo sourceRoleInfo, 
			DomainRelationshipInfo domainRelationshipInfo, 
			ElementLink targetRelationship)
		{
			Guard.ArgumentNotNull(sourceElement, "sourceElement");
			Guard.ArgumentNotNull(domainRelationshipInfo, "domainRelationshipInfo");
			Guard.ArgumentNotNull(targetRelationship, "targetRelationship");

			if(!(sourceElement is ShapeElement))
			{
				foreach(DomainRoleInfo info in domainRelationshipInfo.DomainRoles)
				{
					if(info.GetRolePlayer(targetRelationship) == sourceElement)
					{
						return VisitorFilterResult.Yes;
					}
				}
			}

			return VisitorFilterResult.DoNotCare;
		}

		public virtual VisitorFilterResult ShouldVisitRolePlayer(
			ElementWalker walker, 
			ModelElement sourceElement, 
			ElementLink elementLink, 
			DomainRoleInfo targetDomainRole, 
			ModelElement targetRolePlayer)
		{
			Guard.ArgumentNotNull(targetDomainRole, "targetDomainRole");

			if(targetDomainRole.IsEmbedding)
			{
				return VisitorFilterResult.DoNotCare;
			}

			return VisitorFilterResult.Yes;
		}
	}
}