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
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.ServiceFactory.HostDesigner;

namespace Microsoft.Practices.ServiceFactory.Extenders.HostDesigner.Asmx
{
	[Microsoft.Practices.Modeling.ExtensionProvider.Metadata.ExtensionProvider("FF107115-D18B-4F82-9B82-D6F8C77B8693", "ASMX", "ASMX Extensions", typeof(HostDesignerDomainModel))]
	public class HostDesignerAsmxExtensionProvider : ExtensionProviderBase
	{
		public const string ExtensionProviderPropertyCategory = "ASMX Settings";

		/// <summary>
		/// Initializes a new instance of the <see cref="HostDesignerAsmxExtensionProvider"/> class.
		/// </summary>
		public HostDesignerAsmxExtensionProvider()
		{
			ObjectExtenderList = new List<Type>();

			ObjectExtenderList.Add(typeof(AsmxServiceDescription));
		}

		public override IList<Type> ObjectExtenders
		{
			get { return ObjectExtenderList; }
		}
	}
}
