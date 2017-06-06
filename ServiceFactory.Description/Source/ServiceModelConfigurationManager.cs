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
using System.Collections.ObjectModel;
using System.ServiceModel.Configuration;
using System.Collections;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Globalization;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.Description
{
    /// <summary>
    /// This class manages the WCF configuration section.
    /// </summary>
    public class ServiceModelConfigurationManager
    {
        #region Constants & Fields

        public const string BindingNameSeparator = ", ";

        private System.Configuration.Configuration configuration;
        private ServiceModelSectionGroup serviceModelSectionGroup;

        private const string MexHttpBindingName = "mexHttpBinding";
        private const string MexHttpsBindingName = "mexHttpsBinding";
        private static readonly Uri MexAddress = new Uri("mex", UriKind.Relative);
        private static readonly string MexContractType = typeof(IMetadataExchange).Name;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public System.Configuration.Configuration Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        /// <summary>
        /// Gets the service model section.
        /// </summary>
        /// <value>The service model section.</value>
        public ServiceModelSectionGroup ServiceModelSection
        {
            get { return serviceModelSectionGroup; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceModelConfigurationManager"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ServiceModelConfigurationManager(System.Configuration.Configuration configuration)
        {
            this.configuration = configuration;
            this.serviceModelSectionGroup = ServiceModelSectionGroup.GetSectionGroup(this.configuration);
            
            if (serviceModelSectionGroup == null)
            {
                throw new System.Configuration.ConfigurationErrorsException(
                    Properties.Resources.ConfigurationSectionNotFoundException);
            }

            // check for valid WCF sections
            // will throw ConfigurationErrorsException on any invalid element
            ServicesSection services = this.serviceModelSectionGroup.Services;
			System.Diagnostics.Debug.Assert(services != null, "Config issue in serviceModelSectionGroup.Services");

            ClientSection client = this.serviceModelSectionGroup.Client;
			System.Diagnostics.Debug.Assert(client != null, "Config issue in serviceModelSectionGroup.Client");
		}

        #endregion

        #region Public Implementation

        #region Service methods

        /// <summary>
        /// Gets the service names.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetServiceNames()
        {
            IList<string> services = new Collection<string>();

            foreach (ServiceElement service in GetServices())
            {
                if (!string.IsNullOrEmpty(service.Name))
                {
                    services.Add(service.Name);
                }
            }

            return services;
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <returns></returns>
        public ServiceElementCollection GetServices()
        {
            if (serviceModelSectionGroup.Services != null &&
                serviceModelSectionGroup.Services.Services != null)
            {
                return serviceModelSectionGroup.Services.Services;
            }
            return new ServiceElementCollection();
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public ServiceElement GetService(string serviceName)
        {
            ServiceElement service = null;

            try
            {
                service = serviceModelSectionGroup.Services.Services[serviceName];
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, 
                    Properties.Resources.ServiceNotFoundException, serviceName));
            }

            return service;
        }

        /// <summary>
        /// Updates the service.
        /// </summary>
        /// <param name="serviceElement">The service element.</param>
		public void UpdateService(ServiceElement serviceElement)
        {
			Guard.ArgumentNotNull(serviceElement, "serviceElement");

            if (serviceModelSectionGroup.Services.Services.ContainsKey(serviceElement.Name))
            {
                serviceModelSectionGroup.Services.Services[serviceElement.Name] = serviceElement;
            }
            else
            {
                serviceModelSectionGroup.Services.Services.Add(serviceElement);
            }
        }

        /// <summary>
        /// Updates the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="bindingConfigurationName">Name of the binding configuration.</param>
        /// <param name="bindingName">Name of the binding.</param>
        /// <param name="behaviorName">Name of the behavior.</param>
        public void UpdateService(
            string serviceName,
            string endpointName,
            string bindingConfigurationName,
            string bindingName,
            string behaviorName)
        {
            // Update BehaviorConfiguration
            ServiceElement service = GetService(serviceName);
            service.BehaviorConfiguration = behaviorName;

            // Update BindingConfiguration and Binding
            ServiceEndpointElement endpoint = GetEndpoint(service, endpointName);
            if (!string.IsNullOrEmpty(bindingName))
            {
                endpoint.Binding = bindingName;
            }
            endpoint.BindingConfiguration = bindingConfigurationName;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has services.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has services; otherwise, <c>false</c>.
        /// </value>
        public bool HasServices
        {
            get { return serviceModelSectionGroup.Services.Services.Count > 0; }
        }

        #endregion

        #region Client methods

        /// <summary>
        /// Gets the configuration section for the client.
        /// </summary>
        /// <returns></returns>
        public ClientSection GetClient()
        {
            return serviceModelSectionGroup.Client;
        }

        #endregion

        #region Endpoint methods

        /// <summary>
        /// Gets the endpoint names. 
        /// By default will filter out any metadata exchange endpoint.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
		public IList<string> GetEndpointNames(string serviceName)
        {
			Guard.ArgumentNotNullOrEmptyString(serviceName, "serviceName");
			
			string mexEndpointName = GetEndpointName(ServiceModelConfigurationManager.GetMetadataExchangeEndpoint());
            string secureMexEndpointName = GetEndpointName(ServiceModelConfigurationManager.GetSecureMetadataExchangeEndpoint());
            return GetEndpointNames(serviceName, delegate(string endpointName)
            {
                return !endpointName.Equals(mexEndpointName, StringComparison.OrdinalIgnoreCase) &&
                       !endpointName.Equals(secureMexEndpointName, StringComparison.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// Gets the endpoint names.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="predicate">The predicate that will be used as a filter for the endpoint names.</param>
        /// <returns></returns>
        public IList<string> GetEndpointNames(string serviceName, Predicate<string> predicate)
        {
			Guard.ArgumentNotNullOrEmptyString(serviceName, "serviceName");

            ServiceElement service = GetService(serviceName);

            IList<string> endpoints = new Collection<string>();

            foreach (ServiceEndpointElement endpoint in service.Endpoints)
            {
                if (predicate == null ||
                    predicate(GetEndpointName(endpoint)))
                {
                    endpoints.Add(GetEndpointName(endpoint));
                }
            }

            return endpoints;
        }

        /// <summary>
        /// Gets the client endpoint.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <returns></returns>
		public ChannelEndpointElement GetEndpoint(string endpointName)
        {
			Guard.ArgumentNotNullOrEmptyString(endpointName, "endpointName");

            ChannelEndpointElement result = null;

            foreach (ChannelEndpointElement endpoint in GetClient().Endpoints)
            {
                if (endpointName.Equals(endpointName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    result = endpoint;
                    break;
                }
            }

            if (result == null)
            {
                throw new EndpointNotFoundException();
            }

            return result;
        }

        /// <summary>
        /// Gets the endpoint.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <returns></returns>
		public ServiceEndpointElement GetEndpoint(string serviceName, string endpointName)
        {
			Guard.ArgumentNotNullOrEmptyString(serviceName, "serviceName");
			Guard.ArgumentNotNullOrEmptyString(endpointName, "endpointName");

            return GetEndpoint(GetService(serviceName), endpointName);
        }

        /// <summary>
        /// Gets the endpoint.
        /// </summary>
		/// <param name="service">Name of the service.</param>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <returns></returns>
		public ServiceEndpointElement GetEndpoint(ServiceElement service, string endpointName)
        {
			Guard.ArgumentNotNull(service, "service");
			Guard.ArgumentNotNullOrEmptyString(endpointName, "endpointName");
			
			ServiceEndpointElement result = null;

            foreach (ServiceEndpointElement endpoint in service.Endpoints)
            {
                if (endpointName.Equals(GetEndpointName(endpoint),
                    StringComparison.OrdinalIgnoreCase))
                {
                    result = endpoint;
                    break;
                }
            }

            if (result == null)
            {
                throw new EndpointNotFoundException();
            }

            return result;
        }

        /// <summary>
        /// Gets the name of the endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
		public string GetEndpointName(ServiceEndpointElement endpoint)
        {
			Guard.ArgumentNotNull(endpoint, "endpoint");
			
			return BuildEndpointName(endpoint.Name,
                    endpoint.Address, endpoint.Binding, endpoint.Contract);
        }

        /// <summary>
        /// Adds the endpoint.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="endpoint">The endpoint.</param>
		public void AddEndpoint(string serviceName, ServiceEndpointElement endpoint)
        {
			Guard.ArgumentNotNullOrEmptyString(serviceName, "serviceName");
			Guard.ArgumentNotNull(endpoint, "endpoint");

            ServiceElement service = this.GetService(serviceName);
            string endpointName = this.GetEndpointName(endpoint);
            ServiceEndpointElement duplicate = null;
            foreach (ServiceEndpointElement element in service.Endpoints)
            {
                if (endpointName.Equals(GetEndpointName(element),
                    StringComparison.OrdinalIgnoreCase))
                {
                    duplicate = element;
                    break;
                }
            }
            if (duplicate != null)
            {
                service.Endpoints.Remove(duplicate);
            }
            service.Endpoints.Add(endpoint);
        }

        /// <summary>
        /// Gets the metadata exchange endpoint.
        /// </summary>
        /// <returns></returns>
        public static ServiceEndpointElement GetMetadataExchangeEndpoint()
        {
            ServiceEndpointElement mexEndpoint =
                new ServiceEndpointElement(MexAddress, MexContractType);
            mexEndpoint.Binding = MexHttpBindingName;

            return mexEndpoint;
        }

        /// <summary>
        /// Gets the secure metadata exchange endpoint.
        /// </summary>
        /// <returns></returns>
        public static ServiceEndpointElement GetSecureMetadataExchangeEndpoint()
        {
            ServiceEndpointElement mexEndpoint =
            new ServiceEndpointElement(MexAddress, MexContractType);
            mexEndpoint.Binding = MexHttpsBindingName;

            return mexEndpoint;
        }

        #endregion

        #region Binding methods

        /// <summary>
        /// Gets the binding names.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetBindingNames()
        {
            return GetBindingNames(null);
        }

        /// <summary>
        /// Gets the binding names with the format '[BindingConfiguratioName], [BindingName]'.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public IList<string> GetBindingNames(string serviceName)
        {
            IList<string> bindings = new Collection<string>();

            if (!string.IsNullOrEmpty(serviceName))
            {
                ServiceElement service = this.GetService(serviceName);
                foreach (ServiceEndpointElement endpoint in service.Endpoints)
                {
                    if (!string.IsNullOrEmpty(endpoint.BindingConfiguration))
                    {
                        bindings.Add(endpoint.BindingConfiguration);
                    }
                }
            }
            else
            {
                foreach (BindingCollectionElement binding in serviceModelSectionGroup.Bindings.BindingCollections)
                {
                    foreach (IBindingConfigurationElement element in binding.ConfiguredBindings)
                    {
                        if (!bindings.Contains(element.Name))
                        {
                            bindings.Add(element.Name + BindingNameSeparator + binding.BindingName);
                        }
                    }
                }
            }

            return bindings;
        }

        /// <summary>
        /// Splits the name of the binding. 
        /// </summary>
        /// <param name="bindingName">Name of the binding with the format '[BindingConfiguratioName], [BindingName]'.</param>
        /// <returns>First array element is BindingConfigurationName and second element is BindingName.</returns>
		public static string[] SplitBindingName(string bindingName)
        {
			Guard.ArgumentNotNullOrEmptyString(bindingName, "bindingName");

            return bindingName.Split(
                new string[] { ServiceModelConfigurationManager.BindingNameSeparator }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Builds the name of the endpoint.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
		public static string BuildEndpointName(string name,
            Uri address, string binding, string contract)
        {
			Guard.ArgumentNotNullOrEmptyString(binding, "binding");
			Guard.ArgumentNotNullOrEmptyString(contract, "contract");

			string endpointName = string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                endpointName = "Name=" + name + ", ";
            }

            if (address != null &&
                !string.IsNullOrEmpty(address.OriginalString))
            {
                endpointName += "Address=" + address.OriginalString + ", ";
            }

            endpointName += "Binding=" + binding + ", ";
            endpointName += "Contract=" + contract;

            return endpointName;
        }

        /// <summary>
        /// Gets the standard binding.
        /// </summary>
        /// <param name="bindingName">Name of the binding.</param>
        /// <returns></returns>
        public TBindingConfiguration GetStandardBinding<TStandardBinding, TBindingConfiguration>
            (string bindingName)
            where TStandardBinding : System.ServiceModel.Channels.Binding
            where TBindingConfiguration : StandardBindingElement, new()
        {
            TBindingConfiguration binding = null;

            if (!string.IsNullOrEmpty(bindingName))
            {
                StandardBindingCollectionElement<TStandardBinding, TBindingConfiguration> bindingElement =
                    GetStandardBindingElement<TStandardBinding, TBindingConfiguration>();
                if (bindingElement != null &&
                    bindingElement.Bindings.ContainsKey(bindingName))
                {
                    binding = bindingElement.Bindings[bindingName] as TBindingConfiguration;
                }
                if (binding == null)
                {
                    throw new BindingNotFoundException(
                        string.Format(CultureInfo.CurrentCulture, 
                        Properties.Resources.BindingNotFoundException, bindingName));
                }
            }
            else
            {
                // return an instance of the default standard binding.
                binding = new TBindingConfiguration();
            }

            return binding;
        }

        /// <summary>
        /// Adds the WS HTTP binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void AddStandardBinding<TStandardBinding, TBindingConfiguration>
            (TBindingConfiguration bindingConfiguration)
            where TStandardBinding : System.ServiceModel.Channels.Binding
            where TBindingConfiguration : StandardBindingElement, new()
        {
            StandardBindingCollectionElement<TStandardBinding, TBindingConfiguration> bindingElement =
                    GetStandardBindingElement<TStandardBinding, TBindingConfiguration>();
            if (bindingElement.Bindings.ContainsKey(bindingConfiguration.Name))
            {
                bindingElement.Bindings[bindingConfiguration.Name] = bindingConfiguration;
            }
            else
            {
                bindingElement.Bindings.Add(bindingConfiguration);
            }
        }


        /// <summary>
        /// Gets the custom binding.
        /// </summary>
        /// <param name="bindingName">Name of the binding.</param>
        /// <returns></returns>
        public CustomBindingElement GetCustomBinding(string bindingName)
        {
            try
            {
                return serviceModelSectionGroup.Bindings.CustomBinding.Bindings[bindingName];
            }
            catch (KeyNotFoundException)
            {
                throw new BindingNotFoundException(
                    string.Format(CultureInfo.CurrentCulture, 
                    Properties.Resources.BindingNotFoundException, bindingName));
            }
        }

        /// <summary>
        /// Adds the custom binding.
        /// </summary>
        /// <param name="customBinding">The custom binding.</param>
        public void AddCustomBinding(CustomBindingElement customBinding)
        {
            if (customBinding == null)
            {
                throw new ArgumentNullException("customBinding");
            }

            if (serviceModelSectionGroup.Bindings.CustomBinding == null)
            {
                serviceModelSectionGroup.Bindings.BindingCollections.Add(new CustomBindingCollectionElement());
            }
            serviceModelSectionGroup.Bindings.CustomBinding.Bindings.Remove(customBinding);
            serviceModelSectionGroup.Bindings.CustomBinding.Bindings.Add(customBinding);
        }

        /// <summary>
        /// Adds the service model extension element.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="element">The element.</param>
        public void AddServiceModelExtensionElement<TServiceModelExtensionElement>(
            ServiceModelExtensionCollectionElement<TServiceModelExtensionElement> collection,
            TServiceModelExtensionElement element)
            where TServiceModelExtensionElement : ServiceModelExtensionElement
        {
            if (collection.Contains(element))
            {
                collection.Remove(element);
            }
            collection.Add(element);
        }

        /// <summary>
        /// Gets the service model extension element.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="extensionType">Type of the extension.</param>
        /// <returns></returns>
        public ServiceModelExtensionElement GetServiceModelExtensionElement(
            NamedServiceModelExtensionCollectionElement<ServiceModelExtensionElement> collection,
            Type extensionType)
        {
            return collection[extensionType];
        }

        #endregion

        #region Behavior methods

        /// <summary>
        /// Gets the behavior names.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetBehaviorNames()
        {
            IList<string> behaviors = new Collection<string>();

            foreach (ServiceBehaviorElement behavior in serviceModelSectionGroup.Behaviors.ServiceBehaviors)
            {
                if (!string.IsNullOrEmpty(behavior.Name))
                {
                    behaviors.Add(behavior.Name);
                }
            }

            foreach (EndpointBehaviorElement behavior in serviceModelSectionGroup.Behaviors.EndpointBehaviors)
            {
                if (!string.IsNullOrEmpty(behavior.Name))
                {
                    behaviors.Add(behavior.Name);
                }
            }

            return behaviors;
        }

        /// <summary>
        /// Gets the service behaviors.
        /// </summary>
        /// <returns></returns>
        public ServiceBehaviorElementCollection GetServiceBehaviors()
        {
            return serviceModelSectionGroup.Behaviors.ServiceBehaviors;
        }

        /// <summary>
        /// Gets the endpoint behaviors.
        /// </summary>
        /// <returns></returns>
        public EndpointBehaviorElementCollection GetEndpointBehaviors()
        {
            return serviceModelSectionGroup.Behaviors.EndpointBehaviors;
        }

        /// <summary>
        /// Gets the behavior.
        /// </summary>
        /// <param name="behaviorName">Name of the behavior.</param>
        /// <returns></returns>
        public NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> GetBehavior(string behaviorName)
        {
            if (serviceModelSectionGroup.Behaviors.ServiceBehaviors.ContainsKey(behaviorName))
            {
                return (NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement>)
                serviceModelSectionGroup.Behaviors.ServiceBehaviors[behaviorName];
            }
            if (serviceModelSectionGroup.Behaviors.EndpointBehaviors.ContainsKey(behaviorName))
            {
                return (NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement>)
                serviceModelSectionGroup.Behaviors.EndpointBehaviors[behaviorName];
            }
            throw new BehaviorNotFoundException(
                string.Format(CultureInfo.CurrentCulture, 
                Properties.Resources.BehaviorNotFoundException, behaviorName));
        }

        /// <summary>
        /// Updates the behavior.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public void UpdateBehavior(NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> behavior)
        {
            if (behavior is ServiceBehaviorElement)
            {
                if (this.serviceModelSectionGroup.Behaviors.ServiceBehaviors.ContainsKey(behavior.Name))
                {
                    this.serviceModelSectionGroup.Behaviors.ServiceBehaviors[behavior.Name] = (ServiceBehaviorElement)behavior;
                }
                else
                {
                    this.serviceModelSectionGroup.Behaviors.ServiceBehaviors.Add((ServiceBehaviorElement)behavior);
                }
            }
            else if (behavior is EndpointBehaviorElement)
            {
                if (this.serviceModelSectionGroup.Behaviors.EndpointBehaviors.ContainsKey(behavior.Name))
                {
                    this.serviceModelSectionGroup.Behaviors.EndpointBehaviors[behavior.Name] = (EndpointBehaviorElement)behavior;
                }
                else
                {
                    this.serviceModelSectionGroup.Behaviors.EndpointBehaviors.Add((EndpointBehaviorElement)behavior);
                }
            }
        }

        /// <summary>
        /// Gets the behavior extension element.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        public static T GetBehaviorExtensionElement<T>(NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> behavior)
            where T : BehaviorExtensionElement, new()
        {
            T behaviorExtensionElement = behavior[typeof(T)] as T;

            if (behaviorExtensionElement == null)
            {
                behaviorExtensionElement = new T();
            }

            return behaviorExtensionElement;
        }

        /// <summary>
        /// Updates the behavior extension section.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <param name="behaviorExtensionSection">The behavior extension section.</param>
        public static void UpdateBehaviorExtensionSection<T>(NamedServiceModelExtensionCollectionElement<BehaviorExtensionElement> behavior, T behaviorExtensionElement)
            where T : BehaviorExtensionElement
        {
            T existingBehaviorExtensionElement = behavior[typeof(T)] as T;

            if (existingBehaviorExtensionElement != null)
            {
                behavior.Remove(existingBehaviorExtensionElement);
            }

            behavior.Add(behaviorExtensionElement);
        }

        #endregion

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            if (this.configuration != null)
            {
                this.configuration.Save(System.Configuration.ConfigurationSaveMode.Modified);
            }
        }

        #endregion

        #region Private implementation

        private StandardBindingCollectionElement<TStandardBinding, TBindingConfiguration> GetStandardBindingElement
            <TStandardBinding, TBindingConfiguration>()
            where TStandardBinding : System.ServiceModel.Channels.Binding
            where TBindingConfiguration : StandardBindingElement, new()
        {
            foreach (BindingCollectionElement binding in serviceModelSectionGroup.Bindings.BindingCollections)
            {
                if (binding.BindingType.Equals(typeof(TStandardBinding)))
                {
                    return binding as StandardBindingCollectionElement<TStandardBinding, TBindingConfiguration>;
                }
            }

            return null;
        }

        #endregion
    }
}
