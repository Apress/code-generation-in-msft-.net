// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Container form for editing control as a child
//  NOTE: Root and child are somewhat misnomers. The exact meaning 
//        is whether users will be able to add and remove records and
//        whether selectin will be allowed. 

using System;
using System.Windows.Forms;
using KADGen.BusinessObjectSupport;

namespace KADGen.WinSupport
{
	public class ChildEditForm : BaseEditForm
	{
 
		#region Class Declarations
		private bool mbIsLoaded;
		private System.Windows.Forms.Form mCallingForm;
		private System.Type mBusinessObjectType;
		private System.Windows.Forms.DialogResult mDlgResult;
		#endregion

		#region Windows Form Designer generated code

		public ChildEditForm() : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			mBtnLast = this.btnClose;
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
		internal System.Windows.Forms.Panel Panel1; 
		internal System.Windows.Forms.Panel pnlButtons; 
		internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.Button btnCancel; 
		internal System.Windows.Forms.Button btnSave; 
		internal System.Windows.Forms.Panel pnlShim; 
		internal System.Windows.Forms.Panel pnlback; 
		internal System.Windows.Forms.Panel pnlEdit;
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.Panel1 = new System.Windows.Forms.Panel();
			this.pnlback = new System.Windows.Forms.Panel();
			this.pnlEdit = new System.Windows.Forms.Panel();
			this.pnlShim = new System.Windows.Forms.Panel();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.Panel1.SuspendLayout();
			this.pnlback.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			//
			//Panel1
			//
			this.Panel1.Controls.Add(this.pnlback);
			this.Panel1.Controls.Add(this.pnlButtons);
			this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Panel1.Location = new System.Drawing.Point(0 , 0);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(352 , 198);
			this.Panel1.TabIndex = 19;
			//
			//pnlback
			//
			this.pnlback.Controls.Add(this.pnlEdit);
			this.pnlback.Controls.Add(this.pnlShim);
			this.pnlback.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlback.Location = new System.Drawing.Point(0 , 0);
			this.pnlback.Name = "pnlback";
			this.pnlback.Size = new System.Drawing.Size(272 , 198);
			this.pnlback.TabIndex = 20;
			//
			//pnlEdit
			//
			this.pnlEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlEdit.Location = new System.Drawing.Point(0 , 8);
			this.pnlEdit.Name = "pnlEdit";
			this.pnlEdit.Size = new System.Drawing.Size(272 , 190);
			this.pnlEdit.TabIndex = 1;
			//
			//pnlShim
			//
			this.pnlShim.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlShim.Location = new System.Drawing.Point(0 , 0);
			this.pnlShim.Name = "pnlShim";
			this.pnlShim.Size = new System.Drawing.Size(272 , 8);
			this.pnlShim.TabIndex = 0;
			//
			//pnlButtons
			//
			this.pnlButtons.Controls.Add(this.btnClose);
			this.pnlButtons.Controls.Add(this.btnCancel);
			this.pnlButtons.Controls.Add(this.btnSave);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlButtons.Location = new System.Drawing.Point(272 , 0);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(80 , 198);
			this.pnlButtons.TabIndex = 19;
			//
			//btnClose
			//
			this.btnClose.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(0 , 56);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 15;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			//
			//btnCancel
			//
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(0 , 32);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 14;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			//
			//btnSave
			//
			this.btnSave.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnSave.Location = new System.Drawing.Point(0 , 8);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 13;
			this.btnSave.Text = "&Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			//
			//ChildEditForm
			//
			this.AcceptButton = this.btnSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5 , 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(352 , 198);
			this.Controls.Add(this.Panel1);
			this.Name = "ChildEditForm";
			this.Text = "EditForm";
			this.Panel1.ResumeLayout(false);
			this.pnlback.ResumeLayout(false);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region Constructors
		#endregion

		#region Public Properties and Methods

		#region Show and ShowDialog 

		public void Show(
			WinSupport.BaseEditUserControl editUserControl ,
			CSLA.BusinessBase businessObject ,
			System.Windows.Forms.Form parent )
		{
			this.Show(editUserControl , businessObject ,
				parent , "");
		}

		public System.Windows.Forms.DialogResult ShowDialog(
			WinSupport.BaseEditUserControl editUserControl ,
			CSLA.BusinessBase businessObject ,
			string parentPrimaryKey ,
			System.Windows.Forms.Form parent )
		{
            
			this.ShowDialog(editUserControl , businessObject , parentPrimaryKey ,
				parent , "");
			return this.mDlgResult;
		}

		public void Show(
			WinSupport.BaseEditUserControl editUserControl ,
			CSLA.BusinessBase businessObject ,
			System.Windows.Forms.Form parent ,
			string title )
		{
			this.Show(editUserControl , businessObject ,
				parent , title , 0 , 0);
		}

