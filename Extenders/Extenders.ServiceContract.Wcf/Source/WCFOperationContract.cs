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
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;
using System.Net.Security;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf
{
	[Serializable]
	[ObjectExtender(typeof(Operation))]
	[CLSCompliant(false)]
	public class WCFOperationContract : ObjectExtender<Operation>
	{
		#region Constructors

		public WCFOperationContract()
			: this(true, false, false, String.Empty, ProtectionLevel.None)
		{
		}

        public WCFOperationContract(bool isInitiating, bool isTerminating, bool asyncPattern, string replyAction, ProtectionLevel protectionLevel)
		{
			this.isInitiating = isInitiating;
			this.isTerminating = isTerminating;
			this.asyncPattern = asyncPattern;
			this.replyAction = replyAction;
            this.protectionLevel = protectionLevel;
		}

		#endregion

		#region Properties
        private ProtectionLevel protectionLevel;

        [Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
         Description("Indicates whether all messages supporting the contract have a explicit ProtectionLevel value."),
         DisplayName("Protection Level"),
         ReadOnly(false),
         BrowsableAttribute(true)]
        [XmlElement("ProtectionLevel")]
        public ProtectionLevel ProtectionLevel
        {
            get { return protectionLevel; }
            set { protectionLevel = value; }
        }

		private bool isInitiating;

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
         Description("Indicates whether the service operation causes the server to close the session after the reply message, if any, is sent."),
         DisplayName("Is Initiating"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("IsInitiating")]
		public bool IsInitiating
		{
			get { return isInitiating; }
			set { isInitiating = value; }
		}

		private bool isTerminating;

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
		 Description("Specifies whether WCF attempts to terminate the current session after the operation completes."),
         DisplayName("Is Terminating"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("IsTerminating")]
		public bool IsTerminating
		{
			get { return isTerminating; }
			set { isTerminating = value; }
		}

		private bool asyncPattern;

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
         Description("Indicates if the operation can be called asynchronously by using a Begin/End method pair."),
         DisplayName("Async Pattern"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("AsyncPattern")]
		public bool AsyncPattern
		{
			get { return asyncPattern; }
			set { asyncPattern = value; }
		}

		private string replyAction;

		[Category(ServiceContractWCFExtensionProvider.ExtensionProviderPropertyCategory),
         Description("The SOAP action for the response message of the operation."),
         DisplayName("Reply Action"),
         ReadOnly(false),
		 BrowsableAttribute(true)]
		[XmlElement("ReplyAction")]
		public string ReplyAction
		{
			get { return replyAction; }
			set { replyAction = value; }
		}

		#endregion
	}
}