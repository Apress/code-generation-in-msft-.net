// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Tools to facilitate working with files.

using System;

namespace KADGen.Utility
{
	public class FileTools
	{
		
		public static System.IO.Stream GetFileListXML( string startDir )
		{
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			System.Xml.XmlNode node;
			System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo( startDir );
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			xmlDoc.AppendChild( xmlDoc.CreateXmlDeclaration( "1.0", "utf-8", "yes" ) );
			//node = xmlDoc.CreateElement( "fil:FileLists", "http://kadgen/filelist.xsd" )
			//xmlDoc.AppendChild( node )
			node = xmlDoc.AppendChild( xmlHelpers.NewElement( "fil", "http://kadgen/filelist.xsd", xmlDoc, "FileLists" ) );
			node = node.AppendChild( xmlHelpers.NewElement( "http://kadgen/filelist.xsd", xmlDoc, "fil:FileList" ) );
			node.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "StartDir", startDir ) );
			node.AppendChild( FileListXML( dir, xmlDoc ) );
			xmlDoc.Save( stream );
			return stream;
		}

		private static System.Xml.XmlNode FileListXML( System.IO.DirectoryInfo dir, System.Xml.XmlDocument xmlDoc )
		{
			System.Xml.XmlNode node = xmlHelpers.NewElement( "http://kadgen/filelist.xsd", xmlDoc, "fil:Dir", dir.Name );
			node.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "Name", dir.Name ) );
			System.IO.DirectoryInfo[] dirs;
			System.IO.FileInfo[] files;
			node.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "FullName", dir.FullName ) );
			dirs = dir.GetDirectories();
			files = dir.GetFiles();

			foreach( System.IO.DirectoryInfo d in dirs )
			{
				node.AppendChild( FileListXML( d, xmlDoc ) );
			}

			foreach( System.IO.FileInfo f in files )
			{
				node.AppendChild( FileInfoXML( xmlDoc, f ) );
			}
			return node;
		}

		private static System.Xml.XmlNode FileInfoXML( System.Xml.XmlDocument xmlDoc, System.IO.FileInfo f )
		{
			System.Xml.XmlNode child;
			child = xmlHelpers.NewElement( "http://kadgen/filelist.xsd", xmlDoc, "fil:File", f.Name );
			child.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "Ext", f.Extension ) );
			child.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "FullName", f.FullName ) );
			child.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "Attributes", f.Attributes.ToString() ) );
			child.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "CreationUTC", f.CreationTimeUtc.ToString() ) );
			child.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "LastWriteUTC", f.LastWriteTimeUtc.ToString() ) );
			child.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "Length", f.Length.ToString() ) );
			return child;
		}
	}
}
