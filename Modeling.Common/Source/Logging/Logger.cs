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
using System.Diagnostics.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Practices.Modeling.Common.Logging
{
	/// <summary>
	/// The current logging infrastructure
	/// </summary>
	public static class Logger
	{
		private static LogWriterFactory factory = new LogWriterFactory();
        private static ConcurrentDictionary<TraceEventType, int> messages = new ConcurrentDictionary<TraceEventType, int>();
        private static readonly LogWriter writer = factory.Create();

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Logger()
		{
			InitMessages();
		}

        public static IDictionary<TraceEventType, int> Messages
		{
			get	{ return messages; }
		}

		public static void Write(object message)
		{
			Write(message, LogEntry.DefaultTitle, LogEntry.DefaultSeverity, LogEntry.DefaultEventId);
		}

		public static void Write(object message, string title)
		{
			Write(message, title, LogEntry.DefaultSeverity, LogEntry.DefaultEventId);
		}

		public static void Write(object message, TraceEventType severity)
		{
			Write(message, LogEntry.DefaultTitle, severity, LogEntry.DefaultEventId);
		}

		public static void Write(object message, string title, TraceEventType severity, int eventId)
		{
			Guard.ArgumentNotNull(message, "message");
			LogEntry entry = new LogEntry(message, title, severity, eventId);
			Write(entry);
		}

		public static void Write(LogEntry entry)
		{
			InternalWrite(entry, null);
		}

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static void Write<TListener>(LogEntry entry)
			where TListener : TraceListener
		{
			InternalWrite(entry, typeof(TListener));
		}

		public static void Clear()
		{
			writer.Clear();
			InitMessages();
		}

        public static void SuspendRefresh()
        {
            writer.SuspendRefresh();
        }

        public static void ResumeRefresh()
        {
            writer.ResumeRefresh();
        }

		private static void InternalWrite(LogEntry entry, Type filterListener)
		{
			Guard.ArgumentNotNull(entry, "entry");
			writer.Write(entry, filterListener);
			messages[entry.Severity]++;
		}

		private static void InitMessages()
		{
			messages.Clear();
			foreach (string eventType in Enum.GetNames(typeof(TraceEventType)))
			{
				messages.TryAdd((TraceEventType)Enum.Parse(typeof(TraceEventType), eventType), 0);
			}
		}
	}
}