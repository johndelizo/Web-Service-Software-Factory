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
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ServiceFactory.Extenders.HostDesigner.Wcf
{
	/// <summary>
	/// The binding types for http
	/// </summary>
	public enum BindingType
	{
		/// <summary>
		/// Provides Basic Web Services interoperability.
		/// </summary>
		/// <remarks>
		/// This option provides interoperability with Web Services that implement the 
		/// WS-I Basic Profile and Basic Security Profile specifications. Choose this
		/// option if you are communicating with ASP.NET Web services or other Web Servicees
		/// that conform to the WS-I Basic Profile and Basic Security Profile.
		/// </remarks>
		//FXCOP: BindingType intentionally formatted to match format used by configuration file.
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
		basicHttpBinding,

		/// <summary>
		/// Provides Advanced Web Services interoperability.
		/// </summary>
		/// <remarks>
		/// This option provides interoperability with Web Services that support distributed
		/// transactions and secure, reliable sessions. Choose this option if you are communicating
		/// with Windows Communication Foundation (WCF) or other Web Services that conform to
		/// WS-* specifications.
		/// </remarks>
		//FXCOP: BindingType intentionally formatted to match format used by configuration file.
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
		wsHttpBinding,
	}
}
