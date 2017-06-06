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

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.Tests.TextTemplating
{
	internal class MockCodeGenerationService : ICodeGenerationService
	{
		bool valid = false;

		public MockCodeGenerationService(bool valid)
		{
			this.valid = valid;
		}

		#region ICodeGenerationService Members

		int ICodeGenerationService.GenerateArtifact(IArtifactLink link)
		{
            throw new NotImplementedException();
        }

		int ICodeGenerationService.GenerateArtifacts(ICollection<IArtifactLink> artifactLinks)
		{
            throw new NotImplementedException();
        }

		bool ICodeGenerationService.IsValid(IArtifactLink link)
		{
			return valid;
		}

        int ICodeGenerationService.GenerateArtifacts(ICollection<IArtifactLink> artifactLinks, Action<IArtifactLink> linkUpdated)
        {
            throw new NotImplementedException();
        }

		bool ICodeGenerationService.AreValid(ICollection<IArtifactLink> artifactLinks)
		{
            throw new NotImplementedException();
        }

		bool ICodeGenerationService.IsArtifactAlreadyGenerated(IArtifactLink link)
		{
            throw new NotImplementedException();
        }

		Microsoft.Practices.VisualStudio.Helper.HierarchyNode ICodeGenerationService.ValidateDelete(IArtifactLink artifactLink)
		{
            throw new NotImplementedException();
        }

		Microsoft.Practices.VisualStudio.Helper.HierarchyNode ICodeGenerationService.ValidateRename(IArtifactLink artifactLink, string newName, string oldName)
		{
            throw new NotImplementedException();
        }

		ICollection<Microsoft.Practices.VisualStudio.Helper.HierarchyNode> ICodeGenerationService.ValidateDeleteFromCollection(ICollection<IArtifactLink> artifactLinks)
		{
            throw new NotImplementedException();
        }

		ICollection<Microsoft.Practices.VisualStudio.Helper.HierarchyNode> ICodeGenerationService.ValidateRenameFromCollection(ICollection<IArtifactLink> artifactLinks, string newName, string oldName)
		{
            throw new NotImplementedException();
        }

		#endregion
    }
}
