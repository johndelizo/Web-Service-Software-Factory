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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.Modeling.Common.Logging;
using Vs = VSLangProj;
using Web = VsWebSite;

namespace Microsoft.Practices.ServiceFactory.Commands
{
    /// <summary>
    /// Run the Code Analysis rules in the selected project and all its references.
    /// </summary>
    public abstract class CodeAnalysisRulesCommand : CommandBase
    {
        public const string RunCodeAnalysisonWebSite = "Build.RunCodeAnalysisonWebSite";
        public const string RunCodeAnalysisonSelection = "ClassViewContextMenus.ClassViewProject.RunCodeAnalysis";

        private Project project;
        private EnvDTE.BuildEvents buildEvents;
        private string webTempFile;
        private StringDictionary runCodeAnalysisValues;

        public CodeAnalysisRulesCommand(IServiceProvider provider)
            : base(provider)
        {
            this.project = VsHelper.ToDteProject(DteHelper2.GetCurrentSelection(provider));
        }
        
        protected abstract string RuleCheckIdPrefix {get;}
        protected abstract string RulesetFileName { get; }

		protected override void OnExecute()
		{
            string rulesPath = RuntimeHelper.GetExecutionPath(RulesetFileName);
			runCodeAnalysisValues = new StringDictionary();

			// set the rules on each reference
			SetCodeAnalysisOnReferences(rulesPath);

			if (DteHelper2.IsWebProject(this.project))
			{
				PrepareConfigFile();
				SetCodeAnalysisOnWebProject(this.project, rulesPath);
				SetUpBuildEventHandling();
				this.project.DTE.ExecuteCommand(RunCodeAnalysisonWebSite, "");
			}
			else
			{
				SetCodeAnalysisOnProject(this.project, rulesPath);
				SetUpBuildEventHandling();
				this.project.DTE.ExecuteCommand(RunCodeAnalysisonSelection, "");
			}
		}

		#region Private methods

		private void SetUpBuildEventHandling()
        {
            // we need to know when the process ends so we may 
            // rollback all property changes. 
            // We will monitor the build event so we can rollback all changes after the buid process ends
            // Notice that the code analysis is done through the build process.
            buildEvents = (EnvDTE.BuildEvents)this.project.DTE.Events.BuildEvents;
            buildEvents.OnBuildDone += OnBuildDone;
        }

        private void OnBuildDone(EnvDTE.vsBuildScope Scope, EnvDTE.vsBuildAction Action)
        {
            // detach from the event
            buildEvents.OnBuildDone -= OnBuildDone;

            // rollback the properties changes
            Rollback();

            EnvDTE80.ErrorList errorList = ((EnvDTE80.DTE2)this.project.DTE).ToolWindows.ErrorList;
            // ShowResult if there is any WCF rule violation
            int ruleWarnings = RuleWarningsCount(errorList);
            if (ruleWarnings == 0)
            {
				Logger.Write(
					Resources.CodeAnalysisSuccess, string.Empty, TraceEventType.Information, 1);
            }
            else
            {
				Logger.Write(
					string.Format(CultureInfo.CurrentCulture, Resources.CodeAnalysisWarnings, ruleWarnings));
                // We may force the Show Warnings button to be ON so the user will always see any 
                // warning message from the result of the code analysis run.
                errorList.ShowWarnings = true;
            }
            this.project = null;
        }

