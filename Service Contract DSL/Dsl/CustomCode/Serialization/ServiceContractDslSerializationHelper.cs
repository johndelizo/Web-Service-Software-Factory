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
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.ServiceFactory.Common.Dsl;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
    /// <summary>
    /// Customised serialization behavior to handle a custom schema resolver
    /// </summary>
    public partial class ServiceContractDslSerializationHelper
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Generated code.")]
        public override ServiceContractModel LoadModelAndDiagram(
            SerializationResult serializationResult, 
            Partition modelPartition, 
            string modelFileName, 
            Partition diagramPartition, 
            string diagramFileName, 
            ISchemaResolver schemaResolver, 
            ValidationController validationController, 
            ISerializerLocator serializerLocator)
        {
            ISchemaResolver resolver = new CustomModelingSchemaResolver(schemaResolver, "GeneratedCode\\ServiceContractSchema.xsd");

            return base.LoadModelAndDiagram(
                serializationResult,
                modelPartition,
                modelFileName,
                diagramPartition,
                diagramFileName,
                resolver,
                validationController,
                serializerLocator);
        }
    }
}
