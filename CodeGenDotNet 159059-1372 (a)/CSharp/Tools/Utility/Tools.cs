// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: The main utility class with miscellaneous tools

using System;

namespace KADGen.Utility
{
	public enum OutputType
	{
		None,
		VB,
		CSharp,
		XML,
		StoredProc
	}
	public enum GenType
	{
		None,
		Once,
		UntilEdited,
		Always,
		Overwrite
	}
	public class Tools
	{
		
		const string vbcrlf = "\r\n";
		const string vbTab = "\t";

		public static string HelloWorld()
		{
			return "Hello World";
		}

		public string HelloWorld2()
		{
			return "Hello World 2";
		}

		public string ToUpper( string s )
		{
			return s.ToUpper();
		}

		public static string SpaceAtCaps( string s )
		{
			Char[] chars = s.ToCharArray();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			for( int i = 0; i <= chars.GetUpperBound(0 ); i++ )
			{

				if( (i > 0 ) && ( IsSpaceChar(chars[i] ) & !IsSpaceChar( chars[i - 1])) )
				{
					sb.Append( " " );
				}
				sb.Append( chars[i] );
			}
			return sb.ToString();
		}
		private static bool IsSpaceChar( Char c )
		{
			return System.Char.IsUpper( c ) | System.Char.IsNumber( c );
		}

		public static string GetAttributeOrEmpty( System.Xml.XmlNode node, string attributeName )
		{
			string ret = "";
			if( node != null )
			{
				if( node.Attributes != null )
				{
					System.Xml.XmlNode attr = node.Attributes.GetNamedItem( attributeName );
					if( attr != null )
					{
						ret = attr.Value;
					}
				}
			}
			return ret;
		}

		public static string FixPath( string sPath, string basePath, string docPath, System.Xml.XmlNode nodeFilePath )
		{
			// Assume any square bracketed things should be in a path def in the XML
			string sStart;
			string sEnd;
			string sRet = sPath;
			string sSearch = "";
			string sNew = "";
			int iStart;
			int iEnd;
			System.Xml.XmlNode node = null;
			System.Xml.XmlNamespaceManager nsmgr = Tools.BuildNamespaceManager( nodeFilePath, "kg", false );
			while( sRet.IndexOf("[" ) >= 0)
			{
				iStart = sRet.IndexOf( "[" );
				iEnd = sRet.IndexOf( "]" );
				sEnd = sRet.Substring( iEnd + 1 );
				sSearch = sRet.Substring( iStart + 1, iEnd - iStart - 1 );
				sStart = sRet.Substring( 0, iStart );
				if( nodeFilePath != null )
				{
					node = nodeFilePath.SelectSingleNode( "kg:FilePath[@Name='" + sSearch + "']", nsmgr );
				}
				if( node != null )
				{
					sNew = System.IO.Path.Combine( Utility.Tools.GetAttributeOrEmpty( node, "Path" ), Utility.Tools.GetAttributeOrEmpty( node, "File" ) );
				}
				else
				{
					sNew = basePath;
				}
				if( sNew.StartsWith("/" ) | sNew.StartsWith( "\\" ) )
				{
					sNew = sNew.Substring( 1 );
				}
				if( sEnd.StartsWith("/" ) | sEnd.StartsWith( "\\" ) )
				{
					sEnd = sEnd.Substring( 1 );
				}
				sRet = System.IO.Path.Combine( System.IO.Path.Combine(sStart, sNew ), sEnd);
				if( docPath != null )
				{
					System.IO.Path.Combine( docPath, sRet );
				}
			}
			//If nodeFilePath != null Then
			//   node = nodeFilePath.SelectSingleNode( '               "kg:FilePath[@Name='" + s + "']", nsmgr )
			//End If
			//If node != null Then
			//   sNew = Utility.Tools.GetAttributeOrEmpty( node, "File" )
			//End If
			//If sRet != null & sNew != null Then
			//   sRet = System.IO.Path.Combine( sRet, sNew )
			//End If
			return sRet;
		}

