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
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.ServiceFactory.Description;
using Microsoft.Practices.ServiceFactory.HostDesigner;
using SCModel = Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio.Modeling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Text;
using System.Xml;
using System.Globalization;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.VisualStudio.Modeling.Integration;

namespace Microsoft.Practices.ServiceFactory.Extenders.HostDesigner.Wcf.Strategies
{
	[CLSCompliant(false)]
	public class ServiceModelConfigurationStrategy : ICodeGenerationStrategy
	{
		#region Fields

		private IServiceProvider serviceProvider;
		private IList<Guid> projectReferences;
		private IList<string> assemblyReferences;
		private IList<LogEntry> errors;

		#endregion

		public ServiceModelConfigurationStrategy()
		{
			this.projectReferences = new List<Guid>();
			this.assemblyReferences = new List<string>();
			this.errors = new List<LogEntry>();
		}

		#region ICodeGenerationStrategy Members

		public CodeGenerationResults Generate(IArtifactLink link)
		{
			CodeGenerationResults result = new CodeGenerationResults();
			string serviceImplementationName = string.Empty;
			string serviceContractName = string.Empty;
            string serviceNamespace = string.Empty;
            const string behavior = "_Behavior";

			if (link is IModelReference)
			{
				this.serviceProvider =  Utility.GetData<IServiceProvider>(link);
				ProjectNode project = Utility.GetData<ProjectNode>(link);

				ServiceDescription serviceDescription = ((IModelReference)link).ModelElement as ServiceDescription;
				Configuration configuration = GetConfiguration(link, project);
				
				// abort if we got errors in config file
				if (configuration == null)
				{
					return result;
				}

				try
				{
					ServiceReference serviceReference = (ServiceReference)serviceDescription;
					SCModel.Service service = GetMelReference<SCModel.Service>(serviceReference.ServiceImplementationType);
					serviceImplementationName = ResolveTypeReference(service);
					serviceContractName = GetServiceContractName(service.ServiceContract);
                    serviceNamespace = service.ServiceContract.Namespace;

					ServiceModelConfigurationManager manager = new ServiceModelConfigurationManager(configuration);

					ServiceElement serviceElement = new ServiceElement();
					serviceElement.Name = serviceImplementationName;
                    serviceElement.BehaviorConfiguration = string.Concat(serviceImplementationName, behavior);

					foreach (Endpoint endpoint in serviceDescription.Endpoints)
					{
						ServiceEndpointElement endpointElement = new ServiceEndpointElement();
						endpointElement.Name = endpoint.Name;
						endpointElement.Contract = serviceContractName;
						endpointElement.Binding = ((WcfEndpoint)endpoint.ObjectExtender).BindingType.ToString();
						endpointElement.Address = new Uri(endpoint.Address ?? string.Empty, UriKind.RelativeOrAbsolute);
                        endpointElement.BindingNamespace = serviceNamespace;
						serviceElement.Endpoints.Add(endpointElement);
					}

					manager.UpdateService(serviceElement);

					ServiceBehaviorElement behaviorElement = new ServiceBehaviorElement();
                    behaviorElement.Name = string.Concat(serviceImplementationName, behavior);
					ServiceDebugElement debugElement = new ServiceDebugElement();
					debugElement.IncludeExceptionDetailInFaults = false;
					behaviorElement.Add(debugElement);

					if (((WcfServiceDescription)serviceDescription.ObjectExtender).EnableMetadataPublishing)
					{
						ServiceMetadataPublishingElement metadataPublishingElement = new ServiceMetadataPublishingElement();
						metadataPublishingElement.HttpGetEnabled = true;
						behaviorElement.Add(metadataPublishingElement);
						ServiceEndpointElement mexEndpointElement = ServiceModelConfigurationManager.GetMetadataExchangeEndpoint();
						serviceElement.Endpoints.Add(mexEndpointElement);
					}

					manager.UpdateBehavior(behaviorElement);
					manager.Save();

					result.Add(link.ItemPath, File.ReadAllText(configuration.FilePath));
				}
				finally
				{
					if (configuration != null && File.Exists(configuration.FilePath))
					{
						File.Delete(configuration.FilePath);
					}
				}
			}

			return result;
		}

		public IList<LogEntry> Errors
		{
			get { return errors; }
		}

