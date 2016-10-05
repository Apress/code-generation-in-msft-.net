// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: An abstraction of the information needed for the menu items

using System;
using KADGen.BusinessObjectSupport;


namespace KADGen.WinSupport
{
	public class ActionItem
	{
		protected System.Type mBusinessObjectType;
		protected System.Type mEditUCType;
		protected System.Type mSelectUCType;
		private string mCaption;
		private string mName;
		private System.Windows.Forms.Form mMDIParent;
		private const string vbcrlf = Microsoft.VisualBasic.ControlChars.CrLf;

		internal ActionItem( System.Type BusinessObjectType , 
			System.Type EditUCType , 
			System.Type SelectUCType , 
			System.Windows.Forms.Form MDIParent )
		{
			mBusinessObjectType = BusinessObjectType;
			mEditUCType = EditUCType;
			mSelectUCType = SelectUCType;
			mMDIParent = MDIParent;
			mCaption = (string) Utility.InvokeSharedPropertyGet(mBusinessObjectType , "Caption");
			mName = (string) Utility.InvokeSharedPropertyGet(mBusinessObjectType , "ObjectName");
		}

		protected BaseSelectUserControl NewSelectUserControl() 
		{
			return (BaseSelectUserControl)Utility.CreateInstance(mSelectUCType);
		}

		protected BaseEditUserControl NewEditUserControl() 
		{
			return (BaseEditUserControl)Utility.CreateInstance(mEditUCType); 
		}

		//protected Function NewBusinessObject() CSLA.BusinessBase
		//{
		//   return (CSLA.BusinessBase)this.CreateInstance(mBusinessObjectType);
		//}

		internal bool CanCreate() 
		{
			return (bool) Utility.InvokeSharedMethod(this.mBusinessObjectType , "CanCreate");
		}

		internal bool CanUpdate() 
		{
			return (bool) Utility.InvokeSharedMethod(this.mBusinessObjectType , "CanUpdate");
		}

		internal bool CanDelete() 
		{
			return (bool) Utility.InvokeSharedMethod(this.mBusinessObjectType , "CanDelete");
		}

		internal string Caption
		{
			get
			{
				return mCaption;
			}
		}

		internal string BusinessObjectName
		{
			get
			{
				return (string) Utility.InvokeSharedPropertyGet(mBusinessObjectType , "ObjectName");
			}
		}

		internal void Edit( System.EventArgs e )
		{
			RootEditForm frm = new RootEditForm();
			frm.Show(this.NewEditUserControl() ,
				this.NewSelectUserControl() ,
				this.mBusinessObjectType ,
				mMDIParent , "" , true);
			//mMDIParent)
		}

	}
}