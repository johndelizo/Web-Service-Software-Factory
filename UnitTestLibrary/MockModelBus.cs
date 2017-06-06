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
using Microsoft.VisualStudio.Modeling.Integration;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.UnitTestLibrary
{
    public class MockModelBus : IModelBus
    { 
        public MockModelBus()
        {
        }

        public ModelBusAdapter CreateAdapter(ModelBusReference reference, System.IServiceProvider serviceProvider)
        {
            return new MockModelBusAdapter();
        }

        public ModelBusAdapter CreateAdapter(ModelBusReference reference)
        {
            return new MockModelBusAdapter();
        }

        public ModelBusReference DeserializeReference(string serializedReference, ReferenceContext context)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<ModelBusAdapterManager> FindAdapterManagers(params object[] modelLocatorInfo)
        {
            throw new System.NotImplementedException();
        }

        public ModelBusAdapterManager GetAdapterManager(string id)
        {
            throw new System.NotImplementedException();
        }

        public void LogError(ErrorCategory errorCategory, string message)
        {
            throw new System.NotImplementedException();
        }

        public string SerializeReference(ModelBusReference reference)
        {
            throw new System.NotImplementedException();
        }

        public ReferenceStatus ValidateReference(ModelBusReference reference, ValidateReferenceOption validationOption)
        {
            throw new System.NotImplementedException();
        }

        public object GetService(System.Type serviceType)
        {
            throw new System.NotImplementedException();
        }
    }

}
