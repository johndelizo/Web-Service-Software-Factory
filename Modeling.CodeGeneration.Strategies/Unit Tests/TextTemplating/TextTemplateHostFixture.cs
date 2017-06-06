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
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies.Tests.Mocks;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.Tests.TextTemplating
{
	[TestClass]
	public class TextTemplatHostFixture
	{
		[TestMethod]
		public void NewInstanceWithDefaultValues()
		{
			TextTemplateHost host = new TextTemplateHost(null, null, null);
			
			Assert.AreEqual(0, host.ProjectReferences.Count);
			Assert.IsNull(host.CurrentElement);
			Assert.IsNull(host.CurrentExtender);
			Assert.IsNull(host.Model);
			Assert.IsNull(host.RootElement);
			Assert.AreEqual(14, host.StandardAssemblyReferences.Count);
			Assert.AreEqual(3, host.StandardImports.Count);
		}

		[TestMethod]
		public void InvokeHostSimpleTemplate()
		{
			Engine engine = new Engine();
			string result = engine.ProcessTemplate("Hello World", new TextTemplateHost(null, null, null));
			Assert.AreEqual("Hello World", result);
		}

		[TestMethod]
		public void InvokeHostWithModel()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition,string.Empty);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel,null);
				string templateContent = GetStandardTemplateHeader() + @"
					<#= this.Model.DomainModelInfo.Id #>";

				string transformResult = engine.ProcessTemplate(
					templateContent,
					host);
				Assert.AreEqual(domainModel.DomainModelInfo.Id, new Guid(transformResult));
				t.Rollback();
			}
		}

		[TestMethod]
		public void CanAddProjectReference()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, "test");
				serviceModel.ObjectExtender = new MockObjectExtender();
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, serviceModel);
				host.StandardAssemblyReferences.Add(typeof(Microsoft.Practices.Modeling.ExtensionProvider.Extension.ObjectExtender<ExtensibleMockModelElement>).Assembly.FullName);
				
				string transformResult = engine.ProcessTemplate(
					GetStandardTemplateHeader() + @"
					<# AddProjectReference(CurrentExtender.ArtifactLink); #>",
					host);
				Assert.AreEqual(1, host.ProjectReferences.Count);
				t.Rollback();
			}
		}

		[TestMethod]
		public void CanFormatToCamelCase()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, string.Empty);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, null);
				string transformResult = engine.ProcessTemplate(
					GetStandardTemplateHeader() + @"
					<#= Utility.ToCamelCase(""MyData"") #>",
					host);
				Assert.IsTrue(transformResult.Contains("myData"));
				t.Rollback();
			}
		}

		[TestMethod]
		public void CanFormatToPascalCase()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, string.Empty);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, null);
				string transformResult = engine.ProcessTemplate(
					GetStandardTemplateHeader() + @"
					<#= Utility.ToPascalCase(""myData"") #>",
					host);
				Assert.IsTrue(transformResult.Contains("MyData"));
				t.Rollback();
			}
		}

		[TestMethod]
		public void CanGetCSharpTypeOutput()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, string.Empty);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, null);
				string transformResult = engine.ProcessTemplate(
					GetStandardTemplateHeader() + @"
					<#= Utility.GetCSharpTypeOutput(""System.String"") #>",
					host);
				Assert.IsTrue(transformResult.Contains("string"));
				t.Rollback();
			}
		}

		[TestMethod]
		public void HostReturnsErrorsInCollection()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, string.Empty);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, null);
				string transformResult = engine.ProcessTemplate(
					GetStandardTemplateHeader() + @"
					<# throw new global::System.Exception(""TestException""); #>",
					host);

				Assert.AreEqual<int>(2, host.CompilerErrors.Count);
				Assert.IsTrue(host.CompilerErrors[1].ErrorText.Contains("TestException"),"Could not find expected exception in compiler errors."); 

				t.Rollback();
			}
		}

		[TestMethod]
		public void HostReturnsErrorsFromLogCall()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			const string LogMessage = "Message1";
			const string LogTitle = "Title1";

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, string.Empty);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, null);
				string transformResult = engine.ProcessTemplate(
					GetStandardTemplateHeader() + @"
					<# LogError(""" +LogMessage+@""","""+LogTitle+@"""); #>",
					host);
				Assert.AreEqual<int>(1, host.CompilerErrors.Count);
				Assert.IsFalse(host.CompilerErrors[0].IsWarning);
				Assert.IsTrue(host.CompilerErrors[0].ErrorNumber.Contains(LogMessage), "Could not find expected error in compiler errors.");
				Assert.IsTrue(host.CompilerErrors[0].ErrorText.Contains(LogTitle), "Could not find expected error in compiler errors.");

				t.Rollback();
			}
		}

		[TestMethod]
		public void HostReturnsWarningsFromLogCall()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			const string LogMessage = "Message1";
			const string LogTitle = "Title1";

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, string.Empty);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, null);
				string transformResult = engine.ProcessTemplate(
					GetStandardTemplateHeader() + @"
					<# LogWarning(""" + LogMessage + @""",""" + LogTitle + @"""); #>",
					host);
				Assert.AreEqual<int>(1, host.CompilerErrors.Count);
				Assert.IsTrue(host.CompilerErrors[0].IsWarning);
				Assert.IsTrue(host.CompilerErrors[0].ErrorNumber.Contains(LogMessage), "Could not find expected error in compiler errors.");
				Assert.IsTrue(host.CompilerErrors[0].ErrorText.Contains(LogTitle), "Could not find expected error in compiler errors.");

				t.Rollback();
			}
		}

		[TestMethod]
		public void TestIsValidMethod()
		{
			Assert.AreEqual<string>(false.ToString(), IsValid(false));
			Assert.AreEqual<string>(true.ToString(), IsValid(true));
		}

		[TestMethod]
		public void HostReturnsEmptyContentOnCancelOutput()
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, string.Empty);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, null);
				string templateContent = GetStandardTemplateHeader() + @"<# CancelOutput(); #>";

				string transformResult = engine.ProcessTemplate(templateContent, host);

				Assert.AreEqual<int>(0, host.CompilerErrors.Count);
				Assert.IsFalse(host.GenerateOutput);

				t.Rollback();
			}
		}

		private string GetStandardTemplateHeader()
		{
			return @"<#@ Template Language=""C#"" Inherits=""" + typeof(ModelingTextTransformation).FullName + @"""  #>
					<#@ ModelInjector processor=""ModelInjectorDirectiveProcessor"" #>";
		}


		private string IsValid(bool expectedValue)
		{
			Engine engine = new Engine();
			Store store = new Store(typeof(MockDomainModel));
			MockDomainModel domainModel = GetModel(store);

			using (Transaction t = store.TransactionManager.BeginTransaction())
			{
				ExtensibleMockModelElement serviceModel = new ExtensibleMockModelElement(store.DefaultPartition, string.Empty);
				MockCodeGenerationService cgs = new MockCodeGenerationService(expectedValue);
				TextTemplateHost host = new TextTemplateHost(domainModel, serviceModel, serviceModel, cgs);
				string transformResult = engine.ProcessTemplate(
					GetStandardTemplateHeader().Replace("/n","") + @"<#= this.IsValid(CurrentElement.InvalidArtifactLink).ToString()#>",
					host);
				t.Rollback();
				return transformResult.Trim();
			}
		}


		private MockDomainModel GetModel(Store store)
		{
			MockDomainModel domainModel = null;
			foreach (DomainModel dm in store.DomainModels)
			{
				if (dm is MockDomainModel)
				{
					domainModel = (MockDomainModel)dm;
				}
			}
			Assert.IsNotNull(domainModel);
			return domainModel;
		}
	}
}
