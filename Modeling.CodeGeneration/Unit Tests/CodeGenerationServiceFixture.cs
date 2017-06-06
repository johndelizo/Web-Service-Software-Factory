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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.VisualStudio.Helper;

namespace Microsoft.Practices.Modeling.CodeGeneration.Tests
{
	[TestClass]
	public class CodeGenerationServiceFixture
	{
		private const string ItemPath = "MyItem.cs";
		private MockVsRDT rdt;

		[TestInitialize]
		public void TestInitialize()
		{
			rdt = null;
		}

		[TestCleanup]
		public void TestCleanup()
		{
			if (File.Exists(GetItemPath()))
			{
				File.Delete(GetItemPath());
			}
		}

		[TestMethod]
		public void TestMyCodeGeneratorGetsCalled()
		{
			MyArtifactLink link = new MyArtifactLink(ItemPath);
			CodeGenerationService codeGenerator = CreateCodeGenerator(ItemPath, out rdt);
			
			Assert.IsTrue(codeGenerator.GenerateArtifact(link) > 0);
			Assert.AreEqual<string>(MyCodeGenerator.HelloWorld, GetGeneratedCode());
		}

		[TestMethod]
		public void TestCreatedItemGeneratesErrorMessageInContent()
		{
			NoStrategyArtifactLink link = new NoStrategyArtifactLink(ItemPath);
			CodeGenerationService codeGenerator = CreateCodeGenerator(ItemPath, out rdt);
			
			Assert.IsFalse(codeGenerator.GenerateArtifact(link) > 0);
			Assert.AreEqual<string>("An unexpected exception has occurred while generating code. Check the Error List Window.", GetGeneratedCode());
		}

		[TestMethod]
		public void ShouldNotGenerateContentWithErrorsOnConfigFiles()
		{
			const string configFile = "foo.config";
			NoStrategyArtifactLink link = new NoStrategyArtifactLink(configFile);
			CodeGenerationService codeGenerator = CreateCodeGenerator(configFile, out rdt);

			Assert.IsFalse(codeGenerator.GenerateArtifact(link) > 0);
			Assert.AreEqual<string>("", GetGeneratedCode(configFile));
		}

		[TestMethod]
		public void TestValidate()
		{
			MockServiceProvider serviceProvider = new MockServiceProvider();
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			MockVSHierarchy project = new MockVSHierarchy("Project1.project");
			root.AddProject(project);
			serviceProvider.AddService(typeof(SVsSolution), solution);
			CodeGenerationService target = new CodeGenerationService(serviceProvider);
			MyArtifactLink validLink = new MyArtifactLink(project.GUID, "item1.cs");

			Assert.IsTrue(target.IsValid(validLink));
			MyArtifactLink invalidLink1 = new MyArtifactLink(Guid.NewGuid(), "item2.cs");
			Assert.IsFalse(target.IsValid(invalidLink1));
			MyArtifactLink invalidLink2 = new MyArtifactLink(project.GUID, "it:em3.cs");
			Assert.IsFalse(target.IsValid(invalidLink2));
			MyArtifactLink invalidLink3 = new MyArtifactLink(Guid.NewGuid(), "<item3.cs>");
			Assert.IsFalse(target.IsValid(invalidLink3));
		}

		[TestMethod]
		public void TestArtifactIsGenerated()
		{
			MockServiceProvider serviceProvider = new MockServiceProvider();
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			MockVSHierarchy project = new MockVSHierarchy("Project1.project");
			root.AddProject(project);
			serviceProvider.AddService(typeof(SVsSolution), solution);
			CodeGenerationService target = new CodeGenerationService(serviceProvider);
			MyArtifactLink validLink = new MyArtifactLink(project.GUID, "item1.cs");

			Assert.IsFalse(target.IsArtifactAlreadyGenerated(validLink));
			project.AddChild("item1.cs");
			Assert.IsTrue(target.IsArtifactAlreadyGenerated(validLink));
		}

		[TestMethod]
		public void TestValidateDelete()
		{
			MockServiceProvider serviceProvider = new MockServiceProvider();
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			MockVSHierarchy project = new MockVSHierarchy("Project1.project");
			root.AddProject(project);
			serviceProvider.AddService(typeof(SVsSolution), solution);
			TestableCodeGenerationService target = new TestableCodeGenerationService(serviceProvider);
			MyArtifactLink validLink = new MyArtifactLink(project.GUID, "item1.cs");
			HierarchyNode node = target.ValidateDelete(validLink);

			Assert.IsNull(node);
			Assert.AreEqual<int>(0, target.LogEntries.Count);
			project.AddChild("item1.cs");
			node = target.ValidateDelete(validLink);
			Assert.IsNotNull(node);
			Assert.AreEqual<int>(1, target.LogEntries.Count);
		}

