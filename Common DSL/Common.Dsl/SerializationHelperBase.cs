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
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
    public abstract class SerializationHelperBase<TModel, TDiagram> 
        where TModel : ModelElement 
        where TDiagram : Diagram
    {
        /// <summary>
        /// Customize Model Loading.
        /// </summary>
        /// <param name="serializationResult">Stores serialization result from the load operation.</param>
        /// <param name="partition">Partition in which the new DataContractModel instance will be created.</param>
        /// <param name="fileName">Name of the file from which the DataContractModel instance will be deserialized.</param>
        /// <param name="modelRoot">The root of the file that was loaded.</param>
        protected void OnPostLoadModel(SerializationResult serializationResult, Partition partition, string fileName, TModel modelRoot)
        {
        }

        /// <summary>
        /// Customize Model and Diagram Loading.
        /// </summary>
        /// <param name="serializationResult">Stores serialization result from the load operation.</param>
        /// <param name="modelPartition">Partition in which the new DslLibrary instance will be created.</param>
        /// <param name="modelFileName">Name of the file from which the DslLibrary instance will be deserialized.</param>
        /// <param name="diagramPartition">Partition in which the new DslDesignerDiagram instance will be created.</param>
        /// <param name="diagramFileName">Name of the file from which the DslDesignerDiagram instance will be deserialized.</param>
        /// <param name="modelRoot">The root of the file that was loaded.</param>
        /// <param name="diagram">The diagram matching the modelRoot.</param>
        protected void OnPostLoadModelAndDiagram(SerializationResult serializationResult, Partition modelPartition, string modelFileName, Partition diagramPartition, string diagramFileName, TModel modelRoot, TDiagram diagram)
        {
            foreach (SerializationMessage message in serializationResult)
            {
                if (message.Kind == SerializationMessageKind.Warning &&
                    message.Message.StartsWith(Properties.Resources.MissingIdKey, StringComparison.OrdinalIgnoreCase))
                {
                    SerializationUtilities.AddMessage(serializationResult, modelFileName, SerializationMessageKind.Info, Properties.Resources.MissingIdWarnings, 0, 0);
                    return;
                }
            }
        }
    }
}
