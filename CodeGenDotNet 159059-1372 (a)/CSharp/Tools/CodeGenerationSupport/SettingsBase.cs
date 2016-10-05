// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Currently unused code that could be reused to isolate project settings. This proved to complex for most scenarios in testing.

using System;

namespace KADGen.CodeGenerationSupport
{
	public class SettingsBase
	{
		//NOTE: I used get functions rather than readonly properties with 
		//      parameters because I planned to convert this code to C#

		protected System.Xml.XmlNode mNode;
		protected System.Xml.XmlNamespaceManager mNsmgr;
		const string nspaceName = "http://kadgen.com/KADGenDriving.xsd";

		protected internal virtual System.Xml.XmlNode Node
		{
			get
			{
				return mNode;
			}
			set
			{
				mNode = value;
				mNsmgr = new System.Xml.XmlNamespaceManager( mNode.OwnerDocument.NameTable );
				mNsmgr.AddNamespace( "kg", nspaceName );
			}
		}
	}
}
