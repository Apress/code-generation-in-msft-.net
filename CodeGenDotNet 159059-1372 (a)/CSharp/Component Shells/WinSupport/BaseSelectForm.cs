// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Base class for the selection form
//  Refactor: Change select form to better support user control

using System;

namespace KADGen.WinSupport
{
	public class BaseSelectForm : System.Windows.Forms.Form
	{
		#region Class Declarations
		private CSLA.ReadOnlyCollectionBase mList;
		#endregion

		#region Windows Form Designer generated code

		public BaseSelectForm() : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}

		//Form overrides dispose to clean up the component list.
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
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Button btnCancel;
		protected internal System.Windows.Forms.DataGrid dgDisplay;

		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.dgDisplay = new System.Windows.Forms.DataGrid();
			this.SuspendLayout();
			//
			//btnOK
			//
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.Location = new System.Drawing.Point(368 , 8);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			//
			//btnCancel
			//
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(368 , 40);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			//
			//dgDisplay
			//
			this.dgDisplay.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right);
			this.dgDisplay.DataSource = null;
			this.dgDisplay.Location = new System.Drawing.Point(0 , 0);
			this.dgDisplay.Name = "dgDisplay";
			this.dgDisplay.Size = new System.Drawing.Size(352 , 232);
			this.dgDisplay.TabIndex = 2;
			this.dgDisplay.DoubleClick += new System.EventHandler(this.btnOK_Click);
			//
			//ProjectSelect
			//
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5 , 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(456 , 229);
			this.Controls.Add(this.dgDisplay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Name = "ProjectSelect";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Project Select";
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

		protected virtual CSLA.ReadOnlyCollectionBase GetList() 
		{
			throw new System.Exception("Must Implement GetList in derived form");
		}

		protected virtual void SetResult()
		{
			throw new System.Exception("Must Implement GetResult in derived form");
		}

		protected virtual void SetEmptyResult()
		{
			throw new System.Exception("Must Implement GetEmptyResult in derived form");
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

		private void btnOK_Click(
			object sender,
			System.EventArgs e)
		{
			SetResult();
			Hide();
		}

		private void btnCancel_Click(
			object sender,
			System.EventArgs e)
		{
			SetEmptyResult();
			Hide();
		}

	}
}