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
// PkgCmdID.cs
// MUST match PkgCmdID.h
using System;

namespace Microsoft.Practices.ServiceFactory
{
    static class PkgCmdIDList
    {
        public const int cmdAddNewAsmxProject = 0x1;
        public const int cmdAddNewWcfProject = 0x2;
        public const int cmdAddPmt = 0x3;
        public const int cmdSemanticCA = 0x4;
        public const int cmdSecurityCA = 0x5;
    };
}