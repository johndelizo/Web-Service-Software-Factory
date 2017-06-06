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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;
using Microsoft.Practices.Modeling.ExtensionProvider.Metadata;
using System.Diagnostics;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Extension
{
	[CLSCompliant(false)]
	[Serializable]
	public abstract class ObjectExtender<TModelElement>: IObjectExtender<TModelElement>, IObjectExtenderInternal
		where TModelElement:ModelElement
	{
        [NonSerialized]
        private TModelElement modelElement;
        
        #region IObjectExtenderInternal Members

		[Browsable(false)]
        [XmlIgnore]
        Microsoft.VisualStudio.Modeling.ModelElement IObjectExtenderInternal.ModelElement
		{
			set { this.ModelElement = (TModelElement)value; }
		}

		#endregion

		#region IObjectExtender Members

		[Browsable(false)]
        [XmlIgnore]
        Microsoft.VisualStudio.Modeling.ModelElement IObjectExtender.ModelElement
		{
			get { return this.ModelElement; }
		}

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual ICollection<IArtifactLink> ArtifactLinks
		{
			get
			{
				try
				{
					List<IArtifactLink> links = new List<IArtifactLink>();
					foreach (PropertyInfo prop in this.GetType().GetProperties())
					{
						if (typeof(IArtifactLink).IsAssignableFrom(prop.PropertyType))
						{
							object oLink = prop.GetValue(this, null);
							if (oLink is IArtifactLink)
							{
								links.Add((IArtifactLink)oLink);
							}
						}
					}
					return links;
				}
				catch (TargetInvocationException targetInvocationEx)
				{
					Debug.Assert(targetInvocationEx.InnerException != null);
					// Because we are using the reflection APIs on prop.GetValue if an exception is thrown
					// then the real exception is wrapped around a TargetInvocationException
					// The inner exception is the one that we are really insterested in seeing or displaying 
					// to the user.
					if (targetInvocationEx.InnerException != null)
					{
						throw targetInvocationEx.InnerException;
					}
					else
					{
						throw;
					}
				}
			}
		}

		#endregion

		protected object GetService(Type serviceType)
		{
			if (modelElement != null && modelElement.Store!=null )
			{
				return modelElement.Store.GetService(serviceType);
			}
			return null;
		}

		// FXCOP: False positive, methods have different generic signatures
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		protected T GetService<T>()
		{
			return (T)GetService(typeof(T));
		}

		// FXCOP: False positive, methods have different generic signatures
		[SuppressMessage("Microsoft.Usage", "CA2223:MembersShouldDifferByMoreThanReturnType")]
		protected TInterface GetService<TInterface, TImpl>()
		{
			return (TInterface)GetService(typeof(TImpl));
		}

		[Browsable(false)]
		[ReadOnly(true)]
		[XmlIgnore]
		public virtual TModelElement ModelElement
		{
			get { return modelElement; }
			set { modelElement = value; }
		}
	}
}
