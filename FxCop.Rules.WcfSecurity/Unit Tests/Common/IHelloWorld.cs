using System;
using System.Collections.Generic;
using System.Text;

using System.ServiceModel;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity.Tests
{
    [System.ServiceModel.ServiceContractAttribute(
        Namespace = "http://WCFService1.ServiceContracts/2006/08",
        Name = "IPing",
        ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
    public interface IHelloWorld
    {
        [OperationContract(Name = "helloWorld", ProtectionLevel = System.Net.Security.ProtectionLevel.None)]
        string HelloWorld(string message);
    }

    [System.ServiceModel.ServiceContractAttribute(
    Namespace = "http://WCFService1.ServiceContracts/2006/08",
    Name = "IPing")]
    public interface IHelloWorld2
    {
        [OperationContract(Name = "helloWorld")]
        string HelloWorld(string message);
    }
}
