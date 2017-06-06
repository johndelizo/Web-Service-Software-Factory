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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Collections;
using System.IO;
using System.ServiceModel;
using Microsoft.Practices.ServiceFactory.Description;
using Microsoft.Practices.UnitTestLibrary.Utilities;

namespace Microsoft.Practices.ServiceFactory.Description.Tests
{
    [TestClass]
    public class ServiceModelConfigurationManagerFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowOnCreateWithNullConfiguration()
        {
            new ServiceModelConfigurationManager(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
		[DeploymentItem(@"SampleData\ServiceModel\InvalidServiceModelConfiguration.config")]
        public void ThrowOnInvalidServiceModelConfiguration()
        {
			System.Configuration.Configuration config = ConfigurationLoader.LoadConfiguration(@"SampleData\ServiceModel\InvalidServiceModelConfiguration.config");
            new ServiceModelConfigurationManager(config);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void GetServiceNamesTest()
        {
            IList<string> actual = LoadManager().GetServiceNames();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
            Assert.IsTrue(actual.Contains(Constants.ServiceName));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void GetServicesTest()
        {
            ServiceElementCollection services = LoadManager().GetServices();

            Assert.IsNotNull(services);
            Assert.IsTrue(services.Count > 0);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void GetBehaviorNamesTest()
        {
			IList<string> actual = LoadManager().GetBehaviorNames();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
            Assert.IsTrue(actual.Contains(Constants.ServiceBehaviorName));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void GetBindingNamesTest()
        {
			IList<string> actual = LoadManager().GetBindingNames();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
            bool passed = false;
            foreach (string bindingName in actual)
            {
                string[] names = ServiceModelConfigurationManager.SplitBindingName(bindingName);
                if (names[0] == Constants.ServiceBindingName &&
                    names[1] == Constants.WSHttpBindingName)
                {
                    passed = true;
                    break;
                }
            }
            Assert.IsTrue(passed);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void GetEndpointTest()
        {
            ServiceEndpointElement actual = LoadManager().GetEndpoint(Constants.ServiceName, Constants.EndpointName);

            Assert.AreEqual(actual.Contract, Constants.ContractType);
            Assert.AreEqual(actual.Binding, Constants.BasicHttpBindingName);
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ThrowOnEndpointNotFoundTest()
        {
            LoadManager().GetEndpoint(Constants.ServiceName, "NonExistentEndpoint");
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ThrowOnUpdateServiceEndpointNotFoundTest()
        {
            LoadManager().UpdateService(Constants.ServiceName,
                "NonExistentService", "bindingName", "binding", "behavior");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ThrowOnGetServiceThatNotExistsTest()
        {
            ServiceElement actual = LoadManager().GetService("unknown");
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void GetStandardBindingTest()
        {
            BasicHttpBindingElement element = LoadManager().GetStandardBinding<BasicHttpBinding, BasicHttpBindingElement>(Constants.ServiceBindingName);

            Assert.AreEqual(Constants.ServiceBindingName, element.Name);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void CanGetDefaultStandardBindingWithNullBindingName()
        {
            BasicHttpBindingElement defaultElement = new BasicHttpBindingElement("NewBinding");
            BasicHttpBindingElement element = LoadManager().GetStandardBinding<BasicHttpBinding, BasicHttpBindingElement>(null);

            Assert.IsTrue(string.IsNullOrEmpty(element.Name));
            Assert.AreEqual(defaultElement.Security, element.Security);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void AddStandardBindingTest()
        {
            BasicHttpBindingElement element = new BasicHttpBindingElement("newBasicBinding");
            ServiceModelConfigurationManager manager = LoadManager();
            manager.AddStandardBinding<BasicHttpBinding, BasicHttpBindingElement>(element);

            BasicHttpBindingElement addedElement = manager.GetStandardBinding<BasicHttpBinding, BasicHttpBindingElement>(element.Name);

            Assert.AreEqual(element.Name, addedElement.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(BindingNotFoundException))]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ThrowOnGetStandardBindingThatNotExistsTest()
        {
            BasicHttpBindingElement addedElement = LoadManager().GetStandardBinding<BasicHttpBinding, BasicHttpBindingElement>("NotExist");
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void AddCustomBindingTest()
        {
            CustomBindingElement element = new CustomBindingElement("newBasicBinding");
            ServiceModelConfigurationManager manager = LoadManager();
            manager.AddCustomBinding(element);

            CustomBindingElement addedElement = manager.GetCustomBinding("newBasicBinding");
            Assert.AreEqual(element.Name, addedElement.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(BindingNotFoundException))]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ThrowOnGetCustomBindingThatNotExistsTest()
        {
            CustomBindingElement element = LoadManager().GetCustomBinding("NotExist");
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void AddCustomBindingNewSectionTest()
        {
            // Remove any previous CustomBinding section
            ServiceModelConfigurationManager manager = LoadManager();
            manager.Configuration.Sections.Remove("customBinding");

            CustomBindingElement element = new CustomBindingElement("newBasicBinding");
            manager.AddCustomBinding(element);

            CustomBindingElement addedElement = manager.GetCustomBinding("newBasicBinding");

            Assert.AreEqual(element.Name, addedElement.Name);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void UpdateCustomBindingTest()
        {
            ServiceModelConfigurationManager manager = LoadManager();
            manager.AddCustomBinding(new CustomBindingElement(Constants.ServiceBindingName));

            CustomBindingElement element = manager.GetCustomBinding(Constants.ServiceBindingName);
            element.Add(new SecurityElement());
            element.Add(new HttpTransportElement());
            manager.AddCustomBinding(element);

            CustomBindingElement updatedElement = manager.GetCustomBinding(Constants.ServiceBindingName);

            Assert.AreEqual(2, updatedElement.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ThrowOnGetNullCustomBindingTest()
        {
            LoadManager().AddCustomBinding(null);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void GetBehaviorTest()
        {
            NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> element = LoadManager().GetBehavior(Constants.ServiceBehaviorName);

            Assert.IsNotNull(element);
            Assert.AreEqual(Constants.ServiceBehaviorName, element.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(BehaviorNotFoundException))]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ThrowOnBehaviorThatNotExistsTest()
        {
            NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> element = LoadManager().GetBehavior("NewBehavior");
            Assert.IsNotNull(element);
            Assert.AreEqual("NewBehavior", element.Name);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void GetBehaviorExtensionTest()
        {
            NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> element = LoadManager().GetBehavior(Constants.ServiceBehaviorExtension);
            ServiceCredentialsElement extension = ServiceModelConfigurationManager.GetBehaviorExtensionElement<ServiceCredentialsElement>(element);
            Assert.IsNotNull(extension);
            Assert.AreEqual(Constants.TestCert, extension.ServiceCertificate.FindValue);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void UpdateBehaviorExtensionTest()
        {
            NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> element = LoadManager().GetBehavior(Constants.ServiceBehaviorName);
            ServiceCredentialsElement extension = new ServiceCredentialsElement();
            extension.UserNameAuthentication.MembershipProviderName = Constants.MembershipProviderName;
            ServiceCredentialsElement updatedExtension = ServiceModelConfigurationManager.GetBehaviorExtensionElement<ServiceCredentialsElement>(element);

            Assert.IsNotNull(updatedExtension);
            Assert.AreEqual(updatedExtension.BehaviorType, extension.BehaviorType);
            Assert.AreEqual(Constants.MembershipProviderName, updatedExtension.UserNameAuthentication.MembershipProviderName);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void UpdateServiceTest()
        {
            ServiceModelConfigurationManager manager = LoadManager();
            manager.UpdateService(Constants.ServiceName, Constants.EndpointName, "someBinding", "customBinding", "someBehavior");

            ServiceElement service = manager.GetService(Constants.ServiceName);

            Assert.AreEqual("someBehavior", service.BehaviorConfiguration);

            ServiceEndpointElement endpointFound = null;
            foreach (ServiceEndpointElement endpoint in service.Endpoints)
            {
                if (endpoint.Binding.Equals("customBinding"))
                {
                    endpointFound = endpoint;
                    break;
                }
            }
            Assert.IsNotNull(endpointFound);
            Assert.AreEqual("customBinding", endpointFound.Binding);
            Assert.AreEqual("someBinding", endpointFound.BindingConfiguration);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ClientSectionTest()
        {
            ClientSection client = LoadManager().GetClient();
            Assert.IsNotNull(client);
            Assert.AreEqual(1, client.Endpoints.Count);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ClientBindingTest()
        {
            ServiceModelConfigurationManager manager = LoadManager();
            ClientSection client = manager.GetClient();
            CustomBindingElement binding = manager.GetCustomBinding(client.Endpoints[0].BindingConfiguration);
            SecurityElement securitySection = binding[typeof(SecurityElement)] as SecurityElement;
            Assert.IsNotNull(securitySection);
            Assert.AreEqual(AuthenticationMode.UserNameForSslNegotiated, securitySection.SecureConversationBootstrap.AuthenticationMode);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void ClientBehaviorTest()
        {
            ServiceModelConfigurationManager manager = LoadManager();
            ClientSection client = manager.GetClient();
            NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> behavior = manager.GetBehavior(client.Endpoints[0].BehaviorConfiguration);
            ClientCredentialsElement credentialsSection = ServiceModelConfigurationManager.GetBehaviorExtensionElement<ClientCredentialsElement>(behavior);

            Assert.AreEqual(Constants.TestCert, credentialsSection.ClientCertificate.FindValue);
            Assert.AreEqual(System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust,
                credentialsSection.ServiceCertificate.Authentication.CertificateValidationMode);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void CanGetClientEndpoint()
        {
            ServiceModelConfigurationManager manager = LoadManager();
            ChannelEndpointElement endpoint = manager.GetEndpoint(Constants.ServiceDescription.ClientEndpointName);
            
            Assert.AreEqual(Constants.ServiceDescription.ClientEndpointName, endpoint.Name);
            Assert.AreEqual(Constants.ServiceDescription.ClientBehaviorConfiguration, endpoint.BehaviorConfiguration);
            Assert.AreEqual(Constants.ServiceDescription.ClientBindingName, endpoint.Binding);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void CanAddNewEndpoint()
        {
            ServiceEndpointElement mexEndpoint = ServiceModelConfigurationManager.GetMetadataExchangeEndpoint();
            ServiceModelConfigurationManager manager = LoadManager();
            manager.AddEndpoint(Constants.ServiceName, mexEndpoint);

            Assert.AreEqual(mexEndpoint,
                manager.GetEndpoint(Constants.ServiceName, manager.GetEndpointName(mexEndpoint)));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void CanAddExistingEndpoint()
        {
            ServiceModelConfigurationManager manager = LoadManager();
            ServiceEndpointElement endpoint = manager.GetEndpoint(Constants.ServiceName, Constants.EndpointName);
            manager.AddEndpoint(Constants.ServiceName, endpoint);

            Assert.AreEqual(endpoint, manager.GetEndpoint(Constants.ServiceName, Constants.EndpointName));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void CanGetEndpointName()
        {
            ServiceEndpointElement endpoint = new ServiceEndpointElement(
                new Uri(Constants.EndpointAddressClientConfigService), Constants.ContractType);
            endpoint.Binding = Constants.ServiceBindingName;
            endpoint.Name = Constants.EndpointName;

            string endpointName = LoadManager().GetEndpointName(endpoint);

            Assert.IsTrue(endpointName.Contains("Name=" + endpoint.Name));
            Assert.IsTrue(endpointName.Contains("Binding=" + endpoint.Binding));
            Assert.IsTrue(endpointName.Contains("Contract=" + endpoint.Contract));
            Assert.IsTrue(endpointName.Contains("Address=" + endpoint.Address.OriginalString));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void CanGetEndpointNamesWithoutMexEndpoints()
        {
            ServiceEndpointElement mexEndpoint = ServiceModelConfigurationManager.GetMetadataExchangeEndpoint();
            ServiceEndpointElement secureMexEndpoint = ServiceModelConfigurationManager.GetSecureMetadataExchangeEndpoint();
            ServiceModelConfigurationManager manager = LoadManager();
            manager.AddEndpoint(Constants.ServiceName, mexEndpoint);
            manager.AddEndpoint(Constants.ServiceName, secureMexEndpoint);

			IList<string> endpointNames = manager.GetEndpointNames(Constants.ServiceName);

            Assert.IsFalse(endpointNames.Contains(manager.GetEndpointName(mexEndpoint)));
            Assert.IsFalse(endpointNames.Contains(manager.GetEndpointName(secureMexEndpoint)));
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void CanGetEndpointNamesWithNullPredicate()
        {
            ServiceModelConfigurationManager manager = LoadManager();
            manager.AddEndpoint(Constants.ServiceName, ServiceModelConfigurationManager.GetSecureMetadataExchangeEndpoint());
			IList<string> endpointNames = manager.GetEndpointNames(Constants.ServiceName, null);

            Assert.AreEqual(3, endpointNames.Count);
        }

        [TestMethod]
        [DeploymentItem(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config")]
        public void CanCheckIfContainsServices()
        {
            ServiceModelConfigurationManager manager = LoadManager();
            Assert.IsTrue(manager.GetServices().Count > 0);
            Assert.IsTrue(manager.HasServices);
        }

        private ServiceModelConfigurationManager LoadManager()
        {
			System.Configuration.Configuration config = ConfigurationLoader.LoadConfiguration(@"SampleData\ServiceModel\ServiceModelConfigurationManagerFixture.config");
            return new ServiceModelConfigurationManager(config);
        }
    }
}
