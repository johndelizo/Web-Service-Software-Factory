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
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Build.Framework;
using System.Runtime.InteropServices;
using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.VisualStudio.Helper;

namespace Microsoft.Practices.Modeling.CodeGeneration
{
	[ComVisible(true)]
	[Guid("A7C4A96E-6D0B-4bc1-B8A3-6B0B030AD272")]
	public interface ICodeGenerationService
	{
		int GenerateArtifact(IArtifactLink artifactLink);
		int GenerateArtifacts(ICollection<IArtifactLink> artifactLinks);
        int GenerateArtifacts(ICollection<IArtifactLink> artifactLinks, Action<IArtifactLink> linkUpdated);

		bool IsValid(IArtifactLink artifactLink);
		bool AreValid(ICollection<IArtifactLink> artifactLinks);

		bool IsArtifactAlreadyGenerated(IArtifactLink artifactLink);

		HierarchyNode ValidateDelete(IArtifactLink artifactLink);
		HierarchyNode ValidateRename(IArtifactLink artifactLink, string newName, string oldName);
		ICollection<HierarchyNode> ValidateDeleteFromCollection(ICollection<IArtifactLink> artifactLinks);
		ICollection<HierarchyNode> ValidateRenameFromCollection(ICollection<IArtifactLink> artifactLinks, string newName, string oldName);
	}
}
