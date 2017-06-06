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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Globalization;
using System.Diagnostics;

namespace Microsoft.Practices.Modeling.Common
{
    public static class DteLocator
    {
        #region Unmanaged code declarations

        const uint S_OK = 0;

        [DllImport("ole32.dll")]
        static extern uint GetRunningObjectTable(uint reserved, out IRunningObjectTable ROT);

        [DllImport("ole32.dll")]
        static extern uint CreateBindCtx(uint reserved, out IBindCtx ctx);

        #endregion#

        public static EnvDTE._DTE GetCurrentInstance()
        {
            // This won't work with service prov.
            //return Marshal.GetActiveObject("VisualStudio.DTE.11.0") as EnvDTE._DTE;
            return GetDteFromRot(System.Diagnostics.Process.GetCurrentProcess().Id) as EnvDTE._DTE;
        }

        private static object GetDteFromRot(int processId)
        {
            IRunningObjectTable runningObjectTable;
            IEnumMoniker monikerEnumerator;

            string expectedMonikerEnd = String.Format(CultureInfo.InvariantCulture, "DTE.11.0:{0}", processId);

            try
            {
                uint hResult = GetRunningObjectTable(0, out runningObjectTable);
                if (hResult != 0)
                {
                    return null;
                }

                runningObjectTable.EnumRunning(out monikerEnumerator);
                monikerEnumerator.Reset();

                IntPtr numberFetched = IntPtr.Zero;
                IMoniker[] monikers = new IMoniker[1];
                while (monikerEnumerator.Next(1, monikers, numberFetched) == 0)
                {
                    IBindCtx ctx;
                    hResult = CreateBindCtx(0, out ctx);
                    if (hResult != 0)
                    {
                        // release the bind ctx obj
                        Marshal.ReleaseComObject(ctx);
                        continue;
                    }

                    string runningObjectName;
                    monikers[0].GetDisplayName(ctx, null, out runningObjectName);
                    // release the bind ctx obj
                    Marshal.ReleaseComObject(ctx);

                    if (runningObjectName.EndsWith(expectedMonikerEnd))
                    {
                        object runningObjectValue;
                        int hRes = runningObjectTable.GetObject(monikers[0], out runningObjectValue);
                        return hRes != 0 ? null : runningObjectValue as EnvDTE._DTE; 
                    }
                }
            }
            catch(Exception e)
            {
                Trace.TraceError(e.ToString());
            }
            return null;
        } 
    }
} 
