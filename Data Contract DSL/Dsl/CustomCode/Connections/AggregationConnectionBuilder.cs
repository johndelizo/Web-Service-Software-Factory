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
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	public static partial class AggregationConnectionBuilder
	{
		public static bool CanAcceptSource(ModelElement candidate)
		{
			if(candidate == null)
			{
				return false;
			}
			else if(candidate is DataContractBase)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool CanAcceptTarget(ModelElement candidate)
		{
			if(candidate == null)
			{
				return false;
			}
			else if(candidate is DataContract)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool CanAcceptSourceAndTarget(ModelElement candidateSource, ModelElement candidateTarget)
		{
			if(candidateSource == null)
			{
				return false;
			}

			bool acceptSource = CanAcceptSource(candidateSource);
			// If the source wasn't accepted then there's no point checking targets.
			// If there is no target then the source controls the accept.
			if(!acceptSource || candidateTarget == null)
			{
				return acceptSource;
			}
			else // Check combinations
			{
				if(candidateSource is DataContractBase)
				{
                    if (candidateTarget is DataContract) 
					{
						if(HasNullReferences((DataContractBase)candidateSource, (Contract)candidateTarget))
						{
							return false;
						}						
						return true;
					}
				}
			}
			return false;
		}

		private static bool HasNullReferences(DataContractBase candidateSource, Contract candidateTarget)
		{
			return candidateSource == null || candidateTarget == null;
		}

		public static ElementLink Connect(ModelElement source, ModelElement target)
		{
			return Connect(source, target, null);
		}

		public static ElementLink Connect(ModelElement source, ModelElement target, string targetDataElementName)
		{
            Guard.ArgumentNotNull(source, "source");
            Guard.ArgumentNotNull(target, "target");

			if(CanAcceptSourceAndTarget(source, target))
			{
				if(source is DataContractBase)
				{
					if(target is DataContract)
					{
						DataContractBase sourceAccepted = (DataContractBase)source;
						Contract targetAccepted = (Contract)target;
						ElementLink result = new DataContractBaseCanBeContainedOnContracts(sourceAccepted, targetAccepted);
						if(DomainClassInfo.HasNameProperty(result))
						{
							DomainClassInfo.SetUniqueName(result);
						}

						using(Transaction transaction = targetAccepted.Store.TransactionManager.BeginTransaction())
						{
							ModelElementReference dataElement = target.Store.ElementFactory.CreateElement(ModelElementReference.DomainClassId) as ModelElementReference;
							dataElement.ModelElementGuid = result.Id;
							dataElement.Type = sourceAccepted.Name;

                            LinkedElementCollection<DataMember> members = ((DataContract)target).DataMembers;

                            dataElement.Name = GetDataElementName(sourceAccepted.Name, targetAccepted.Name, members);
                            members.Add(dataElement);

							transaction.Commit();
						}
						return result;
					}
				}
			}
			
			Debug.Fail("Having agreed that the connection can be accepted we should never fail to make one.");
			throw new InvalidOperationException();
		}

        private static string GetDataElementName(
            string sourceName, 
            string targetName, 
            LinkedElementCollection<DataMember> dataElements)
        {
            // First check if we have duplicates in target DataElements
            string name = sourceName;
 
            for (int suffix = 1; dataElements.Find(m => { return m.Name.Equals(name, StringComparison.OrdinalIgnoreCase); }) != null; suffix++)
            {
                name = string.Concat(sourceName, suffix);
            } 

            // Second check if this is a self reference and therefore we need to get a new name (!= element name)
            for(int sfx = 1; name == targetName; sfx++)
            {
                name = string.Concat(sourceName, sfx);
            }

            return name; 
        }
	}
}