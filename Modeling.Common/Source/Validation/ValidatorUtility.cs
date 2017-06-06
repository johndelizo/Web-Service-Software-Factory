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
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using System.Reflection;

namespace Microsoft.Practices.Modeling.Validation
{
    public static class ValidatorUtility
    {
        public static string GetTargetName(object named)
        {
            if (named == null)
            {
                return string.Empty;
            }

            ModelElement modelElement = named as ModelElement;

            if (modelElement == null)
            {
                PropertyInfo property = named.GetType().GetProperty("ModelElement");
                if (property == null)
                {
                    return named.ToString();
                }
                modelElement = property.GetValue(named, null) as ModelElement;
            }

            string modelElementName = string.Empty;
            if (!DomainClassInfo.TryGetName(modelElement, out modelElementName))
            {
                //if model element doesnt have a name, we return the class' displayname
                DomainClassInfo classInfo = modelElement.GetDomainClass();
                modelElementName = classInfo.DisplayName;
            }

            return modelElementName;
        }

        public static string ShowFormattedMessage(string format, object target)
        {
            return ShowFormattedMessage(format, new object[]{ GetTargetName(target) }); 
        }

        public static string ShowFormattedMessage(string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentUICulture, format, args);
        }
    }
}
