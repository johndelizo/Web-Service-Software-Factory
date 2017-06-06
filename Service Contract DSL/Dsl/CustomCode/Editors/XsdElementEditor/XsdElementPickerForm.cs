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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.ServiceFactory.Description;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	[CLSCompliant(false)]
	public partial class XsdElementPickerForm : Form
	{
		//For Design time only
		private XsdElementPickerForm()
			: this(null)
		{
		}

		public XsdElementPickerForm(IServiceProvider serviceProvider)
		{
			InitializeComponent();
			InitializeControls(serviceProvider);
		}

		public string XsdElementUri
		{
			get { return xsdElementBrowser.XsdElementUri.ToString(); }
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.None;
			Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void xsdElementBrowser_ElementAccepted(object sender, EventArgs e)
		{
			ProcessSelection(true);
		}

		private void xsdElementBrowser_SelectionChanged(object sender, EventArgs e)
		{
			ProcessSelection(false);
		}

		private void InitializeControls(IServiceProvider serviceProvider)
		{
			this.xsdElementBrowser.ServiceProvider = serviceProvider;
			this.xsdElementBrowser.SelectionChanged += new EventHandler(xsdElementBrowser_SelectionChanged);
			this.xsdElementBrowser.ElementAccepted += new EventHandler(xsdElementBrowser_ElementAccepted);
		}

		private void ProcessSelection(bool closeDialog)
		{
			if(xsdElementBrowser.XsdElementUri != null)
			{
				if(closeDialog)
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				this.btnOk.Enabled = true;
			}
			else
			{
				this.btnOk.Enabled = false;
			}
		}
	}
}