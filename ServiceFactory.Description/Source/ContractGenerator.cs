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
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.CodeDom;
using System.Xml.Schema;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.CodeDom.Compiler;
using System.Xml.Serialization;
using System.Data;
using System.Web.Services.Description;
using System.Xml;
using System.ServiceModel;
using System.Web.Services.Discovery;
using System.Configuration;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Data.Design;
using System.IO;
using System.Reflection;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.ServiceFactory.Description
{
    /// <summary>
    /// Creates the client proxy class and configuration file from a service WSDL contract URI. 
    /// </summary>
    public class ContractGenerator
    {
        #region Fields

        private CodeCompileUnit codeCompileUnit = null;
        private readonly Collection<ChannelEndpointElement> generatedChannelElements = null;
        private readonly ContractGenerationOptions options = null;
        private readonly System.Configuration.Configuration configuration = null;
        private CodeAttributeDeclaration outAttribute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProxyGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ContractGenerator(ContractGenerationOptions options)
        {
            if (!string.IsNullOrEmpty(options.OutputConfigurationFile))
            {
                System.Configuration.Configuration machineConfiguration = ConfigurationManager.OpenMachineConfiguration();
                ExeConfigurationFileMap configurationMap = new ExeConfigurationFileMap();
                configurationMap.ExeConfigFilename = options.OutputConfigurationFile;
                configurationMap.MachineConfigFilename = machineConfiguration.FilePath;
                this.configuration = ConfigurationManager.OpenMappedExeConfiguration(configurationMap, ConfigurationUserLevel.None);
            }
            else if (options.OutputConfiguration != null)
            {
                this.configuration = options.OutputConfiguration;
            }

            this.codeCompileUnit = new CodeCompileUnit();
            this.options = options;
            this.generatedChannelElements = new Collection<ChannelEndpointElement>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the generated channel elements.
        /// </summary>
        /// <value>The generated channel elements.</value>
        public Collection<ChannelEndpointElement> GeneratedChannelElements
        {
            get { return this.generatedChannelElements; }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public System.Configuration.Configuration Configuration
        {
            get { return this.configuration; }
        }

        /// <summary>
        /// Gets the code compile unit.
        /// </summary>
        /// <value>The code compile unit.</value>
        public CodeCompileUnit CodeCompileUnit
        {
            get { return this.codeCompileUnit; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the proxy and configuration file.
        /// </summary>
        /// <param name="metadataSet">The metadata set.</param>
        /// <exception cref="FileNotFoundException">An importer assembly was not found.</exception>
        /// <exception cref="ArgumentException">Unable to load metadata document error.</exception>
        /// <exception cref="TypeLoadException">Unable to load reference type.</exception>
        /// <exception cref="InvalidOperationException">Will throw on metadata conversion errors.</exception>
        public void Generate(MetadataSet metadataSet)
        {
            // create the WSDL importer
            WsdlImporter wsdlImporter = CreateWsdlImporter(metadataSet);
            Generate(wsdlImporter, this.configuration != null, false);
        }

        /// <summary>
        /// Generates the proxy and configuration file.
        /// </summary>
        /// <param name="wsdlImporter">The WSDL importer.</param>
        /// <exception cref="FileNotFoundException">An importer assembly was not found.</exception>
        /// <exception cref="ArgumentException">Unable to load metadata document error.</exception>
        /// <exception cref="TypeLoadException">Unable to load reference type.</exception>
        /// <exception cref="InvalidOperationException">Will throw on metadata conversion errors.</exception>
        public void Generate(WsdlImporter wsdlImporter)
        {
            Generate(wsdlImporter, true, true);
        }

		/// <summary>
		/// Generates the specified WSDL importer.
		/// </summary>
		/// <param name="wsdlImporter">The WSDL importer.</param>
		/// <param name="updateConfigurationFile">if set to <c>true</c> [update configuration file].</param>
		public void Generate(WsdlImporter wsdlImporter, bool updateConfigurationFile)
		{
			Generate(wsdlImporter, updateConfigurationFile, true);
		}

		/// <summary>
		/// Generates the proxy and configuration file.
		/// </summary>
		/// <param name="wsdlImporter">The WSDL importer.</param>
		/// <param name="updateConfigurationFile">if set to <c>true</c> [update configuration file].</param>
		/// <param name="syncCodeCompileUnits">if set to <c>true</c> [sync code compile units from importer and <see cref="CodeCompileUnit"/>].</param>
		/// <exception cref="FileNotFoundException">An importer assembly was not found.</exception>
		/// <exception cref="ArgumentException">Unable to load metadata document error.</exception>
		/// <exception cref="TypeLoadException">Unable to load reference type.</exception>
		/// <exception cref="InvalidOperationException">Will throw on metadata conversion errors.</exception>
        public void Generate(WsdlImporter wsdlImporter, bool updateConfigurationFile, bool syncCodeCompileUnits)
        {
			if (syncCodeCompileUnits)
			{
				// get the ccu from an importer in State and assign it to the local ccu value (this.codeCompileUnit).			
				SyncUpCodeCompileUnit(wsdlImporter.State);
			}

            // Create the Service Contract generator
            ServiceContractGenerator contractGenerator = CreateServiceContractGenerator(this.options,
                this.configuration, this.codeCompileUnit);

            // Add ReferencedTypes to contractGenerator and KnownContracts to wsdlImporter
            AddReferencedTypesAndKnownContracts(this.options, wsdlImporter, contractGenerator);

            // Import all the matadata and check errors.
            ServiceEndpointCollection endpoints;
            Collection<System.ServiceModel.Channels.Binding> bindings;
            Collection<ContractDescription> contracts;
            ImportMetadata(wsdlImporter, out endpoints, out bindings, out contracts);

            // generate each contract.
            foreach (ContractDescription contract in contracts)
            {
                contractGenerator.GenerateServiceContractType(contract);
            }

            // Check for out params in VB
            if (options.CodeProvider is VBCodeProvider)
            {
                PostProcessCode(this.CodeCompileUnit);
            }

            if (updateConfigurationFile)
            {
                GenerateConfig(contractGenerator, endpoints);
            }
        }

        /// <summary>
        /// Creates the WSDL importer.
        /// </summary>
        /// <param name="metadataSet">The metadata set.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">An importer assembly was not found.</exception>
        /// <exception cref="ArgumentException">Unable to load metadata document error.</exception>
        /// <exception cref="TypeLoadException">Unable to load reference type.</exception>
        public WsdlImporter CreateWsdlImporter(MetadataSet metadataSet)
        {
            WsdlImporter importer = null;

            try
            {
                // catch any importer assembly that could not be found 
                // and try to find it in ReferencedAssemblies
                AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
                Collection<IPolicyImportExtension> policyImportExtension = LoadPolicyImportExtensions();
                Collection<IWsdlImportExtension> wsdlImportExtensions = LoadWsdlImportExtensions();
                RemoveUnneededSerializers(wsdlImportExtensions);
                importer = new WsdlImporter(metadataSet, policyImportExtension, wsdlImportExtensions);
            }
            catch (FileNotFoundException fileNotFound)
            {
                throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.CustomImporterNotFoundException, fileNotFound.Message));
            }
            catch (ArgumentException argException)
            {
                throw new ArgumentException(Properties.Resources.ErrUnableToLoadMetadataDocument, argException.InnerException);
            }
            finally
            {
                // remove the event
                AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
            }

            if (this.options.SchemaImporterType == ContractGenerationOptions.ImporterType.All ||
				this.options.SchemaImporterType == ContractGenerationOptions.ImporterType.XsdDataContractImporter)
            {
                AddStateForDataContractSerializerImport(this.options, importer, this.codeCompileUnit);
            }

			if (this.options.SchemaImporterType == ContractGenerationOptions.ImporterType.All ||
				this.options.SchemaImporterType == ContractGenerationOptions.ImporterType.XmlSerializerImporter)
			{
				AddStateForXmlSerializerImport(this.options, importer, this.codeCompileUnit);
			}

            return importer;
        }

        /// <summary>
        /// Throws the on metadata conversion errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <exception cref="InvalidOperationException">Will throw on metadata conversion errors.</exception>
        public static void ThrowOnMetadataConversionErrors(Collection<MetadataConversionError> errors)
        {
            InvalidOperationException exception = null;

            foreach (MetadataConversionError error in errors)
            {
                if (!error.IsWarning)
                {
                    exception = new InvalidOperationException(error.Message, exception);
                }
                Trace.TraceWarning(error.Message);
            }

            if (exception != null)
            {
                throw exception;
            }
        }

        #endregion

        #region Private Implementation

        #region Contract & Config processing

        private void GenerateConfig(
            ServiceContractGenerator contractGenerator, 
            ServiceEndpointCollection endpoints)
        {
			List<string> addedEndpoints = new List<string>();
            foreach (ServiceEndpoint endpoint in endpoints)
            {
                // filter by endpoint address so we generate only the endpoint 
                // that matches the endpoint names in ImportedEndpointNames
                if (!addedEndpoints.Contains(endpoint.Name) &&
					(options.ImportedEndpointNames.Count == 0 ||
                     options.ImportedEndpointNames.Contains(endpoint.Name)))
                {
                    // generate service endpoint
                    ChannelEndpointElement channelElement;
                    contractGenerator.GenerateServiceEndpoint(endpoint, out channelElement);
                    this.generatedChannelElements.Add(channelElement);
                    // generate the binding
                    string bindingSectionName;
                    string configurationName;
                    contractGenerator.GenerateBinding(endpoint.Binding, out bindingSectionName, out configurationName);
                    ThrowOnMetadataConversionErrors(contractGenerator.Errors);
					addedEndpoints.Add(endpoint.Name);
                }
            }

            // Save changes if specified.
            if (!string.IsNullOrEmpty(options.OutputConfigurationFile))
            {
                configuration.Save(ConfigurationSaveMode.Modified);
            }
        }

        private void AddReferencedTypesAndKnownContracts(
            ContractGenerationOptions options,
            WsdlImporter wsdlImporter, 
            ServiceContractGenerator contractGenerator)
        {
            foreach (Type type in options.ReferencedTypes)
            {
                if (type.IsDefined(typeof(ServiceContractAttribute), false))
                {
                    try
                    {
                        ContractDescription contractDescription = ContractDescription.GetContract(type);
                        XmlQualifiedName xmlName = new XmlQualifiedName(contractDescription.Name, contractDescription.Namespace);
                        wsdlImporter.KnownContracts.Add(xmlName, contractDescription);
                        contractGenerator.ReferencedTypes.Add(contractDescription, type);
                        continue;
                    }
                    catch (Exception exception)
                    {
                        throw new TypeLoadException(
                            string.Format(CultureInfo.CurrentCulture,
                                Properties.Resources.ErrUnableToLoadReferenceType,
                                type.AssemblyQualifiedName), exception);
                    }
                }
            }
        }

        #endregion

        #region ServiceContractGenerator functions

        private ServiceContractGenerator CreateServiceContractGenerator(ContractGenerationOptions options,
            System.Configuration.Configuration inputConfiguration, CodeCompileUnit codeCompileUnit)
        {
            ServiceContractGenerator generator = new ServiceContractGenerator(codeCompileUnit, inputConfiguration);
            SetContractGeneratorOptions(options, generator);

            foreach (KeyValuePair<string, string> pair in options.NamespaceMappings)
            {
                generator.NamespaceMappings.Add(pair.Key, pair.Value);
            }

            return generator;
        }

        private void SetContractGeneratorOptions(ContractGenerationOptions options, ServiceContractGenerator contractGenerator)
        {
            if (options.GenerateAsyncMethods)
            {
                contractGenerator.Options |= ServiceContractGenerationOptions.AsynchronousMethods;
            }
            else
            {
                contractGenerator.Options &= ~ServiceContractGenerationOptions.AsynchronousMethods;
            }

            if (options.GenerateInternalTypes)
            {
                contractGenerator.Options |= ServiceContractGenerationOptions.InternalTypes;
            }
            else
            {
                contractGenerator.Options &= ~ServiceContractGenerationOptions.InternalTypes;
            }

            if (options.GenerateTypedMessages)
            {
                contractGenerator.Options |= ServiceContractGenerationOptions.TypedMessages;
            }
            else
            {
                contractGenerator.Options &= ~ServiceContractGenerationOptions.TypedMessages;
            }

			if (options.GenerateChannelInterface)
			{
				contractGenerator.Options |= ServiceContractGenerationOptions.ChannelInterface;
			}
			else
			{
				contractGenerator.Options &= ~ServiceContractGenerationOptions.ChannelInterface;
			}

			if (options.GenerateClientClass)
			{
				contractGenerator.Options |= ServiceContractGenerationOptions.ClientClass;
			}
			else
			{
				contractGenerator.Options &= ~ServiceContractGenerationOptions.ClientClass;
			}
        }

        #region Check for out params in VB

        private void PostProcessCode(CodeCompileUnit codeCompileUnit)
        {
            foreach (CodeNamespace codeNamespace in codeCompileUnit.Namespaces)
            {
                foreach (CodeTypeDeclaration declaration in codeNamespace.Types)
                {
                    ProcessTypeDeclaration(declaration);
                }
            }
        }

        private void ProcessTypeDeclaration(CodeTypeDeclaration codeClass)
        {
            foreach (CodeTypeMember member in codeClass.Members)
            {
                if (member is CodeTypeDeclaration)
                {
                    ProcessTypeDeclaration((CodeTypeDeclaration)member);
                }
                else if (member is CodeMemberMethod)
                {
                    CodeMemberMethod method = member as CodeMemberMethod;
                    foreach (CodeParameterDeclarationExpression expression in method.Parameters)
                    {
                        if (expression.Direction == FieldDirection.Out &&
                            !IsDefined(typeof(OutAttribute), expression.CustomAttributes))
                        {
                            expression.CustomAttributes.Add(this.OutAttribute);
                        }
                    }
                    continue;
                }
            }
        }

        private static bool IsDefined(Type type, CodeAttributeDeclarationCollection metadata)
        {
            foreach (CodeAttributeDeclaration declaration in metadata)
            {
                if (declaration.Name == type.FullName || 
                    declaration.Name == type.Name)
                {
                    return true;
                }
            }
            return false;
        }

        private CodeAttributeDeclaration OutAttribute
        {
            get
            {
                if (outAttribute == null)
                {
                    outAttribute = new CodeAttributeDeclaration(typeof(OutAttribute).FullName);
                }
                return outAttribute;
            }
        }

        #endregion

        #endregion

        #region WsdlImporter functions

        private void ImportMetadata(WsdlImporter wsdlImporter,
            out ServiceEndpointCollection endpoints, 
            out Collection<System.ServiceModel.Channels.Binding> bindings, 
            out Collection<ContractDescription> contracts)
        {
            endpoints = wsdlImporter.ImportAllEndpoints();
            bindings = wsdlImporter.ImportAllBindings();
            contracts = wsdlImporter.ImportAllContracts();
            ThrowOnMetadataConversionErrors(wsdlImporter.Errors);
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assembly = new AssemblyName(args.Name);
			List<string> referencedAssemblies = new List<string>(this.options.ReferencedAssemblies);
			string assemblyFile = referencedAssemblies.Find(delegate(string match)
            {
                return match.Contains(assembly.Name);
            });

            if (!string.IsNullOrEmpty(assemblyFile))
            {
                return Assembly.LoadFrom(assemblyFile);
            }

            // try resolving with AssemblyResolvePath if defined.
            if (!string.IsNullOrEmpty(this.options.AssemblyResolvePath))
            {
                assemblyFile = Path.Combine(this.options.AssemblyResolvePath, assembly.Name + ".dll");
                if(File.Exists(assemblyFile))
                {
                    return Assembly.LoadFrom(assemblyFile);
                }
            }

            return null;
        }

        private Collection<IPolicyImportExtension> LoadPolicyImportExtensions()
        {
            if (this.configuration != null)
            {
                ServiceModelConfigurationManager manager = new ServiceModelConfigurationManager(this.configuration);
                ClientSection client = manager.GetClient();
                if (client != null)
                {
                    return client.Metadata.LoadPolicyImportExtensions();
                }
            }
            return null;
        }

        private Collection<IWsdlImportExtension> LoadWsdlImportExtensions()
        {
            if (this.configuration != null)
            {
                ServiceModelConfigurationManager manager = new ServiceModelConfigurationManager(this.configuration);
                ClientSection client = manager.GetClient();
                if (client != null)
                {
                    return client.Metadata.LoadWsdlImportExtensions();
                }
            }
            return null;
        }

        private void RemoveUnneededSerializers(Collection<IWsdlImportExtension> wsdlImportExtensions)
        {
			if (this.options.SchemaImporterType == ContractGenerationOptions.ImporterType.XmlSerializerImporter)
            {
                if (wsdlImportExtensions == null)
                {
                    ClientSection clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
					if (clientSection == null)
					{
						return;
					}
                    wsdlImportExtensions = clientSection.Metadata.LoadWsdlImportExtensions();
                }
                RemoveExtension(typeof(DataContractSerializerMessageContractImporter), wsdlImportExtensions);
            }
        }

        private void RemoveExtension(Type extensionType, Collection<IWsdlImportExtension> wsdlImportExtensions)
        {
            for (int index = 0; index < wsdlImportExtensions.Count; index++)
            {
                if (wsdlImportExtensions[index].GetType() == extensionType)
                {
                    wsdlImportExtensions.RemoveAt(index);
                }
            }
        }

        private void AddStateForDataContractSerializerImport(ContractGenerationOptions options,
            WsdlImporter wsdlImporter, CodeCompileUnit codeCompileUnit)
        {
            XsdDataContractImporter xsdImporter = new XsdDataContractImporter(codeCompileUnit);
            xsdImporter.Options = CreateDataContractImportOptions(options);
            wsdlImporter.State.Add(typeof(XsdDataContractImporter), xsdImporter);
        }

        private void AddStateForXmlSerializerImport(ContractGenerationOptions options,
            WsdlImporter wsdlImporter, CodeCompileUnit codeCompileUnit)
        {
            XmlSerializerImportOptions importOptions = new XmlSerializerImportOptions(codeCompileUnit);
            importOptions.WebReferenceOptions = new WebReferenceOptions();
            importOptions.WebReferenceOptions.CodeGenerationOptions = CodeGenerationOptions.GenerateOrder |
                                                                      CodeGenerationOptions.GenerateProperties;
            if (options.EnableDataBinding)
            {
                importOptions.WebReferenceOptions.CodeGenerationOptions |= CodeGenerationOptions.EnableDataBinding;
            }
            if (options.GenerateAsyncMethods)
            {
                importOptions.WebReferenceOptions.CodeGenerationOptions |= CodeGenerationOptions.GenerateNewAsync;
            }

            importOptions.WebReferenceOptions.SchemaImporterExtensions.Add(typeof(TypedDataSetSchemaImporterExtension).AssemblyQualifiedName);
            importOptions.WebReferenceOptions.SchemaImporterExtensions.Add(typeof(DataSetSchemaImporterExtension).AssemblyQualifiedName);
            importOptions.CodeProvider = options.CodeProvider;

            string clrNamespace = null;
            options.NamespaceMappings.TryGetValue(ContractGenerationOptions.NamespaceMappingsAllKeyName, out clrNamespace);
            importOptions.ClrNamespace = clrNamespace;

            importOptions.WebReferenceOptions.Verbose = true;
            //importOptions.WebReferenceOptions.Style = ServiceDescriptionImportStyle.Client;

            wsdlImporter.State.Add(typeof(XmlSerializerImportOptions), importOptions);
        }

        private ImportOptions CreateDataContractImportOptions(ContractGenerationOptions options)
        {
            ImportOptions importOptions = new ImportOptions();
            importOptions.GenerateSerializable = options.GenerateSerializable;
            importOptions.GenerateInternal = options.GenerateInternalTypes;
            importOptions.ImportXmlType = options.ImportXmlType;
            importOptions.EnableDataBinding = options.EnableDataBinding;
            importOptions.CodeProvider = options.CodeProvider;

            foreach (Type type in options.ReferencedTypes)
            {
                importOptions.ReferencedTypes.Add(type);
            }

            foreach (Type type in options.ReferencedCollectionTypes)
            {
                importOptions.ReferencedCollectionTypes.Add(type);
            }

            foreach (KeyValuePair<string, string> pair in options.NamespaceMappings)
            {
                importOptions.Namespaces.Add(pair.Key, pair.Value);
            }

            return importOptions;
        }

		private void SyncUpCodeCompileUnit(Dictionary<object, object> state)
		{
			if (state == null)
			{
				return;
			}

			foreach (KeyValuePair<object, object> pair in state)
			{
				XsdDataContractImporter dcImporter = pair.Value as XsdDataContractImporter;
				if (dcImporter != null)
				{
					this.codeCompileUnit = dcImporter.CodeCompileUnit;
					return;
				}
				XmlSerializerImportOptions xmlImporter = pair.Value as XmlSerializerImportOptions;
				if (xmlImporter != null)
				{
					this.codeCompileUnit = xmlImporter.CodeCompileUnit;
					return;
				}
			}
		}

        #endregion

        #endregion
    }
}