		public static string FixFilter( string sPath, System.Xml.XmlNode nodeFilter )
		{
			// Assume any square bracketed things should be in a path def in the XML
			string sStart;
			string sEnd;
			string sRet = sPath;
			string sSearch = "";
			int iStart;
			int iEnd;
			System.Xml.XmlNode node = null;
			System.Xml.XmlNamespaceManager nsmgr = Tools.BuildNamespaceManager( nodeFilter, "kg", false );
			// This does NOT support nesting!
			if( sRet.IndexOf("[" ) >= 0 )
			{
				iStart = sRet.IndexOf( "[" );
				iEnd = sRet.IndexOf( "]" );
				sEnd = sRet.Substring( iEnd + 1 );
				sSearch = sRet.Substring( iStart + 1, iEnd - iStart - 1 );
				sStart = sRet.Substring( 0, iStart );
				if( nodeFilter != null )
				{
					node = nodeFilter.SelectSingleNode( "kg:Filter[@Name=\"" + sSearch + "\"]", nsmgr );
				}
				if( node != null )
				{
					sRet = Utility.Tools.GetAttributeOrEmpty( node, "Filter" );
				}
			}
			return sRet;
		}


		public static string GetAttributeOrEmpty( System.Xml.XmlNode node, string ElementName, string attributeName, System.Xml.XmlNamespaceManager nsmgr )
		{
			System.Xml.XmlNode elem;
			if( ElementName.Trim().Length == 0 )
			{
				return GetAttributeOrEmpty( node, attributeName );
			}
			else
			{
				elem = node.SelectSingleNode( ElementName, nsmgr );
				if( elem == null )
				{
					return "";
				}
				else
				{
					return GetAttributeOrEmpty( elem, attributeName );
				}
			}
		}

		public static string GetAnnotOrEmpty( System.Xml.XmlNode node, string attributeName )
		{
			System.Xml.XmlNode nodeTmp;
			nodeTmp = node.SelectSingleNode( "annotation[@" + attributeName + "]" );
			if( nodeTmp != null )
			{
				return Tools.GetAttributeOrEmpty( nodeTmp, attributeName );
			}
			else
			{
				return "";
			}
		}

		public static string GetAnnotOrEmpty( System.Xml.XmlNode node, string attributeName, System.Xml.XmlNamespaceManager nsmgr )
		{
			System.Xml.XmlNode nodeTmp;
			nodeTmp = node.SelectSingleNode( "xs:annotation[@" + attributeName + "]", nsmgr );
			if( nodeTmp != null )
			{
				return Tools.GetAttributeOrEmpty( nodeTmp, attributeName );
			}
			else
			{
				return "";
			}
		}

		public static string GetNamespace( System.Xml.XmlDocument xmlDoc, string prefix )
		{
			System.Xml.XmlNode node = xmlDoc.ChildNodes[1];
			if( prefix.Trim().Length == 0 )
			{
				return GetAttributeOrEmpty( node, "xmlns" );
			}
			else
			{
				return GetAttributeOrEmpty( node, "xmlns:" + prefix );
			}
		}


		public static System.Xml.XmlNamespaceManager BuildNamespaceManager( System.Xml.XmlDocument xmlDoc, System.Xml.XmlNode node, string elemName, System.Xml.XmlNamespaceManager nsmgr, string prefix )
		{
			string namespaceName = GetAttributeOrEmpty( node, elemName, prefix + "Namespace", nsmgr );
			string NSPrefix = GetAttributeOrEmpty( node, elemName, prefix + "NSPrefix", nsmgr );
			System.Xml.XmlNamespaceManager newNsmgr = new System.Xml.XmlNamespaceManager( xmlDoc.NameTable );
			newNsmgr.AddNamespace( NSPrefix, namespaceName );
			return newNsmgr;
		}


		public static System.Xml.XmlNamespaceManager BuildNamespaceManager( System.Xml.XmlNode node, bool includeXSD )
		{
			return BuildNamespaceManager( node, "", includeXSD );
		}

