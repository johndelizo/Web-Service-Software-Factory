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
using System.IO;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.ServiceFactory.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary/>
	public class FileExtensionValidator : NonEmptyStringValidator
	{       
        private string extension;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FileExtensionValidator"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
		public FileExtensionValidator(string extension)
			: this(extension, Resources.FileExtensionValidatorMessage)
		{
			this.extension = extension;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FileExtensionValidator"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="errorMessage">The error message.</param>
		public FileExtensionValidator(string extension, string errorMessage)
			: base(errorMessage)
		{
			this.extension = extension;
		}

		/// <summary>
		/// Does the validate.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The current target.</param>
		/// <param name="key">The key.</param>
		/// <param name="validationResults">The validation results.</param>
		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			base.DoValidate(objectToValidate, currentTarget, key, validationResults);

			if (validationResults.IsValid &&
				!Path.GetExtension(objectToValidate).Equals(extension, StringComparison.OrdinalIgnoreCase))
			{
				this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
			}
		}
	}
}
