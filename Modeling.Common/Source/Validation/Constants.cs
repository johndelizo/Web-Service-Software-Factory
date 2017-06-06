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

namespace Microsoft.Practices.Modeling.Validation
{
	/// <summary>
	/// Validation constants
	/// </summary>
	public sealed class Constants
	{
        private Constants() { }

		/// <summary>
		/// The validation code used in <see cref="Microsoft.VisualStudio.Modeling.Validation.ValidationContext"/> instances.
		/// </summary>
		public const string ValidationCode = "Validation";
		
		/// <summary>
		/// The tag value to force logging warnings messages from the <see cref="ValidationEngine"/> class.
		/// </summary>
		public const string LogWarningTag = "Warning";
	}
}
