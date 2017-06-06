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
using System.ComponentModel.Design;
using EnvDTE;

namespace Microsoft.Practices.Modeling.Presentation.Models
{
    /// <summary>
    /// Interface providing various services for querying or updating
    /// an open Visual Studio project.
    /// </summary>
    public interface IProjectModel
    {
        /// <summary>
        /// Projects the contains file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        bool ProjectContainsFile(string filename);

		/// <summary>
		/// Returns an instance of <see cref="ITypeResolutionService"/> for the project scope
		/// </summary>
		ITypeResolutionService TypeResolutionService { get; }

		/// <summary>
		/// Returns a list with the types defined in the project
		/// </summary>
		IList<ITypeModel> Types { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is web project.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is web project; otherwise, <c>false</c>.
        /// </value>
        bool IsWebProject { get; }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <value>The project.</value>
        object Project { get; }

        /// <summary>
        /// Gets the project items.
        /// </summary>
        /// <value>The project items.</value>
        IList<IProjectItemModel> ProjectItems { get;}

        /// <summary>
        /// Gets the full path of the project.
        /// </summary>
        /// <value>The full path.</value>
        string FullPath { get; }
    }
}
