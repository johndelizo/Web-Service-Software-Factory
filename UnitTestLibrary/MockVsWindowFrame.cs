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
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Microsoft.Practices.UnitTestLibrary
{
	public sealed class MockVsWindowFrame : IVsWindowFrame
	{
		#region IVsWindowFrame Members

		public int CloseFrame(uint grfSaveOptions)
		{
			return VSConstants.S_OK;
		}

		public int GetFramePos(VSSETFRAMEPOS[] pdwSFP, out Guid pguidRelativeTo, out int px, out int py, out int pcx, out int pcy)
		{
			pguidRelativeTo = Guid.Empty;
			px = 0;
			py = 0;
			pcx = 0;
			pcy = 0;
			return VSConstants.S_OK;
		}

		public int GetGuidProperty(int propid, out Guid pguid)
		{
			pguid = Guid.Empty;
			return VSConstants.E_NOTIMPL;
		}

		public int GetProperty(int propid, out object pvar)
		{
			pvar = null;
			return VSConstants.E_NOTIMPL;
		}

		public int Hide()
		{
			return VSConstants.S_OK;
		}

		public int IsOnScreen(out int pfOnScreen)
		{
			pfOnScreen = 1;
			return VSConstants.S_OK;
		}

		public int IsVisible()
		{
			return VSConstants.S_FALSE;
		}

		public int QueryViewInterface(ref Guid riid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			return VSConstants.E_NOTIMPL;
		}

		public int SetFramePos(VSSETFRAMEPOS dwSFP, ref Guid rguidRelativeTo, int x, int y, int cx, int cy)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int SetGuidProperty(int propid, ref Guid rguid)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int SetProperty(int propid, object var)
		{
			return VSConstants.E_NOTIMPL;
		}

		public int Show()
		{
			return VSConstants.S_OK;
		}

		public int ShowNoActivate()
		{
			return VSConstants.S_OK;
		}

		#endregion
	}
}
