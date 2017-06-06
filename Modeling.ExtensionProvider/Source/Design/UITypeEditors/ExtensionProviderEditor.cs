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
using System.Security.Permissions;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Globalization;
using Microsoft.Practices.Modeling.ExtensionProvider.Services;
using Microsoft.Practices.Modeling.ExtensionProvider.Extension;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.ExtensionProvider.Helpers;

namespace Microsoft.Practices.Modeling.ExtensionProvider.Design.UITypeEditors
{
	/// <summary>
	/// Base UITypeEditor used to provide a list of ExtensionProviders and responsible for inyecting the ObjectExtender on the correspoding MEL
	/// </summary>
	public class ExtensionProviderEditor : UITypeEditor, IDisposable
	{
		#region Fields
		private ListView control;
		private IWindowsFormsEditorService windowsFormsEditorService;
		private bool disposed;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionProviderEditor"/> class.
		/// </summary>
		public ExtensionProviderEditor()
		{
			this.control = new ListView();
			this.control.MultiSelect = false;
			this.control.SelectedIndexChanged += new EventHandler(control_SelectedIndexChanged);
			this.control.View = View.List;
		}
		#endregion

		#region Public Implementation
		/// <summary>
		/// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> method.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
		/// <param name="provider">An <see cref="T:System.IServiceProvider"></see> that this editor can use to obtain services.</param>
		/// <param name="value">The object to edit.</param>
		/// <returns>
		/// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
		/// </returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

			FillValues(ServiceHelper.GetExtensionProviderService(context).ExtensionProviders, context);

			windowsFormsEditorService.DropDownControl(this.control);

			if (this.control.SelectedItems.Count == 1)
			{
				value = this.control.SelectedItems[0].Tag;
				ExtendModelElements(context, value);
			}

			return value;
		}

		/// <summary>
		/// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
		/// <returns>
		/// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
		/// </returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		/// <summary>
		/// Gets a value indicating whether drop-down editors should be resizable by the user.
		/// </summary>
		/// <value></value>
		/// <returns>true if drop-down editors are resizable; otherwise, false. </returns>
		public override bool IsDropDownResizable
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return true;
			}
		}
		#endregion

		#region Protected Implementation
		/// <summary>
		/// Extends a model element with the corresponding ObjectExtender based of the ExtenderProvider.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="value">The value.</param>
		protected virtual void ExtendModelElements(ITypeDescriptorContext context, object value)
		{
			ModelElement rootElement = context.Instance as ModelElement;
			if (rootElement == null)
			{
				return;
			}

			IList<ModelElement> extensibleObjects = new List<ModelElement>();
			ElementWalker walker = new DepthFirstElementWalker(
					new TypeMatchingElementVisitor<IExtensibleObject>(extensibleObjects),
					new EmbeddingVisitorFilter()
				);

			walker.DoTraverse(rootElement);

			if (extensibleObjects.Count > 0)
			{
				using (Transaction transaction = rootElement.Store.TransactionManager.BeginTransaction())
				{
					foreach (ModelElement mel in extensibleObjects)
					{
						ProvideObjectExtender(mel, mel as IExtensibleObject, value as IExtensionProvider);
					}

					transaction.Commit();
				}
			}
		}

		#endregion

		#region Private Implementation
		
		private static void ProvideObjectExtender(ModelElement mel, IExtensibleObject extensibleObject, IExtensionProvider extensionProvider)
		{
			ExtensionProviderHelper extensionProviderHelper = new ExtensionProviderHelper(extensibleObject);

			if (extensibleObject.ObjectExtenderContainer == null)
			{
				extensionProviderHelper.CreateObjectExtenderContainer();
				object extender = extensionProviderHelper.CreateObjectExtender(extensionProvider, mel);
				extensionProviderHelper.SetObjectExtender(extender);
			}
			else
			{
				if (extensionProviderHelper.GetObjectExtender(extensionProvider) == null)
				{
					object extender = extensionProviderHelper.CreateObjectExtender(extensionProvider, mel);
					extensionProviderHelper.SetObjectExtender(extender);
				}
				else
				{
					//Redirect object extender
					extensionProviderHelper.SetObjectExtender(extensionProviderHelper.GetObjectExtender(extensionProvider));
				}
			}
		}

		private void FillValues(IList<IExtensionProvider> extensionProviders, ITypeDescriptorContext context)
		{
			this.control.Items.Clear();
			ModelElement mel = context.Instance as ModelElement;

			if (mel == null)
			{
				return;
			}

			foreach (IExtensionProvider extensionProvider in extensionProviders)
			{
				foreach (DomainModel model in mel.Store.DomainModels)
				{
					if (model.GetType() == ExtensionProviderHelper.GetDomainModelType(extensionProvider))
					{
						ListViewItem item = new ListViewItem(extensionProvider.Description);
						item.Tag = extensionProvider;
						this.control.Items.Add(item);
					}
				}
			}
		}

		private void control_SelectedIndexChanged(object sender, EventArgs e)
		{
			windowsFormsEditorService.CloseDropDown();
		}
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					control.Dispose();
				}
			}

			disposed = true;
		}

		#endregion
	}
}