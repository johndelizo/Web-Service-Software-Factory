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
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using System.Net;
using Microsoft.Practices.ServiceFactory.Description;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.Web.Services.Discovery;

namespace Microsoft.Practices.ServiceFactory.Description.Tests
{
    /// <summary>
    /// Summary description for MetadataDiscoveryFixture
    /// </summary>
    [TestClass]
    public class MetadataDiscoveryFixture
    {
        readonly Uri Address = new Uri("http://localhost:7777/Host/Service.svc");
        readonly Uri MexAddress = new Uri("http://localhost:7777/Host/Service.svc/mex");
        readonly Uri WsdlAddress = new Uri("http://localhost:7777/Host/Service.svc?wsdl");
        readonly Uri SslAddress = new Uri("https://localhost/Host/Service.svc");
        readonly Uri SslMexAddress = new Uri("https://localhost:7777/Host/Service.svc/mex");

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void ThrowOnInvalidAddress()
        {
            new MetadataDiscovery("invalid");
        }

        [TestMethod]
        public void CanCreateInstance()
        {
            MetadataDiscovery instance = new MetadataDiscovery(Address);
            Assert.AreEqual(Address, instance.Address);
        }

		[TestMethod]
		[ExpectedException(typeof(FileNotFoundException))]
		public void ThrowOnFileNotFound()
		{
			MetadataDiscovery discovery = new MetadataDiscovery(@"c:\foo");
			discovery.InspectMetadata();
		}