		public static System.Xml.XmlNamespaceManager BuildNamespaceManager( System.Xml.XmlNode node, string prefix, bool includeXSD )
		{
			System.Xml.XmlDocument ownerDocument;
			if( node == null )
			{
				System.Diagnostics.Debug.WriteLine( "oops" );
			}
			else
			{
				if( node is System.Xml.XmlDocument )
				{
					ownerDocument = ((System.Xml.XmlDocument)(node));
				}
				else
				{
					ownerDocument = node.OwnerDocument;
				}
				return BuildNamespaceManager( ownerDocument, prefix, includeXSD );
			}
			return null;
		}

		public static System.Xml.XmlNamespaceManager BuildNamespaceManager( System.Xml.XmlDocument doc, string prefix, bool includeXSD )
		{
			if( doc == null )
			{
				System.Diagnostics.Debug.WriteLine( "oops" );
			}
			else
			{
				System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager( doc.NameTable );
				nsmgr.AddNamespace( prefix, Tools.GetNamespace(doc, prefix ));
				if( includeXSD )
				{
					nsmgr.AddNamespace( "xs", "http://www.w3.org/2001/XMLSchema" );
				}
				return nsmgr;
			}
			return null;
		}

		public static System.Reflection.MethodInfo GetMethodInfo( System.Xml.XmlNode node, System.Type callingType, System.Xml.XmlNamespaceManager nsmgr, string basePath, string docPath, System.Xml.XmlNode nodeFilePath )
		{
			return GetMethodInfo( node, callingType, nsmgr, "", basePath, docPath, nodeFilePath );
		}

		public static System.Reflection.MethodInfo GetMethodInfo( System.Xml.XmlNode node, System.Type callingType, System.Xml.XmlNamespaceManager nsmgr, string elem, string basePath, string docPath, System.Xml.XmlNode nodeFilePath )
		{
			string name;
			// KAD  System.Reflection.Assembly asm;
			System.Type type;
			System.Reflection.MethodInfo methodInfo;
			if( node == null )
			{
			}
			else
			{
				type = GetSpecifiedType( node, callingType, nsmgr, elem, basePath, docPath, nodeFilePath );
				name = Tools.GetAttributeOrEmpty( node, elem, "MethodName", nsmgr );
				if( name.Trim().Length == 0 )
				{
					name = StripNamespacePrefix( Tools.GetAttributeOrEmpty(node, elem, "name", nsmgr ));
				}
				methodInfo = type.GetMethod( name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic );
				return methodInfo;
			}
			return null;
		}

		public static System.Type GetSpecifiedType( System.Xml.XmlNode node, System.Type callingType, System.Xml.XmlNamespaceManager nsmgr, string elem, string basePath, string docPath, System.Xml.XmlNode nodeFilePath )
		{
			string name;
			System.Reflection.Assembly asm;
			System.Type type = null;
			try
			{
				name = Tools.FixPath( Tools.GetAttributeOrEmpty(node, elem, "AssemblyFileName", nsmgr ), basePath, docPath, nodeFilePath);
				if( name.Trim().Length == 0 )
				{
					name = Tools.GetAttributeOrEmpty( node, elem, "AssemblyName", nsmgr );
					if( name.Trim().Length == 0 )
					{
						asm = callingType.Assembly;
					}
					else
					{
						asm = System.Reflection.Assembly.Load( name );
					}
				}
				else
				{
					asm = System.Reflection.Assembly.LoadFrom( name );
				}

				name = Tools.GetAttributeOrEmpty( node, elem, "TypeName", nsmgr );
				if( name.Trim().Length == 0 )
				{
					type = callingType;
				}
				else
				{
					// Kind of ugly but it works
					foreach( System.Type testType in asm.GetTypes() )
					{
						if( testType.Name == name )
						{
							type = testType;
						}
					}
				}
				return type;
			}
			catch
			{
				throw;
			}
		}

		public static string StripNamespacePrefix( string name )
		{
			if( name.IndexOf(":" ) >= 0 )
			{
				return name.Substring( name.IndexOf( ":" ) + 1 );
			}
			else
			{
				return name;
			}
		}

