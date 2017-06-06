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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Microsoft.Practices.Modeling.Common.Logging;

namespace Microsoft.Practices.Modeling.CodeGeneration.Tests.Logging
{
	[TestClass]
	public class TextFormatterFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestFormatWithNullParameter()
		{
			ILogFormatter formatter = new TextFormatter();
			formatter.Format(null);
		}

		[TestMethod]
		public void TestApplyTextFormat()
		{
			LogEntry entry = new CustomLogEntry();

			string actual = FormatEntry("{timestamp}: {title} - {message}", entry);
			string expected = entry.TimestampString + ": " + entry.Title + " - " + entry.Message;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestApplyTextXmlFormat()
		{
			LogEntry entry = new CustomLogEntry();
			entry.Timestamp = DateTime.MaxValue;

			string template = "<Log><message>{message}</message><timestamp>{timestamp}</timestamp><title>{title}</title></Log>";
			string actual = FormatEntry(template, entry);

			string expected = "<Log><message>Foo</message><timestamp>" + DateTime.Parse("12/31/9999 11:59:59 PM", CultureInfo.InvariantCulture).ToString() + "</timestamp><title>FooTitle</title></Log>";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestFormat()
		{
			ILogFormatter formatter = new TextFormatter();
			LogEntry entry = new CustomLogEntry();

			string message = formatter.Format(entry);

			Assert.AreNotEqual(message, string.Empty, "Empty log");
			
			Assert.AreNotEqual(message.IndexOf("Message: Foo"), 0, "Message not present");
			Assert.AreNotEqual(message.IndexOf("EventId: 1"), 0, "EventId not present");
			Assert.AreNotEqual(message.IndexOf("Severity: Error"), 0, "Severity not present");
			Assert.AreNotEqual(message.IndexOf("Title:FooTitle"), 0, "Title not present");

			Assert.AreNotEqual(message.IndexOf(string.Format("Timestamp: {0}", entry.TimestampString)), -1, "Timestamp not present");
			Assert.AreNotEqual(message.IndexOf(string.Format("Message: {0}", entry.Message)), 0, "Message not present");
			Assert.AreNotEqual(message.IndexOf(string.Format("EventId: {0}", entry.EventId)), 0, "EventId not present");
			Assert.AreNotEqual(message.IndexOf(string.Format("Severity: {0}", entry.Severity)), 0, "Severity not present");
			Assert.AreNotEqual(message.IndexOf(string.Format("Title: {0}", entry.Title)), 0, "Title not present");
		}

		[TestMethod]
		public void TimeStampTokenUtcTime()
		{
			LogEntry entry = new CustomLogEntry();
			entry.Timestamp = DateTime.MaxValue;

			ILogFormatter formatter = new TextFormatter("TimeStamp: {timestamp}");
			string actual = formatter.Format(entry);

			string expected = string.Concat("TimeStamp: " + DateTime.MaxValue.ToString());
			Assert.AreEqual(expected, actual);
		}

		private string FormatEntry(string template, LogEntry entry)
		{
			ILogFormatter formatter = new TextFormatter(template);
			return formatter.Format(entry);
		}
	}
}