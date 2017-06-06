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
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using System.Xml.Schema;
using System.Diagnostics;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
    public class CustomModelingSchemaResolver : ISchemaResolver
    {
        ISchemaResolver defaultResolver;
        string schemaFileName;
        IList<string> schemas = new List<string>();

        public CustomModelingSchemaResolver(ISchemaResolver defaultResolver, string schemaFileName)
        {
            this.defaultResolver = defaultResolver;            
            this.schemaFileName = schemaFileName;
        }

        public IList<string> ResolveSchema(string targetNamespace)
        {
            if(defaultResolver != null)
            {
                IList<string> resolved = this.defaultResolver.ResolveSchema(targetNamespace);
                if (resolved != null)
                {
                    return resolved;
                }
            }

            // probing in current extension folder
            // the xsd file should be placed in the extender root folder.
            string schemaPath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), schemaFileName);
            if (File.Exists(schemaPath))
            {
                return ResolveSchema(targetNamespace, schemaPath);
            }

            return null;
        }

        private IList<string> ResolveSchema(string targetNamespace, string schemaUri)
        {
            IList<string> result = new List<string>() { schemaUri };
            
            if (schemas.Contains(targetNamespace))
            {
                return result;
            }

            try
            {
                // Check that the schema in path has targetNamespace
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(targetNamespace, schemaUri);
                // Validation in Add(..) was fine so return schemaUri
                schemas.Add(targetNamespace);
                return result;
            }
            catch (XmlSchemaException e)
            {
                Trace.TraceWarning(e.ToString());
            }

            return null;
        }
    }
}
