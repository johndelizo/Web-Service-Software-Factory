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

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that the model reference points to a MEL that exists on another model.
	/// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class CrossModelReferenceValidator : Validator<ModelBusReference>
	{
		private string elementNameProperty;
		private bool validateReferencedElement;

		/// <summary>
		/// Initializes a new instance of the <see cref="CrossModelReferenceValidator"/> class.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		public CrossModelReferenceValidator(NameValueCollection attributes)
			: base(null, null)
        {
			if (attributes == null)
			{
				return;
			}

			elementNameProperty = attributes.Get("elementNameProperty") ?? "Name";
			Boolean.TryParse(attributes.Get("validateReferencedElement") ?? "false", out validateReferencedElement);
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
			string elementNamePropertyValue = ValidationEngine.GetUniquePropertyValue(currentTarget, elementNameProperty);

            if (!validationResults.IsValid)
            {
                return;
            }

			if (objectToValidate == null)
			{
				validationResults.AddResult(
					new ValidationResult(String.Format(CultureInfo.CurrentUICulture, Resources.ModelReferenceValidatorMessage, key, currentTarget.GetType().Name, elementNamePropertyValue), currentTarget, key, String.Empty, this));
				return;
			}

            //Check if we need to refresh an updated DC reference.
            ModelElement element = ModelBusReferenceResolver.ResolveAndDispose(objectToValidate);
            if (element == null)
            {
                validationResults.AddResult(
                    new ValidationResult(
                        String.Format(CultureInfo.CurrentUICulture,
                        Resources.CannotResolveReference, currentTarget.GetType().Name, elementNamePropertyValue, objectToValidate.GetDisplayName()), currentTarget, key, String.Empty, this));
                return;
            }

            string referenceElementDisplayName = ValidationEngine.GetUniquePropertyValue(element, "Name");

            if (!objectToValidate.ElementDisplayName.Equals(referenceElementDisplayName, StringComparison.CurrentCulture))
            {
                using (Transaction tx = ((ModelElement)currentTarget).Store.TransactionManager.BeginTransaction("Update reference value"))
                {
                    ModelBusReference refresh = new ModelBusReference(
                        objectToValidate.ModelBus, 
                        objectToValidate.LogicalAdapterId,
                        objectToValidate.ModelDisplayName,
                        referenceElementDisplayName, 
                        objectToValidate.SerializedAdapterReference, 
                        objectToValidate.ReferenceContext);
                    // refresh value
                    PropertyInfo typeProp = currentTarget.GetType().GetProperty("Type");
                    typeProp.SetValue(currentTarget, null, null); //Set it null to force updating
                    typeProp.SetValue(currentTarget, refresh, null); //Now set the final value 
                    tx.Commit();
                }
                validationResults.AddResult(new ValidationResult(
                        string.Format(CultureInfo.CurrentCulture, Resources.ModelBusRefrenceUpdate, elementNamePropertyValue, referenceElementDisplayName), 
                        currentTarget, key, Constants.LogWarningTag, this));
            }
		}

		/// <summary>
		/// Gets the default message template.
		/// </summary>
		/// <value>The default message template.</value>
		protected override string DefaultMessageTemplate
		{
			get { return Resources.ModelReferenceValidatorMessage; }
		}
	}
}
