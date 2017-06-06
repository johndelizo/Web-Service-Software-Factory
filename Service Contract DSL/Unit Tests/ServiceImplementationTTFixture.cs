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

namespace ServiceContractDsl.Tests
{
	public abstract class ServiceImplementationTTFixture : ServiceContractModelFixture
	{
		protected enum ImplementationKind
		{
			Response = 0x01,
			Request = 0x10,
			RequestResponse = 0x11,
		}

		protected string GetImplementationContent(ImplementationKind implKind)
		{
			string responseText = "void";
			string requestText = string.Empty;
			if ((implKind & ImplementationKind.Request) == ImplementationKind.Request)
			{
				requestText = "RequestName request";
			}
			if ((implKind & ImplementationKind.Response) == ImplementationKind.Response)
			{
				responseText = "ResponseName";
			}
			string implementationContent = String.Format(
				Environment.NewLine +
				"namespace Namespace1" + Environment.NewLine +
				"{{" + Environment.NewLine +
				"	public partial class MyService" + Environment.NewLine +
				"	{{" + Environment.NewLine +
				"		public override {0} OperationName({1})" + Environment.NewLine +
				"		{{" + Environment.NewLine +
				"			throw new NotImplementedException();" + Environment.NewLine +
				"		}}" + Environment.NewLine +
				"	}}" + Environment.NewLine +
				"}}",responseText,requestText);
			return implementationContent;
		}
	}
}
