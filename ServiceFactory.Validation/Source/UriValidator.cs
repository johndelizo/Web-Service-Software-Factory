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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// A validator class that checks a URI
	/// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class UriValidator : Validator<string>
	{
		public UriValidator()
			: base(null, null)
		{
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
		public UriValidator(NameValueCollection attributes)
			: base(null, null)
		{
		}

		protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			bool response = false;
			Uri uri;

			if(!String.IsNullOrEmpty(objectToValidate))
			{
				response = Uri.TryCreate(objectToValidate, UriKind.Absolute, out uri);

				if(!response)
				{
					this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
				}
			}
		}

		protected override string DefaultMessageTemplate
		{
			get { return Resources.InvalidUriValidatorMessage; }
		}
	}
}