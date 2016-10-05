using System;
using System.CodeDom;

namespace CodeDOMExample
{
	/// <summary>
	/// Summary description for CodeDOMExample.
	/// </summary>
	public class CodeDOMExample
	{
		#region Class level declarations - empty
		#endregion

		#region Constructors - empty
		#endregion

		#region Public Methods and Properties
		public static CodeCompileUnit BuildGraph( System.Xml.XmlNode nodeSelect )
		{
			// Create the compile unit
			CodeCompileUnit compileUnit = new CodeCompileUnit();
			try
			{
				// Create some literals
				CodePrimitiveExpression exp = new CodePrimitiveExpression( "Hello World" );
				CodePrimitiveExpression exp2 = new CodePrimitiveExpression( 42 );
				CodePrimitiveExpression exp3 = new CodePrimitiveExpression( 3 );
				string s = "Sam";
				CodePrimitiveExpression exp4 = new CodePrimitiveExpression( s );
				CodePrimitiveExpression exp5 = new CodePrimitiveExpression( true );

				// Declare some objects and create some object references
				CodeVariableDeclarationStatement decl = new CodeVariableDeclarationStatement( typeof( System.Int32 ), "iSum" );
				CodeVariableDeclarationStatement decl2 = new CodeVariableDeclarationStatement( "System.Int32", "iValue", exp2 );
				CodeVariableDeclarationStatement decl3 = new CodeVariableDeclarationStatement( typeof( System.IO.Stream ), "stream" );
				CodeVariableDeclarationStatement decl4 = new CodeVariableDeclarationStatement( typeof( System.String ), "fileName", new CodePrimitiveExpression( "Test.txt" ) );


				// Create the CodeGraph structure and include declarations
				CodeNamespace nSpace = new CodeNamespace( "CodeDOMTest" );
				compileUnit.Namespaces.Add( nSpace );
				// Add an import/ CSharp using statment(just for demonstration)
				nSpace.Imports.Add( new CodeNamespaceImport( "Fred2 = System" ) );
				// Create a class to hold code
				CodeTypeDeclaration clsStartup = new CodeTypeDeclaration( "Startup" );
				nSpace.Types.Add( clsStartup );
				// To run as an executable, you'll need a method that's an entry point
				CodeEntryPointMethod entry = new CodeEntryPointMethod();
				entry.Name = "Main";
				clsStartup.Members.Add( entry );

				// Add Option Strict and Option Explicit for Visual Basic .NET @@@
				compileUnit.UserData.Add( "AllowLateBound", false );
				compileUnit.UserData.Add( "RequireVariableDeclaration", true );

				// Output some code based on earlier declarations and show function usage
				entry.Statements.Add( decl );
				entry.Statements.Add( decl2 );
				entry.Statements.Add( decl3 );
				entry.Statements.Add( decl4 );
				CodeTypeReferenceExpression rExpConsole = new CodeTypeReferenceExpression( 
																	typeof( System.Console ) );
				CodeExpressionStatement stmt1 = new CodeExpressionStatement( 
															new CodeMethodInvokeExpression( rExpConsole, 
																							"WriteLine", 
																							exp ) );
				entry.Statements.Add( stmt1 );
				CodeAssignStatement stmt2 = new CodeAssignStatement(	
													new CodeVariableReferenceExpression( "iSum" ), 
													new CodeBinaryOperatorExpression(	exp2,
																						CodeBinaryOperatorType.Add, 
																						new CodePrimitiveExpression( 23 ) ) );
				entry.Statements.Add( stmt2 );

				// Create an object and assign to a variable accessing an enum
				CodeFieldReferenceExpression enumValue = new CodeFieldReferenceExpression( new CodeTypeReferenceExpression( typeof( System.IO.FileMode ) ), "Create" );
				CodeAssignStatement stmt3 = new CodeAssignStatement( new CodeVariableReferenceExpression( "stream" ), new CodeObjectCreateExpression( typeof( System.IO.FileStream ), new CodeVariableReferenceExpression( "fileName" ), enumValue ) );
				entry.Statements.Add( stmt3 );


				// Declare an array
				entry.Statements.Add( new CodeVariableDeclarationStatement( typeof( System.Int32[] ), "aInts" ) );
				// Shows option for type declaration
				entry.Statements.Add( new CodeVariableDeclarationStatement( new CodeTypeReference( new CodeTypeReference( typeof( System.Int32 ) ), 1 ), "a2Ints" ) );
				CodeVariableReferenceExpression var2AInts = new CodeVariableReferenceExpression( "aInts" );
				entry.Statements.Add( new CodeAssignStatement( new CodeVariableReferenceExpression( "a2Ints" ), new CodeArrayCreateExpression( "System.Int32", 10 ) ) );
				CodeVariableReferenceExpression varAInts = new CodeVariableReferenceExpression( "aInts" );
				entry.Statements.Add( new CodeAssignStatement( varAInts, new CodeArrayCreateExpression( "System.Int32", new CodePrimitiveExpression( 0 ), new CodePrimitiveExpression( 1 ), new CodePrimitiveExpression( 2 ), new CodePrimitiveExpression( 3 ), new CodePrimitiveExpression( 4 ), new CodePrimitiveExpression( 5 ), new CodePrimitiveExpression( 6 ), new CodePrimitiveExpression( 7 ), new CodePrimitiveExpression( 8 ), new CodePrimitiveExpression( 9 ) ) ) );
				// Assign an array value to a variable
				entry.Statements.Add( new CodeAssignStatement( new CodeVariableReferenceExpression( "iValue" ), new CodeArrayIndexerExpression( varAInts, new CodePrimitiveExpression( 3 ) ) ) );

				// Assign move an item down by one in the array
				entry.Statements.Add( new CodeVariableDeclarationStatement( "System.Int32", "i", new CodePrimitiveExpression( 0 ) ) );
				CodeVariableReferenceExpression varI = new CodeVariableReferenceExpression( "i" );
				entry.Statements.Add( new CodeAssignStatement( new CodeArrayIndexerExpression( varAInts, varI ), new CodeArrayIndexerExpression( varAInts, new CodeBinaryOperatorExpression( varI, CodeBinaryOperatorType.Add, new CodePrimitiveExpression( 1 ) ) ) ) );


				// Conditional Statement
				CodeConditionStatement ifBlock = new CodeConditionStatement( new CodeBinaryOperatorExpression( varI, CodeBinaryOperatorType.GreaterThan, new CodePrimitiveExpression( 6 ) ) );
				ifBlock.TrueStatements.Add( WriteLineExpression( "True Executed" ) );
				ifBlock.FalseStatements.Add( WriteLineExpression( "False Executed" ) );
				entry.Statements.Add( ifBlock );

				// Loop using previously defined variable I
				CodeIterationStatement forLoop = new CodeIterationStatement();
				forLoop.InitStatement = new CodeAssignStatement( varI, new CodePrimitiveExpression( 0 ) );
				forLoop.TestExpression = new CodeBinaryOperatorExpression( varI, CodeBinaryOperatorType.LessThanOrEqual, new CodePrimitiveExpression( 9 ) );
				forLoop.IncrementStatement = new CodeAssignStatement( varI, new CodeBinaryOperatorExpression( varI, CodeBinaryOperatorType.Add, new CodePrimitiveExpression( 1 ) ) );
				forLoop.Statements.Add( new CodeExpressionStatement( new CodeMethodInvokeExpression( rExpConsole, "WriteLine", new CodeArrayIndexerExpression( varAInts, varI ) ) ) );
				entry.Statements.Add( forLoop );

				entry.Statements.Add( new CodeExpressionStatement( new CodeMethodInvokeExpression( new CodeMethodReferenceExpression( new CodeVariableReferenceExpression( "obj" ), "MethodA" ), new CodeMethodInvokeExpression( new CodeMethodReferenceExpression( new CodeVariableReferenceExpression( "obj" ), "MethodC" ), new CodeVariableReferenceExpression( "j" ) ) ) ) );


				// Display some weirdnesses
				entry.Statements.Add( new CodeMethodInvokeExpression( new CodeTypeReferenceExpression( "Startup" ), "EqualityDifference" ) );
				clsStartup.Members.Add( equalityDifferenceExample() );
			}
			catch( System.Exception ex )
			{
				throw ex;
			}
			return compileUnit;
		}

