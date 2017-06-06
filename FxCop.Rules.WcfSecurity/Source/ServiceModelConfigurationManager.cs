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
using System.ServiceModel.Configuration;
using System.Collections;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary>
    /// This class manages the WCF configuration section.
    /// </summary>
    public class ServiceModelConfigurationManager
    {
        /// <summary>
        /// Name of the service model section in the configuration file.
        /// </summary>
        public const string ServiceModelSectionName = "system.serviceModel";

        private System.Configuration.Configuration configuration;
        private ServiceModelSectionGroup serviceModelSectionGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ServiceModelConfigurationManager"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        // FxCop: False positive, access to the Services and Client properties causes validation.
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "client")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "services")]
        public ServiceModelConfigurationManager(System.Configuration.Configuration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            this.configuration = configuration;
            this.serviceModelSectionGroup = this.configuration.SectionGroups.Get(ServiceModelSectionName) as ServiceModelSectionGroup;

            if (serviceModelSectionGroup == null)
            {
                throw new System.Configuration.ConfigurationErrorsException();
            }

            // check for valid WCF sections
            // will throw ConfigurationErrorsException on any invalid element
            ServicesSection services = this.serviceModelSectionGroup.Services;
            ClientSection client = this.serviceModelSectionGroup.Client;
        }

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
                    throw new KeyNotFoundException("bindingName");
                }
            }
            else
            {
                // return in instance of the default standard binding.
                binding = new TBindingConfiguration();
            }

            return binding;
        }

        /// <summary>
        /// Gets the custom binding.
        /// </summary>
        /// <param name="bindingName">Name of the binding.</param>
        /// <returns></returns>
        public CustomBindingElement GetCustomBinding(string bindingName)
        {
            return serviceModelSectionGroup.Bindings.CustomBinding.Bindings[bindingName];
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
            throw new KeyNotFoundException("behaviorName");
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
    }
}
