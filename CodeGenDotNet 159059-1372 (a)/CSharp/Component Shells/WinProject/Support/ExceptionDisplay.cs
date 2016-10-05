// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Displays exception details for programmers
//  Refactor: Please provide a friendlier display for your users.


using System;

//! class Summary: 

namespace KADGen.WinProject
{
	public class ExceptionDisplay : System.Windows.Forms.Form
	{

		private enum exType
		{
			Unknown,
			Harness,
			XSLT,
			XML
		}

		#region Class level declarations
		private ExceptionDisplay.StackEntry[] mStackArray;
		private ExceptionDisplay.StackEntry[] mStackArrayShort;
		private System.Xml.XmlDocument mXMLHints;
		private string mExcFullName;
		private string mExcShortName;

		const string vbcrlf = Microsoft.VisualBasic.ControlChars.CrLf;
		#endregion

		#region Windows Form Designer generated code

		public ExceptionDisplay() : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			InitForm();
		}

		//Form overrides dispose to clean up the component list.
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null) 
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
		internal System.Windows.Forms.Splitter Splitter1;
		internal System.Windows.Forms.Panel Panel2;
		internal System.Windows.Forms.TreeView tvExc;
		internal System.Windows.Forms.Splitter Splitter3;
		internal System.Windows.Forms.Panel pnlMessage;
		internal System.Windows.Forms.Panel pnlStack;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.TextBox txtStack;
		internal System.Windows.Forms.TextBox pnlStackGrid;
		internal System.Windows.Forms.Splitter Splitter4;
		internal System.Windows.Forms.DataGrid gridStack;
		internal System.Windows.Forms.TextBox txtMessage;
		internal System.Windows.Forms.Splitter Splitter2;
		internal System.Windows.Forms.Panel pnlWarning;
		internal System.Windows.Forms.TextBox txtWarning;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.ToolTip ToolTip;
		internal System.Windows.Forms.CheckBox chkShowAllStack;
		internal System.Windows.Forms.Button btnAbort;
		internal System.Windows.Forms.Button btnClose;
		internal System.Windows.Forms.Panel pnlButtons;
		internal System.Windows.Forms.Panel pnlGrid;
		internal System.Windows.Forms.ComboBox ComboBox1;
		internal System.Windows.Forms.TextBox TextBox1;
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Panel1 = new System.Windows.Forms.Panel();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnAbort = new System.Windows.Forms.Button();
			this.tvExc = new System.Windows.Forms.TreeView();
			this.Label5 = new System.Windows.Forms.Label();
			this.Splitter2 = new System.Windows.Forms.Splitter();
			this.pnlWarning = new System.Windows.Forms.Panel();
			this.txtWarning = new System.Windows.Forms.TextBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.Splitter1 = new System.Windows.Forms.Splitter();
			this.Panel2 = new System.Windows.Forms.Panel();
			this.pnlGrid = new System.Windows.Forms.Panel();
			this.TextBox1 = new System.Windows.Forms.TextBox();
			this.ComboBox1 = new System.Windows.Forms.ComboBox();
			this.chkShowAllStack = new System.Windows.Forms.CheckBox();
			this.gridStack = new System.Windows.Forms.DataGrid();
			this.Splitter4 = new System.Windows.Forms.Splitter();
			this.pnlStack = new System.Windows.Forms.Panel();
			this.txtStack = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.Splitter3 = new System.Windows.Forms.Splitter();
			this.pnlMessage = new System.Windows.Forms.Panel();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.pnlStackGrid = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.Panel1.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.pnlWarning.SuspendLayout();
			this.Panel2.SuspendLayout();
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridStack)).BeginInit();
			this.pnlStack.SuspendLayout();
			this.pnlMessage.SuspendLayout();
			this.SuspendLayout();
			// 
			// Panel1
			// 
			this.Panel1.Controls.Add(this.pnlButtons);
			this.Panel1.Controls.Add(this.tvExc);
			this.Panel1.Controls.Add(this.Label5);
			this.Panel1.Controls.Add(this.Splitter2);
			this.Panel1.Controls.Add(this.pnlWarning);
			this.Panel1.Controls.Add(this.Label1);
			this.Panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.Panel1.Location = new System.Drawing.Point(0, 0);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(288, 413);
			this.Panel1.TabIndex = 0;
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnClose);
			this.pnlButtons.Controls.Add(this.btnAbort);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlButtons.Location = new System.Drawing.Point(0, 379);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(288, 34);
			this.pnlButtons.TabIndex = 10;
			this.pnlButtons.Layout += new System.Windows.Forms.LayoutEventHandler(this.pnlButtons_Layout);
			// 
			// btnClose
			// 
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnClose.Location = new System.Drawing.Point(176, 0);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(88, 32);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnAbort
			// 
			this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnAbort.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnAbort.Location = new System.Drawing.Point(72, 0);
			this.btnAbort.Name = "btnAbort";
			this.btnAbort.Size = new System.Drawing.Size(88, 32);
			this.btnAbort.TabIndex = 2;
			this.btnAbort.Text = "Abort App";
			this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
			// 
			// tvExc
			// 
			this.tvExc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvExc.ImageIndex = -1;
			this.tvExc.Location = new System.Drawing.Point(0, 234);
			this.tvExc.Name = "tvExc";
			this.tvExc.SelectedImageIndex = -1;
			this.tvExc.Size = new System.Drawing.Size(288, 179);
			this.tvExc.TabIndex = 0;
			this.tvExc.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvExc_AfterSelect);
			// 
			// Label5
			// 
			this.Label5.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.Label5.Dock = System.Windows.Forms.DockStyle.Top;
			this.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.Label5.Location = new System.Drawing.Point(0, 211);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(288, 23);
			this.Label5.TabIndex = 9;
			this.Label5.Text = "Exception Tree";
			// 
			// Splitter2
			// 
			this.Splitter2.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.Splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.Splitter2.Location = new System.Drawing.Point(0, 208);
			this.Splitter2.Name = "Splitter2";
			this.Splitter2.Size = new System.Drawing.Size(288, 3);
			this.Splitter2.TabIndex = 7;
			this.Splitter2.TabStop = false;
			// 
			// pnlWarning
			// 
			this.pnlWarning.AutoScroll = true;
			this.pnlWarning.BackColor = System.Drawing.SystemColors.Info;
			this.pnlWarning.Controls.Add(this.txtWarning);
			this.pnlWarning.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.pnlWarning.ForeColor = System.Drawing.SystemColors.InfoText;
			this.pnlWarning.Location = new System.Drawing.Point(0, 24);
			this.pnlWarning.Name = "pnlWarning";
			this.pnlWarning.Size = new System.Drawing.Size(288, 184);
			this.pnlWarning.TabIndex = 6;
			// 
			// txtWarning
			// 
			this.txtWarning.BackColor = System.Drawing.SystemColors.Info;
			this.txtWarning.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtWarning.ForeColor = System.Drawing.SystemColors.InfoText;
			this.txtWarning.Location = new System.Drawing.Point(0, 0);
			this.txtWarning.Multiline = true;
			this.txtWarning.Name = "txtWarning";
			this.txtWarning.ReadOnly = true;
			this.txtWarning.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtWarning.Size = new System.Drawing.Size(288, 184);
			this.txtWarning.TabIndex = 0;
			this.txtWarning.Text = "";
			// 
			// Label1
			// 
			this.Label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.Label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.Label1.Location = new System.Drawing.Point(0, 0);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(288, 24);
			this.Label1.TabIndex = 8;
			this.Label1.Text = "Overall Problem";
			this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Splitter1
			// 
			this.Splitter1.Location = new System.Drawing.Point(288, 0);
			this.Splitter1.Name = "Splitter1";
			this.Splitter1.Size = new System.Drawing.Size(3, 413);
			this.Splitter1.TabIndex = 1;
			this.Splitter1.TabStop = false;
			// 
			// Panel2
			// 
			this.Panel2.Controls.Add(this.pnlGrid);
			this.Panel2.Controls.Add(this.Splitter4);
			this.Panel2.Controls.Add(this.pnlStack);
			this.Panel2.Controls.Add(this.Label3);
			this.Panel2.Controls.Add(this.Splitter3);
			this.Panel2.Controls.Add(this.pnlMessage);
			this.Panel2.Controls.Add(this.Label2);
			this.Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Panel2.Location = new System.Drawing.Point(291, 0);
			this.Panel2.Name = "Panel2";
			this.Panel2.Size = new System.Drawing.Size(533, 413);
			this.Panel2.TabIndex = 2;
			// 
			// pnlGrid
			// 
			this.pnlGrid.Controls.Add(this.chkShowAllStack);
			this.pnlGrid.Controls.Add(this.gridStack);
			this.pnlGrid.Controls.Add(this.TextBox1);
			this.pnlGrid.Controls.Add(this.ComboBox1);
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.Location = new System.Drawing.Point(0, 188);
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.Size = new System.Drawing.Size(533, 225);
			this.pnlGrid.TabIndex = 9;
			// 
			// TextBox1
			// 
			this.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TextBox1.Location = new System.Drawing.Point(328, 168);
			this.TextBox1.Name = "TextBox1";
			this.TextBox1.TabIndex = 3;
			this.TextBox1.Text = "TextBox1";
			// 
			// ComboBox1
			// 
			this.ComboBox1.Location = new System.Drawing.Point(256, 96);
			this.ComboBox1.Name = "ComboBox1";
			this.ComboBox1.Size = new System.Drawing.Size(121, 21);
			this.ComboBox1.TabIndex = 2;
			this.ComboBox1.Text = "ComboBox1";
			// 
			// chkShowAllStack
			// 
			this.chkShowAllStack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowAllStack.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.chkShowAllStack.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkShowAllStack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkShowAllStack.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.chkShowAllStack.Location = new System.Drawing.Point(424, 0);
			this.chkShowAllStack.Name = "chkShowAllStack";
			this.chkShowAllStack.Size = new System.Drawing.Size(104, 16);
			this.chkShowAllStack.TabIndex = 1;
			this.chkShowAllStack.Text = "Show All";
			this.chkShowAllStack.CheckedChanged += new System.EventHandler(this.chkShowAllStack_CheckedChanged);
			// 
			// gridStack
			// 
			this.gridStack.CausesValidation = false;
			this.gridStack.DataMember = "";
			this.gridStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridStack.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.gridStack.Location = new System.Drawing.Point(0, 0);
			this.gridStack.Name = "gridStack";
			this.gridStack.PreferredColumnWidth = 150;
			this.gridStack.ReadOnly = true;
			this.gridStack.RowHeadersVisible = false;
			this.gridStack.Size = new System.Drawing.Size(533, 225);
			this.gridStack.TabIndex = 0;
			this.gridStack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridStack_MouseMove);
			// 
			// Splitter4
			// 
			this.Splitter4.Dock = System.Windows.Forms.DockStyle.Top;
			this.Splitter4.Location = new System.Drawing.Point(0, 185);
			this.Splitter4.Name = "Splitter4";
			this.Splitter4.Size = new System.Drawing.Size(533, 3);
			this.Splitter4.TabIndex = 8;
			this.Splitter4.TabStop = false;
			// 
			// pnlStack
			// 
			this.pnlStack.Controls.Add(this.txtStack);
			this.pnlStack.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlStack.Location = new System.Drawing.Point(0, 106);
			this.pnlStack.Name = "pnlStack";
			this.pnlStack.Size = new System.Drawing.Size(533, 79);
			this.pnlStack.TabIndex = 4;
			// 
			// txtStack
			// 
			this.txtStack.BackColor = System.Drawing.SystemColors.Control;
			this.txtStack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtStack.ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtStack.Location = new System.Drawing.Point(0, 0);
			this.txtStack.Multiline = true;
			this.txtStack.Name = "txtStack";
			this.txtStack.ReadOnly = true;
			this.txtStack.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtStack.Size = new System.Drawing.Size(533, 79);
			this.txtStack.TabIndex = 0;
			this.txtStack.Text = "";
			// 
			// Label3
			// 
			this.Label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.Label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.Label3.Location = new System.Drawing.Point(0, 83);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(533, 23);
			this.Label3.TabIndex = 7;
			this.Label3.Text = "This Stack";
			// 
			// Splitter3
			// 
			this.Splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.Splitter3.Location = new System.Drawing.Point(0, 80);
			this.Splitter3.Name = "Splitter3";
			this.Splitter3.Size = new System.Drawing.Size(533, 3);
			this.Splitter3.TabIndex = 3;
			this.Splitter3.TabStop = false;
			// 
			// pnlMessage
			// 
			this.pnlMessage.AutoScroll = true;
			this.pnlMessage.Controls.Add(this.txtMessage);
			this.pnlMessage.Controls.Add(this.pnlStackGrid);
			this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlMessage.Location = new System.Drawing.Point(0, 23);
			this.pnlMessage.Name = "pnlMessage";
			this.pnlMessage.Size = new System.Drawing.Size(533, 57);
			this.pnlMessage.TabIndex = 2;
			// 
			// txtMessage
			// 
			this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtMessage.Location = new System.Drawing.Point(0, 0);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtMessage.Size = new System.Drawing.Size(533, 57);
			this.txtMessage.TabIndex = 1;
			this.txtMessage.Text = "";
			// 
			// pnlStackGrid
			// 
			this.pnlStackGrid.BackColor = System.Drawing.SystemColors.Control;
			this.pnlStackGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlStackGrid.ForeColor = System.Drawing.SystemColors.ControlText;
			this.pnlStackGrid.Location = new System.Drawing.Point(0, 0);
			this.pnlStackGrid.Multiline = true;
			this.pnlStackGrid.Name = "pnlStackGrid";
			this.pnlStackGrid.ReadOnly = true;
			this.pnlStackGrid.Size = new System.Drawing.Size(533, 57);
			this.pnlStackGrid.TabIndex = 0;
			this.pnlStackGrid.Text = "";
			// 
			// Label2
			// 
			this.Label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.Label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.Label2.Location = new System.Drawing.Point(0, 0);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(533, 23);
			this.Label2.TabIndex = 6;
			this.Label2.Text = "This Exception Message";
			// 
			// ExceptionDisplay
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnAbort;
			this.ClientSize = new System.Drawing.Size(824, 413);
			this.Controls.Add(this.Panel2);
			this.Controls.Add(this.Splitter1);
			this.Controls.Add(this.Panel1);
			this.Name = "ExceptionDisplay";
			this.Text = "ExceptionDisplay";
			this.Panel1.ResumeLayout(false);
			this.pnlButtons.ResumeLayout(false);
			this.pnlWarning.ResumeLayout(false);
			this.Panel2.ResumeLayout(false);
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridStack)).EndInit();
			this.pnlStack.ResumeLayout(false);
			this.pnlMessage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region "Event Handlers"
		protected override void OnLoad( System.EventArgs e )
		{
			SetTableStyle();
		}

		private void chkShowAllStack_CheckedChanged(
			object sender,
			System.EventArgs e)
		{
			//                 Handles chkShowAllStack.CheckedChanged
			System.Windows.Forms.CheckBox chk;
			if(   sender is System.Windows.Forms.CheckBox )
			{
				chk = ( System.Windows.Forms.CheckBox ) sender;
				if(  chk.Checked )
				{
					gridStack.SetDataBinding(mStackArray, "");
				}
				else
				{
					gridStack.SetDataBinding(mStackArrayShort, "");
				} 
			} 
		}

		private void tvExc_AfterSelect(
			object sender,
			System.Windows.Forms.TreeViewEventArgs e)
		{
			//                 Handles tvExc.AfterSelect
			System.Exception ex  = null;
			System.Windows.Forms.TreeNode tnode;
			System.Collections.ArrayList ar = new System.Collections.ArrayList();
			System.Collections.ArrayList arShort = new System.Collections.ArrayList();
			StackEntry entry;
			try
			{
				tnode = tvExc.SelectedNode;
				if(   tnode != null )
				{
					if(   tnode.Tag is System.Exception )
					{
						ex = (System.Exception) tnode.Tag ;
					} 
				} 
				if( ex != null )
				{
					txtMessage.Text = ex.Message;
					txtStack.Text = ex.StackTrace;
					string[] stack  = ex.StackTrace.Split(Microsoft.VisualBasic.ControlChars.Cr);
					for( int i = 0 ; i <= stack.GetUpperBound(0) ; i++ )
					{
						entry = new StackEntry(stack[i], i);
						ar.Add(entry);
						if(  ! entry.IsFramework )
						{
							arShort.Add(entry);
						} 
					}
				} 
				mStackArray = (StackEntry[]) ar.ToArray(typeof(StackEntry));
				mStackArrayShort = (StackEntry[])arShort.ToArray(typeof(StackEntry));
				gridStack.SetDataBinding(mStackArrayShort, "");
			}
			catch ( System.Exception exception )
			{
				System.Diagnostics.Debug.WriteLine(exception);
			}
		}


		private void gridStack_MouseMove(
			object sender,
			System.Windows.Forms.MouseEventArgs e)
		{
			//                 Handles gridStack.MouseMove
			System.Windows.Forms.DataGrid.HitTestInfo hit;
			hit = gridStack.HitTest(e.X, e.Y);
			StackEntry stackentry = mStackArray[hit.Row];
			string memberName;
			memberName = gridStack.TableStyles[0].GridColumnStyles[hit.Column].MappingName;
			this.ToolTip.SetToolTip(gridStack, stackentry.GetText(memberName));
		}

		private void btnClose_Click(
			object sender,
			System.EventArgs e)
		{
			//              Handles btnClose.Click
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void btnAbort_Click(
			object sender,
			System.EventArgs e)
		{
			//                 Handles btnAbort.Click
			this.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.Close();
		}

		private void pnlButtons_Layout(
			object sender,
			System.Windows.Forms.LayoutEventArgs e)
		{
			//                Handles pnlButtons.Layout

			int width;
			width = pnlButtons.Width / 2;
			btnAbort.Left = 0;
			btnAbort.Width = width;
			btnClose.Left = btnAbort.Right;
			btnClose.Width = width;
		}

		#endregion

		#region public Methods and Properties
		public System.Windows.Forms.DialogResult Show(
			System.Exception ex,
			System.Windows.Forms.Control ctl,
			System.Xml.XmlDocument xmlHints)
		{               
			string warning = FindWarning(ex, xmlHints);
			System.Windows.Forms.TreeNode nodeDeep;
			nodeDeep = LoadTreeNode(ex, tvExc.Nodes);
			txtWarning.Text = warning;
			tvExc.SelectedNode = nodeDeep;
			mXMLHints = xmlHints;
			this.mExcFullName = ex.GetType().FullName;
			this.mExcShortName = ex.GetType().Name;
			return this.ShowDialog();
		}

		public System.Windows.Forms.DialogResult Show(
			System.Exception ex)
		{        
			System.Windows.Forms.TreeNode nodeDeep;
			nodeDeep = LoadTreeNode(ex, tvExc.Nodes);
			tvExc.SelectedNode = nodeDeep;
			this.mExcFullName = ex.GetType().FullName;
			this.mExcShortName = ex.GetType().Name;
			return this.ShowDialog();
		}
		#endregion

		#region protected and internal Methods and Properties-empty
		#endregion

		#region protected Event Response Methods-empty
		#endregion

		#region private Methods and Properties
		private void InitForm()
		{
		}

		private System.Windows.Forms.TreeNode LoadTreeNode(
			System.Exception ex,
			System.Windows.Forms.TreeNodeCollection t)
		{
			System.Windows.Forms.TreeNode tnode = new System.Windows.Forms.TreeNode();
			System.Windows.Forms.TreeNode nodeDeep;
			tnode.Text = ex.GetType().Name;
			tnode.Tag = ex;
			t.Add(tnode);
			if(  ex.InnerException == null )
			{
				return tnode;
			}
			else
			{
				return LoadTreeNode(ex.InnerException, tnode.Nodes);
			} 
		}

		private void LoadTreeXML(
			System.Xml.XmlNode xmlInfo,
			System.Windows.Forms.TreeNodeCollection tv)
		{
			System.Windows.Forms.TreeNode tnode;
			System.Windows.Forms.TreeNode tAttr;
			switch( xmlInfo.NodeType )
			{
				case System.Xml.XmlNodeType.Element:
					tnode = new System.Windows.Forms.TreeNode();
					tnode.Text = "<" + xmlInfo.Name + ">";
					foreach( System.Xml.XmlNode xnode in xmlInfo.ChildNodes)
					{
						LoadTreeXML(xnode, tnode.Nodes);
					}
					foreach( System.Xml.XmlAttribute xAttr in xmlInfo.Attributes)
					{
						tAttr = new System.Windows.Forms.TreeNode();
						tAttr.Text = "@" + xAttr.Name + " = " + xAttr.Value;
						tnode.Nodes.Add(tAttr);
					}
					tv.Add(tnode);
					break;
				case System.Xml.XmlNodeType.Attribute: 
					break;
				default: 
					// just skip it
					break;
			}
		}

		private void SetTableStyle()
		{
			System.Diagnostics.Debug.WriteLine(gridStack.TableStyles.Count);
			System.Windows.Forms.DataGridTableStyle ts = new System.Windows.Forms.DataGridTableStyle();
			//.GridColumnStyles.Clear()
			ts.RowHeadersVisible = false;
			ts.MappingName = "StackEntry[]";
			ts.GridColumnStyles.Add(BuildCS("", "RowNum", 30));
			ts.GridColumnStyles.Add(BuildCS("Location", "MethodShort", 100));
			ts.GridColumnStyles.Add(BuildCS("Line Num", "Position", 40));
			ts.GridColumnStyles.Add(BuildCS("File", "File", 100));
			ts.GridColumnStyles.Add(BuildCS("Long Method Name", "Method", 200));
			ts.GridColumnStyles.Add(BuildCS("Long File Name", "FullFile", 200));
			gridStack.TableStyles.Add(ts);
			gridStack.Refresh();
		}

		public System.Windows.Forms.DataGridColumnStyle BuildCS(
			string HeaderText,
			string MappingName,
			int Width)
		{              
			System.Windows.Forms.DataGridTextBoxColumn cs = new System.Windows.Forms.DataGridTextBoxColumn();
			cs.HeaderText = HeaderText;
			cs.MappingName = MappingName;
			cs.Width = Width;
			return cs;

		}

		private string FindWarning(
			System.Exception ex,
			System.Xml.XmlDocument xmlHints)
		{    
			System.Xml.XmlNodeList nodeList;
			string fullName;
			string shortName;
			System.Exception thisEx = ex;
			string sRet = "";  // Expecting only a few so no stringbuilder
			while ( thisEx != null )
			{
				fullName = thisEx.GetType().FullName;
				shortName = thisEx.GetType().Name;
				nodeList = xmlHints.SelectNodes("//Hint[@FullName=//" + fullName +
					"// or @ShortName=//" + shortName + "//]/HintMessage");
				foreach( System.Xml.XmlNode node in nodeList )
				{
					sRet += node.InnerText + Microsoft.VisualBasic.ControlChars.CrLf;
				}
				thisEx = thisEx.InnerException;
			}
			return sRet;
		}
		#endregion


		public class StackEntry
		{
			private string mMethod;
			private string mMethodShort;
			private string mFile;
			private string mFullFile;
			private string mPosition;
			private string mRowNum;

			public StackEntry( string line, int rowNum )
			{
				ParseLine(line, rowNum);
			}

			public bool IsFramework
			{
				get
				{
					return (mPosition.Trim().Length == 0);
				}
			}

			public string GetText( string memberName ) 
			{
				// We could use reflection, but this in intended for tooltips
				// and this is faster, even if ugly. 
				switch( memberName.ToLower() )
				{
					case "method":
						return Method;
						break;
					case "methodshort":
						return MethodShort;
						break;
					case "file":
						return file;
						break;
					case "fullfile":
						return FullFile;
						break;
					case "position":
						return Position;
						break;
					case "rownum":
						return RowNum;
						break;
					default:
						return "";
						break;
				}
			}

			public void ParseLine(string line, int rowNum)
			{
				string[] arr;
				int iPos;
				line = line.Trim();
				if(  line.StartsWith("at ") )
				{
					line = line.Substring(3);
				} 
				arr = line.Split(')');
				mRowNum = rowNum.ToString();
				mMethod = arr[0];
				mMethodShort = SubstringAfter(SubstringBefore(arr[0], "("), ".");
				if(  arr.GetLength(0) > 1 )
				{
					line = arr[1].Trim();
					if(  line.StartsWith("in ") )
					{
						line = line.Substring(3);
					} 
					mFullFile = line;
					mPosition = SubstringAfter(line, ":").Trim();
					if(  mPosition.StartsWith("line ") )
					{
						mPosition = mPosition.Substring(5);
					} 
					mFile = SubstringAfter(line, @"\");
				} 

			}

			public string Method
			{
				get
				{
					return mMethod;
				}

				set
				{
					mMethod = value;
				}
			}

			public string RowNum
			{
				get
				{
					return mRowNum;
				}
				set
				{
					mRowNum = value;
				}
			}

			public string MethodShort
			{
				get
				{
					return mMethodShort;
				}
				set
				{
					mMethodShort = value;
				}
			}

			public string file
			{
				get
				{
					return mFile;
				}
				set
				{
					mFile = value;
				}
			}

			public string FullFile
			{
				get
				{
					return mFullFile;
				}
				set
				{
					mFullFile = value;
				}
			}

			public string Position
			{
				get
				{
					return mPosition;
				}
				set
				{
					mPosition = value;
				}
			}

			private string SubstringAfter(
				string s,
				string c)
			{
				int ipos;
				ipos = s.LastIndexOf(c);
				if(  ipos >= 0 )
				{
					return s.Substring(ipos + 1);
				}
				else
				{
					return "";
				} 
			}

			private string SubstringBefore(
				string s,
				string c)
			{
				int ipos;
				ipos = s.LastIndexOf(c);
				if(  ipos >= 0 )
				{
					return s.Substring(0, ipos);
				}
				else
				{
					return "";
				} 
			}

		}
	}
}
