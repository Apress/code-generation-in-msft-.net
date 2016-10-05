// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: A root class for the Child and Root edit forms
//  Refactor: Move additional code from child and root into this form.

using System;
using System.Windows.Forms;
using KADGen.BusinessObjectSupport;



namespace KADGen.WinSupport
{
	public class BaseEditForm : System.Windows.Forms.Form
	{
		protected enum FormMode
		{
			Root,
			Child,
		}

		//protected mMargin int = 5
		protected Control mBtnLast;
		protected IBusinessObject mObject;
		protected BaseEditUserControl mEditUserControl;
		protected FormMode mFormMode;

		protected void ResizeForm(
			System.Windows.Forms.ToolBar toolBar ,
			BaseSelectUserControl ucSelect ,
			BaseEditUserControl ucEdit ,
			System.Windows.Forms.Panel pnlBottom ,
			System.Windows.Forms.Panel pnlButtons ,
			System.Windows.Forms.Panel pnlSelect )
		{
			int height = 0;
			int top = 0;
			int width = this.ClientSize.Width;
			int vMargin = ((IEditUserControl) ucEdit).VerticalMargin;
			ucEdit.Width = this.ClientSize.Width - pnlButtons.Width;
			if( toolBar != null && toolBar.Visible )
			{
				height += toolBar.Height + vMargin;
				top += toolBar.Bottom + vMargin;
			}
			if( ucSelect != null && ucSelect.Visible )
			{
				height += ucSelect.Height + vMargin;
				ucSelect.Width = width;
				ucSelect.Top = top;
				top += ucSelect.Bottom + vMargin;
			}
			if( pnlSelect != null && pnlSelect.Visible )
			{
				height += pnlSelect.Height + vMargin;
				pnlSelect.Width = width;
				pnlSelect.Top = top;
				top += pnlSelect.Bottom + vMargin;
			}
			pnlBottom.Top = top;
			pnlBottom.Width = width;
			height = ucEdit.Height;
			if( height < mBtnLast.Bottom + vMargin )
			{
				height = mBtnLast.Bottom + vMargin;
			}
			else
			{
				height += 2 * vMargin;
			}
			pnlBottom.Height = height;
			top += pnlBottom.Bottom + vMargin;

			this.ClientSize = new System.Drawing.Size(this.ClientSize.Width , height);
		}

		protected override void OnClosing(
			System.ComponentModel.CancelEventArgs e )
		{
			if( this.mFormMode == FormMode.Root )
			{
				((WinSupport.IEditUserControl) mEditUserControl).OnClosing(e);
			}
		}

	}
}