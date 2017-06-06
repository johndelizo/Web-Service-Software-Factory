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
// Guids.cs
// MUST match guids.h
using System;

namespace Microsoft.Practices.ServiceFactory
{
    static class GuidList
    {
        public const string guidServiceFactory_PackagePkgString = "19bb3318-cdf8-44bc-b02b-5788104f8cf8";
        public const string guidServiceFactory_PackageCmdSetString = "8ef5eefd-70fa-4400-810f-219eb8ed968f";

        public static readonly Guid guidServiceFactory_PackageCmdSet = new Guid(guidServiceFactory_PackageCmdSetString);
    };
}