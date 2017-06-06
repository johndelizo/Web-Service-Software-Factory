namespace Microsoft.Practices.ServiceFactory.ServiceContracts
{
	partial class XsdElementBrowserControl
	{
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
			if(disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XsdElementBrowserControl));
			this.trvHierarchy = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// trvHierarchy
			// 
			this.trvHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvHierarchy.ImageIndex = 0;
			this.trvHierarchy.ImageList = this.imageList;
			this.trvHierarchy.Location = new System.Drawing.Point(0, 0);
			this.trvHierarchy.Name = "trvHierarchy";
			this.trvHierarchy.SelectedImageIndex = 0;
			this.trvHierarchy.Size = new System.Drawing.Size(343, 288);
			this.trvHierarchy.TabIndex = 0;
			this.trvHierarchy.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trvHierarchy_AfterCollapse);
			this.trvHierarchy.DoubleClick += new System.EventHandler(this.trvHierarchy_DoubleClick);
			this.trvHierarchy.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvHierarchy_AfterSelect);
			this.trvHierarchy.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trvHierarchy_AfterExpand);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "");
			this.imageList.Images.SetKeyName(3, "");
			this.imageList.Images.SetKeyName(4, "");
			this.imageList.Images.SetKeyName(5, "");
			this.imageList.Images.SetKeyName(6, "");
			this.imageList.Images.SetKeyName(7, "");
			this.imageList.Images.SetKeyName(8, "");
			this.imageList.Images.SetKeyName(9, "");
			this.imageList.Images.SetKeyName(10, "");
			this.imageList.Images.SetKeyName(11, "");
			this.imageList.Images.SetKeyName(12, "NoAction.bmp");
			// 
			// XsdElementBrowserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.trvHierarchy);
			this.Name = "XsdElementBrowserControl";
			this.Size = new System.Drawing.Size(343, 288);
			this.Load += new System.EventHandler(this.XsdElementBrowserControl_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView trvHierarchy;
		private System.Windows.Forms.ImageList imageList;
	}
}
