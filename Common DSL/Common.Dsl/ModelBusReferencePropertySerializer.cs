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
using Microsoft.VisualStudio.Modeling.Integration;
using System;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
    public abstract class ModelBusReferencePropertySerializer<TContract> : ModelBusReferencePropertySerializer
        where TContract : ModelElement
    {
        private const string MelSchema = "mel://";

        public abstract string FileExtension { get; }
        public abstract string LogicalAdapterId { get; }

        public ModelBusReferencePropertySerializer()
            : base()
        { }

        public override T GetValue<T>(SerializationContext serializationContext, string input)
        {
            // Detect old moniker DIS format and convert it to new MBR format
            if(IsV3reference(input))
            {
                return (T)ConvertToModelBusReference<T>(serializationContext, input);                
            }

            return base.GetValue<T>(serializationContext, input);
        }

        // MBR format:
        // modelbus://logicalAdapterId/model display name/element display name/adapter reference data.
        // V3 format:
        // mel://[DSLNAMESPACE]\[MODELELEMENTTYPE]\[MODELELEMENT]@[PROJECT]\[MODELFILE]
        private object ConvertToModelBusReference<T>(SerializationContext serializationContext, string input)             
        {
            if(serializationContext == null) throw new ArgumentNullException("serializationContext");
            if (string.IsNullOrWhiteSpace(input) ||
               !typeof(ModelBusReference).IsAssignableFrom(typeof(T)))
            {
                return default(T);
            }

            // filter out the schema part
            input = input.Replace(MelSchema, string.Empty);

            string[] data = input.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length != 2)
            {
                serializationContext.Result.AddMessage(BuildSerializationMessage(Properties.Resources.InvalidMoniker, input));
                return default(T);
            }

            string[] modelData = data[0].Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            if (modelData.Length != 3)
            {
                serializationContext.Result.AddMessage(BuildSerializationMessage(Properties.Resources.InvalidMoniker, input));
                return default(T);
            }

            string[] locationData = data[1].Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            if (locationData.Length != 2)
            {
                serializationContext.Result.AddMessage(BuildSerializationMessage(Properties.Resources.InvalidMoniker, input));
                return default(T);
            }
            // set full path to model file
            if(!Path.IsPathRooted(locationData[1])) locationData[1] = Path.Combine(Path.GetDirectoryName(serializationContext.Location), locationData[1]);

            ModelBusReference result = null;
            IModelBus bus = serializationContext[ModelBusReferencePropertySerializer.ModelBusLoadContextKey] as IModelBus;
            if (bus != null)
            {
                using (ModelBusAdapterManager manager = bus.GetAdapterManager(LogicalAdapterId))
                {
                    ModelBusReference reference = null;
                    if (manager.TryCreateReference(out reference, Path.ChangeExtension(locationData[1], FileExtension)))
                    {
                        using (ModelBusAdapter adapter = manager.CreateAdapter(reference))
                        {
                            IModelingAdapterWithStore storeAdapter = adapter as IModelingAdapterWithStore;
                            if (storeAdapter.Store != null)
                            {
                                foreach (ModelElement mel in FilterElementsByType(storeAdapter.Store, modelData[1]))
                                {
                                    if (ValidatorUtility.GetTargetName(mel).Equals(modelData[2], StringComparison.OrdinalIgnoreCase))
                                    {
                                        return adapter.GetElementReference(mel);
                                    }
                                }
                                // If we are still here, we could not find any match so try will all mels
                                foreach (ModelElement mel in FilterElementsByType(storeAdapter.Store, string.Empty))
                                {
                                    if (ValidatorUtility.GetTargetName(mel).Equals(modelData[2], StringComparison.OrdinalIgnoreCase))
                                    {
                                        return adapter.GetElementReference(mel);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            serializationContext.Result.AddMessage(BuildSerializationMessage(Properties.Resources.ReferenceElementNotFound, input));
            return result;
        }

        private bool IsV3reference(string input)
        {
            return !string.IsNullOrWhiteSpace(input) &&
                   (input.StartsWith(MelSchema, StringComparison.OrdinalIgnoreCase) ||
                    input.Contains("@"));
        }

        private SerializationMessage BuildSerializationMessage(string message, string input)
        {
            return new SerializationMessage(SerializationMessageKind.Warning,
                string.Format(CultureInfo.CurrentCulture, message, input),
                null, 0, 0);
        }

        private ICollection FilterElementsByType(Store store, string elementType)
        {
            if(elementType.StartsWith(typeof(TContract).Name, StringComparison.OrdinalIgnoreCase))
            {
                return store.ElementDirectory.FindElements<TContract>();
            }
            return (ICollection)store.ElementDirectory.AllElements;
        }
    }
}
