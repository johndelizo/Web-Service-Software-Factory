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
using System.Diagnostics;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections.Specialized;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Performs validation on an object by applying the validation rules specified for a supplied type.
	/// </summary>
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class ConfigurableObjectValidator<T> : Validator<T>
    {
        private string targetRuleset;
        private string targetConfigurationFile;

		protected string TargetConfigurationFile
		{
			get { return targetConfigurationFile; }
			set { targetConfigurationFile = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigurableObjectValidator(NameValueCollection configuration)
            : base(null, null)
        {
			this.targetRuleset = configuration["targetRuleset"];
			this.TargetConfigurationFile = configuration["fileConfigurationSource"];
        }

        /// <summary>
        /// Gets the message template to use when logging results no message is supplied.
        /// </summary>
        protected override string DefaultMessageTemplate
        {
            get { return String.Empty; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objectToValidate">The object that will be validated.</param>
		/// <returns></returns>
		protected virtual bool IsValidated(T objectToValidate)
		{
			return false;
		}

		protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate != null)
            {
				//	Ignore objects than have already been validated.
				//	
				if (IsValidated(objectToValidate))
					return;

                Type targetType = objectToValidate.GetType();

                using (FileConfigurationSource configurationSource = new FileConfigurationSource(TargetConfigurationFile))
                {
                    Validator v = ValidationFactory.CreateValidator(targetType, "Common", configurationSource);
                    v.Validate(objectToValidate, validationResults);

                    v = ValidationFactory.CreateValidator(targetType, targetRuleset, configurationSource);
                    v.Validate(objectToValidate, validationResults);
                }
				Debug.WriteLine(String.Format(CultureInfo.CurrentUICulture, "{0} {1}", objectToValidate.ToString(), validationResults.IsValid ? "Succeeded" : "Failed"));
            }
        }
    }
}