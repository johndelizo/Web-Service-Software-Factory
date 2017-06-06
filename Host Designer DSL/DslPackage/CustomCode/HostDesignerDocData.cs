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
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.Modeling.Common.Logging;
using System.Diagnostics;
using System;
using Microsoft.Practices.ServiceFactory.Common.Dsl;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.HostDesigner
{
    internal partial class HostDesignerDocData : IValidationControllerAccesor
	{
		ModelingDocDataObserver observer;
        ValidationOutputObserver validationObserver;

		public override void Initialize(Store sharedStore)
		{
			base.Initialize(sharedStore);
			observer = new ModelingDocDataObserver(this);
            validationObserver = new ValidationOutputObserver(this);
            this.ValidationController.AddObserver(validationObserver);
        }

        public override void ClearErrorListItems()
        {
            base.ClearErrorListItems();
            Logger.Clear();
        }

        // disable serialization on view invalidation operations
        // to avoid perf issues.
        protected override string SerializedModel
        {
            get
            {
                if (this.Store.TransactionManager.InTransaction &&
                    this.Store.TransactionManager.CurrentTransaction.IsSerializing)
                {
                    return base.SerializedModel;
                }
                return string.Empty;
            }
        }

		protected override void Dispose(bool disposing)
		{
            try
            {
                if (disposing && observer != null)
                {
                    observer.Dispose();
                    observer = null;
                }
                this.ValidationController.RemoveObserver(validationObserver);
                base.Dispose(disposing);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
		}

		#region IValidationControllerAccesor Members

		public ValidationController Controller
		{
			get
			{
				return this.ValidationController;
			}
		}

		#endregion
	}
}