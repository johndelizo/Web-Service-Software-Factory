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
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.ServiceFactory.ServiceContracts;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf
{
	[ExtensionProviderAttribute("3D062FD1-5877-4760-AE82-D8A31AF845B5", "WCF", "WCF Extension", typeof(ServiceContractDslDomainModel))]
	public class ServiceContractWCFExtensionProvider : ExtensionProviderBase
	{
		public const string ExtensionProviderPropertyCategory = "WCF Settings";

		public ServiceContractWCFExtensionProvider()
		{
			ObjectExtenderList = new List<Type>();

			ObjectExtenderList.Add(typeof(WCFService));
			ObjectExtenderList.Add(typeof(WCFServiceContract));
			ObjectExtenderList.Add(typeof(WCFOperationContract));
			ObjectExtenderList.Add(typeof(WCFMessageContract));
			ObjectExtenderList.Add(typeof(WCFXsdMessageContract));
			ObjectExtenderList.Add(typeof(WCFXsdElementFault));
            ObjectExtenderList.Add(typeof(WCFPrimitiveMessagePart));
            ObjectExtenderList.Add(typeof(WCFDataContractMessagePart));
		}

		public override IList<Type> ObjectExtenders
		{
			get
			{
				return ObjectExtenderList;
			}
		}
	}
}