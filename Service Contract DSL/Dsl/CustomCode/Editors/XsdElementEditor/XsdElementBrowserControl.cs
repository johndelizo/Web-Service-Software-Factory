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
using System.Windows.Forms;
using System.Windows.Forms.Design;
using EnvDTE;
using EnvDTE80;
using Microsoft.Practices.Modeling.Common.Logging;
using Microsoft.Practices.ServiceFactory.Description;
using Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	[CLSCompliant(false)]
	public partial class XsdElementBrowserControl : UserControl
	{
		private const string VBProject = ProvideRelatedFileAttribute.VisualBasicProjectGuid; // "{F184B08F-C81C-45F6-A57F-5ABD9991F28F}";
		private const string CSharpProject =  ProvideRelatedFileAttribute.CSharpProjectGuid; // "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
		private const string SchemaExtension = ".xsd";
		private const string WsdlExtension = ".wsdl";

		public event EventHandler SelectionChanged;
		public event EventHandler ElementAccepted;

		public XsdElementBrowserControl()
		{
			InitializeComponent();
		}

		private IServiceProvider serviceProvider;

		public IServiceProvider ServiceProvider
		{
			get { return serviceProvider; }
			set { serviceProvider = value; }
		}

		private XmlSchemaElementMoniker xsdElementUri;

		public XmlSchemaElementMoniker XsdElementUri
		{
			get { return xsdElementUri; }
			set { xsdElementUri = value; }
		}

		private void XsdElementBrowserControl_Load(object sender, EventArgs e)
		{
			Cursor currentCursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			LoadHierarchy();
			this.Cursor = currentCursor;
		}

		private void trvHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.XsdElementUri = null;

			if(e.Node.Tag is ProjectItem)
			{
				ProjectItem projectItem = e.Node.Tag as ProjectItem;

				if(!projectItem.Kind.Equals(Constants.vsProjectItemKindPhysicalFolder, StringComparison.OrdinalIgnoreCase))
				{
					Cursor currentCursor = this.Cursor;
					this.Cursor = Cursors.WaitCursor;
					LoadXsdElementsHierarchy(e.Node);
					this.Cursor = currentCursor;
				}
			}
			else if(e.Node is XsdElementNode)
			{
				ProjectItem parent = e.Node.Parent.Tag as ProjectItem;
				this.XsdElementUri =
					new XmlSchemaElementMoniker(DteHelper2.BuildPath(parent), e.Node.Text);
			}

			if(SelectionChanged != null)
			{
				SelectionChanged(this, EventArgs.Empty);
			}
		}

		private void trvHierarchy_DoubleClick(object sender, EventArgs e)
		{
			if(ElementAccepted != null)
			{
				ElementAccepted(this, EventArgs.Empty);
			}
		}

		private void trvHierarchy_AfterExpand(object sender, TreeViewEventArgs e)
		{
			if(e.Node.Tag is Project)
			{
				Project project = e.Node.Tag as Project;

				if(project.Object is SolutionFolder)
				{
					SetImage(e.Node, 11);
				}
			}
			else if(e.Node.Tag is ProjectItem)
			{
				ProjectItem projectItem = e.Node.Tag as ProjectItem;

				if(projectItem.Kind.Equals(Constants.vsProjectItemKindPhysicalFolder))
				{
					SetImage(e.Node, 6);
				}
			}
		}

		private void trvHierarchy_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			if(e.Node.Tag is Project)
			{
				Project project = e.Node.Tag as Project;

				if(project.Object is SolutionFolder)
				{
					SetImage(e.Node, 10);
				}
			}
			else if(e.Node.Tag is ProjectItem)
			{
				ProjectItem projectItem = e.Node.Tag as ProjectItem;

				if(projectItem.Kind.Equals(Constants.vsProjectItemKindPhysicalFolder))
				{
					SetImage(e.Node, 1);
				}
			}
		}

		private void LoadHierarchy()
		{
			//For Design time only
			if(this.serviceProvider != null)
			{
				DTE vs = (DTE)this.serviceProvider.GetService(typeof(DTE));

				trvHierarchy.Nodes.Add(
					UISolutionHierarchy.CreateHierarchy(vs.Solution,
					delegate(Project project)
					{
						return (project != null ||
							(project != null && project.Object is SolutionFolder));
					},
					delegate(ProjectItem projectItem)
					{
						return
							(projectItem.Kind.Equals(Constants.vsProjectItemKindPhysicalFolder) || 
							 projectItem.Name.EndsWith(SchemaExtension, StringComparison.OrdinalIgnoreCase) ||
							 projectItem.Name.EndsWith(WsdlExtension, StringComparison.OrdinalIgnoreCase));
					},
					delegate(TreeNode node)
					{
						if(node.Tag is Solution)
						{
							SetImage(node, 0);
						}
						else if(node.Tag is Project)
						{
							Project project = node.Tag as Project;

							if(project.Object is SolutionFolder)
							{
								SetImage(node, 10);
								return;
							}

							switch(project.Kind)
							{
								case CSharpProject:
									if(HasProperty(project.Properties, "CurrentWebsiteLanguage"))
									{
										SetImage(node, 4);
										break;
									}

									SetImage(node, 2);
									break;
								case VBProject:
									if(HasProperty(project.Properties, "CurrentWebsiteLanguage"))
									{
										SetImage(node, 5);
										break;
									}

									SetImage(node, 3);
									break;

								default:
									SetImage(node, 2);
									break;
							}
						}
						else if(node.Tag is ProjectItem)
						{
							ProjectItem projectItem = node.Tag as ProjectItem;

							if(projectItem.Kind.Equals(Constants.vsProjectItemKindPhysicalFolder))
							{
								SetImage(node, 1);
							}
							else
							{
								// XSD project item
								SetImage(node, 7);
							}
						}
					}
					));
			}
		}

		private void LoadXsdElementsHierarchy(TreeNode node)
		{
			if (node.Nodes.Count > 0)
			{
				// use already loaded nodes
				return;
			}

			DTE vs = (DTE)this.serviceProvider.GetService(typeof(DTE));
			ProjectItem projectItemNode = node.Tag as ProjectItem;
			string itemPath = null;

			if(projectItemNode.Properties != null &&
				projectItemNode.Properties.Item("FullPath").Value != null)
			{
				itemPath = projectItemNode.Properties.Item("FullPath").Value.ToString();
			}
			else
			{
				itemPath = projectItemNode.get_FileNames(1);
			}

			if(!String.IsNullOrEmpty(itemPath))
			{
				// try first with DC serializer
				XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(false);
				try
				{
					AddNodesFromTypes(generator, node, itemPath);
				}
				catch (InvalidSerializerException)
				{
					// now try with Xml serializer
					generator = new XmlSchemaTypeGenerator(true);
					AddNodesFromTypes(generator, node, itemPath);
				}
				catch (Exception ex)
				{
					IUIService iUIservice =
						this.serviceProvider.GetService(typeof(IUIService)) as IUIService;

					iUIservice.ShowError(ex, LogEntry.ErrorMessageToString(ex));
				}
			}
		}

		private void AddNodesFromTypes(XmlSchemaTypeGenerator generator, TreeNode node, string itemPath)
		{
			foreach (System.CodeDom.CodeNamespace ns in generator.GenerateCodeCompileUnit(itemPath).Namespaces)
			{
				foreach (System.CodeDom.CodeTypeDeclaration codeType in ns.Types)
				{
					if ((codeType.IsClass || codeType.IsEnum || codeType.IsStruct) &&
						!node.Nodes.ContainsKey(codeType.Name))
					{
						XsdElementNode elementNode = new XsdElementNode(codeType.Name, codeType);
						node.Nodes.Add(elementNode);
					}
				}
			}
			// add empty node if no elem added
			if (node.Nodes.Count == 0)
			{
				node.Nodes.Add(new XsdEmptyNode());
			}
		}

		private static void SetImage(TreeNode node, int imageIndex)
		{
			node.SelectedImageIndex = imageIndex;
			node.ImageIndex = imageIndex;
			node.StateImageIndex = imageIndex;
		}

		private static bool HasProperty(EnvDTE.Properties properties, string name)
		{
			try
			{
				return (((properties.Item(name) != null) && (properties.Item(name).Value != null)) && !string.IsNullOrEmpty(properties.Item(name).Value.ToString()));
			}
			catch
			{
				return false;
			}
		}
	}
}