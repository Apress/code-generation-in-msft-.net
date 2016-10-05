// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Base for selection user controls - WinForms 

using System;

namespace KADGen.WinSupport
{
	public class SelectionMadeEventArgs : System.EventArgs
	{
		public object primaryKey;
		public SelectionMadeEventArgs( object pk )
		{
			primaryKey = pk;
		}
	}

	public class BaseSelectUserControl : System.Windows.Forms.UserControl
	{
		public delegate void SelectionMadeHandler(
			object sender,
			SelectionMadeEventArgs e );
		public event SelectionMadeHandler SelectionMade;


		#region Class Declarations
		private CSLA.ReadOnlyCollectionBase mList;
		protected string mCaption;
		#endregion

		#region Windows Form Designer generated code

		public BaseSelectUserControl() : base()
		{

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}

		//Form overrides dispose to clean up the component list.
		protected override void Dispose( bool disposing )
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
		protected internal System.Windows.Forms.DataGrid dgDisplay;

		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.dgDisplay = new System.Windows.Forms.DataGrid();
			this.SuspendLayout();
			//
			//dgDisplay
			//
			this.dgDisplay.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right) ;
			this.dgDisplay.DataSource = null;
			this.dgDisplay.Location = new System.Drawing.Point(0 , 0);
			this.dgDisplay.Name = "dgDisplay";
			this.dgDisplay.Size = new System.Drawing.Size(352 , 232);
			this.dgDisplay.TabIndex = 2;
			this.dgDisplay.CurrentCellChanged += new System.EventHandler(this.dgDisplay_CurrentCellChanged);
			//
			//ProjectSelect
			//
			this.ClientSize = new System.Drawing.Size(456 , 229);
			this.Controls.Add(this.dgDisplay);
			this.Name = "ProjectSelect";
			this.Text = "";
			this.ResumeLayout(false);

		}

		#endregion

		public object Result
		{
			get
			{
				return GetResult;
			}
		}

		public virtual  CSLA.ReadOnlyCollectionBase GetList()
		{
			throw new System.Exception("Must Implement GetList in derived form");
		}

		protected virtual void BuildColumns()
		{
			throw new System.Exception("Must Implement BuildColumns in derived form");
		}

		protected virtual object GetResult 
		{
			get
			{
				throw new System.Exception("Must Implement GetResult in derived form");
			}
		}

		protected override void OnLoad( System.EventArgs e )
		{
			mList = GetList();
			dgDisplay.DataSource = mList;
			dgDisplay.Focus();
		}

		private void dgDisplay_CurrentCellChanged( object sender, System.EventArgs e ) 
	{
			SelectionMade(this , new SelectionMadeEventArgs(this.Result));
		}

		public string Caption
		{
			get
			{
				return mCaption;
			}
		}
	}
}