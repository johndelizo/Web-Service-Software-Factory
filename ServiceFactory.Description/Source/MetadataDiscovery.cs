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
using System.ServiceModel.Description;
using System.Web.Services.Discovery;
using System.Xml.Schema;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Net;
using System.IO;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.Description
{
    public class MetadataDiscovery
    {
        private Uri address;

        private const string RelativeMexUri = "./mex";
        private const string MexPath = "/mex";
        private const string DiscoPath = "?wsdl";
        private const string WSPolicyElementName = "Policy";
        private const string WSPolicyNamespace = "http://schemas.xmlsoap.org/ws/2004/09/policy";
		public const string MapFileExtension = ".map";

        /// <summary>Initializes a new instance of the <see cref="T:MetadataDiscovery"></see> class with the specified address.</summary>
        /// <param name="address">A URI. </param>
        /// <exception cref="T:System.ArgumentNullException">address is null. </exception>
        /// <exception cref="T:System.UriFormatException">address is empty.-or- 
        /// The scheme specified in address is not correctly formed. 
        /// See <see cref="M:System.Uri.CheckSchemeName(System.String)"></see>.
        /// -or- address contains too many slashes.
        /// -or- The password specified in address is not valid.
        /// -or- The host name specified in address is not valid.
        /// -or- The file name specified in address is not valid.
        /// -or- The user name specified in address is not valid.
        /// -or- The host or authority name specified in address cannot be terminated by backslashes.
        /// -or- The port number specified in address is not valid or cannot be parsed.
        /// -or- The length of address exceeds 65534 characters.
        /// -or- The length of the scheme specified in address exceeds 1023 characters.
        /// -or- There is an invalid character sequence in address.
        /// -or- The MS-DOS path specified in uriString must isFirstElementToValidate with c:\\.</exception>
        public MetadataDiscovery(string address)
            : this(new Uri(address))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:MetadataDiscovery"></see> class with the specified address.</summary>
        /// <param name="address">A URI. </param>
        /// <exception cref="T:System.ArgumentNullException">address is null. </exception>
        /// <exception cref="T:System.UriFormatException">address is empty.-or- 
        /// The scheme specified in address is not correctly formed. 
        /// See <see cref="M:System.Uri.CheckSchemeName(System.String)"></see>.
        /// -or- address contains too many slashes.
        /// -or- The password specified in address is not valid.
        /// -or- The host name specified in address is not valid.
        /// -or- The file name specified in address is not valid.
        /// -or- The user name specified in address is not valid.
        /// -or- The host or authority name specified in address cannot be terminated by backslashes.
        /// -or- The port number specified in address is not valid or cannot be parsed.
        /// -or- The length of address exceeds 65534 characters.
        /// -or- The length of the scheme specified in address exceeds 1023 characters.
        /// -or- There is an invalid character sequence in address.
        /// -or- The MS-DOS path specified in uriString must isFirstElementToValidate with c:\\.</exception>
        public MetadataDiscovery(Uri address)
        {
			Guard.ArgumentNotNull(address, "address");
            this.address = address;
        }

        /// <summary>
        /// Notifies when the <see cref="InspectMetadata"/> has completed.
        /// </summary>
        public event EventHandler<InspectMetadataCompletedEventArgs> InspectMetadataCompleted;

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public Uri Address
        {
            get { return address; }
        }

        /// <summary>
        /// Inspects the metadata.
        /// </summary>
        /// <returns></returns>
        public MetadataSet InspectMetadata()
        {
            return DiscoverMetadata(address);
        }

        /// <summary>
        /// Inspects the metadata async.
        /// </summary>
        public void InspectMetadataAsync()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(OnDiscoverMetadataCallback));
        }

		/// <summary>
		/// Writes all metadata documents, XML Schema Definition (XSD) schemas, and Service Descriptions 
		/// to the supplied file directory and creates a file that maps all the created files in that directory.
		/// </summary>
		/// <param name="mapFullPath">The name of the file to create or overwrite containing a map of all documents saved.</param>
		/// <returns>A <see cref="DiscoveryClientResultCollection"/> containing the results of all files saved.</returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public DiscoveryClientResultCollection WriteMetadata(string mapFullPath)
		{
			Guard.ArgumentNotNullOrEmptyString(mapFullPath, "mapFullPath");

			using (DiscoveryClientProtocol discovery = new DiscoveryClientProtocol())
			{				
				ResolveDiscovery(discovery, address);
				string directory = Path.GetDirectoryName(mapFullPath);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				return discovery.WriteAll(directory, Path.GetFileName(mapFullPath));
			}
		}

        /// <summary>
        /// Raises the <see cref="E:InspectMetadataCompleted"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:Microsoft.Practices.ServiceFactory.WCF.Description.InspectMetadataCompletedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnInspectMetadataCompleted(InspectMetadataCompletedEventArgs e)
        {
            if (this.InspectMetadataCompleted != null)
            {
                this.InspectMetadataCompleted(this, e);
            }
        }

		private void ResolveDiscovery(DiscoveryClientProtocol discovery, Uri address)
		{
			discovery.UseDefaultCredentials = true;
			discovery.AllowAutoRedirect = true;
			discovery.DiscoverAny(address.AbsoluteUri);
			discovery.ResolveAll();
			ThrowOnDiscoveryErrors(discovery.Errors, address.IsFile);
		}

        private MetadataSet DiscoverMetadata(Uri address)
        {
            if (UriSchemeSupportsDisco(address) &&
                !IsMexUri(address))
            {
				return GetMetadataWithDiscovery(address);
            }
            else
            {
                MetadataExchangeClient metadataTransferClient = 
					new MetadataExchangeClient(address, MetadataExchangeClientMode.MetadataExchange);
                return metadataTransferClient.GetMetadata();
            }
        }

		private MetadataSet GetMetadataWithDiscovery(Uri address)
		{
			try
			{
				using (DiscoveryClientProtocol discovery = new DiscoveryClientProtocol())
				{
					ResolveDiscovery(discovery, address);
					MetadataSet metadataSet = new MetadataSet();
					foreach (object document in discovery.Documents.Values)
					{
						metadataSet.MetadataSections.Add(CreateMetadataSection(document));
					}
					return metadataSet;
				}
			}
			catch (WebException wex)
			{
				Trace.TraceError(wex.Message);
				if (address.IsFile &&
				   !File.Exists(address.AbsolutePath))
				{
					throw new FileNotFoundException(
						string.Format(CultureInfo.CurrentCulture, Properties.Resources.FileNotFound, "address"), address.AbsolutePath);
				}
				throw;
			}
			catch (InvalidOperationException ioex)
			{
				Trace.TraceError(ioex.Message);
				// if we already have a Mex address or is a local file, rethrow.
				if (IsMexUri(address))
				{
					throw;
				}
				if (address.IsFile)
				{
					if (MapFileExists(address.LocalPath))
					{
						return DiscoverMetadataFromMapFile(address.LocalPath);
					}
					throw;
				}
				// retry with mex address
				return DiscoverMetadata(GetDefaultMexUri(address));
			}
		}

		private bool MapFileExists(string file)
		{
			return File.Exists(Path.ChangeExtension(file, MapFileExtension));
		}

		private MetadataSet DiscoverMetadataFromMapFile(string file)
		{
			string mapFile = Path.ChangeExtension(file, MapFileExtension);

			using (DiscoveryClientProtocol discovery = new DiscoveryClientProtocol())
			{
				discovery.ReadAll(mapFile);
				MetadataSet metadataSet = new MetadataSet();
				foreach (object document in discovery.Documents.Values)
				{
					metadataSet.MetadataSections.Add(CreateMetadataSection(document));
				}
				return metadataSet;
			}
		}

        private void ThrowOnDiscoveryErrors(DiscoveryExceptionDictionary errors, bool fromFile)
        {
            Exception exception = null;
            foreach (DictionaryEntry entry in errors)
            {
                Exception error = (Exception)entry.Value;
                if (!(fromFile && 
                      error is WebException))
                {
                    exception = new InvalidOperationException(error.Message, exception);
                    Trace.TraceWarning(error.Message);
                }
            }
            if (exception != null)
            {
                throw exception;
            }
        }

		private MetadataSection CreateMetadataSection(object document)
        {
            System.Web.Services.Description.ServiceDescription description = 
				document as System.Web.Services.Description.ServiceDescription;

            XmlSchema schema = document as XmlSchema;
            XmlElement element = document as XmlElement;
			MetadataSection section;

            if (description != null)
            {
                section = MetadataSection.CreateFromServiceDescription(description);
            }
            else if (schema != null)
			{			
                section = MetadataSection.CreateFromSchema(schema);
            }
            else if ((element != null) && IsPolicyElement(element))
            {
               section = MetadataSection.CreateFromPolicy(element, null);
            }
            else
            {
                section = new MetadataSection();
                section.Metadata = document;
            }

			return section;
        }

        private bool IsPolicyElement(XmlElement policy)
        {
            if (policy.NamespaceURI.Equals(WSPolicyNamespace, StringComparison.OrdinalIgnoreCase))
            {
                return (policy.LocalName == WSPolicyElementName);
            }
            return false;
        }

		//	FxCop: False positive. In this case we handle exceptions by remembering the exception.
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void OnDiscoverMetadataCallback(object state)
        {
            InspectMetadataCompletedEventArgs args = null;

            try
            {
                MetadataSet metadata = DiscoverMetadata(address);
                args = new InspectMetadataCompletedEventArgs(metadata);
            }
            catch (Exception exception)
            {
                args = new InspectMetadataCompletedEventArgs(exception);
            }
            finally
            {
                OnInspectMetadataCompleted(args);
            }
        }

		private bool UriSchemeSupportsDisco(Uri serviceAddress)
        {
			if (serviceAddress.Scheme != Uri.UriSchemeHttp)
            {
				return (serviceAddress.Scheme == Uri.UriSchemeHttps ||
						serviceAddress.Scheme == Uri.UriSchemeFile);
            }
            return true;
        }

		private bool IsMexUri(Uri serviceAddress)
        {
			return serviceAddress.AbsolutePath.EndsWith(MexPath, StringComparison.OrdinalIgnoreCase);
        }

		private bool IsDiscoUri(Uri serviceAddress)
        {
			return serviceAddress.Query.StartsWith(DiscoPath, StringComparison.OrdinalIgnoreCase);
        }

		private Uri GetDefaultMexUri(Uri serviceAddress)
        {
			if (IsMexUri(serviceAddress))
            {
				return serviceAddress;
            }
			if (IsDiscoUri(serviceAddress))
            {
				UriBuilder uri = new UriBuilder(serviceAddress);
                uri.Query = string.Empty;
				serviceAddress = uri.Uri;
            }
			if (serviceAddress.AbsoluteUri.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
				return new Uri(serviceAddress, RelativeMexUri);
            }
			return new Uri(serviceAddress.AbsoluteUri + MexPath);
        }
    }
}
