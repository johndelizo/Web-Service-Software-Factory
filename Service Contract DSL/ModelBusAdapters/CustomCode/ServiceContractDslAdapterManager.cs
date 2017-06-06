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
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.Modeling.Integration;
using Microsoft.Practices.ServiceFactory.Common.Dsl;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts.ModelBusAdapters
{
    public partial class ServiceContractDslAdapterManager
    {
        public override bool CanCreateReference(params object[] modelLocatorInfo)
        {
            return GetFileLocation(modelLocatorInfo) != null;
        }

        public override ModelBusReference CreateReference(params object[] modelLocatorInfo)
        {
            string file = GetFileLocation(modelLocatorInfo);

            if (file != null)
            {
                // Create the part of the reference which depends on the Adapter
                ModelingAdapterReference mar = new ModelingAdapterReference(null, null, file);

                // And aggregate it with the adapter manager's part
                ModelBusReference mbr = new ModelBusReference(
                    this.ModelBus, ServiceContractDslAdapter.AdapterId, Path.GetFileNameWithoutExtension(file), mar);

                return mbr;
            }
            return null;
        }

        private string GetFileLocation(params object[] modelLocatorInfo)
        {
            return AdapterHelper.GetFileLocation(this.FileExtension, modelLocatorInfo);
        }
    }
}
