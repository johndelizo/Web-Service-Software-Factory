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
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions;
using System.IO;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Artifacts
{
	public class DefaultResourceResolver : IResourceResolver
	{
		#region IResourceResolver Members

		public string GetResourcePath(string resourceItem)
		{
            return RuntimeHelper.GetExecutionPath(resourceItem);
		}

		public string GetResource(string resourceItem)
		{
			if (!File.Exists(resourceItem))
			{
				return string.Empty;
			}
			else
			{
				return File.ReadAllText(resourceItem);
			}
		}

		#endregion
	}
}
