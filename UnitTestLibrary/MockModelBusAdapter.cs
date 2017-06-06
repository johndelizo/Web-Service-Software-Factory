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
    public class MockModelBusAdapter : ModelBusAdapter
    {
        public MockModelBusAdapter() : base(new MockModelBusReference(null), new MockModelBusAdapterManager())
        { }

        public override object ResolveElementReference(ModelBusReference elementReference)
        {
            MockModelBusReference reference = elementReference as MockModelBusReference;
            return reference.ReferencedElement;
        }

        public override string DisplayName
        {
            get { throw new NotImplementedException(); }
        }

        public override ModelBusView GetDefaultView()
        {
            throw new NotImplementedException();
        }

        public override ModelBusReference GetElementReference(object element)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<ModelBusReference> GetElementReferences(Type elementType)
        {
            throw new NotImplementedException();
        }

        public override ModelBusView GetView(ModelBusReference reference)
        {
            throw new NotImplementedException();
        }
    }
}
