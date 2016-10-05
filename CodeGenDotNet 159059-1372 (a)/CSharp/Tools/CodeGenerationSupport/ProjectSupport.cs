// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Utilties for making project files and moving projects

using System;

namespace KADGen.CodeGenerationSupport
{
	public class ProjectSupport
	{
		#region UpdateProjectFile with new source flies
		public static System.IO.Stream UpdateProjectFile( string projectFileName, string fileListXMLFileName, string directoryList )
		{
			System.Xml.XmlDataDocument prjXML = new System.Xml.XmlDataDocument();
			System.Xml.XmlNodeList nodes;
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			string[] reloadDirs;
			System.Xml.XmlDocument listXML = new System.Xml.XmlDocument();
			System.Xml.XmlNode nodeInclude;
			reloadDirs = SplitToArray( directoryList );
			listXML.Load( fileListXMLFileName );
			prjXML.Load( projectFileName );
			nodeInclude = prjXML.SelectSingleNode( "//Files/Include" );
			nodes = prjXML.SelectNodes( "//Files/Include/*" );

			// Delete files in reload dirs
			foreach( System.Xml.XmlNode node in nodes )
			{
				foreach( string dirName in reloadDirs )
				{
					if( Utility.Tools.GetAttributeOrEmpty(node, "RelPath" ).StartsWith( dirName ) )
					{
						nodeInclude.RemoveChild( node );
					}
				}
			}

			// Add back the files in these directories recursively
			// TODO: Test the following change
			foreach( string dirName in reloadDirs )
			{
				nodes = listXML.SelectNodes( "//FileList//Dir[starts-with(@Name,'" + dirName + "' )]/*" );
				foreach( System.Xml.XmlNode node in nodes )
				{
					if( !(Utility.Tools.GetAttributeOrEmpty(node, "Exclude" ) == "true") )
					{
						LoadVBFiles( node, nodeInclude, Utility.Tools.GetAttributeOrEmpty( node.ParentNode, "Name" ) + "\\");
					}
				}
			}

			prjXML.Save( stream );
			return stream;
		}


		private static void LoadVBFiles( System.Xml.XmlNode fileNode, System.Xml.XmlNode includeNode, string relDir )
		{
			System.Xml.XmlNodeList dirNodes;
			System.Xml.XmlNodeList fileNodes;
			relDir += Utility.Tools.GetAttributeOrEmpty( fileNode, "Name" ) + "\\";
			dirNodes = fileNode.SelectNodes( "Dir" );
			fileNodes = fileNode.SelectNodes( "File[@Ext='.vb' or @Ext='.cs']" );
			foreach( System.Xml.XmlNode dNode in dirNodes )
			{
				LoadVBFiles( dNode, includeNode, relDir );
			}
			foreach( System.Xml.XmlNode fNode in fileNodes )
			{
				includeNode.AppendChild( MakeNewVBIncludeNode( includeNode.OwnerDocument, relDir + Utility.Tools.GetAttributeOrEmpty( fNode, "Name" ), Utility.Tools.GetAttributeOrEmpty( fNode, "Ext" ) ) );
			}
		}

		private static System.Xml.XmlNode MakeNewVBIncludeNode( System.Xml.XmlDocument xmldoc, string fileName, string ext )
		{
         // KAD subtype isn't used
			System.Xml.XmlNode node = xmldoc.CreateElement( "File" );
			string subType = "";
			string buildAction = "";
			node.Attributes.Append( Utility.xmlHelpers.NewAttribute(xmldoc, "RelPath", fileName ));
			switch( ext.ToUpper() )
			{
				case ".VB" :
					subType = "Code";
					buildAction = "Compile";
					break;
				case ".CS":
					subType = "Code";
					buildAction = "Compile";
					break;
			}
			if( subType.Length > 0)
			{
				node.Attributes.Append( Utility.xmlHelpers.NewAttribute(xmldoc, "SubType", subType ));
			}
			if( subType.Length > 0)
			{
				node.Attributes.Append( Utility.xmlHelpers.NewAttribute(xmldoc, "BuildAction", buildAction ));
			}
			return node;
		}


		#endregion

