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
using Microsoft.VisualStudio.Modeling;
using System.Diagnostics;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.Modeling.Validation;
using Microsoft.VisualStudio.Modeling.Integration;
using Microsoft.VisualStudio.Modeling.Integration.Picker;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that the model reference points to a MEL that exists on another model.
	/// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
    public class CircularRefereceModelValidator : Validator<ModelBusReference>
	{
        private HashSet<Guid> alreadyVisited;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularRefereceModelValidator"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public CircularRefereceModelValidator()
            : base(null, null)
        {
            alreadyVisited = new HashSet<Guid>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularRefereceModelValidator"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public CircularRefereceModelValidator(NameValueCollection attributes)
            : this()
        {
        }

		/// <summary>
		/// Does the validate.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The current target.</param>
		/// <param name="key">The key.</param>
		/// <param name="validationResults">The validation results.</param>
        protected override void DoValidate(ModelBusReference objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
			ModelElement currentElement = currentTarget as ModelElement;
			Debug.Assert(currentElement != null);

            if (!validationResults.IsValid)
            {
                return;
            }

			if (objectToValidate != null)
			{
                using (ModelBusReferenceResolver resolver = new ModelBusReferenceResolver())
                {
                    ModelElement referenced = resolver.Resolve(objectToValidate);
                    // Check if we are not in the same model and we had visited this model before 
                    if (!currentElement.Store.Id.Equals(referenced.Store.Id) &&
                        Visited(referenced.Store.Id))
                    {
                        this.LogValidationResult(validationResults,
                            String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate,
                            ValidatorUtility.GetTargetName(currentTarget), objectToValidate.ElementDisplayName, objectToValidate.ModelDisplayName), currentTarget, key);
                        return;
                    }
                }
                // store the current model to compare with references 
                // If referenced model == current model implies that we have a circular ref.
                alreadyVisited.Add(currentElement.Store.Id);
            }
		}

		/// <summary>
		/// Gets the default message template.
		/// </summary>
		/// <value>The default message template.</value>
		protected override string DefaultMessageTemplate
		{
			get { return Resources.CircularReferenceDetected; }
		}

        private bool Visited(Guid storeId)
        {
            return this.alreadyVisited.Contains(storeId);
        }
	}
}
