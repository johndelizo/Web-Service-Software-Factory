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
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using Microsoft.Practices.Modeling.Common;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;

namespace Microsoft.Practices.VisualStudio.Helper.Design
{
	/// <summary>
	/// Allows selection of an element in the current solution.
	/// </summary>
	/// <remarks>
	/// The editor can be configured through the <see cref="IAttributesConfigurable"/> interface, 
	/// used when attributes are added to the Editor element in the configuration file. 
	/// <para>
	/// If an attributes named <c>UnboundReferenceType</c> is not specified, the editor will use 
	/// the ITypeDescriptorContext.PropertyDescriptor.PropertyType received in the 
	/// <see cref="EditValue(IServiceProvider,HierarchyNode,HierarchyNode)"/> method to determine a valid selection in the picker.
	/// </para>
	/// </remarks>
	public class SolutionPickerEditor : UITypeEditor, IAttributesConfigurable
	{
		private IWindowsFormsEditorService formsService;
		private StringDictionary attributes;
		private string filterTypeName = string.Empty;
		private bool nodeSelected = false;
		/// <summary>
		/// Optional attribute that can be specified at the Editor XML element with the 
		/// name "Filter", used to specify the type of a custom filter object.
		/// The filter object must implement the interface <see cref="ISolutionPickerFilter"/> for default and valid values.
		/// </summary>
		public const string FilterAttribute = "Filter";

		/// <summary>
		/// Returns the edit style of the Solution that is a dropdown
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		/// <summary>
		/// Returns if the SolutionPicker is a dropdown resizable which is true
		/// </summary>
		public override bool IsDropDownResizable
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get { return true; }
		}

		/// <summary>
		/// Edits an HierarchyNode object
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="root"></param>
		/// <param name="value"></param>
		/// <returns></returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        public HierarchyNode EditValue(IServiceProvider provider, HierarchyNode root, HierarchyNode value)
        {
            ISolutionPickerFilter filter = null;
            formsService = (IWindowsFormsEditorService)ServiceHelper.GetService(provider, typeof(IWindowsFormsEditorService), this);
            if (formsService != null)
            {
                using (CreateEditorServiceProvider(provider, out filter))
                {
                    using (SolutionPickerControl control = new SolutionPickerControl(root, value, filter))
                    {
                        control.SelectionChanged += OnSelect;
                        formsService.DropDownControl(control);
                        control.SelectionChanged -= OnSelect;
                        formsService = null;

                        if (nodeSelected)
                        {
                            return control.SelectedTarget;
                        }

                        return null;
                    }
                }
            }
            return value;
        }

		#region IDictionaryService wrapper

		private class DictionaryWrapper : SitedComponent, IDictionaryService
		{
			StringDictionary attributes;
			IDictionaryService dictionaryService;

			public DictionaryWrapper(StringDictionary attributes)
			{
				this.attributes = attributes;
			}

			#region Overides

			protected override void OnSited()
			{
				base.OnSited();
				dictionaryService = (IDictionaryService)GetService(typeof(IDictionaryService));
			}

			#endregion

			#region IDictionaryService Members

			object IDictionaryService.GetKey(object value)
			{
				return dictionaryService.GetKey(value);
			}

			object IDictionaryService.GetValue(object key)
			{
				if(attributes.ContainsKey(key.ToString()))
				{
					return attributes[key.ToString()];
				}
				return dictionaryService.GetValue(key);
			}

			void IDictionaryService.SetValue(object key, object value)
			{
				dictionaryService.SetValue(key, value);
			}

			#endregion
		}

		#endregion IDictionaryService wrapper

		internal Microsoft.Practices.ComponentModel.ServiceContainer CreateEditorServiceProvider(IServiceProvider provider, out ISolutionPickerFilter filter)
		{
			Microsoft.Practices.ComponentModel.ServiceContainer editorServiceProvider = new Microsoft.Practices.ComponentModel.ServiceContainer(true);
			editorServiceProvider.Site = new Site(provider, editorServiceProvider, editorServiceProvider.GetType().FullName);
			editorServiceProvider.AddService(typeof(IDictionaryService), new DictionaryWrapper(attributes));
			filter = CreateEditorFilter(provider);
			if(filter is IComponent)
			{
				editorServiceProvider.Add((IComponent)filter);
			}
			return editorServiceProvider;
		}

