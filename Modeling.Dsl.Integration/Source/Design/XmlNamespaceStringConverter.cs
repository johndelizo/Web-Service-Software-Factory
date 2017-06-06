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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Practices.Modeling.Dsl.Integration.Design
{
    public class XmlNamespaceStringConverter : StringConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string val = value as string;
            if (!string.IsNullOrWhiteSpace(val))
            {
                return val.ToLower(culture);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            string val = value as string;
            if (!string.IsNullOrWhiteSpace(val))
            {
                return val.ToLower(culture);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        // Methods
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            bool flag = false;
            string uriString = value as string;
            if (uriString != null)
            {
                flag = Uri.IsWellFormedUriString(uriString, UriKind.Absolute);
            }
            return flag;
        }
    }
}
