// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Home for Singleton methods

namespace KADGen.LibraryInterop
{
	public class Singletons
	{
		private static System.Xml.XmlDocument mXMLDoc;
		public static System.Xml.XmlNamespaceManager NsMgr;
		public static System.Xml.XmlDocument XMLDoc
		{
			get
			{
				return mXMLDoc;
			}
			set
			{
				mXMLDoc = value;
			}
		}
	}
}
