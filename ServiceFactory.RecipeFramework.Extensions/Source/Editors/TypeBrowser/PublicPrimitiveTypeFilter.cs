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
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors.TypeBrowser
{
    /// <summary>
	/// Filter for public types that are compatible with XML Schema primitive types. 
    /// </summary>
    public class PublicPrimitiveTypeFilter : PublicTypeFilter
    {
        // This filter is provided in case the end user wants to add some custom type to the built-in provided primitive types.
        // This can bo done by referencing the custom type project from the project containing the model files.
        // Note that the custom types should comply with the DC serialization rules described here:
        // Serializable Types: http://msdn.microsoft.com/en-us/library/cc656732(v=VS.100).aspx
        // Types Supported by the Data Contract Serializer: http://msdn.microsoft.com/en-us/library/ms731923(v=VS.100).aspx
        private PublicNonSystemTypeFilter customTypeFilter;

		/// <summary>
        /// Initializes a new instance of the <see cref="T:PublicPrimitiveTypeFilter"/> class.
        /// </summary>
        //We need a default constructor for the FilteredTypeBrowser to work properly
        public PublicPrimitiveTypeFilter() : base()
        {
            customTypeFilter = new PublicNonSystemTypeFilter();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PublicPrimitiveTypeFilter"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        //We need a default constructor for the FilteredTypeBrowser to work properly
		public PublicPrimitiveTypeFilter(IServiceProvider provider) : base(provider)
        {
            customTypeFilter = new PublicNonSystemTypeFilter(provider);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified type can be filtered.
        /// </summary>
        /// <param name="type">The <see cref="T:System.Type"></see> to check for filtering.</param>
        /// <param name="throwOnError">true to throw an exception when the <see cref="M:System.Workflow.ComponentModel.Design.ITypeFilterProvider.CanFilterType(System.Type,System.Boolean)"></see> is processed; otherwise, false.</param>
        /// <returns>
        /// true if the specified type can be filtered; otherwise, false.
        /// </returns>
		public override bool CanFilterType(Type type, bool throwOnError)
        {
			if ((base.CanFilterType(type, throwOnError) && IsKnownType(type)) || 
                customTypeFilter.CanFilterType(type, throwOnError))
			{
				return true;
			}

			ThrowIfOnError(throwOnError, true, Properties.Resources.InvalidTypeError);

			return false;
		}

        /// <summary>
        /// Gets the description for the filter to be displayed on the class browser dialog box.
        /// </summary>
        /// <value></value>
        /// <returns>A string value that contains the description of the filter.</returns>
        public override string FilterDescription
        {
            get { return Properties.Resources.PublicPrivateTypesFilter_Description; }
        }

        private static bool IsKnownType(Type type)
        {
            if (type == typeof(object))
            {
                return true;
            }

            if (type.IsEnum)
            {
                return false;
            }
            
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return true;

                case TypeCode.Char:
                    return true;

                case TypeCode.SByte:
                    return true;

                case TypeCode.Byte:
                    return true;

                case TypeCode.Int16:
                    return true;

                case TypeCode.UInt16:
                    return true;

                case TypeCode.Int32:
                    return true;

                case TypeCode.UInt32:
                    return true;

                case TypeCode.Int64:
                    return true;

                case TypeCode.UInt64:
                    return true;

                case TypeCode.Single:
                    return true;

                case TypeCode.Double:
                    return true;

                case TypeCode.Decimal:
                    return true;

                case TypeCode.DateTime:
                    return true;

                case TypeCode.String:
                    return true;
            }

            return type == typeof(XmlQualifiedName) || 
                   type == typeof(byte[]) || 
                   type == typeof(Guid) || 
                   type == typeof(XmlNode[]) ||
                   type == typeof(Uri) ||
                   type == typeof(TimeSpan);
        }
    }
}