		public static string[] StringArrayFromNodeList( System.Xml.XmlNodeList nodeList )
		{
			return StringArrayFromNodeList( nodeList, "value" );
		}
		public static string[] StringArrayFromNodeList( System.Xml.XmlNodeList nodeList, string attrName )
		{
			string[] ret;
			ret = new string[nodeList.Count];
			for( int i = 0; i <= ret.GetUpperBound(0 ); i++ )
			{
				ret[i] = GetAttributeOrEmpty( nodeList[i], attrName);
			}
			return ret;
		}

		public static string StripPrefix( string s )
		{
			if( s.IndexOf(":" ) >= 0 )
			{
				return s.Substring( s.IndexOf( ":" ) + 1 );
			}
			else
			{
				return s;
			}
		}

		public static string GetSQLTypeFromXSDType( string XSDTypeName )
		{
			string localType = GetLocalPart( XSDTypeName );

			switch( localType.ToLower() )
			{
				case "string":
					return "char";
				case "integer":
					return "int";
				case "boolean":
					return "bit";
				default:
					return localType.ToLower();
			}
		}

		public static string GetSQLTypeFromSQLType( string SQLTypeName )
		{
			// This is kind of ugly, and currently we don't store the 
			// original type, trusting this synonum and case insensitity
			switch( SQLTypeName.ToLower() )
			{
				case "numeric":
					SQLTypeName = "decimal";
					break;
			}
			// This is to fix the case for C#
			SQLTypeName = Enum.Parse( typeof( System.Data.SqlDbType ), SQLTypeName, true ).ToString();
			return SQLTypeName;
		}

		public static string GetNETTypeFromSQLType( string SQLTypeName )
		{
			switch( SQLTypeName.ToLower() )
			{
				case "int":
					return "int";
				case "smallint":
					return "System.Int16";
				case "bigint":
					return "System.Int64";
				case "decimal":
					return "System.Decimal";
				case "base64Binary":
					return "System.Byte()";
				case "boolean":
					return "bool";
				case "dateTime":
					return "System.DateTime";
				case "float":
					return "System.Double";
				case "real":
					return "System.Single";
				case "unsignedByte":
					return "System.Byte";
				case "char":
				case "nchar":
				case "varchar": 
				case "nvarchar":
				case "ntext":
				case "text":
					return "string";
				case "datetime":
				case "smalldatetime":
					return "System.DateTime";
				case "money":
				case "smallmoney":
				case "numeric":
					return "System.Decimal";
				case "bit":
					return "bool";
				case "tinyint":
					return "System.Byte";
				case "timestamp":
					return "System.Byte()";
				case "uniqueidentifier":
					return "System.Guid";
				case "image":
				case "binary":
				case "varbinary":
					return "System.Byte()"; // This is an issue as it is VB or C# specific
				default:
					System.Diagnostics.Debug.WriteLine( "type not found - {0}", SQLTypeName );
					break;
			}
			return null;
		}

		public static string GetPrefix( string name )
		{
			string ret = name;

			for( int i = 0; i <= name.Length - 1; i++ )
			{
            // VBCode is name.Substring( i, 1 ) < "a"
            // BEN Check this if( name.Substring( i, 1 ).CompareTo( "a" ) < 0 )
         if( Char.IsUpper(name, i ) )
         {
					return name.Substring( 0, i );
				}
			}
			return name;
		}

		public static string FixName( string name )
		{
			return FixName( name, false );
		}

		public static string FixName( string name, string removePrefix )
		{
			return FixName( name, (removePrefix.Trim().ToLower() == "true" ) );
		}

