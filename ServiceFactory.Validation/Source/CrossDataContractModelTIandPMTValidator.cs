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
using System.Globalization;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.VisualStudio.Modeling;
using System.Diagnostics;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling.Integration;
using System.Collections.ObjectModel;
using Microsoft.Practices.Modeling.Validation;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that the data contract model reference has associated a Technology Information and PMT.
	/// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
	public class CrossDataContractModelTIandPMTValidator : CrossModelReferenceValidator
	{
		private const string crossModelReferenceValidatorMessageKeyName = "crossModelReferenceValidatorMessage";
        private string dcModelMessageTemplate;
		private string currentMessageTemplate;

		/// <summary>
		/// Initializes a new instance of the <see cref="CrossDataContractModelTIandPMTValidator"/> class.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
        public CrossDataContractModelTIandPMTValidator(NameValueCollection attributes)
			: base(attributes)
        {
			if (attributes == null)
			{
				return;
			}

			currentMessageTemplate = String.IsNullOrEmpty(attributes.Get(crossModelReferenceValidatorMessageKeyName)) ?
				Resources.CrossDataContractModelTIandPMTValidator : 
				attributes.Get(crossModelReferenceValidatorMessageKeyName);
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
			// store default message (in case the error comes from a DC element) and set our new message
			dcModelMessageTemplate = this.MessageTemplate;
			this.MessageTemplate = currentMessageTemplate;			
			
			// Validate cross model references
            base.DoValidate(objectToValidate, currentTarget, key, validationResults);

			if (!validationResults.IsValid)
			{
				return;
			}

            using (ModelBusReferenceResolver resolver = new ModelBusReferenceResolver())
            {
                ModelElement referencedElement = resolver.Resolve(objectToValidate);

                if (referencedElement != null)
                {
                    DataContractModel dcm = referencedElement.Store.ElementDirectory.FindElements<DataContractModel>()[0];
                    if (dcm.ImplementationTechnology == null ||
                        String.IsNullOrWhiteSpace(dcm.ProjectMappingTable) ||
                        !dcm.ImplementationTechnology.Name.Equals(GetItName(currentTarget),StringComparison.OrdinalIgnoreCase))
                    {
                        validationResults.AddResult(
                            new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.dcModelMessageTemplate, ValidatorUtility.GetTargetName(currentTarget)), currentTarget, key, String.Empty, this));
                    }
                }
            }
		}

		/// <summary>
		/// Gets the default message template.
		/// </summary>
		/// <value>The default message template.</value>
    	protected override string DefaultMessageTemplate
		{
            get { return Resources.CrossDataContractModelTIandPMTValidator; }
		}

        private string GetItName(object element)
        {
            if (element == null)
                return string.Empty;

            ModelElement mel = element as ModelElement;
            if (mel != null)
            {
                // Try with DC model first
                if (!string.IsNullOrWhiteSpace(dcModelMessageTemplate))
                {
                    DataContractModel dcModel = DomainModelHelper.GetElement<DataContractModel>(mel.Store);
                    if (dcModel != null && dcModel.ImplementationTechnology != null) return dcModel.ImplementationTechnology.Name;
                }
                if (!string.IsNullOrWhiteSpace(dcModelMessageTemplate))
                {
                    ServiceContractModel scModel = DomainModelHelper.GetElement<ServiceContractModel>(mel.Store);
                    if(scModel != null && scModel.ImplementationTechnology != null) return scModel.ImplementationTechnology.Name;
                }                
            }
            return string.Empty;
        }
	}
}
