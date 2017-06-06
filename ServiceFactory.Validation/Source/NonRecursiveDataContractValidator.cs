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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Globalization;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.Modeling;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Validate that a DatContract does not have a recursive reference.
    /// </summary>
    [ConfigurationElementType(typeof(CustomValidatorData))]
	public class NonRecursiveDataContractValidator : Validator<DataContractBase>
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
		public NonRecursiveDataContractValidator(NameValueCollection attributes)
			: base(null, null)
        {
        }

		protected override void DoValidate(DataContractBase objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			if (objectToValidate != null)				
			{
				DataContractBase dc = currentTarget as DataContractBase;
				if (dc != null && HasRecursiveGraph(dc.Name, dc))
				{
					this.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, dc.Name), currentTarget, key);
				}
			}
		}

		protected override string DefaultMessageTemplate
		{
			get
			{
				return Resources.NonRecursiveDataContractValidator;
			}
		}

		private bool HasRecursiveGraph(string name, DataContractBase dataContract)
		{
			DataContractCollection collection = dataContract as DataContractCollection;
			if (collection != null && 
				collection.DataContract != null)
			{
				return name == collection.DataContract.Name || 
					   HasRecursiveGraph(name, collection.DataContract);
			}
			return false;
		}
    }
}
