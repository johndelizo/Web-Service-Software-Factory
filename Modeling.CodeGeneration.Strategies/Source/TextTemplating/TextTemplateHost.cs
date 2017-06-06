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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Policy;
using System.Text;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Integration;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating
{
	public sealed class TextTemplateHost : Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost
	{
		#region Fields

		private AssemblyReferenceCollection standardAssemblyReferences = new AssemblyReferenceCollection();
		private ImportReferenceCollection standardImports = new ImportReferenceCollection();
		private DomainModel model;
		private ModelElement rootElement;
		private ModelElement currentElement;
		private ICodeGenerationService codeGenerationService;
		private static TextTemplateHost instance;
		private string baseDirectory;
		private IList<Guid> projectReferences;
		private List<CompilerError> compilerErrors = new List<CompilerError>();
		private IResourceResolver resourceResolver;
		private bool generateOutput;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TextTemplateHost"/> class.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="rootElement">The root element.</param>
		/// <param name="currentElement">The current element.</param>
		public TextTemplateHost(DomainModel model, ModelElement rootElement, ModelElement currentElement)
			: this(model, rootElement, currentElement, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextTemplateHost"/> class.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="rootElement">The root element.</param>
		/// <param name="currentElement">The current element.</param>
		/// <param name="codeGenerationService">The service to validate ArtifactLinks</param>
		[CLSCompliant(false)]
		public TextTemplateHost(DomainModel model, ModelElement rootElement, ModelElement currentElement, ICodeGenerationService codeGenerationService)
			: this(model, rootElement, currentElement, codeGenerationService, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextTemplateHost"/> class.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="rootElement">The root element.</param>
		/// <param name="currentElement">The current element.</param>
		/// <param name="dslIntegrationService">The service to resolve model to model references</param>
		/// <param name="codeGenerationService">The service to validate ArtifactLinks</param>
		/// <param name="extenderType">The type of the extender in the current element.</param>
		[CLSCompliant(false)]
		public TextTemplateHost(DomainModel model, ModelElement rootElement, ModelElement currentElement, ICodeGenerationService codeGenerationService, Type extenderType)
		{
			Init(model, rootElement, currentElement, codeGenerationService, extenderType);
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Gets or sets the base directory.
		/// </summary>
		/// <value>The base directory.</value>
		public string BaseDirectory
		{
			get { return baseDirectory; }
			set { baseDirectory = value; }
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static TextTemplateHost Instance
		{
			get { return instance; }
		}

		/// <summary>
		/// Gets the project references.
		/// </summary>
		/// <value>The project references.</value>
		public IList<Guid> ProjectReferences
		{
			get { return projectReferences; }
		}

		[CLSCompliant(false)]
		public IResourceResolver ResourceResolver
		{
			get { return resourceResolver; }
			set { resourceResolver = value; }
		}

		/// <summary>
		/// Returns access to the compiler errors.
		/// </summary>
		public ReadOnlyCollection<CompilerError> CompilerErrors
		{
			get
			{
				return new ReadOnlyCollection<CompilerError>(this.compilerErrors);
			}
		}

		/// <summary>
		/// Clears the compiler errors
		/// </summary>
		public void ClearErrors()
		{
			compilerErrors.Clear();
		}


		/// <summary>
		/// Logs the errors.
		/// </summary>
		/// <param name="errors">The errors.</param>
		public void LogErrors(CompilerErrorCollection errors)
		{
			Guard.ArgumentNotNull(errors, "errors");

			if (errors.Count > 0)
			{
				Debug.WriteLine("TextTemplateHost Errors Ocurred: " + errors.Count);
				foreach (CompilerError error in errors)
				{
					compilerErrors.Add(error);
					if (error.IsWarning)
					{
						Trace.TraceWarning(error.ToString());
					}
					else
					{
						Trace.TraceError(error.ToString());
					}
					Debug.WriteLine(error.ToString());
				}
			}
		}

		#endregion

		#region ITextTemplatingEngineHost Members

		/// <summary>
		/// Loads the include text.
		/// </summary>
		/// <param name="requestFileName">Name of the request file.</param>
		/// <param name="content">The content.</param>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public bool LoadIncludeText(string requestFileName, out string content, out string location)
		{
			Guard.ArgumentNotNullOrEmptyString(requestFileName, "requestFileName");

			location = this.ResourceResolver.GetResourcePath(requestFileName);
			content = this.ResourceResolver.GetResource(location);
			return true;
		}

		/// <summary>
		/// Resolves the assembly reference.
		/// </summary>
		/// <param name="assemblyReference">The assembly reference.</param>
		/// <returns></returns>
		public string ResolveAssemblyReference(string assemblyReference)
		{
			Guard.ArgumentNotNullOrEmptyString(assemblyReference, "assemblyReference");

			// Check if the reference already has a full path.
			if (Path.IsPathRooted(assemblyReference))
			{
				Zone zone1 = Zone.CreateFromUrl(new Uri(assemblyReference).AbsoluteUri);
				if (zone1.SecurityZone == SecurityZone.Trusted ||
					zone1.SecurityZone == SecurityZone.MyComputer)
				{
					return assemblyReference;
				}
				return string.Empty;
			}

			if (Path.GetExtension(assemblyReference).Equals(".dll", StringComparison.OrdinalIgnoreCase))
			{
				// probe in the current BaseDir
				string path = Path.Combine(this.baseDirectory, assemblyReference);

				if (File.Exists(path))
				{
					return path;
				}

				// probe in PublicAssemblies & PrivateAssemblies
				if (!File.Exists(assemblyReference))
				{
					path = Path.Combine(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "PublicAssemblies"), assemblyReference);
					if (File.Exists(path))
					{
						return path;
					}

					path = Path.Combine(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "PrivateAssemblies"), assemblyReference);
					if (File.Exists(path))
					{
						return path;
					}

					if (string.IsNullOrEmpty(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath))
					{
						return assemblyReference;
					}

					foreach (string privatePath in AppDomain.CurrentDomain.SetupInformation.PrivateBinPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
					{
						path = Path.Combine(
								Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, privatePath), assemblyReference);
						if (File.Exists(path))
						{
							return path;
						}
					}

					return assemblyReference;
				}
			}

			// Probe in GAC
			try
			{
				Assembly asm = Assembly.Load(assemblyReference);
				if (asm != null)
					return asm.Location;
			}
			catch (FileNotFoundException)
            {
                // probe in Lib path
                assemblyReference = Path.Combine(RuntimeHelper.GetExecutionPath("Lib"), assemblyReference + ".dll");
                try
                {
                    Assembly asm = Assembly.LoadFrom(assemblyReference);
                    if (asm != null)
                        return asm.Location;
                }
                catch (FileNotFoundException) { }
                catch (FileLoadException) { }
                catch (BadImageFormatException) { }
            }
			catch (FileLoadException) { }
			catch (BadImageFormatException) { }

			// if we get here, then we could not resolve the reference and then just throw
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.AssemblyNotResolvedException, assemblyReference));
		}

		/// <summary>
		/// Resolves the directive processor.
		/// </summary>
		/// <param name="processorName">Name of the processor.</param>
		/// <returns></returns>
		public Type ResolveDirectiveProcessor(string processorName)
		{
			if (typeof(ModelInjectorDirectiveProcessor).Name == processorName)
			{
				return typeof(ModelInjectorDirectiveProcessor);
			}
			return null;
		}

		/// <summary>
		/// Resolves the path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public string ResolvePath(string path)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Resolves the parameter value.
		/// </summary>
		/// <param name="directiveId">The directive id.</param>
		/// <param name="processorName">Name of the processor.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <returns></returns>
		public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Provides the templating app domain.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		public AppDomain ProvideTemplatingAppDomain(string content)
		{
			return System.AppDomain.CurrentDomain;
		}

		/// <summary>
		/// Sets the file extension.
		/// </summary>
		/// <param name="extension">The extension.</param>
		public void SetFileExtension(string extension)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sets the output encoding.
		/// </summary>
		/// <param name="encoding">The encoding.</param>
		/// <param name="fromOutputDirective">if set to <c>true</c> [from output directive].</param>
		public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the host option.
		/// </summary>
		/// <param name="optionName">Name of the option.</param>
		/// <returns></returns>
		public object GetHostOption(string optionName)
		{
			return null;
		}

		/// <summary>
		/// Gets the standard assembly references.
		/// </summary>
		/// <value>The standard assembly references.</value>
		public IList<string> StandardAssemblyReferences
		{
			get { return standardAssemblyReferences; }
		}

		/// <summary>
		/// Gets the standard imports.
		/// </summary>
		/// <value>The standard imports.</value>
		public IList<string> StandardImports
		{
			get { return standardImports; }
		}

		/// <summary>
		/// Gets the template file.
		/// </summary>
		/// <value>The template file.</value>
		public string TemplateFile
		{
			get { return string.Empty; }
		}

		#endregion

		#region Inyected Directives

		/// <summary>
		/// Gets the model.
		/// </summary>
		/// <value>The model.</value>
		public DomainModel Model
		{
			get { return model; }
		}

		/// <summary>
		/// Gets the root element.
		/// </summary>
		/// <value>The root element.</value>
		public ModelElement RootElement
		{
			get { return rootElement; }
		}

		/// <summary>
		/// Gets the current element.
		/// </summary>
		/// <value>The current element.</value>
		public ModelElement CurrentElement
		{
			get { return currentElement; }
		}

		/// <summary>
		/// Gets the extender object from the current ModelElement
		/// </summary>
		public object CurrentExtender
		{
			get
			{
				IExtensibleObject extender = CurrentElement as IExtensibleObject;
				if (extender != null)
					return extender.ObjectExtender;
				else
					return null;
			}
		}

		/// <summary>
		/// Resolves the model reference.
		/// </summary>
		/// <param name="mbReference">The ModelBusReference intance.</param>
		/// <returns></returns>
		// FXCOP: False positive
		public ModelElement ResolveModelReference(ModelBusReference mbRefenrece)
		{
            return ModelBusReferenceResolver.ResolveAndDispose(mbRefenrece);
		}

		/// <summary>
		/// Determines whether the specified link is valid.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <returns>
		/// 	<c>true</c> if the specified link is valid; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValid(IArtifactLink link)
		{
			if (codeGenerationService != null)
			{
				return codeGenerationService.IsValid(link);
			}
			return false;
		}

		/// <summary>
		/// Adds the project reference.
		/// </summary>
		/// <param name="link">The link.</param>
		public void AddProjectReference(IArtifactLink link)
		{
			this.projectReferences.Add(link.Container);
		}

		//public void CancelOutput()
		//{
		//    this.generateOutput = false;
		//}

		/// <summary>
		/// Determines whether this instance has aborted.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance has aborted; otherwise, <c>false</c>.
		/// </returns>
		public bool GenerateOutput
		{
			get { return this.generateOutput; }
			set { this.generateOutput = value; }
		}

		#endregion

		#region Private Implementation

		/// <summary>
		/// Inits the specified model.
		/// </summary>
		private void Init(DomainModel model, ModelElement rootElement, ModelElement currentElement, 
            ICodeGenerationService codeGenerationService, Type extenderType)
		{
			this.codeGenerationService = codeGenerationService;
			this.model = model;
			this.rootElement = rootElement;
			this.currentElement = currentElement;
			this.baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			this.projectReferences = new List<Guid>();

			AddStandardImports();
			AddStandardAssemblyReferences();
			standardAssemblyReferences.AddFromAssembly(Assembly.GetExecutingAssembly());

			InitializeFromModel(model);
			InitializeFromElement(rootElement);
			
			InitializeModelElementExtender(currentElement, extenderType);
			
			TextTemplateHost.instance = this;
			this.generateOutput = true;
		}

		private void InitializeFromElement(ModelElement rootElement)
		{
			if (rootElement != null)
			{
				AddReferenceAndImportFromType(rootElement.GetType());
				standardAssemblyReferences.AddFromAssembly(rootElement.GetType().Assembly);
			}
		}

		private void InitializeFromModel(DomainModel model)
		{
			if (model != null)
			{
				AddReferenceAndImportFromType(model.GetType());
			}
		}

		private void InitializeModelElementExtender(ModelElement element, Type extenderType)
		{
			IExtensibleObject extensibleObject = element as IExtensibleObject;
			if (extensibleObject != null)
			{
				object extender = extensibleObject.ObjectExtender;
				if (extender == null && extenderType != null)
				{
					extender = Activator.CreateInstance(extenderType);
					extensibleObject.ObjectExtender = extender;
					PropertyInfo modelElementProp = extenderType.GetProperty("ModelElement");
					if (modelElementProp != null)
					{
						modelElementProp.SetValue(extender, element, null);
					}
				}
				
				if (extender != null) 
					AddReferenceAndImportFromType(extender.GetType());
			}
		}

		private void AddReferenceAndImportFromType(Type type)
		{
			if (type != null)
			{
				standardAssemblyReferences.AddFromType(type);
				standardImports.AddFromType(type);
			}
		}

		private void AddStandardAssemblyReferences()
		{
			standardAssemblyReferences.AddFromType(typeof(string));
			standardAssemblyReferences.AddFromType(typeof(Uri));
			standardAssemblyReferences.AddFromType(this.GetType());
			standardAssemblyReferences.AddFromType(typeof(Utility));
			standardAssemblyReferences.AddFromType(typeof(DomainModel));
            standardAssemblyReferences.AddFromType(typeof(ModelBusReference));
		}

		private void AddStandardImports()
		{
			standardImports.Add("System");
			standardImports.AddFromType(typeof(Utility));
			standardImports.AddFromType(typeof(DomainModel));
		}

		private class ImportReferenceCollection : Collection<string>
		{
			public void AddFromType(Type type)
			{
				Add(type.Namespace);
			}
		}

		private class AssemblyReferenceCollection : Collection<string>
		{
			public void AddFromType(Type type)
			{
				this.Add((new Uri(type.Assembly.CodeBase)).LocalPath);
			}

			public void AddFromAssembly(Assembly asm)
			{
				foreach (AssemblyName refName in asm.GetReferencedAssemblies())
				{
					Assembly refAssembly = Assembly.Load(refName);
					string assemblyPath = new Uri(refAssembly.CodeBase).LocalPath;
					if (!Contains(assemblyPath))
					{
						Add(assemblyPath);
					}
				}
			}
		}

		#endregion
	}
}

