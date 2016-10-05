// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Very simple user control to support null handling of dates

using System;

namespace KADGen.WinSupport
{
	public class DateTimeUC : System.Windows.Forms.UserControl
	{

		private System.Drawing.Color mOriginalForeColor;
		private System.Windows.Forms.DateTimePickerFormat mOriginalFormat;
		private string mOriginalCustomFormat;
		private bool mReadOnly;

		#region Windows Form Designer generated code

		public DateTimeUC() : base()
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
		internal System.Windows.Forms.CheckBox chkEmpty;
		internal System.Windows.Forms.DateTimePicker dtsValue ;
		internal  System.Windows.Forms.TextBox txtReadOnlyValue;
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.chkEmpty = new System.Windows.Forms.CheckBox();
			this.txtReadOnlyValue = new System.Windows.Forms.TextBox();
			this.dtsValue = new System.Windows.Forms.DateTimePicker();
			this.SuspendLayout();
			//
			//chkEmpty
			//
			this.chkEmpty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEmpty.Location = new System.Drawing.Point(0 , 0);
			this.chkEmpty.Name = "chkEmpty";
			this.chkEmpty.Size = new System.Drawing.Size(56 , 20);
			this.chkEmpty.TabIndex = 0;
			this.chkEmpty.Text = "Empty";
			this.chkEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkEmpty.CheckedChanged += new System.EventHandler(this.chkEmpty_CheckedChanged);
			//
			//txtReadOnlyValue
			//
			this.txtReadOnlyValue.Enabled = false;
			this.txtReadOnlyValue.Location = new System.Drawing.Point(0 , 0);
			this.txtReadOnlyValue.Name = "txtReadOnlyValue";
			this.txtReadOnlyValue.ReadOnly = true;
			this.txtReadOnlyValue.Size = new System.Drawing.Size(264 , 20);
			this.txtReadOnlyValue.TabIndex = 2;
			this.txtReadOnlyValue.Text = "";
			this.txtReadOnlyValue.Visible = false;
			//
			//dtsValue
			//
			this.dtsValue.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtsValue.Location = new System.Drawing.Point(88 , 0);
			this.dtsValue.Name = "dtsValue";
			this.dtsValue.Size = new System.Drawing.Size(176 , 20);
			this.dtsValue.TabIndex = 3;
			this.dtsValue.CloseUp += new System.EventHandler(this.dtsValue_CloseUp);
			this.dtsValue.TextChanged += new System.EventHandler(this.dtsValue_TextChanged);
			//
			//DateTimeUC
			//
			this.Controls.Add(this.dtsValue);
			this.Controls.Add(this.chkEmpty);
			this.Controls.Add(this.txtReadOnlyValue);
			this.Name = "DateTimeUC";
			this.Size = new System.Drawing.Size(264 , 20);
			this.ResumeLayout(false);

		}

		#endregion

		#region Public Properties and Methods
		public override string Text 
		{
			get
			{
				if( chkEmpty.Checked )
				{
					return "";
				}
				else
				{
					return dtsValue.Text;
				}
			}
			set
			{
				if( (value == "") | (value == "1/1/1900") )
				{
					chkEmpty.Checked = true;
					dtsValue.Width = dtsValue.Height;
					txtReadOnlyValue.Text = "";
				}
				else
				{
					chkEmpty.Checked = false;
					dtsValue.Width = dtsValue.Height;
					dtsValue.Text = value;
					txtReadOnlyValue.Text = value;
				}
			}
		}

		public string EmptyText
		{
			get
			{
				return chkEmpty.Text;
			}
			set
			{
				chkEmpty.Text = value;
			}
		}

		public System.Windows.Forms.Appearance CheckAppearance
		{
			get
			{
				return chkEmpty.Appearance;
			}
			set
			{
				chkEmpty.Appearance = value;
			}
		}

		public System.Windows.Forms.DateTimePickerFormat Format 
		{
			get
			{
				return dtsValue.Format;
			}
			set
			{
				dtsValue.Format = value;
			}
		}

		public string CustomFormat
		{ 
			get
			{
				return dtsValue.CustomFormat;
			}
			set
			{
				dtsValue.CustomFormat = value;
			}
		}

		public System.DateTime MinDate 
		{
			get
			{
				return dtsValue.MinDate;
			}
			set
			{
				dtsValue.MinDate = value;
			}
		}

		public System.DateTime MaxDate 
		{
			get
			{
				return dtsValue.MaxDate;
			}
			set
			{
				dtsValue.MaxDate = value;
			}
		}

		public bool @ReadOnly
		{
			get
			{
				return mReadOnly;
			}
			set
			{
				mReadOnly = value;
				chkEmpty.Visible = ! value;
				dtsValue.Visible = ! value;
				txtReadOnlyValue.Visible = value;
			}
		}

		#endregion

		#region Event Handlers
		private void chkEmpty_CheckedChanged( object sender, System.EventArgs e )
		{
			if( this.ActiveControl != dtsValue )
			{
				this.PerformLayout();
			}
		}

		private void dtsValue_TextChanged(object sender, System.EventArgs e )
		{
			chkEmpty.Checked = false;
			OnTextChanged(e);
		}

		private void dtsValue_CloseUp(object sender, System.EventArgs e )
		{
			this.PerformLayout();
		}

		protected override void OnLayout(System.Windows.Forms.LayoutEventArgs levent )
		{
			if( mReadOnly )
			{
				dtsValue.Left = 0;
				dtsValue.Width = this.Width;
			}
			else
			{
				dtsValue.Left = chkEmpty.Right;
				dtsValue.Width = this.Width - chkEmpty.Right;
			}
			if( ! this.DesignMode )
			{
				if( this.mOriginalFormat != 0 )
				{
					if( chkEmpty.Checked )
					{
						dtsValue.ForeColor = System.Drawing.Color.AntiqueWhite;
						dtsValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
						dtsValue.CustomFormat = "////";
					}
					else
					{
						dtsValue.Format = this.mOriginalFormat;
						dtsValue.CustomFormat = this.mOriginalCustomFormat;
						dtsValue.ForeColor = mOriginalForeColor;
					}
				}
			}
		}

		protected override void OnLoad( System.EventArgs e )
		{
			this.mOriginalForeColor = this.ForeColor;
			this.mOriginalCustomFormat = this.CustomFormat;
			this.mOriginalFormat = this.Format;
		}

		#endregion
	}
}