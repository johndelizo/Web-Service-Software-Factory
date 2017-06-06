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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors
{
	/// <summary>
	/// Base list editor class
	/// </summary>
	[ServiceDependency(typeof(IWindowsFormsEditorService))]
	[ServiceDependency(typeof(IDictionaryService))]
	public class ListEditor : UITypeEditor, IAttributesConfigurable, IDisposable
	{
		private const string ListArgumentAttributeName = "ListArgument";
		private bool disposed;
		private ListBox control;
		private IWindowsFormsEditorService windowsFormsEditorService;
		private string listArgumentAttributeName;
		private IEnumerable list;
		private IDictionary dictionary;

		/// <summary>
		/// Initializes a new instance of the <see cref="ListEditor"/> class.
		/// </summary>
		public ListEditor()
		{
			this.Control = new ListBox();
			this.Control.SelectedIndexChanged += OnControlSelectedIndexChanged;
		}

		/// <summary>
		/// Gets or sets the control.
		/// </summary>
		/// <value>The control.</value>
		protected ListBox Control
		{
			get { return this.control;	}
			set	{ this.control = value;	}
		}

		/// <summary>
		/// Gets or sets the windows forms editor service.
		/// </summary>
		/// <value>The windows forms editor service.</value>
		protected IWindowsFormsEditorService WindowsFormsEditorService
		{
			get	{ return this.windowsFormsEditorService; }
			set	{ this.windowsFormsEditorService = value; }
		}

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
			this.WindowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(
				typeof(IWindowsFormsEditorService));

			InitializeList(provider);
			FillValues();

			this.WindowsFormsEditorService.DropDownControl(this.control);

			if (this.Control.SelectedItems.Count == 1)
			{
				value = (dictionary != null ? 
					dictionary[this.Control.SelectedItem] : 
					this.Control.SelectedItem);
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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
		public override bool IsDropDownResizable
		{
			get	{return true;}
		}

		#endregion

		#region Protected Implementation

		/// <summary>
		/// Fills the values.
		/// </summary>
		protected virtual void FillValues()
		{
			InternalFillValues();
		}

		/// <summary>
		/// Gets the list of values provided in .
		/// </summary>
		/// <value>The list.</value>
		protected virtual IEnumerable List
		{
			get	{ return list; }
			set	{ list = value; }
		}

		/// <summary>
		/// Adds the key value items.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		protected virtual void AddKeyValueItems(IDictionary dictionary)
		{
			Guard.ArgumentNotNull(dictionary, "dictionary");
			this.dictionary = dictionary;
			this.InternalFillValues();
		}

		/// <summary>
		/// Handles the SelectedIndexChanged event of the control control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
		protected void OnControlSelectedIndexChanged(object sender, EventArgs e)
		{
			this.WindowsFormsEditorService.CloseDropDown();
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes the specified disposing.
		/// </summary>
		/// <param name="disposing">if set to <c>true</c> [disposing].</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.Control.Dispose();
				}
			}
			disposed = true;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="ListEditor"/> is reclaimed by garbage collection.
		/// </summary>
		~ListEditor()
		{
			Dispose(false);
		}

		#endregion

		#region IAttributesConfigurable Members

		/// <summary>
		/// Configures the component using the dictionary of attributes specified
		/// in the configuration file.
		/// </summary>
		/// <param name="attributes">The attributes in the configuration element.</param>
		public void Configure(StringDictionary attributes)
		{
			Guard.ArgumentNotNull(attributes, "attributes");

			if (attributes.ContainsKey(ListArgumentAttributeName))
			{
				listArgumentAttributeName = attributes[ListArgumentAttributeName];
			}
		}

		#endregion

		#region Private Implementation

		private void InitializeList(IServiceProvider provider)
		{
			if (list == null)
			{
				IDictionaryService dictionaryService = (IDictionaryService)provider.GetService(typeof(IDictionaryService));
				if (dictionaryService != null)
				{
					list = dictionaryService.GetValue(listArgumentAttributeName) as IEnumerable;
					Guard.ArgumentNotNull(list, ListArgumentAttributeName);
				}
			}
		}

		private void InternalFillValues()
		{
			if (dictionary != null)
			{
				this.Control.DataSource = new ArrayList(dictionary.Keys);
			}
			else
			{
				this.Control.DataSource = this.List;
			}
		}

		#endregion
	}
}