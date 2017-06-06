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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.ServiceContracts;

namespace ServiceContractDsl.Tests
{
	/// <summary>
	/// Summary description for ServiceContractMessageContractFixture
	/// </summary>
	[TestClass]
	public class ServiceContractMessageContractFixture : ServiceContractModelFixture
	{
        const string dataContractModelProjectName = "Project1.model";
        const string dataContractModelFileName = "dc.datacontract";

		// Normally unit tests that excersize generated code are a questionable investment,
		// but these actually helped us in some of the refactoring.
		[TestMethod]
		public void CanAddDataContractMessageContractPart()
		{
			ServiceContract serviceContract = new ServiceContract(Store);
			serviceContract.ServiceContractModel = new ServiceContractModel(Store);

			Message message = new Message(Store);
			DataContractMessagePart dcMessagePart = new DataContractMessagePart(Store);

            string moniker = string.Format(@"mel://{0}\{1}\{2}@{3}\{4}",
                dcMessagePart.GetType().Namespace,
                dcMessagePart.GetType().Name,
                dcMessagePart.Id.ToString(),
                dataContractModelProjectName, dataContractModelFileName);

            //dcMessagePart.Type = moniker;

			message.MessageParts.Add(dcMessagePart);
			serviceContract.ServiceContractModel.Messages.Add(message);

			Assert.AreEqual<int>(serviceContract.ServiceContractModel.Messages[0].MessageParts.Count, 1);
		}

		// Normally unit tests that excersize generated code are a questionable investment,
		// but these actually helped us in some of the refactoring.
		[TestMethod]
		public void CanAddStringPrimitiveMessageContractPart()
		{
			ServiceContract serviceContract = new ServiceContract(Store);
			serviceContract.ServiceContractModel = new ServiceContractModel(Store);

			Message message = new Message(Store);
			PrimitiveMessagePart primitiveMessagePart = new PrimitiveMessagePart(Store);
			primitiveMessagePart.Type = typeof(System.String).ToString();

			message.MessageParts.Add(primitiveMessagePart);
			serviceContract.ServiceContractModel.Messages.Add(message);

			Assert.AreEqual<int>(serviceContract.ServiceContractModel.Messages[0].MessageParts.Count, 1);
		}

		protected override Type ContractType
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		protected override string Template
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}
	}
}
