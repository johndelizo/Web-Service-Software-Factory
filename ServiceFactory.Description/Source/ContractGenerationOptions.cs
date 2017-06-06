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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.ServiceModel.Description;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ServiceFactory.Description
{
    /// <summary>
    /// Options settings for the <see cref="ProxyGeneration"/> class.
    /// </summary>    
    public class ContractGenerationOptions
    {    
        [SuppressMessage("Microsoft.Design","CA1034:NestedTypesShouldNotBeVisible")]
		public enum ImporterType
		{
			XsdDataContractImporter,
			XmlSerializerImporter,
			All
		}

        #region Fields declaration & Constructors

        private IList<Type> referencedTypes;
        private IList<Type> referencedCollectionTypes;
        private Dictionary<string, string> namespaceMappings;
        private IList<string> referencedAssemblies;
        private CodeDomProvider codeProvider;
        private System.Configuration.Configuration outputConfiguration;
        private string outputConfigurationFile;
		private ImporterType schemaImporterType;
        private IList<string> importedEndpointNames;
        private bool importXmlType;
        private bool enableDataBinding;
        private bool generateSerializable;
        private bool generateInternalTypes;
        private bool generateTypedMessages;
        private bool generateAsyncMethods;
		private bool generateChannelInterface;
		private bool generateClientClass;
        private string assemblyResolvePath;

        /// <summary>
        /// The key mapping that contains all namespaces.
        /// </summary>
        public const string NamespaceMappingsAllKeyName = "*";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProxyGenerationOptions"/> class.
        /// </summary>
        public ContractGenerationOptions()
        {
            generateAsyncMethods = false; 
            generateInternalTypes = false;
            generateSerializable = true;
			generateChannelInterface = true;
			generateClientClass = true;
            enableDataBinding = false;
			schemaImporterType = ImporterType.All;
            importedEndpointNames = new Collection<string>();
            generateTypedMessages = false;
            importXmlType = true;
            codeProvider = CodeDomProvider.CreateProvider("C#");
			referencedAssemblies = new Collection<string>();
			referencedCollectionTypes = new Collection<Type>();
			referencedTypes = new Collection<Type>();
            namespaceMappings = new Dictionary<string, string>();
            namespaceMappings.Add(NamespaceMappingsAllKeyName, string.Empty);
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets a value indicating whether [generate async methods].
        /// Generate asynchronous method signatures.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generate async methods]; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateAsyncMethods
        {
            get { return generateAsyncMethods; }
            set { generateAsyncMethods = value; }
        } 

        /// <summary>
        /// Gets or sets a value indicating whether [generate typed messages].
        /// Support for MessageContracts.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generate typed messages]; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateTypedMessages
        {
            get { return generateTypedMessages; }
            set { generateTypedMessages = value; }
        } 

        /// <summary>
        /// Gets or sets a value that specifies whether generated code will be marked internal or public.
        /// </summary>
        /// <value>true, if the code will be marked internal; otherwise, false. The default is true.</value>
        public bool GenerateInternalTypes
        {
            get { return generateInternalTypes; }
            set { generateInternalTypes = value; }
        } 

        /// <summary>
        /// Gets or sets a value that specifies whether generated data contract classes 
        /// will be marked with the <see cref="T:System.SerializableAttribute"></see>.
        /// </summary>
        /// <value>true, to generated classes with the <see cref="T:System.SerializableAttribute"></see> applied; otherwise, false.</value>
        public bool GenerateSerializable
        {
            get { return generateSerializable; }
            set { generateSerializable = value; }
        }

		/// <summary>
		/// Gets or sets a value indicating whether [generate channel interface].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [generate channel interface]; otherwise, <c>false</c>.
		/// </value>
		public bool GenerateChannelInterface
		{
			get { return generateChannelInterface; }
			set { generateChannelInterface = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether [generate client class].
		/// </summary>
		/// <value><c>true</c> if [generate client class]; otherwise, <c>false</c>.</value>
		public bool GenerateClientClass
		{
			get { return generateClientClass; }
			set { generateClientClass = value; }
		}

        /// <summary>
        /// Gets or sets a value that specifies whether types in generated code should implement the 
        /// <see cref="T:System.ComponentModel.INotifyPropertyChanged"></see> interface.
        /// </summary>
        /// <value>true, if the generated code should implement the 
        /// <see cref="T:System.ComponentModel.INotifyPropertyChanged"></see> interface; otherwise, false.</value>
        public bool EnableDataBinding
        {
            get { return enableDataBinding; }
            set { enableDataBinding = value; }
        }

        /// <summary>
        /// Gets or sets a value that determines whether all schema types, 
        /// even those that do not conform to the data contract will be imported.
        /// </summary>
        /// <value>true, to import all schema types; otherwise, false.</value>
        public bool ImportXmlType
        {
            get { return importXmlType; }
            set { importXmlType = value; }
        } 

        /// <summary>
        /// Gets or sets the imported endpoint names.
        /// </summary>
        /// <value>The imported endpoint names collection.</value>
        public IList<string> ImportedEndpointNames
        {
            get { return importedEndpointNames; }
            set { importedEndpointNames = value; }
        }

		/// <summary>
		/// Gets or sets the type of the schema importer.
		/// </summary>
		/// <value>The type of the schema importer.</value>
		public ImporterType SchemaImporterType
        {
			get { return schemaImporterType; }
			set { schemaImporterType = value; }
        }

        /// <summary>
        /// Gets or sets the output configuration file.
        /// </summary>
        /// <value>The output configuration file.</value>
        public string OutputConfigurationFile
        {
            get { return outputConfigurationFile; }
            set { outputConfigurationFile = value; }
        } 

        /// <summary>
        /// Gets or sets the proxy namespace.
        /// </summary>
        /// <value>The proxy namespace.</value>
        public string ClrNamespace
        {
            get { return this.namespaceMappings[NamespaceMappingsAllKeyName]; }
            set { this.namespaceMappings[NamespaceMappingsAllKeyName] = value ?? string.Empty; }
        }

        /// <summary>
        /// Gets or sets the output configuration.
        /// </summary>
        /// <value>The output configuration.</value>
        public System.Configuration.Configuration OutputConfiguration
        {
            get { return outputConfiguration; }
            set { outputConfiguration = value; }
        } 

        /// <summary>
        /// Gets or sets a <see cref="T:System.CodeDom.Compiler.CodeDomProvider"></see> 
        /// instance that provides the means to check if particular options for a target language are supported.
        /// </summary>
        /// <value>A <see cref="T:System.CodeDom.Compiler.CodeDomProvider"></see> that provides the means to 
        /// check if particular options for a target language are supported.
        /// </value>
        public CodeDomProvider CodeProvider
        {
            get { return codeProvider; }
            set { codeProvider = value; }
        } 

        /// <summary>
        /// Gets or sets the collection of referenced assembly paths.
        /// </summary>
        /// <value>The referenced assemblies.</value>
        public IList<string> ReferencedAssemblies
        {
            get { return referencedAssemblies; }
            set { referencedAssemblies = value; }
        } 

        /// <summary>
        /// Gets the namespace mappings.
        /// </summary>
        /// <value>The namespace mappings.</value>
        public Dictionary<string, string> NamespaceMappings
        {
            get { return namespaceMappings; }
        } 

        /// <summary>
        /// Gets the referenced collection types.
        /// </summary>
        /// <value>The referenced collection types.</value>
        public IList<Type> ReferencedCollectionTypes
        {
            get { return this.referencedCollectionTypes; }
        }

        /// <summary>
        /// Gets or sets the referenced types.
        /// </summary>
        /// <value>The referenced types.</value>
        public IList<Type> ReferencedTypes
        {
            get { return referencedTypes; }
            set { referencedTypes = value; }
        }

        /// <summary>
        /// Gets or sets the assembly resolve path.
        /// </summary>
        /// <value>The assembly resolve path.</value>
        public string AssemblyResolvePath
        {
            get { return assemblyResolvePath; }
            set { assemblyResolvePath = value; }
        }

        #endregion
    }
}
