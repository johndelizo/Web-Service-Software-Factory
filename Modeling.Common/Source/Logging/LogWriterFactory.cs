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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Concurrent;

namespace Microsoft.Practices.Modeling.Common.Logging
{
	/// <summary>
	/// The current logging infraestructure
	/// </summary>
	public class LogWriterFactory
	{
        static HashSet<TraceListener> listeners = new HashSet<TraceListener>() { new DefaultTraceListener() };
        static readonly TraceListener vsErrorListener = new VSErrorListWindowListener();
        static readonly TraceListener vsOutputListener = new VSOutputWindowListener();

        public static void AddVSListeners()
        {
            // this will add only once each listener
            listeners.Add(vsErrorListener);
            listeners.Add(vsOutputListener);
        }

		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		public LogWriter Create()
		{
			return new LogWriter(listeners);
		}
	}
}