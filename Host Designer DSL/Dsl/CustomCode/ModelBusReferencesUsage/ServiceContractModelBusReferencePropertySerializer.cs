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
using Microsoft.Practices.ServiceFactory.Common.Dsl;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using System;

namespace Microsoft.Practices.ServiceFactory.HostDesigner
{
    [CLSCompliant(false)]
    public class ServiceContractModelBusReferencePropertySerializer : ModelBusReferencePropertySerializer<Service>
    {
        public override string FileExtension
        {
            get { return ".servicecontract"; }
        }

        public override string LogicalAdapterId
        {
            get { return "Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractDslAdapter"; }
        }        
    }
}
