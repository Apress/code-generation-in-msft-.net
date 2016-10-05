Option Strict On
Option Explicit On

Imports System

'! Class Summary: Create Hello World program using the CodeDOM

Public Class ClassViaCodeDOM

#Region "Public Methods and Properties"
   Public Shared Function BuildGraph( _
               ByVal xmlMetaData As Xml.XmlDocument, _
               ByVal tableName As String) _
               As CodeDom.CodeCompileUnit
      Dim node As Xml.XmlNode
      Dim nodeList As Xml.XmlNodeList
      Dim compileUnit As New CodeDom.CodeCompileUnit
      Dim nSpace As CodeDom.CodeNamespace
      Dim clsTable As CodeDom.CodeTypeDeclaration

      nodeList = xmlMetaData.SelectNodes("/DataSet/Table[@Name='" & tableName & "']/Column")
      compileUnit.UserData.Add("AllowLateBound", False)
      compileUnit.UserData.Add("RequireVariableDeclaration", True)

      nSpace = New CodeDom.CodeNamespace("ClassViaCodeDOM")
      compileUnit.Namespaces.Add(nSpace)

      nSpace.Imports.Add(New CodeDom.CodeNamespaceImport("System"))

      clsTable = New CodeDom.CodeTypeDeclaration(tableName)
      nSpace.Types.Add(clsTable)

      Dim field As CodeDom.CodeMemberField
      For Each node In nodeList
         field = New CodeDom.CodeMemberField
         field.Name = "m_" & node.Attributes("Name").Value
         field.Attributes = CodeDom.MemberAttributes.Private
         field.Type = New CodeDom.CodeTypeReference(node.Attributes("Type").Value)
         clsTable.Members.Add(field)
      Next

      Dim prop As CodeDom.CodeMemberProperty
      Dim statement As CodeDom.CodePropertySetValueReferenceExpression
      Dim Name As String
      For Each node In nodeList
         prop = New CodeDom.CodeMemberProperty
         Name = node.Attributes("Name").Value
         prop.Name = Name
         prop.Attributes = CodeDom.MemberAttributes.Public
         prop.Type = New CodeDom.CodeTypeReference(node.Attributes("Type").Value)
         prop.GetStatements.Add(New CodeDom.CodeMethodReturnStatement(New CodeDom.CodeFieldReferenceExpression(New CodeDom.CodeThisReferenceExpression, "m_" & Name)))
         prop.SetStatements.Add(New CodeDom.CodeAssignStatement(New CodeDom.CodeFieldReferenceExpression(New CodeDom.CodeThisReferenceExpression, "m_" & Name), New CodeDom.CodePropertySetValueReferenceExpression))
         clsTable.Members.Add(prop)
      Next

      Return compileUnit
   End Function

   Public Shared Sub GenerateCode( _
               ByVal fileName As String, _
               ByVal provider As CodeDom.Compiler.CodeDomProvider, _
               ByVal compileunit As CodeDom.CodeCompileUnit)
      Dim gen As CodeDom.Compiler.ICodeGenerator = provider.CreateGenerator()
      Dim tw As CodeDom.Compiler.IndentedTextWriter
      Try
         tw = New CodeDom.Compiler.IndentedTextWriter(New IO.StreamWriter(fileName, False), "    ")
         gen.GenerateCodeFromCompileUnit(compileunit, tw, New CodeDom.Compiler.CodeGeneratorOptions)
      Finally
         If Not tw Is Nothing Then
            tw.Flush()
            tw.Close()
         End If
      End Try
   End Sub
#End Region

#Region "Protected and Friend Methods and Properties - empty"
#End Region

#Region "Private Methods and Properties - empty"
#End Region

End Class