		[TestMethod]
		public void TestValidateRename()
		{
			MockServiceProvider serviceProvider = new MockServiceProvider();
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			MockVSHierarchy project = new MockVSHierarchy("Project1.project");
			root.AddProject(project);
			serviceProvider.AddService(typeof(SVsSolution), solution);
			TestableCodeGenerationService target = new TestableCodeGenerationService(serviceProvider);
			string oldName = "item1";
			string newName = "item2";
			MyArtifactLink validLink = new MyArtifactLink(project.GUID, oldName+".cs");
			HierarchyNode node = target.ValidateRename(validLink,newName,oldName);

			Assert.IsNull(node);
			project.AddChild(oldName+".cs");
			node = target.ValidateRename(validLink,oldName,oldName);
			Assert.IsNull(node);
			Assert.AreEqual<int>(0, target.LogEntries.Count);
			node = target.ValidateRename(validLink, newName, oldName);
			Assert.IsNotNull(node);
			Assert.AreEqual<int>(1, target.LogEntries.Count);
		}

		[TestMethod]
		public void TestValidateEmptyRename()
		{
			MockServiceProvider serviceProvider = new MockServiceProvider();
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			MockVSHierarchy project = new MockVSHierarchy("Project1.project");
			root.AddProject(project);
			serviceProvider.AddService(typeof(SVsSolution), solution);
			TestableCodeGenerationService target = new TestableCodeGenerationService(serviceProvider);
			string oldName = "item1";
			string newName = "";
			MyArtifactLink validLink = new MyArtifactLink(project.GUID, oldName + ".cs");
			HierarchyNode node = target.ValidateRename(validLink, newName, oldName);

			Assert.IsNull(node);
			project.AddChild(oldName + ".cs");
			node = target.ValidateRename(validLink, oldName, oldName);
			Assert.IsNull(node);
			Assert.AreEqual<int>(0, target.LogEntries.Count);
			node = target.ValidateRename(validLink, newName, oldName);
			Assert.IsNotNull(node);
			Assert.AreEqual<int>(1, target.LogEntries.Count);
		}

		[TestMethod]
		public void ShouldLogErrorIfNoCodeGenerationStrategy()
		{
			MockVsRDT rdt;
			TestableCodeGenerationService target = CreateCodeGenerator(ItemPath, out rdt) as TestableCodeGenerationService;

			Assert.AreEqual<int>(0, target.LogEntries.Count);
			target.GenerateArtifact(new NoStrategyArtifactLink(ItemPath));

			Assert.AreEqual<int>(1, target.LogEntries.Count);
			foreach (LogEntry entry in target.LogEntries)
			{
				Assert.AreEqual<string>("No CodeGenerationStrategyAttribute found on IArtifactLink object.", entry.Message);
			}
		}

		[TestMethod]
		public void TestEmptyCodeGenerator()
		{
			EmptyArtifactLink link = new EmptyArtifactLink(ItemPath);
			CodeGenerationService codeGenerator = CreateCodeGenerator(ItemPath, out rdt);

			Assert.IsFalse(codeGenerator.GenerateArtifact(link) > 0);
			Assert.AreEqual<string>("", GetGeneratedCode());
		}

		[TestMethod]
		public void TestWithDuplicateLink()
		{
			List<IArtifactLink> links = new List<IArtifactLink>();
			links.Add(new MyArtifactLink(ItemPath));
			links.Add(new MyArtifactLink(ItemPath));
			CodeGenerationService codeGenerator = CreateCodeGenerator(ItemPath, out rdt);

			Assert.AreEqual<int>(1, codeGenerator.GenerateArtifacts(links));
		}

		[TestMethod]
		public void ShouldGenerateOnceWithMultipleSameFiles()
		{
			MyArtifactLink link = new MyArtifactLink(ItemPath);
			CodeGenerationService codeGenerator = CreateCodeGenerator(ItemPath, out rdt);

			Assert.AreEqual<int>(1, codeGenerator.GenerateArtifact(link));
			Assert.AreEqual<int>(0, codeGenerator.GenerateArtifact(link));
			Assert.AreEqual<string>(MyCodeGenerator.HelloWorld, GetGeneratedCode());
		}

		#region Private Implementation

		private string GetGeneratedCode()
		{
			return GetGeneratedCode(ItemPath);
		}

		private string GetGeneratedCode(string path)
		{
			string result = rdt.GetEntry(path).docData.Buffer.ToString();
			if (string.IsNullOrEmpty(result) && 
				File.Exists(GetItemPath()))
			{
				result = File.ReadAllText(GetItemPath());
			}
			return result;
		}

		private string GetItemPath()
		{
			return Path.Combine(Directory.GetCurrentDirectory(), ItemPath);
		}

		#endregion

