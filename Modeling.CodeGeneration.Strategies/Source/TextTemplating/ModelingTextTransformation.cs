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
using System.CodeDom.Compiler;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Integration;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.Modeling.CodeGeneration.Strategies.TextTemplating
{
	[CLSCompliant(false)]
	public abstract class ModelingTextTransformation : TextTransformation, ITextTemplateHost
	{
		/// <summary>
		/// Gets the object extender.
		/// </summary>
		/// <param name="modelElement">The model element.</param>
		/// <returns></returns>
		protected TExtension GetObjectExtender<TExtension>(ModelElement modelElement)
			where TExtension:class
		{
			Guard.ArgumentNotNull(modelElement, "modelElement");

			if (modelElement is IExtensibleObject)
			{
				return ((IExtensibleObject)modelElement).ObjectExtender as TExtension;
			}
			throw new NotImplementedException(Properties.Resources.IExtensibleObjectNotImplemented);
		}

		/// <summary>
		/// Gets the artifact link.
		/// </summary>
		/// <param name="modelElement">The model element.</param>
		/// <returns></returns>
		protected ArtifactLink GetArtifactLink(ModelElement modelElement)
		{
			ArtifactLink link = ArtifactLinkHelper.GetFirstArtifactLink(modelElement);
			if (link != null)
			{
				return link; 
			}
			throw new NotImplementedException(Properties.Resources.ArtifactLinkNotImplemented);
		}

		private void Log(CompilerError error)
		{
			this.Errors.Add(error);
		}

		#region ITextTemplateHost Members

		public abstract void AddProjectReference(IArtifactLink link);

		public abstract bool IsValid(IArtifactLink link);

		public abstract ModelElement ResolveModelReference(ModelBusReference mbReference);

		public void LogError(string title, string description)
		{
			CompilerError error = new CompilerError(string.Empty,0,0,title,description);
			error.IsWarning = false;
			Log(error);
		}

		public void LogError(string description)
		{
			LogError(string.Empty, description);
		}

		public void LogWarning(string title, string description)
		{
			CompilerError error = new CompilerError(string.Empty, 0, 0, title, description);
			error.IsWarning = true;
			Log(error);
		}

		public void LogWarning(string description)
		{
			LogWarning(string.Empty, description);
		}

		#endregion
	}
}
