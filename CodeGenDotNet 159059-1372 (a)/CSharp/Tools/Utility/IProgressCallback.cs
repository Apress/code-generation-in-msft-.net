// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Interface for calling code that wants status updates

using System;

namespace KADGen.Utility
{
	public interface IProgressCallback
	{
		void UpdateProgress( int complete );
		void UpdateCurrentNode( System.Xml.XmlNode node );
		void UpdateCurrentFile( string file );
		bool GetCancel();
		void ResetCancel();
	}
}
