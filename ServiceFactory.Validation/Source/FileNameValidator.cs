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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.ServiceFactory.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	public class FileNameValidator : AndCompositeValidator
	{
		private const int MaxFileNameLength = 110;

		/// <summary>
		/// Initializes a new instance of the <see cref="FileNameValidator"/> class.
		/// </summary>
		public FileNameValidator() 
			: base
			(
				new StringLengthValidator(1, RangeBoundaryType.Inclusive, MaxFileNameLength, RangeBoundaryType.Inclusive,
					Resources.FileNameLengthValidatorMessage),
				new RegexValidator(@"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:""><|/]+$",
					Resources.ReservedSystemWordsFileNameValidatorMessage)
			)
		{
		}
	}
}
