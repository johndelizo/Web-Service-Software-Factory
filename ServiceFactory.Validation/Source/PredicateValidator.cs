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

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary/>
	/// <typeparam name="T"></typeparam>
	public class PredicateValidator<T> : Validator<T>
	{
		private Predicate<T> predicate;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PredicateValidator&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		public PredicateValidator(Predicate<T> predicate, string messageTemplate)
			: base(messageTemplate, null)
		{
			this.predicate = predicate;
		}

		protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if(!predicate(objectToValidate))
			{
				this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
			}
		}

		protected override string DefaultMessageTemplate
		{
			get { return string.Empty; }
		}
	}
}