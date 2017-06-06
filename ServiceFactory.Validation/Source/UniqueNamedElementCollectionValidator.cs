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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.CodeGeneration;
using System.Globalization;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that all ModelElements in a collection of type T (of ModelElements) have unique values for a specified property.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class UniqueNamedElementCollectionValidator<T> : UniquePropertyCollectionValidator<T> where T : ModelElement
	{
		public UniqueNamedElementCollectionValidator(NameValueCollection attributes)
			: base(attributes)
		{
		}

		/// <summary>
		/// Return the name of the object.
		/// </summary>
		/// <remarks>
		/// This returns the name of the model element.
		/// </remarks>
		/// <param name="namedObject">The object.</param>
		/// <returns>The name of the object.</returns>
		public override string GetObjectName(object named)
		{
			if (named == null) return string.Empty;
			
			ModelElement modelElement = named as ModelElement;

			if (modelElement == null)
				return named.ToString();
				
			string modelElementName = string.Empty;
			if (!DomainClassInfo.TryGetName(modelElement, out modelElementName))
			{
				//if model element doesnt have a name, we return the class' displayname
				DomainClassInfo classInfo = modelElement.GetDomainClass();
				modelElementName = classInfo.DisplayName;
			}
			
			return modelElementName;
		}
	}
}
