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

namespace Microsoft.Practices.UnitTestLibrary
{
    public class MockModelBusAdapterManager : ModelBusAdapterManager
    {
        public override bool CanCreateReference(params object[] modelLocatorInfo)
        {
            throw new NotImplementedException();
        }

        public override ModelBusReference CreateReference(params object[] modelLocatorInfo)
        {
            throw new NotImplementedException();
        }

        protected override ModelBusAdapterReference DeserializeAdapterReference(string serializedReference, ReferenceContext context)
        {
            throw new NotImplementedException();
        }

        protected override ModelBusAdapter DoCreateAdapter(ModelBusReference reference, IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<SupportedType> GetExposedElementTypes(string logicalAdapterId)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetSupportedLogicalAdapterIds()
        {
            throw new NotImplementedException();
        }

        protected override ModelBusView GetView(ModelBusAdapter viewOwner, ModelBusReference viewReference)
        {
            throw new NotImplementedException();
        }

        protected override string SerializeAdapterReference(ModelBusAdapterReference reference, ReferenceContext context)
        {
            throw new NotImplementedException();
        }
    }
}
