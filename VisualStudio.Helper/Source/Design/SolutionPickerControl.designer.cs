using System;
namespace Microsoft.Practices.VisualStudio.Helper.Design
{
	public partial class SolutionPickerControl
	{
		private System.Windows.Forms.ImageList treeIcons;
		private System.Windows.Forms.TreeView solutionTree;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionPickerControl));
			this.treeIcons = new System.Windows.Forms.ImageList(this.components);
			this.solutionTree = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// treeIcons
			// 
			this.treeIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			resources.ApplyResources(this.treeIcons, "treeIcons");
			this.treeIcons.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// solutionTree
			// 
			resources.ApplyResources(this.solutionTree, "solutionTree");
			this.solutionTree.ImageList = this.treeIcons;
			this.solutionTree.Name = "solutionTree";
			this.solutionTree.Sorted = true;
			this.solutionTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnBeforeExpand);
			this.solutionTree.DoubleClick += new System.EventHandler(this.OnSelect);
			// 
			// SolutionPickerControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.solutionTree);
			this.Name = "SolutionPickerControl";
			this.ResumeLayout(false);

		}

		#endregion
	}
}
