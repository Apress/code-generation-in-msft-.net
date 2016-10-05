// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Base class for the main MDI parent

using System;
using System.Configuration;
using System.Security.Principal;
using CSLA.Security;
using System.Threading;
//using CSLA.BatchQueue;
using KADGen.BusinessObjectSupport;
using System.Windows.Forms;

namespace KADGen.WinSupport
{
	public class BaseMain : System.Windows.Forms.Form
	{
		private const string vbcrlf = Microsoft.VisualBasic.ControlChars.CrLf;
		private static BaseMain mMain;


		#region  Windows Form Designer generated code 

		public BaseMain() : base()
		{
			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
			InitializeForm();
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
		internal System.Windows.Forms.MainMenu MainMenu1;
		internal System.Windows.Forms.MenuItem MenuItem1; 
		internal System.Windows.Forms.MenuItem mnuFileLogin; 
		internal System.Windows.Forms.MenuItem MenuItem3; 
		internal System.Windows.Forms.MenuItem mnuFileExit; 
		protected internal System.Windows.Forms.MenuItem mnuAction ;
		internal System.Windows.Forms.StatusBar StatusBar1; 
		internal System.Windows.Forms.StatusBarPanel pnlStatus;
		internal System.Windows.Forms.StatusBarPanel pnlUser; 
		protected internal System.Windows.Forms.MenuItem mnuOtherTools; 
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.MainMenu1 = new System.Windows.Forms.MainMenu();
			this.MenuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuFileLogin = new System.Windows.Forms.MenuItem();
			this.MenuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuAction = new System.Windows.Forms.MenuItem();
			this.StatusBar1 = new System.Windows.Forms.StatusBar();
			this.pnlStatus = new System.Windows.Forms.StatusBarPanel();
			this.pnlUser = new System.Windows.Forms.StatusBarPanel();
			this.mnuOtherTools = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)this.pnlStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.pnlUser).BeginInit();
			this.SuspendLayout();
			//
			//MainMenu1
			//
			this.MainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this.MenuItem1 , this.mnuAction , this.mnuOtherTools});
			//
			//MenuItem1
			//
			this.MenuItem1.Index = 0;
			this.MenuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this.mnuFileLogin , this.MenuItem3 , this.mnuFileExit});
			this.MenuItem1.Text = "&File";
			//
			//mnuFileLogin
			//
			this.mnuFileLogin.Index = 0;
			this.mnuFileLogin.Text = "&Login";
			this.mnuFileLogin.Click += new System.EventHandler(this.mnuFileLogin_Click);
			//
			//MenuItem3
			//
			this.MenuItem3.Index = 1;
			this.MenuItem3.Text = "-";
			//
			//mnuFileExit
			//
			this.mnuFileExit.Index = 2;
			this.mnuFileExit.Text = "E&xit";
			this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			//
			//mnuAction
			//
			this.mnuAction.Index = 1;
			this.mnuAction.Text = "&Action";
			//
			//StatusBar1
			//
			this.StatusBar1.Location = new System.Drawing.Point(0 , 384);
			this.StatusBar1.Name = "StatusBar1";
			this.StatusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {this.pnlStatus , this.pnlUser});
			this.StatusBar1.ShowPanels = true;
			this.StatusBar1.Size = new System.Drawing.Size(720 , 22);
			this.StatusBar1.TabIndex = 0;
			this.StatusBar1.Text = "StatusBar1";
			//
			//pnlStatus
			//
			this.pnlStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.pnlStatus.Width = 604;
			//
			//mnuOtherTools
			//
			this.mnuOtherTools.Index = 2;
			this.mnuOtherTools.Text = "Other Tools";
			this.mnuOtherTools.Visible = false;
			//
			//BaseMain
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5 , 13);
			this.ClientSize = new System.Drawing.Size(720 , 406);
			this.Controls.Add(this.StatusBar1);
			this.IsMdiContainer = true;
			this.Menu = this.MainMenu1;
			this.Name = "BaseMain";
			this.Text = "Project Tracker";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			((System.ComponentModel.ISupportInitialize)this.pnlStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.pnlUser).EndInit();
			this.ResumeLayout(false);
		}

		#endregion

		#region  Load and Exit 

		private void InitializeForm()
		{
			mMain = this;
		}

		private void mnuFileExit_Click( object sender ,
			System.EventArgs e )
		{
			Close();
		}

		protected override void OnLoad( System.EventArgs e )
		{
			base.OnLoad(e);
			if( ConfigurationSettings.AppSettings["Authentication"] == "Windows" )
			{
				mnuFileLogin.Visible = false;
				AppDomain.CurrentDomain.SetPrincipalPolicy(
					PrincipalPolicy.WindowsPrincipal);
				BuildMenus();
			}
			else
			{
				DoLogin();
			}
		}


		protected virtual void BuildMenus()
		{
			//System.Type type;
			System.Reflection.Assembly[] asms = BusinessObjectAssemblyList;
			System.Reflection.Assembly asm; 
			System.Type[] types; 
			System.Attribute[] attrs; 
			IBusinessObject bo; 
			WinSupport.ActionMenuItem mnu; 
			System.Type frmSelect; 
			System.Type ucEdit; 
			System.Type selectUCType; 
			System.Type editUCType; 
			System.Type meType  = this.GetType();

			mnuAction.MenuItems.Clear();
			for (int i = 0 ; i <= asms.GetUpperBound(0) ; i++ )
			{
				asm = asms[i];
				types = asm.GetTypes();
				foreach( System.Type type in types )
				{
					if( ! type.IsAbstract )
					{
						attrs = System.Attribute.GetCustomAttributes(type);
						foreach( System.Attribute attr in attrs )
						{
							if( attr is RootAttribute )
							{
								// type belongs in menu
								selectUCType = meType.Assembly.GetType(
									meType.Namespace + "." + type.Name + "SelectUC");
								editUCType = meType.Assembly.GetType(
									meType.Namespace + "." + type.Name + "Edit");
								if( selectUCType != null &&
									 editUCType != null )
								{
									mnu = new WinSupport.ActionMenuItem(type ,
										editUCType , selectUCType , this);
									mnuAction.MenuItems.Add(mnu);
								}
							}
						}
					}
				}
			}
			mnuAction.Enabled = true;
		}


		protected virtual System.Reflection.Assembly[] BusinessObjectAssemblyList 
		{
			get
			{
				throw new System.ApplicationException("You must overrride this method");
			}
		}

		#endregion

		#region  Login/Logout/Authorization 

		private void mnuFileLogin_Click(object sender ,
			System.EventArgs e )
		{
			DoLogin();

		}

		private void DoLogin()
		{
			Login dlg = new Login();

			dlg.ShowDialog(this);
			if( dlg.HasData )
			{
				Cursor = Cursors.WaitCursor;
				BaseMain.SetStatus("Verifying user...");
				BusinessPrincipal.Login(dlg.Username , dlg.Password);
				BaseMain.SetStatus("");
				Cursor = Cursors.Default;

				if( Thread.CurrentPrincipal.Identity.IsAuthenticated )
				{
					pnlUser.Text = Thread.CurrentPrincipal.Identity.Name;
					BuildMenus();
				}
				else
				{
					DoLogout();
					MessageBox.Show("The username and password were not valid" ,
						"Incorrect Password" , MessageBoxButtons.OK ,
						MessageBoxIcon.Exclamation);
				}
			}
			else
			{
				DoLogout();
			}

		}

		private void DoLogout()
		{
			//Can//t set Thread.CurrentPrincipal to null; it does nothing.
			//Instead , create an unauthenticated identity and principal
			// Change from Rocky//s book
			GenericIdentity identity = new GenericIdentity("" , "");
			GenericPrincipal principal = new GenericPrincipal(identity , new string[] {});
			Thread.CurrentPrincipal = principal;

			pnlUser.Text = "";

			BuildMenus();

			try
			{
				//    GetMenuItem("&Logout").Text = "&Login"
			}
			catch
			{
				//
			}

		}

		#endregion

		#region  Status 


		public static void SetStatus(string Text )
		{
			mMain.pnlStatus.Text = Text;
		}


		#endregion

	}
}