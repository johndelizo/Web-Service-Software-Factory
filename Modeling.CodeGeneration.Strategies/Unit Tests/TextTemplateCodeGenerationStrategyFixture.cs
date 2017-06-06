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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.UnitTestLibrary;
using System.Collections.Generic;
using Microsoft.Practices.VisualStudio.Helper;
using System.IO;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.Tests
{
	[TestClass]
	public class TextTemplateCodeGenerationStrategyFixture
	{
		Store store;
		Partition partition;
		Transaction transaction;

		[TestInitialize]
		public void TestInitialize()
		{
			store = new Store(new Type[] { typeof(MockDomainModel) });
			partition = new Partition(store);
			transaction = store.TransactionManager.BeginTransaction();
		}

		[TestCleanup]
		public void TestCleanup()
		{
			if (transaction != null)
			{
				transaction.Rollback();
			}
		}


		[TestMethod]
		public void TestMyTemplateGetsCalled()
		{
			string message = "Hello World";
			ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, message);
			MyArtifactLink link = CreateLink(myModelElement);
			
			TextTemplateCodeGenerationStrategy strategy = new TextTemplateCodeGenerationStrategy();
			strategy.ResourceResolver = link;
			IDictionary<string, string> result = strategy.Generate(link);

			Assert.AreEqual<string>(message, result[link.ItemPath]);
		}

		[TestMethod]
		public void ShouldResolveToVBTemplateWhenProjectIsVB()
		{
			string message = "Hello World";
			ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, message);
			MyArtifactLink link = CreateLink(myModelElement, EnvDTE.CodeModelLanguageConstants.vsCMLanguageVB);
			TextTemplateCodeGenerationStrategy strategy = new TextTemplateCodeGenerationStrategy();
			strategy.ResourceResolver = link;
			IDictionary<string, string> result = strategy.Generate(link);

			StringAssert.Contains(result[link.ItemPath], @"Generated from VB");
		}
		
		[TestMethod]
		public void ShouldSelectAnyTextTemplateIfLanguageSpecificNotAvailable()
		{
			string message = "Hello World";
			ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, message);
			ProjectNode project = GetTestProjectNode(EnvDTE.CodeModelLanguageConstants.vsCMLanguageVB);
			MyLanguageIndependentArtifactLink link = new MyLanguageIndependentArtifactLink(myModelElement);
			Utility.SetData<IServiceProvider>(link, new MockServiceProvider());
			Utility.SetData<ProjectNode>(link, project);
			TextTemplateCodeGenerationStrategy strategy = new TextTemplateCodeGenerationStrategy();
			strategy.ResourceResolver = link;
			IDictionary<string, string> result = strategy.Generate(link);

			Assert.AreEqual<string>("NoTemplate.any.tt", result[link.ItemPath]);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldGenerateErrorIfNoApplicableTemplate()
		{
			string message = "Hello World";
			ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, message);

			// CSharp language project should look for C# template then any, but link only has VB.
			ProjectNode project = GetTestProjectNode(EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp);

			VbOnlyArtifactLink link = new VbOnlyArtifactLink(myModelElement);
			Utility.SetData<IServiceProvider>(link, new MockServiceProvider());
			Utility.SetData<ProjectNode>(link, project);

			TextTemplateCodeGenerationStrategy strategy = new TextTemplateCodeGenerationStrategy();
			strategy.ResourceResolver = link;
			strategy.Generate(link);
		}

		[TestMethod]
		public void ShouldGetAssemblyReferences()
		{
			string message = "Hello World";
			ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, message);
			ProjectNode project = GetTestProjectNode();
			AssemblyReferencesArtifactLink link = new AssemblyReferencesArtifactLink(myModelElement);
			Utility.SetData<IServiceProvider>(link, new MockServiceProvider());
			Utility.SetData<ProjectNode>(link, project);
			TextTemplateCodeGenerationStrategy strategy = new TextTemplateCodeGenerationStrategy();
			strategy.ResourceResolver = link;
			strategy.Generate(link);

			Assert.AreEqual<int>(2, strategy.AssemblyReferences.Count);
		}

		[TestMethod]
		public void TestExceptionsGetLogged()
		{
			string message = "Hello World";
			ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, message);
			ProjectNode project = GetTestProjectNode();
			ErrorArtifactLink link = new ErrorArtifactLink(myModelElement);
			Utility.SetData<IServiceProvider>(link, new MockServiceProvider());
			Utility.SetData<ProjectNode>(link, project);
			TextTemplateCodeGenerationStrategy strategy = new TextTemplateCodeGenerationStrategy();
			strategy.ResourceResolver = link;
			IDictionary<string, string> result = strategy.Generate(link);
			
			Assert.IsFalse(result.Values.Contains(typeof(InvalidOperationException).Name));
			Assert.IsTrue(result.Values.Contains(message));
			Assert.AreEqual<int>(2, strategy.Errors.Count);
			Assert.IsTrue(strategy.Errors[1].Message.Contains(message));
		}

		[TestMethod]
		public void TestCompileErrorsGetLogged()
		{
			string message = "Foo";
			ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, message);
			ProjectNode project = GetTestProjectNode();
			CompileErrorArtifactLink link = new CompileErrorArtifactLink(myModelElement);
			Utility.SetData<IServiceProvider>(link, new MockServiceProvider());
			Utility.SetData<ProjectNode>(link, project);
			TextTemplateCodeGenerationStrategy strategy = new TextTemplateCodeGenerationStrategy();
			strategy.ResourceResolver = link;
			IDictionary<string, string> result = strategy.Generate(link);
			
			Assert.AreEqual<int>(1, strategy.Errors.Count);
			Assert.IsTrue(strategy.Errors[0].Message.Contains(message));
		}

		[TestMethod]
		public void TestCancelOutputGetsCalled()
		{
			ExtensibleMockModelElement myModelElement = new ExtensibleMockModelElement(partition, "");
			ProjectNode project = GetTestProjectNode();
			CancelOutputArtifactLink link = new CancelOutputArtifactLink(myModelElement);
			Utility.SetData<IServiceProvider>(link, new MockServiceProvider());
			Utility.SetData<ProjectNode>(link, project);
			TextTemplateCodeGenerationStrategy strategy = new TextTemplateCodeGenerationStrategy();
			strategy.ResourceResolver = link;
			IDictionary<string, string> result = strategy.Generate(link);

			Assert.AreEqual<int>(0, result.Count);
		}

		private MyArtifactLink CreateLink(ExtensibleMockModelElement element)
		{
			return CreateLink(element, EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp);
		}

		private MyArtifactLink CreateLink(ExtensibleMockModelElement element, string lang)
		{
			MyArtifactLink link = new MyArtifactLink(element);
			link.Data.Add(typeof(ProjectNode).FullName, GetTestProjectNode(lang));
			link.Data.Add(typeof(IServiceProvider).FullName, new MockServiceProvider());
			return link;
		}

		#region ErrorArtifactLink class

		internal class ErrorArtifactLink : MyArtifactLink
		{
			string message;

			public ErrorArtifactLink(ExtensibleMockModelElement modelElement)
				: base(modelElement)
			{
				message = modelElement.Message;
			}

			public override string GetResource(string resourceItem)
			{
				return @"<#=""" + message + @"""#><# throw new InvalidOperationException(""" + message + @"""); #>";
			}
		}

		#endregion

		#region CancelOutputArtifactLink class

		internal class CancelOutputArtifactLink : MyArtifactLink
		{
			public CancelOutputArtifactLink(ExtensibleMockModelElement modelElement)
				: base(modelElement)
			{
			}

			public override string GetResource(string resourceItem)
			{
				return @"<#@ Template Language=""C#"" Inherits=""Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating.ModelingTextTransformation"" #>" +
						@"<#@ ModelInjector processor=""ModelInjectorDirectiveProcessor"" #>" +
						@"<# CancelOutput(); #>";
			}
		}

		#endregion

		#region CompileErrorArtifactLink

		internal class CompileErrorArtifactLink : MyArtifactLink
		{
			string funcName;

			public CompileErrorArtifactLink(ExtensibleMockModelElement modelElement)
				: base(modelElement)
			{
				funcName = modelElement.Message;
			}

			public override string GetResource(string resourceItem)
			{
				return @"<# " + funcName + "(); #>";
			}
		}

		#endregion

		#region AssemblyReferencesArtifactLink

		[AssemblyReference("System.Runtime.Serialization, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
		[AssemblyReference("System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
		internal class AssemblyReferencesArtifactLink : MyArtifactLink
		{
			public AssemblyReferencesArtifactLink(ExtensibleMockModelElement modelElement)
				: base(modelElement)
			{
			}
		}

		#endregion

		#region MyArtifactLink class

		[TextTemplate("NoTemplate.cs.tt", TextTemplateTargetLanguage.CSharp)]
		[TextTemplate("NoTemplate.vb.tt", TextTemplateTargetLanguage.VB)]
		internal class MyArtifactLink : IArtifactLink, IModelReference, IResourceResolver
		{
			public MyArtifactLink(ExtensibleMockModelElement modelElement)
			{
				this.modelElement = modelElement;
			}

			#region IArtifactLink Members

			public Guid Container
			{
				get { throw new Exception("The method or operation is not implemented."); }
				set { throw new Exception("The method or operation is not implemented."); }
			}

			public string ItemPath
			{
				get { return "foo.cs"; }
				set { throw new Exception("The method or operation is not implemented."); }
			}

			private IDictionary<string, object> data = new Dictionary<string, object>();

			public IDictionary<string, object> Data
			{
				get { return data; }
			}

			#endregion

			#region IModelReference Members

			ModelElement modelElement = null;

			public ModelElement ModelElement
			{
				get { return modelElement; }
			}

			#endregion

			#region IResourceResolver Members

			public virtual string GetResourcePath(string resourceItem)
			{
				return resourceItem;
			}

			public virtual string GetResource(string resourceItem)
			{

				switch (resourceItem)
				{
					case "NoTemplate.cs.tt":
						return @"<#@ Template Language=""C#"" Inherits=""Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating.ModelingTextTransformation"" #>" +
								@"<#@ ModelInjector processor=""ModelInjectorDirectiveProcessor"" #>" +
								@"<#= RootElement.Message #>";
					case "NoTemplate.vb.tt":
						return @"<#@ Template Debug=""true"" Language=""C#"" Inherits=""Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating.ModelingTextTransformation"" #>" +
						@"<#@ ModelInjector processor=""ModelInjectorDirectiveProcessor"" #>" +
						@"<#= ""//Generated from VB Template"" #>" +
						@"<#= RootElement.Message#>";
					default:
						return string.Empty;
				}
			}

			#endregion
		}

		#endregion

		#region MyLanguageIndependentArtifactLink class

		[TextTemplate("NoTemplate.any.tt", TextTemplateTargetLanguage.Any)]
		internal class MyLanguageIndependentArtifactLink : IArtifactLink, IModelReference, IResourceResolver
		{
			public MyLanguageIndependentArtifactLink(ExtensibleMockModelElement modelElement)
			{
				this.modelElement = modelElement;
			}

			#region IArtifactLink Members

			public Guid Container
			{
				get { throw new Exception("The method or operation is not implemented."); }
				set { throw new Exception("The method or operation is not implemented."); }
			}

			public string ItemPath
			{
				get { return "foo.cs"; }
				set { throw new Exception("The method or operation is not implemented."); }
			}

			private IDictionary<string, object> data = new Dictionary<string, object>();

			public IDictionary<string, object> Data
			{
				get { return data; }
			}

			#endregion

			#region IModelReference Members

			ModelElement modelElement = null;

			public ModelElement ModelElement
			{
				get { return modelElement; }
			}

			#endregion

			#region IResourceResolver Members

			public virtual string GetResourcePath(string resourceItem)
			{
				return resourceItem;
			}

			public virtual string GetResource(string resourceItem)
			{

				return @"<#@ Template Language=""C#"" Inherits=""Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating.ModelingTextTransformation"" #>" +
						@"<#@ ModelInjector processor=""ModelInjectorDirectiveProcessor"" #>" +
						@"<#= """ + resourceItem + @""" #>";
			}

			#endregion
		}

		#endregion

		#region VbOnlyArtifactLink class

		[TextTemplate("NoTemplate.vb.tt", TextTemplateTargetLanguage.VB)]
		internal class VbOnlyArtifactLink : IArtifactLink, IModelReference, IResourceResolver
		{
			public VbOnlyArtifactLink(ExtensibleMockModelElement modelElement)
			{
				this.modelElement = modelElement;
			}

			#region IArtifactLink Members

			public Guid Container
			{
				get { throw new Exception("The method or operation is not implemented."); }
				set { throw new Exception("The method or operation is not implemented."); }
			}

			public string ItemPath
			{
				get { return "foo.cs"; }
				set { throw new Exception("The method or operation is not implemented."); }
			}

			private IDictionary<string, object> data = new Dictionary<string, object>();

			public IDictionary<string, object> Data
			{
				get { return data; }
			}

			#endregion

			#region IModelReference Members

			ModelElement modelElement = null;

			public ModelElement ModelElement
			{
				get { return modelElement; }
			}

			#endregion

			#region IResourceResolver Members

			public virtual string GetResourcePath(string resourceItem)
			{
				return resourceItem;
			}

			public virtual string GetResource(string resourceItem)
			{

				return @"<#@ Template Language=""C#"" Inherits=""Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating.ModelingTextTransformation"" #>" +
						@"<#@ ModelInjector processor=""ModelInjectorDirectiveProcessor"" #>" +
						@"<#= """ + resourceItem + @""" #>";
			}

			#endregion
		}

		#endregion

		private static ProjectNode GetTestProjectNode()
		{
			return GetTestProjectNode(EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp);
		}

		private static ProjectNode GetTestProjectNode(string language)
		{
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution vsSolution = new MockVsSolution(root);
			MockVSHierarchy project = new MockVSHierarchy("Project.project");

			MockEnvDTEProject mockEnvDteProject = project.ExtObject as MockEnvDTEProject;
			if (mockEnvDteProject != null)
			{
				mockEnvDteProject.SetCodeModel(new MockCodeModel(language));
			}
			root.AddProject(project);

			ProjectNode projectNode = new ProjectNode(vsSolution, project.GUID);

			return projectNode;
		}

	}
}
