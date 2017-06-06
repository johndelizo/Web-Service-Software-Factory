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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Microsoft.Practices.Modeling.Common.Logging
{
	/// <summary>
	/// The current logging infraestructure
	/// </summary>
	[XmlRoot("LogEntry")]
	[Serializable]
	public class LogEntry
	{
		public const string DefaultTitle = "";
		public const TraceEventType DefaultSeverity = TraceEventType.Error;
		public const int DefaultEventId = 1;

		private static readonly TextFormatter toStringFormatter = new TextFormatter();
		private string title;
		private string message;
		private TraceEventType severity;
		private int eventId;
		private DateTime timestamp;

		public LogEntry(object logMessage)
			: this(logMessage, DefaultTitle, DefaultSeverity, DefaultEventId)
		{
		}

		public LogEntry(object logMessage, TraceEventType logSeverity)
			: this(logMessage, DefaultTitle, logSeverity, DefaultEventId)
		{
		}

		public LogEntry(object logMessage, string logTitle, TraceEventType logSeverity, int eventIdentifier)
		{
			Guard.ArgumentNotNull(logMessage, "logMessage");
            Exception msgException = logMessage as Exception;
            this.message = msgException != null ? ErrorMessageToString(msgException) : logMessage.ToString();
			this.title = logTitle;
			this.severity = logSeverity;
			this.eventId = eventIdentifier;

			InitializeLogEntry();
		}

		public static string ErrorMessageToString(Exception exception)
		{
			Guard.ArgumentNotNull(exception, "exception");
			StringBuilder builder = new StringBuilder();
			while (exception != null)
			{
                ReflectionTypeLoadException typeLoadException = exception as ReflectionTypeLoadException;
                FileNotFoundException fnf = exception as FileNotFoundException;

				builder.AppendLine(typeLoadException != null ?
                    GetReflectionTypeLoadExceptionMessage(typeLoadException) :
                    fnf != null ? fnf.Message+fnf.FileName : exception.Message);
				exception = exception.InnerException;
			}
			return builder.ToString();
		}

		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		public string Message
		{
			get { return this.message; }
			set { this.message = value; }
		}

		public TraceEventType Severity
		{
			get { return this.severity; }
			set { this.severity = value; }
		}

		public string LoggedSeverity
		{
			get { return severity.ToString(); }
		}

		public int EventId
		{
			get { return this.eventId; }
			set { this.eventId = value; }
		}

		public DateTime Timestamp
		{
			get { return this.timestamp; }
			set { this.timestamp = value; }
		}

		public string TimestampString
		{
			get { return Timestamp.ToString(CultureInfo.CurrentCulture); }
		}

		public override string ToString()
		{
			return toStringFormatter.Format(this);
		}

		private void InitializeLogEntry()
		{
			this.Timestamp = DateTime.UtcNow;
		}

        private static string GetReflectionTypeLoadExceptionMessage(ReflectionTypeLoadException exception)
        {
            StringBuilder builder = new StringBuilder();
            // lookup table for duplicate messages
            List<string> messages = new List<string>(); 

            builder.AppendLine(exception.Message);
            builder.AppendLine(Properties.Resources.LoaderExceptionMessages);
            foreach (Exception error in exception.LoaderExceptions)
            {
                if (!messages.Contains(error.Message))
                {
                    builder.AppendLine(error.Message);
                    messages.Add(error.Message);
                }
            }

            builder.AppendLine(Properties.Resources.LoaderExceptionTypeMessages);
            foreach (Type type in exception.Types)
            {
                if (type != null && 
                    !messages.Contains(type.FullName))
                {
                    builder.AppendLine(type.FullName);
                    messages.Add(type.FullName);
                }
            }

            return builder.ToString();
        }
	}
}