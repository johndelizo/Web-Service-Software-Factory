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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using Microsoft.CSharp;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualBasic;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	public static class Utility
	{
		private static CodeDomProvider csProvider = new CSharpCodeProvider();
		private static CodeDomProvider vbProvider = new VBCodeProvider();

		#region Casing

		/// <summary>
		/// Produces a camel-case string from an input string and add the Field postfix in case the current
		/// culture does not makes a difference in casing.
		/// </summary>
		/// <param name="identifier">The name of a code entity, such as a method parameter or property identifier.</param>
		/// <returns></returns>
		public static string ToCamelCase(string identifier)
		{
			Guard.ArgumentNotNullOrEmptyString(identifier, "identifier");

			string camelCase = CodeIdentifier.MakeCamel(identifier);
			// add Field postfix if the current culture does not dist. lower/upper case 
			return identifier == camelCase ||
				   !csProvider.IsValidIdentifier(camelCase) ? camelCase + "Field" : camelCase;
		}

		/// <summary>
		/// Produces a Pascal-case string from an input string.
		/// </summary>
		/// <param name="identifier">The name of a code entity, such as a method parameter.</param>
		/// <returns></returns>
		public static string ToPascalCase(string identifier)
		{
			Guard.ArgumentNotNullOrEmptyString(identifier, "identifier");

			return csProvider.CreateValidIdentifier(CodeIdentifier.MakePascal(identifier));
		}

        /// <summary>
        /// Produces a Pascal-case string from an input string.
        /// It compares the idenfier and compareIdentifier params, in case they're the same then the suffix Field is appended to the identifier.
        /// </summary>
        /// <param name="identifier">The name of a code entity, such as a method parameter.</param>
        /// <param name="compareIdentifier">The name of a code entity, such as a method parameter to compare.</param>
        /// <returns></returns>
        public static string ToPascalCase(string identifier, string compareIdentifier)
        {
            Guard.ArgumentNotNullOrEmptyString(identifier, "identifier");
            Guard.ArgumentNotNullOrEmptyString(compareIdentifier, "compareIdentifier");

            string pascalCase = CodeIdentifier.MakePascal(identifier);

            return compareIdentifier == pascalCase ||
                   !csProvider.IsValidIdentifier(pascalCase) ? pascalCase + "Field" : pascalCase;
        }

		#endregion

		#region CSharp type output

		/// <summary>
		/// Gets the CSharp type indicated by the specified value.
		/// </summary>
		/// <param name="type">A value that indicates the type to return.</param>
		/// <returns></returns>
		public static string GetCSharpTypeOutput(string typeName)
		{
			return GetCSharpTypeOutput(typeName, false);
		}

		/// <summary>
		/// Gets the CSharp type indicated by the specified value.
		/// </summary>
		/// <param name="typeName">Name of the type.</param>
		/// <param name="isNullableType">if set to <c>true</c> [is nullable type].</param>
		/// <returns></returns>
		public static string GetCSharpTypeOutput(string typeName, bool isNullableType)
		{
			Guard.ArgumentNotNullOrEmptyString(typeName, "typeName");

			if (isNullableType && 
				CanBeNullable(typeName))
			{
				typeName = string.Format(CultureInfo.InvariantCulture, "System.Nullable`1[{0}]", typeName);
			}

			string output = csProvider.GetTypeOutput(new CodeTypeReference(typeName));
			return FormatAlreadyConverted(typeName, output);
		}

		/// <summary>
		/// Gets the type declaration.
		/// </summary>
		/// <param name="collectionType">Type of the collection.</param>
		/// <param name="member">The member.</param>
		/// <returns></returns>
		public static string GetCSharpTypeDeclaration(Type collectionType)
		{
			return GetCSharpTypeDeclaration(collectionType, null);
		}

		/// <summary>
		/// Gets the type declaration.
		/// </summary>
		/// <param name="collectionType">Type of the collection.</param>
		/// <param name="member">The member.</param>
		/// <returns></returns>
		public static string GetCSharpTypeDeclaration(Type collectionType, string member)
		{
			if(collectionType == null)
			{
				return GetCSharpTypeOutput(member, false);
			}

			CodeTypeReference codeType = new CodeTypeReference(collectionType);
			
			if(!string.IsNullOrEmpty(member))
			{
				if ( collectionType.IsArray )
				{
					codeType.ArrayElementType = new CodeTypeReference(member);
				}
				else
				{
					if (collectionType == typeof(Dictionary<,>))
					{
						codeType.TypeArguments.Add(new CodeTypeReference("System.String"));
					}
					codeType.TypeArguments.Add(new CodeTypeReference(member));
				}
			}

			return csProvider.GetTypeOutput(codeType);
		}

		/// <summary>
		/// Returns if the type can be nullable.
		/// </summary>
		/// <param name="typeName">The Type name.</param>
		/// <returns>True if the type can be nullable</returns>
		public static bool CanBeNullable(string typeName)
		{
			Type type = Type.GetType(typeName, false);
			return (type != null && type.IsValueType);
		}

		/// <summary>
		/// Gets the data from the data dictionary in the specified <see cref="IArtifactLink"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public static T GetData<T>(IArtifactLink link)
		{
			return GetData<T>(link, typeof(T).FullName);
		}

		/// <summary>
		/// Gets the data from the data dictionary in the specified <see cref="IArtifactLink"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public static T GetData<T>(IArtifactLink link, string key)
		{
			Guard.ArgumentNotNull(link, "link");
			Guard.ArgumentNotNullOrEmptyString(key, "key");

			if(link.Data.ContainsKey(key))
			{
				return (T)link.Data[key];
			}
			return default(T);
		}

		/// <summary>
		/// Gets the data from the data dictionary in the specified <see cref="IArtifactLink"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public static void SetData<T>(IArtifactLink link, object value)
		{
			SetData(link, value, typeof(T).FullName);
		}

		/// <summary>
		/// Gets the data from the data dictionary in the specified <see cref="IArtifactLink"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public static void SetData(IArtifactLink link, object value, string key)
		{
			Guard.ArgumentNotNull(link, "link");
			Guard.ArgumentNotNull(value, "value");
			Guard.ArgumentNotNullOrEmptyString(key, "key");

			link.Data[key] = value;
		}

		#endregion

		// If the input is already formatted, then return the input string to avoid special chars like @, []
		private static string FormatAlreadyConverted(string inputType, string outputType)
		{
			return outputType.Contains(inputType) ? inputType : outputType;
		}
	}
}
