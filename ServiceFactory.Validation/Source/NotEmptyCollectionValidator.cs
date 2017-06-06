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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that a collection of type T is not empty
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class NotEmptyCollectionValidator<T> : Validator<ICollection<T>>
	{
		public NotEmptyCollectionValidator()
			: base(null, null)
		{
		}

		public NotEmptyCollectionValidator(string errorMessage)
			: base(errorMessage, null)
		{
		}

		protected override void DoValidate(ICollection<T> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if(objectToValidate == null || objectToValidate.Count == 0)
			{
				this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
			}
		}

		protected override string DefaultMessageTemplate
		{
			get { return Resources.EmptyCollectionValidatorMessage; }
		}
	}
}