		public static string FixName( string name, bool removePrefix )
		{
			string ret = name;
			string keywords;
			// remove any blanks
			if( removePrefix )
			{
				ret = ret.Substring( GetPrefix(ret ).Length);
				//For i As int = 0 To ret.Length - 1
				//   If ret.Substring( i, 1 ) < "a" Then
				//      ret = ret.Substring( i )
				//break
				//   End If
				//Next
			}
			ret = ret.Replace( " ", "_" );

			// replace c# keywords( C# is case sensitive )
			keywords = " abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while ";
			if( keywords.IndexOf(" " + ret + " " ) > 0 )
			{
				ret = "_" + ret;
			}

			// replace vb keywords ( vb is not case sensitive )
			keywords = " addhandler addressof andalso alias and ansi as assembly auto boolean byref byte byval call case catch cbool cbyte cchar cdate cdec cdbl char cint class clng cobj const cshort csng cstr ctype date decimal declare default delegate dim directcast do double each else elseif end enum erase error event exit false finally for friend function get gettype gosub goto handles if implements imports in inherits integer interface is let lib like long loop me mod module mustinherit mustoverride mybase myclass namespace new next not nothing notinheritable notoverridable object on option optional or orelse overloads overridable overrides paramarray preserve private property protected public raiseevent readonly redim rem removehandler resume return select set shadows shared short single static step stop string structure sub synclock then throw to true try typeof unicode until variant when while with withevents writeonly xor ";
			if( keywords.ToLower().IndexOf(" " + ret + " " ) > 0 )
			{
				ret = "_" + ret;
			}
			return ret;
		}

		public static string GetSingular( string name )
		{
			if( name.ToLower().EndsWith("ss" ) | !name.ToLower().EndsWith( "s" ) )
			{
				// This is generally not a plural    
				return name;
			}
			else if( name.ToLower().EndsWith( "us" ) )
			{
				// This is generally not a plural
				return name;
			}
			else if( name.ToLower().EndsWith( "sses" ) )
			{
				return name.Substring( 0, name.Length - 2 );
			}
			else if( name.ToLower().EndsWith( "uses" ) )
			{
				return name.Substring( 0, name.Length - 2 );
			}
			else if( name.ToLower().EndsWith( "ies" ) )
			{
				// NOTE: You can't tell from here if it used to be ey or y. You must
				//       add the specific cases important to you.
				return name.Substring( 0, name.Length - 3 ) + "y";
			}
			else if( name.ToLower().EndsWith( "s" ) )
			{
				return name.Substring( 0, name.Length - 1 );
			}
			else
			{
				return name; // We shouldnt be able to get here
			}
		}


		public static string GetPlural( string name )
		{
			// Simple rules, you may need to expand these. Create new Case entries 
			// above the Else for any exceptions important to you
			string testName = GetSingular( name );
			switch( testName )
			{
				default:
					if( testName.EndsWith( "ey" ) )
					{
						return testName.Substring( 0, testName.Length - 2 ) + "ies";
					}
					else if( testName.EndsWith( "ay" ) )
					{
						return testName.Substring( 0, testName.Length ) + "s";
					}
					else if( testName.EndsWith( "y" ) )
					{
						return testName.Substring( 0, testName.Length - 1 ) + "ies";
					}
					else if( testName.EndsWith( "ss" ) )
					{
						return testName.Substring( 0, testName.Length ) + "es";
					}
					else if( testName.EndsWith( "us" ) )
					{
						return testName.Substring( 0, testName.Length ) + "es";
					}
					else if( testName.EndsWith( "s" ) )
					{
						return testName.Substring( 0, testName.Length - 1 ) + "es";
					}
					else
					{
						return testName + "s";
					}
			}
		}

		public static string GetLocalPart( string s )
		{
			return s.Substring( s.IndexOf( ":" ) + 1 );
		}

