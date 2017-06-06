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
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.Practices.ServiceFactory.Common.Dsl;
using System.ComponentModel.Design;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	internal partial class ServiceContractDslCommandSet
	{
        DslCommandSetUtility<ServiceContractDiagram> commandSetUtility;

        protected override IList<MenuCommand> GetMenuCommands()
        {
            IList<MenuCommand> commands = base.GetMenuCommands();

            commandSetUtility =
                new DslCommandSetUtility<ServiceContractDiagram>(
                    this.ServiceProvider,
                    Properties.Resources.AddNewServiceContractTitle,
                    "Web Service Software Factory 2010 SC Model");

            return commandSetUtility.GetMenuCommands(commands, new Guid(Constants.ServiceContractDslCommandSetId));
        }

        internal override void OnMenuValidate(object sender, EventArgs e)
        {
            commandSetUtility.OnMenuValidate(sender, e, this.CurrentServiceContractDslDocData);
        }

        internal override void OnMenuValidateModel(object sender, EventArgs e)
        {
            commandSetUtility.OnMenuValidateModel<ServiceContractModel>(sender, e, this.CurrentServiceContractDslDocData);
        }
	}
}