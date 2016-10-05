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
	public class LocalSettings : SettingsBase
	{
		
		//NOTE: I used get functions rather than readonly properties with 
		//      parameters because I planned to convert this code to C#
		const string localNspaceName = "http://kadgen.com/KADGenLocalSettings.xsd";

		protected internal override System.Xml.XmlNode Node
		{
			get
			{
				return base.Node;
			}
			set
			{
				base.Node = value;
				mNsmgr.AddNamespace( "kl", localNspaceName );
			}
		}

		protected internal string GetBasePath()
		{
			if( mNode == null )
			{
				return "";
			}
			else
			{
				System.Xml.XmlNode elem;
				elem = mNode.SelectSingleNode( "kl:BasePath", mNsmgr );
				return Utility.Tools.GetAttributeOrEmpty( elem, "Path" );
			}
		}
	}
}
