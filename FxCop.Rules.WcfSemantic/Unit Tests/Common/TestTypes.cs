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

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic.Tests.Common
{
	[ServiceContract(Namespace = "urn:foo", Name = "IValidServiceContract", SessionMode = SessionMode.Allowed)]
	public partial interface IValidServiceContract
	{
		[OperationContract(IsTerminating = false, IsInitiating = true, IsOneWay = false, AsyncPattern = false, Action = "Operation1")]
		void Operation1();
	}

    [ServiceContract(Name = "INoSessionDeclared")]
    public interface INoSessionDeclared
    {
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        string Operation(string message);
    }

    [ServiceContract(Name = "INotAllowedSessionDeclared", SessionMode = SessionMode.NotAllowed)]
    public interface INotAllowedSessionDeclared
    {
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        string Operation(string message);
    }

    [ServiceContract(Name = "INoOperationContractValuesDeclared", SessionMode = SessionMode.Required)]
    public interface INoOperationContractValuesDeclared
    {
        [OperationContract()]
        string Operation(string message);
    }

    [ServiceContract()]
    public interface INoDefinedNameInServiceContract
    {
        [OperationContract()]
        string Operation(string message);
    }

    [ServiceContract(Name = "IMultipleUnmatchedMessageHandlers")]
    public interface IMultipleUnmatchedMessageHandlers
    {
        [OperationContract(Action="*")]
        string Operation1(string message);

        [OperationContract()]
        string Operation2(string message);

        [OperationContract(Action = "*")]
        string Operation3(string message);
    }

    [ServiceContract(Name = "IDuplicateOperations")]
    public interface IDuplicateOperations
    {
        [OperationContract()]
        string Operation(string message);

        [OperationContract()]
        string Operation(string message, int otherParam);

        [OperationContract()]
        string Operation2(string message);

        [OperationContract()]
        string Operation2(string message, int otherParam);
    }

    public interface IBadOperationBehaviorUsage
    {
        [OperationBehavior()]
        string Operation(string message);
    }

    [ServiceContract(Name = "IReturnVoidWithOneWay")]
    public interface IReturnVoidWithOneWay
    {
        [OperationContract(IsOneWay=true)]
        string Operation(string message);
    }
    
    [ServiceContract(Name = "IFaultsWithOneWayOperation")]
    public interface IFaultsWithOneWayOperation
    {
        [OperationContract(IsOneWay = true)]
        [FaultContract(typeof(ArgumentNullException))]
        void Operation(string message);
    }

    [ServiceContract(Name = "IOutputParamsWithOneWay")]
    public interface IOutputParamsWithOneWay
    {
        [OperationContract(IsOneWay = true)]
        void Operation(string message, out string outputParam);
    }

    [ServiceContract(Name = "IReplyActionWithOneWay")]
    public interface IReplyActionWithOneWay
    {
        [OperationContract(IsOneWay = true, ReplyAction="NotAllowedReplyAction")]
        void Operation(string message, out string outputParam);
    }

    [ServiceContract(Name = "IInValidCallbackContractType", CallbackContract = typeof(InValidCallbackContract))]
    public interface IInValidCallbackContractType
    {
        [OperationContract()]
        void Operation(string message);
    }

    public class InValidCallbackContract // should be interface or MarshalByRef class
    {
        [OperationContract(IsOneWay = true)]
        public void Reply(string responseToGreeting){ }
    }

    [ServiceContract(Name = "INonEmptyCallbackContractType", CallbackContract = typeof(INoOperationOnCallbackContractType))]
    public interface INonEmptyCallbackContractType
    {
        [OperationContract()]
        void Operation(string message);
    }

    public interface INoOperationOnCallbackContractType 
    {
        void Reply(string responseToGreeting);
        [OperationContract()]
        void Reply2(string responseToGreeting);
    }

    [ServiceContract(Name = "ISessionlessBinding", SessionMode = SessionMode.NotAllowed)]
    public interface ISessionlessBinding
    {
        [OperationContract()]
        void Operation(string message, out string outputParam);
    }

    [ServiceContract(Name = "ISessionfullBinding", SessionMode = SessionMode.Required)]
    public interface ISessionfullBinding
    {
        [OperationContract()]
        void Operation(string message, out string outputParam);
    }

    [ServiceContract(Name = "IContractBindingMismatch", CallbackContract = typeof(InValidCallbackContract))]
    public interface IContractBindingMismatch
    {
        [OperationContract(IsOneWay = false)]
        string Operation(string message);
    }

    [ServiceContract(Name = "IMixingMessageContractAttributes")]
    public interface IMixingMessageContractAttributes 
    { 
        [OperationContract]
        string Operation1(TestMessageContract message);

        [OperationContract]
        string Operation1(int id); // Should pass with same op name

        [OperationContract]
        TestMessageContract Operation2(TestMessageContract message, string otherType);

        [OperationContract]
        TestMessageContract Operation3(string message);

        [OperationContract(IsOneWay=true)]
        void Operation4(string message);
    }

    [MessageContract]
    public class TestMessageContract
    {
        [MessageBodyMember]
        public string Name;
    }

	[ServiceContract(Name = "IAsyncWithOneWayOperation")]
	public interface IAsyncWithOneWayOperation
	{
		[OperationContract(IsOneWay = true, AsyncPattern=true)]
		void Operation(string message);
	}
}
