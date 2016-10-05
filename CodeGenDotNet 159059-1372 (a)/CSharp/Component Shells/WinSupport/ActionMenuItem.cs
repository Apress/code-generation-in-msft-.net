// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Action Menu Items are used in dynamic menuing


using System;

namespace KADGen.WinSupport
{
	public class ActionMenuItem : System.Windows.Forms.MenuItem
	{

		private ActionItem mActionItem;

		private const string vbcrlf = Microsoft.VisualBasic.ControlChars.CrLf;

		public ActionMenuItem(
			System.Type BusinessObjectType ,
			System.Type EditUCType ,
			System.Type SelectFormType ,
			System.Windows.Forms.Form MDIParent ) : base()
		{
			mActionItem = new ActionItem(BusinessObjectType , EditUCType , SelectFormType , MDIParent);
			this.Text = mActionItem.Caption;
		}

		protected override void OnClick( System.EventArgs e )
		{
			mActionItem.Edit(e);
		}

	}
}