		public static System.Xml.XmlNode MakeNewNodeFromXSDNode( System.Xml.XmlDocument xmldoc, string namespaceURI, string prefix, System.Xml.XmlNode xsdElementNode )
		{
			string name = Utility.Tools.GetAttributeOrEmpty( xsdElementNode, "name" );
			string attrName;
			System.Xml.XmlElement dataNode = xmldoc.CreateElement( prefix + ":" + name, namespaceURI );
			System.Xml.XmlNodeList xsdAttrNodeList;
			string attrValue;
			System.Xml.XmlNodeList xsdNodeList;
			System.Xml.XmlNode xsdTypeNode;
			System.Xml.XmlNode xsdAttrTypeNode;
			System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager( xmldoc.NameTable );
			string typeName = Utility.Tools.GetAttributeOrEmpty( xsdElementNode, "type" );
			nsmgr.AddNamespace( "xs", "http://www.w3.org/2001/XMLSchema" );

			if( xsdElementNode.Attributes.GetNamedItem("type" ) == null )
			{
				// This is an anonymous type
				xsdTypeNode = xsdElementNode.SelectSingleNode( "xs:complexType", nsmgr );
			}
			else
			{
				// This is a named type
				// The following is an XPath union
				xsdTypeNode = xsdElementNode.SelectSingleNode( "/xs:schema/xs:complexType[@name='" + typeName + "']", nsmgr );
			}

			if( xsdTypeNode != null )
			{
				xsdAttrNodeList = xsdTypeNode.SelectNodes( "xs:attribute", nsmgr );
				foreach( System.Xml.XmlNode xsdAttrNode in xsdAttrNodeList )
				{
					attrName = Utility.Tools.GetAttributeOrEmpty( xsdAttrNode, "name" );
					typeName = Utility.Tools.GetAttributeOrEmpty( xsdAttrNode, "type" );
					// Cheat on namespace handling
					attrValue = "";
					if( typeName.StartsWith("xs:" ) )
					{
						switch( typeName )
						{
							case "xs:int":
								attrValue = "0";
								break;
							case "xs:boolean":
								attrValue = "false";
								break;
						}
					}
					else
					{
						// A simple type, assume its a simple type of enumeratio, grab first
						xsdAttrTypeNode = xsdElementNode.SelectSingleNode( "/xs:schema/xs:simpleType[@name='" + typeName + "']/xs:restriction/xs:enumeration", nsmgr );
						if( xsdAttrTypeNode != null )
						{
							attrValue = GetAttributeOrEmpty( xsdAttrTypeNode, "value" );
						}
					}
					dataNode.Attributes.Append( Utility.xmlHelpers.NewAttribute( xmldoc, attrName, attrValue ));
				}
				xsdNodeList = xsdTypeNode.SelectNodes( "xs:sequence/xs:element|xs:all/xs:element", nsmgr );
				// I'm not currently supporting choice because I am not sure what you'd do
				foreach( System.Xml.XmlNode xsdChildNode in xsdNodeList )
				{
					dataNode.AppendChild( MakeNewNodeFromXSDNode( xmldoc, namespaceURI, prefix, xsdChildNode ));
				}
			}
			return dataNode;
		}

		public static void MakePathIfNeeded( string fileName )
		{
			string path = System.IO.Path.GetDirectoryName( fileName );
			if( !System.IO.Directory.Exists(path ) )
			{
				System.IO.Directory.CreateDirectory( path );
			}
		}

		public static System.IO.Stream GetStreamFromStringResource( System.Type type, string name )
		{
			System.Reflection.Assembly ass;
			System.IO.Stream stream;
			ass = type.Assembly;
			stream = ass.GetManifestResourceStream( type, name );
			return stream;
		}

		public static string ShowNamespaceManager( System.Xml.XmlNamespaceManager nsmgr )
		{
			string s = "";
			string sThis;
			foreach( object ns in nsmgr )
			{
				sThis = ns.GetType().Name + vbTab;
				if( sThis == null )
				{
					sThis += "null" + vbcrlf;
				}
				else
				{
					sThis += s.ToString() + vbcrlf;
				}
				s += sThis;
			}
			return s;
		}

		public static string SubstringBefore( string searchIn, string searchFor )
		{
			return SubstringBefore( searchIn, searchFor, true );
		}

		public static string SubstringAfter( string searchIn, string searchFor )
		{
			return SubstringAfter( searchIn, searchFor, true );
		}

		public static string SubstringAfterLast( string searchIn, string searchFor )
		{
			return SubstringAfterLast( searchIn, searchFor, true );
		}

