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
using System.Net.Security;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using System.Drawing.Design;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using EntServ = System.EnterpriseServices;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx
{
	[Serializable]
	[CLSCompliant(false)]
	[ObjectExtender(typeof(Operation))]
	public class AsmxOperationContract : ObjectExtender<Operation>
	{
		public AsmxOperationContract()
		{
		}

		#region Properties

		private string description;

		[Category(ServiceContractAsmxExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("A description of this operation."),
		 ReadOnly(false),
         DisplayName("Description"),
         BrowsableAttribute(true)]
		[XmlElement("Description")]
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		private bool enableSession;

		[Category(ServiceContractAsmxExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies whether session state is enabled for this operation."),
		 ReadOnly(false),
         DisplayName("Enable Session"),
         BrowsableAttribute(true)]
		[XmlElement("EnableSession")]
		public bool EnableSession 
		{
			get { return enableSession; }
			set { enableSession = value; }
		}

		private EntServ::TransactionOption transactionOption;

		[Category(ServiceContractAsmxExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies the transaction type supported by this operation."),
		 ReadOnly(false),
         DisplayName("Transaction Option"),
         BrowsableAttribute(true)]
		[XmlElement("TransactionOption")]
		public EntServ::TransactionOption TransactionOption
		{
			get { return transactionOption; }
			set { transactionOption = value; }
		}

		#endregion
	}
}