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
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.UnitTestLibrary
{
    public class MockModelBusReference : ModelBusReference
    {
        public ModelElement ReferencedElement { get; private set; }

        public MockModelBusReference(ModelElement referencedElement) : 
            base(new MockModelBus(), 
            "someId", 
            GetElementName(referencedElement),
            GetElementName(referencedElement),
            "serializedReference")
        {
            this.ReferencedElement = referencedElement;
            this.LastStatus = ReferenceStatus.FullyResolved;
        }

        private static string GetElementName(ModelElement element)
        {
            return element != null ?
                ValidatorUtility.GetTargetName(element) :
                Guid.NewGuid().ToString();
        }
    }
}
