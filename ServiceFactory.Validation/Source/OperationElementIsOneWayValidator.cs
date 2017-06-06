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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that all elements in a collection of type Operation have unique values for a specified property.
	/// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class OperationElementIsOneWayValidator : Validator<bool>
	{
		// FxCop: Required by validation framework
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "attributes")]
		public OperationElementIsOneWayValidator(NameValueCollection attributes)
			: base(null, null)
		{
		}

		protected override void DoValidate(bool objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			Operation operation = currentTarget as Operation;
			if (operation == null)
			{
				return;
			}

			if (objectToValidate && 
				((operation.Response != null) || (operation.Faults.Count > 0) || HasReplyAction(operation)))
			{
				validationResults.AddResult(
					new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, operation.Name, key), currentTarget, key, String.Empty, this));
			}
		}

		protected override string DefaultMessageTemplate
		{
			get { return Resources.OperationElementValidatorMessage; }
		}

		private bool HasReplyAction(Operation operation)
		{
			if (operation.ObjectExtender != null)
			{
				PropertyInfo property = operation.ObjectExtender.GetType().GetProperty("ReplyAction");
				if (property != null)
				{
					return !string.IsNullOrEmpty(property.GetValue(operation.ObjectExtender, null) as string);
				}
			}
			return false;
		}
	}
}
