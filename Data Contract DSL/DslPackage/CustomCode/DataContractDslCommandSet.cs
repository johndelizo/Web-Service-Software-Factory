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
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.VisualStudio.Helper.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Reflection;
using Microsoft.Practices.Modeling.Common.Logging;
using System.Globalization;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;

namespace Microsoft.Practices.ServiceFactory.DataContracts
{
	internal partial class DataContractDslCommandSet
	{
        DslCommandSetUtility<DataContractDiagram> commandSetUtility;
        const int cmdOrderAllDataMembersId = 0x990;

        protected override IList<MenuCommand> GetMenuCommands()
        {
            IList<MenuCommand> commands = base.GetMenuCommands();
            Guid guidCmdSet = new Guid(Constants.DataContractDslCommandSetId);

            DynamicStatusMenuCommand cmdOrderAllDataMembers =
                new DynamicStatusMenuCommand(
                    new EventHandler(OnStatusChangeOrderAllDataMembers),
                    new EventHandler(OnMenuChangeOrderAllDataMembers),
                    new CommandID(guidCmdSet, cmdOrderAllDataMembersId));

            commands.Add(cmdOrderAllDataMembers);

            commandSetUtility =
                new DslCommandSetUtility<DataContractDiagram>(
                    this.ServiceProvider,
                    Properties.Resources.AddNewDataContractTitle,
                    "Web Service Software Factory 2010 DC Model");

            return commandSetUtility.GetMenuCommands(commands, guidCmdSet);
        }

        #region Order All Data Members

        public void OnStatusChangeOrderAllDataMembers(object sender, EventArgs e)
        {
            MenuCommand command = sender as MenuCommand;
            DataContractModel dcModel = this.CurrentDocData.RootElement as DataContractModel;

            command.Visible = command.Enabled = !this.IsCurrentDiagramEmpty() &&
                                                 this.SingleSelection is DataContractDiagram &&
                                                 dcModel != null && 
                                                 dcModel.ImplementationTechnology != null &&
                                                 dcModel.Contracts.Count > 0;
        }

        public void OnMenuChangeOrderAllDataMembers(object sender, EventArgs e)
        {
            try
            {
                Store store = this.CurrentDocView.CurrentDiagram.Store;
                using (Transaction transaction = store.TransactionManager.BeginTransaction("OrderAllDataMembers"))
                {
                    foreach (DataContract data in store.ElementDirectory.FindElements<DataContract>())
                    {
                        int index = 0;
                        foreach (DataMember member in data.DataMembers)
                        {
                            SetOrder(member.ObjectExtender, index++);
                        }
                        OrderedDataMember(data.Name, index);
                        SetOrderParts(data.ObjectExtender);
                    }

                    foreach (FaultContract fault in store.ElementDirectory.FindElements<FaultContract>())
                    {
                        int index = 0;
                        foreach (DataMember member in fault.DataMembers)
                        {
                            SetOrder(member.ObjectExtender, index++);
                        }
                        OrderedDataMember(fault.Name, index);
                        SetOrderParts(fault.ObjectExtender);
                    }

                    transaction.Commit();
                }
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        private void OrderedDataMember(string name, int index)
        {
            Logger.Write(
                 string.Format(CultureInfo.CurrentCulture, Properties.Resources.OrderedDataMembers, name, index),
                 TraceEventType.Information);
        }

        private void SetOrder(object extender, int index)
        {
            if (extender != null)
            {
                // extender will be a WCFDataElement or a ASMXDataElement
                // we reflect the object to avoid coupling with extenders
                PropertyInfo info = extender.GetType().GetProperty("Order");
                if (info != null)
                {
                    info.SetValue(extender, index, null);
                }
            }
        }

        private void SetOrderParts(object extender)
        {
            if (extender != null)
            {
                // this property will be available only for ASMXDataElement
                PropertyInfo info = extender.GetType().GetProperty("OrderParts");
                if (info != null)
                {
                    info.SetValue(extender, true, null);
                }
            }
        }

        #endregion

        internal override void OnMenuValidate(object sender, EventArgs e)
		{
            commandSetUtility.OnMenuValidate(sender, e, this.CurrentDataContractDslDocData);
		}

		internal override void OnMenuValidateModel(object sender, EventArgs e)
		{
            commandSetUtility.OnMenuValidateModel<DataContractModel>(sender, e, this.CurrentDataContractDslDocData);
        }
    }
}
