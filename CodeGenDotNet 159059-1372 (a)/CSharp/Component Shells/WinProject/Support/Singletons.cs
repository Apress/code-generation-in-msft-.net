// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Provides a home for singleton methods 



using System;

namespace KADGen.WinProject
{
	public class Singletons
	{
		public static void SetStatus( string StatusMessage )
		{
			KADGen.WinProject.Main.SetStatus(StatusMessage);
		}

		public static System.Reflection.Assembly GetBusinessObjectAssembly()
		{
			return typeof(KADGen.BusinessObjects.Singletons).Assembly;
		}
	}
}
