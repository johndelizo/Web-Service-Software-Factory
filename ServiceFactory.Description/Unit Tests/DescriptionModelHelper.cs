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
using Microsoft.Practices.ServiceFactory.Description;
using System.Collections.ObjectModel;
using Microsoft.Practices.UnitTestLibrary.Utilities;

namespace Microsoft.Practices.ServiceFactory.Description.Tests
{
	public static class DescriptionModelHelper
	{
		/// <summary>
		/// Creates the WSDL importer.
		/// </summary>
		/// <param name="wsdlFile">The WSDL file.</param>
		/// <returns></returns>
		public static WsdlImporter CreateImporter(string wsdlFile)
		{
			MetadataDiscovery discovery = new MetadataDiscovery(ConfigurationLoader.GetConfigurationFilePath(wsdlFile));
			ContractGenerator generator = CreateContractGenerator();
			WsdlImporter importer = generator.CreateWsdlImporter(discovery.InspectMetadata());			
			importer.ImportAllEndpoints();
			importer.ImportAllContracts();
			importer.ImportAllBindings();
			ContractGenerator.ThrowOnMetadataConversionErrors(importer.Errors);
			return importer;
		}

		private static ContractGenerator CreateContractGenerator()
		{
			ContractGenerationOptions options = new ContractGenerationOptions();

			options.GenerateClientClass = false;
			options.GenerateChannelInterface = false;
			options.GenerateAsyncMethods = false;
			options.GenerateInternalTypes = false;
			options.GenerateSerializable = false;
			options.GenerateTypedMessages = false;
			options.ImportXmlType = true;
			options.SchemaImporterType = ContractGenerationOptions.ImporterType.All;
			return new ContractGenerator(options);
		}
	}
}
