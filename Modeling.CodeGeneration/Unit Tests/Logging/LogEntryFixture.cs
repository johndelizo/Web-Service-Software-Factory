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
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.Modeling.Common.Logging;

namespace Microsoft.Practices.Modeling.CodeGeneration.Tests.Logging
{
	[TestClass]
	public class LogEntryFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestConstructorWithNullParameter()
		{
			LogEntry entry = new LogEntry(null, string.Empty, TraceEventType.Error, 1);
		}

		[TestMethod]
		public void TestCreation()
		{
			LogEntry entry = new CustomLogEntry();

			Assert.AreEqual(entry.EventId, 1, "Not equal");
			Assert.AreEqual(entry.Message, "Foo", "Not equal");
			Assert.AreEqual(entry.Severity, TraceEventType.Error, "Not equal");
			Assert.AreEqual(entry.Title, "FooTitle", "Not equal");
			Assert.AreEqual(entry.TimestampString, entry.Timestamp.ToString(CultureInfo.CurrentCulture), "Not equal");
		}

		[TestMethod]
		public void TestGetSetProperties()
		{
			LogEntry entry = new CustomLogEntry();

			string stringVal = "my test string";
			int counter = 0;

			entry.EventId = counter;
			Assert.AreEqual(counter, entry.EventId);
			entry.EventId = -1234;
			Assert.AreEqual(-1234, entry.EventId);

			entry.Message = stringVal + counter;
			Assert.AreEqual(stringVal + counter, entry.Message);
			counter++;
			entry.Message = "";
			Assert.AreEqual("", entry.Message);

			entry.Severity = TraceEventType.Warning;
			Assert.AreEqual(TraceEventType.Warning, entry.Severity);
			counter++;
			entry.Severity = TraceEventType.Information;
			Assert.AreEqual(TraceEventType.Information, entry.Severity);

			entry.Timestamp = DateTime.MinValue;
			Assert.AreEqual(DateTime.MinValue, entry.Timestamp);
			counter++;
			entry.Timestamp = DateTime.MaxValue;
			Assert.AreEqual(DateTime.MaxValue, entry.Timestamp);

			entry.Title = stringVal + counter;
			Assert.AreEqual(stringVal + counter, entry.Title);
			counter++;
			entry.Title = "";
			Assert.AreEqual("", entry.Title);
		}

		[TestMethod]
		public void ConfirmSeverityValuesCanBeReadAsStrings()
		{
			LogEntry entry = new CustomLogEntry();

			Assert.AreEqual("Error", entry.LoggedSeverity);
		}

		[TestMethod]
		public void GetSetTimeStampString()
		{
			LogEntry entry = new CustomLogEntry();
			string expected = DateTime.Parse("12/31/9999 11:59:59 PM", CultureInfo.InvariantCulture).ToString();
			entry.Timestamp = DateTime.MaxValue;

			Assert.AreEqual(expected, entry.TimestampString);
		}
	}
}