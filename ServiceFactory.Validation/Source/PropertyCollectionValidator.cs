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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.CodeGeneration;
using System.Globalization;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that all elements in a collection of type T have unique values for a specified property.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class PropertyCollectionValidator<T> : Validator<IEnumerable<T>>
	{
		protected PropertyCollectionValidator()
			: base(null, null)
		{
		}

		/// <summary>
		/// Validate a single item in the collection.
		/// </summary>
		/// <remarks>
		/// Implement this in order to validate individual collection elements.
		/// </remarks>
		protected abstract void DoValidateCollectionItem(T objectToValidate, object currentTarget, string key, ValidationResults validationResults);

		protected override void DoValidate(IEnumerable<T> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			foreach (T item in objectToValidate)
			{
				DoValidateCollectionItem(item, currentTarget, key, validationResults);
			}
		}
	}
}
