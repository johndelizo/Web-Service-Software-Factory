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
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Practices.ServiceFactory.Description.Tests
{
    class Constants
    {
        public const string ServiceName = "ServiceImplementation.CustomerManager";
        public const string ServiceBindingName = "ServiceBinding";
        public const string ServiceBehaviorName = "ServiceBehavior";
        public const string ServiceBehaviorExtension = "BehaviorExtension";
        public const string MembershipProviderName = "SqlProvider";
        public const string TestCert = "CN=WCFSecurity";
        public const string EndpointAddressClientConfigService = "http://localhost:5555/Host/ApplyClientActionFixture";
        public const string BasicHttpBindingName = "basicHttpBinding";
        public const string WSHttpBindingName = "wsHttpBinding";
        public const string EndpointName = "Binding=basicHttpBinding, Contract=ServiceContract.ICustomerManager";
        public const string ContractType = "ServiceContract.ICustomerManager";

        public class Uris
        {
            public const string TestContractGenerationEndpointAddress = "http://localhost:7778/Host/Service.svc";
            public const string TestContractGenerationMultiEndpointAddress = "http://localhost:7779/Host/Service.svc";
        }

        public class ServiceDescription
        {
            public const string ClientEndpointName = "ClientCustomBinding";
            public const string ClientBehaviorConfiguration = "ClientBehavior";
            public const string ClientBindingName = "customBinding";
            public const string WsHttpHostClientName = "WSHttpBinding_IMockServiceContract";
            public const string WsHttpHostClientBinding = "wsHttpBinding";
        }
    }
}
