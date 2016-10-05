using System;

namespace KADGen.BruteForceExample
{
	/// <summary>
	/// Example file outputting the SimpleDataContainer class
	/// </summary>
	public class BruteForceExample
	{
		public static System.IO.Stream GetStream(	string name,
													string fileName,
													string genDateTime,
													System.Xml.XmlNode nodeSelect)
		{
			System.Console.WriteLine("Test");
			System.Console.ReadLine();
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			System.CodeDom.Compiler.IndentedTextWriter inWriter = new System.CodeDom.Compiler.IndentedTextWriter( new System.IO.StreamWriter( stream ) );

			System.Xml.XmlNodeList nodeList;
			string singularName = Utility.Tools.GetAttributeOrEmpty( nodeSelect, "SingularName" );
			System.Xml.XmlNamespaceManager nsmgr = Utility.Tools.BuildNamespaceManager( nodeSelect.OwnerDocument, "dbs", false );

			Support.FileOpen( inWriter, "KADGen,System", fileName, genDateTime);
			inWriter.WriteLine( "" );
			Support.WriteLineAndIndent( inWriter, "public class " + singularName + "Collection : CollectionBase" );
			CollectionConstructors( inWriter, nsmgr, nodeSelect );
			PublicAndFriend( inWriter, nsmgr, nodeSelect );
			Support.WriteLineAndOutdent( inWriter, "}" );

			inWriter.WriteLine( "" );
			inWriter.WriteLine( "" );
			Support.WriteLineAndIndent( inWriter, "public class " + singularName + " : RowBase" );
			ClassLevelDeclarations( inWriter, nsmgr, nodeSelect);
			Constructors( inWriter, nsmgr, nodeSelect );
			BaseClassImplementation( inWriter, nsmgr, nodeSelect );
			Support.MakeRegion( inWriter, "Field access properties" );
			nodeList = nodeSelect.SelectNodes( "dbs:TableColumns/*", nsmgr );
			foreach( System.Xml.XmlNode nodeColumn in nodeList )
			{
				ColumnMethods( inWriter, nsmgr, nodeColumn );
			}
			Support.EndRegion( inWriter );
			Support.WriteLineAndIndent( inWriter, "End Class" );

			inWriter.Flush();
			return stream;
		}

		public static void CollectionConstructors(	System.CodeDom.Compiler.IndentedTextWriter inWriter,
													System.Xml.XmlNamespaceManager nsmgr,
													System.Xml.XmlNode node )
		{
			Support.MakeRegion( inWriter, "Constructors" );
			string className = Utility.Tools.GetAttributeOrEmpty(node, "SingularName") + "Collection";
			inWriter.WriteLine( "protected " + className + "() : base( \"" + className + "\" )" );
			Support.WriteLineAndIndent( inWriter, "{" );
			Support.WriteLineAndOutdent( inWriter, "}" );
			Support.EndRegion( inWriter );
		}

		public static void PublicAndFriend(	System.CodeDom.Compiler.IndentedTextWriter inWriter,
											System.Xml.XmlNamespaceManager nsmgr,
											System.Xml.XmlNode node )
		{
			Support.MakeRegion( inWriter, "Public and Friend Properties, Methods and Events" );
			System.Xml.XmlNode nodeTemp = node.SelectSingleNode( "dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField", nsmgr );
			string primaryKeyName = "";
			System.Xml.XmlNode primaryKey;
			if( nodeTemp != null )
			{
				primaryKeyName = Utility.Tools.GetAttributeOrEmpty( nodeTemp, "Name" );
			}
			primaryKey = node.SelectSingleNode( "dbs:TableColumns/dbs:TableColumn[@Name='" + primaryKeyName + "']", nsmgr );
			inWriter.WriteLine( "public overloads void Fill( " );
			inWriter.Indent += 4;
			if( primaryKey != null )
			{
				inWriter.WriteLine( Utility.Tools.GetAttributeOrEmpty( primaryKey, "NETType" ) + primaryKeyName + "," );
			}
			inWriter.WriteLine( "int UserID" );
			inWriter.Indent -= 4;
			Support.WriteLineAndIndent( inWriter, "{" );
			inWriter.Write( Utility.Tools.GetAttributeOrEmpty( node, "SingularName" ) + "DataAccessor.Fill( this, " );
			if( primaryKey != null )
			{
				inWriter.Write( primaryKeyName + ", " );
			}
			inWriter.WriteLine( "UserID );" );
			Support.WriteLineAndOutdent( inWriter, "}" );
			Support.EndRegion( inWriter );
		}

