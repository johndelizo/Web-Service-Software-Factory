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
using System.Text;
using System.ServiceModel.Description;

namespace Microsoft.Practices.ServiceFactory.Description
{
    /// <summary>
    /// The event args class for the 
    /// <see cref="SecureClientConfigPresenter.InspectMetadataCompleted"/> event handler.
    /// </summary>
    public class InspectMetadataCompletedEventArgs : EventArgs
    {
        private MetadataSet metadata;
        private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:InspectMetadataCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        internal InspectMetadataCompletedEventArgs(MetadataSet metadata)
        {
            this.metadata = metadata;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:InspectMetadataCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        internal InspectMetadataCompletedEventArgs(Exception exception)
        {
            this.exception = exception;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public MetadataSet Metadata
        {
            get { return metadata; }
        } 

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get { return exception; }
        } 
    }
}