        private int RuleWarningsCount(EnvDTE80.ErrorList errorList)
        {
            int warningsCount = 0;
            for (int index = 1; index <= errorList.ErrorItems.Count; index++)
            {
                // check only on warnings
                if (errorList.ErrorItems.Item(index).ErrorLevel ==
                    EnvDTE80.vsBuildErrorLevel.vsBuildErrorLevelLow)
                {
                    if (errorList.ErrorItems.Item(index).Description.StartsWith(
                        RuleCheckIdPrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        warningsCount++;
                    }
                }
            }
            return warningsCount;
        }

        private void PrepareConfigFile()
        {
            // add a temporal file on App_Code folder in case there's no code so we force precompilation and 
            // get code analysis for the web.config file
            // Note: this file may be removed after the code analysis process is done.
            Web.VSWebSite webProject = this.project.Object as Web.VSWebSite;
            if (webProject.CodeFolders.Count == 0)
            {
                // add App_Code folder
                webProject.CodeFolders.Add("App_Code\\");
            }
            Web.CodeFolder codeFolder = (Web.CodeFolder)webProject.CodeFolders.Item(1);
            // check if we need to add a temp file
            foreach (ProjectItem item in codeFolder.ProjectItem.ProjectItems)
            {
                string ext = Path.GetExtension(item.Name);
                if (!string.IsNullOrEmpty(ext) &&
                    !ext.Equals(".exclude", StringComparison.OrdinalIgnoreCase))
                {
                    // do not add the temp file since we already have some code file
                    return;
                }
            }

            this.webTempFile = Path.Combine(webProject.Project.FileName,
                codeFolder.FolderPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
                + Path.ChangeExtension(Path.GetRandomFileName(), DteHelperEx.GetDefaultExtension(this.project)));

            if (!File.Exists(this.webTempFile))
            {
                File.WriteAllText(this.webTempFile, string.Empty);
            }
        }

        private void SetCodeAnalysisOnProject(Project project, string rulesPath)
        {
            string runCodeAnalysisValue = Boolean.TrueString;
            if (string.IsNullOrEmpty(rulesPath))
            {
                runCodeAnalysisValue = runCodeAnalysisValues[project.UniqueName];
                rulesPath = runCodeAnalysisValues[project.UniqueName + "CodeAnalysisRuleSet"];
            }
            else
            {
                runCodeAnalysisValues.Add(project.UniqueName,
                    project.ConfigurationManager.ActiveConfiguration.Properties.Item("RunCodeAnalysis").Value.ToString());
                runCodeAnalysisValues.Add(project.UniqueName + "CodeAnalysisRuleSet",
                    project.ConfigurationManager.ActiveConfiguration.Properties.Item("CodeAnalysisRuleSet").Value.ToString());
            }
            project.ConfigurationManager.ActiveConfiguration.Properties.Item("RunCodeAnalysis").Value = runCodeAnalysisValue;
            project.ConfigurationManager.ActiveConfiguration.Properties.Item("CodeAnalysisRuleSet").Value = rulesPath;
        }

        private void SetCodeAnalysisOnWebProject(Project project, string rulesPath)
        {
            if (string.IsNullOrEmpty(rulesPath))
            {
                rulesPath = runCodeAnalysisValues[project.UniqueName + "CodeAnalysisRuleSet"];
            }
            else
            {
                runCodeAnalysisValues.Add(project.UniqueName + "CodeAnalysisRuleSet",
                    project.Properties.Item("CodeAnalysisRuleSet").Value.ToString());
            }
            project.Properties.Item("CodeAnalysisRuleSet").Value = rulesPath;
        }

        private void SetCodeAnalysisOnReferences(string rulesPath)
        {
            if (!DteHelper2.IsWebProject(this.project))
            {
                Vs.VSProject vsProject = this.project.Object as Vs.VSProject;
                foreach (Vs.Reference reference in vsProject.References)
                {
                    if (reference.SourceProject != null)
                    {
                        SetCodeAnalysisOnProject(reference.SourceProject, rulesPath);
                    }
                }
            }
            else
            {
                Web.VSWebSite webProject = this.project.Object as Web.VSWebSite;
                foreach (Web.AssemblyReference reference in webProject.References)
                {
                    Project project = GetSourceProject(reference);
                    if (project != null)
                    {
                        SetCodeAnalysisOnProject(project, rulesPath);
                    }
                }
            }
        }

        private Project GetSourceProject(Web.AssemblyReference reference)
        {
            Project sourceProject = null;

            if (reference.ReferenceKind != Web.AssemblyReferenceType.AssemblyReferenceConfig)
            {
                sourceProject = DteHelperEx.FindProject(reference.DTE, new Predicate<Project>(delegate(Project match)
                {
                    return (match.Kind == VSLangProj.PrjKind.prjKindCSharpProject ||
                        match.Kind == VSLangProj.PrjKind.prjKindVBProject) &&
                        match.Name.Equals(reference.Name, StringComparison.OrdinalIgnoreCase);
                }));
            }
            return sourceProject;
		}

        private void Rollback()
        {
            string rulesPath = string.Empty;

            //Clean all properties
            SetCodeAnalysisOnReferences(rulesPath);

            if (DteHelper2.IsWebProject(this.project))
            {
                SetCodeAnalysisOnWebProject(this.project, rulesPath);
            }
            else
            {
                SetCodeAnalysisOnProject(this.project, rulesPath);
            }

            if (!string.IsNullOrEmpty(this.webTempFile) &&
                File.Exists(this.webTempFile))
            {
                File.Delete(this.webTempFile);
            }
        }

		#endregion
	}
}
