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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class ElementNameValidator : AndCompositeValidator
    {
        private object currentTarget;
        private int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ElementNameValidator"/> class.
        /// </summary>
        public ElementNameValidator()
            : this(512)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNameValidator"/> class.
        /// </summary>
        public ElementNameValidator(int maxLength)
            : base
            (
                new StringLengthValidator(1, RangeBoundaryType.Inclusive, maxLength, RangeBoundaryType.Inclusive,
                    Resources.ElementNameValidatorLengthMessage),
                new RegexValidator(@"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:""><|/]+$",
                    Resources.ElementNameValidatorInvalidNameMessage),
                new IdentifierValidator(Resources.ElementNameValidatorInvalidNameMessage)
            )
        {
            this.maxLength = maxLength;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNameValidator"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "attributes")]
        public ElementNameValidator(NameValueCollection attributes)
            : this()
        {
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "currentTarget")]
        public override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            this.currentTarget = currentTarget;
            base.DoValidate(objectToValidate, currentTarget, key, validationResults);
        }

        protected override string GetMessage(object objectToValidate, string key)
        {
            return ValidatorUtility.ShowFormattedMessage(
                this.MessageTemplate,
                ValidatorUtility.GetTargetName(currentTarget),
                this.maxLength,
                base.GetMessage(objectToValidate, key));
        }
    }
}
