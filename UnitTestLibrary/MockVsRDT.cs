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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Microsoft.Practices.UnitTestLibrary
{
	public sealed class MockVsRDT : IVsRunningDocumentTable
	{
		public class RDTEntry
		{
			public IVsHierarchy hierarchy;
			public uint itemid;
			public MockDocData docData;
			public RDTEntry(IVsHierarchy hier,uint id)
			{
				this.hierarchy = hier;
				this.itemid = id;
				docData = new MockDocData();
			}
		}

		Dictionary<string, RDTEntry> docs;

		public RDTEntry GetEntry(string document)
		{
			string fullItemPath = Path.Combine(Directory.GetCurrentDirectory(), document);
			return docs[fullItemPath];
		}

		public MockVsRDT(string file, IVsHierarchy hierarchy, uint itemid)
		{
			docs = new Dictionary<string, RDTEntry>();
			docs.Add(file, new RDTEntry(hierarchy, itemid));
		}

		#region IVsRunningDocumentTable Members

		public int AdviseRunningDocTableEvents(IVsRunningDocTableEvents pSink, out uint pdwCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int FindAndLockDocument(uint dwRDTLockType, string pszMkDocument, out IVsHierarchy ppHier, out uint pitemid, out IntPtr ppunkDocData, out uint pdwCookie)
		{
			ppHier = null;
			pitemid = VSConstants.VSITEMID_NIL;
			ppunkDocData = IntPtr.Zero;
			pdwCookie = 0;
			if (docs.ContainsKey(pszMkDocument))
			{
				RDTEntry entry = docs[pszMkDocument];
				ppHier = entry.hierarchy;
				pitemid = entry.itemid;
				pdwCookie = entry.itemid;
				ppunkDocData = Marshal.GetIUnknownForObject(entry.docData);
			}
			return VSConstants.S_OK;
		}

		public int GetDocumentInfo(uint docCookie, out uint pgrfRDTFlags, out uint pdwReadLocks, out uint pdwEditLocks, out string pbstrMkDocument, out IVsHierarchy ppHier, out uint pitemid, out IntPtr ppunkDocData)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int GetRunningDocumentsEnum(out IEnumRunningDocuments ppenum)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int LockDocument(uint grfRDTLockType, uint dwCookie)
		{
			dwCookie=0;
			return VSConstants.S_OK;
		}

		public int ModifyDocumentFlags(uint docCookie, uint grfFlags, int fSet)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int NotifyDocumentChanged(uint dwCookie, uint grfDocChanged)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int NotifyOnAfterSave(uint dwCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int NotifyOnBeforeSave(uint dwCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int RegisterAndLockDocument(uint grfRDTLockType, string pszMkDocument, IVsHierarchy pHier, uint itemid, IntPtr punkDocData, out uint pdwCookie)
		{
			pdwCookie = 0;
			if (docs.ContainsKey(pszMkDocument))
			{
				pdwCookie = docs[pszMkDocument].itemid;
			}
			return VSConstants.S_OK;
		}

		public int RegisterDocumentLockHolder(uint grfRDLH, uint dwCookie, IVsDocumentLockHolder pLockHolder, out uint pdwLHCookie)
		{
			pdwLHCookie = 0;
			return VSConstants.S_OK;
		}

		public int RenameDocument(string pszMkDocumentOld, string pszMkDocumentNew, IntPtr pHier, uint itemidNew)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int SaveDocuments(uint grfSaveOpts, IVsHierarchy pHier, uint itemid, uint docCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int UnadviseRunningDocTableEvents(uint dwCookie)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int UnlockDocument(uint grfRDTLockType, uint dwCookie)
		{
			return VSConstants.S_OK;
		}

		public int UnregisterDocumentLockHolder(uint dwLHCookie)
		{
			return VSConstants.S_OK;
		}

		#endregion
	}
}
