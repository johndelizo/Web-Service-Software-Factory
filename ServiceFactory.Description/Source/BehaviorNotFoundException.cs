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
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.Practices.ServiceFactory.Description
{
	/// <summary>
	/// BehaviorNotFoundException class.
	/// </summary>
	[Serializable]
    public class BehaviorNotFoundException : Exception
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="T:BehaviorNotFoundException"/> class.
		/// </summary>
        public BehaviorNotFoundException() : base()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:BehaviorNotFoundException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
        public BehaviorNotFoundException(string message) : base(message)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:BehaviorNotFoundException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
        public BehaviorNotFoundException(string message, Exception innerException)
            : base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:BehaviorNotFoundException"/> class.
		/// </summary>
		/// <param name="info">Serialization info</param>
		/// <param name="context">Streaming context</param>
		protected BehaviorNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
    }
}
