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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that all elements in a collection of type T have unique values for a specified property. (Case insensitive)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class UniquePropertyCollectionValidator<T> : PropertyCollectionValidator<T>
	{
		private string collectionElementUniqueIdProperty;
		// By default, the comparison is case insensitive
		private HybridDictionary nameCounter = new HybridDictionary(true); 
		private const string defaultUniqueIdProperty = "Name";

		/// <summary>
		/// Initializes a new instance of the <see cref="UniquePropertyCollectionValidator&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collectionElementUniqueIdProperty">The collection element unique id property.</param>
		public UniquePropertyCollectionValidator(string collectionElementUniqueIdProperty)
		{
			this.collectionElementUniqueIdProperty = collectionElementUniqueIdProperty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UniquePropertyCollectionValidator&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		public UniquePropertyCollectionValidator(NameValueCollection attributes)
		{
			collectionElementUniqueIdProperty = defaultUniqueIdProperty;
			if (attributes != null)
			{
				collectionElementUniqueIdProperty = attributes.Get("collectionElementUniqueIdProperty") ?? defaultUniqueIdProperty;
				bool result;
				if(Boolean.TryParse(attributes.Get("caseSensitive") ?? Boolean.FalseString, out result) 
					&& result)
				{
					nameCounter = new HybridDictionary(false); 
				}
			}
		}

		// Added for unit testing purposes.
		/// <summary>
		/// Gets the name of the unique property.
		/// </summary>
		/// <value>The name of the unique property.</value>
		public string UniquePropertyName
		{
			get { return collectionElementUniqueIdProperty; }
		}

		/// <summary>
		/// Return the name of the object.
		/// </summary>
		/// <remarks>
		/// Override this to return user consumable name for an object under validation.
		/// </remarks>
		/// <param name="namedObject">The object.</param>
		/// <returns>The name of the object.</returns>
		public virtual string GetObjectName(object named)
		{
			return named.ToString();
		}

		/// <summary>
		/// Validate a single item in the collection.
		/// </summary>
		/// <param name="objectToValidate"></param>
		/// <param name="currentTarget"></param>
		/// <param name="key"></param>
		/// <param name="validationResults"></param>
		/// <remarks>
		/// Implement this in order to validate individual collection elements.
		/// </remarks>
		protected override void DoValidateCollectionItem(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			string currentTargetName = GetObjectName(currentTarget);

			string uniquePropertyValue = ValidationEngine.GetUniquePropertyValue(objectToValidate, this.UniquePropertyName);
			if (String.IsNullOrEmpty(uniquePropertyValue))
			{
				return;
			}

			if (nameCounter.Contains(uniquePropertyValue))
			{
				validationResults.AddResult(
					new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, currentTargetName, this.UniquePropertyName), objectToValidate, key, String.Empty, this)
					);
			}
			else
			{
				nameCounter.Add(uniquePropertyValue, true);
			}
		}

		/// <summary>
		/// Does the validate.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The current target.</param>
		/// <param name="key">The key.</param>
		/// <param name="validationResults">The validation results.</param>
		protected override void DoValidate(IEnumerable<T> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			//	Validation Application block doesn't recreate validators every time.
			//	Reset name counter before validating the collection
			nameCounter.Clear();
			base.DoValidate(objectToValidate, currentTarget, key, validationResults);
		}

		/// <summary>
		/// Gets the default message template.
		/// </summary>
		/// <value>The default message template.</value>
		protected override string DefaultMessageTemplate
		{
			get 
			{
				return Resources.UniquePropertyCollectionValidatorMessage; 
			}
		}
	}
}
