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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.DataContracts;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that a collection of EnumNamedValues is not empty
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class NotEmptyEnumDataElementCollectionValidator : NotEmptyCollectionValidator<EnumNamedValue>
	{
		public NotEmptyEnumDataElementCollectionValidator()
			: base()
		{
		}

		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
		public NotEmptyEnumDataElementCollectionValidator(NameValueCollection attributes)
			: base()
		{
		}
	}
}