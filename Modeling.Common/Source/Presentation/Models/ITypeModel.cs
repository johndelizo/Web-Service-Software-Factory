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

namespace Microsoft.Practices.Modeling.Presentation.Models
{
    /// <summary>
    /// Interface providing type properties.
    /// </summary>
	public interface ITypeModel
	{
		/// <summary>
		/// Gets the fully qualified type name (namespace + type name)
		/// </summary>
		string FullName { get; }

		/// <summary>
		/// Gets the name of the type (without namespace)
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Verifies if the type is decorated with the specified custom attribute
		/// </summary>
		/// <param name="attributeFullName">The fully qualified name of the attribute</param>
		/// <returns>True if the attribute is found, false otherwise</returns>
		bool HasAttribute(string attributeFullName);

		/// <summary>
		/// Verifies if the type is decorated with the specified custom attribute
		/// </summary>
		/// <param name="attributeFullName">The fully qualified name of the attribute</param>
		/// <param name="inherited">Allows to search the attribute in inherited types</param>
		/// <returns>True if the attribute is found, false otherwise</returns>
		bool HasAttribute(string attributeFullName, bool inherited);

        /// <summary>
        /// Determines whether the specified attribute has a property with the specified value.
        /// </summary>
        /// <param name="attributeFullName">Full name of the attribute.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <returns>
        /// 	<c>true</c> if the attribute property has the specified value; otherwise, <c>false</c>.
        /// </returns>
        bool HasAttributePropertyValue(string attributeFullName, string propertyName, object propertyValue);

        /// <summary>
        /// Verifies if the type contains any member with the specified name
        /// </summary>
        /// <param name="memberName">The name of the member</param>
        /// <returns>True if contains at least one member with the specified name, false otherwise</returns>
        bool HasMember(string memberName);

		/// <summary>
		/// Returns true if the type is a class
		/// </summary>
		bool IsClass { get; }

		/// <summary>
		/// Returns true if the type is an interface
		/// </summary>
		bool IsInterface { get; }

		/// <summary>
		/// Returns true if the type is public
		/// </summary>
		bool IsPublic { get; }

        object TypeModel { get; }
	}
}
