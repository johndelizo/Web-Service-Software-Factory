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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.ServiceFactory.Description;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies
{
	[CLSCompliant(false)]
	public class XmlSchemaCodeGenerationStrategy : ICodeGenerationStrategy
	{
		private IList<Guid> projectReferences;
		private IList<string> assemblyReferences;
		private IList<LogEntry> errors;

		public const string UseXmlSerializerDataKey = "UseXmlSerializer";
		public const string ElementDataKey = "Element";

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSchemaCodeGenerationStrategy"/> class.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		/// <param name="attributes">The attributes.</param>
		public XmlSchemaCodeGenerationStrategy()
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
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual CodeGenerationResults Generate(IArtifactLink link)
		{
			Guard.ArgumentNotNull(link, "link");

			CodeGenerationResults content = new CodeGenerationResults();

			try
			{
				XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(UseXmlSerializer(link), GetNamespace(link));
				string xsdMoniker = Utility.GetData<string>(link, ElementDataKey);
				string xmlSchemaSource = XmlSchemaUtility.GetXmlSchemaSource(xsdMoniker, link);
				if (!string.IsNullOrEmpty(xmlSchemaSource))
				{
					CodeCompileUnit unit = generator.GenerateCodeCompileUnit(xmlSchemaSource);
					string element = new XmlSchemaElementMoniker(xsdMoniker).ElementName;
					UpdateUnit(unit, element, link);
					ThrowOnNoTypes(unit.Namespaces, element);
					this.assemblyReferences = GetAssemblyReferences(link, unit.ReferencedAssemblies);
					CodeDomProvider provider = GetCodeDomProvider(link);
					GenerateCode(unit, provider, content, link.ItemPath);
				}
			}
			catch (Exception exception)
			{
				LogErrors(exception);
			}

			return content;
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

		#region Protected Members

		protected virtual void UpdateUnit(CodeCompileUnit unit, string currentTypeName, IArtifactLink link)
		{
		}

		protected virtual CodeDomProvider GetCodeDomProvider(IArtifactLink link)
		{
			if (link is ArtifactLink)
			{
				return CodeDomProvider.CreateProvider(
					CodeDomProvider.GetLanguageFromExtension(((ArtifactLink)link).DefaultExtension));
			}
			return new CSharp.CSharpCodeProvider();
		}

		protected virtual void LogErrors(Exception errorToLog)
		{
			LogEntry entry = new LogEntry(
				errorToLog,
				Properties.Resources.Generation_Error_Title,
				TraceEventType.Error,
				0);

			errors.Add(entry);
		}

		protected virtual IList<string> GetAssemblyReferences(IArtifactLink link, StringCollection referencedAssemblies)
		{
			IList<string> assemblyReferences = new List<string>();

			// Get references from AssemblyReferenceAttribute
			AssemblyReferenceAttribute[] asmReferenceAttributes =
				ReflectionHelper.GetAttributes<AssemblyReferenceAttribute>(link.GetType(), true);

			foreach (AssemblyReferenceAttribute asmReferenceAttribute in asmReferenceAttributes)
			{
				assemblyReferences.Add(asmReferenceAttribute.AssemblyName);
			}

			// get references from CodeDom
			foreach (string reference in referencedAssemblies)
			{
				assemblyReferences.Add(reference);
			}

			// Add reference to CodeDom asm
			assemblyReferences.Add("System.dll");

			return assemblyReferences;
		}

		protected virtual bool UseXmlSerializer(IArtifactLink link)
		{
			if (UseXmlSerializerFromAttribute(link))
			{
				return true;
			}
			return Utility.GetData<bool>(link, UseXmlSerializerDataKey);
		}

		protected virtual string GetNamespace(IArtifactLink link)
		{
			if (link is ArtifactLink)
			{
				return ((ArtifactLink)link).Namespace;
			}
			return string.Empty;
		}

		protected virtual void ThrowOnNoTypes(CodeNamespaceCollection namespaces, string element)
		{
			if (namespaces.Count == 0)
			{
				throw new InvalidOperationException(
					string.Format(CultureInfo.CurrentCulture, Properties.Resources.CodeGenerationElementWithoutType, element));
			}
		}

		protected virtual void GenerateCode(CodeCompileUnit unit, CodeDomProvider provider, 
			CodeGenerationResults content, string itemPath)
		{
			CodeCompileUnit cloneUnit = CloneUnit(unit);

			foreach (CodeNamespace ns in unit.Namespaces)
			{
				cloneUnit.Namespaces.Clear();
				cloneUnit.Namespaces.Add(CloneNamespace(ns));
				foreach (CodeTypeDeclaration codeType in ns.Types)
				{
					string file = Path.Combine(Path.GetDirectoryName(itemPath),
						Path.ChangeExtension(codeType.Name, Path.GetExtension(itemPath)));
					if (!content.ContainsFile(file))
					{
						cloneUnit.Namespaces[0].Types.Clear();
						cloneUnit.Namespaces[0].Types.Add(codeType);
						using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
						{
							provider.GenerateCodeFromCompileUnit(cloneUnit, stringWriter, new CodeGeneratorOptions());
							content.Add(file, stringWriter.ToString());
						}
					}
				}
			}
		}

		#endregion

		#region Private Members

		private bool UseXmlSerializerFromAttribute(IArtifactLink link)
		{
			// try getting from Attr
			XmlSchemaCodeGenerationAttribute attribute = ReflectionHelper.GetAttribute<XmlSchemaCodeGenerationAttribute>(link.GetType(), true);
			if (attribute == null)
			{
				return false;
			}
			return attribute.UseXmlSerializerImporter;
		}

		private CodeCompileUnit CloneUnit(CodeCompileUnit unit)
		{
			CodeCompileUnit clone = new CodeCompileUnit();
			clone.AssemblyCustomAttributes.AddRange(unit.AssemblyCustomAttributes);
			clone.EndDirectives.AddRange(unit.EndDirectives);
			string[] asms = new string[unit.ReferencedAssemblies.Count];
			unit.ReferencedAssemblies.CopyTo(asms, 0);
			clone.ReferencedAssemblies.AddRange(asms);
			clone.StartDirectives.AddRange(unit.StartDirectives);

			return clone;
		}

		private CodeNamespace CloneNamespace(CodeNamespace ns)
		{
			CodeNamespace clone = new CodeNamespace(ns.Name);
			clone.Comments.AddRange(ns.Comments);
			foreach (CodeNamespaceImport import in ns.Imports)
			{
				clone.Imports.Add(import);
			}
			return clone;
		}

		#endregion

		#endregion
	}
}
