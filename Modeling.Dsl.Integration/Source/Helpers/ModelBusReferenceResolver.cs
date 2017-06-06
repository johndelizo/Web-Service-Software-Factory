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
using Microsoft.Practices.Modeling.Common;
using System.Diagnostics;

namespace Microsoft.Practices.Modeling.Dsl.Integration.Helpers
{
    public class ModelBusReferenceResolver : IDisposable
    {
        ModelBusAdapter modelAdapter = null;

        public ModelElement Resolve(ModelBusReference reference)
        {
            if (reference != null)
            {
                this.modelAdapter = CreateModelBusAdapter(reference);
                return modelAdapter.ResolveElementReference(reference) as ModelElement;
            }
            return null;
        }

        public static ModelElement ResolveAndDispose(ModelBusReference reference)
        {
            if(reference == null) return null;
            using (ModelBusReferenceResolver resolver = new ModelBusReferenceResolver())
            {
                return resolver.Resolve(reference);
            }
        }

        public static ModelElement ResolveAndCache(ModelBusReference reference)
        {
            if (reference == null) return null;
            ModelBusAdapter modelAdapter = GlobalCache.AddOrGetExisting<ModelBusAdapter>(reference.ModelDisplayName, c => CreateModelBusAdapter(reference));
            return modelAdapter.ResolveElementReference(reference) as ModelElement;
        }

        private static ModelBusAdapter CreateModelBusAdapter(ModelBusReference reference)
        {
            IModelBus bus = reference.ModelBus ?? RuntimeHelper.ServiceProvider.GetService(typeof(IModelBus)) as IModelBus;
            return bus.CreateAdapter(reference);
        }

        #region IDisposable Members

        /// <summary>
        /// IDisposable.Dispose().
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Unregister the observer on dispose.
        /// </summary>
        private void Dispose(bool disposing)
        {
            Debug.Assert(disposing, "ModelBusReferenceResolver finalized without being disposed!");
            if (disposing && 
                this.modelAdapter != null)
            {
                this.modelAdapter.Dispose();
                this.modelAdapter = null;
            }
        }

        #endregion
    }
}
