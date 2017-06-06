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
using System.IO;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
    public static class AdapterHelper
    {
        public static string GetFileLocation(string fileExtension, params object[] modelLocatorInfo)
        {
            if (modelLocatorInfo == null ||
                modelLocatorInfo.Length == 0)
            {
                return null;
            }

            // Only interested in project items
            foreach (object item in modelLocatorInfo)
            {
                string file = item as string;
                if (!string.IsNullOrWhiteSpace(file) &&
                    File.Exists(file) &&
                    Path.GetExtension(file).Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    return file;
                }
            }
            return null;
        }
    }
}
