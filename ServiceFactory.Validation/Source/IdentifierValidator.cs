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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.ServiceFactory.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Collections.Specialized;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// A validator class that checks if a string is a valid identifier.
    /// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class IdentifierValidator : Validator<string>
    {
        public const string CSharpLanguage = "C#";

        private CodeDomProvider provider;
		private bool optionalValue = false;
		private int length = 512;

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentifierValidator"/> class.
		/// </summary>
		public IdentifierValidator()
			: this(IdentifierValidator.CSharpLanguage, Resources.InvalidLanguageIdentifier)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentifierValidator"/> class.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		public IdentifierValidator(NameValueCollection attributes)
			: base(null, null)
		{
			string language = IdentifierValidator.CSharpLanguage;
			int tempLength = 0;

			if (attributes != null)
			{
				language = attributes.Get("language") ?? CSharpLanguage;
				Boolean.TryParse(attributes.Get("optionalValue"), out optionalValue);

				if(Int32.TryParse(attributes.Get("length"), out tempLength))
				{
					length = tempLength;
				}
			}
			Initialize(language);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:IdentifierValidator"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public IdentifierValidator( string errorMessage )
            : base(errorMessage, null)
        {
			Initialize(IdentifierValidator.CSharpLanguage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:IdentifierValidator"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="errorMessage">The error message.</param>
        public IdentifierValidator(string language, string errorMessage)
			: base(errorMessage, null)
        {
			Initialize(language);
        }

		private void Initialize(string language)
		{
			if (!CodeDomProvider.IsDefinedLanguage(language))
				throw new InvalidOperationException(Resources.IdentifierValidatorUnknownLanguage);

			provider = CodeDomProvider.CreateProvider(language);
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
			if (optionalValue && String.IsNullOrEmpty(objectToValidate))
				return;

			if (!provider.IsValidIdentifier(objectToValidate) || objectToValidate.Length > length)
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
			get { return Resources.InvalidLanguageIdentifier; }
		}
    }
}