		public static string SubstringBefore( string searchIn, string searchFor, bool IsCaseSensitive )
		{
			if( !IsCaseSensitive )
			{
				searchIn = searchIn.ToLower();
				searchFor = searchFor.ToLower();
			}
			int ipos = searchIn.IndexOf( searchFor );
			return searchIn.Substring( 0, ipos );
		}

		public static string SubstringAfter( string searchIn, string searchFor, bool IsCaseSensitive )
		{
			if( !IsCaseSensitive )
			{
				searchIn = searchIn.ToLower();
				searchFor = searchFor.ToLower();
			}
			int ipos = searchIn.IndexOf( searchFor );
			return searchIn.Substring( ipos + searchFor.Length );
		}

		public static string SubstringAfterLast( string searchIn, string searchFor, bool IsCaseSensitive )
		{
			if( !IsCaseSensitive )
			{
				searchIn = searchIn.ToLower();
				searchFor = searchFor.ToLower();
			}
			int ipos = searchIn.LastIndexOf( searchFor );
			return searchIn.Substring( ipos + searchFor.Length );
		}

		public static string SubstringBefore( string searchIn, string[] searchFor )
		{
			return SubstringBefore( searchIn, searchFor, true );
		}

		public static string SubstringAfter( string searchIn, string[] searchFor )
		{
			return SubstringAfter( searchIn, searchFor, true );
		}

		public static string SubstringAfterLast( string searchIn, string[] searchFor )
		{
			return SubstringAfterLast( searchIn, searchFor, true );
		}

		public static bool SubstringContains( string searchIn, string[] searchFor )
		{
			return SubstringContains( searchIn, searchFor, true );
		}

		public static string SubstringContainsWhich( string searchIn, string[] searchFor )
		{
			return SubstringContainsWhich( searchIn, searchFor, true );
		}

		public static string SubstringBefore( string searchIn, string[] searchFor, bool IsCaseSensitive )
		{
			string find;
			if( !IsCaseSensitive )
			{
				searchIn = searchIn.ToLower();
			}
			for( int i = 0; i <= searchFor.GetLength(0 ); i++ )
			{
				if( IsCaseSensitive )
				{
					find = searchFor[i];
				}
				else
				{
					find = searchFor[i].ToLower();
				}
				if( searchIn.IndexOf(find ) > 0 )
				{
					int ipos = searchIn.IndexOf( find );
					return searchIn.Substring( 0, ipos + 1 );
				}
			}
			return "";
		}

		public static string SubstringAfter( string searchIn, string[] searchFor, bool IsCaseSensitive )
		{
			string find;
			if( !IsCaseSensitive )
			{
				searchIn = searchIn.ToLower();
			}
			for( int i = 0; i <= searchFor.GetLength(0 ); i++ )
			{
				if( IsCaseSensitive )
				{
					find = searchFor[i];
				}
				else
				{
					find = searchFor[i].ToLower();
				}
				if( searchIn.IndexOf(find ) > 0 )
				{
					int ipos = searchIn.IndexOf( find );
					return searchIn.Substring( ipos + searchFor.Length );
				}
			}
			return "";
		}

		public static string SubstringAfterLast( string searchIn, string[] searchFor, bool IsCaseSensitive )
		{
			string find;
			if( !IsCaseSensitive )
			{
				searchIn = searchIn.ToLower();
			}
			for( int i = 0; i <= searchFor.GetLength(0 ); i++ )
			{
				if( IsCaseSensitive )
				{
					find = searchFor[i];
				}
				else
				{
					find = searchFor[i].ToLower();
				}
				if( searchIn.IndexOf(find ) > 0 )
				{
					int ipos = searchIn.LastIndexOf( find );
					return searchIn.Substring( ipos + searchFor.Length );
				}
			}
			return "";
		}