		public IList<Guid> ProjectReferences
		{
			get { return projectReferences; }
		}

		public IList<string> AssemblyReferences
		{
			get { return assemblyReferences; }
		}

		#endregion

		#region Private Implementation

		private T GetService<T>()
		{
			return (T)this.serviceProvider.GetService(typeof(T));
		}

		private ArtifactLink GetArtifactLink(ModelElement modelElement)
		{
			ArtifactLink link = ArtifactLinkHelper.GetFirstArtifactLink(modelElement);
			if (link != null)
			{
				return link;
			}
			throw new NotImplementedException(Properties.Resources.ArtifactLinkNotImplemented);
		}

		private TReferencedMEL GetMelReference<TReferencedMEL>(ModelBusReference moniker) where TReferencedMEL : ModelElement
		{
			TReferencedMEL mel = (TReferencedMEL)ModelBusReferenceResolver.ResolveAndDispose(moniker);
			return mel;
		}

		private string GetServiceContractName(SCModel.ServiceContract serviceContract)
		{
            if (serviceContract != null && 
                serviceContract.ObjectExtender != null)
            {
                PropertyInfo pi = serviceContract.ObjectExtender.GetType().GetProperty("ServiceContractName");

                if (pi == null)
                    throw new ArgumentNullException(string.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.ExtenderNotFound,
                            serviceContract.GetType().ToString()
                            )
                        );

                ArtifactLink alink = GetArtifactLink(serviceContract);
                if (alink != null)
                {
                    return string.Concat(alink.Namespace, ".", pi.GetValue(serviceContract.ObjectExtender, null));
                }
            }

			return string.Empty;
		}

		private string ResolveTypeReference(ModelElement mel)
		{
			ArtifactLink alink = GetArtifactLink(mel);

			if (alink != null)
			{
				PropertyInfo property = mel.GetType().GetProperty("Name");
				if (property != null)
				{
					object value = property.GetValue(mel, null);
					return string.Concat(alink.Namespace, ".", value.ToString());
				}
			}

			return string.Empty;
		}

		private void LogError(Exception error)
		{
			LogEntry entry = new LogEntry(
				error,
				Properties.Resources.Generation_Error_Title,
				TraceEventType.Error,
				0);

			errors.Add(entry);
		}

		private Configuration GetConfiguration(IArtifactLink link, ProjectNode projectNode)
		{
			string tempFile = Path.GetTempFileName();
			FileInfo fileInfo = null;

			if (Path.IsPathRooted(link.ItemPath))
			{
				fileInfo = new FileInfo(link.ItemPath);
			}
			else
			{
				fileInfo = new FileInfo(Path.Combine(projectNode.ProjectDir, link.ItemPath));
			}

			if (File.Exists(fileInfo.FullName))
			{
				if (!IsValidConfigFile(fileInfo.FullName))
				{
					return null;
				}
				string configurationContent = File.ReadAllText(fileInfo.FullName);
				File.WriteAllText(tempFile, configurationContent);
			}
			else
			{
				CreateEmptyConfigurationFile(tempFile);
			}

			return OpenExeConfiguration(tempFile);
		}

		private static void CreateEmptyConfigurationFile(string filePath)
		{
			using (XmlTextWriter writer = new XmlTextWriter(filePath, new UTF8Encoding(true, true)))
			{
				writer.Formatting = Formatting.Indented;				
				writer.WriteStartDocument();
				writer.WriteStartElement("configuration");
				writer.WriteEndElement();
				writer.WriteEndDocument();
				writer.Flush();
			}
		}

		private Configuration OpenExeConfiguration(string configurationFile)
		{
			FileInfo fileInfo = new FileInfo(configurationFile);
			ExeConfigurationFileMap exeFileMap = new ExeConfigurationFileMap();
			exeFileMap.ExeConfigFilename = fileInfo.FullName;
			return ConfigurationManager.OpenMappedExeConfiguration(exeFileMap, ConfigurationUserLevel.None);
		}

		private bool IsValidConfigFile(string file)
		{
			try
			{
				OpenExeConfiguration(file);
				return true;
			}
			catch (ConfigurationErrorsException e)
			{
				LogError(e);
				return false;
			}
		}

		#endregion
	}
}