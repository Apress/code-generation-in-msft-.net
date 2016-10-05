Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class HelloWorldViaCodeDOM

#Region "Class level declarations - empty"
#End Region

#Region "Constructors - empty"
#End Region

#Region "Public Methods and Properties"
   Public Shared Function BuildGraph() As CodeDom.CodeCompileUnit
      Dim CompileUnit As New CodeDom.CodeCompileUnit

      Dim nSpace As New CodeDom.CodeNamespace("HelloWorldViaCodeDOM")
      CompileUnit.Namespaces.Add(nSpace)

      nSpace.Imports.Add(New CodeDom.CodeNamespaceImport("System"))

      Dim clsStartup As New CodeDom.CodeTypeDeclaration("Startup")
      nSpace.Types.Add(clsStartup)

      Dim main As New CodeDom.CodeEntryPointMethod
      Dim exp As New CodeDom.CodePrimitiveExpression("Hello World!")
      Dim refExp As New CodeDom.CodeTypeReferenceExpression("System.Console")
      Dim invoke As New CodeDom.CodeMethodInvokeExpression(refExp, "WriteLine", exp)
      main.Statements.Add(New CodeDom.CodeExpressionStatement(invoke))

      clsStartup.Members.Add(main)

      Return CompileUnit
   End Function
#End Region

#Region "Protected and Friend Methods and Properties - empty"
#End Region

#Region "Private Methods and Properties - empty"
#End Region

End Class
