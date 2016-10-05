// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Entry point for WinForms application 


using System;
using System.Windows.Forms;

namespace KADGen.WinProject
{
	public class Startup
	{
		public KADGen.WinProject.Main frmFred;
		private static KADGen.WinProject.Main frm ;
		const string vbcrlf = Microsoft.VisualBasic.ControlChars.CrLf;

		public static void Main()
		{
			frm = new KADGen.WinProject.Main();
			GlobalExceptionHandler exceptionHandler = new GlobalExceptionHandler();
			// Adds the event handler to to the event.
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(exceptionHandler.OnThreadException);
			System.Windows.Forms.Application.Run(frm);
		}

		public class GlobalExceptionHandler
		{

			// Handles the exception event.
			public void OnThreadException(object sender , System.Threading.ThreadExceptionEventArgs t )
			{
				DialogResult result = System.Windows.Forms.DialogResult.Cancel;
				try
				{
					result = this.ShowThreadExceptionDialog(t.Exception);
				}
				catch (System.Exception exLocal)
				{
					try
					{
						MessageBox.Show("Fatal Error" + vbcrlf + vbcrlf + t.Exception.ToString(), "Fatal Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
					}
					finally
					{
						Application.Exit();
					}
				}
				// Exits the program when the user clicks Abort.
				if( result == System.Windows.Forms.DialogResult.Abort) 
				{
					Application.Exit();
				}
			}

			// Creates the error message and displays it.
			private System.Windows.Forms.DialogResult ShowThreadExceptionDialog( Exception ex ) 
			{
				ExceptionDisplay frm = new ExceptionDisplay();
				frm.Show(ex);
				return frm.DialogResult;
			}
		}
	}
}
