// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: The main MDI form for the application

using System;
using System.Configuration;
using System.Security.Principal;
using System.Threading;
using KADGen.BusinessObjectSupport;
using System.Windows.Forms;

namespace KADGen.WinProject
{
	public class Main : KADGen.WinSupport.BaseMain
	{
		System.Windows.Forms.ComboBox cbo;
		System.Windows.Forms.TextBox txt;
		System.Windows.Forms.Button btn;
		System.Windows.Forms.RadioButton rdo;
		System.Windows.Forms.GroupBox grp;


		#region  Windows Form Designer generated code 

		public Main() : base()
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
				if( components != null ) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.SuspendLayout();
			//
			//Main
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(720, 427);
			this.IsMdiContainer = true;
			this.Name = "Main";
			this.Text = "Project Tracker";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);
		}

		#endregion

		protected override System.Reflection.Assembly[] BusinessObjectAssemblyList
		{
			get
			{
				return new System.Reflection.Assembly[] {Singletons.GetBusinessObjectAssembly()} ;
			}
		}

		protected override void BuildMenus()
		{
			base.BuildMenus();
			this.mnuOtherTools.Visible = true;
		}
	}
}
