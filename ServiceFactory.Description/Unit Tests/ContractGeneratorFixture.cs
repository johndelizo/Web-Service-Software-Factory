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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Threading;
using System.IO;
using System.ServiceModel.Description;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Xml;
using System.Collections.ObjectModel;
using System.Web.Services.Description;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Runtime.Serialization;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using Microsoft.Practices.ServiceFactory.Description;

namespace Microsoft.Practices.ServiceFactory.Description.Tests
{
    [TestClass]
    public class ContractGeneratorFixture 
    {
        readonly Uri HostAddress = new Uri(Constants.Uris.TestContractGenerationEndpointAddress);
        readonly Uri HostAddressMultiEndpoint = new Uri(Constants.Uris.TestContractGenerationMultiEndpointAddress);

        [TestMethod]
		public void CanCreateInstanceWithDefaultOptions()
        {
            ContractGenerator generator = new ContractGenerator(new ContractGenerationOptions());

            Assert.IsNull(generator.Configuration);
            Assert.AreEqual(0, generator.GeneratedChannelElements.Count);
            Assert.IsNotNull(generator.CodeCompileUnit);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\Empty.config", "ShouldGenerateContractFromWsdlWithDefaultOptions")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl0")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd0")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd1")]
        public void ShouldGenerateContractFromWsdlWithDefaultOptions()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfiguration = ConfigurationLoader.LoadConfiguration(@"SampleData\Empty.config", "ShouldGenerateContractFromWsdlWithDefaultOptions");
            Uri wsdlLocation = new Uri(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\MockService.wsdl"));
            ContractGenerator generator = new ContractGenerator(options);
            generator.Generate(GetMetadataSet(wsdlLocation));

            ContractGeneratorCommonAsserts(generator, options);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\Empty.config")]
        public void ShouldGenerateContractFromHostWithDefaultOptions()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfiguration = ConfigurationLoader.LoadConfiguration(@"SampleData\Empty.config");
            ContractGenerator generator = new ContractGenerator(options);
            generator.Generate(GetMetadataSet(HostAddress));

            ContractGeneratorCommonAsserts(generator, options);
        }

        [TestMethod]
        public void ShouldGenerateProtectionLevelSignValueFromHost()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
            ContractGenerator generator = new ContractGenerator(options);
            generator.Generate(GetMetadataSet(HostAddress));

            string proxyClass = GetClassFromCcu(options.CodeProvider, generator.CodeCompileUnit);
            Assert.IsNotNull(proxyClass);
            Assert.IsTrue(proxyClass.Contains("ProtectionLevel.Sign"), "Contains ProtectionLevel value.");
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl0")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd0")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd1")]
        public void ShouldGenerateProtectionLevelSignValueFromWsdlFile()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
            ContractGenerator generator = new ContractGenerator(options);
			Uri wsdlLocation = new Uri(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\MockService.wsdl"));
            generator.Generate(GetMetadataSet(wsdlLocation));

            string proxyClass = GetClassFromCcu(options.CodeProvider, generator.CodeCompileUnit);
            Assert.IsNotNull(proxyClass);
            Assert.IsTrue(proxyClass.Contains("ProtectionLevel.Sign"), "Contains ProtectionLevel value.");
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\Empty.config", "ShouldGenerateAndSaveConfigFile")]
        public void ShouldGenerateAndSaveConfigFile()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfigurationFile = ConfigurationLoader.GetConfigurationFilePath(@"SampleData\Empty.config", "ShouldGenerateAndSaveConfigFile", true);

            string originalConfig = File.ReadAllText(options.OutputConfigurationFile);
            ContractGenerator generator = new ContractGenerator(options);
            generator.Generate(GetMetadataSet(HostAddress));

            Assert.AreNotEqual(originalConfig, File.ReadAllText(options.OutputConfigurationFile));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\Empty.config", "ShouldGenerateConfigFileWithWsHttpBinding")]
        public void ShouldGenerateConfigFileWithWsHttpBinding()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfigurationFile = ConfigurationLoader.GetConfigurationFilePath(@"SampleData\Empty.config", "ShouldGenerateConfigFileWithWsHttpBinding");
            ContractGenerator generator = new ContractGenerator(options);
            generator.Generate(GetMetadataSet(HostAddress));
            string configFileContent = File.ReadAllText(options.OutputConfigurationFile);
            Assert.IsTrue(configFileContent.Contains("<wsHttpBinding>"));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\DescriptionModel\PolicyImporterNotFound.config")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThrowOnCustomPolicyImporterNotFound()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfiguration = ConfigurationLoader.LoadConfiguration(@"SampleData\DescriptionModel\PolicyImporterNotFound.config");
            ContractGenerator generator = new ContractGenerator(options);
            generator.Generate(GetMetadataSet(HostAddress));
        }

        [TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\WsdlImporterNotFound.config")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThrowOnCustomWsdlImporterNotFound()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfiguration = ConfigurationLoader.LoadConfiguration(@"SampleData\DescriptionModel\WsdlImporterNotFound.config");
            ContractGenerator generator = new ContractGenerator(options);
            generator.Generate(GetMetadataSet(HostAddress));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\Empty.config")]
        public void ShouldGenerateOneEndpointFromHostWithMultipleEndpoints()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfiguration = ConfigurationLoader.LoadConfiguration(@"SampleData\Empty.config");
            options.ImportedEndpointNames.Add(Constants.ServiceDescription.WsHttpHostClientName);
            ContractGenerator generator = new ContractGenerator(options);
            MetadataSet metadata = GetMetadataSet(HostAddressMultiEndpoint);
            generator.Generate(GetMetadataSet(HostAddressMultiEndpoint));

            ContractGeneratorCommonAsserts(generator, options);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl0")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd0")]
        [DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd1")]
        public void ShouldGenerateVBProxy()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
            options.CodeProvider = new VBCodeProvider();
            ContractGenerator generator = new ContractGenerator(options);
			Uri wsdlLocation = new Uri(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\MockService.wsdl"));
            generator.Generate(GetMetadataSet(wsdlLocation));

            string proxyClass = GetClassFromCcu(options.CodeProvider, generator.CodeCompileUnit);
            Assert.IsNotNull(proxyClass);
            Assert.IsTrue(proxyClass.Contains("Public Sub New()"), "Contains VB code");
        }

        [TestMethod]
		[DeploymentItem(@"SampleData\Empty.config")]        
		public void ShouldGenerateContractFromHostWithDataContractSerializerType()
        {
            ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfiguration = ConfigurationLoader.LoadConfiguration(@"SampleData\Empty.config");
            options.ClrNamespace = "Test.Namespace1";
            ContractGenerator generator = new ContractGenerator(options);
            generator.Generate(GetMetadataSet(HostAddress));

            string proxyClass = GetClassFromCcu(options.CodeProvider, generator.CodeCompileUnit);
            Assert.IsNotNull(proxyClass);
            Assert.IsTrue(proxyClass.Contains(options.ClrNamespace), "Contains namespace");
            Assert.IsTrue(proxyClass.Contains("MyDataContract"), "Contains MyDataContract class");

			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(proxyClass);

			Type generatedType = results.CompiledAssembly.GetType(options.ClrNamespace + ".MyDataContract", false);
			Assert.IsNotNull(generatedType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanThrowOnMetadataConversionErrors()
        {
            Collection<MetadataConversionError> errors = new Collection<MetadataConversionError>();
            errors.Add(new MetadataConversionError("test", false));
            ContractGenerator.ThrowOnMetadataConversionErrors(errors);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\Empty.config", "ShouldMergeConfigContentWithSameEndpoint")]
		public void ShouldMergeConfigContentWithSameEndpoint()
		{
			ContractGenerationOptions options = new ContractGenerationOptions();
			options.OutputConfigurationFile = ConfigurationLoader.GetConfigurationFilePath(@"SampleData\Empty.config", "ShouldMergeConfigContentWithSameEndpoint", true);

			string originalConfig = File.ReadAllText(options.OutputConfigurationFile);
			ContractGenerator generator = new ContractGenerator(options);
			MetadataSet metadata = GetMetadataSet(HostAddress);
			WsdlImporter importer = generator.CreateWsdlImporter(metadata);
			generator.Generate(importer);
			// generate twice in order to update and get only one endpoint in config
			generator.Generate(importer);

			Assert.AreNotEqual(originalConfig, File.ReadAllText(options.OutputConfigurationFile));

			// Assert config file
			ServiceModelConfigurationManager manager = new ServiceModelConfigurationManager(generator.Configuration);
			ClientSection client = manager.GetClient();
			Assert.AreEqual<int>(2, client.Endpoints.Count);
		}

		#region Private Methods

		private string CreateMockImportersAssembly(string assemblyName)
		{
			CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(
				File.ReadAllText(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\MockImporters.cs")),
				assemblyName);

			if (results.PathToAssembly == null)
			{
				return null;
			}

			return ConfigurationLoader.GetConfigurationFilePath(results.PathToAssembly);
		}

        private void ContractGeneratorCommonAsserts(ContractGenerator generator, ContractGenerationOptions options)
        {
            // Assert proxy class
            string proxyClass = GetClassFromCcu(options.CodeProvider, generator.CodeCompileUnit);
            Assert.IsNotNull(proxyClass);
            Assert.IsTrue(proxyClass.Contains("IMockServiceContract"));

            // Assert config file
            ServiceModelConfigurationManager manager = new ServiceModelConfigurationManager(generator.Configuration);
            ClientSection client = manager.GetClient();

            Assert.AreEqual(1, client.Endpoints.Count);
            ChannelEndpointElement createdEndpoint = client.Endpoints[0];
            Assert.AreEqual(Constants.Uris.TestContractGenerationEndpointAddress, createdEndpoint.Address.AbsoluteUri);
            Assert.AreEqual(Constants.ServiceDescription.WsHttpHostClientName, createdEndpoint.Name);
            Assert.AreEqual(Constants.ServiceDescription.WsHttpHostClientBinding, createdEndpoint.Binding);
        }

        private string GetClassFromCcu(CodeDomProvider provider, CodeCompileUnit ccu)
        {
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            using (StringWriter tw = new StringWriter(CultureInfo.InvariantCulture))
            {
                provider.GenerateCodeFromCompileUnit(ccu, tw, options);
                return tw.ToString();
            }
        }

        private MetadataSet GetMetadataSet(Uri address)
        {
            return GetMetadataSet(
             address,
             typeof(MockService),
             typeof(IMockServiceContract),
             typeof(IMockServiceContract2));
        }

        private MetadataSet GetMetadataSet(Uri address, 
            Type service, Type serviceContract, Type secondaryServiceContract)
        {
            MetadataDiscovery metadata = new MetadataDiscovery(address);
            if (!address.IsFile)
            {
                using (ServiceHost host = CreateWsHttpHost(address, service, serviceContract, secondaryServiceContract))
                {
                    return metadata.InspectMetadata();
                }
            }            
            return metadata.InspectMetadata();
        }

        private ServiceHost CreateWsHttpHost(Uri address)
        {
            return CreateWsHttpHost(
                address, 
                typeof(MockService), 
                typeof(IMockServiceContract), 
                typeof(IMockServiceContract2));
        }

        private ServiceHost CreateWsHttpHost(Uri address, 
            Type service, Type serviceContract, Type secondaryServiceContract)
        {
            ServiceHost serviceHost = new ServiceHost(service, address);
            
            if (address.Equals(HostAddressMultiEndpoint))
            {
                // add a second endpoint
                serviceHost.AddServiceEndpoint(secondaryServiceContract, new WSHttpBinding(), address);
                // now switch to the standard address
                address = HostAddress;
            }

            serviceHost.AddServiceEndpoint(serviceContract, new WSHttpBinding(), address);
            // add mex endpoint and behavior
            ServiceMetadataBehavior mexBehavior = new ServiceMetadataBehavior();
            mexBehavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(mexBehavior);
            serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "/mex");
            serviceHost.Open();
            return serviceHost;
		}

		#endregion
	}
}
