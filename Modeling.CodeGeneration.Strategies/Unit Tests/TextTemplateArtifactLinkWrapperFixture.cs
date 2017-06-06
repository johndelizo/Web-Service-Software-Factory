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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.Tests
{

	/// <summary>
	/// Test fixture for TextTemplateArtifactLinkWrapp
	/// </summary>
	[TestClass]
	public class TextTemplateArtifactLinkWrapperFixture
	{
		public TextTemplateArtifactLinkWrapperFixture()
		{
		}


		[TestMethod]
		public void ShouldReturnCSharpTemplateRefFromAttribute()
		{
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(new TestableArtifactLink());
			wrappedLink.ResourceResolver = new MockResourceResolver();

			Assert.AreEqual<string>("MyTemplate.cs.tt", wrappedLink.GetTemplateRef(TextTemplateTargetLanguage.CSharp));		
		}

		[TestMethod]
		public void ShouldReturnVBTemplateRefFromAttribute()
		{
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(new TestableArtifactLink());
			wrappedLink.ResourceResolver = new MockResourceResolver();

			Assert.AreEqual<string>("MyTemplate.vb.tt", wrappedLink.GetTemplateRef(TextTemplateTargetLanguage.VB));
		}

		[TestMethod]
		public void ShouldReturnAnyTemplateRefFromAttribute()
		{
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(new TestableArtifactLink());
			wrappedLink.ResourceResolver = new MockResourceResolver();

			Assert.AreEqual<string>("MyTemplate.tt", wrappedLink.GetTemplateRef(TextTemplateTargetLanguage.Any));
		}

		[TestMethod]
		public void ShouldReturnNullIfTargetLanguageNotFound()
		{
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(new TestableArtifactLinkWithOnlyVB());
			wrappedLink.ResourceResolver = new MockResourceResolver();

			Assert.IsNull(wrappedLink.GetTemplateRef(TextTemplateTargetLanguage.CSharp), "GetTemlateRef did not return null");
		}

		[TestMethod]
		public void ShouldReturnCSharpTemplateFromAttribute()
		{
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(new TestableArtifactLink());
			wrappedLink.ResourceResolver = new MockResourceResolver();

			Assert.AreEqual<string>(MockResourceResolver.ResourceContent, wrappedLink.GetTemplate(TextTemplateTargetLanguage.CSharp));

		}

		[TestMethod]
		public void ShouldReturnTemplateRefFromAttribute()
		{
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(new TestableArtifactLink());
			wrappedLink.ResourceResolver = new MockResourceResolver();

			Assert.AreEqual<string>("MyTemplate.cs.tt", wrappedLink.GetTemplateRef(TextTemplateTargetLanguage.CSharp));			
		}

		[TestMethod]
		public void ShouldReturnTemplateFromAttribute()
		{
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(new TestableArtifactLink());
			wrappedLink.ResourceResolver = new MockResourceResolver();

			Assert.AreEqual<string>(MockResourceResolver.ResourceContent, wrappedLink.GetTemplate(TextTemplateTargetLanguage.CSharp));

		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ShouldThrowIfNoTextTemplateAttribute()
		{
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(new NonAttributedArtifactLink());
		}

		[TestMethod]
		public void ShouldDelegateContainerPropertyToArtifactLink()
		{
			TestableArtifactLink testableLink = new TestableArtifactLink();
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(testableLink);

			Guid guid = wrappedLink.Container;

			Assert.IsTrue(testableLink.ContainerCalled, "TestableArtifactLink Container property not called");
		}

		[TestMethod]
		public void ShouldDelegateItemPathPropertyToArtifactLink()
		{
			TestableArtifactLink testableLink = new TestableArtifactLink();
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(testableLink);

			string itemPath = wrappedLink.ItemPath;

			Assert.IsTrue(testableLink.ItemPathCalled, "TestableArtifactLink ItemPath property not called");
		}

		[TestMethod]
		public void ShouldDelegateDataPropertyToArtifactLink()
		{
			TestableArtifactLink testableLink = new TestableArtifactLink();
			TextTemplateArtifactLinkWrapper wrappedLink = new TextTemplateArtifactLinkWrapper(testableLink);

			IDictionary<string, object> data = wrappedLink.Data;

			Assert.IsTrue(testableLink.DataCalled, "TestableArtifactLink Data property not called");
		}

		class MockResourceResolver : IResourceResolver
		{

			public static readonly string ResourceContent = "TextTemplateContent";
			#region IResourceResolver Members

			public string GetResourcePath(string resourceItem)
			{
				return resourceItem;
			}

			public string GetResource(string resourceItem)
			{
				return ResourceContent;
			}

			#endregion
		}

		class NonAttributedArtifactLink : IArtifactLink
		{
			#region IArtifactLink Members

			public Guid Container
			{
				get { throw new NotImplementedException("The method or operation is not implemented."); }
			}

			public string ItemPath
			{
				get { throw new NotImplementedException("The method or operation is not implemented."); }
			}

			public IDictionary<string, object> Data
			{
				get { throw new Exception("The method or operation is not implemented."); }
			}

			#endregion
		}

		[TextTemplate("MyTemplate.vb.tt", TextTemplateTargetLanguage.VB)]
		class TestableArtifactLinkWithOnlyVB : IArtifactLink
		{

			public bool ContainerCalled = false;
			public bool ItemPathCalled = false;
			public bool DataCalled = false;

			#region IArtifactLink Members

			public Guid Container
			{
				get
				{
					ContainerCalled = true;
					return Guid.NewGuid();
				}
			}

			public string ItemPath
			{
				get
				{
					ItemPathCalled = true;
					return string.Empty;
				}
			}

			public IDictionary<string, object> Data
			{
				get
				{
					DataCalled = true;
					return new Dictionary<string, object>();
				}
			}

			#endregion
		}

		[TextTemplate("MyTemplate.vb.tt", TextTemplateTargetLanguage.VB)]
		[TextTemplate("MyTemplate.cs.tt", TextTemplateTargetLanguage.CSharp)]
		[TextTemplate("MyTemplate.tt", TextTemplateTargetLanguage.Any)]
		class TestableArtifactLink : IArtifactLink
		{

			public bool ContainerCalled = false;
			public bool ItemPathCalled = false;
			public bool DataCalled = false;

			#region IArtifactLink Members

			public Guid Container
			{
				get
				{
					ContainerCalled = true;
					return Guid.NewGuid();
				}
			}

			public string ItemPath
			{
				get
				{
					ItemPathCalled = true;
					return string.Empty;
				}
			}

			public IDictionary<string, object> Data
			{
				get
				{
					DataCalled = true;
					return new Dictionary<string, object>();
				}
			}

			#endregion
		}
	}
}
