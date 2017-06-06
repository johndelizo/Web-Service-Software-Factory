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
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Microsoft.Practices.UnitTestLibrary
{
	public sealed class MockVsTextManager: IVsTextManager
	{
		#region IVsTextManager Members

		public int AdjustFileChangeIgnoreCount(IVsTextBuffer pBuffer, int fIgnore)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int AttemptToCheckOutBufferFromScc(IVsUserData pBufData, out int pfCheckoutSucceeded)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int AttemptToCheckOutBufferFromScc2(string pszFileName, out int pfCheckoutSucceeded, out int piStatusFlags)
		{
			pfCheckoutSucceeded = 1;
			piStatusFlags = 0;
			return VSConstants.S_OK;
		}

		public int CreateSelectionAction(IVsTextBuffer pBuffer, out IVsTextSelectionAction ppAction)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int EnumBuffers(out IVsEnumTextBuffers ppEnum)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int EnumIndependentViews(IVsTextBuffer pBuffer, out IVsEnumIndependentViews ppEnum)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int EnumLanguageServices(out IVsEnumGUID ppEnum)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int EnumViews(IVsTextBuffer pBuffer, out IVsEnumTextViews ppEnum)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetActiveView(int fMustHaveFocus, IVsTextBuffer pBuffer, out IVsTextView ppView)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetBufferSccStatus(IVsUserData pBufData, out int pbNonEditable)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetBufferSccStatus2(string pszFileName, out int pbNonEditable, out int piStatusFlags)
		{
			pbNonEditable = 0;
			piStatusFlags = 0;
			return VSConstants.S_OK;
		}

		public int GetMarkerTypeCount(out int piMarkerTypeCount)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetMarkerTypeInterface(int iMarkerTypeID, out IVsTextMarkerType ppMarkerType)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetPerLanguagePreferences(LANGPREFERENCES[] pLangPrefs)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetRegisteredMarkerTypeID(ref Guid pguidMarker, out int piMarkerTypeID)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetShortcutManager(out IVsShortcutManager ppShortcutMgr)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetUserPreferences(VIEWPREFERENCES[] pViewPrefs, FRAMEPREFERENCES[] pFramePrefs, LANGPREFERENCES[] pLangPrefs, FONTCOLORPREFERENCES[] pColorPrefs)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int IgnoreNextFileChange(IVsTextBuffer pBuffer)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int MapFilenameToLanguageSID(string pszFileName, out Guid pguidLangSID)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int NavigateToLineAndColumn(IVsTextBuffer pBuffer, ref Guid guidDocViewType, int iStartRow, int iStartIndex, int iEndRow, int iEndIndex)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int NavigateToPosition(IVsTextBuffer pBuffer, ref Guid guidDocViewType, int iPos, int iLen)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int RegisterBuffer(IVsTextBuffer pBuffer)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int RegisterIndependentView(object pUnk, IVsTextBuffer pBuffer)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int RegisterView(IVsTextView pView, IVsTextBuffer pBuffer)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int SetFileChangeAdvise(string pszFileName, int fStart)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int SetPerLanguagePreferences(LANGPREFERENCES[] pLangPrefs)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int SetUserPreferences(VIEWPREFERENCES[] pViewPrefs, FRAMEPREFERENCES[] pFramePrefs, LANGPREFERENCES[] pLangPrefs, FONTCOLORPREFERENCES[] pColorPrefs)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int SuspendFileChangeAdvise(string pszFileName, int fSuspend)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int UnregisterBuffer(IVsTextBuffer pBuffer)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int UnregisterIndependentView(object pUnk, IVsTextBuffer pBuffer)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int UnregisterView(IVsTextView pView)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