		public System.Windows.Forms.DialogResult ShowDialog(
			WinSupport.BaseEditUserControl editUserControl ,
			CSLA.BusinessBase businessObject ,
			string parentPrimaryKey  ,
			System.Windows.Forms.Form parent ,
			string title )
		{
			this.ShowDialog(editUserControl , businessObject ,
				parentPrimaryKey , parent , title , 0 , 0);
			return this.mDlgResult;
		}

		public void Show(
			WinSupport.BaseEditUserControl editUserControl ,
			CSLA.BusinessBase businessObject ,
			System.Windows.Forms.Form parent ,
			string title  ,
			int openingWidth )
		{
			this.Show(editUserControl , businessObject ,
				parent , title , openingWidth , 0);
		}

		public System.Windows.Forms.DialogResult ShowDialog(
			WinSupport.BaseEditUserControl editUserControl ,
			CSLA.BusinessBase businessObject ,
			string parentPrimaryKey ,
			System.Windows.Forms.Form parent ,
			string title ,
			int openingWidth)
		{
			this.ShowDialog(editUserControl , businessObject ,
				parentPrimaryKey , parent , title , openingWidth , 0);
			return this.mDlgResult;
		}

		public void Show(
			WinSupport.BaseEditUserControl editUserControl ,
			CSLA.BusinessBase businessObject ,
			System.Windows.Forms.Form parent ,
			string title  ,
			int openingWidth  ,
			int openingHeight )
		{
			this.SetupForm(editUserControl ,
				businessObject , "" , parent , title ,
				openingWidth , openingHeight , false);
			base.Show();
		}

		public System.Windows.Forms.DialogResult ShowDialog(
			WinSupport.BaseEditUserControl editUserControl ,
			CSLA.BusinessBase businessObject ,
			string parentPrimaryKey ,
			System.Windows.Forms.Form parent ,
			string title ,
			int openingWidth ,
			int openingHeight )
		{
			this.SetupForm(editUserControl ,
				businessObject , parentPrimaryKey , parent , title ,
				openingWidth , openingHeight , true);
			base.ShowDialog(parent);
			return this.mDlgResult;
		}

		#endregion

		public IBusinessObject BusinessObject
		{
			get
			{
				return mObject;
			}
			set
			{
				mObject = value;
			}
		}

		public System.Windows.Forms.Form CallingForm 
		{
			get
			{
				return mCallingForm;
			}
			set
			{
				mCallingForm = value;
			}
		}

		#endregion

		#region Event Handlers 
		private void btnCancel_Click( object sender, System.EventArgs e )
		{
			OnBtnCancelClick(sender, e);
		}

		private void btnSave_Click(object sender, System.EventArgs e )
		{
			OnBtnSaveClick(sender , e);
		}

		private void btnClose_Click(object sender, System.EventArgs e )
		{
			OnBtnCloseClick(sender , e);
		}

		private void uc_DataChanged(object sender, System.EventArgs e )
		{
			System.Diagnostics.Debug.WriteLine("Here we are");
			this.SetState();
		}

		#endregion

		#region Protected Event Response
		protected override void OnLoad( System.EventArgs e )
		{
			int vMargin = ((IEditUserControl)this.mEditUserControl).VerticalMargin;
			int height = 0;
			int width = 0;

			base.OnLoad(e);
			if( this.CallingForm == null )
			{
				mFormMode = FormMode.Root;
			}
			else if (this.CallingForm is BaseEditForm )
			{
				mFormMode = FormMode.Child;
			}
			else
			{
				mFormMode = FormMode.Root;
			}
			if( mFormMode == FormMode.Child )
			{
				btnSave.Text = "OK";
			}
			else
			{
				btnSave.Text = "Save";
			}

			foreach( System.Windows.Forms.Control ctl  in this.Controls)
			{
				if( ctl is IEditUserControl )
				{
					((IEditUserControl) ctl).SetupControl(mObject);
				}
			}

			//  this.pnlButtons.BringToFront()
			this.pnlButtons.Dock = DockStyle.Right;
			this.pnlEdit.Dock = DockStyle.Fill;
			this.mEditUserControl.Dock = DockStyle.None;
			this.mEditUserControl.Dock = DockStyle.Fill;

			//Height = CType(this.mEditUserControl , IEditUserControl).IdealHeight
			IEditUserControl ieuc =  (IEditUserControl)this.mEditUserControl;
			if( ieuc.IdealHeight > 0 )
			{
				height = ieuc.IdealHeight;
			}
			else
			{
				height = mEditUserControl.Height;
			}
			if( ieuc.IdealWidth > 0 )
			{
				width = ieuc.IdealWidth + this.pnlButtons.Width;
			}
			else
			{
				width = this.ClientSize.Width;
			}
			if( height < mBtnLast.Bottom + vMargin )
			{
				height = mBtnLast.Bottom + vMargin;
			}
			else
			{
				height += 2 * vMargin;
			}
			this.ClientSize = new System.Drawing.Size(width , height);

			mbIsLoaded = true;

		}

