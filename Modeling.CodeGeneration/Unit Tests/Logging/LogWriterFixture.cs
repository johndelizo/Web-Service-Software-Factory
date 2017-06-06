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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Microsoft.Practices.Modeling.Common.Logging;

namespace Microsoft.Practices.Modeling.CodeGeneration.Tests.Logging
{
	[TestClass]
	public class LogWriterFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestConstructor()
		{
			LogWriter writer = new LogWriter(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestWriteWithNullParameter()
		{
			List<TraceListener> listeners = new List<TraceListener>();

			listeners.Add(new MockTraceListener());

			LogWriter writer = new LogWriter(listeners);

			writer.Write(null);
		}
	}
}