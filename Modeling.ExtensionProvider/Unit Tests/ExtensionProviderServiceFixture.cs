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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Modeling.ExtensionProvider.Services;
using Microsoft.Practices.ServiceFactory.DataContracts;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.Common;
using System.Reflection;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Tests
{
	[TestClass]
	public class ExtensionProviderServiceFixture
	{
		[TestMethod]
		public void ServiceShouldReturnAllLoadedExtenders()
		{
			Type generatedType = GenerateTestAssemblyAndGetType("ServiceShouldReturnLoadedExtenders.TestCode");
			IList<Type> typeCache = new List<Type>();
			typeCache.Add(generatedType);

			IExtensionProviderService target = new ExtensionProviderService(typeCache);
			IList<IExtensionProvider> result = target.ExtensionProviders;

			Assert.AreEqual<int>(1, result.Count, "More than one extension provider found.");
			Assert.AreEqual<string>(generatedType.Name, result[0].GetType().Name, "Types do not match");
		}

		private Type GenerateTestAssemblyAndGetType(string assemblyName)
		{
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(
				@"
				using System;
				using System.Collections.Generic;
				using Microsoft.Practices.Modeling;
				using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
				using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;

				namespace " + assemblyName + @"
				{
					[ExtensionProviderAttribute(""74ECFD8C-4906-4a71-9B23-DCB43C26928A"", ""TestExt"", ""Test Extension"", typeof(Object))]
					public class TestExtensionProvider : ExtensionProviderBase
					{
						public override IList<Type> ObjectExtenders
						{
							get { return new List<Type>(); }
						}				
					}
				}",
				assemblyName);

			Assert.IsFalse(results.Errors.HasErrors, "Errors in CompileAssemblyFromSource: " + (results.Errors.HasErrors ? results.Errors[0].ErrorText : ""));
			Type generatedType = results.CompiledAssembly.GetType( assemblyName + ".TestExtensionProvider", false);
			Assert.IsTrue(File.Exists(results.PathToAssembly));

			return generatedType;
		}
	}
}
