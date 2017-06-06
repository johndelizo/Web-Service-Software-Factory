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

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{	
	// This partial implementation will catch the AmbiguousMonikerException and will yield 
	// the exception handling to the validation framework in the Services Guidance Package
	public partial class ServiceContractDslSerializationBehaviorMonikerResolver
	{
		public override bool ProcessAddedElement(ModelElement mel)
		{
			try
			{
				return base.ProcessAddedElement(mel);
			}
			catch (AmbiguousMonikerException)
			{
				// This will be logged by the validation framework
				return false;
			} 
		}
	}
}
