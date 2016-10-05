// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Enum for the current state of the record

using System;

namespace KADGen.WinSupport
{
[Flags()]
	public enum EditMode
	{
		IsEmpty = 1,
		IsClean = 2,
		IsNew = 4,
		IsDirty = 8,
		IsDeleted = 16,
	}
}