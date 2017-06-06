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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validates if the specified value is a valid URI string or an existing file.
	/// </summary>
	public class LocationValidator : AndCompositeValidator
	{
		private string errorMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocationValidator"/> class.
		/// </summary>
		/// <param name="extension">The extension.</param>
		public LocationValidator(string extension) : this(extension, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocationValidator"/> class.
		/// </summary>
		public LocationValidator(string extension, string errorMessage)
			: base(
				new NonEmptyStringValidator(),
				new FileExistsValidator(),
				new FileExtensionValidator(extension))
		{
			this.errorMessage = errorMessage;
		}

        /// <summary>
        /// Does the validate.
        /// </summary>
        /// <param name="objectToValidate">The object to validate.</param>
        /// <param name="currentTarget">The current target.</param>
        /// <param name="key">The key.</param>
        /// <param name="validationResults">The validation results.</param>
        public override void DoValidate(
            object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            Uri uri = null;
            bool isUri = Uri.TryCreate(objectToValidate as string, UriKind.Absolute, out uri);

            if (isUri)
            {
                if (uri.IsFile)
                {
                    base.DoValidate(objectToValidate, currentTarget, key, validationResults);
                }
            }
            else
            {
                this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
            }
        }

		/// <summary>
		/// Gets the default message template.
		/// </summary>
		/// <value>The default message template.</value>
		protected override string DefaultMessageTemplate
		{
			get	{ return this.errorMessage ?? base.DefaultMessageTemplate; }
		}
	}
}
