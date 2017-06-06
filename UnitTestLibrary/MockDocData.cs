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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Microsoft.Practices.UnitTestLibrary
{
	public sealed class MockDocData: IVsTextStream
	{
		StringBuilder builder;

		public MockDocData()
		{
			this.builder = new StringBuilder();
		}

		public string Buffer
		{
			get { return builder.ToString(); }
		}

		#region IVsTextStream Members

		public int AdviseTextStreamEvents(IVsTextStreamEvents pSink, out uint pdwCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int CanReplaceStream(int iPos, int iOldLen, int iNewLen)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int CreateEditPoint(int iPosition, out object ppEditPoint)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int CreateStreamMarker(int iMarkerType, int iPos, int iLength, IVsTextMarkerClient pClient, IVsTextStreamMarker[] ppMarker)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int CreateTextPoint(int iPosition, out object ppTextPoint)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int EnumMarkers(int iPos, int iLen, int iMarkerType, uint dwFlags, out IVsEnumStreamMarkers ppEnum)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int FindMarkerByPosition(int iMarkerType, int iStartingPos, uint dwFlags, out IVsTextStreamMarker ppMarker)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetLanguageServiceID(out Guid pguidLangService)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetLastLineIndex(out int piLine, out int piIndex)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetLengthOfLine(int iLine, out int piLength)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetLineCount(out int piLineCount)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetLineIndexOfPosition(int iPosition, out int piLine, out int piColumn)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetPositionOfLine(int iLine, out int piPosition)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetPositionOfLineIndex(int iLine, int iIndex, out int piPosition)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetSize(out int piLength)
		{
			piLength = builder.Length;
			return VSConstants.S_OK;
		}

		public int GetStateFlags(out uint pdwReadOnlyFlags)
		{
			pdwReadOnlyFlags = 0;
			return VSConstants.S_OK;
		}

		public int GetStream(int iPos, int iLength, IntPtr pszDest)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetUndoManager(out Microsoft.VisualStudio.OLE.Interop.IOleUndoManager ppUndoManager)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int InitializeContent(string pszText, int iLength)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int LockBuffer()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int LockBufferEx(uint dwFlags)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reload(int fUndoable)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int ReloadStream(int iPos, int iOldLen, IntPtr pszText, int iNewLen)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int ReplaceStream(int iPos, int iOldLen, IntPtr pszText, int iNewLen)
		{
			string s = Marshal.PtrToStringAuto(pszText);
			builder.Append(s);
			return VSConstants.S_OK;
		}

		public int ReplaceStreamEx(uint dwFlags, int iPos, int iOldLen, IntPtr pszText, int iNewLen, out int piActualLen)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved1()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved10()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved2()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved3()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved4()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved5()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved6()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved7()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved8()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int Reserved9()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int SetLanguageServiceID(ref Guid guidLangService)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int SetStateFlags(uint dwReadOnlyFlags)
		{
			return VSConstants.S_OK;
		}

		public int UnadviseTextStreamEvents(uint dwCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int UnlockBuffer()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int UnlockBufferEx(uint dwFlags)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
