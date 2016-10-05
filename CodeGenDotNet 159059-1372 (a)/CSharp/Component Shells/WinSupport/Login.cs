// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Login form

using System;

namespace KADGen.WinSupport
{
	public class Login : System.Windows.Forms.Form
	{

		#region Windows Form Designer generated code

		public Login() : base()
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
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.TextBox txtUsername;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TextBox txtPassword;
		internal System.Windows.Forms.Button btnLogin;
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.PictureBox PictureBox1;
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Login));
			this.Label1 = new System.Windows.Forms.Label();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.PictureBox1 = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// Label1
			// 
			this.Label1.Location = new System.Drawing.Point(256, 56);
			this.Label1.Name = "Label1";
			this.Label1.TabIndex = 0;
			this.Label1.Text = "Username";
			// 
			// txtUsername
			// 
			this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtUsername.Location = new System.Drawing.Point(328, 56);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(112, 20);
			this.txtUsername.TabIndex = 1;
			this.txtUsername.Text = "";
			this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
			// 
			// Label2
			// 
			this.Label2.Location = new System.Drawing.Point(256, 88);
			this.Label2.Name = "Label2";
			this.Label2.TabIndex = 2;
			this.Label2.Text = "Password";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(328, 88);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(112, 20);
			this.txtPassword.TabIndex = 3;
			this.txtPassword.Text = "";
			// 
			// btnLogin
			// 
			this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLogin.Enabled = false;
			this.btnLogin.Location = new System.Drawing.Point(256, 120);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.TabIndex = 4;
			this.btnLogin.Text = "Login";
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(368, 120);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// PictureBox1
			// 
			this.PictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
			this.PictureBox1.Location = new System.Drawing.Point(0, 0);
			this.PictureBox1.Name = "PictureBox1";
			this.PictureBox1.Size = new System.Drawing.Size(248, 198);
			this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PictureBox1.TabIndex = 6;
			this.PictureBox1.TabStop = false;
			// 
			// Login
			// 
			this.AcceptButton = this.btnLogin;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(472, 198);
			this.ControlBox = false;
			this.Controls.Add(this.PictureBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtUsername);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.Label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Login";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Login";
			this.ResumeLayout(false);

		}

		#endregion

		private string mUsername;
		private string mPassword;
		private bool mHasData;

		private void btnLogin_Click( object sender , System.EventArgs e ) 
		{
			mUsername = txtUsername.Text;
			mPassword = txtPassword.Text;
			mHasData = true;
			Hide();
		}

		private void btnCancel_Click( object sender ,
			System.EventArgs e)
		{
			mUsername = "";
			mPassword = "";
			mHasData = false;
			Hide();

		}

		public string Username
		{
			get
			{
				return mUsername;
			}
		}

		public string Password
		{
			get
			{
				return mPassword;
			}
		}

		public bool HasData 
		{
			get
			{
				return mHasData;
			}
		}

		private void txtUsername_TextChanged( object sender,
			System.EventArgs e )
		{
			btnLogin.Enabled = (txtUsername.Text.Length > 0);

		}

	}
}