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
using System.Xml.Serialization;
using System.ComponentModel;
using System.Net.Security;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf.CodeGeneration;
using System.Globalization;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Wcf
{
    [Serializable]
    [ObjectExtender(typeof(PrimitiveMessagePart))]
    [CLSCompliant(false)]
    public class WCFPrimitiveMessagePart : WCFMessagePartBase<PrimitiveMessagePart>
    {
        public WCFPrimitiveMessagePart()
        {
        }

        protected override ServiceContractModel Model
        {
            get { return this.ModelElement.Message.ServiceContractModel; }
        }
    }
}
