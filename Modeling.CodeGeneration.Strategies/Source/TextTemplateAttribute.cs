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

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
	public sealed class TextTemplateAttribute: Attribute
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="TextTemplateAttribute"/> class.
		/// </summary>
		/// <param name="templateName">Name of the template.</param>
		public TextTemplateAttribute(string templateName, TextTemplateTargetLanguage targetLanguage)
		{
			this.templateName = templateName;
			this.targetLanguage = targetLanguage;
		} 
		#endregion

		#region Properties
		string templateName;

		/// <summary>
		/// Gets or sets the name of the template.
		/// </summary>
		/// <value>The name of the template.</value>
		public string TemplateName
		{
			get { return templateName; }
		}

		private TextTemplateTargetLanguage targetLanguage;

		public TextTemplateTargetLanguage TargetLanguage
		{
			get { return targetLanguage; }
		}
	
		#endregion
	}
}