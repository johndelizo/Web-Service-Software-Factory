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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.CodeGeneration;
using System.Globalization;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using System.Diagnostics;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.VisualStudio.Modeling.Integration;
using Microsoft.VisualStudio.Modeling.Integration.Picker;

namespace Microsoft.Practices.ServiceFactory.Validation
{
    /// <summary>
    /// Validate unique FC are assigned to Faults elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class FaultElementUniqueContractCollectionValidator : Validator<IEnumerable<Fault>>
    {
        HashSet<FaultContract> faultContracts;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public FaultElementUniqueContractCollectionValidator(NameValueCollection attributes)
            : base(null, null)
        {
            faultContracts = new HashSet<FaultContract>();
        }

        protected override void DoValidate(IEnumerable<Fault> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            Operation operation = currentTarget as Operation;

            if (operation == null)
            {
                return;
            }

            //	Validation Application block doesn't recreate validators every time.
            //	Reset fc counter before validating the collection
            faultContracts.Clear();

            foreach (Fault item in objectToValidate)
            {
                DataContractFault fault = item as DataContractFault;

                if (fault == null || 
                    fault.Type == null)
                {
                    continue;
                }

                if (!fault.Type.IsValidReference())
                {
                    validationResults.AddResult(
                        new ValidationResult(
                            String.Format(CultureInfo.CurrentUICulture,
                            Resources.CannotResolveReference, currentTarget.GetType().Name, fault.Name, fault.Type.GetDisplayName()), fault, key, String.Empty, this));
                    return;
                }

                ModelElement mel = ModelBusReferenceResolver.ResolveAndDispose(fault.Type);
                if (mel == null)
                {
                    return;
                }

                FaultContract dcFault = mel as FaultContract;
                if (dcFault == null)
                {
                    return;
                }

                if (faultContracts.Contains(dcFault))
                {
                    validationResults.AddResult(
                        new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, fault.Name, operation.Name), objectToValidate, key, String.Empty, this)
                    );
                }
                else
                {
                    faultContracts.Add(dcFault);
                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.FaultCollectionUniqueContractValidatorMessage; }
        }
    }
}
