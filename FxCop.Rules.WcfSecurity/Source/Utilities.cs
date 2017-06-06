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
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSecurity
{
    /// <summary/>
    public static class Utilities
    {
        /// <summary>
        /// Returns the rules assembly file in FxCop rules folder or the assembly location.
        /// </summary>
        /// <returns></returns>
        public static string GetRulesAssemblyLocation()
        {
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            
            // check of we find this asm in the FxCop rules folder
            string rulesFolder = Path.GetFullPath(Path.Combine(
                Environment.GetEnvironmentVariable("VS80COMNTOOLS", EnvironmentVariableTarget.Process),
                "..\\..\\Team Tools\\Static Analysis Tools\\FxCop\\Rules"));
            if (Directory.Exists(rulesFolder))
            {
                string rulesAsm = Path.Combine(rulesFolder, Path.GetFileName(thisAssemblyPath));
                if (File.Exists(rulesAsm))
                {
                    return rulesAsm;
                }

            }
            return thisAssemblyPath;
        }

        public static AttributeNode GetAttribute(Member search, Type target)
        {
            if (search == null)
            {
                return null;
            }
            return GetAttribute(search.Attributes, target);
        }

        public static AttributeNode GetAttribute(AttributeNodeCollection attributes, Type target)
        {
            if (attributes == null || target == null)
            {
                return null;
            }

            foreach (AttributeNode attribute in attributes)
            {
                if (attribute.Type.FullName.Equals(target.FullName, StringComparison.Ordinal))
                {
                    return attribute;
                }
            }
            return null;
        }

        public static bool HasAttribute<T>(AttributeNode attribute) where T : Attribute
        {
            if (attribute == null)
            {
                return false;
            }
            return attribute.Type.FullName.Equals(typeof(T).FullName, StringComparison.Ordinal);
        }

        public static bool HasAttribute<T>(Member member) where T : Attribute
        {
            return GetAttribute(member, typeof(T)) != null;
        }

        public static bool HasAttribute<T>(AttributeNode attribute, string argumentName) where T : Attribute
        {
            return HasAttribute<T>(attribute) &&
                   attribute.GetNamedArgument(Identifier.For(argumentName)) != null;
        }

        public static T GetAttributeValue<T>(AttributeNode attribute, string argumentName)
        {
            if (attribute == null || string.IsNullOrEmpty(argumentName))
            {
                return default(T);
            }

            Literal argument = attribute.GetNamedArgument(Identifier.For(argumentName)) as Literal;
            if (argument != null)
            {
                return (T)argument.Value;
            }

            return default(T);
        }

        public static bool TryGetAttributeValue<T>(AttributeNode attribute, string argumentName, out T value)
        {
            value = default(T);

            if (attribute == null || 
                string.IsNullOrEmpty(argumentName))
            {
                return false;
            }

            Literal argument = attribute.GetNamedArgument(Identifier.For(argumentName)) as Literal;
            if (argument != null)
            {
                value = (T)argument.Value;
                return true;
            }

            return false;
        }
    }
}
