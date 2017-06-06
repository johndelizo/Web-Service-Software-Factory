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
using System.IO;
using System.Globalization;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.Description
{
	public class XmlSchemaElementMoniker
	{
		string xmlSchemaPath;
		string elementName;
		Uri uri;
		const string UriSchemeXsd = "xsd";
		const string UriHostName = "root";

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSchemaElementMoniker"/> class.
		/// </summary>
		/// <param name="moniker">The moniker string.</param>
		public XmlSchemaElementMoniker(string moniker) //: this(new Uri(EscapeToUriFormat(moniker)))
		{
			Guard.ArgumentNotNullOrEmptyString(moniker, "moniker");

			Uri uri = new Uri(EscapeToUriFormat(moniker));
			if (!uri.Scheme.Equals(UriSchemeXsd, StringComparison.OrdinalIgnoreCase))
			{
				throw new UriFormatException(Properties.Resources.BadXmlSchemaElementMonikerSchema);
			}
			Initialize(uri);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSchemaElementMoniker"/> class.
		/// </summary>
		/// <param name="xmlSchemaPath">The XmlSchema path.</param>
		/// <param name="elementName">Name of the element.</param>
		public XmlSchemaElementMoniker(string xmlSchemaPath, string elementName)
		{
			Guard.ArgumentNotNullOrEmptyString(xmlSchemaPath, "xmlSchemaPath");

			Uri uri;
			if (string.IsNullOrEmpty(elementName))
			{
				uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", UriSchemeXsd, xmlSchemaPath)); 
			}
			else
			{
				uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}/{3}?{4}",
								UriSchemeXsd, Uri.SchemeDelimiter, UriHostName, EscapeToUriFormat(xmlSchemaPath), elementName ?? string.Empty));
			}

			Initialize(uri);
		}

		/// <summary>
		/// Gets or sets the xmlSchema path.
		/// </summary>
		/// <value>The xmlSchema path.</value>
		public string XmlSchemaPath
		{
			get { return Uri.UnescapeDataString(xmlSchemaPath); }
		}

		/// <summary>
		/// Gets or sets the name of the element.
		/// </summary>
		/// <value>The name of the type.</value>
		public string ElementName
		{
			get { return elementName; }
		}

		/// <summary>
		/// Gets the XmlSchema URI.
		/// </summary>
		/// <value>The URI.</value>
		public Uri Uri
		{
			get { return this.uri; }
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="XmlSchemaUriBuilder"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="XmlSchemaUriBuilder"></see>.
		/// </returns>
		public override string ToString()
		{
			if (ElementName == null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", UriSchemeXsd, this.XmlSchemaPath);
			}
			return EscapeToFileFormat(this.uri.ToString());
		}

		private static string EscapeToFileFormat(string path)
		{
			string result = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

			if(result.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
			{
				result = result.Remove(result.Length - 1);
			}

			if (result.StartsWith(@"\", StringComparison.OrdinalIgnoreCase))
			{
				result = result.Substring(1);
			}

			if (result.IndexOf(@":\\",StringComparison.OrdinalIgnoreCase) != -1)
			{
				return result.Replace(@":\\", Uri.SchemeDelimiter); 
			}

			return result;
		}

		private static string EscapeToUriFormat(string uri)
		{
			Guard.ArgumentNotNullOrEmptyString(uri, "uri");
			return uri.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}

		private void Initialize(Uri uri)
		{
			this.uri = uri;
			this.xmlSchemaPath = EscapeToFileFormat(
				(this.uri.Host.Equals(UriHostName, StringComparison.Ordinal) ? string.Empty : this.uri.Host) + this.uri.AbsolutePath);
			this.elementName = (!string.IsNullOrEmpty(this.uri.Query) ? this.uri.Query.Substring(1) : null);
		}
	}
}
