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
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Microsoft.Practices.ServiceFactory.Description.Tests
{
    public partial class MockService : IMockServiceContract, IMockServiceContract2
    {
        #region IMockServiceContract Members

        public string Echo(string input)
        {
            return input;
        }

        public string EchoData(MyDataContract data)
        {
            return data.Data1;
        }

        #endregion

        #region IMockServiceContract2 Members

        public string Echo2(string input)
        {
            return input;
        }

        #endregion
    }

    // contract used for WsHttpBinding and for testing ProtectionLevel (ContractGenerator tests)
    [ServiceContract(Name = "IMockServiceContract", 
        Namespace = "http://Microsoft.Practices.ServiceFactory.WCF.Security.Tests.Mocks",
        ProtectionLevel=System.Net.Security.ProtectionLevel.Sign)]
    public interface IMockServiceContract
    {
        [OperationContract(Action = "Echo", IsTerminating = false, IsInitiating = true, IsOneWay = false, AsyncPattern = false, ProtectionLevel=System.Net.Security.ProtectionLevel.EncryptAndSign)]
        string Echo(string input);

        [OperationContract]
        string EchoData(MyDataContract data);
    }

    // contract used for BasicHttpBinding (MetadataDiscovery tests)
    [ServiceContract(Name = "IMockServiceContract2", 
        Namespace = "http://Microsoft.Practices.ServiceFactory.WCF.Security.Tests.Mocks")]
    public interface IMockServiceContract2
    {
        [OperationContract(Action = "Echo2", IsTerminating = false, IsInitiating = true, IsOneWay = false, AsyncPattern = false)]
        string Echo2(string input);
    }

    [DataContract]
    public class MyDataContract
    {
        [DataMember]
        public string Data1;

        [DataMember]
        public int Data2;
    }
}
