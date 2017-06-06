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

namespace Microsoft.Practices.UnitTestLibrary
{
	[DomainObjectId("f79dc996-e28c-4367-bde9-4e5d7b9a3872")]
	public class MockDomainModel : DomainModel
	{
		public static readonly Guid DomainModelId = new global::System.Guid(0xf79dc996, 0xe28c, 0x4367, 0xbd, 0xe9, 0x4e, 0x5d, 0x7b, 0x9a, 0x38, 0x72);

		public MockDomainModel(Store s)
			: base(s, DomainModelId)
		{
		}

		protected override Type[] GetGeneratedDomainModelTypes()
		{
			return new Type[] { typeof(ExtensibleMockModelElement), typeof(UnextendedMockModelElement) };
		}
	}
}
