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
using System.Diagnostics;
using System.Threading;

namespace Microsoft.Practices.Modeling.Common.Logging
{
	/// <summary>
	/// The current logging infraestructure
	/// </summary>
	public class LogWriter : IDisposable
	{
        private ICollection<TraceListener> listeners;
		private bool disposed;
        private static object sync = new object();

		public LogWriter()
			: this(new List<TraceListener>(new TraceListener[] { new DefaultTraceListener() }))
		{
		}

		public LogWriter(ICollection<TraceListener> listeners)
		{
			Guard.ArgumentNotNull(listeners, "listeners");
			this.listeners = listeners;
		}

        public ICollection<TraceListener> Listeners
		{
			get { return listeners; }
		}

		public void Write(LogEntry entry)
		{
			Write(entry, null);
		}

		public void Write(LogEntry entry, Type filterListener)
		{
			Guard.ArgumentNotNull(entry, "entry");

			TraceEventCache manager = new TraceEventCache();

			foreach(TraceListener listener in this.Listeners)
			{
                bool lockTaken = false;
				try
				{                    
					if (!listener.IsThreadSafe)
					{
                        Monitor.Enter(sync, ref lockTaken);
					}
					if (filterListener == null ||
						filterListener == listener.GetType())
					{
						listener.TraceData(manager, string.Empty, entry.Severity, 0, entry);
						listener.Flush();
					}
				}
				finally
				{
                    if (!listener.IsThreadSafe && lockTaken)
					{
                        Monitor.Exit(sync);
					}
				}
			}
		}
		
		public void Clear()
		{
			foreach(TraceListener listener in this.Listeners)
			{
				IClearableListener clearableListener = listener as IClearableListener;
				if(clearableListener != null)
				{
                    bool lockTaken = false;
                    try
                    {
                        if (!listener.IsThreadSafe)
                        {
                            Monitor.Enter(sync, ref lockTaken);
                        }
                        clearableListener.Clear();
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.ToString());
                    }					
					finally
					{
						if(!listener.IsThreadSafe && lockTaken)
						{
                            Monitor.Exit(sync);
						}
					}
				}
			}
		}

        public void SuspendRefresh()
        {
            foreach (TraceListener listener in this.Listeners)
            {
                IRefreshableListener refreshableListener = listener as IRefreshableListener;
                if (refreshableListener != null)
                {
                    bool lockTaken = false;
                    try
                    {
                        if (!listener.IsThreadSafe)
                        {
                            Monitor.Enter(sync, ref lockTaken);
                        }
                        refreshableListener.SuspendRefresh();
                    }
                    finally
                    {
                        if (!listener.IsThreadSafe && lockTaken)
                        {
                            Monitor.Exit(sync);
                        }
                    }
                }
            }
        }

        public void ResumeRefresh()
        {
            foreach (TraceListener listener in this.Listeners)
            {
                IRefreshableListener refreshableListener = listener as IRefreshableListener;
                if (refreshableListener != null)
                {
                    bool lockTaken = false;
                    try
                    {
                        if (!listener.IsThreadSafe)
                        {
                            Monitor.Enter(sync, ref lockTaken);
                        }
                        refreshableListener.ResumeRefresh();
                    }
                    finally
                    {
                        if (!listener.IsThreadSafe && lockTaken)
                        {
                            Monitor.Exit(sync);
                        }
                    }
                }
            }
        }

		#region IDisposable Members
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!this.disposed)
			{
				if(disposing)
				{
					foreach(TraceListener listener in this.Listeners)
					{
						listener.Dispose();
					}
				}
			}

			this.disposed = true;
		}

		~LogWriter()
		{
			Dispose(false);
		}
		#endregion
	}
}