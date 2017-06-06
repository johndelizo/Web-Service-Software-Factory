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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Modeling.Presentation.Models;
using Microsoft.Practices.ServiceFactory.Validation;
using System;
using System.Reflection;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Checks if a specified class has a default constructor. 
    /// </summary>
    public class ClassHasDefaultConstructorValidator : Validator<Type>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectItemIsUniqueValidator"/> class.
        /// </summary>
        /// <param name="currentProject">The current project.</param>
        /// <param name="languageExtension">The language extension.</param>
        /// 
        public ClassHasDefaultConstructorValidator()
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
        protected override void DoValidate(Type objectToValidate,
            object currentTarget, string key, ValidationResults validationResults)
        {
            if (!HasDefaultConstructor(objectToValidate))
            {
                this.LogValidationResult(validationResults, string.Format(CultureInfo.InvariantCulture,this.MessageTemplate, objectToValidate.Name), currentTarget, key);
            }
        }

        /// <summary>
        /// Gets the default message template.
        /// </summary>
        /// <value>The default message template.</value>
        protected override string DefaultMessageTemplate
        {
            get { return Resources.ClassHasDefaultConstructorValidatorMessage; }
        }

        private bool HasDefaultConstructor(Type type)
        {
            bool hasDefaultConstructor = false;

            foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
            {
                ParameterInfo[] parameters = constructor.GetParameters();

                if (parameters.Length == 0)
                {
                    hasDefaultConstructor = true;
                    break;
                }
            }

            return hasDefaultConstructor;
        }
    }
}
