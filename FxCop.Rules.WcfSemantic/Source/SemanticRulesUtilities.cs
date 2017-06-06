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
using System.ServiceModel.Configuration;
using System.IO;
using System.ServiceModel.Description;
using System.Diagnostics.CodeAnalysis;
using Microsoft.FxCop.Sdk;
using System.Reflection;

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
{
    /// <summary>
    /// Class that reports if any problem was raised by all other rules in this library.
    /// </summary>
    static class SemanticRulesUtilities
    {
        public delegate void Contract(TypeNode contractType, ServiceEndpointElement endpoint);

        /// <summary>
        /// Evaluates the contracts.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="module">The module.</param>
        /// <param name="contract">The contract.</param>
        public static void EvaluateContracts(ServiceModelConfigurationManager configurationManager, ModuleNode module, Contract contract)
        {
            if (configurationManager != null)
            {
                foreach (ServiceElement service in configurationManager.GetServices())
                {
                    foreach (ServiceEndpointElement endpoint in service.Endpoints)
                    {
                        TypeNode typeNode = LoadTypeNode(endpoint.Contract, module);
                        if (typeNode != null)
                        {
                            contract(typeNode, endpoint);
                            if (typeNode.DeclaringModule != module)
                            {
								// unload external module, loaded in FileGetType()
                                typeNode.DeclaringModule.Dispose();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="AttributeNode"/>.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
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
			if(attribute == null || string.IsNullOrEmpty(argumentName))
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

		#region Private members

		private static TypeNode LoadTypeNode(string contract, ModuleNode module)
        {
            Identifier contractNamespace = Identifier.For(ParseNamespace(contract));
            Identifier contractName = Identifier.For(ParseName(contract));

            // filter IMetadataExchange
            if (contractName.Name.Equals(typeof(IMetadataExchange).Name, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

			TypeNode typeNode = module.GetType(contractNamespace, contractName, true);

            if (typeNode == null)
            {
                // try to get the type from the asms in the local directory of the current module
                typeNode = FileGetType(module, contractNamespace, contractName);
            }
			
            return typeNode;
        }

        private static string ParseNamespace(string fullName)
        {
            return fullName.IndexOf(Type.Delimiter) == -1 ?
                   string.Empty :
                   fullName.Substring(0, fullName.LastIndexOf(Type.Delimiter));
        }

        private static string ParseName(string fullName)
        {
            return fullName.IndexOf(Type.Delimiter) == -1 ?
                   fullName :
                   fullName.Substring(fullName.LastIndexOf(Type.Delimiter) + 1);
        }

		private static TypeNode FileGetType(ModuleNode module, Identifier nameSpace, Identifier name)
        {
            foreach (string file in Directory.GetFiles(
                module.Directory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                if (file != module.Location)
                {
					ModuleNode localModule = ModuleNode.GetModule(file, true, false, true);					
					if (localModule != null)
					{
						TypeNode typeNode = localModule.GetType(nameSpace, name);
						if (typeNode != null)
						{
							// resolve any assembly reference not loaded (GACed)
							localModule.AssemblyReferenceResolutionAfterProbingFailed += 
								new ModuleNode.AssemblyReferenceResolver(OnAssemblyReferenceResolutionAfterProbingFailed);
							return typeNode;
						}
						localModule.Dispose();
					}
                }
            }
            return null;
        }

		private static AssemblyNode OnAssemblyReferenceResolutionAfterProbingFailed(
			AssemblyReference assemblyReference, ModuleNode referencingModule)
		{
			return AssemblyNode.GetAssembly(Assembly.Load(assemblyReference.GetAssemblyName().FullName).Location, true, false, true);
		}

		#endregion
	}
}