		public static bool SubstringContains( string searchIn, string[] searchFor, bool IsCaseSensitive )
		{
			string find;
			if( !IsCaseSensitive )
			{
				searchIn = searchIn.ToLower();
			}
			for( int i = 0; i <= searchFor.GetLength(0 ); i++ )
			{
				if( IsCaseSensitive )
				{
					find = searchFor[i];
				}
				else
				{
					find = searchFor[i].ToLower();
				}
				if( searchIn.IndexOf(find ) > 0 )
				{
					return true;
				}
			}
			return false;
		}

		public static string SubstringContainsWhich( string searchIn, string[] searchFor, bool IsCaseSensitive )
		{
			string find;
			int index;
			int maxIndex = 0;
			int pos = -1;
			if( !IsCaseSensitive )
			{
				searchIn = searchIn.ToLower();
			}
			for( int i = 0; i <= searchFor.GetLength(0 ); i++ )
			{
				if( IsCaseSensitive )
				{
					find = searchFor[i];
				}
				else
				{
					find = searchFor[i].ToLower();
				}
				index = searchIn.IndexOf( find );
				if( index > 0 )
				{
					if( index < maxIndex )
					{
						pos = i;
					}
				}
			}
			return searchFor[pos];
		}

		public static int GetMatchingParenPosition( string s )
		{
			return GetMatchingPunctuation( s, '(', ')');
		}

		public static int GetMatchingPunctuation( string s, Char punc1, Char punc2 )
		{
			int iDepth = 0;
			Char[] chars = s.ToCharArray();
			if( punc1 == punc2 )
			{
				throw new System.ApplicationException( "The GetMatchingPunctuation method doesn't work with matching open and close characters" );
			}
			if( s.IndexOf(punc1 ) >= 0 )
			{
				for( int i = 0; i <= s.Length - 1; i++ )
				{
					if( chars[i] == punc1 )
					{
						iDepth += 1;
					}
					else
					{
						iDepth -= 1;
					}
					if( iDepth == 0 )
					{
						return i;
					}
				}
				throw new System.ApplicationException( "Improperly nested parentheses" );
			}
			else
			{
				return -1;
			}

		}

		public static string CharArrayToString( Char[] chars, int startpos )
		{
			return CharArrayToString( chars, startpos, chars.GetLength(0 ));
		}

		public static string CharArrayToString( Char[] chars )
		{
			return CharArrayToString( chars, 0, chars.GetLength(0 ));
		}

		public static string CharArrayToString( Char[] chars, int startpos, int length )
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for( int i = startpos; i <= length - 1; i++ )
			{
				sb.Append( chars[i] );
			}
			return sb.ToString();
		}

		public static string Test( System.Xml.XmlNode node )
		{
			return "George";
		}

		public static string Test2( System.Xml.XPath.XPathNavigator node )
		{
			return "Ron";
		}

		public static string Test3( System.Xml.XPath.XPathNodeIterator node )
		{
			return "Ginny";
		}

		public static System.Xml.XmlNode TranslateSQLExpression( string constraint, string currentObject, string currentProperty )
		{
			System.Xml.XmlDocument xmlDoc;
			System.Xml.XmlNamespaceManager nsmgr;
	      KADGen.SQLTranslation.TranslateSQL oTranslate = new KADGen.SQLTranslation.TranslateSQL();
			xmlDoc = KADGen.LibraryInterop.Singletons.XMLDoc;
			nsmgr = KADGen.LibraryInterop.Singletons.NsMgr;
			System.Xml.XmlNode node = xmlDoc.CreateElement( "Checkconstraint" );
			node.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "Fred", "true" ) );
			node.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "OriginalClause", constraint ) );
			node.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "VBClause", oTranslate.TranslateSQLToVB( constraint, currentObject, currentProperty, xmlDoc, nsmgr, true ) ) );
			node.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "CSharpClause", oTranslate.TranslateSQLToVCSharp( constraint, currentObject, currentProperty, xmlDoc, nsmgr, true ) ) );
			node.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "EnglishClause", oTranslate.TranslateSQLToEnglish( constraint, currentObject, currentProperty, xmlDoc, nsmgr ) ) );
			return node;

		}
	}
}