		/// <summary>
		/// Creates the filter if there's one
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		protected virtual ISolutionPickerFilter CreateEditorFilter(IServiceProvider provider)
		{
			if(!string.IsNullOrEmpty(filterTypeName))
			{
				ITypeResolutionService typeResService =
					(ITypeResolutionService)provider.GetService(typeof(ITypeResolutionService));
				if(typeResService != null)
				{
					Type filterType = typeResService.GetType(filterTypeName);
					if(filterType != null)
					{
						return (ISolutionPickerFilter)Activator.CreateInstance(filterType);
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Allows to change the value of the Editor
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
            if (provider == null)
            {
                return string.Empty;
            }

			IVsSolution vsSolution = (IVsSolution)ServiceHelper.GetService(provider, typeof(IVsSolution), this);
			HierarchyNode root = new HierarchyNode(vsSolution);
			HierarchyNode currentValue = null;

			if (value == null)
			{
				value = Guid.Empty;
			}

			try
			{
                VSShellHelper.SetWaitCursor(provider);
                if (!string.IsNullOrWhiteSpace(value.ToString()))
				{
					currentValue = new HierarchyNode(vsSolution, value.ToString());
				}

				using (HierarchyNode newValue = EditValue(provider, root, currentValue))
				{
					if (newValue != null)
					{
						return newValue.UniqueName;
					}
				}
			}
			//Thrown if Project doesn't exist on solution
			catch (InvalidOperationException) { }
			//Thrown if Project doesn't exist on solution
			catch (COMException) { }
            catch (ServiceMissingException) { throw ;}
            catch (Exception e)
            {
                VSShellHelper.ShowErrorDialog(provider, e.Message);
            }
			finally
			{
				if (root != null)
					root.Dispose();
				if (currentValue != null)
					currentValue.Dispose();
			}

			return string.Empty;
		}

		protected virtual void OnSelect(object sender, EventArgs<HierarchyNode> e)
		{
			nodeSelected = true;
			formsService.CloseDropDown();
		}

		/// <summary>
		/// Returns true to support custom paint
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// Paints project icon
		/// </summary>
		/// <param name="e"></param>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
		public override void PaintValue(PaintValueEventArgs e)
		{
			Guard.ArgumentNotNull(e, "e");

			IVsSolution vsSolution = (IVsSolution)e.Context.GetService(typeof(IVsSolution));
			string value = Guid.Empty.ToString();
			if(!string.IsNullOrEmpty(e.Value as string))
			{
				value = e.Value.ToString();
			}

			try
			{
				using (HierarchyNode node = new HierarchyNode(vsSolution, value))
				{
					if ((node != null) && (node.Icon != null))
					{
						e.Graphics.DrawIcon(node.Icon, 2, 2);
					}
				}
			}
			catch(COMException)
			{
				//Thrown if Project doesnt exist on solution
			}
		}

		#region IAttributesConfigurable Members

		/// <summary>
		/// Receives the configuration data which must contain an attribute 
		/// <c>UnboundReferenceType</c> with the type to use with 
		/// the <see cref="IUnboundAssetReference"/> to use to determine 
		/// validity of the selection in the dialog.
		/// </summary>
		public void Configure(StringDictionary attributes)
		{
			Guard.ArgumentNotNull(attributes, "attributes");

			if(attributes.ContainsKey(FilterAttribute))
			{
				filterTypeName = attributes[FilterAttribute];
			}
			this.attributes = attributes;
		}

		#endregion
	}

	/// <summary>
	/// Generic version of the SolutionPickerEditor.
	/// </summary>
	/// <typeparam name="Filter">Filter type to be used by the editor</typeparam>
	public class SolutionPickerEditor<TFilter> : SolutionPickerEditor
		where TFilter : ISolutionPickerFilter, new()
	{
		/// <summary>
		/// <see cref="SolutionPickerEditor.CreateEditorFilter"/>
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		protected override ISolutionPickerFilter CreateEditorFilter(IServiceProvider provider)
		{
			return new TFilter();
		}
	}
}
