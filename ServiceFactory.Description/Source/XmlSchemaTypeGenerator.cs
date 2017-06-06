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
using System.Xml.Schema;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using Web = System.Web.Services.Description;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.Description
{
	/// <summary>
	/// Generate classes files from XML schemas
	/// </summary>
	public class XmlSchemaTypeGenerator
	{
		private ValidationEventHandler validationEventHandler;
		private bool useXmlSerializerImporter;
		private string targetNamespace;

		public const string DefaultNamespace = "namespace1";

		/// <summary>
		/// Initializes the type generator
		/// </summary>
		public XmlSchemaTypeGenerator() : 
			this(false, string.Empty)
		{
		}

		/// <summary>
		/// Initializes the type generator
		/// </summary>
		/// <param name="useXmlSerializerImporter">if set to <c>true</c> [use XML serializer importer].</param>
		public XmlSchemaTypeGenerator(bool useXmlSerializerImporter) : 
			this(useXmlSerializerImporter, string.Empty)
		{
		}

		/// <summary>
		/// Initializes the type generator
		/// </summary>
		/// <param name="useXmlSerializerImporter">if set to <c>true</c> [use XML serializer importer].</param>
		/// <param name="targetNamespace">The target namespace.</param>
		public XmlSchemaTypeGenerator(bool useXmlSerializerImporter, string targetNamespace)
		{
			Guard.ArgumentNotNull(targetNamespace, "targetNamespace");

			this.validationEventHandler = new ValidationEventHandler(OnSchemasValidation);
			this.useXmlSerializerImporter = useXmlSerializerImporter;
			this.targetNamespace = targetNamespace;
		}

		/// <summary>
		/// Generates the types for the schema files
		/// </summary>
		/// <param name="schemaSource">The schema source (a file or URI).</param>
		public CodeTypeDeclarationCollection GenerateTypes(string schemaSource)
		{
			return PrepareTypes(GenerateCodeCompileUnit(schemaSource));
		}

		/// <summary>
		/// Generates the types for the schema files
		/// </summary>
		/// <param name="schemaSources">The schema sources. (files or URIs)</param>
		public CodeTypeDeclarationCollection GenerateTypes(string[] schemaSources)
		{
			return PrepareTypes(GenerateCodeCompileUnit(schemaSources));
		}

		/// <summary>
		/// Generates the types for the specified wsdl importer
		/// </summary>
		/// <param name="importer">The WsdlImporter importer.</param>
		public CodeTypeDeclarationCollection GenerateTypes(WsdlImporter importer)
		{
			return PrepareTypes(GenerateCodeCompileUnit(importer));
		}

		/// <summary>
		/// Generates the code compile unit from a schema source.
		/// </summary>
		/// <param name="schemaSource">The schema source (a file or URI).</param>
		/// <returns></returns>
		public CodeCompileUnit GenerateCodeCompileUnit(string schemaSource)
		{
			Guard.ArgumentNotNullOrEmptyString(schemaSource, "schemaSource");

			XmlSchemaSet schemas = ReadSchemas(schemaSource);
			return InternalGenerateTypes(schemas, this.useXmlSerializerImporter);
		}

		/// <summary>
		/// Generates the code compile unit from a schema source.
		/// </summary>
		/// <param name="schemaSources">The schema sources. (files or URIs)</param>
		/// <returns></returns>
		public CodeCompileUnit GenerateCodeCompileUnit(string[] schemaSources)
		{
			XmlSchemaSet schemas = ReadSchemas(schemaSources);
			return InternalGenerateTypes(schemas, this.useXmlSerializerImporter);
		}

		/// <summary>
		/// Generates the code compile unit.
		/// </summary>
		/// <param name="importer">The importer.</param>
		/// <returns></returns>
		public CodeCompileUnit GenerateCodeCompileUnit(WsdlImporter importer)
		{
			Guard.ArgumentNotNull(importer, "importer");

			// now generate the full collection types
			ContractGenerator generator = CreateContractGenerator(this.useXmlSerializerImporter);
			generator.Generate(importer, false);
			return generator.CodeCompileUnit;
		}

		/// <summary>
		/// Read the schemas resolving dependencies
		/// </summary>
		/// <param name="schemaSource">The schema source. (A file or URI)</param>
		/// <returns>Read schema files</returns>
		public XmlSchemaSet ReadSchemas(string schemaSource)
		{
			Guard.ArgumentNotNullOrEmptyString(schemaSource, "schemaSource");
			return ReadSchemas(new string[] { schemaSource });		
		}

		/// <summary>
		/// Read the schemas resolving dependencies
		/// </summary>
		/// <param name="schemaSources">The schema sources. (files or URIs)</param>
		/// <returns>Read schema files</returns>
		public XmlSchemaSet ReadSchemas(string[] schemaSources)
		{
			Guard.ArgumentNotNull(schemaSources, "schemaSources");

			MetadataSet metadataDocuments = new MetadataSet();
			foreach (string schemaSource in schemaSources)
			{
				MetadataDiscovery discovery = new MetadataDiscovery(schemaSource);
				MetadataSet metadataSet = discovery.InspectMetadata();
				foreach (MetadataSection section in metadataSet.MetadataSections)
				{
					metadataDocuments.MetadataSections.Add(section);
				}
			}

			ContractGenerator generator = CreateContractGenerator(this.useXmlSerializerImporter);
			return ReadSchemas(generator.CreateWsdlImporter(metadataDocuments));
		}

		/// <summary>
		/// Read the schemas resolving dependencies
		/// </summary>
		/// <param name="schemaSource">The schema source.</param>
		/// <returns>Read schema files</returns>
		public XmlSchemaSet ReadSchemas(WsdlImporter importer)
		{
			Guard.ArgumentNotNull(importer, "importer");
			
			XmlSchemaSet schemas = importer.XmlSchemas;

			if (schemas.Count == 0)
			{
				foreach (Web.ServiceDescription serviceDescription in importer.WsdlDocuments)
				{
					foreach (XmlSchema schema in serviceDescription.Types.Schemas)
					{
						schemas.Add(schema);
					}

					foreach (Web.Import import in serviceDescription.Imports)
					{
						schemas.Add(ReadSchemas(import.Location));
					}
				}
			}

			return schemas;
		}

		#region ReadSchemas

		private ContractGenerator CreateContractGenerator(bool importXmlType)
		{
			ContractGenerationOptions options = new ContractGenerationOptions();
			ImportOptions importOptions = CreateImportOptions(importXmlType);
			
			options.GenerateClientClass = false;
			options.GenerateChannelInterface = false;
			options.GenerateAsyncMethods = false;
			options.GenerateInternalTypes = importOptions.GenerateInternal;
			options.GenerateSerializable = importOptions.GenerateSerializable;
			options.GenerateTypedMessages = false;
			options.ImportXmlType = importOptions.ImportXmlType;
			options.SchemaImporterType = importXmlType ? ContractGenerationOptions.ImporterType.XmlSerializerImporter : 
														 ContractGenerationOptions.ImporterType.XsdDataContractImporter;
			return new ContractGenerator(options);
		}

		private ImportOptions CreateImportOptions(bool importXmlType)
		{
			ImportOptions options = new ImportOptions();
			options.ImportXmlType = importXmlType;
			options.Namespaces.Add(ContractGenerationOptions.NamespaceMappingsAllKeyName, this.targetNamespace);			
			return options;
		}

		#endregion

		#region GenerateTypes

        private CodeCompileUnit InternalGenerateTypes(XmlSchemaSet schemas, bool importXmlType)
        {
			if (importXmlType)
			{
				return GenerateTypesWithXmlSchemaImporter(schemas);
			}
			return GenerateTypesWithDataContractImporter(schemas);
        }

        private CodeCompileUnit GenerateTypesWithDataContractImporter(XmlSchemaSet schemas)
		{
			XsdDataContractImporter importer = new XsdDataContractImporter();
			importer.Options = CreateImportOptions(false);

			schemas.ValidationEventHandler += this.validationEventHandler;

			try
			{				
				importer.Import(schemas);
				return importer.CodeCompileUnit;
			}
			catch (InvalidDataContractException dataContractException)
			{
				// double check since CanImport may trigger a "false" positive
				// after adding serialization and types schemas
				importer.CanImport(schemas);
				if (!importer.CanImport(schemas))
				{
					throw new InvalidSerializerException(dataContractException.Message);
				}
				throw;
			}
			catch (ArgumentException argumentException)
			{
				throw new InvalidOperationException(argumentException.Message, argumentException.InnerException);
			}
			finally
			{
				schemas.ValidationEventHandler -= this.validationEventHandler;
			}
		}

		private CodeCompileUnit GenerateTypesWithXmlSchemaImporter(XmlSchemaSet schemas)
		{
			CodeNamespace ns = new CodeNamespace(this.targetNamespace);
			CodeCompileUnit unit = new CodeCompileUnit();
			unit.Namespaces.Add(ns);

			schemas.ValidationEventHandler += this.validationEventHandler;

			try
			{
				// Validate schemas
				schemas.Compile();

				XmlSchemas xsds = new XmlSchemas();
				foreach (XmlSchema xsd in schemas.Schemas())
				{
					xsds.Add(xsd);
				}

				foreach (XmlSchema schema in xsds)
				{
					XmlSchemaImporter importer = new XmlSchemaImporter(xsds);
					XmlCodeExporter exporter = new XmlCodeExporter(ns, unit);

					// export only elements
					foreach (XmlSchemaElement schemaElement in schema.Elements.Values)
					{
						exporter.ExportTypeMapping(importer.ImportTypeMapping(schemaElement.QualifiedName));
					}

					CodeGenerator.ValidateIdentifiers(ns);
				}
			}
			catch (ArgumentException argumentException)
			{
				throw new InvalidOperationException(argumentException.Message, argumentException.InnerException);
			}
			finally
			{
				schemas.ValidationEventHandler -= this.validationEventHandler;
			}

			return unit;
		}

		private void OnSchemasValidation(object sender, ValidationEventArgs e)
		{
			if (e.Exception != null)
			{
				throw e.Exception;
			}
			throw new XmlSchemaException(e.Message);
		}

		private CodeTypeDeclarationCollection PrepareTypes(CodeCompileUnit unit)
		{
			if (unit.Namespaces.Count == 0)
			{
				return new CodeTypeDeclarationCollection();
			}

			if (unit.Namespaces.Count == 1)
			{
				return unit.Namespaces[0].Types;
			}

			throw new InvalidOperationException(Properties.Resources.MultipleNamespacesError);
		}

		#endregion
	}
}
