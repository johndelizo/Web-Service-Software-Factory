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
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDiagrams = global::Microsoft.VisualStudio.Modeling.Diagrams;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	public static partial class ConnectionBuilder
	{
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		public static bool CanAcceptSource(DslModeling::ModelElement candidate)
		{
			if(candidate == null) return false;
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)
			{
				return true;
			}
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
			{
				return true;
			}
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
			{
				return true;
			}
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)
			{
				return true;
			}
			else
				return false;
		}

		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		public static bool CanAcceptTarget(DslModeling::ModelElement candidate)
		{
			if(candidate == null) return false;
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)
			{
				return true;
			}
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
			{
				return true;
			}
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
			{
				return true;
			}
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)
			{
				return true;
			}
			else if(candidate is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
			{
				return true;
			}
			else
				return false;
		}

		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Generated code.")]
		public static bool CanAcceptSourceAndTarget(DslModeling::ModelElement candidateSource, DslModeling::ModelElement candidateTarget)
		{
			// Accepts null, null; source, null; source, target but NOT null, target
			if(candidateSource == null)
			{
				if(candidateTarget != null)
				{
					throw new global::System.ArgumentNullException("candidateSource");
				}
				else // Both null
				{
					return false;
				}
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
				if(candidateSource is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)
				{
					if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)
					{
						return false;
					}
					else if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service sourceService = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)candidateSource;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract targetServiceContract = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)candidateTarget;

						if(targetServiceContract == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceReferencesServiceContract.GetLinkToService(targetServiceContract) != null) return false;
						if(sourceService == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceReferencesServiceContract.GetLinkToServiceContract(sourceService) != null) return false;
						if(targetServiceContract == null || sourceService == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceReferencesServiceContract.GetLinks(sourceService, targetServiceContract).Count > 0) return false;
						return true;
					}
				}
				if(candidateSource is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
				{
					if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract sourceServiceContract = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)candidateSource;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation targetOperation = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)candidateTarget;

						if(targetOperation == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractReferencesOperations.GetLinkToServiceContract(targetOperation) != null) return false;
						if(targetOperation == null || sourceServiceContract == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractReferencesOperations.GetLinks(sourceServiceContract, targetOperation).Count > 0) return false;
						return true;
					}
					else if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract sourceServiceContract = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)candidateSource;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service targetService = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)candidateTarget;

						if(targetService == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceReferencesServiceContract.GetLinkToService(sourceServiceContract) != null) return false;
						if(sourceServiceContract == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceReferencesServiceContract.GetLinkToServiceContract(targetService) != null) return false;
						if(targetService == null || sourceServiceContract == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceReferencesServiceContract.GetLinks(targetService, sourceServiceContract).Count > 0) return false;
						return true;
					}
					else if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
					{
						return false;
					}
				}
				if(candidateSource is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
				{
					if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation sourceOperation = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)candidateSource;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase targetMessageContract = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)candidateTarget;
						if(sourceOperation == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.OperationReferencesResponse.GetLinkToResponse(sourceOperation) != null) return false;
						if(targetMessageContract == null || sourceOperation == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.OperationReferencesResponse.GetLinks(sourceOperation, targetMessageContract).Count > 0) return false;
						// NOT reuse the MC shapes across operations 
						if (IsReused(targetMessageContract, sourceOperation.Request)) return false;
						return true;
					}
					if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation sourceOperation = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)candidateSource;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract targetServiceContract = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)candidateTarget;

						if(targetServiceContract == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractReferencesOperations.GetLinkToServiceContract(sourceOperation) != null) return false;
						if(targetServiceContract == null || sourceOperation == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractReferencesOperations.GetLinks(targetServiceContract, sourceOperation).Count > 0) return false;
						return true;
					}
					else if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
					{
						return false;
					}
				}
				if(candidateSource is global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)
				{
					if(candidateTarget is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase sourceMessageContract = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)candidateSource;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation targetOperation = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)candidateTarget;
						if(targetOperation == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.RequestReferencedByOperation.GetLinkToRequest(targetOperation) != null) return false;
						if(targetOperation == null || sourceMessageContract == null || global::Microsoft.Practices.ServiceFactory.ServiceContracts.RequestReferencedByOperation.GetLinks(sourceMessageContract, targetOperation).Count > 0) return false;
						// NOT reuse the MC shapes across operations 
						if (IsReused(sourceMessageContract, targetOperation.Response)) return false;
						return true;
					}
				}

			}
			return false;
		}

		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Generated code.")]
		public static DslModeling::ElementLink Connect(DslModeling::ModelElement source, DslModeling::ModelElement target)
		{
			if(source == null)
			{
				throw new global::System.ArgumentNullException("source");
			}
			if(target == null)
			{
				throw new global::System.ArgumentNullException("target");
			}

			if(CanAcceptSourceAndTarget(source, target))
			{
				if(source is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)
				{
					if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service sourceAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)source;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract targetAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)target;

						return ConnectServiceToServiceContract(sourceAccepted, targetAccepted);
					}
					else if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)
					{
						return null;
					}
				}
				if(source is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
				{
					if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract sourceAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)source;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation targetAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)target;

						return ConnectServiceContractToOperation(sourceAccepted, targetAccepted);
					}
					else if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract sourceAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)source;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service targetAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Service)target;

						return ConnectServiceContractToService(sourceAccepted, targetAccepted);
					}
					else if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
					{
						return null;
					}
				}
				if(source is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
				{
					if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation sourceAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)source;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase targetAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)target;
						DslModeling::ElementLink result = new global::Microsoft.Practices.ServiceFactory.ServiceContracts.OperationReferencesResponse(sourceAccepted, targetAccepted);
						if(DslModeling::DomainClassInfo.HasNameProperty(result))
						{
							DslModeling::DomainClassInfo.SetUniqueName(result);
						}
						return result;
					}
					else if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation sourceAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)source;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract targetAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract)target;

						return ConnectOperationToServiceContract(sourceAccepted, targetAccepted);
					}
					else if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
					{
						return null;
					}
				}
				if(source is global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)
				{
					if(target is global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)
					{
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase sourceAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase)source;
						global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation targetAccepted = (global::Microsoft.Practices.ServiceFactory.ServiceContracts.Operation)target;
						DslModeling::ElementLink result = new global::Microsoft.Practices.ServiceFactory.ServiceContracts.RequestReferencedByOperation(sourceAccepted, targetAccepted);
						if(DslModeling::DomainClassInfo.HasNameProperty(result))
						{
							DslModeling::DomainClassInfo.SetUniqueName(result);
						}
						return result;
					}
				}

			}
			global::System.Diagnostics.Debug.Fail("Having agreed that the connection can be accepted we should never fail to make one.");
			throw new global::System.InvalidOperationException();
		}

		private static DslModeling::ElementLink ConnectServiceContractToService(ServiceContract sourceBeforePathTraverse, Service targetBeforePathTraverse)
		{
			DslModeling::ElementLink result = new ServiceReferencesServiceContract(targetBeforePathTraverse, sourceBeforePathTraverse);

			SetUniqueName(result);

			return result;
		}

		private static DslModeling::ElementLink ConnectServiceToServiceContract(Service sourceBeforePathTraverse, ServiceContract targetBeforePathTraverse)
		{
			DslModeling::ElementLink result = new ServiceReferencesServiceContract(sourceBeforePathTraverse, targetBeforePathTraverse);

			SetUniqueName(result);

			return result;
		}

		private static DslModeling::ElementLink ConnectServiceContractToOperation(ServiceContract sourceBeforePathTraverse, Operation targetBeforePathTraverse)
		{
			DslModeling::ElementLink result = new ServiceContractReferencesOperations(sourceBeforePathTraverse, targetBeforePathTraverse);
			
			SetUniqueName(result);

			targetBeforePathTraverse.Action = Operation.BuildDefaultAction(targetBeforePathTraverse);

			return result;
		}

		private static DslModeling::ElementLink ConnectOperationToServiceContract(Operation sourceBeforePathTraverse, ServiceContract targetBeforePathTraverse)
		{
			DslModeling::ElementLink result = new ServiceContractReferencesOperations(targetBeforePathTraverse, sourceBeforePathTraverse);

			SetUniqueName(result);

			sourceBeforePathTraverse.Action = Operation.BuildDefaultAction(sourceBeforePathTraverse);

			return result;
		}

		private static void SetUniqueName(DslModeling::ElementLink  link)
		{
			if(DslModeling::DomainClassInfo.HasNameProperty(link))
			{
				DslModeling::DomainClassInfo.SetUniqueName(link);
			}
		}

		private static bool IsReused(global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase message,
			global::Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase messageOp)
		{			
			return message != messageOp && 
				  (message.RequestFor.Count != 0 || message.ResponseFor.Count != 0);
		}
	}
}