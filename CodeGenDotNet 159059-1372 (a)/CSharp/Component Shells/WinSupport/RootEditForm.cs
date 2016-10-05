// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Container form for editing control as a root
//  NOTE: Root and child are somewhat misnomers. The exact meaning 
//        is whether users will be able to add and remove records and
//        whether selectin will be allowed. 

using System;
using System.Windows.Forms;
using KADGen.BusinessObjectSupport;

namespace KADGen.WinSupport
{
	public class RootEditForm : BaseEditForm
	{


		#region Class Declarations
		private bool mbIsLoaded;
		private System.Windows.Forms.Form mCallingForm;  
		private System.Type mBusinessObjectType; 
		private BaseSelectUserControl mSelectUserControl;
		#endregion

		#region Windows Form Designer generated code

		public RootEditForm() : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			mBtnLast = btnClose;

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
		internal System.Windows.Forms.Splitter Splitter1;
		internal System.Windows.Forms.Splitter Splitter2;
		internal System.Windows.Forms.ToolBar frmRootEditForm;
		internal System.Windows.Forms.ImageList ImageList;
		internal System.Windows.Forms.ToolBarButton tbbSearch;
		internal System.Windows.Forms.ToolBarButton tbbGrid;
		internal System.Windows.Forms.ToolBarButton tbbSave;
		internal System.Windows.Forms.ToolBarButton tbbCancel;
		internal System.Windows.Forms.Panel pnlButtons;
		internal System.Windows.Forms.Label lblSearch;
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.Button btnSave;
		internal System.Windows.Forms.Panel pnlBottom;
		internal System.Windows.Forms.Panel pnlEdit;
		internal System.Windows.Forms.ComboBox cboSelect;
		internal System.Windows.Forms.Button btnNew;
		internal System.Windows.Forms.Button btnDelete;
		internal System.Windows.Forms.Panel pnlSelect;
		internal System.Windows.Forms.ToolBar ToolBar;
		internal System.Windows.Forms.ToolBarButton tbbDelete;
		internal System.Windows.Forms.ToolBarButton tbbNew;
		internal System.Windows.Forms.Button btnClose;
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources  = new System.Resources.ResourceManager(typeof( RootEditForm ));
			this.pnlSelect = new System.Windows.Forms.Panel();
			this.cboSelect = new System.Windows.Forms.ComboBox();
			this.lblSearch = new System.Windows.Forms.Label();
			this.Splitter1 = new System.Windows.Forms.Splitter();
			this.Splitter2 = new System.Windows.Forms.Splitter();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.pnlEdit = new System.Windows.Forms.Panel();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnNew = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.ToolBar = new System.Windows.Forms.ToolBar();
			this.tbbSearch = new System.Windows.Forms.ToolBarButton();
			this.tbbGrid = new System.Windows.Forms.ToolBarButton();
			this.tbbSave = new System.Windows.Forms.ToolBarButton();
			this.tbbCancel = new System.Windows.Forms.ToolBarButton();
			this.tbbDelete = new System.Windows.Forms.ToolBarButton();
			this.tbbNew = new System.Windows.Forms.ToolBarButton();
			this.ImageList = new System.Windows.Forms.ImageList(this.components);
			this.pnlSelect.SuspendLayout();
			this.pnlBottom.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			//
			//pnlSelect
			//
			this.pnlSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSelect.Controls.Add(this.cboSelect);
			this.pnlSelect.Controls.Add(this.lblSearch);
			this.pnlSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSelect.Location = new System.Drawing.Point(0 , 28);
			this.pnlSelect.Name = "pnlSelect";
			this.pnlSelect.Size = new System.Drawing.Size(456 , 36);
			this.pnlSelect.TabIndex = 11;
			//
			//cboSelect
			//
			this.cboSelect.Location = new System.Drawing.Point(112 , 8);
			this.cboSelect.Name = "cboSelect";
			this.cboSelect.Size = new System.Drawing.Size(168 , 21);
			this.cboSelect.TabIndex = 1;
			this.cboSelect.Text = "ComboBox1";
			this.cboSelect.SelectedValueChanged += new System.EventHandler(this.cboSelect_SelectedValueChanged);
			//
			//lblSearch
			//
			this.lblSearch.Location = new System.Drawing.Point(8 , 8);
			this.lblSearch.Name = "lblSearch";
			this.lblSearch.TabIndex = 0;
			this.lblSearch.Text = "Select";
			//
			//Splitter1
			//
			this.Splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.Splitter1.Location = new System.Drawing.Point(0 , 64);
			this.Splitter1.Name = "Splitter1";
			this.Splitter1.Size = new System.Drawing.Size(456 , 3);
			this.Splitter1.TabIndex = 12;
			this.Splitter1.TabStop = false;
			//
			//Splitter2
			//
			this.Splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.Splitter2.Location = new System.Drawing.Point(0 , 67);
			this.Splitter2.Name = "Splitter2";
			this.Splitter2.Size = new System.Drawing.Size(456 , 3);
			this.Splitter2.TabIndex = 14;
			this.Splitter2.TabStop = false;
			//
			//pnlBottom
			//
			this.pnlBottom.Controls.Add(this.pnlEdit);
			this.pnlBottom.Controls.Add(this.pnlButtons);
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBottom.Location = new System.Drawing.Point(0 , 70);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(456 , 224);
			this.pnlBottom.TabIndex = 15;
			//
			//pnlEdit
			//
			this.pnlEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlEdit.Location = new System.Drawing.Point(0 , 0);
			this.pnlEdit.Name = "pnlEdit";
			this.pnlEdit.Size = new System.Drawing.Size(376 , 224);
			this.pnlEdit.TabIndex = 13;
			//
			//pnlButtons
			//
			this.pnlButtons.Controls.Add(this.btnClose);
			this.pnlButtons.Controls.Add(this.btnDelete);
			this.pnlButtons.Controls.Add(this.btnNew);
			this.pnlButtons.Controls.Add(this.btnCancel);
			this.pnlButtons.Controls.Add(this.btnSave);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlButtons.Location = new System.Drawing.Point(376 , 0);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(80 , 224);
			this.pnlButtons.TabIndex = 14;
			//
			//btnClose
			//
			this.btnClose.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(0 , 96);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 17;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			//
			//btnDelete
			//
			this.btnDelete.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnDelete.Location = new System.Drawing.Point(0 , 48);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 16;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			//
			//btnNew
			//
			this.btnNew.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnNew.Location = new System.Drawing.Point(0 , 24);
			this.btnNew.Name = "btnNew";
			this.btnNew.TabIndex = 15;
			this.btnNew.Text = "&New";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			//
			//btnCancel
			//
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(0 , 72);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 14;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			//
			//btnSave
			//
			this.btnSave.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnSave.Location = new System.Drawing.Point(0 , 0);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 13;
			this.btnSave.Text = "&Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			//
			//ToolBar
			//
			this.ToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.ToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {this.tbbSearch , this.tbbGrid , this.tbbSave , this.tbbCancel , this.tbbDelete , this.tbbNew});
			this.ToolBar.DropDownArrows = true;
			this.ToolBar.ImageList = this.ImageList;
			this.ToolBar.Location = new System.Drawing.Point(0 , 0);
			this.ToolBar.Name = "ToolBar";
			this.ToolBar.ShowToolTips = true;
			this.ToolBar.Size = new System.Drawing.Size(456 , 28);
			this.ToolBar.TabIndex = 16;
			this.ToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.ToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ToolBar_ButtonClick);
			//
			//tbbSearch
			//
			this.tbbSearch.ImageIndex = 2;
			this.tbbSearch.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbbSearch.Text = "Show Search";
			this.tbbSearch.Visible = false;
			//
			//tbbGrid
			//
			this.tbbGrid.ImageIndex = 3;
			this.tbbGrid.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tbbGrid.Text = "Show Grid";
			this.tbbGrid.Visible = false;
			//
			//tbbSave
			//
			this.tbbSave.ImageIndex = 1;
			this.tbbSave.Text = "Save";
			//
			//tbbCancel
			//
			this.tbbCancel.ImageIndex = 0;
			this.tbbCancel.Text = "Cancel";
			//
			//tbbDelete
			//
			this.tbbDelete.ImageIndex = 4;
			this.tbbDelete.Text = "Delete";
			//
			//tbbNew
			//
			this.tbbNew.ImageIndex = 5;
			this.tbbNew.Text = "New";
			//
			//ImageList
			//
			this.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ImageList.ImageSize = new System.Drawing.Size(16 , 16);
			this.ImageList.ImageStream = (System.Windows.Forms.ImageListStreamer) resources.GetObject("ImageList.ImageStream");
			this.ImageList.TransparentColor = System.Drawing.Color.Silver;
			//
			//frmRootEditForm
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5 , 13);
			this.ClientSize = new System.Drawing.Size(456 , 294);
			this.Controls.Add(this.pnlBottom);
			this.Controls.Add(this.Splitter2);
			this.Controls.Add(this.Splitter1);
			this.Controls.Add(this.pnlSelect);
			this.Controls.Add(this.ToolBar);
			this.Name = "frmRootEditForm";
			this.Text = "EditForm";
			this.pnlSelect.ResumeLayout(false);
			this.pnlBottom.ResumeLayout(false);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region Public Properties and Methods 

		#region Show and ShowDialog 

		public void Show(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessObjectType,
			System.Windows.Forms.Form parent)
		{
			Show(editUserControl , selectUserControl , businessObjectType ,
				parent , true);
		}

		public void ShowDialog(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessobjecttype,
			System.Windows.Forms.Form parent)
		{
			ShowDialog(editUserControl , selectUserControl , businessobjecttype ,
				parent , true);
		}

		public void Show(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessObjectType,
			System.Windows.Forms.Form parent,
			string title)
		{
			Show(editUserControl , selectUserControl , businessObjectType ,
				parent , title , true);
		}

		public void ShowDialog(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessobjecttype,
			System.Windows.Forms.Form parent,
			string title)
		{
			ShowDialog(editUserControl , selectUserControl , businessobjecttype ,
				parent , title , true);
		}

		public void Show(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessObjectType,
			System.Windows.Forms.Form parent,
			bool showToolbar)
		{
			Show(editUserControl , selectUserControl ,
				businessObjectType , parent ,
				"" , showToolbar);
		}

		public void ShowDialog(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessobjecttype,
			System.Windows.Forms.Form parent,
			bool showToolbar)
		{
			ShowDialog(editUserControl , selectUserControl ,
				businessobjecttype , parent ,
				"" , showToolbar);
		}

		public void Show(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessObjectType,
			System.Windows.Forms.Form parent,
			string title,
			bool showToolbar)
		{
			this.Show(editUserControl , selectUserControl ,
				businessObjectType , parent , title , showToolbar , 0 , 0);
		}

		public void ShowDialog(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessobjecttype,
			System.Windows.Forms.Form parent,
			string title,
			bool showToolbar)
		{
			this.ShowDialog(editUserControl , selectUserControl ,
				businessobjecttype , parent , title , showToolbar , 0 , 0);
		}

		public void Show(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessObjectType,
			System.Windows.Forms.Form parent,
			string title,
			bool showToolbar,
			int openingWidth)
		{
			this.Show(editUserControl , selectUserControl ,
				businessObjectType , parent , title , showToolbar , openingWidth , 0);
		}

		public void ShowDialog(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessobjecttype,
			System.Windows.Forms.Form parent,
			string title,
			bool showToolbar,
			int openingWidth)
		{
			this.ShowDialog(editUserControl , selectUserControl ,
				businessobjecttype , parent , title , showToolbar , openingWidth , 0);
		}

		public void Show(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessObjectType,
			System.Windows.Forms.Form parent,
			string title,
			bool showToolbar,
			int openingWidth,
			int openingHeight)
		{
			this.SetupForm(editUserControl , selectUserControl ,
				businessObjectType , parent , title , showToolbar ,
				openingWidth , openingHeight);
			base.Show();
		}

		public void ShowDialog(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessobjecttype,
			System.Windows.Forms.Form parent,
			string title,
			bool showToolbar,
			int openingWidth,
			int openingHeight)
		{
			this.SetupForm(editUserControl , selectUserControl ,
				businessobjecttype , parent , title , showToolbar ,
				openingWidth , openingHeight);
			base.ShowDialog(parent);
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
		private void btnCancel_Click(
			object sender,
			System.EventArgs e)
		{
			OnBtnCancelClick(sender , e);
		}

		private void btnClose_Click(
			object sender,
			System.EventArgs e)
		{
			OnBtnCloseClick(sender , e);
		}

		private void btnSave_Click(
			object sender,
			System.EventArgs e)
		{
			OnBtnSaveClick(sender , e);
		}

		private void btnNew_Click(
			object sender,
			System.EventArgs e)
		{
			OnBtnNewClick(sender , e);
		}

		private void btnDelete_Click(
			object sender,
			System.EventArgs e)
		{
			OnBtnDeleteClick(sender , e);
		}

		private void ToolBar_ButtonClick(
			object sender,
			System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			onToolBarButtonClick(sender , e);
		}

		private void selectUC_SelectionMade(
			object sender,
			SelectionMadeEventArgs e)
		{
			OnUCSelectionMade(sender , e);
		}

		private void cboSelect_SelectedValueChanged(
			object sender,
			System.EventArgs e)
		{
			OnCboSelectionmade(sender , e);
		}

		private void uc_DataChanged(
			object sender,
			System.EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Here we are again");
			this.SetState();
		}

		#endregion

		#region Protected Event Response
		protected override void OnLoad( System.EventArgs e )
		{
			int vMargin = ((IEditUserControl)this.mEditUserControl).VerticalMargin;
			int height;
			int width;

			base.OnLoad(e);
			if( this.CallingForm == null )
			{
				mFormMode = FormMode.Root;
			}
			else if( this.CallingForm is BaseEditForm ) 
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

			IEditUserControl ieuc = (IEditUserControl)this.mEditUserControl;
			this.cboSelect.Left = ieuc.ControlLeft;
			this.cboSelect.Width = ieuc.ControlWidth;
			btnSave.Top = ieuc.ControlTop;
			btnNew.Top = btnSave.Bottom;
			btnDelete.Top = btnNew.Bottom;
			btnCancel.Top = btnDelete.Bottom;
			btnClose.Top = btnCancel.Bottom;

			foreach( System.Windows.Forms.Control ctl in this.Controls )
			{
				if( ctl is IEditUserControl )
				{
					(( IEditUserControl ) ctl).SetupControl(mObject);
				}
			}

			ResetDatasource(null);
			this.tbbSearch.Pushed = true;
			this.tbbGrid.Pushed = false;
			this.pnlSelect.Visible = tbbSearch.Pushed;

			this.pnlSelect.Dock = DockStyle.Top;
			this.pnlBottom.Dock = DockStyle.Fill;
			this.pnlButtons.Dock = DockStyle.Right;
			this.pnlEdit.Dock = DockStyle.Fill;
			this.mEditUserControl.Dock = DockStyle.Fill;
			this.cboSelect.Width = ((IEditUserControl)this.mEditUserControl).ControlWidth;
			this.cboSelect.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

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
			this.ClientSize = new System.Drawing.Size(width , pnlBottom.Top + height);


			mbIsLoaded = true;
			if( this.cboSelect.Items.Count > 0 )
			{
				this.cboSelect.SelectedIndex = -1;
				this.cboSelect.SelectedIndex = 0;
			}
		}

		protected virtual void OnBtnNewClick(
			object sender,
			System.EventArgs e)
		{
			IBusinessObject bo;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				// KD - The second time you added a new record wasn//t clearing the combobox until there were 
				//     two calls to set the SelectIndex
				cboSelect.SelectedIndex = -1;
				cboSelect.SelectedIndex = -1;
				bo = (IBusinessObject)Utility.InvokeSharedMethod(
					this.mBusinessObjectType ,
					"New" + this.mBusinessObjectType.Name);
                 
				mObject = bo;
				IEditUserControl ieuc = (IEditUserControl)this.mEditUserControl;
				ieuc.SetupControl(bo);
				Cursor.Current = Cursors.Default;
			}
			catch ( Exception ex )
			{
				Cursor.Current = Cursors.Default;
				MessageBox.Show(ex.ToString());
			}
			SetState();
			this.mEditUserControl.Focus();
		}

		protected virtual void OnBtnDeleteClick(
			object sender,
			System.EventArgs e)
		{
			if( MessageBox.Show(
				"Do you really want to delete " + this.cboSelect.Text + "?" ,
				"Please confirm deletion" ,
				MessageBoxButtons.YesNo) == DialogResult.Yes )
			{
				((IEditUserControl)mEditUserControl).Delete();
				ResetDatasource(null);
			}
			SetState();
		}

		protected virtual void OnBtnSaveClick(
			object sender,
			System.EventArgs e)
		{
			this.BindingContext[mObject].EndCurrentEdit();
			if( btnSave.Text == "Save" )
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
						mObject = ((IEditUserControl) mEditUserControl).Save();
						ResetDatasource(mObject);
						this.mEditUserControl.Height = pnlEdit.Height;
						Cursor.Current = Cursors.Default;
					}
				}
				catch ( Exception ex )
				{
					Cursor.Current = Cursors.Default;
					throw;
				}
			}
			SetState();
		}

		protected virtual void OnBtnCancelClick(
			object sender,
			System.EventArgs e)
		{
			IEditUserControl ieuc = (IEditUserControl)this.mEditUserControl;
			bool isNew = ((ieuc.EditMode & EditMode.IsNew) > 0);
			ieuc.CancelEdit();
			this.mEditUserControl.VisitControls();
			if( isNew && ( cboSelect.Items.Count == 0 ) )
			{
				SetState();
			}
			else if( isNew )
			{
				cboSelect.SelectedIndex = 0;
			}
			else
			{
				SetState();
			}
		}

		protected virtual void OnBtnCloseClick(
			object sender,
			System.EventArgs e)
		{
			this.Close();
		}

		protected virtual void onToolBarButtonClick(
			object sender,
			System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if( e.Button == this.tbbSearch )
			{
				if( tbbGrid.Pushed )
				{
					tbbGrid.Pushed = false;
				}
				SetPanels();
			}
			else if(e.Button == this.tbbGrid )
			{
				if( tbbSearch.Pushed )
				{
					tbbSearch.Pushed = false;
				}
				SetPanels();
			}
			else if( e.Button == this.tbbSave )
			{
				btnSave.PerformClick();
			}
			else if( e.Button == this.tbbNew )
			{
				btnNew.PerformClick();
			}
			else if( e.Button == this.tbbDelete )
			{
				btnDelete.PerformClick();
			}
			else if( e.Button == this.tbbCancel )
			{
				btnCancel.PerformLayout();
			}
			SetState();
		}

		protected virtual void OnUCSelectionMade(
			object sender,
			SelectionMadeEventArgs e)
		{
			DisplayForEdits(e , e.primaryKey);
			SetState();
		}

		protected virtual void OnCboSelectionmade(
			object sender,
			System.EventArgs e)
		{
			if( mbIsLoaded )
			{
				object obj;
				obj = cboSelect.SelectedItem;
				if( obj != null )
				{
					obj = (( BusinessObjectSupport.IListInfo ) obj).GetPrimaryKey();
					DisplayForEdits(e , obj);
				}
			}
			SetState();
		}
		#endregion

		#region Protected Properties and Methods
		protected virtual void DataBindButtons()
		{
			if( ((IEditUserControl)mEditUserControl).CanCreate() )
			{
				Utility.BindField(btnSave , "Enabled" , mObject , "IsValid");
			}
			else
			{
				btnSave.Enabled = false;
			}
		}
		#endregion

		#region Private Properties and Methods
		protected virtual void SetupForm(
			WinSupport.BaseEditUserControl editUserControl,
			WinSupport.BaseSelectUserControl selectUserControl,
			System.Type businessobjecttype,
			System.Windows.Forms.Form parent,
			string title,
			bool showToolbar,
			int openingWidth,
			int openingHeight)
		{
			bool showSelectUC = false;
			int iWidth = 0;
			int iHeight = 0;
			this.mEditUserControl = editUserControl;
			this.mEditUserControl.DataChanged += new BaseEditUserControl.DataChangedHandler(this.uc_DataChanged);
			this.mEditUserControl.BringToFront();
			this.mSelectUserControl = selectUserControl;
			this.mSelectUserControl.SelectionMade += new BaseSelectUserControl.SelectionMadeHandler(this.selectUC_SelectionMade);
			this.mBusinessObjectType = businessobjecttype;
			selectUserControl.Visible = false;
			this.pnlEdit.Controls.Add(editUserControl);
			if( (title == null) || (title.Trim().Length == 0) )
			{
				this.Text = selectUserControl.Caption;
			}
			else
			{
				this.Text = title;
			}
			this.cboSelect.ValueMember = "UniqueKey";
			this.cboSelect.DisplayMember = "DisplayText";
			this.MdiParent = parent;
			this.ToolBar.Visible = showToolbar;
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
			if( iHeight < this.MdiParent.ClientSize.Height )
			{
				iHeight = this.MdiParent.ClientSize.Height;
			}
			this.ClientSize = new System.Drawing.Size(iWidth , iHeight);
			SetState();
		}

		private void SetState()
		{
			IEditUserControl ieuc = (IEditUserControl)this.mEditUserControl;
			EditMode editMode = ieuc.EditMode;
			this.btnDelete.Enabled = ((editMode == EditMode.IsClean) |
				(editMode == EditMode.IsDirty));
			this.btnSave.Enabled = ((editMode & EditMode.IsDirty) > 0);
			this.btnNew.Enabled = ((editMode == EditMode.IsClean) |
				(editMode == EditMode.IsEmpty));
			this.btnCancel.Enabled = ((editMode & EditMode.IsDirty) > 0) |
				((editMode & EditMode.IsNew) > 0);
			this.tbbDelete.Enabled = this.btnDelete.Enabled;
			this.tbbSave.Enabled = this.btnSave.Enabled;
			this.tbbNew.Enabled = this.btnNew.Enabled;
			this.tbbCancel.Enabled = this.btnCancel.Enabled;
		}

		private void ResetDatasource( IBusinessObject ibo )
		{
			string objKey = "";
			if( ibo != null )
			{
				objKey = ibo.UniqueKey;
			}
			this.cboSelect.DataSource = mSelectUserControl.GetList();
			if( ibo != null )
			{
				this.cboSelect.SelectedValue = objKey;
			}
			else if( this.cboSelect.Items.Count > 0 )
			{
				this.cboSelect.SelectedIndex = 0;
			}
			else
			{
				this.cboSelect.SelectedIndex = -1;
			}
			this.OnCboSelectionmade(this.cboSelect , new System.EventArgs());
		}

		private void SetPanels()
		{
			this.pnlSelect.Visible = tbbSearch.Pushed;
		}

		private void DisplayForEdits( System.EventArgs e,  object pk)
		{
			IBusinessObject bo; 
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			bo = (IBusinessObject)Utility.InvokeSharedMethod(
				this.mBusinessObjectType ,
				"Get" + this.mBusinessObjectType.Name ,
				pk);
			mObject = bo;
			IEditUserControl ieuc = (IEditUserControl)this.mEditUserControl;
			ieuc.SetupControl((BusinessObjectSupport.IBusinessObject)bo);
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
		}

		#endregion

 
	}
}