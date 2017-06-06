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
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.ServiceFactory.Commands;
using Microsoft.Practices.ServiceFactory.Common.Dsl;
using Microsoft.Practices.VisualStudio.Helper.Design;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Practices.ServiceFactory
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidServiceFactory_PackagePkgString)]
    public sealed class ServiceFactoryPackage : Package
    {
        public ServiceFactoryPackage()
        {
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            PackageInitializer.Initialize(this);

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the Add New WCF Projects.
                var addWcfCommandID = new CommandID(GuidList.guidServiceFactory_PackageCmdSet, (int)PkgCmdIDList.cmdAddNewWcfProject);
                var addWcfMenuItem = new MenuCommand(AddWcfCallback, addWcfCommandID); // { Enabled = DteLanguage.IsCSharpOrVbProject(this, true) };
                mcs.AddCommand(addWcfMenuItem);

                // Create the command for the Add New ASMX Projects.
                var addAsmxCommandID = new CommandID(GuidList.guidServiceFactory_PackageCmdSet, (int)PkgCmdIDList.cmdAddNewAsmxProject);
                var addAsmxMenuItem = new MenuCommand(AddAsmxCallback, addAsmxCommandID);
                mcs.AddCommand(addAsmxMenuItem);

                // Create the command for the PMT
                var addPmtCommandID = new CommandID(GuidList.guidServiceFactory_PackageCmdSet, (int)PkgCmdIDList.cmdAddPmt);
                var addPmtMenuItem = new MenuCommand(AddPmtCallback, addPmtCommandID);
                mcs.AddCommand(addPmtMenuItem);

                // Create the command for the CA
                var addSemanticCACommandID = new CommandID(GuidList.guidServiceFactory_PackageCmdSet, (int)PkgCmdIDList.cmdSemanticCA);
                var addSemanticCAMenuItem = new MenuCommand(AddSemanticCaCallback, addSemanticCACommandID);
                mcs.AddCommand(addSemanticCAMenuItem);

                // Create the command for the Security CA
                var addSecurityCACommandID = new CommandID(GuidList.guidServiceFactory_PackageCmdSet, (int)PkgCmdIDList.cmdSecurityCA);
                var addSecurityCAMenuItem = new MenuCommand(AddSecurityCaCallback, addSecurityCACommandID);
                mcs.AddCommand(addSecurityCAMenuItem);
            }
        }

        private void AddWcfCallback(object sender, EventArgs e)
        {
            try
            {
                VsShellDialogs.AddProjectDialog(this, 
                    "New WCF Implementation Projects", "WSSF WCF Implementation Projects");
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        private void AddAsmxCallback(object sender, EventArgs e)
        {
            try
            {
                VsShellDialogs.AddProjectDialog(this,
                    "New ASMX Implementation Projects", "WSSF ASMX Implementation Projects");
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        private void AddPmtCallback(object sender, EventArgs e)
        {            
            try
            {
                var operation = new PopulatePmtCommand(this);
                operation.Execute();
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        private void AddSemanticCaCallback(object sender, EventArgs e)
        {
            try
            {
                var operation = new RunSemanticCodeAnalysisRulesCommand(this);
                operation.Execute();
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        private void AddSecurityCaCallback(object sender, EventArgs e)
        {
            try
            {
                var operation = new RunSecurityCodeAnalysisRulesCommand(this);
                operation.Execute();
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }
    }
}
