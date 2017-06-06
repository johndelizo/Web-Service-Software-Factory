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
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.Practices.ServiceFactory.DataContracts;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx
{
	[ExtensionProviderAttribute("B7DCA4AE-F363-4054-A6DA-5821B82A13FF", "ASMX", "ASMX Extension", typeof(DataContractDslDomainModel))]
    public class DataContractAsmxExtensionProvider : ExtensionProviderBase
    {
		public const string ExtensionProviderPropertyCategory = "ASMX Settings";
		
		/// <summary>
        /// Initializes a new instance of the <see cref="DataContractAsmxExtensionProvider"/> class.
        /// </summary>
		public DataContractAsmxExtensionProvider()
        {
            ObjectExtenderList = new List<Type>();
            ObjectExtenderList.Add(typeof(AsmxDataContract));
			ObjectExtenderList.Add(typeof(AsmxFaultContract));
			ObjectExtenderList.Add(typeof(AsmxDataElement));
			ObjectExtenderList.Add(typeof(AsmxDataContractEnum));
			ObjectExtenderList.Add(typeof(AsmxDataContractCollection));
		}

        /// <summary>
        /// Gets the object extenders.
        /// </summary>
        /// <value>The object extenders.</value>
        public override IList<Type> ObjectExtenders
        {
            get { return ObjectExtenderList; }
        }
    }
}
