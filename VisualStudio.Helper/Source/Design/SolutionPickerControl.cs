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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.VisualStudio.Helper.Design
{
	/// <summary>
	/// User control that allows selection of a valid target given a <see cref="IUnboundAssetReference"/> or 
	/// <see cref="Type"/> of the target.
	/// </summary>
	public partial class SolutionPickerControl : UserControl
	{
		ISolutionPickerFilter filter;
		ISolutionPickerFilter onSelectFilter = new DefaultProjectsOnlyFilter();

		HierarchyNode root;
		// The initially selected element.
		object originalSelection;

		/// <summary>
		/// Empty constructor for design-time support.
		/// </summary>
		public SolutionPickerControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Event rised whenever the user selects a new node in the tree.
		/// </summary>
		public event EventHandler<EventArgs<HierarchyNode>> SelectionChanged;

		/// <summary>
		/// Initializes the control receiving the root value 
		/// to customize the behavior of the control.
		/// </summary>
		/// <param name="root"></param>
		public SolutionPickerControl(HierarchyNode root)
			:this(root,null,null)
		{
		}

		/// <summary>
		/// Initializes the control receiving the root value and a filter
		/// to customize the behavior of the control.
		/// </summary>
		/// <param name="root"></param>
		/// <param name="filter"></param>
		public SolutionPickerControl(HierarchyNode root, ISolutionPickerFilter filter)
			: this(root, null, filter)
		{
		}


		/// <summary>
		/// Initializes the control receiving the root guid and current value 
		/// to customize the behavior of the control.
		/// </summary>
		/// <param name="root">The root used for the Tree</param>
		/// <param name="currentValue">The current value</param>
		/// <param name="filter">The filter. Can be <see langword="null"/>.</param>
		public SolutionPickerControl(HierarchyNode root, HierarchyNode currentValue,ISolutionPickerFilter filter)
		{
			Initialize(root,currentValue,filter);
		}

		private void Initialize(HierarchyNode root, HierarchyNode currentValue, ISolutionPickerFilter filter)
		{
			this.filter = filter;
			this.root = root;
			this.originalSelection = currentValue;
			InitializeComponent();
			this.SuspendLayout();
			CreateNode(solutionTree.Nodes, this.root);
			this.ResumeLayout(false);
		}

		private object childrenTag = new object();

		private TreeNode CreateNode(TreeNodeCollection parentCollection, HierarchyNode hierarchyNode)
		{
			Debug.Assert(hierarchyNode.Icon != null);
			TreeNode node = new TreeNode(hierarchyNode.Name);
			if (!treeIcons.Images.ContainsKey(hierarchyNode.IconKey) && hierarchyNode.Icon != null)
			{
				treeIcons.Images.Add(hierarchyNode.IconKey, hierarchyNode.Icon);
			}
			node.ImageKey = hierarchyNode.IconKey;
			node.SelectedImageKey = hierarchyNode.IconKey;
			node.Name = hierarchyNode.Name;
			node.Tag = hierarchyNode;
			if (hierarchyNode.HasChildren)
			{
				bool filterAll=true;
				foreach (HierarchyNode child in hierarchyNode.Children)
				{
					if (!Filter(child))
					{
						filterAll = false;
						break;
					}
				}
				if (!filterAll)
				{
					TreeNode firstChildNode = new TreeNode();
					firstChildNode.Tag = childrenTag;
					node.Nodes.Add(firstChildNode);
				}
			}
			parentCollection.Add(node);
			return node;
		}

		/// <summary>
		/// Gets the target selected in the treeview.
		/// </summary>
		public HierarchyNode SelectedTarget
		{
			get
			{
				return this.solutionTree.SelectedNode == null ?
						null : (HierarchyNode)this.solutionTree.SelectedNode.Tag;
			}
		}

		private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == this.childrenTag)
			{
				e.Node.Nodes.Remove(e.Node.Nodes[0]);
				HierarchyNode hierarchyNode = (HierarchyNode)e.Node.Tag;
				foreach (HierarchyNode child in hierarchyNode.Children)
				{
					if (!Filter(child))
					{
						CreateNode(e.Node.Nodes, child);
					}
				}
			}
		}

		private bool Filter(HierarchyNode node)
		{
			if (filter != null && filter.Filter(node))
			{
				return true;
			}
			return false;
		}

		private bool SelectFilter(HierarchyNode node)
		{
			if (onSelectFilter != null & onSelectFilter.Filter(node))
			{
				return true;
			}

			return false;
		}

		private void OnSelect(object sender, EventArgs e)
		{
			HierarchyNode node = this.solutionTree.SelectedNode.Tag as HierarchyNode;

			if (SelectionChanged != null && 
                node != null && 
                !SelectFilter(node))
			{
				SelectionChanged(this, new EventArgs<HierarchyNode>(node));
			}
		}

		internal class DefaultProjectsOnlyFilter : ISolutionPickerFilter
		{
			#region ISolutionPickerFilter Members

			public bool Filter(HierarchyNode node)
			{
				Guard.ArgumentNotNull(node,"node");
				return ( !(node.ExtObject is EnvDTE.Project) ||
						node.TypeGuid == Microsoft.VisualStudio.VSConstants.GUID_ItemType_VirtualFolder);
			}

			#endregion
		}
    }
}