		#region Create new Solution from existing project
		public static System.IO.Stream CreateNewSolution( System.Xml.XmlNode nodeDirective, string rootSourceName, string slnFile, string SkipDirectories, string EmptyDirectories, string basePath )
		{
			// The memory stream will be returned empty as this is a self  
			// contained process and nothing is output
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			string rootTargetName;
			string[] emptyDirArray;
			System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager( nodeDirective.OwnerDocument.NameTable );
         System.Xml.XmlNode nodeFilePaths = Generation.GetFilePathNode();
			nsmgr.AddNamespace( "kg", "http://kadgen.com/KADGenDriving.xsd" );
			System.IO.DirectoryInfo dirSourceRoot;
			System.IO.DirectoryInfo dirTargetRoot;
			rootTargetName = Utility.Tools.GetAttributeOrEmpty( nodeDirective, "kg:SinglePass", "OutputFile", nsmgr );
			rootTargetName = Utility.Tools.FixPath( rootTargetName, basePath, null, nodeFilePaths );
			dirSourceRoot = new System.IO.DirectoryInfo( rootSourceName );
			dirTargetRoot = new System.IO.DirectoryInfo( rootTargetName );

			emptyDirArray = SplitToArray( EmptyDirectories );
			BuildDirectoryStructure( dirSourceRoot, dirTargetRoot, SplitToArray(SkipDirectories ), emptyDirArray);

			if( slnFile.IndexOf(@"\") < 0 ) 
			{
				slnFile = rootSourceName + @"\" + slnFile;
			}

			UpdateProjectAndSolution( slnFile, rootSourceName, rootTargetName, emptyDirArray );

			return stream;
		}

		private static void BuildDirectoryStructure( System.IO.DirectoryInfo dirSourceRoot, System.IO.DirectoryInfo dirTargetRoot, string[] skipDirectories, string[] emptyDirectories )
		{
			System.IO.DirectoryInfo[] dirs;
			//string dirName;
			System.IO.DirectoryInfo dirNew;
			string[] childEmptyArray;
			dirs = dirSourceRoot.GetDirectories();
			foreach( System.IO.DirectoryInfo dir in dirs )
			{
				if( Array.IndexOf(skipDirectories, dir.Name ) < 0 )
				{
					dirNew = dirTargetRoot.CreateSubdirectory( dir.Name );
					dirNew.Create();
					if( ! (emptyDirectories[0] == "*" | Array.IndexOf( emptyDirectories, dir.Name ) >= 0) )
					{
						//Copy the files - there has to be a better way!
						System.IO.FileInfo[] files = dir.GetFiles();
						foreach( System.IO.FileInfo file in files )
						{
							if( file.Extension.ToLower() == ".vbproj" | file.Extension.ToLower() == ".csproj" )
							{
								// Don't copy file now because you can't update project GUID
							}
							else
							{
								file.CopyTo( dirNew.FullName + "\\" + file.Name );
							}
						}
						childEmptyArray = emptyDirectories;
					}
					else
					{
						childEmptyArray = new string[] {"*"};
					}
					BuildDirectoryStructure( dir, dirNew, skipDirectories, childEmptyArray );
				}
			}
		}

		private static void UpdateProjectAndSolution( string slnFile, string sourceDir, string targetDir, string[] emptyDirectories )
		{
			// NOTE: The logic in this routine counts on unique GUIDS
			// Open the file and read project
			System.IO.StreamReader reader = new System.IO.StreamReader( slnFile );
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			System.Xml.XmlNodeList nodes;
			System.IO.StreamWriter writer;
			string sln = reader.ReadToEnd();
			string[] slnLines;
			string[] sAttr;
			string[] fileNames = null;
			string[][] GUIDLookup = null;
			//System.IO.Stream stream;
			string sProj;
			reader.Close();

			slnLines = sln.Split( '\n' );
			foreach( string s in slnLines )
			{
				if( s.Trim().ToLower().StartsWith("project(" ) )
				{
					if( GUIDLookup == null )
					{
						GUIDLookup = new string[1][];
						fileNames = new string[1];
					}
					else
					{
						string[][] GUIDLookupTemp = new string[GUIDLookup.GetLength( 0 ) + 1][];
						GUIDLookup.CopyTo( GUIDLookupTemp, 0 );
						GUIDLookup = GUIDLookupTemp;
						//ReDim Preserve GUIDLookup( GUIDLookup.GetUpperBound( 0 ) + 1);
						string[] fileNamesTemp = new string[fileNames.GetLength( 0 ) + 1];
						fileNames.CopyTo( fileNamesTemp, 0 );
						fileNames = fileNamesTemp;
						//ReDim Preserve fileNames( fileNames.GetUpperBound( 0 ) + 1);
					}
					sAttr = s.Split( ',' );
					// Second position is file, third is GUID  with junk after
					sAttr[2] = sAttr[2].Substring( sAttr[2].IndexOf( "{" ) + 1 );
					sAttr[2] = sAttr[2].Substring( 0, sAttr[2].IndexOf( "}" ) );
					fileNames[fileNames.GetUpperBound( 0 )] = sAttr[1].Replace( "\"", "" ).Trim();
					GUIDLookup[GUIDLookup.GetUpperBound( 0 )] = new string[]{ new Guid( sAttr[2]).ToString(), Guid.NewGuid().ToString() };
				}
			}
			sln = ReplaceArray( sln, GUIDLookup );
         slnFile = System.IO.Path.GetFileName(slnFile);
			writer = new System.IO.StreamWriter( System.IO.Path.Combine(targetDir, slnFile ));
			writer.Write( sln );
			writer.Flush();
			writer.Close();

			foreach( string fileName in fileNames )
			{
				reader = new System.IO.StreamReader( System.IO.Path.Combine( System.IO.Path.Combine( sourceDir, System.IO.Path.GetDirectoryName( slnFile ) ), fileName ) );
				sProj = reader.ReadToEnd();
				reader.Close();
				sProj = ReplaceArray( sProj, GUIDLookup );
				writer = new System.IO.StreamWriter( System.IO.Path.Combine( System.IO.Path.Combine( targetDir, System.IO.Path.GetDirectoryName( slnFile ) ), fileName ) );
				writer.Write( sProj );
				writer.Flush();
				writer.Close();

				xmlDoc.Load( System.IO.Path.Combine( System.IO.Path.Combine( targetDir, System.IO.Path.GetDirectoryName( slnFile ) ), fileName ) );
				foreach( string exclude in emptyDirectories )
				{
					nodes = xmlDoc.SelectNodes( "//Files/Include/File[starts-with(@RelPath,'" + exclude + "' )]" );
					foreach( System.Xml.XmlNode node in nodes )
					{
						node.ParentNode.RemoveChild( node );
					}
				}
				xmlDoc.Save( System.IO.Path.Combine( System.IO.Path.Combine(targetDir, System.IO.Path.GetDirectoryName( slnFile ) ), fileName ) );
			}
		}

		private static void ReplaceArray( string sourceFile, string targetFile, string[][] repArray )
		{
			System.IO.StreamReader reader = new System.IO.StreamReader( sourceFile );
			System.IO.StreamWriter writer = new System.IO.StreamWriter( targetFile );
			string s = reader.ReadToEnd();
			reader.Close();
			s = ReplaceArray( s, repArray );
			writer.Write( s );
			writer.Flush();
			writer.Close();
		}

		private static string ReplaceArray( string sln, string[][] repArray )
		{
			for( int i = 0; i <= repArray.GetUpperBound( 0 ); i++ )
			{
				sln = sln.Replace( repArray[i][0], repArray[i][0] );
				sln = sln.Replace( repArray[i][0].ToLower(), repArray[i][0].ToLower() );
				sln = sln.Replace( repArray[i][0].ToUpper(), repArray[i][0].ToUpper() );
			}
			return sln;
		}

		private static string[] SplitToArray( string s )
		{
			string[] sarr;
			if( s.StartsWith( "{" ) )
			{
				s = s.Substring( 1 );
			}
			if( s.EndsWith( "}" ) )
			{
				s = s.Substring( 0, s.Length - 1 );
			}
			sarr = s.Split( ',' );
			for( int i = 0; i <= sarr.GetUpperBound( 0 ); i++ )
			{
				sarr[i] = sarr[i].Trim();
			}
			return sarr;
		}
		#endregion
	}
}
