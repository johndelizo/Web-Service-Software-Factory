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
using System.ServiceModel.Description;

namespace MockImporters
{
	public class MockCustomWsdlImporter : IWsdlImportExtension
	{
		public MockCustomWsdlImporter()
		{
		}

		#region IWsdlImportExtension Members

		public void BeforeImport(System.Web.Services.Description.ServiceDescriptionCollection wsdlDocuments, System.Xml.Schema.XmlSchemaSet xmlSchemas, System.Collections.Generic.ICollection<System.Xml.XmlElement> policy)
		{
			// not implemented
		}

		public void ImportContract(WsdlImporter importer, WsdlContractConversionContext context)
		{
			// not implemented
		}

		public void ImportEndpoint(WsdlImporter importer, WsdlEndpointConversionContext context)
		{
			// not implemented
		}

		#endregion
	}

	public class MockCustomPolicyImporter : IPolicyImportExtension
	{
		public MockCustomPolicyImporter()
		{
		}

		#region IPolicyImportExtension Members

		public void ImportPolicy(MetadataImporter importer, PolicyConversionContext context)
		{
			//not implemented;
		}

		#endregion
	}
}


