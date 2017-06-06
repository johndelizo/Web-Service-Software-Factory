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
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using System.CodeDom.Compiler;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceContractDsl.Tests
{
	public abstract class MessageContractTTBaseFixture : ServiceContractModelFixture
	{
		string messageContractElementName = "Name1";
		string messageContractElementNamespace = "http://mynamespace";

		protected string MessageContractElementName
		{
			get { return messageContractElementName; }
			set { messageContractElementName = value; }
		}

		protected string MessageContractElementNamespace
		{
			get { return messageContractElementNamespace; }
			set { messageContractElementNamespace = value; }
		}

		protected virtual Type CompileAndGetType(string content)
		{
			EnsureNamespace(ref content);
			string typeName = DefaultNamespace + "." + MessageContractElementName;
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(content);

			Type generatedType = results.CompiledAssembly.GetType(typeName, false);
			
            Assert.IsNotNull(generatedType, "Invalid type: " + typeName);
			return generatedType;
		}

		protected virtual T CreateRoot<T>(string name, string ns) where T: MessageBase
		{
			T rootElement = (T)Activator.CreateInstance(typeof(T), Store);
			rootElement.ServiceContractModel = new ServiceContractModel(Store);
			rootElement.Name = name;
			rootElement.Namespace = ns;
			return rootElement;
		}
	}
}
