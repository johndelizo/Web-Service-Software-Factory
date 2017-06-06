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
using System.Runtime.Caching;
using Microsoft.Practices.Modeling.Common.Logging;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Specialized;
using Microsoft.VisualStudio.Modeling.Shell;
using EnvDTE;
using System.Collections.Generic;

namespace Microsoft.Practices.Modeling.Common
{
    public static class GlobalCache
    {
        private const string CacheName = "WSSF";
        private static MemoryCache cache = new MemoryCache(CacheName, InitCache()); // Update mem statistics every 5 secs

        public static T AddOrGetExisting<T>(string key, Func<string, T> value) where T : class
        {
            if (cache == null)
                return value(key);

            T itemValue = (T)cache.Get(key);
            if (itemValue == default(T))
            {
                itemValue = value(key);
                ModelChangeMonitor changeMonitor = CreateModelChangeMonitor();
                CacheItemPolicy itemPolicy = new CacheItemPolicy() { RemovedCallback = new CacheEntryRemovedCallback(OnRemovedItem) };
                if(changeMonitor != null) itemPolicy.ChangeMonitors.Add(changeMonitor);
                cache.Set(key, itemValue, itemPolicy);
            }
            return itemValue;
        }

        public static void Reset()
        {
            if(cache != null) cache.Dispose();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized);            
            cache = new MemoryCache(CacheName, InitCache());
        }

        public static void Off()
        {
            cache = null;
        }

        private static void OnRemovedItem(CacheEntryRemovedArguments arguments)
        {
            if (arguments.RemovedReason == CacheEntryRemovedReason.Evicted)
            {
                var entry = new LogEntry(string.Format(CultureInfo.CurrentCulture, "Cache item evicted: {0}", arguments.CacheItem.Key), TraceEventType.Verbose);
                Logger.Write<VSOutputWindowListener>(entry);
            }
        }

        private static ModelChangeMonitor CreateModelChangeMonitor()
        {
            IMonitorSelectionService monitorSelectionService = RuntimeHelper.ServiceProvider.GetService(typeof(IMonitorSelectionService)) as IMonitorSelectionService;
            if (monitorSelectionService != null) // may be null on tests
            {
                DocData docData = monitorSelectionService.CurrentDocument as DocData;
                if (docData != null)
                {
                    return new ModelChangeMonitor(docData);
                }
            }
            return null;
        }

        private static NameValueCollection InitCache()
        {
            return new NameValueCollection() { { "PollingInterval", "00:00:10" } }; // poll every 10 secs
        }
    }   
}
