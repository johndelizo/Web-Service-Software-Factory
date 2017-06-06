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
using System.Collections.Specialized;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that the ServiceContract Model Implementation technology and Serializer are compliant
	/// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class ImplementationTechnologyAndSerializerValidator : Validator<SerializerType>
	{

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
		public ImplementationTechnologyAndSerializerValidator(NameValueCollection attributes)
			: base(null, null)
		{
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
		public ImplementationTechnologyAndSerializerValidator()
			: base(null, null)
		{
		}

		/// <summary>
		/// Does the validate.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The current target.</param>
		/// <param name="key">The key.</param>
		/// <param name="validationResults">The validation results.</param>
		protected override void DoValidate(SerializerType objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if(!validationResults.IsValid)
			{
				return;
			}

			ServiceContractModel serviceContractModel = currentTarget as ServiceContractModel;

			if(serviceContractModel != null)
			{
				if(IsASMX(serviceContractModel.ImplementationTechnology.Name))
				{
					if(objectToValidate.Equals(SerializerType.DataContractSerializer))
					{
						this.LogValidationResult(
							validationResults,
							string.Format(CultureInfo.CurrentCulture, this.MessageTemplate),
							currentTarget,
							key);
					}
				}
			}
		}

		protected override string DefaultMessageTemplate
		{
			get { return Resources.ImplementationTechnologyAndSerializerModelValidatorMessage; }
		}

		private bool IsASMX(string implementationTechnology)
		{
			return implementationTechnology.StartsWith("ASMX", StringComparison.OrdinalIgnoreCase);
		}
	}
}
