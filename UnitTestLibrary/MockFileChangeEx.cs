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
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;

namespace Microsoft.Practices.UnitTestLibrary
{
	public class MockFileChangeEx : IVsFileChangeEx
	{
		#region IVsFileChangeEx Members

		public int AdviseDirChange(string pszDir, int fWatchSubDir, IVsFileChangeEvents pFCE, out uint pvsCookie)
		{
			pvsCookie = 0;
			return VSConstants.S_OK;
		}

		public int AdviseFileChange(string pszMkDocument, uint grfFilter, IVsFileChangeEvents pFCE, out uint pvsCookie)
		{
			pvsCookie = 0;
			return VSConstants.S_OK;
		}

		public int IgnoreFile(uint VSCOOKIE, string pszMkDocument, int fIgnore)
		{
			return VSConstants.S_OK;
		}

		public int SyncFile(string pszMkDocument)
		{
			return VSConstants.S_OK;
		}

		public int UnadviseDirChange(uint VSCOOKIE)
		{
			return VSConstants.S_OK;
		}

		public int UnadviseFileChange(uint VSCOOKIE)
		{
			return VSConstants.S_OK;
		}

		#endregion
	}
}
