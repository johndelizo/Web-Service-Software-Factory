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
using Microsoft.VisualStudio.Modeling.Shell;
using System.ComponentModel.Design;
using Microsoft.Practices.VisualStudio.Helper.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Practices.Modeling.Dsl.Integration.Helpers;
using System.IO;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
    public class DslCommandSetUtility<TDiagram> : CommandSet where TDiagram : Diagram
    {
        string addNewDataContractTitle;
        string addNewDataContractTemplateName;

        public DslCommandSetUtility(IServiceProvider provider,
            string addNewDataContractTitle, string addNewDataContractTemplateName)
            : base(provider)
        {
            this.addNewDataContractTitle = addNewDataContractTitle;
            this.addNewDataContractTemplateName = addNewDataContractTemplateName;
        }

        #region Commands Definition

        const int cmdAddNewItem = 0x920;
        const int cmdAutoLayoutId = 0x930;
        const int cmdGenerateCodeId = 0x1000;
        const int cmdGenerateAllCodeId = 0x1001;
        const int cmdViewGeneratedCodeId = 0x1002;

        public IList<MenuCommand> GetMenuCommands(IList<MenuCommand> commands, Guid guidCmdSet)
        {   
            DynamicStatusMenuCommand cmdAddNewModelCommand =
                   new DynamicStatusMenuCommand(
                       new EventHandler(OnStatusChangeInAddNewItem),
                       new EventHandler(OnMenuChangeAddNewItem),
                       new CommandID(guidCmdSet, cmdAddNewItem));
            
            DynamicStatusMenuCommand cmdAutoLayout =
                    new DynamicStatusMenuCommand(
                        new EventHandler(OnStatusChangeInDiagram),
                        new EventHandler(OnMenuChangeAutoLayout),
                        new CommandID(guidCmdSet, cmdAutoLayoutId));

            DynamicStatusMenuCommand cmdGenerateCode =
                    new DynamicStatusMenuCommand(
                        new EventHandler(OnStatusChangeInGenerateCode),
                        new EventHandler(OnMenuChangeGenerateCode),
                        new CommandID(guidCmdSet, cmdGenerateCodeId));

            DynamicStatusMenuCommand cmdGenerateAllCode =
                    new DynamicStatusMenuCommand(
                        new EventHandler(OnStatusChangeInGenerateCode),
                        new EventHandler(OnMenuChangeGenerateAllCode),
                        new CommandID(guidCmdSet, cmdGenerateAllCodeId));

            DynamicStatusMenuCommand cmdViewGeneratedCode =
                     new DynamicStatusMenuCommand(
                         new EventHandler(OnStatusChangeInViewGeneratedCode),
                         new EventHandler(OnMenuChangeViewGeneratedCode),
                         new CommandID(guidCmdSet, cmdViewGeneratedCodeId));

            commands.Add(cmdAddNewModelCommand);
            commands.Add(cmdAutoLayout);
            commands.Add(cmdGenerateCode);
            commands.Add(cmdGenerateAllCode);
            commands.Add(cmdViewGeneratedCode);

            return commands;
        }

        #endregion

        #region Add New Item

        public void OnStatusChangeInAddNewItem(object sender, EventArgs e)
        {
            DynamicStatusMenuCommand command = sender as DynamicStatusMenuCommand;
            command.Visible = command.Enabled = DteLanguage.IsCSharpOrVbProject(this.ServiceProvider, true);
        }

        public void OnMenuChangeAddNewItem(object sender, EventArgs e)
        {
            try
            {
                bool added = VsShellDialogs.AddProjectItemDialog(
                    this.ServiceProvider, this.addNewDataContractTitle, this.addNewDataContractTemplateName, false);
                //Create PMT if not exist
                if(added)
                    ProjectMappingManager.Instance.CreateMappingFile().Dispose();
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        #endregion

        #region AutoLayout

        public void OnMenuChangeAutoLayout(object sender, EventArgs e)
        {
            if (this.FocusedSelection is TDiagram)
            {
                try
                {
                    using (Transaction transaction =
                        this.CurrentDocView.CurrentDiagram.Store.TransactionManager.BeginTransaction("AutoLayout"))
                    {
                        VGRoutingStyle routingStyle = VGRoutingStyle.VGRouteRightAngle;
                        PlacementValueStyle placementStyle = PlacementValueStyle.VGPlaceWE;
                        this.CurrentDocView.CurrentDiagram.AutoLayoutShapeElements(
                            this.CurrentDocView.CurrentDiagram.NestedChildShapes,
                            routingStyle,
                            placementStyle,
                            false);
                        transaction.Commit();
                    }
                }
                catch (Exception error)
                {
                    Logger.Write(error);
                }
            }
        }

        #endregion

        #region GenerateCode and GenerateAllCode

        public void OnStatusChangeInGenerateCode(object sender, EventArgs e)
        {
            DynamicStatusMenuCommand command = sender as DynamicStatusMenuCommand;
            bool activateCommand = false;

            if (!this.IsCurrentDiagramEmpty() && 
                this.IsSingleSelection())
            {
                try
                {
                    activateCommand = ModelCollector.HasValidArtifactsAndRoles(this.ServiceProvider);
                }
                catch (Exception error)
                {
                    Logger.Write(error);
                }
            }

            command.Visible = command.Enabled = activateCommand;
        }

        public void OnMenuChangeGenerateCode(object sender, EventArgs e)
        {
            try
            {
                if (IsValidModel())
                {
                    IArtifactLinkContainer links = ModelCollector.GetArtifacts(this.ServiceProvider);
                    GenerateArtifacts(links);
                }
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        public void OnMenuChangeGenerateAllCode(object sender, EventArgs e)
        {
            try
            {
                if (IsValidModel())
                {
                    GenerateAllArtifacts();
                }
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        private bool IsValidModel()
        {
            OnMenuValidate(null, null, this.CurrentDocData);
            
            IValidationControllerAccesor accesor = this.CurrentDocData as IValidationControllerAccesor;
            
            return 0 == Logger.Messages[TraceEventType.Error] &&
                    accesor != null && 
                    accesor.Controller.ErrorMessages.Count == 0;        
        }

        private void GenerateArtifacts(IArtifactLinkContainer links)
        {            
            if (links != null)
            {
                if (links != null &&
                    links.ArtifactLinks != null)
                {
                    VSStatusBar status = new VSStatusBar(this.ServiceProvider);
                    try
                    {
                        ICodeGenerationService codeGenerationService = this.ServiceProvider.GetService(typeof(ICodeGenerationService)) as ICodeGenerationService;
                        int generatedObjects = codeGenerationService.GenerateArtifacts(links.ArtifactLinks,
                            l => { status.ShowMessage(l.ItemPath); });
                        Logger.Write(
                            string.Format(CultureInfo.CurrentCulture, Properties.Resources.GeneratedObjects, generatedObjects),
                            System.Diagnostics.TraceEventType.Information);
                    }
                    finally
                    {
                        status.Clear();
                    }
                }
            }
        }

        private void GenerateAllArtifacts()
        {
            HashSet<ModelElement> elementList = new HashSet<ModelElement>();
            FullDepthElementWalker elementWalker = new FullDepthElementWalker(
                new ModelElementVisitor(elementList), new EmbeddingReferenceVisitorFilter(), false);

            elementWalker.DoTraverse(DomainModelHelper.GetModelElement(this.FocusedSelection));
            IArtifactLinkContainer links = ModelCollector.GetArtifacts(elementList);
            GenerateArtifacts(links);
            elementList.Clear();
        }

        #endregion

        #region ViewGeneratedCode

        public void OnStatusChangeInViewGeneratedCode(object sender, EventArgs e)
        {
            DynamicStatusMenuCommand command = sender as DynamicStatusMenuCommand;
            command.Visible = command.Enabled = this.IsSingleSelection() && 
                                                !this.IsDiagramSelected() && 
                                                IsValidSelectedContainer();
        }

        public void OnMenuChangeViewGeneratedCode(object sender, EventArgs e)
        {
            IArtifactLinkContainer container = ModelCollector.GetArtifacts(DomainModelHelper.GetSelectedElement(this.ServiceProvider));
            foreach (IArtifactLink link in container.ArtifactLinks)
            {
                string fullPath = GetFullPath(link);
                if (File.Exists(fullPath))
                {
                    EnvDTE.DTE dte = (EnvDTE.DTE)this.ServiceProvider.GetService(typeof(EnvDTE.DTE));
                    if (dte != null)
                    {
                        dte.ItemOperations.OpenFile(fullPath, EnvDTE.Constants.vsViewKindCode);
                    }
                    return;
                }
            }
        }

        private bool IsValidSelectedContainer()
        {
            IArtifactLinkContainer container = ModelCollector.GetArtifacts(DomainModelHelper.GetSelectedElement(this.ServiceProvider));

            bool isValid = (container != null && container.ArtifactLinks.Count > 0);
            
            if(isValid)
            { 
                foreach(IArtifactLink link in container.ArtifactLinks)
                {
                    if (!File.Exists(GetFullPath(link)))
                    {
                        isValid = false;
                        break;
                    }
                }                 
            }

            return isValid;
        }

        private string GetFullPath(IArtifactLink artifactLink)
        {
            IVsSolution vsSolution = this.ServiceProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            using (ProjectNode projectNode = new ProjectNode(vsSolution, artifactLink.Container))
            {
                return Path.Combine(projectNode.ProjectDir, artifactLink.ItemPath.Substring(2));
            }
        }

        #endregion

        #region Common Methods

        public void OnStatusChangeInDiagram(object sender, EventArgs e)
        {
            DynamicStatusMenuCommand command = sender as DynamicStatusMenuCommand;
            command.Visible = command.Enabled = !this.IsCurrentDiagramEmpty() &&
                                                 this.IsDiagramSelected();
        }

        private object FocusedSelection
        {
            get
            {
                foreach (object element in this.CurrentDocView.CurrentDesigner.Selection.FocusedItem.RepresentedElements)
                {
                    return element;
                }
                return this.SingleSelection;
            }
        }

        #endregion

        #region OnMenuValidate overrides
        
        public void OnMenuValidate(object sender, EventArgs e, ModelingDocData docData)
        {
            try
            {
                MenuValidation.ValidateFromElement(docData, this.CurrentSelection, o => { return DomainModelHelper.GetModelElement(o); });
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        public void OnMenuValidateModel<T>(object sender, EventArgs e, ModelingDocData docData) where T : ModelElement
        {
            try
            {
                MenuValidation.ValidateFromModel<T>(docData);
            }
            catch (Exception error)
            {
                Logger.Write(error);
            }
        }

        #endregion
    }
}
