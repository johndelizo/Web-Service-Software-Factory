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
using EnvDTE;
using System.IO;
using System.ComponentModel.Design;
using System.Globalization;
using System.Diagnostics;

namespace Microsoft.Practices.Modeling.Common
{
	/// <summary>
	/// Helper methods for Team Fundation System operations.
	/// </summary>
	public static class TFSHelper
	{
        /// <summary>
		/// Checks the out file.
		/// </summary>
        /// <param name="serviceProvider">The service provider instance.</param>
		/// <param name="filePath">The file path.</param>
        public static void CheckOutFile(IServiceProvider serviceProvider, string filePath)
        {
            Guard.ArgumentNotNull(serviceProvider, "serviceProvider");
            CheckOutFile((EnvDTE.DTE)serviceProvider.GetService(typeof(EnvDTE.DTE)), filePath);
        }

		/// <summary>
		/// Checks the out file.
		/// </summary>
		/// <param name="vs">The vs.</param>
		/// <param name="filePath">The file path.</param>
		public static void CheckOutFile(_DTE vs, string filePath)
		{
            Guard.ArgumentNotNullOrWhiteSpaceString(filePath, "filePath");

            //Do nothing if no DTE instance
            if (vs == null)
            {
                Trace.TraceWarning("TFSHelper.CheckOutFile: No DTE instance available to access TFS functions.");
                return;
            }

			if (File.Exists(filePath))
			{
				if (vs.SourceControl.IsItemUnderSCC(filePath) &&
					!vs.SourceControl.IsItemCheckedOut(filePath))
				{
					bool checkout = vs.SourceControl.CheckOutItem(filePath);
					if (!checkout)
					{
						throw new CheckoutException(
							string.Format(CultureInfo.CurrentCulture, Properties.Resources.CheckoutException, filePath));
					}
				}
				else
				{
					// perform an extra check if the file is read only.
					if (IsReadOnly(filePath))
					{
						ResetReadOnly(filePath);
					}
				}
			}
		}

		private static bool IsReadOnly(string path)
		{
			return (File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
		}

		private static void ResetReadOnly(string path)
		{
			File.SetAttributes(path, File.GetAttributes(path) ^ FileAttributes.ReadOnly);
		}
	}
}
