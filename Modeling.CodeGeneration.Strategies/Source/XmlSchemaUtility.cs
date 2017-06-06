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
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.ServiceFactory.Description;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies
{
	/// <summary>
	/// XmlSchema utility class.
	/// </summary>
	public static class XmlSchemaUtility
	{
		/// <summary>
		/// Gets the XML schema source.
		/// </summary>
		/// <param name="xsdMoniker">The XSD moniker.</param>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public static string GetXmlSchemaSource(string xsdMoniker, IArtifactLink link)
		{
			Guard.ArgumentNotNullOrEmptyString(xsdMoniker, "xsdMoniker");
			Guard.ArgumentNotNull(link, "link");

			IResourceResolver resolver = (link as IResourceResolver) ?? new XmlSchemaResourceResolver(link);
			XmlSchemaElementMoniker uri = new XmlSchemaElementMoniker(xsdMoniker);
			return (uri.ElementName != null ? resolver.GetResourcePath(uri.XmlSchemaPath) : null);
		}

		/// <summary>
		/// Gets the type of the base types from referenced.
		/// </summary>
		/// <param name="xsdMoniker">The XSD moniker.</param>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public static IList<string> GetBaseTypesFromReferencedType(string xsdMoniker, IArtifactLink link)
		{
			Guard.ArgumentNotNullOrEmptyString(xsdMoniker, "xsdMoniker");
			Guard.ArgumentNotNull(link, "link");
	
			IList<string> types = new List<string>();
			string xmlSchemaSource = GetXmlSchemaSource(xsdMoniker, link);
			string element = new XmlSchemaElementMoniker(xsdMoniker).ElementName;
			// try first with DC serializer
			XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(false);
			CodeCompileUnit unit;
			try
			{
				unit = generator.GenerateCodeCompileUnit(xmlSchemaSource);
			}
			catch (InvalidSerializerException)
			{
				// now try with Xml serializer
				generator = new XmlSchemaTypeGenerator(true);
				unit = generator.GenerateCodeCompileUnit(xmlSchemaSource);
			}

			foreach (CodeNamespace ns in unit.Namespaces)
			{
				foreach (CodeTypeDeclaration codeType in ns.Types)
				{
					if (codeType.Name.Equals(element, StringComparison.OrdinalIgnoreCase))
					{
						CollectNestedTypes(codeType, types, unit, ns.Types, link);
						return types;
					}
				}
			}

			return types;
		}

		/// <summary>
		/// Gets the type of the code type from reference.
		/// </summary>
		/// <param name="reference">The reference.</param>
		/// <param name="types">The types.</param>
		/// <returns></returns>
		public static CodeTypeDeclaration GetCodeTypeFromReferenceType(CodeTypeReference reference, 
			CodeTypeDeclarationCollection types)
		{
			Guard.ArgumentNotNull(reference, "reference");
			Guard.ArgumentNotNull(types, "types");
			
			string searchType = StripNamespace(reference.BaseType);
			foreach (CodeTypeDeclaration codeType in types)
			{
				if (searchType.Equals(codeType.Name, StringComparison.OrdinalIgnoreCase))
				{
					return codeType;
				}
			}
			return null;
		}

		private static void CollectNestedTypes(CodeTypeDeclaration codeType, IList<string> addedTypes,
			CodeCompileUnit unit, CodeTypeDeclarationCollection types, IArtifactLink link)
		{
			foreach (CodeTypeReference baseType in codeType.BaseTypes)
			{
				if (!IsPrimitiveMember(baseType.BaseType) &&
					!IsExternalType(unit, baseType.BaseType))
				{
					addedTypes.Add(ResolveTypeReference(link, baseType));
					// walk down the class hierarchy
					CollectNestedTypes(GetCodeTypeFromReferenceType(baseType, types), addedTypes, unit, types, link);
				}
			}
		}

		private static string ResolveTypeReference(IArtifactLink link, CodeTypeReference type)
		{
			return ((ArtifactLink)link).Namespace + "." + type.BaseType.Substring(type.BaseType.LastIndexOf(".",StringComparison.OrdinalIgnoreCase) + 1);
		}

		private static bool IsExternalType(CodeCompileUnit unit, string typeName)
		{
			foreach (string asm in unit.ReferencedAssemblies)
			{
				if (typeName.StartsWith(Path.GetFileNameWithoutExtension(asm), StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsPrimitiveMember(string baseType)
		{
			return (Type.GetType(baseType, false, true) != null);
		}

		private static string StripNamespace(string type)
		{
			int suffixPos = type.LastIndexOf(".", StringComparison.Ordinal);
			if (suffixPos > 0)
			{
				type = type.Substring(suffixPos + 1);
			}
			return type;
		}
	}
}
