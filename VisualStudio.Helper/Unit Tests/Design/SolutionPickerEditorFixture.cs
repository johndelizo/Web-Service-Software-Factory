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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.VisualStudio.Helper.Design;
using System.Windows.Forms;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.Practices.UnitTestLibrary;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.VisualStudio.Helper.Tests
{
	[TestClass]
	public class SolutionPickerEditorFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsIfPaintValueArgumentIsNull()
		{
			SolutionPickerEditor target = new SolutionPickerEditor();
			target.PaintValue(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ServiceMissingException))]
		public void EditValueWithNoFormsServiceThrows()
		{
			MockVSHierarchy root = new MockVSHierarchy();
			MockVsSolution solution = new MockVsSolution(root);
			MockServiceProvider serviceProvider = new MockServiceProvider();
			serviceProvider.AddService(typeof(IVsSolution), solution);
			MockTypeDescriptorContext context = new MockTypeDescriptorContext(serviceProvider);
			string value = "Project1.txt";
			MockVSHierarchy project = new MockVSHierarchy(value);
			root.AddProject(project);
			SolutionPickerEditor target = new SolutionPickerEditor();
			target.EditValue(serviceProvider, null);
		}
		internal class MockWindowsFormsEditorService : IWindowsFormsEditorService
		{
			Form parentForm;

			public MockWindowsFormsEditorService(Form parentForm)
			{
				this.parentForm = parentForm;
			}

			#region IWindowsFormsEditorService Members

			void IWindowsFormsEditorService.CloseDropDown()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			void IWindowsFormsEditorService.DropDownControl(Control control)
			{
				parentForm.Controls.Add(control);
				TreeView treeView = SolutionPickerControlFixture.GetControl<TreeView>(control.Controls);
				treeView.ExpandAll();
				treeView.SelectedNode = treeView.Nodes[0].Nodes[0];
			}

			DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

	}
}