        [TestMethod]
        public void ShouldGetMetadataFromBasicHttpHostAddress()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address))
            {
                MetadataDiscovery instance = new MetadataDiscovery(Address);
                MetadataSet metadata = instance.InspectMetadata();
                AssertMetadataForBasicHttp(metadata);
            }
        }

        [TestMethod]
        public void ShouldGetMetadataFromBasicHttpHostWsdlAddress()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address))
            {
                MetadataDiscovery instance = new MetadataDiscovery(WsdlAddress);
                MetadataSet metadata = instance.InspectMetadata();
                AssertMetadataForBasicHttp(metadata);
            }
        }

        [TestMethod]
        public void ShouldGetMetadataFromBasicHttpHostWsdlAddressWithNoGet()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address, false, false))
            {
                MetadataDiscovery instance = new MetadataDiscovery(WsdlAddress);
                MetadataSet metadata = instance.InspectMetadata();
                AssertMetadataForBasicHttp(metadata);
            }
        }

        [TestMethod]
        public void ShouldGetMetadataFromBasicHttpHostMexAddress()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address))
            {
                MetadataDiscovery instance = new MetadataDiscovery(MexAddress);
                MetadataSet metadata = instance.InspectMetadata();
                AssertMetadataForBasicHttp(metadata);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void ThrowOnBasicHttpHostSslAddress()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address))
            {
                MetadataDiscovery instance = new MetadataDiscovery(SslAddress);
                instance.InspectMetadata();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThrowOnBasicHttpHostSslMexAddress()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address))
            {
                MetadataDiscovery instance = new MetadataDiscovery(SslMexAddress);
                instance.InspectMetadata();
            }
        }

        // InspectMetadata will try first with WSDL and if fails
        // will try with mex.
        // we call a mex endpint with no WSDL support
        // Notice that this test case will apply for Https as well.
        [TestMethod]
        public void ShouldGetMetadataFromBasicHttpHostDiscoveryProbing()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address, false, false))
            {
                MetadataDiscovery instance = new MetadataDiscovery(Address);
                MetadataSet metadata = instance.InspectMetadata();
                AssertMetadataForBasicHttp(metadata);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldFailWithNoMexEndpoint()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address, false, true))
            {
                MetadataDiscovery instance = new MetadataDiscovery(Address);
                MetadataSet metadata = instance.InspectMetadata();
            }
        }

        [TestMethod]
        public void ShouldGetMetadataFromBasicHttpHostAsync()
        {
            using (ServiceHost host = CreateBasicHttpHost(Address))
            {
                AutoResetEvent reset = new AutoResetEvent(false);
                MetadataDiscovery instance = new MetadataDiscovery(Address);
                instance.InspectMetadataCompleted += delegate(object sender, InspectMetadataCompletedEventArgs e)
                {
                    try
                    {
                        Assert.IsNull(e.Exception);
                        AssertMetadataForBasicHttp(e.Metadata);
                    }
                    finally
                    {
                        reset.Set();
                    }
                };
                instance.InspectMetadataAsync();
                reset.WaitOne(100000, true);
            }
        }

        // use an external web site with SSL enabled
        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void ThrowOnNoValidSslAddress()
        {
            MetadataDiscovery instance = new MetadataDiscovery(SslAddress);
            instance.InspectMetadata();
        }

        //// use an external web site with SSL enabled
        //[TestMethod]
        //public void CanGetMetadataFromValidSslAddress()
        //{
        //    MetadataDiscovery instance = new MetadataDiscovery("https://hdl620/IisHost/Service.svc"); //new Uri("https://hdl620/IisHost/Service.svc"));
        //    MetadataSet metadata = instance.InspectMetadata();
        //    AssertMetadataForWsHttp(metadata);
        //}

        [TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl0")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd0")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd1")]
        public void ShouldGetMetadataFromWsdlFile()
        {
			string wsdlLocation = ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\MockService.wsdl");
            MetadataDiscovery instance = new MetadataDiscovery(wsdlLocation);
            MetadataSet metadata = instance.InspectMetadata();
            AssertMetadataForWsdlFile(metadata);
        }

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl0")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd0")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd1")]
		public void ShouldGetWritenMetadataDocumentsToFiles()
		{
			string wsdlLocation = ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\MockService.wsdl");
			MetadataDiscovery instance = new MetadataDiscovery(wsdlLocation);
			string mapFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Test\", "MockService.map");
			DiscoveryClientResultCollection results = instance.WriteMetadata(mapFile);

			Assert.AreEqual<int>(4, results.Count);
		}

        private ServiceHost CreateBasicHttpHost(Uri address)
        {
            return CreateBasicHttpHost(address, true, false);
        }

        private ServiceHost CreateBasicHttpHost(Uri address, bool getEnabled, bool noMexEndpoint)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(MockService), address);
            // add endpoint with basicHttpBinding
            serviceHost.AddServiceEndpoint(typeof(IMockServiceContract2), new BasicHttpBinding(), address);
            if (false == noMexEndpoint)
            {
                // add mex endpoint and behavior
                ServiceMetadataBehavior mexBehavior = new ServiceMetadataBehavior();
                mexBehavior.HttpGetEnabled = getEnabled;
                serviceHost.Description.Behaviors.Add(mexBehavior);
                serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "/mex");
            }
            serviceHost.Open();
            return serviceHost;
        }

        private void AssertMetadataForBasicHttp(MetadataSet metadata)
        {
            WsdlImporter importer = new WsdlImporter(metadata);
            Collection<ContractDescription> contracts = importer.ImportAllContracts();
            Collection<Binding> bindings = importer.ImportAllBindings();
            ServiceEndpointCollection endpoints = importer.ImportAllEndpoints();

            Assert.IsTrue(metadata.MetadataSections.Count == 4 || metadata.MetadataSections.Count == 5);
            Assert.AreEqual(0, importer.Errors.Count);
            Assert.AreEqual(1, contracts.Count);
            Assert.AreEqual(1, bindings.Count);
            Assert.AreEqual(1, endpoints.Count);
            Assert.AreEqual("IMockServiceContract2", contracts[0].Name);
            Assert.AreEqual(typeof(BasicHttpBinding), bindings[0].GetType());
        }

        private void AssertMetadataForWsHttp(MetadataSet metadata)
        {
            WsdlImporter importer = new WsdlImporter(metadata);
            Collection<ContractDescription> contracts = importer.ImportAllContracts();
            Collection<Binding> bindings = importer.ImportAllBindings();
            ServiceEndpointCollection endpoints = importer.ImportAllEndpoints();

            Assert.IsTrue(metadata.MetadataSections.Count == 4 || metadata.MetadataSections.Count == 5);
            Assert.AreEqual(0, importer.Errors.Count);
            Assert.AreEqual(1, contracts.Count);
            Assert.AreEqual(1, bindings.Count);
            Assert.AreEqual(1, endpoints.Count);
            Assert.AreEqual("IMyService", contracts[0].Name);
            Assert.AreEqual(typeof(WSHttpBinding), bindings[0].GetType());
        }

        private void AssertMetadataForWsdlFile(MetadataSet metadata)
        {
            WsdlImporter importer = new WsdlImporter(metadata);

            Assert.AreEqual(4, metadata.MetadataSections.Count);
            Assert.AreEqual(0, importer.Errors.Count);
        }
    }
}
