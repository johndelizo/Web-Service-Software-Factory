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
using System.Text;
using Microsoft.Practices.Modeling.Common.Properties;

namespace Microsoft.Practices.Modeling.Common.Logging
{
	/// <summary>
	/// The current logging infraestructure
	/// </summary>
	public class TextFormatter : ILogFormatter
	{
		private string template;
		private const string timeStampToken = "{timestamp}";
		private const string messageToken = "{message}";
		private const string categoryToken = "{category}";
		private const string eventIdToken = "{eventId}";
		private const string severityToken = "{severity}";
		private const string titleToken = "{title}";
		private const string NewLineToken = "{newLine}";
		private const string TabToken = "{tab}";

		public TextFormatter(string template)
		{
            if (!string.IsNullOrEmpty(template))
            {
                this.template = template;
            }
            else
            {
                this.template = Resources.DefaultTextFormat;
            }
		}

		public TextFormatter()
			: this(Resources.DefaultTextFormat)
		{
		}

		public string Template
		{
			get { return template; }
			set { template = value; }
		}

		public string Format(LogEntry log)
		{
			Guard.ArgumentNotNull(log, "log");
			return Format(CreateTemplateBuilder(), log);
		}

		protected virtual string Format(StringBuilder templateBuilder, LogEntry log)
		{
			templateBuilder.Replace(timeStampToken, log.TimestampString);
			templateBuilder.Replace(titleToken, log.Title);
			templateBuilder.Replace(messageToken, log.Message);
			templateBuilder.Replace(eventIdToken, log.EventId.ToString(Resources.Culture));
			templateBuilder.Replace(severityToken, log.Severity.ToString());
			templateBuilder.Replace(NewLineToken, Environment.NewLine);
			templateBuilder.Replace(TabToken, "\t");

			return templateBuilder.ToString();
		}

		protected StringBuilder CreateTemplateBuilder()
		{
			StringBuilder templateBuilder =
							new StringBuilder((this.template == null) || (this.template.Length > 0) ? this.template : Resources.DefaultTextFormat);
			return templateBuilder;
		}
	}
}