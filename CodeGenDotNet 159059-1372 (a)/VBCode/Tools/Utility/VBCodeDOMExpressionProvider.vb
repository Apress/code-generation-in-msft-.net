' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Provides output for SQL translations

Option Explicit On 
Option Strict On

Imports System

Public Class VBCodeDOMExpressionProvider
   Inherits Microsoft.VisualBasic.VBCodeProvider

   Public Overrides Function CreateCompiler() As System.CodeDom.Compiler.ICodeCompiler
      Return Nothing
   End Function

   Public Overloads Overrides Function CreateGenerator() As System.CodeDom.Compiler.ICodeGenerator
      Return New VBCodeDOMExpressionGenerator
   End Function
End Class

Public Class VBCodeDOMExpressionGenerator
   ' inherits Microsoft.VisualBasic.
   Implements CodeDom.Compiler.ICodeGenerator

   Private vbcrlf As String = Microsoft.VisualBasic.ControlChars.CrLf

   Public Function CreateEscapedIdentifier( _
               ByVal value As String) _
               As String _
               Implements System.CodeDom.Compiler.ICodeGenerator.CreateEscapedIdentifier
      Return "[" & value & "]"
   End Function

   Public Function CreateValidIdentifier( _
               ByVal value As String) _
               As String _
               Implements System.CodeDom.Compiler.ICodeGenerator.CreateValidIdentifier
      Return Tools.FixName(value, False)
   End Function

   Public Sub GenerateCodeFromCompileUnit( _
               ByVal e As System.CodeDom.CodeCompileUnit, _
               ByVal w As System.IO.TextWriter, _
               ByVal o As System.CodeDom.Compiler.CodeGeneratorOptions) _
               Implements System.CodeDom.Compiler.ICodeGenerator.GenerateCodeFromCompileUnit
      Dim obj As Object
      Dim iw As CodeDom.compiler.IndentedTextWriter
      If TypeOf w Is CodeDom.Compiler.IndentedTextWriter Then
         iw = CType(w, CodeDom.Compiler.IndentedTextWriter)
      Else
         iw = New CodeDom.Compiler.IndentedTextWriter(w)
      End If

      obj = e.UserData.Item("RequireVariableDeclaration")
      If TypeOf obj Is System.Boolean AndAlso CType(obj, System.Boolean) Then
         iw.WriteLine("Option Explicit On")
      Else
         iw.WriteLine("Option Explicit Off")
      End If

      obj = e.UserData.Item("AllowLateBound")
      If TypeOf obj Is System.Boolean AndAlso CType(obj, System.Boolean) Then
         iw.WriteLine("Option Strict Off")
      Else
         iw.WriteLine("Option Strict On")
      End If

      For Each ns As CodeDom.CodeNamespace In e.Namespaces
         ' All imports must be output at the top of the file in VB.NET
         For Each im As CodeDom.CodeNamespaceImport In ns.imports
            ' Add derived class or userdata for alias's 
            iw.WriteLine("Imports " & im.Namespace)
         Next
      Next

      For Each ns As CodeDom.CodeNamespace In e.Namespaces
         GenerateCodeFromNamespace(ns, iw, o)
      Next

   End Sub

   Public Sub GenerateCodeFromNamespace( _
               ByVal e As System.CodeDom.CodeNamespace, _
               ByVal w As System.IO.TextWriter, _
               ByVal o As System.CodeDom.Compiler.CodeGeneratorOptions) _
               Implements System.CodeDom.Compiler.ICodeGenerator.GenerateCodeFromNamespace
      Dim iw As CodeDom.compiler.IndentedTextWriter
      If TypeOf w Is CodeDom.Compiler.IndentedTextWriter Then
         iw = CType(w, CodeDom.Compiler.IndentedTextWriter)
      Else
         iw = New CodeDom.Compiler.IndentedTextWriter(w)
      End If

      If e.Name.Trim.Length > 0 Then
         iw.WriteLine("Namespace " & e.Name)
         iw.Indent += 1
      End If

      ' Implement Regions as a member of ns.Types
      For Each type As CodeDom.CodeTypeDeclaration In e.Types
         GenerateCodeFromType(type, iw, o)
      Next

      If e.Name.Trim.Length > 0 Then
         iw.Indent -= 1
         iw.WriteLine("End Namespace")
      End If

   End Sub

   Public Sub GenerateCodeFromType( _
               ByVal e As System.CodeDom.CodeTypeDeclaration, _
               ByVal w As System.IO.TextWriter, _
               ByVal o As System.CodeDom.Compiler.CodeGeneratorOptions) _
               Implements System.CodeDom.Compiler.ICodeGenerator.GenerateCodeFromType
      Dim typeText As String
      Dim iw As CodeDom.compiler.IndentedTextWriter
      If TypeOf w Is CodeDom.Compiler.IndentedTextWriter Then
         iw = CType(w, CodeDom.Compiler.IndentedTextWriter)
      Else
         iw = New CodeDom.Compiler.IndentedTextWriter(w)
      End If

      If e.IsEnum Then
         typeText = "Enum"
      ElseIf e.IsInterface Then
         typeText = "Interface"
      ElseIf e.IsStruct Then
         typeText = "Structure"
      Else
         typeText = "Class"
      End If

      For Each c As CodeDom.CodeCommentStatement In e.Comments
         If c.Comment.DocComment Then
            iw.WriteLine("'' " & c.Comment.Text)
         Else
            iw.WriteLine("' " & c.Comment.Text)
         End If
      Next

      If (e.TypeAttributes And Reflection.TypeAttributes.Serializable) <> 0 Then
         iw.WriteLine("<Serializable> _ " & vbcrlf)
      End If

      ' These aren't proper bit flags, so it's critical that the you do the order right
      If (e.TypeAttributes And Reflection.TypeAttributes.NestedFamORAssem) <> 0 Then
         iw.Write("Protected Friend ")
      ElseIf (e.TypeAttributes And Reflection.TypeAttributes.NestedFamANDAssem) <> 0 Then
         iw.Write("NOTSUPPORTED ")
      ElseIf (e.TypeAttributes And Reflection.TypeAttributes.NestedAssembly) <> 0 Then
         iw.Write("Friend ")
      ElseIf (e.TypeAttributes And Reflection.TypeAttributes.NestedFamily) <> 0 Then
         iw.Write("Protected ")
      ElseIf (e.TypeAttributes And Reflection.TypeAttributes.NestedPrivate) <> 0 Then
         iw.Write("Private ")
      ElseIf (e.TypeAttributes And Reflection.TypeAttributes.NestedPublic) <> 0 Then
         iw.Write("Public ")
      ElseIf (e.TypeAttributes And Reflection.TypeAttributes.Public) <> 0 Then
         iw.Write("Public ")
      ElseIf (e.TypeAttributes And Reflection.TypeAttributes.NotPublic) <> 0 Then
         iw.Write("Friend ")
      End If

      If (e.TypeAttributes And Reflection.TypeAttributes.Abstract) <> 0 Then
         iw.Write("MustInherit ")
      End If

      If (e.TypeAttributes And Reflection.TypeAttributes.Sealed) <> 0 Then
         iw.Write("NotInheritable ")
      End If

      iw.WriteLine(typeText)
      iw.Indent += 1

      If e.BaseTypes.Count > 0 Then
         iw.WriteLine("Inherits " & e.BaseTypes(0).BaseType)
         For i As Int32 = 1 To e.BaseTypes.Count - 1
            iw.WriteLine("Implements " & e.BaseTypes(i).BaseType)
         Next
      End If

      For Each m As CodeDom.CodeTypeMember In e.Members
         If TypeOf e Is CodeDom.CodeMemberEvent Then
            GenerateCodeFromMemberEvent(CType(m, CodeDom.CodeMemberEvent), iw, o)
         End If
      Next

      For Each m As CodeDom.CodeTypeMember In e.Members
         If TypeOf e Is CodeDom.CodeMemberField Then
            GenerateCodeFromMemberField(CType(m, CodeDom.CodeMemberField), iw, o)
         End If
      Next

      For Each m As CodeDom.CodeTypeMember In e.Members
         If TypeOf e Is CodeDom.CodeTypeDeclaration Then
            GenerateCodeFromType(CType(m, CodeDom.CodeTypeDeclaration), iw, o)
         End If
      Next

      For Each m As CodeDom.CodeTypeMember In e.Members
         If TypeOf e Is CodeDom.CodeEntryPointMethod Or _
               TypeOf e Is CodeDom.CodeConstructor Or _
               TypeOf e Is CodeDom.CodeTypeConstructor Then
            GenerateCodeFromMemberMethod(CType(m, CodeDom.CodeMemberMethod), iw, o)
         End If
      Next

      For Each m As CodeDom.CodeTypeMember In e.Members
         ' Explore fixing response to New attribute in VB.NET
         If TypeOf e Is CodeDom.CodeMemberMethod And _
               Not (TypeOf e Is CodeDom.CodeEntryPointMethod Or _
               TypeOf e Is CodeDom.CodeConstructor Or _
               TypeOf e Is CodeDom.CodeTypeConstructor) Then
            GenerateCodeFromMemberMethod(CType(m, CodeDom.CodeMemberMethod), iw, o)
         End If
      Next

      iw.Indent -= 1
      iw.WriteLine("End " & typeText)

      iw.WriteLine()
   End Sub

   Public Sub GenerateCodeFromMemberEvent( _
               ByVal e As System.CodeDom.CodeMemberEvent, _
               ByVal iw As CodeDom.Compiler.IndentedTextWriter, _
               ByVal o As System.CodeDom.Compiler.CodeGeneratorOptions)
      ' NOTE: This is not part of the ICodeGenerator interface
      For Each c As CodeDom.CodeCommentStatement In e.Comments
         If c.Comment.DocComment Then
         Else
            iw.WriteLine("'' " & c.Comment.Text)
         End If
      Next
      iw.Write(GetVisibility(e) & "Event " & e.Name & "()")
      ' TODO: More work here on the VB.NET event 
      'e.
   End Sub

   Public Sub GenerateCodeFromMemberField( _
               ByVal e As System.CodeDom.CodeMemberField, _
               ByVal iw As CodeDom.Compiler.IndentedTextWriter, _
               ByVal o As System.CodeDom.Compiler.CodeGeneratorOptions)
      ' NOTE: This is not part of the ICodeGenerator interface
      For Each c As CodeDom.CodeCommentStatement In e.Comments
         If c.Comment.DocComment Then
         Else
            iw.WriteLine("'' " & c.Comment.Text)
         End If
      Next
      iw.Write(GetVisibility(e))
      If (e.Attributes And CodeDom.MemberAttributes.Const) <> 0 Then
         iw.Write("Const ")
      End If
      ' TODO: Manage arrays
      iw.Write(e.Name & " As " & e.Type.BaseType)
      If Not e.InitExpression Is Nothing Then
         GenerateCodeFromExpression(e.InitExpression, iw, o)
      End If
   End Sub

   Public Sub GenerateCodeFromMemberMethod( _
               ByVal e As System.CodeDom.CodeMemberMethod, _
               ByVal iw As CodeDom.Compiler.IndentedTextWriter, _
               ByVal o As System.CodeDom.Compiler.CodeGeneratorOptions)
      ' NOTE: This is not part of the ICodeGenerator interface
      Dim methodType As String
      If e.ReturnType Is Nothing Then
         methodType = "Sub "
      Else
         methodType = "Function "
      End If

      For Each c As CodeDom.CodeCommentStatement In e.Comments
         If c.Comment.DocComment Then
         Else
            iw.WriteLine("'' " & c.Comment.Text)
         End If
      Next
      iw.Write(GetVisibility(e))
      If (e.Attributes And CodeDom.MemberAttributes.Abstract) <> 0 Then
         iw.Write("MustOverride ")
      End If

      If (e.Attributes And CodeDom.MemberAttributes.Override) <> 0 Then
         iw.Write("Overrides ")
         If (e.Attributes And CodeDom.MemberAttributes.Final) <> 0 Then
            iw.Write("NotOverridable ")
         End If
      Else
         If (e.Attributes And CodeDom.MemberAttributes.Final) = 0 Then
            iw.Write("Overridable ")
         End If
      End If

      If (e.Attributes And CodeDom.MemberAttributes.Static) <> 0 Then
         iw.Write("Shared ")
      End If

      iw.Write(methodType)
      iw.Indent += 1

      OutputParameters(e, iw)

      ' TODO: Deal with implementations

      For Each s As CodeDom.CodeStatement In e.Statements
         GenerateCodeFromStatement(s, iw, o)
      Next

      iw.Indent -= 1
      iw.WriteLine("End " & methodType)
   End Sub


   Public Sub GenerateCodeFromStatement( _
               ByVal e As System.CodeDom.CodeStatement, _
               ByVal w As System.IO.TextWriter, _
               ByVal o As System.CodeDom.Compiler.CodeGeneratorOptions) _
               Implements System.CodeDom.Compiler.ICodeGenerator.GenerateCodeFromStatement
      Dim iw As CodeDom.compiler.IndentedTextWriter
      If TypeOf w Is CodeDom.Compiler.IndentedTextWriter Then
         iw = CType(w, CodeDom.Compiler.IndentedTextWriter)
      Else
         iw = New CodeDom.Compiler.IndentedTextWriter(w)
      End If

      If TypeOf e Is System.CodeDom.CodeAssignStatement Then
         With CType(e, CodeDom.CodeAssignStatement)
            GenerateCodeFromExpression(.Left, iw, o)
            iw.Write(" = ")
            GenerateCodeFromExpression(.Left, iw, o)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeAttachEventStatement Then
         With CType(e, CodeDom.CodeAttachEventStatement)
            ' TODO: Attach Events 
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeCommentStatement Then
         With CType(e, CodeDom.CodeCommentStatement)
            If .Comment.DocComment Then
               iw.WriteLine("'' ", .Comment.Text)
            Else
               iw.WriteLine("' ", .Comment.Text)
            End If
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeConditionStatement Then
         With CType(e, CodeDom.CodeConditionStatement)
            iw.Write("If ")
            GenerateCodeFromExpression(.Condition, iw, o)
            iw.WriteLine("Then")
            iw.Indent += 1
            For Each s As CodeDom.CodeStatement In .TrueStatements
               GenerateCodeFromStatement(s, iw, o)
            Next
            iw.Indent -= 1
            iw.WriteLine("Else")
            iw.Indent += 1
            For Each s As CodeDom.CodeStatement In .TrueStatements
               GenerateCodeFromStatement(s, iw, o)
            Next
            iw.Indent -= 1
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeExpressionStatement Then
         With CType(e, CodeDom.CodeExpressionStatement)
            GenerateCodeFromExpression(.Expression, iw, o)
            iw.WriteLine("")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeGotoStatement Then
         With CType(e, CodeDom.CodeGotoStatement)
            ' TODO: Are you going to support Goto?
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeIterationStatement Then
         With CType(e, CodeDom.CodeIterationStatement)
            ' TODO: Figure out loops
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeLabeledStatement Then
         With CType(e, CodeDom.CodeLabeledStatement)
            ' TODO: Are you going to support labels?
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeMethodReturnStatement Then
         With CType(e, CodeDom.CodeMethodReturnStatement)
            iw.Write("Return ")
            GenerateCodeFromExpression(.Expression, iw, o)
            iw.WriteLine("")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeRemoveEventStatement Then
         With CType(e, CodeDom.CodeRemoveEventStatement)
            ' TODO: Attach Events 
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeSnippetStatement Then
         With CType(e, CodeDom.CodeSnippetStatement)
            ' TODO: Are you going to support snippets? 
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeThrowExceptionStatement Then
         With CType(e, CodeDom.CodeThrowExceptionStatement)
            iw.Write("Throw ")
            GenerateCodeFromExpression(.ToThrow, iw, o)
            iw.WriteLine("")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeTryCatchFinallyStatement Then
         With CType(e, CodeDom.CodeTryCatchFinallyStatement)
            iw.WriteLine("Try ")
            iw.Indent += 1
            For Each s As CodeDom.CodeStatement In .TryStatements
               GenerateCodeFromStatement(s, iw, o)
            Next
            iw.Indent -= 1
            For Each tc As CodeDom.CodeCatchClause In .CatchClauses
               iw.WriteLine("Catch " & tc.CatchExceptionType.BaseType)
               iw.Indent += 1
               For Each s As CodeDom.CodeStatement In tc.Statements
                  GenerateCodeFromStatement(s, iw, o)
               Next
               iw.Indent -= 1
            Next
            iw.Indent += 1
            iw.WriteLine("Finally")
            For Each s As CodeDom.CodeStatement In .FinallyStatements
               GenerateCodeFromStatement(s, iw, o)
            Next
            iw.Indent -= 1
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeVariableDeclarationStatement Then
         With CType(e, CodeDom.CodeVariableDeclarationStatement)
            iw.Write("Dim " & .Name & " As " & .Type.BaseType)
            GenerateCodeFromExpression(.InitExpression, iw, o)
            iw.WriteLine("")
         End With
      End If

   End Sub

   Public Sub GenerateCodeFromExpression( _
               ByVal e As System.CodeDom.CodeExpression, _
               ByVal w As System.IO.TextWriter, _
               ByVal o As System.CodeDom.Compiler.CodeGeneratorOptions) _
               Implements System.CodeDom.Compiler.ICodeGenerator.GenerateCodeFromExpression
      Dim iw As CodeDom.compiler.IndentedTextWriter
      If TypeOf w Is CodeDom.Compiler.IndentedTextWriter Then
         iw = CType(w, CodeDom.Compiler.IndentedTextWriter)
      Else
         iw = New CodeDom.Compiler.IndentedTextWriter(w)
      End If

      If TypeOf e Is System.CodeDom.CodeArgumentReferenceExpression Then
         With CType(e, CodeDom.CodeArgumentReferenceExpression)
            iw.Write(.ParameterName)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeArrayCreateExpression Then
         With CType(e, CodeDom.CodeArrayCreateExpression)
            ' TODO: Deal with arrays 
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeArrayIndexerExpression Then
         With CType(e, CodeDom.CodeArrayIndexerExpression)
            GenerateCodeFromExpression(.TargetObject, iw, o)
            iw.Write("(")
            For i As Int32 = 0 To .Indices.Count - 1
               If i > 0 Then
                  iw.Write(",")
               End If
               GenerateCodeFromExpression(.Indices(i), iw, o)
            Next
            iw.Write(")")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeBaseReferenceExpression Then
         With CType(e, CodeDom.CodeBaseReferenceExpression)
            iw.Write("MyBase")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeBinaryOperatorExpression Then
         With CType(e, CodeDom.CodeBinaryOperatorExpression)
            If .Operator = CodeDom.CodeBinaryOperatorType.IdentityInequality Then
               iw.Write(" Not ")
               .Operator = CodeDom.CodeBinaryOperatorType.ValueEquality
            End If
            GenerateCodeFromExpression(.Left, iw, o)
            Select Case .Operator
               Case CodeDom.CodeBinaryOperatorType.Add
                  iw.Write("+")
               Case CodeDom.CodeBinaryOperatorType.Assign
                  iw.Write("=")
               Case CodeDom.CodeBinaryOperatorType.BitwiseAnd
                  iw.Write(" And ")
               Case CodeDom.CodeBinaryOperatorType.BitwiseOr
                  iw.Write(" Or ")
               Case CodeDom.CodeBinaryOperatorType.BooleanAnd
                  iw.Write(" AndAlso ")
               Case CodeDom.CodeBinaryOperatorType.BooleanOr
                  iw.Write(" OrElse ")
               Case CodeDom.CodeBinaryOperatorType.Divide
                  iw.Write("/")
               Case CodeDom.CodeBinaryOperatorType.GreaterThan
                  iw.Write(">")
               Case CodeDom.CodeBinaryOperatorType.GreaterThanOrEqual
                  iw.Write(">=")
               Case CodeDom.CodeBinaryOperatorType.IdentityEquality
                  iw.Write(" Is ")
               Case CodeDom.CodeBinaryOperatorType.IdentityInequality
                  iw.Write(" NOTSUPPORTED ")
               Case CodeDom.CodeBinaryOperatorType.LessThan
                  iw.Write("<")
               Case CodeDom.CodeBinaryOperatorType.LessThanOrEqual
                  iw.Write("<=")
               Case CodeDom.CodeBinaryOperatorType.Modulus
                  iw.Write(" Mod ")
               Case CodeDom.CodeBinaryOperatorType.Multiply
                  iw.Write("*")
               Case CodeDom.CodeBinaryOperatorType.Subtract
                  iw.Write("-")
               Case CodeDom.CodeBinaryOperatorType.ValueEquality
                  iw.Write("=")
            End Select
            GenerateCodeFromExpression(.Right, iw, o)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeCastExpression Then
         With CType(e, CodeDom.CodeCastExpression)
            iw.Write("CType(")
            GenerateCodeFromExpression(.Expression, iw, o)
            iw.Write(", " & .TargetType.BaseType)
            iw.Write(")")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeDelegateCreateExpression Then
         With CType(e, CodeDom.CodeDelegateCreateExpression)
            ' TODO: Manage delegates
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeDelegateInvokeExpression Then
         With CType(e, CodeDom.CodeDelegateInvokeExpression)
            ' TODO: Manage delegates
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeDirectionExpression Then
         With CType(e, CodeDom.CodeDirectionExpression)
            If .Direction = CodeDom.FieldDirection.Ref Then
               iw.Write("ByRef ")
            Else
               iw.Write("ByVal ")
            End If
            GenerateCodeFromExpression(.Expression, iw, o)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeEventReferenceExpression Then
         With CType(e, CodeDom.CodeEventReferenceExpression)
            ' TODO: Deal with events
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeFieldReferenceExpression Then
         With CType(e, CodeDom.CodeFieldReferenceExpression)
            GenerateCodeFromExpression(.TargetObject, iw, o)
            iw.Write("." & .FieldName)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeIndexerExpression Then
         With CType(e, CodeDom.CodeIndexerExpression)
            GenerateCodeFromExpression(.TargetObject, iw, o)
            iw.Write("(")
            For i As Int32 = 0 To .Indices.Count - 1
               If i > 0 Then
                  iw.Write(",")
               End If
               GenerateCodeFromExpression(.Indices(i), iw, o)
            Next
            iw.Write(")")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeMethodInvokeExpression Then
         With CType(e, CodeDom.CodeMethodInvokeExpression)
            GenerateCodeFromExpression(.Method, iw, o)
            iw.Write("(")
            For i As Int32 = 0 To .Parameters.Count - 1
               If i > 0 Then
                  iw.Write(",")
               End If
               GenerateCodeFromExpression(.Parameters(i), iw, o)
            Next
            iw.Write(")")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeMethodReferenceExpression Then
         With CType(e, CodeDom.CodeMethodReferenceExpression)
            GenerateCodeFromExpression(.TargetObject, iw, o)
            iw.Write("." & .MethodName)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeObjectCreateExpression Then
         With CType(e, CodeDom.CodeObjectCreateExpression)
            iw.Write("New " & .CreateType.BaseType & "(")
            For i As Int32 = 0 To .Parameters.Count - 1
               If i > 0 Then
                  iw.Write(",")
               End If
               GenerateCodeFromExpression(.Parameters(i), iw, o)
            Next
            iw.Write(")")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeParameterDeclarationExpression Then
         With CType(e, CodeDom.CodeParameterDeclarationExpression)
            If (.Direction And CodeDom.FieldDirection.Ref) <> 0 Then
               iw.Write("ByRef ")
            Else
               iw.Write("ByVal ")
            End If
            iw.Write(.Name & " As " & .Type.BaseType)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodePrimitiveExpression Then
         With CType(e, CodeDom.CodePrimitiveExpression)
            iw.Write(.Value)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodePropertyReferenceExpression Then
         With CType(e, CodeDom.CodePropertyReferenceExpression)
            GenerateCodeFromExpression(.TargetObject, iw, o)
            iw.Write("." & .PropertyName)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodePropertySetValueReferenceExpression Then
         With CType(e, CodeDom.CodePropertySetValueReferenceExpression)
            iw.Write("Value")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeSnippetExpression Then
         With CType(e, CodeDom.CodeSnippetExpression)
            ' TODO: Are you going to support snippets?
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeThisReferenceExpression Then
         With CType(e, CodeDom.CodeThisReferenceExpression)
            iw.Write("Me")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeTypeOfExpression Then
         With CType(e, CodeDom.CodeTypeOfExpression)
            iw.Write("GetType(" & .Type.BaseType & ")")
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeTypeReferenceExpression Then
         With CType(e, CodeDom.CodeTypeReferenceExpression)
            iw.Write(.Type.BaseType)
         End With
      ElseIf TypeOf e Is System.CodeDom.CodeVariableReferenceExpression Then
         With CType(e, CodeDom.CodeVariableReferenceExpression)
            iw.Write(.VariableName)
         End With
      End If

   End Sub


   Public Function GetTypeOutput( _
               ByVal type As System.CodeDom.CodeTypeReference) _
               As String _
               Implements System.CodeDom.Compiler.ICodeGenerator.GetTypeOutput
      ' TODO: Deal with arrays 
      ' I'm just removing the system namespace for now
      If type.BaseType.StartsWith("System.") Then
         Return type.BaseType.Substring(7)
      End If
   End Function

   Public Function IsValidIdentifier( _
               ByVal value As String) _
               As Boolean _
               Implements System.CodeDom.Compiler.ICodeGenerator.IsValidIdentifier
      ' OK, maybe this isn't the most efficient, but...
      Dim temp As String = Tools.FixName(value, False)
      Return (temp.Trim = value.Trim)
   End Function

   Public Function Supports( _
               ByVal InSupports As System.CodeDom.Compiler.GeneratorSupport) _
               As Boolean _
               Implements System.CodeDom.Compiler.ICodeGenerator.Supports
      ' TODO: Fix temporary hack for Supports
      Return True
   End Function

   Public Sub ValidateIdentifier( _
               ByVal value As String) _
               Implements System.CodeDom.Compiler.ICodeGenerator.ValidateIdentifier
      If Not IsValidIdentifier(value) Then
         Throw New System.ApplicationException("Identifier is invalid")
      End If
   End Sub

   Private Function GetVisibility(ByVal e As CodeDom.CodeTypeMember) As String
      If (e.Attributes And CodeDom.MemberAttributes.Public) <> 0 Then
         Return "Public "
      ElseIf (e.Attributes And CodeDom.MemberAttributes.Assembly) <> 0 Then
         Return "Friend "
      ElseIf (e.Attributes And CodeDom.MemberAttributes.Family) <> 0 Then
         Return "Protected "
      ElseIf (e.Attributes And CodeDom.MemberAttributes.FamilyOrAssembly) <> 0 Then
         Return "Protected Friend "
      ElseIf (e.Attributes And CodeDom.MemberAttributes.FamilyAndAssembly) <> 0 Then
         Return "NOTSUPPORTED "
      Else
         Return "Private "
      End If
   End Function

   Private Sub OutputParameters( _
               ByVal e As CodeDom.CodeMemberMethod, _
               ByVal iw As CodeDom.Compiler.IndentedTextWriter)
      Dim p As CodeDom.CodeParameterDeclarationExpression

      iw.Write(e.Name & "(")

      For i As Int32 = 0 To e.Parameters.Count - 1
         If i < e.Parameters.Count - 1 Then
            iw.Write(", _" & vbcrlf)
         End If
         p = e.Parameters(i)
         If (p.Direction And CodeDom.FieldDirection.Ref) <> 0 Then
            iw.Write("ByRef ")
         Else
            iw.Write("ByVal ")
         End If
      Next

      iw.Write(e.Name & ")")
      If Not e.ReturnType Is Nothing Then
         iw.Write(" _" & vbcrlf & " As " & e.ReturnType.BaseType)
      End If


   End Sub
End Class
