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
using System.Reflection;
using System.IO;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.CodeDom.Compiler;
using System.Security.AccessControl;
using System.Threading;
using System.Diagnostics;
using Microsoft.Practices.Modeling.Common.Logging;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests
{
	[TestClass]
	public class AssemblyLoaderFixture
	{
        [TestInitialize]
        public void Initialize()
        {
            ResetLibrary();
            AssemblyLoader.ResetCache();
            Logger.SuspendRefresh();
        }
        
        [TestMethod]
		public void AssemblyLoaderLoadsAnAssemblyInLibraryFolder()
		{
			string assemblyName = "AssemblyLoaderLoadsAnAssemblyInLibraryFolder.TestCode";

			GenerateTestAssembly(assemblyName);
			Assert.IsFalse(IsAssemblyLoaded(assemblyName));

			AssemblyLoader.LoadAll(AppDomain.CurrentDomain.BaseDirectory);

			Assert.IsTrue(IsAssemblyLoaded(assemblyName));
		}

		[TestMethod]
		public void AssemblyLoaderLoadsMultipleAssembliesInLibraryFolder()
		{
			string assemblyName1 = "AssemblyLoaderLoadsMultipleAssembliesInLibraryFolder.TestCode";
			GenerateTestAssembly(assemblyName1);
			Assert.IsFalse(IsAssemblyLoaded(assemblyName1));

			string assemblyName2 = "AssemblyLoaderLoadsMultipleAssembliesInLibraryFolder2.TestCode";
			GenerateTestAssembly(assemblyName2);
			Assert.IsFalse(IsAssemblyLoaded(assemblyName2));

			AssemblyLoader.LoadAll(AppDomain.CurrentDomain.BaseDirectory);

			Assert.IsTrue(IsAssemblyLoaded(assemblyName1));
			Assert.IsTrue(IsAssemblyLoaded(assemblyName2));
		}

		[TestMethod]
		public void AssemblyLoaderShouldIgnoreInvalidDllsInLibraryFolder()
		{
			string assemblyName = "aaa";

			string fileName = Path.Combine(LibraryPath, assemblyName + ".dll");
			File.WriteAllText(fileName, "NotReallyADll");

			AssemblyLoader.LoadAll(AppDomain.CurrentDomain.BaseDirectory);

			Assert.IsFalse(IsAssemblyLoaded(assemblyName));
		}

		[TestMethod]
		public void AssemblyLoaderShouldIgnoreDllsWhosDependenciesAreNotSatisfied()
		{
			string assemblyName = "AssemblyLoaderShouldIgnoreDllsWhosDependenciesAreNotSatisfied.TestCode";
			string assemblyFilename = Path.Combine(LibraryPath, assemblyName + ".dll");

			GenerateTestAssembly(assemblyName);
			FileSecurity settings = File.GetAccessControl(assemblyFilename);
			settings.AddAccessRule(new FileSystemAccessRule(Environment.UserDomainName + @"\" + Environment.UserName, FileSystemRights.ReadAndExecute, AccessControlType.Deny));
			File.SetAccessControl(assemblyFilename, settings);

			AssemblyLoader.LoadAll(AppDomain.CurrentDomain.BaseDirectory);

			Assert.IsFalse(IsAssemblyLoaded(assemblyName));
		}

		//	Helper functions

		private bool IsAssemblyLoaded(string name)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly assembly in assemblies)
			{
				AssemblyName assemblyName = assembly.GetName();

				if (String.Compare(assemblyName.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
					return true;
			}
			return false;
		}

		private static void ResetLibrary()
		{
			if (!Directory.Exists(LibraryPath))
				Directory.CreateDirectory(LibraryPath);

			string[] assemblyFilenames = Directory.GetFiles(LibraryPath, "*.dll");
			foreach (string filename in assemblyFilenames)
			{
				try
				{
					File.Delete(Path.Combine(LibraryPath, filename));
				}
				catch (UnauthorizedAccessException) { } // Ignore assemblies that have been loaded by previous tests.
			}

		}

		private static void GenerateTestAssembly(string assemblyName)
		{
			string path = Path.Combine(LibraryPath, assemblyName + ".dll");

			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(
				@" 
				namespace " + assemblyName + @" {
					public class TestClass { public void TestMethod() { } }
					
					public interface ITestInterface { }

					public class TestInterfaceClass : ITestInterface { public void TestNewMethod() { } }
				}
				",
				path,
				new CSharp.CSharpCodeProvider(),
				new string[] { });
			Assert.IsTrue(File.Exists(path));
		}

		private static string LibraryPath
		{
			get
			{
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib");
			}
		}
	}
}
