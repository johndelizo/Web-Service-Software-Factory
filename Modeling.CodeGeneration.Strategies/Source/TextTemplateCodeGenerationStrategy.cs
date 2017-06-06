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
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.VisualStudio.TextTemplating;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies
{
	public class TextTemplateCodeGenerationStrategy: ICodeGenerationStrategy
	{
		private IServiceProvider serviceProvider;
		private IList<Guid> projectReferences;
		private IList<string> assemblyReferences;
		private IList<LogEntry> errors;
		private IResourceResolver resourceResolver = new DefaultResourceResolver();

		public TextTemplateCodeGenerationStrategy()
		{
			this.projectReferences = new List<Guid>();
			this.assemblyReferences = new List<string>();
			this.errors = new List<LogEntry>();
		}

		#region ICodeGenerationStrategy Members

		/// <summary>
		/// Generates code according to the specified link information.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public CodeGenerationResults Generate(IArtifactLink link)
		{
			CodeGenerationResults result = new CodeGenerationResults();

			if (link is IModelReference)
			{
				this.serviceProvider = Utility.GetData<IServiceProvider>(link);
				ProjectNode project = Utility.GetData<ProjectNode>(link);

				ModelElement modelElement = ((IModelReference)link).ModelElement;
				TextTemplateArtifactLinkWrapper textTemplateArtifactLink = new TextTemplateArtifactLinkWrapper(link);				
				textTemplateArtifactLink.ResourceResolver = ResourceResolver;
				string template = GetTemplateBasedOnProject(textTemplateArtifactLink, project);
				
				if (modelElement != null && !string.IsNullOrEmpty(template))
				{
					Engine engine = new Engine();
					DomainModel model = (DomainModel)modelElement.Store.GetDomainModel(modelElement.GetDomainClass().DomainModel.Id);					
                    TextTemplateHost host = new TextTemplateHost(model, modelElement, modelElement,	GetService<ICodeGenerationService>());
					host.ResourceResolver = textTemplateArtifactLink.ResourceResolver;
					string content = engine.ProcessTemplate(template, host);

					if (host.GenerateOutput)
					{
						this.projectReferences = new List<Guid>(host.ProjectReferences);
						this.assemblyReferences = GetAssemblyReferences(link);
						if (host.CompilerErrors.Count > 0)
						{
							foreach (CompilerError error in host.CompilerErrors)
							{
								LogError(error);
							}
						}
                        // Will create a file with the 'ErrorGeneratingOutput' text in the generated file.
						result.Add(link.ItemPath, content);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the project references.
		/// </summary>
		/// <value>The project references.</value>
		public IList<Guid> ProjectReferences
		{
			get { return projectReferences; }
		}

		/// <summary>
		/// Gets the project references.
		/// </summary>
		/// <value>The project references.</value>
		public IList<string> AssemblyReferences
		{
			get { return assemblyReferences; }
		}

		public IList<LogEntry> Errors
		{
			get { return errors; }
		}

		#endregion

		#region Private Implementation

		private T GetService<T>()
		{
			return (T)serviceProvider.GetService(typeof(T));
		}

		[CLSCompliant(false)]
		public IResourceResolver ResourceResolver
		{
			get { return resourceResolver; }
			set { resourceResolver = value; }
		}

		private string GetTemplateBasedOnProject(TextTemplateArtifactLinkWrapper link, ProjectNode project)
		{
			TextTemplateTargetLanguage targetLanguage = ResolveTargetProjectLanguage(project);
			string template = link.GetTemplate(targetLanguage);

			if (string.IsNullOrEmpty(template))
			{
				template = link.GetTemplate(TextTemplateTargetLanguage.Any);
			}

			if (string.IsNullOrEmpty(template))
			{
				throw new InvalidOperationException(Properties.Resources.EmptyOrNonExistentTemplate);
			}

			return template;
		}

		private TextTemplateTargetLanguage ResolveTargetProjectLanguage(ProjectNode project)
		{
			switch (project.Language)
			{
				case EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp:
					return TextTemplateTargetLanguage.CSharp;

				case EnvDTE.CodeModelLanguageConstants.vsCMLanguageVB:
					return TextTemplateTargetLanguage.VB;

				default:
					throw new InvalidOperationException(Properties.Resources.InvalidProjectLanguage);
			}
		}

		private void LogError(CompilerError error)
		{
			int errorNumber = 0;
			// Try and get an error number from the error, if parsing fails then ignore and use 0.
			int.TryParse(error.ErrorNumber, out errorNumber);

			LogEntry entry = new LogEntry(
				error.ToString(),
				Properties.Resources.Generation_Error_Title,
				error.IsWarning ? TraceEventType.Warning : TraceEventType.Error,
				errorNumber);

			errors.Add(entry);
		}

		private IList<string> GetAssemblyReferences(IArtifactLink link)
		{
			IList<string> assemblyReferences = new List<string>();

			AssemblyReferenceAttribute[] asmReferenceAttributes =
				ReflectionHelper.GetAttributes<AssemblyReferenceAttribute>(link.GetType(), true);

			foreach (AssemblyReferenceAttribute asmReferenceAttribute in asmReferenceAttributes)
			{
				assemblyReferences.Add(asmReferenceAttribute.AssemblyName);
			}

			return assemblyReferences;
		}

		#endregion
	}
}
