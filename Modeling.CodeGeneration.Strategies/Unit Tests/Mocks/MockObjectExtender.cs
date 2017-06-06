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
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts.Design;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using Microsoft.Practices.UnitTestLibrary;
using System;
using System.ComponentModel;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.Tests.Mocks
{
	[Serializable]
	[ObjectExtender(typeof(ExtensibleMockModelElement))]
	public class MockObjectExtender : ObjectExtender<ExtensibleMockModelElement>
	{
		public MockArtifactLink ArtifactLink
		{
			get
			{
				MockArtifactLink link = new MockArtifactLink();
				link.Container = Guid.NewGuid();

				return link;
			}
		}
	}

	[TextTemplate("SomeTemplate.tt", TextTemplateTargetLanguage.CSharp )]
	[TypeConverter(typeof(ArtifactLinkConverter<MockArtifactLink>))]
	[CodeGenerationStrategy(typeof(TextTemplateCodeGenerationStrategy))]
	public class MockArtifactLink : ArtifactLink
	{
	}
}