		public static void ClassLevelDeclarations(	System.CodeDom.Compiler.IndentedTextWriter inWriter,
													System.Xml.XmlNamespaceManager nsmgr,
													System.Xml.XmlNode node )
		{
			System.Xml.XmlNodeList nodeList;
			string pre = "";
			Support.MakeRegion( inWriter, "Class Level Declarations" );
			inWriter.WriteLine( string.Format( "protected {0} mCollection;", Utility.Tools.GetAttributeOrEmpty( node, "SingularName" ) ) );
			inWriter.WriteLine( "private static int mNextPrimaryKey = -1;" );
			nodeList = node.SelectNodes( "dbs:TableColumns/*", nsmgr );
			inWriter.WriteLine( "" );
			foreach( System.Xml.XmlNode nodeSub in nodeList )
			{
				if( Utility.Tools.GetAttributeOrEmpty(nodeSub, "NETType").Length == 0 )
				{
					pre = "// ";
				}
				inWriter.WriteLine( string.Format( "{0}private {1} {2};", pre, Utility.Tools.GetAttributeOrEmpty(nodeSub, "NETType"), Utility.Tools.GetAttributeOrEmpty(nodeSub, "Name") ) );
			}
			Support.EndRegion( inWriter );
		}
		private static void Constructors(	System.CodeDom.Compiler.IndentedTextWriter inWriter,
											System.Xml.XmlNamespaceManager nsmgr,
											System.Xml.XmlNode node )
		{
			string singularName = Utility.Tools.GetAttributeOrEmpty( node, "SingularName" );
			Support.MakeRegion( inWriter, "Constructors" );
			inWriter.WriteLine( "internal {0}( {1} {1} ) : base()", singularName, singularName+"Collection" );
			Support.WriteLineAndIndent( inWriter, "{" );
			inWriter.WriteLine( "mCollection = " + singularName + "Collection" );
			Support.WriteLineAndOutdent( inWriter, "}" );
			Support.EndRegion( inWriter );
		}
		private static void BaseClassImplementation(	System.CodeDom.Compiler.IndentedTextWriter inWriter,
													System.Xml.XmlNamespaceManager nsmgr,
													System.Xml.XmlNode node )
		{
			Support.MakeRegion( inWriter, "Base Class Implementation" );
			System.Xml.XmlNode nodeTemp = node.SelectSingleNode( "dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField", nsmgr );
			string primaryKeyName = "";
			System.Xml.XmlNode primaryKey;
			if( nodeTemp != null )
			{
				primaryKeyName = Utility.Tools.GetAttributeOrEmpty( nodeTemp, "Name" );
			}
			primaryKey = node.SelectSingleNode( "dbs:TableColumns/dbs:TableColumn[@Name='" + primaryKeyName + "']", nsmgr );
			if( primaryKey!= null )
			{
				if( Utility.Tools.GetAttributeOrEmpty( primaryKey, "IsAutoIncrement" ) == "1" )
				{
					inWriter.WriteLine( "internal void SetNewPrimaryKey()" );
					Support.WriteLineAndIndent( inWriter, "{" );
					inWriter.WriteLine( primaryKeyName + " = mNextPrimaryKey" );
					inWriter.WriteLine( "mNextPrimaryKey --" );
					Support.WriteLineAndOutdent( inWriter, "}" );
				}
			}
			Support.EndRegion( inWriter );
		}
		private static void ColumnMethods(	System.CodeDom.Compiler.IndentedTextWriter inWriter,
			System.Xml.XmlNamespaceManager nsmgr,
			System.Xml.XmlNode node )
		{
			string netTypeName = Utility.Tools.GetAttributeOrEmpty( node, "NETType" );
			string name = Utility.Tools.GetAttributeOrEmpty( node, "Name" );
			if( netTypeName.Trim().Length == 0 )
			{
				inWriter.WriteLine( "" );
				inWriter.WriteLine( "// Column " + name + " is not included because it uses" );
				inWriter.WriteLine( "// a SQLType (" + netTypeName + ") that is not yet supported" );
				inWriter.WriteLine( "" );
			}
			else
			{
				inWriter.WriteLine( "public ColumnInfo " + name + "()" );
				Support.WriteLineAndIndent( inWriter, "{" );
				inWriter.WriteLine( "ColumnInfo columnInfo = new ColumnInfo" );
				inWriter.WriteLine( "columnInfo.FieldName = " + Support.DQ + name + Support.DQ );
				inWriter.WriteLine( "columnInfo.FieldType = GetType(" + netTypeName + ")" );
				inWriter.WriteLine( "columnInfo.SQLType = " + Support.DQ + Utility.Tools.GetAttributeOrEmpty( node, "SQLType" ) + Support.DQ );
				inWriter.WriteLine( "columnInfo.Caption = " + Support.DQ + Utility.Tools.GetAttributeOrEmpty( node, "Caption" ) + Support.DQ );
				inWriter.WriteLine( "columnInfo.Desc = " + Support.DQ + Utility.Tools.GetAttributeOrEmpty( node, "Desc" ) + Support.DQ );
				inWriter.WriteLine( "return columnInfo" );
				Support.WriteLineAndOutdent( inWriter, "}" );

				inWriter.WriteLine( "" );
				inWriter.WriteLine( "public " + netTypeName + " " + name );
				Support.WriteLineAndIndent( inWriter, "{" );
				inWriter.WriteLine( "get" );
				Support.WriteLineAndIndent( inWriter, "{" );
				inWriter.WriteLine( "return m" + name + ";" );
				Support.WriteLineAndOutdent(inWriter, "}");
				inWriter.WriteLine( "set" );
				Support.WriteLineAndIndent( inWriter, "{" );
				inWriter.WriteLine( "m" + name + " = value;" );
				Support.WriteLineAndOutdent( inWriter, "}" );
				Support.WriteLineAndOutdent( inWriter, "}" );
			}

		}
	}
}