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
using Microsoft.VisualStudio.Modeling;
using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceFactory.Common.Dsl;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.Practices.VisualStudio.Helper.Design;
using System.Reflection;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio.WCFReference.Interop;
using Microsoft.VisualStudio.Shell;
using System.Globalization;
using System.IO;
using Microsoft.Practices.Modeling.Common.Logging;

namespace Microsoft.Practices.ServiceFactory.HostDesigner
{
    internal partial class HostDesignerCommandSet
	{
        DslCommandSetUtility<HostDesignerDiagram> commandSetUtility;
        const int cmdAddServiceReferenceId = 0x940;

        protected override IList<MenuCommand> GetMenuCommands()
        {
            IList<MenuCommand> commands = base.GetMenuCommands();
            Guid guidCmdSet = new Guid(Constants.HostDesignerCommandSetId);

            DynamicStatusMenuCommand cmdAddServiceReference =
                new DynamicStatusMenuCommand(
                    new EventHandler(OnStatusChangeAddServiceReference),
                    new EventHandler(OnMenuChangeAddServiceReference),
                    new CommandID(guidCmdSet, cmdAddServiceReferenceId));

            commands.Add(cmdAddServiceReference);

            commandSetUtility =
                new DslCommandSetUtility<HostDesignerDiagram>(
                    this.ServiceProvider,
                    Resources.AddNewHostDesignerTitle,
                    "Web Service Software Factory 2010 Host Model");
            return commandSetUtility.GetMenuCommands(commands, guidCmdSet);
        }

        #region Order All Data Members

        public void OnStatusChangeAddServiceReference(object sender, EventArgs e)
        {
            MenuCommand command = sender as MenuCommand;
            command.Visible = command.Enabled = IsEnabled();
        }

        public void OnMenuChangeAddServiceReference(object sender, EventArgs e)
        {
            try
            {
                Store store = this.CurrentDocView.CurrentDiagram.Store;
                Proxy proxy = DomainModelHelper.GetSelectedElement(this.ServiceProvider) as Proxy;

                if (proxy != null &&
                    !string.IsNullOrWhiteSpace(proxy.ClientApplication.ImplementationProject) &&
                    proxy.ClientApplication.ImplementationTechnology != null)
                {
                    IVsSolution solution = this.ServiceProvider.GetService(typeof(SVsSolution)) as IVsSolution;
                    using (HierarchyNode projectNode = new HierarchyNode(solution, proxy.ClientApplication.ImplementationProject))
                    {
                        if (projectNode != null)
                        {
                            IVsAddWebReferenceResult result = VsShellDialogs.AddWebReferenceDialog(this.ServiceProvider, null, projectNode);
                            if (result != null)
                            {
                                result.Save();  // IVsWCFReferenceGroup refGropup = (IVsWCFReferenceGroup)result.Save()                              
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        private bool IsEnabled()
        {
            HostDesignerModel hdModel = this.CurrentDocData.RootElement as HostDesignerModel;
            Proxy proxy = DomainModelHelper.GetSelectedElement(this.ServiceProvider) as Proxy;
            bool enabled =
                proxy != null &&
                !this.IsCurrentDiagramEmpty() &&
                hdModel != null &&
                hdModel.ClientApplications.Count > 0;

            if (enabled)
            {
                if (!string.IsNullOrWhiteSpace(proxy.ClientApplication.ImplementationProject))
                {
                    if (proxy.Endpoint != null &&
                        proxy.Endpoint.ServiceDescription != null &&
                        IsWCF(proxy.ClientApplication.ImplementationTechnology.Name))
                    {
                        //In case this is a WCF service
                        ServiceDescription service = proxy.Endpoint.ServiceDescription;
                        if (service.ObjectExtender != null)
                        {
                            // Reflect EnableMetadataPublishing property to avoid coupling with Extender
                            PropertyInfo property = service.ObjectExtender.GetType().GetProperty("EnableMetadataPublishing");
                            enabled = property != null && Convert.ToBoolean(property.GetValue(service.ObjectExtender, null));
                            if (!enabled) AddEnableMetadataPublishingWarning(proxy.Endpoint.ServiceDescription.Name);
                        }
                    }
                    else if(IsASMX(proxy.ClientApplication.ImplementationTechnology.Name))
                    {
                        // This may be an ASMX service
                        enabled = (proxy.Endpoint != null);
                    }
                }
            }
            return enabled;
        }

        private bool IsASMX(string implementationTechnology)
        {
            return !string.IsNullOrWhiteSpace(implementationTechnology) &&
                implementationTechnology.StartsWith("ASMX", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsWCF(string implementationTechnology)
        {
            return !string.IsNullOrWhiteSpace(implementationTechnology) && 
                implementationTechnology.StartsWith("WCF", StringComparison.OrdinalIgnoreCase);
        }

        private void AddEnableMetadataPublishingWarning(string serviceDescription)
        {
            SimpleErrorListItem listItem = new SimpleErrorListItem(
                string.Format(CultureInfo.CurrentCulture, Resources.EnableMetadataPublishingWarning, serviceDescription),
                this.CurrentDocData.FileName, TaskPriority.Normal, TaskErrorCategory.Warning);
            this.CurrentDocData.AddErrorListItem(listItem);
        }

        #endregion

        internal override void OnMenuValidate(object sender, EventArgs e)
		{
            commandSetUtility.OnMenuValidate(sender, e, this.CurrentHostDesignerDocData);
		}

		internal override void OnMenuValidateModel(object sender, EventArgs e)
		{
            commandSetUtility.OnMenuValidateModel<HostDesignerModel>(sender, e, this.CurrentHostDesignerDocData);
        }
    }
}
