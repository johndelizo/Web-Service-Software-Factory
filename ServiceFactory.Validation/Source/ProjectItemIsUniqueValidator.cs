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
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Modeling.Presentation.Models;
using Microsoft.Practices.ServiceFactory.Validation;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validator class that checks if a specified project item exists within the current project. 
	/// </summary>
	public class ProjectItemIsUniqueValidator : Validator<string>
	{
		private ProjectProvider projectProvider;
		private string languageExtension;

        [SuppressMessage("Microsoft.Design","CA1034:NestedTypesShouldNotBeVisible")]
		public delegate IProjectModel ProjectProvider();

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ProjectItemIsUniqueValidator"/> class.
		/// </summary>
		/// <param name="currentProject">The current project.</param>
		/// <param name="languageExtension">The language extension.</param>
		public ProjectItemIsUniqueValidator(
			IProjectModel currentProject, string languageExtension)
			: this(currentProject, languageExtension, Resources.ProjectItemIsUniqueValidatorMessage)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ProjectItemIsUniqueValidator"/> class.
		/// </summary>
		/// <param name="currentProject">The current project.</param>
		/// <param name="languageExtension">The language extension.</param>
		/// <param name="errorMessage">The error message.</param>
		public ProjectItemIsUniqueValidator(
			IProjectModel currentProject, 
			string languageExtension, string errorMessage) 
			: this(delegate { return currentProject; }, languageExtension, errorMessage)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ProjectItemIsUniqueValidator"/> class.
		/// </summary>
		/// <param name="projectProvider">The current project provider.</param>
		/// <param name="languageExtension">The language extension.</param>
		public ProjectItemIsUniqueValidator(
			ProjectProvider projectProvider, string languageExtension)
			: this(projectProvider, languageExtension, Resources.ProjectItemIsUniqueValidatorMessage)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ProjectItemIsUniqueValidator"/> class.
		/// </summary>
		/// <param name="projectProvider">The current project provider.</param>
		/// <param name="languageExtension">The language extension.</param>
		/// <param name="errorMessage">The error message.</param>
		public ProjectItemIsUniqueValidator(
			ProjectProvider projectProvider, 
			string languageExtension, string errorMessage)
			: base(errorMessage, null)
		{
			this.projectProvider = projectProvider;
			this.languageExtension = languageExtension;
		}

		/// <summary>
		/// Does the validate.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The current target.</param>
		/// <param name="key">The key.</param>
		/// <param name="validationResults">The validation results.</param>
		protected override void DoValidate(string objectToValidate, 
			object currentTarget, string key, ValidationResults validationResults)
		{
			IProjectModel project = projectProvider();
			if (project != null &&
				project.ProjectContainsFile(string.Join(".", new string[] { objectToValidate, languageExtension })))
			{
				this.LogValidationResult(validationResults, this.MessageTemplate, currentTarget, key);
			}
		}

		/// <summary>
		/// Gets the default message template.
		/// </summary>
		/// <value>The default message template.</value>
		protected override string DefaultMessageTemplate
		{
			get { return Resources.ProjectItemIsUniqueValidatorMessage; }
		}
	}
}
