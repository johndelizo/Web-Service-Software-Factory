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

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx
{
	[ExtensionProviderAttribute("C60DD2D7-0F7B-4d4d-B94F-6D8BDAE883B2", "ASMX", "ASMX Extension", typeof(ServiceContractDslDomainModel))]
	public class ServiceContractAsmxExtensionProvider : ExtensionProviderBase
	{
		public const string ExtensionProviderPropertyCategory = "ASMX Settings";

		public ServiceContractAsmxExtensionProvider()
		{
			ObjectExtenderList = new List<Type>();
			ObjectExtenderList.Add(typeof(AsmxService));
			ObjectExtenderList.Add(typeof(AsmxServiceContract));
			ObjectExtenderList.Add(typeof(AsmxOperationContract));
			ObjectExtenderList.Add(typeof(AsmxMessageContract));
			ObjectExtenderList.Add(typeof(AsmxXsdMessageContract));
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