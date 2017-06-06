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
using System.Runtime.Serialization;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping
{
	[Serializable]
	public class ProjectMappingTableAlreadyExistsException : Exception
	{
		public ProjectMappingTableAlreadyExistsException()
		{
		}

		public ProjectMappingTableAlreadyExistsException(string message)
			: base(message)
		{
		}

		protected ProjectMappingTableAlreadyExistsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public ProjectMappingTableAlreadyExistsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}