		public static CodeTypeMember equalityDifferenceExample()
		{
			CodeMemberMethod mbr = new CodeMemberMethod();
			mbr.Name = "EqualityDifference";
			// Use a line like  the following to create a function returning a value
			// mbr.ReturnType = new CodeTypeReference(GetType(Int32))
			mbr.Attributes = MemberAttributes.Static;
			mbr.Statements.Add(new CodeVariableDeclarationStatement( typeof( int ), "iValue" ) );

			CodeBinaryOperatorExpression bexp = new CodeBinaryOperatorExpression( new System.CodeDom.CodeVariableReferenceExpression( "iValue" ), System.CodeDom.CodeBinaryOperatorType.Assign, new System.CodeDom.CodeBinaryOperatorExpression( new System.CodeDom.CodePrimitiveExpression( 42 ), System.CodeDom.CodeBinaryOperatorType.Add, new System.CodeDom.CodePrimitiveExpression( 23 ) ) );
			CodeConditionStatement ifBlock = new CodeConditionStatement( bexp );
			ifBlock.TrueStatements.Add( WriteLineExpression( "True Executed" ) );
			ifBlock.FalseStatements.Add( WriteLineExpression( "False Executed" ) );
			mbr.Statements.Add( ifBlock );
			return mbr;
		}

		public static CodeStatement WriteLineExpression( object value )
		{
			CodeTypeReferenceExpression refExp = new CodeTypeReferenceExpression( typeof( System.Console ) );
			CodePrimitiveExpression primExp = new CodePrimitiveExpression( value );
			return new CodeExpressionStatement( new System.CodeDom.CodeMethodInvokeExpression( refExp, "WriteLine", primExp ) );
		}
		#endregion

		#region Protected and Friend Methods and Properties - empty
		#endregion

		#region Private Methods and Properties - empty
		#endregion
	}
}
