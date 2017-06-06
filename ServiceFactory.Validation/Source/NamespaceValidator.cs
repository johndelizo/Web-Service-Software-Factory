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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Specialized;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary/>
	public class NamespaceValidator : IdentifierValidator
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceValidator"/> class.
		/// </summary>
		public NamespaceValidator()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:NamespaceValidator"/> class.
		/// </summary>
		/// <param name="errorMessage">Message posted to the validation context when
		/// validation rule fails.</param>
		public NamespaceValidator(string errorMessage)
			: base(errorMessage)
		{
		}
		
		/// <summary>
        /// Initializes a new instance of the <see cref="T:NamespaceValidator"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="errorMessage">The error message.</param>
        public NamespaceValidator(string language, string errorMessage)
			: base(language, errorMessage)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="NamespaceValidator"/> class.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		public NamespaceValidator(NameValueCollection attributes)
			: base(attributes)
		{
		}

		/// <summary>
		/// Does the validate.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The current target.</param>
		/// <param name="key">The key.</param>
		/// <param name="validationResults">The validation results.</param>
		[SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation")]
		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			string stringToValidate = objectToValidate ?? string.Empty;
			string[] parts = stringToValidate.Split('.');
			string previousPart = string.Empty;

			// validate each part of the namespace
			foreach (string part in parts)
			{
				base.DoValidate(part, currentTarget, key, validationResults);
				// validates for two consecutive parts with the same value
				if (validationResults.IsValid && 
					String.Compare(part, previousPart, StringComparison.CurrentCulture) == 0)
				{
					this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
				}
				previousPart = part;
			}
		}
    }
}
