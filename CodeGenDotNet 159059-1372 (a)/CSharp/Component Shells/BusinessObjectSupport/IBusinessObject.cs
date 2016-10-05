// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Interface for BusinessObject Support

namespace KADGen.BusinessObjectSupport
{
	public interface IBusinessObject
	{
		string Caption
		{
			get;
		}

		string ObjectName
		{
			get;
		}

		bool IsDirty
		{
			get;
		}

		bool IsValid
		{
			get;
		}

		string UniqueKey
		{
			get;
			set;
		}

		string DisplayText
		{
			get;
			set;
		}

		void CancelEdit();
		void ApplyEdit();
		IBusinessObject Save();
		IBusinessObject GetNew();
		bool CanUpdate(); 
		bool CanCreate(); 
		bool CanDelete();
		bool CanRetrieve(); 
	}

}
