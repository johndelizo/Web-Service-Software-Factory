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
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.Practices.Modeling.ExtensionProvider.TypeDescription;

namespace Microsoft.Practices.UnitTestLibrary
{
	public class MockInvalidArtifactLink : IArtifactLink
	{
		#region IArtifactLink Members

		public Guid Container
		{
			get { return Guid.Empty; }
		}

		public string ItemPath
		{
			get { return "<>"; }
		}

		public IDictionary<string, object> Data
		{
			get { return new Dictionary<string, object>(); }
		}

		#endregion
	}

	[TypeDescriptionProvider(typeof(ExtendedTypeDescriptionProvider))]
	[DomainObjectId("7565f39d-24a8-432b-b44f-fcc66b778c9b")]
	public class ExtensibleMockModelElement : ModelElement, IExtensibleObject
	{
		private object objectExtender;
		private ObjectExtenderContainer objectExtenderContainer;
		string message;

		public ExtensibleMockModelElement(Partition partition, string message)
			: base(partition, new PropertyAssignment[] { })
		{
			this.message = message;
		}

		public string Message
		{
			get { return message; }
		}

		public IArtifactLink InvalidArtifactLink
		{
			get
			{
				return new MockInvalidArtifactLink();
			}
		}

		#region IExtensibleObject Members

		public object ObjectExtender
		{
			get { return objectExtender; }
			set { objectExtender = value; }
		}

		public ObjectExtenderContainer ObjectExtenderContainer
		{
			get { return objectExtenderContainer; }
			set { objectExtenderContainer = value; }
		}

		public ModelElement ModelElement
		{
			get { return (ModelElement)this; }
		}

		public IExtensionProvider ExtensionProvider
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
	}
}
