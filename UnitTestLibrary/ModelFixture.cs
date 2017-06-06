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
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Tests.Mocks;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;
using System.IO;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.UnitTestLibrary
{
	public abstract class ModelFixture : ICodeGenerationService
	{
		protected const string DefaultNamespace = "Namespace1";
		private MockServiceProvider serviceProvider = new MockMappingServiceProvider();
		protected Transaction transaction = null;

		[TestInitialize]
		public void BeginTrans()
		{
            GlobalCache.Off();
			// end any pending tx
			EndTrans();
			// start a new tx
			transaction = Store.TransactionManager.BeginTransaction();
		}

		[TestCleanup]
		public void Cleanup()
		{
			EndTrans();
		}

		public void EndTrans()
		{
			if (transaction != null)
			{
				transaction.Rollback();
				transaction = null;
			}
		}

		protected MockServiceProvider ServiceProvider
		{
			get { return serviceProvider; }
			set { serviceProvider = value; }
		}

		protected virtual ModelElement ResolveModelElement(string instanceNamespace)
		{
			return null;
		}

		protected virtual bool ValidateArtifactLink(IArtifactLink link)
		{
			return false;
		}

		protected virtual void EnsureNamespace(ref string content)
		{
			if (content.Contains("namespace " + Environment.NewLine))
			{
				content = content.Replace("namespace ", "namespace " + DefaultNamespace);
			}
		}

		protected virtual void EnsureType(ref string content, string typeName)
		{
			content += "namespace " + DefaultNamespace + " {";
			content += "public " + 
				(typeName.StartsWith("I") ? "interface " : "class ") + typeName + "{}";
			content += "} ";
		}

		protected string RunTemplate(ModelElement rootElement)
		{
			return RunTemplateInternal(rootElement, null);
		}

		protected TemplateResult RunTemplateWithErrors(ModelElement rootElement)
		{
			return RunTemplateReturningErrors(rootElement, null);
		}

		protected string RunTemplateWithCGS(ModelElement rootElement)
		{
			return RunTemplateInternal(rootElement, this);
		}

		protected string RunTemplateWithDIS(ModelElement rootElement)
		{
			return RunTemplateInternal(rootElement, null);
		}

		protected abstract Type ContractType
		{
			get;
		}

		protected abstract string Template
		{
			get;
		}

		protected abstract Store Store
		{
			get;
		}

		protected abstract DomainModel DomainModel
		{
			get;
		}

		/// <summary>
		/// Returns a TextTemplateArtifactLinkWrapper.
		/// </summary>
		/// <param name="artifactLink"></param>
		/// <returns></returns>
		protected virtual TextTemplateArtifactLinkWrapper GetWrappedLink(IArtifactLink artifactLink)
		{
			TextTemplateArtifactLinkWrapper wrapper = new TextTemplateArtifactLinkWrapper(artifactLink);
			wrapper.ResourceResolver = new AssemblyResourceResolver();
			return wrapper;
		}
		
		private string RunTemplateInternal(ModelElement rootElement, ICodeGenerationService codeGenerationService)
		{
			TemplateResult result = RunTemplateReturningErrors(rootElement, codeGenerationService);
			Assert.AreNotEqual<string>("ErrorGeneratingOutput", result.ContentResults);
			return result.ContentResults;
		}

		private TemplateResult RunTemplateReturningErrors(ModelElement rootElement, ICodeGenerationService codeGenerationService)
		{
			TextTemplateHost host = new TextTemplateHost(DomainModel, rootElement, rootElement, codeGenerationService, ContractType);
			host.ResourceResolver = new AssemblyResourceResolver();
			Engine engine = new Engine();
			string result = engine.ProcessTemplate(Template, host);
			string[] errors = GetErrors(host);
			return new TemplateResult(host.GenerateOutput ? result : string.Empty, errors);
		}

		private static string[] GetErrors(TextTemplateHost host)
		{
			string[] errors = new string[0] ;
			if (host.CompilerErrors.Count > 0)
			{
				errors = new string[host.CompilerErrors.Count];
				for (int i = 0; i < host.CompilerErrors.Count; i++)
				{
					errors[i] = host.CompilerErrors[i].ErrorText;
				}
			}
			return errors;
		}

		public class TemplateResult
		{
			public TemplateResult(string TemplateResults, string[] errors)
			{
				contentResults = TemplateResults;
				errorList = errors;
			}

			private string contentResults;
			public string ContentResults
			{
				get { return contentResults; }
			}

			private string[] errorList;
			public string[] Errors
			{
				get { return errorList; }
			}
	
		}

		#region ICodeGenerationService Members

		int ICodeGenerationService.GenerateArtifact(IArtifactLink link)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		int ICodeGenerationService.GenerateArtifacts(ICollection<IArtifactLink> artifactLinks)
		{
			throw new Exception("The method or operation is not implemented.");
		}

        int ICodeGenerationService.GenerateArtifacts(ICollection<IArtifactLink> artifactLinks, Action<IArtifactLink> updatedLink)
        {
            throw new Exception("The method or operation is not implemented.");
        }

		bool ICodeGenerationService.IsValid(IArtifactLink link)
		{
			return ValidateArtifactLink(link);
		}

		bool ICodeGenerationService.AreValid(ICollection<IArtifactLink> artifactLinks)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		bool ICodeGenerationService.IsArtifactAlreadyGenerated(IArtifactLink link)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		Microsoft.Practices.VisualStudio.Helper.HierarchyNode ICodeGenerationService.ValidateDelete(IArtifactLink artifactLink)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		Microsoft.Practices.VisualStudio.Helper.HierarchyNode ICodeGenerationService.ValidateRename(IArtifactLink artifactLink, string newName, string oldName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		ICollection<Microsoft.Practices.VisualStudio.Helper.HierarchyNode> ICodeGenerationService.ValidateDeleteFromCollection(ICollection<IArtifactLink> artifactLinks)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		ICollection<Microsoft.Practices.VisualStudio.Helper.HierarchyNode> ICodeGenerationService.ValidateRenameFromCollection(ICollection<IArtifactLink> artifactLinks, string newName, string oldName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
