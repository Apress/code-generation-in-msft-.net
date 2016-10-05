// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Interface for editing user control

using KADGen.BusinessObjectSupport;

namespace KADGen.WinSupport
{
	public interface IEditUserControl
	{
		void SetupControl( IBusinessObject businessObject );
		void SetupControl( IBusinessObject businessObject, string parentPrimaryKey );
		IBusinessObject BusinessObject
		{
			get;
		}

		System.Drawing.Size MinimumSize 
		{
			get;
		}

		int LabelWidth
		{
			get;
		}

		int ControlWidth
		{
			get;
		}

		int ControlTop
		{
			get;
		}

		int ControlLeft
		{
			get;
		}

		int HorizontalMargin
		{
			get;
		}

		int VerticalMargin
		{
			get;
		}

		int IdealHeight
		{
			get;
		}

		int IdealWidth
		{
			get;
		}

		EditMode EditMode
		{
			get;
		}

		void OnClosing( System.ComponentModel.CancelEventArgs e );
		IBusinessObject Save() ;
		void Delete();
		void CancelEdit();
		CSLA.ReadOnlyCollectionBase GetList();
		bool CanCreate();
		bool CanRetrieve(); 
		bool CanUpdate(); 
		bool CanDelete(); 
	}
}