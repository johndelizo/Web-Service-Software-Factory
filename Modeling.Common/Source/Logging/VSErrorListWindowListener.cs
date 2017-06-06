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
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Practices.Modeling.Common.Logging
{
    public class VSErrorListWindowListener : TraceListener, IClearableListener, IRefreshableListener
	{
		#region Properties

		private ErrorListProvider errorListProvider;

		protected ErrorListProvider ErrorListProvider
		{
			get
			{
				if(errorListProvider == null)
				{
                    IServiceProvider serviceProvider = RuntimeHelper.ServiceProvider;
					errorListProvider = new ErrorListProvider(serviceProvider);
				}
				return errorListProvider;
			}
		}

		#endregion

		public override void Write(string message)
		{
			WriteError(message);
		}

		public override void WriteLine(string message)
		{
			Write(message);
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if(data is LogEntry)
			{
				WriteError((LogEntry)data);
			}
			else if(data is string)
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
			if (this.ErrorListProvider != null)
			{
				this.ErrorListProvider.Tasks.Clear();
			}
		}

		#endregion

		#region Protected Implementation

		protected virtual void WriteError(string message)
		{
			ErrorTask errorTask = new ErrorTask();

			errorTask.CanDelete = false;
			errorTask.ErrorCategory = TaskErrorCategory.Error;
			errorTask.Text = message;

			WriteError(errorTask);
		}

		protected virtual void WriteError(LogEntry logEntry)
		{
			ErrorTask errorTask = new ErrorTask();

			errorTask.CanDelete = false;

			switch(logEntry.Severity)
			{
				case TraceEventType.Critical:
					errorTask.ErrorCategory = TaskErrorCategory.Error;
					break;
				case TraceEventType.Error:
					errorTask.ErrorCategory = TaskErrorCategory.Error;
					break;
				case TraceEventType.Information:
					errorTask.ErrorCategory = TaskErrorCategory.Message;
					break;
				case TraceEventType.Warning:
					errorTask.ErrorCategory = TaskErrorCategory.Warning;
					break;
				default:
					errorTask.ErrorCategory = TaskErrorCategory.Error;
					break;
			}

			errorTask.Text = logEntry.Message;

			WriteError(errorTask);
		}

		protected virtual void WriteError(ErrorTask errorTask)
		{
			if (this.ErrorListProvider != null)
			{
                try
                {
                    this.ErrorListProvider.Tasks.Add(errorTask);
                    this.ErrorListProvider.BringToFront();
                    this.ErrorListProvider.ForceShowErrors();
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
			}
		}

		#endregion

        #region IRefreshableListener Members

        public void SuspendRefresh()
        {
            if (this.ErrorListProvider != null)
            {
                this.ErrorListProvider.SuspendRefresh();
            }
        }

        public void ResumeRefresh()
        {
            if (this.ErrorListProvider != null)
            {
                this.ErrorListProvider.ResumeRefresh();
            }
        }

        #endregion
    }
}