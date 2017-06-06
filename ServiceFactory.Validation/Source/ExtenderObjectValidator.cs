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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections.Specialized;
using Microsoft.Practices.Modeling.Validation;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.CodeGeneration;
using System.Globalization;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Performs validation on an extender object by applying the validation rules specified for a supplied type.
	/// </summary>
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class ExtenderObjectValidator : ConfigurableObjectValidator<object>
    {
		public ExtenderObjectValidator(NameValueCollection configuration)
			: base(configuration)
        {
			base.TargetConfigurationFile = ValidationEngine.GetConfigurationRulePath();
        }

		protected override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			base.DoValidate(objectToValidate, currentTarget, key, validationResults);
            IArtifactLinkContainer container = objectToValidate as IArtifactLinkContainer;
            if (validationResults.IsValid &&
                ModelCollector.HasValidArtifacts(container) &&
                !ModelCollector.HasRoles(container))
            {
                this.LogValidationResult(validationResults,
                    ValidatorUtility.ShowFormattedMessage(Resources.ExtenderObjectValidatorMessage, currentTarget), 
                    currentTarget, 
                    key);
            }
		}
    }
}
