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
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping
{
	public class ProjectMappingManagerMonitor : IDisposable, IVsSolutionEvents
	{
		#region Fields

        private IServiceProvider serviceProvider;
		private IVsSolution solution;
		private bool disposed;
        private FileSystemWatcher watcher;
        private uint solutionEventsCookie;
        private IProjectMappingManager manager;

		#endregion

		#region Constructors

		public ProjectMappingManagerMonitor(IServiceProvider serviceProvider, IProjectMappingManager manager)
		{
			this.serviceProvider = serviceProvider;
            this.manager = manager;
            this.manager.Created += OnMappingFileCreated;
            this.solution = serviceProvider.GetService(typeof(IVsSolution)) as IVsSolution;
			AdviseSolutionEvents();
            SetFileWatcher();
 		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!this.disposed)
			{
				if(disposing)
				{
					ResetFileWatcher();
					UnAdviseSolutionEvents();
                    this.manager.Created -= OnMappingFileCreated;
				}
			}

			disposed = true;
		}

		~ProjectMappingManagerMonitor()
		{
			Dispose(false);
		}

		#endregion

        #region FileSystemWatcher

        private void SetFileWatcher()
		{
            try
            {
                string file = GetMappingFileName();
                if (!string.IsNullOrEmpty(file))
                {
                    watcher = new FileSystemWatcher();
                    watcher.NotifyFilter = NotifyFilters.LastWrite;
                    watcher.Path = Path.GetDirectoryName(file);
                    watcher.Filter = Path.GetFileName(file);
                    watcher.Changed += MappingFileChanged;
                    watcher.EnableRaisingEvents = true;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
		}

        private void MappingFileChanged(object sender, FileSystemEventArgs e)
        {
            this.manager.ReloadMappingFile();
        }

		private void ResetFileWatcher()
		{
            if (watcher != null)
            {
                 watcher.EnableRaisingEvents = false;
            }
		}

		#endregion

		#region IVsSolutionEvents Members

		private void AdviseSolutionEvents()
		{
			solution.AdviseSolutionEvents(this, out solutionEventsCookie);
		}

		private void UnAdviseSolutionEvents()
		{
            try
            {
                solution.UnadviseSolutionEvents(solutionEventsCookie);
                solutionEventsCookie = 0;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
		}

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            this.manager.ReloadMappingFile();
            SetFileWatcher();
            return VSConstants.S_OK;
        }

        #region Default implementation

        public int OnAfterCloseSolution(object pUnkReserved)
		{
            ResetFileWatcher();
            GlobalCache.Reset();
			return VSConstants.S_OK;
		}

		public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
		{
			return VSConstants.S_OK;
		}

		public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
		{
			return VSConstants.S_OK;
		}

		public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
		{
			return VSConstants.S_OK;
		}

		public int OnBeforeCloseSolution(object pUnkReserved)
		{
			return VSConstants.S_OK;
		}

		public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
		{
			return VSConstants.S_OK;
		}

		public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

		public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

		public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
		{
			return VSConstants.S_OK;
		}

        #endregion#

        #endregion

        #region Private Implementation

        // Update FileChangeEvents with the new file
        private void OnMappingFileCreated(object sender, EventArgs e)
        {
            SetFileWatcher();
        }

        private TService GetService<TService, SService>()
		{
			return (TService)serviceProvider.GetService(typeof(SService));
		}

		private string GetMappingFileName()
		{
            using (HierarchyNode item = this.manager.GetMappingFile())
            {
                if (item != null)
                {
                    return item.Path;
                }
                return null;
            }
		}

		#endregion
	}
}