		private static CodeGenerationService CreateCodeGenerator(string itemPath, out MockVsRDT rdt)
		{
			string fullItemPath = Path.Combine(Directory.GetCurrentDirectory(), itemPath);
			string fullProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "Project.project");
			MockServiceProvider serviceProvider = new MockServiceProvider();
			MockVSHierarchy vsHierarchy = new MockVSHierarchy(fullProjectPath);
			MockVsSolution vsSolution = new MockVsSolution(vsHierarchy);
			rdt = new MockVsRDT(fullItemPath, vsHierarchy, 0);
			MockVsTextManager textManager = new MockVsTextManager();
            MockVsUIShellOpenDocument shellOpenDocument = new MockVsUIShellOpenDocument();
			serviceProvider.AddService(typeof(SVsSolution), vsSolution);
			serviceProvider.AddService(typeof(IVsRunningDocumentTable), rdt);
			serviceProvider.AddService(typeof(VsTextManagerClass), textManager);
            serviceProvider.AddService(typeof(SVsUIShellOpenDocument), shellOpenDocument);
			return new TestableCodeGenerationService(serviceProvider);
		}

		#region TestableCodeGenerationService class 

		public class TestableCodeGenerationService : CodeGenerationService
		{
			List<LogEntry> logEntries;

			public TestableCodeGenerationService(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				logEntries = new List<LogEntry>();
			}

			protected override void LogEntry(LogEntry entry)
			{
				logEntries.Add(entry);
			}

			public ICollection<LogEntry> LogEntries
			{
				get { return logEntries; }
			}
		}

		#endregion
		
		#region NoStrategyArtifactLink

		internal class NoStrategyArtifactLink : IArtifactLink
		{
			private string itemPath;

			public NoStrategyArtifactLink(string itemPath)
			{
				this.container = Guid.Empty;
				this.itemPath = itemPath;
			}

			#region IArtifactLink Members

			Guid container;

			public Guid Container
			{
				get { return container; }
				set { container = value; }
			}

			public string ItemPath
			{
				get { return itemPath; }
				set { itemPath = value; }
			}

			IDictionary<string, object> data = new Dictionary<string, object>();

			public IDictionary<string, object> Data
			{
				get { return data; }
			}

			#endregion
		}

		#endregion

		#region MyArtifactLink

		[CodeGenerationStrategy(typeof(MyCodeGenerator))]
		internal class MyArtifactLink : IArtifactLink
		{
			private string itemPath;

			public MyArtifactLink(string itemPath)
				: this(Guid.Empty, itemPath)
			{
			}

			public MyArtifactLink(Guid container, string itemPath)
			{
				this.container = container;
				this.itemPath = itemPath;
			}

			#region IArtifactLink Members

			Guid container;

			public Guid Container
			{
				get { return container; }
				set { container = value; }
			}

			public string ItemPath
			{
				get { return itemPath; }
				set { itemPath = value; }
			}

			private IDictionary<string, object> data = new Dictionary<string, object>();

			public IDictionary<string, object> Data
			{
				get { return data; }
			}

			#endregion

			public override bool Equals(object obj)
			{
				IArtifactLink other = obj as IArtifactLink;
				if (other == null)
				{
					return false;
				}
				return this.Container == other.Container &&
					   this.ItemPath == other.ItemPath;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		#endregion

		#region MyCodeGenerator class

		internal class MyCodeGenerator : ICodeGenerationStrategy
		{
			public const string HelloWorld = "Hello World";
			private IList<Guid> projectReferences;
			private IList<string> assemlyReferences;
			private IList<LogEntry> errors;

			public MyCodeGenerator()
			{
				projectReferences = new List<Guid>();
				assemlyReferences = new List<string>();
				errors = new List<LogEntry>();
			}

			public bool Overwrite
			{
				get { return true; }
			}

			public bool CanCreateItem
			{
				get { return false; }
			}

			public uint CreateItem(IVsHierarchy hierarchy, uint itemId)
			{
				return itemId;
			}

			public virtual CodeGenerationResults Generate(IArtifactLink link)
			{
				CodeGenerationResults result = new CodeGenerationResults();
				result.Add(link.ItemPath, HelloWorld);
				return result;
			}

			public IList<LogEntry> Errors
			{
				get { return errors; }
			}

			public IList<Guid> ProjectReferences
			{
				get { return projectReferences; }
			}

			public IList<string> AssemblyReferences
			{
				get { return assemlyReferences; }
			}
		}

		#endregion

		#region EmptyArtifactLink class 

		[CodeGenerationStrategy(typeof(EmptyCodeGenerator))]
		internal class EmptyArtifactLink : MyArtifactLink
		{
			public EmptyArtifactLink(string itemPath) : base(itemPath) { }
		}

		#endregion

		#region EmptyCodeGenerator class

		internal class EmptyCodeGenerator : MyCodeGenerator
		{
			public override CodeGenerationResults Generate(IArtifactLink link)
			{
				return new CodeGenerationResults();
			}
		}

		#endregion
	}
}
