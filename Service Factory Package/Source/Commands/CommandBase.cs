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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using EnvDTE;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.Practices.ServiceFactory.Common.Dsl;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common.Services;
using System.ComponentModel;

namespace Microsoft.Practices.ServiceFactory.Commands
{
	public abstract class CommandBase
	{
		private DTE dte;
        private IServiceProvider provider;

        protected CommandBase(IServiceProvider provider)
        {
            this.provider = provider;
            this.dte = this.GetService<DTE>();
        }

		/// <summary>
		/// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public void Execute()
		{
			try
			{
				Logger.Clear();
				OnExecute();
			}
			catch (Exception e)
			{
				HandleException(e);
			}
			finally
			{
				RestoreStatusBar();
			}
		}

		/// <summary>
		/// Called when [execute].
		/// </summary>
		protected virtual void OnExecute()
		{
		}

		protected virtual void HandleException(Exception e)
		{
			LogException(e);
		}

		/// <summary>
		/// Called when [undo].
		/// </summary>
		protected virtual void OnUndo()
		{
		}

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns></returns>
        protected TInterface GetService<TInterface>()
        {
            return (TInterface)this.provider.GetService(typeof(TInterface));
        }

		/// <summary>
		/// Gets the service.
		/// </summary>
		/// <returns></returns>
		protected TInterface GetService<TInterface, TImpl>()
		{
			return (TInterface)this.provider.GetService(typeof(TImpl));
		}

		/// <summary>
		/// Gets the DTE.
		/// </summary>
		/// <value>The DTE.</value>
		protected DTE Dte
		{
			get { return this.dte; }
		}

        protected IServiceProvider Provider
        {
            get { return this.provider; }
        }

		protected void ClearLogs()
		{
			Logger.Clear();
		}

		protected void LogException(Exception e)
		{
			ClearLogs();
			LogEntry entry = new LogEntry(
				e.Message,
				Resources.LogErrorTitle,
				TraceEventType.Error,
				1000);

			Logger.Write(entry);
		}

	    /// <summary>
		/// Shows the message in output window.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		protected void Trace(string message, params object[] args)
		{
			Trace(message, TraceEventType.Information, args);
		}

		/// <summary>
		/// Shows the message in output window.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		protected void Trace(string message, TraceEventType logSeverity, params object[] args)
		{
			LogEntry entry = new LogEntry(
				args.Length == 0 ? message : string.Format(CultureInfo.CurrentCulture, message, args),
				Resources.LogInformationTitle,
				logSeverity,
				1000);

			Logger.Write(entry);
		}

		/// <summary>
		/// Show a progress bar in the VS status bar.
		/// </summary>
		/// <param name="label">The label to show in the status bar.</param>
		/// <param name="amountCompleted">The amount completed.</param>
		/// <param name="total">The total.</param>
		protected void Progress(string label, int amountCompleted, int total)
		{
			this.dte.StatusBar.Progress(true, label, amountCompleted, total);
		}

		/// <summary>
		/// Updates the status.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="args">The args.</param>
		protected void UpdateStatus(string text, params object[] args)
		{
			if (args != null)
			{
				this.dte.StatusBar.Text = String.Format(CultureInfo.CurrentCulture, text, args);
			}
			else
			{
				this.dte.StatusBar.Text = text;
			}
		}

		private void RestoreStatusBar()
		{
			this.dte.StatusBar.Progress(false, string.Empty, 0, 0);
			UpdateStatus(Resources.IdleStatusBarMessage);
		}
	}
}