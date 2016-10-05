using System;

/// <summary>
/// Create Hello World program using the CodeDOM
/// </summary>
public class ClassViaCodeDOM
{
	#region Public Methods and Properties
	public static System.CodeDom.CodeCompileUnit BuildGraph(	System.Xml.XmlDocument xmlMetaData,
																string tableName )
	{
		System.Xml.XmlNodeList nodeList;
		System.CodeDom.CodeCompileUnit compileUnit = new System.CodeDom.CodeCompileUnit();
		System.CodeDom.CodeNamespace nSpace;
		System.CodeDom.CodeTypeDeclaration clsTable;
		
		nodeList = xmlMetaData.SelectNodes( "/DataSet/Table[@Name='" + tableName + "']/Column" );

		nSpace = new System.CodeDom.CodeNamespace( "ClassViaCodeDOM" );
		compileUnit.Namespaces.Add( nSpace );
		
		nSpace.Imports.Add( new System.CodeDom.CodeNamespaceImport( "System" ) );
		
		clsTable = new System.CodeDom.CodeTypeDeclaration( tableName );
		nSpace.Types.Add( clsTable );

		System.CodeDom.CodeMemberField field;
		foreach( System.Xml.XmlNode node in nodeList )
		{
			field = new System.CodeDom.CodeMemberField();
			field.Name = "m_" + node.Attributes["Name"].Value;
			field.Attributes = System.CodeDom.MemberAttributes.Private;
			field.Type = new System.CodeDom.CodeTypeReference( node.Attributes["Type"].Value );
			clsTable.Members.Add( field );
		}
		
		System.CodeDom.CodeMemberProperty prop;
		string name;
		foreach( System.Xml.XmlNode node in nodeList )
		{
			prop = new System.CodeDom.CodeMemberProperty();
			name = node.Attributes["Name"].Value;
			prop.Name = name;
			prop.Attributes = System.CodeDom.MemberAttributes.Public;
			prop.Type = new System.CodeDom.CodeTypeReference( node.Attributes["Type"].Value );
			prop.GetStatements.Add( new System.CodeDom.CodeMethodReturnStatement( new System.CodeDom.CodeFieldReferenceExpression( new System.CodeDom.CodeThisReferenceExpression(), "m_" + name ) ) );
			prop.SetStatements.Add( new System.CodeDom.CodeAssignStatement( new System.CodeDom.CodeFieldReferenceExpression( new System.CodeDom.CodeThisReferenceExpression(), "m_" + name ), new System.CodeDom.CodePropertySetValueReferenceExpression() ) );
			clsTable.Members.Add( prop );
		}
		return compileUnit;
	}

	public static void GenerateCode(	string fileName, 
										System.CodeDom.Compiler.CodeDomProvider provider,
										System.CodeDom.CodeCompileUnit compileUnit )
	{
		System.CodeDom.Compiler.ICodeGenerator gen = provider.CreateGenerator();
		System.CodeDom.Compiler.IndentedTextWriter tw = null;
		try
		{
			tw = new System.CodeDom.Compiler.IndentedTextWriter( new System.IO.StreamWriter( fileName, false ), "	" );
			gen.GenerateCodeFromCompileUnit( compileUnit, tw, new System.CodeDom.Compiler.CodeGeneratorOptions() );
		}
		finally
		{
			if( tw != null )
			{
				tw.Flush();
				tw.Close();
			}
		}
	}
	#endregion

	#region Protected and Friend Methods and Properties - empty
	#endregion

	#region Private Methods and Properties - empty
	#endregion
}
