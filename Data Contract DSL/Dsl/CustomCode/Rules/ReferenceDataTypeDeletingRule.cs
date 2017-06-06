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
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.ServiceFactory.Common.Dsl;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
    [RuleOn(typeof(ReferenceDataType), FireTime = TimeToFire.Inline)]
    public partial class ReferenceDataTypeDeletingRule : DeletingRule
    {
        public override void ElementDeleting(ElementDeletingEventArgs e)
        {
            ReferenceDataType rdt = e.ModelElement as ReferenceDataType;
            if (rdt != null && rdt.DataContract != null)
            {
                // If this is the last element in DataMembers, restore the dash style to solid 
                if (rdt.DataContract.DataMembers.FindAll(m => { return (m is ReferenceDataType); }).Count == 1)
                {
                    DataContractCompartmentShape shape = DomainModelHelper.GetShapeFromElement<DataContractCompartmentShape>(rdt.DataContract);
                    if (shape != null)
                    {
                        shape.OutlineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        shape.OutlineThickness = 0.0125f;
                    }
                }
            }
        }
    }
}