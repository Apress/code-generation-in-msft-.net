using System;

/// <summary>
/// Hello World via the CodeDOM
/// </summary>
public class HelloWorldViaCodeDOM
{
	#region Class level declarations - empty
	#endregion

	#region Constructors - empty
	#endregion

	#region Public Methods and Properties
	public static System.CodeDom.CodeCompileUnit BuildGraph()
	{
		System.CodeDom.CodeCompileUnit CompileUnit = new System.CodeDom.CodeCompileUnit();

		System.CodeDom.CodeNamespace nSpace = new System.CodeDom.CodeNamespace( "HelloWorldViaCodeDOM" );
		CompileUnit.Namespaces.Add( nSpace );

		nSpace.Imports.Add( new System.CodeDom.CodeNamespaceImport( "System" ) );
		
		System.CodeDom.CodeTypeDeclaration clsStartup = new System.CodeDom.CodeTypeDeclaration( "Startup" );
		nSpace.Types.Add( clsStartup );

		System.CodeDom.CodeEntryPointMethod main = new System.CodeDom.CodeEntryPointMethod();
		System.CodeDom.CodePrimitiveExpression exp = new System.CodeDom.CodePrimitiveExpression( "Hello World!" );
		System.CodeDom.CodeTypeReferenceExpression refExp = new System.CodeDom.CodeTypeReferenceExpression( "System.Console" );
		System.CodeDom.CodeMethodInvokeExpression invoke = new System.CodeDom.CodeMethodInvokeExpression( refExp, "WriteLine", exp );
		main.Statements.Add( new System.CodeDom.CodeExpressionStatement( invoke ) );

		clsStartup.Members.Add( main );

		return CompileUnit;
	}
	#endregion

	#region Protected and Friend Methods and Properties - empty
	#endregion

	#region Private Methods and Properties - empty
	#endregion
}
