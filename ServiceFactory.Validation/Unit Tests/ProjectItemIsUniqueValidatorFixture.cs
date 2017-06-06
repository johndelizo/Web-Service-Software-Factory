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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicesGuidancePackage.Tests.Common;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Modeling.Presentation.Models;

namespace Microsoft.Practices.ServiceFactory.Validation.Tests
{
	[TestClass]
    public class ProjectItemIsUniqueValidatorFixture
	{
		IProjectModel currentProject;
		Validator<string> itemDoesntExistValidator;

		[TestInitialize]
		public void Setup()
		{
			currentProject = new MockProjectModel("ExistingItem.cs");
			itemDoesntExistValidator = new ProjectItemIsUniqueValidator(currentProject, "cs");
		}

		[TestMethod]
		public void ShouldPassOnUniqueProjectItem()
		{
			ValidationResults validationResults = itemDoesntExistValidator.Validate("ANewItem");

			Assert.IsTrue(validationResults.IsValid);
		}

		[TestMethod]
		public void ShouldFailOnDuplicateProjectItem()
		{
			ValidationResults validationResults =  itemDoesntExistValidator.Validate("ExistingItem");

			Assert.IsFalse(validationResults.IsValid);
			foreach (ValidationResult result in validationResults)
			{
				Assert.AreEqual(itemDoesntExistValidator.MessageTemplate, result.Message);
			}
		}
	}
}