		protected override void OnActivated( System.EventArgs e )
		{
			SetState();
		}

		protected virtual void OnBtnSaveClick( object sender ,System.EventArgs e )
		{
			this.BindingContext[mObject].EndCurrentEdit();
			mDlgResult = System.Windows.Forms.DialogResult.OK;
			if( mFormMode == FormMode.Root )
			{
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					this.mEditUserControl.VisitControls();
					bool bValid = this.mEditUserControl.IsFormValid();
					this.btnSave.Focus();
					if( ! bValid )
					{
						System.Windows.Forms.MessageBox.Show(
							"Please correct the errors on this form. You can hover the mouse over the red symbol next to each field to find out what the problem is." ,
							"Input Errors" ,
							System.Windows.Forms.MessageBoxButtons.OK ,
							MessageBoxIcon.Error);
					}
					else
					{
						mObject = ((IEditUserControl)mEditUserControl).Save();
						Cursor.Current = Cursors.Default;
					}
				}
				catch ( Exception ex )
				{
					Cursor.Current = Cursors.Default;
					throw;
				}
			}
			else
			{
				this.Hide();
			}
			SetState();
		}

		protected virtual void OnBtnCancelClick( object sender ,System.EventArgs e )
		{
			mDlgResult = System.Windows.Forms.DialogResult.Cancel;
			mObject.CancelEdit();
			if( mFormMode == FormMode.Child )
			{
				this.Hide();
			}
			else
			{
				(( IEditUserControl)this.mEditUserControl).SetupControl(mObject);
				this.mEditUserControl.VisitControls();
				this.SetState();
			}
		}

		protected virtual void OnBtnCloseClick( object sender ,System.EventArgs e )
		{
			this.Hide();
		}

		#endregion

		#region Protected Properties and Methods
		protected virtual void DataBindButtons()
		{
			if( ((IEditUserControl) mEditUserControl).CanCreate() )
			{
				Utility.BindField(btnSave , "Enabled" , mObject , "IsValid");
			}
			else
			{
				btnSave.Enabled = false;
			}
		}

		protected void SetupForm(
			BaseEditUserControl editUserControl ,
			CSLA.BusinessBase busObject ,
			string parentPrimaryKey ,
			System.Windows.Forms.Form parent ,
			string title ,
			int openingWidth ,
			int openingHeight ,
			bool isModal )
		{
			int iWidth = 0;
			int iHeight = 0;
			this.mEditUserControl = editUserControl;
			this.mEditUserControl.DataChanged += new BaseEditUserControl.DataChangedHandler(this.uc_DataChanged);
			this.mEditUserControl.BringToFront();
			this.pnlEdit.Controls.Add(editUserControl);
			mObject = (BusinessObjectSupport.IBusinessObject) busObject;
			if( (title == null) || (title.Trim().Length == 0) )
			{
				this.Text = editUserControl.Caption;
			}
			else
			{
				this.Text = title;
			}
			if( ! isModal && parent != null && parent.IsMdiContainer )
			{
				this.MdiParent = parent;
			}
			((IEditUserControl) this.mEditUserControl).SetupControl(mObject , parentPrimaryKey);
			mCallingForm = parent;
			if( openingWidth == 0 )
			{
				iWidth = this.ClientSize.Width + 1;
			}
			else
			{
				iWidth = openingWidth;
			}
			if( openingHeight == 0 )
			{
				iHeight = this.ClientSize.Height + 1;
			}
			else
			{
				iHeight = openingHeight;
			}
			if( iHeight < this.CallingForm.ClientSize.Height )
			{
				iHeight = this.CallingForm.ClientSize.Height;
			}
			this.ClientSize = new System.Drawing.Size(iWidth , iHeight);
			SetState();
		}
		#endregion

		#region Private Properties and Methods

		private void DisplayForEdits( System.EventArgs e , object pk )
		{
			IBusinessObject bo; 
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			bo = (IBusinessObject) Utility.InvokeSharedMethod(
				this.mBusinessObjectType ,
				"get" + this.mBusinessObjectType.Name ,
				pk);
			mObject = bo;
			((IEditUserControl) this.mEditUserControl).SetupControl((BusinessObjectSupport.IBusinessObject) bo );
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
		}

		private void SetState()
		{
			EditMode editmode  = ((IEditUserControl) this.mEditUserControl).EditMode;
			this.btnSave.Enabled = ((editmode & EditMode.IsDirty) > 0);
			this.btnCancel.Enabled = ((editmode & EditMode.IsDirty) > 0) ||
				((editmode & EditMode.IsNew) > 0);
			this.btnClose.Enabled = ! btnCancel.Enabled;
		}
		#endregion


	}
}