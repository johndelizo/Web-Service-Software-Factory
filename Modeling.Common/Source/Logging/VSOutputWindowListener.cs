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
using System.Diagnostics;
using EnvDTE80;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.Globalization;

namespace Microsoft.Practices.Modeling.Common.Logging
{
	public class VSOutputWindowListener : TraceListener, IClearableListener
	{
		private OutputWindowHelper outputWindowHelper;

		protected OutputWindowHelper OutputWindowHelper
		{
			get
			{
				if(outputWindowHelper == null)
				{
					DTE2 dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
					if(dte == null) return null;
					outputWindowHelper = new OutputWindowHelper(dte, Properties.Resources.OutputWindowPanelName);
				}
				return outputWindowHelper;
			}
		}

		public override void Write(string message)
		{
			WriteLine(message);
		}

		public override void WriteLine(string message)
		{
			message = string.Format(CultureInfo.CurrentCulture, "{0}{1}", message, Environment.NewLine);
			if (OutputWindowHelper != null)
			{
				this.OutputWindowHelper.WriteMessage(message);
			}
			else
			{
				Trace.TraceError(message);
			}
		}

		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if(data is LogEntry || 
			   data is string)
			{
				Write(data);
			}
			else
			{
				base.TraceData(eventCache, source, eventType, id, data);
			}
		}

		#region IClearableListener Members

		public void Clear()
		{
			if (OutputWindowHelper != null)
			{
				this.OutputWindowHelper.Clear();
			}
		}

		#endregion
	}
}