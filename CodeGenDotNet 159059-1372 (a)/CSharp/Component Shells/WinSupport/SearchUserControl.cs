// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Search User Control
//  NOTE: THis code is preliminary

using System;

namespace KADGen.WinSupport
{
	public class SearchUserControl : System.Windows.Forms.UserControl
	{

		#region Class Declarations
		private CSLA.ReadOnlyCollectionBase mList;
		#endregion

		#region Windows Form Designer generated code

		public SearchUserControl() : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}

		//UserControl overrides dispose to clean up the component list.
		protected  override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( ! (components == null) )
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		internal System.Windows.Forms.Label lblSearch;
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.lblSearch = new System.Windows.Forms.Label();
			this.SuspendLayout();
			//
			//lblSearch
			//
			this.lblSearch.AutoSize = true;
			this.lblSearch.Location = new System.Drawing.Point(0 , 0);
			this.lblSearch.Name = "lblSearch";
			this.lblSearch.Size = new System.Drawing.Size(40 , 16);
			this.lblSearch.TabIndex = 0;
			this.lblSearch.Text = "Search";
			//
			//SearchUserControl
			//
			this.Controls.Add(this.lblSearch);
			this.Name = "SearchUserControl";
			this.Size = new System.Drawing.Size(304 , 24);
			this.ResumeLayout(false);

		}

		#endregion

		public CSLA.ReadOnlyCollectionBase BusinessObject 
		{
			get
			{
				return mList;
			}
			set
			{
				mList = value;
				SetupControl();
			}
		}

		private void SetupControl()
		{
		}
	}
}