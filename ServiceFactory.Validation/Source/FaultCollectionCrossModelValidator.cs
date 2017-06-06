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
    /// Validate that only XsdElementFaults are added when using XmlSerializer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    [ConfigurationElementType(typeof(CustomValidatorData))]
    public class FaultCollectionCrossModelValidator : Validator<IEnumerable<Fault>>
    {
        private const string WCFExtension = "WCF";
        private const string ASMXExtension = "ASMX";
        private string asmxExtensionInvalidSerializerMessage;
        private string faultInvalidSerializerMessage;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public FaultCollectionCrossModelValidator(NameValueCollection attributes)
            : base(null, null)
        {
            asmxExtensionInvalidSerializerMessage = Resources.AsmxExtensionInvalidSerializerMessage;
            faultInvalidSerializerMessage = Resources.FaultInvalidSerializerMessage;

            if (attributes == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(attributes.Get("asmxExtensionInvalidSerializerMessage")))
            {
                asmxExtensionInvalidSerializerMessage = attributes.Get("asmxExtensionInvalidSerializerMessage");
            }

            if (!string.IsNullOrEmpty(attributes.Get("faultInvalidSerializerMessage")))
            {
                faultInvalidSerializerMessage = attributes.Get("faultInvalidSerializerMessage");
            }
        }

        protected override void DoValidate(IEnumerable<Fault> objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            Operation operation = currentTarget as Operation;

            if (operation == null)
            {
                return;
            }

            string serviceContractImplementationTech = String.Empty;

            if (operation.ServiceContractModel.ImplementationTechnology == null)
            {
                return;
            }
            else
            {
                serviceContractImplementationTech = operation.ServiceContractModel.ImplementationTechnology.Name;
            }

            SerializerType serviceContractSerializer = operation.ServiceContractModel.SerializerType;

            foreach (Fault item in objectToValidate)
            {
                bool isValid = true;
                DataContractFault fault = item as DataContractFault;

                if (fault == null || fault.Type == null)
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
                if (dcFault == null ||
                    dcFault.DataContractModel == null ||
                    dcFault.DataContractModel.ImplementationTechnology == null)
                {
                    return;
                }

                if (serviceContractImplementationTech.Equals(ASMXExtension, StringComparison.OrdinalIgnoreCase))
                {
                    if (serviceContractSerializer.Equals(SerializerType.XmlSerializer))
                    {
                        isValid = !(dcFault.DataContractModel.ImplementationTechnology.Name.Equals(WCFExtension, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        // Asmx Extension only supports XmlSerializer
                        validationResults.AddResult(
                            new ValidationResult(String.Format(CultureInfo.CurrentUICulture, asmxExtensionInvalidSerializerMessage, fault.Name, operation.Name), objectToValidate, key, String.Empty, this)
                        );
                        return;
                    }
                }
                else if (serviceContractImplementationTech.Equals(WCFExtension, StringComparison.OrdinalIgnoreCase))
                {
                    if (serviceContractSerializer.Equals(SerializerType.DataContractSerializer))
                    {
                        isValid = !(dcFault.DataContractModel.ImplementationTechnology.Name.Equals(ASMXExtension, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        if (dcFault.DataContractModel.ImplementationTechnology.Name.Equals(ASMXExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            // Faults cannot be XMLSerializable
                            validationResults.AddResult(
                                new ValidationResult(String.Format(CultureInfo.CurrentUICulture, faultInvalidSerializerMessage, operation.Name, fault.Name), objectToValidate, key, String.Empty, this)
                            );
                            return;
                        }
                    }
                }

                if (!isValid)
                {
                    validationResults.AddResult(
                        new ValidationResult(String.Format(CultureInfo.CurrentUICulture, this.MessageTemplate, fault.Name, operation.Name), objectToValidate, key, String.Empty, this)
                    );
                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.FaultCollectionCrossModelValidatorMessage; }
        }
    }
}
