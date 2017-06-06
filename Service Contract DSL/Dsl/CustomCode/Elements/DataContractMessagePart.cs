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
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using Microsoft.VisualStudio.Modeling.Validation;
//using Microsoft.VisualStudio.Modeling.Integration.Picker;
//using Microsoft.VisualStudio.Modeling.Integration;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
    [ValidationState(ValidationState.Enabled)]
    public partial class DataContractMessagePart : ICrossModelingPropertyHolder
	{
        // Validation is being resolved by Validator library (CrossModelReferenceValidator)

        ///// <summary>
        ///// Validates the Domain property modelbus reference of the DataContractMessagePart DomainClass class
        ///// </summary>
        ///// <param name="context"></param>
        //[ValidationMethod(ValidationCategories.Menu)]
        //public void ValidateModelBusReferences(ValidationContext context)
        //{            
        //    BrokenReferenceDetector.DetectBrokenReferences(context.ValidationSubjects, (IServiceProvider)this.Store,
        //        new Action<ModelElement, DomainPropertyInfo, ModelBusReference>(
        //            delegate(ModelElement element, DomainPropertyInfo property, ModelBusReference reference)
        //            {
        //                DataContractMessagePart dataContract = element as DataContractMessagePart;
        //                if (dataContract != null)
        //                    context.LogError(
        //                        string.Format(CultureInfo.CurrentCulture,
        //                        Properties.Resources.CannotResolveReference, property.Name, dataContract.Name, reference.GetDisplayName()),
        //                        "MBRef", element);
        //            }));
        //}
    }
}