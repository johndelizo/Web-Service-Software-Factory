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
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.ServiceFactory.DataContracts;

namespace Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf
{
    [ExtensionProviderAttribute("B327A38E-00F0-4967-838B-8E8FCC93DB30", "WCF", "WCF Extension", typeof(DataContractDslDomainModel))]
    [CLSCompliant(false)]
    public class DataContractWcfExtensionProvider : ExtensionProviderBase
    {
		public const string ExtensionProviderPropertyCategory = "WCF Settings";

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContractWcfExtensionProvider"/> class.
        /// </summary>
        public DataContractWcfExtensionProvider()
        {
            ObjectExtenderList = new List<Type>();
            ObjectExtenderList.Add(typeof(WCFDataContract));
			ObjectExtenderList.Add(typeof(WCFFaultContract));
			ObjectExtenderList.Add(typeof(WCFDataElement));
			ObjectExtenderList.Add(typeof(WCFDataContractEnum));
			ObjectExtenderList.Add(typeof(WCFDataContractCollection));
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
