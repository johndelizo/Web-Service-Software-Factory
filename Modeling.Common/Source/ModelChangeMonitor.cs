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
using System.Runtime.Caching;
using System.Globalization;
using Microsoft.VisualStudio.Modeling.Shell;

namespace Microsoft.Practices.Modeling.Common
{
    public class ModelChangeMonitor : ChangeMonitor
    {
        private string uniqueId;
 
        public ModelChangeMonitor(DocData docData)
        {
            bool initialized = false;
            try
            {
                this.uniqueId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
                docData.DocumentClosed += OnDocumentClosed;
                initialized = true;
            }
            finally
            {
                base.InitializationComplete();
                if (!initialized) base.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override string UniqueId
        {
            get { return this.uniqueId; }
        }

        private void OnDocumentClosed(object sender, EventArgs e)
        {
            base.OnChanged(null);
        }
    }
}
