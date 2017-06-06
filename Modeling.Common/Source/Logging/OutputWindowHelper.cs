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
using EnvDTE80;
using EnvDTE;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Practices.Modeling.Common.Logging
{
	public class OutputWindowHelper
	{
		#region Fields
		DTE2 dte;
		string paneName;
		#endregion

		#region Constructors
		public OutputWindowHelper(DTE2 value, string paneName)
		{
            this.dte = value;
			this.paneName = paneName;
		}
		#endregion

		#region Properties
		private OutputWindowPane pane;

		protected OutputWindowPane Pane
		{
			get
			{
				if(pane == null)
				{
					pane = GetPane(OutputWindow, this.paneName);
				}

				return pane;
			}
		}

		private OutputWindow outputWindow;

		protected OutputWindow OutputWindow
		{
			get
			{
				if(outputWindow == null)
				{
					outputWindow = this.dte.ToolWindows.OutputWindow;
				}

				return outputWindow;
			}
		}
		#endregion

		#region Public Implementation

		public void WriteMessage(string message)
		{
            try
            {
                Application.DoEvents();
                this.Pane.OutputString(message);
                this.Pane.Activate();
                //this.OutputWindow.Parent.Activate();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
		}

		public void Clear()
		{
			this.Pane.Clear();
		}

		#endregion

		#region Private Implementation
		private static OutputWindowPane GetPane(OutputWindow outputWindow, string panelName)
		{
			foreach(OutputWindowPane pane in outputWindow.OutputWindowPanes)
			{
				if(pane.Name.Equals(panelName, StringComparison.OrdinalIgnoreCase))
				{
					return pane;
				}
			}

			return outputWindow.OutputWindowPanes.Add(panelName);
		}
		#endregion
	}
}