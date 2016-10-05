' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Works from XSLT callbacks to translate TSQL expressions
'  NOTE: THis is preliminary code with very little testing
'
' THIS IS A HACK!!!!!
'
' THE SQL TRANSLATION IS NOT YET COMPLETE ENOUGH TO JUSTIFY TRANSLATION TO C#. 
' THIS PROJECT DELAYS THAT TRANSLATION> NOTHING IN THIS PROJECT SHOULD BE
' CONSIDERED COMPLETE OR PROPER
'


' WARNING: This system may explode when expressions are used in the parameter list
Option Explicit On 
Option Strict On

Imports System

Public Class TranslateSQL
   Public mXMLTokens As Xml.XmlDocument
   Public mNsMgr As Xml.XmlNamespaceManager
   Public mSQLExpr As CodeDom.CodeExpression
   Public Const vbcrlf As String = Microsoft.VisualBasic.ControlChars.CrLf

#Region "Public members"
   Public Function TranslateSQLToVB( _
               ByVal s As String, _
               ByVal currentTable As String, _
               ByVal currentColumn As String, _
               ByVal xmlTokens As Xml.XmlDocument, _
               ByVal nsmgr As Xml.XmlNamespaceManager, _
               ByVal stripNewLines As Boolean) _
               As String
      Dim provider As CodeDom.Compiler.CodeDomProvider = New Microsoft.VisualBasic.VBCodeProvider
      Dim sOut As String = TranslateSQLToDotNET(s, xmlTokens, nsmgr, currentTable, currentColumn, provider)
      If stripNewLines Then
         sOut = sOut.Replace("_" & vbcrlf, "")
         Do While sOut.IndexOf("  ") >= 0
            sOut = sOut.Replace("  ", " ")
         Loop
      End If
      Return sOut
   End Function

   Public Function TranslateSQLToVCSharp( _
               ByVal s As String, _
               ByVal currentTable As String, _
               ByVal currentColumn As String, _
               ByVal xmlTokens As Xml.XmlDocument, _
               ByVal nsmgr As Xml.XmlNamespaceManager, _
               ByVal stripNewLines As Boolean) _
               As String
      Dim provider As CodeDom.Compiler.CodeDomProvider = New Microsoft.CSharp.CSharpCodeProvider
      Dim sOut As String = TranslateSQLToDotNET(s, xmlTokens, nsmgr, currentTable, currentColumn, provider)
      If stripNewLines Then
         sOut = sOut.Replace(vbcrlf, "")
         Do While sOut.IndexOf("  ") >= 0
            sOut = sOut.Replace("  ", " ")
         Loop
      End If
      Return sOut
   End Function

   Public Function TranslateSQLToEnglish( _
               ByVal s As String, _
               ByVal currentTable As String, _
               ByVal currentColumn As String, _
               ByVal xmlTokens As Xml.XmlDocument, _
               ByVal nsmgr As Xml.XmlNamespaceManager) _
               As String
      ParseSQLToCodeDOM(s, xmlTokens, nsmgr, currentTable, currentColumn)
      Return Utility.TranslateToEnglish(mSQLExpr)
   End Function

   Public Function TranslateSQLToDotNET( _
               ByVal s As String, _
               ByVal xmlTokens As Xml.XmlDocument, _
               ByVal nsmgr As Xml.XmlNamespaceManager, _
               ByVal currentTable As String, _
               ByVal currentColumn As String, _
               ByVal provider As CodeDom.compiler.CodeDomProvider) _
               As String
      Dim gen As CodeDom.Compiler.ICodeGenerator = provider.CreateGenerator()
      Dim stringwriter As New IO.StringWriter
      Dim sOut As String
      ParseSQLToCodeDOM(s, xmlTokens, nsmgr, currentTable, currentColumn)
      gen.GenerateCodeFromExpression(mSQLExpr, stringwriter, _
                 New CodeDom.Compiler.CodeGeneratorOptions)
      sOut = stringwriter.GetStringBuilder.ToString
      Return sOut
   End Function

   Public Sub ParseSQLToCodeDOM( _
               ByVal s As String, _
               ByVal xmlTokens As Xml.XmlDocument, _
               ByVal nsmgr As Xml.XmlNamespaceManager, _
               ByVal currentTable As String, _
               ByVal currentColumn As String)
      Dim expr As CodeDom.CodeExpression
      If mSQLExpr Is Nothing Then
         Utility.SetupOperators()
         Utility.SetupTypeStuff()
         expr = Utility.ParseToExpression(s)
         mXMLTokens = xmlTokens
         mNsMgr = nsmgr
         expr = Utility.MorphTokens(expr, currentTable, currentColumn, mXMLTokens, mNsMgr)
         expr = Utility.MorphTypes(expr, xmlTokens, nsmgr)
         ' Utility.StoreLexical(expr)
         expr = Utility.MorphToCodeDOM(expr)
         mSQLExpr = expr
      End If
   End Sub
#End Region

#Region "Nested Support Classes"
   Public Enum OpEnum
      ' This is public so the programmer knows what operators are supported
      [And]
      [Or]
      Equal
      NotEqual
      LessThan
      GreaterThan
      LessThanOrEqual
      GreaterThanOrEqual
      Add
      Subtract
      Multiply
      Divide
      Exponent
      [Not]
      Negation
   End Enum

   Public Enum TypeGroup
      Unknown
      [Boolean]
      [DateTime]
      [String]
      [Integer]
      Float
   End Enum

   Private Class CodeUnaryOperatorExpression
      Inherits CodeDom.CodeExpression
      Public Operator As Operator
      Public Operand As CodeDom.CodeExpression
   End Class

   Private Class CodeSQLExpression
      Inherits CodeDom.CodeExpression
      Public NETType As String
      Public SQLName As String
   End Class

   Private Class CodeSQLColumnReferenceExpression
      Inherits CodeSQLExpression
      Public PropertyName As String
      Public Sub New(ByVal dbColumn As String)
         dbColumn = dbColumn.Trim
         Me.SQLName = dbColumn.Substring(0, dbColumn.Length - 1).Substring(1)
      End Sub
   End Class

   Private Class CodeSQLFunctionReferenceExpression
      Inherits CodeSQLExpression
      Public paramNames() As String
      Public parameters() As CodeDom.CodeParameterDeclarationExpression
      Public NETClassName As String
      Public NETMethodName As String
      Public Sub New(ByVal s As String)
         Dim paramString As String
         Dim temp As String
         s = s.Trim
         Me.SQLName = Tools.SubstringBefore(s, "(")
         paramString = Tools.SubstringAfter(s, "(")
         paramString = paramString.Substring(0, paramString.Length - 1) ' chop trailing parens
         Utility.ParseToParameters(paramString)
      End Sub
   End Class

   Private Class Operator
      Public SQLText As String
      Public EnglishText As String
      Public OpEnum As OpEnum
      Public Precedence As PrecedenceEnum
      Public ConstructorInfo As Reflection.ConstructorInfo
      Public CodeDOMOperator As CodeDom.CodeBinaryOperatorType
      Public Sub New( _
                  ByVal SQLText As String, _
                  ByVal EnglishText As String, _
                  ByVal opEnum As OpEnum, _
                  ByVal precedence As PrecedenceEnum, _
                  ByVal CodeDOMOperator As CodeDom.CodeBinaryOperatorType, _
                  ByVal type As System.Type)
         Me.SQLText = SQLText
         Me.EnglishText = EnglishText
         Me.OpEnum = opEnum
         Me.Precedence = precedence
         Me.CodeDOMOperator = CodeDOMOperator
         Me.ConstructorInfo = type.GetConstructor(New type() {})
      End Sub
   End Class

   Private Class TypeStuff
      Public Name As String
      Public Precedence As Int32
      Public Type As System.Type
      Public Group As TypeGroup
      Public Sub New( _
                  ByVal name As String, _
                  ByVal precedence As Int32, _
                  ByVal type As System.Type, _
                  ByVal group As TypeGroup)
         Me.Name = name
         Me.Precedence = precedence
         Me.Type = type
         Me.Group = group
      End Sub
      Public Sub New( _
                  ByVal precedence As Int32, _
                  ByVal type As System.Type, _
                  ByVal group As TypeGroup)
         Me.Name = type.ToString
         Me.Precedence = precedence
         Me.Type = type
         Me.Group = group
      End Sub
   End Class

   Public Enum PrecedenceEnum
      Primary
      Unary
      Multiplicative
      Additive
      Comparison
      Bitwise
      LogicalNot
      Conjunction
      OtherLogical
   End Enum

   Private mGenerator As CodeDom.Compiler.ICodeGenerator
   Private ReadOnly Property Generator() As CodeDom.Compiler.ICodeGenerator
      Get
         If mGenerator Is Nothing Then
            mGenerator = New Microsoft.VisualBasic.VBCodeProvider().CreateGenerator
         End If
         Return mGenerator
      End Get
   End Property
#End Region

#Region "Nested Utillity Class"
   Private Class Utility
      Private Shared mOperators() As Operator
      Private Shared mTypeStuff() As TypeStuff
      Private Shared mUnknownTypeStuff As TypeStuff

      Public Shared Sub SetupOperators()
         ReDim mOperators(17)
         ' The order of these operators is important because it needs to find <> before <, etc.
         mOperators(0) = New Operator("*", "*", OpEnum.Multiply, PrecedenceEnum.Multiplicative, CodeDom.CodeBinaryOperatorType.Multiply, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(1) = New Operator("/", "/", OpEnum.Multiply, PrecedenceEnum.Multiplicative, CodeDom.CodeBinaryOperatorType.Divide, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(2) = New Operator("%", "Modulus", OpEnum.Multiply, PrecedenceEnum.Multiplicative, CodeDom.CodeBinaryOperatorType.Modulus, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(3) = New Operator("+", "+", OpEnum.Multiply, PrecedenceEnum.Additive, CodeDom.CodeBinaryOperatorType.Add, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(4) = New Operator("-", "-", OpEnum.Multiply, PrecedenceEnum.Additive, CodeDom.CodeBinaryOperatorType.Subtract, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(5) = New Operator("=", "must equal", OpEnum.Multiply, PrecedenceEnum.Comparison, CodeDom.CodeBinaryOperatorType.ValueEquality, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(6) = New Operator(">=", "must be greater than or equal to", OpEnum.Multiply, PrecedenceEnum.Comparison, CodeDom.CodeBinaryOperatorType.GreaterThanOrEqual, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(7) = New Operator("<=", "must be less than or equal to", OpEnum.Multiply, PrecedenceEnum.Comparison, CodeDom.CodeBinaryOperatorType.LessThanOrEqual, GetType(CodeDom.CodeBinaryOperatorExpression))
         ' NOTE: This is incorrect - but this is what works in the CodeDOM. 
         mOperators(8) = New Operator("<>", "must not be equal to", OpEnum.Multiply, PrecedenceEnum.Comparison, CodeDom.CodeBinaryOperatorType.IdentityInequality, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(9) = New Operator(">", "must be greater than", OpEnum.Multiply, PrecedenceEnum.Comparison, CodeDom.CodeBinaryOperatorType.GreaterThan, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(10) = New Operator("<", "must be less than", OpEnum.Multiply, PrecedenceEnum.Comparison, CodeDom.CodeBinaryOperatorType.LessThan, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(11) = New Operator("^", "to the power of", OpEnum.Multiply, PrecedenceEnum.Bitwise, 0, GetType(CodeDom.CodeBinaryOperatorExpression)) ' This is XOR
         mOperators(12) = New Operator("&", "bitwise and", OpEnum.Multiply, PrecedenceEnum.Bitwise, CodeDom.CodeBinaryOperatorType.BitwiseAnd, GetType(CodeDom.CodeBinaryOperatorExpression))   ' bitwise AND
         mOperators(13) = New Operator("|", "bitwise or", OpEnum.Multiply, PrecedenceEnum.Bitwise, CodeDom.CodeBinaryOperatorType.BitwiseOr, GetType(CodeDom.CodeBinaryOperatorExpression))   ' bitwise OR
         mOperators(14) = New Operator(" not ", "not", OpEnum.Multiply, PrecedenceEnum.LogicalNot, 0, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(15) = New Operator(" and ", "and", OpEnum.Multiply, PrecedenceEnum.Conjunction, CodeDom.CodeBinaryOperatorType.BooleanAnd, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(16) = New Operator(" or ", "or", OpEnum.Multiply, PrecedenceEnum.OtherLogical, CodeDom.CodeBinaryOperatorType.BooleanOr, GetType(CodeDom.CodeBinaryOperatorExpression))
         mOperators(17) = New Operator(" is ", "is", OpEnum.Multiply, PrecedenceEnum.Comparison, CodeDom.CodeBinaryOperatorType.IdentityEquality, GetType(CodeDom.CodeBinaryOperatorExpression))
      End Sub

      Public Shared Sub SetupTypeStuff()
         ReDim mTypeStuff(10)
         Dim i As Int32 = -1
         i += 1 : mTypeStuff(i) = New TypeStuff("Error - Unknown Type", i, Nothing, TypeGroup.Unknown)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.Boolean), TypeGroup.Boolean)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.DateTime), TypeGroup.DateTime)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.String), TypeGroup.String)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.Double), TypeGroup.Float)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.Single), TypeGroup.Float)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.Int64), TypeGroup.Integer)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.Int32), TypeGroup.Integer)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.Int16), TypeGroup.Integer)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.Byte), TypeGroup.Integer)
         i += 1 : mTypeStuff(i) = New TypeStuff(i, GetType(System.Decimal), TypeGroup.Float)
         mUnknownTypeStuff = mTypeStuff(0)
      End Sub

      Public Shared Function ParseToExpression(ByVal s As String) As CodeDom.CodeExpression
         Dim iLowestOpPos As Int32 = -1
         Dim iParenDepth As Int32
         Dim iLowestParen As Int32 = 999
         Dim lowestOperator As Operator
         Dim chars As Char()
         Dim bFound As Boolean
         Dim left As String
         Dim right As String
         Dim expr As CodeDom.CodeExpression
         Dim hasParens As Boolean
         's = s.ToUpper.Trim
         s = s.Trim()
         If s.StartsWith("(") And s.EndsWith(")") Then
            s = s.Substring(0, s.Length - 1).Substring(1).Trim
            hasParens = True
         End If
         chars = s.ToLower.ToCharArray
         For i As Int32 = 0 To chars.GetUpperBound(0)
            If chars(i) = "(" Then
               iParenDepth += 1
            ElseIf chars(i) = ")" Then
               iParenDepth -= 1
            Else
               If iParenDepth <= iLowestParen Then
                  For j As Int32 = 0 To mOperators.GetUpperBound(0)
                     bFound = True
                     For k As Int32 = 0 To mOperators(j).SQLText.Length - 1
                        If i + k > chars.GetUpperBound(0) Then
                           bFound = False
                           Exit For
                        ElseIf chars(i + k) <> mOperators(j).SQLText.Substring(k, 1) Then
                           bFound = False
                           Exit For
                        End If
                     Next
                     If bFound Then
                        If lowestOperator Is Nothing OrElse _
                                 iParenDepth < iLowestParen OrElse _
                                 mOperators(j).Precedence > lowestOperator.Precedence Then
                           lowestOperator = mOperators(j)
                           iLowestOpPos = i
                           iLowestParen = iParenDepth
                           Exit For
                        End If
                     End If
                  Next
               End If
            End If
         Next
         If Not lowestOperator Is Nothing Then
            left = s.Substring(0, iLowestOpPos)
            right = s.Substring(iLowestOpPos + lowestOperator.SQLText.Length)
            expr = CType(lowestOperator.ConstructorInfo.Invoke(New Object() {}), CodeDom.CodeExpression)
            If TypeOf expr Is CodeDom.CodeBinaryOperatorExpression Then
               If hasParens Then
                  expr.UserData.Add("HasParens", "True")
               End If
               CType(expr, CodeDom.CodeBinaryOperatorExpression).Left = Utility.ParseToExpression(left)
               CType(expr, CodeDom.CodeBinaryOperatorExpression).Right = Utility.ParseToExpression(right)
               CType(expr, CodeDom.CodeBinaryOperatorExpression).Operator = lowestOperator.CodeDOMOperator
            ElseIf TypeOf expr Is CodeUnaryOperatorExpression Then
               CType(expr, CodeUnaryOperatorExpression).Operand = Utility.ParseToExpression(left)
            Else
               Throw New System.ApplicationException("You are using an unknown expression type: " & lowestOperator.ToString)
            End If
         Else
            ' so we're down to the actual, real operand. Its a column value, literal or function
            If s.IndexOf("(") >= 0 Then
               If s.IndexOf(")") < 0 Then
                  Throw New System.ApplicationException("Unclosed Parentheses")
               End If
               expr = New CodeSQLFunctionReferenceExpression(s)
            ElseIf s.IndexOf("[") >= 0 Then
               expr = New CodeSQLColumnReferenceExpression(s)
            Else
               If s.StartsWith("""") Then
                  expr = New CodeDom.CodePrimitiveExpression(s.Substring(0, s.Length - 1).Substring(1))
               ElseIf Microsoft.VisualBasic.IsNumeric(s) Then
                  If s.IndexOf(".") < 0 Then
                     expr = New CodeDom.CodePrimitiveExpression(Int32.Parse(s))
                  Else
                     expr = New CodeDom.CodePrimitiveExpression(Double.Parse(s))
                  End If
               ElseIf Microsoft.VisualBasic.IsDate(s) Then
                  expr = New CodeDom.CodePrimitiveExpression(DateTime.Parse(s))
               ElseIf s = "null" Then
                  expr = New CodeDom.CodePrimitiveExpression("DBNull.Value")
               Else
                  Throw New System.ApplicationException("Error parsing " & s)
               End If
            End If
         End If
         Return expr
      End Function

      Public Shared Function ParseToParameters(ByVal s As String) As CodeDom.CodeExpressionCollection
         Dim chars() As Char
         Dim iDepth As Int32
         Dim coll As New CodeDom.CodeExpressionCollection
         Dim iStartPos As Int32
         Dim expr As CodeDom.CodeExpression
         ' split at all zero depth commas
         s = s.Trim
         If s.StartsWith("(") And s.EndsWith(")") Then
            s = s.Substring(0, s.Length - 1).Substring(1).Trim
         End If
         If s.Length > 0 Then
            chars = s.ToCharArray
            For i As Int32 = 0 To chars.GetUpperBound(0)
               If chars(i) = "("c Then
                  iDepth += 1
               ElseIf chars(i) = ")"c Then
                  iDepth -= 1
               ElseIf chars(i) = "," And iDepth = 0 Then
                  ' Split at this comma
                  expr = ParseToExpression(s.Substring(iStartPos, i - iStartPos).Trim)
                  iStartPos = i + 1
                  coll.Add(expr)
               End If
            Next
            ' Deal with stuff at end
            expr = ParseToExpression(s.Substring(iStartPos).Trim)
            coll.Add(expr)
         End If
      End Function

      Public Shared Function MorphTokens( _
                  ByVal expr As CodeDom.CodeExpression, _
                  ByVal currentTable As String, _
                  ByVal currentColumn As String, _
                  ByVal xmlTokens As Xml.XmlDocument, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As CodeDom.CodeExpression
         Dim this As CodeDom.CodeThisReferenceExpression
         If TypeOf expr Is CodeUnaryOperatorExpression Then
            With CType(expr, CodeUnaryOperatorExpression)
               expr = MorphTokens(.Operand, currentTable, currentColumn, xmlTokens, nsmgr)
            End With
         ElseIf TypeOf expr Is CodeSQLColumnReferenceExpression Then
            With CType(expr, CodeSQLColumnReferenceExpression)
               Dim node As Xml.XmlNode = xmlTokens.SelectSingleNode("//orm:Object[@Name='" & currentTable & "']//orm:Property[@Name='" & .SQLName & "']", nsmgr)
               If Not node Is Nothing Then
                  .PropertyName = Tools.GetAttributeOrEmpty(node, "Name")
                  .NETType = Tools.GetAttributeOrEmpty(node, "NETType")
               End If
            End With
         ElseIf TypeOf expr Is CodeSQLFunctionReferenceExpression Then
            With CType(expr, CodeSQLFunctionReferenceExpression)
               Dim node As Xml.XmlNode = xmlTokens.SelectSingleNode( _
                        "//orm:SQLFunctionReplacement//orm:SQLFunction[@Name='" & _
                        .SQLName & "']", nsmgr)
               Dim functionName As String
               If Not node Is Nothing Then
                  functionName = Tools.GetAttributeOrEmpty(node, "ReplaceWith")
                  .NETMethodName = Tools.SubstringAfterLast(functionName, ".")
                  .NETClassName = functionName.Substring(0, functionName.Length - .NETMethodName.Length - 1)
                  .NETType = Tools.GetAttributeOrEmpty(node, "NETType")
               End If
            End With
         ElseIf TypeOf expr Is CodeDom.CodeBinaryOperatorExpression Then
            With CType(expr, CodeDom.CodeBinaryOperatorExpression)
               .Left = MorphTokens(.Left, currentTable, currentColumn, xmlTokens, nsmgr)
               .Right = MorphTokens(.Right, currentTable, currentColumn, xmlTokens, nsmgr)
            End With
         ElseIf TypeOf expr Is CodeDom.CodeCastExpression Then
            With CType(expr, CodeDom.CodeCastExpression)
               .Expression = MorphTokens(.Expression, currentTable, currentColumn, xmlTokens, nsmgr)
            End With
         ElseIf TypeOf expr Is CodeDom.CodePrimitiveExpression Then
         Else
            Throw New System.ApplicationException("Expression type " & expr.ToString & " not handled")
         End If
         Return expr
      End Function

      Public Shared Function MorphTypes( _
                  ByVal expr As CodeDom.CodeExpression, _
                  ByVal xmlTokens As Xml.XmlDocument, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As CodeDom.CodeExpression
         Dim this As CodeDom.CodeThisReferenceExpression
         If TypeOf expr Is CodeUnaryOperatorExpression Then
            With CType(expr, CodeUnaryOperatorExpression)
               expr = MorphTypes(.Operand, xmlTokens, nsmgr)
            End With
         ElseIf TypeOf expr Is CodeSQLColumnReferenceExpression Then
            ' do nothing
         ElseIf TypeOf expr Is CodeSQLFunctionReferenceExpression Then
            ' do nothing
         ElseIf TypeOf expr Is CodeDom.CodeBinaryOperatorExpression Then
            With CType(expr, CodeDom.CodeBinaryOperatorExpression)
               .Left = MorphTypes(.Left, xmlTokens, nsmgr)
               .Right = MorphTypes(.Right, xmlTokens, nsmgr)
               expr = CheckTypeMatch(CType(expr, CodeDom.CodeBinaryOperatorExpression), _
                        .Operator, xmlTokens, nsmgr)
            End With
         ElseIf TypeOf expr Is CodeDom.CodeCastExpression Then
            With CType(expr, CodeDom.CodeCastExpression)
               .Expression = MorphTypes(.Expression, xmlTokens, nsmgr)
            End With
         ElseIf TypeOf expr Is CodeDom.CodePrimitiveExpression Then
         ElseIf TypeOf expr Is CodeDom.CodeObjectCreateExpression Then
            With CType(expr, CodeDom.CodeObjectCreateExpression)
               For i As Int32 = 0 To .Parameters.Count - 1
                  .Parameters(i) = MorphTypes(.Parameters(i), xmlTokens, nsmgr)
               Next
            End With
         Else
            Throw New System.ApplicationException("Expression type " & expr.ToString & " not handled")
         End If
         Return expr
      End Function

      Private Shared Function CheckTypeMatch( _
                  ByVal expr As CodeDom.CodeBinaryOperatorExpression, _
                  ByVal operator As CodeDom.CodeBinaryOperatorType, _
                  ByVal xmlTokens As Xml.XmlDocument, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As CodeDom.CodeExpression
         Dim sw As New IO.StringWriter
         Dim isLiteralDate As Boolean
         Dim typeStuffLeft As TypeStuff
         Dim typeStuffRight As TypeStuff
         Dim exprRet As CodeDom.CodeExpression
         typeStuffLeft = SetType(expr.Left, xmlTokens, nsmgr)
         typeStuffRight = SetType(expr.Right, xmlTokens, nsmgr)
         If typeStuffRight.Group = typeStuffLeft.Group Then
            ' No conversion needed, even if its unknown
            exprRet = expr
         ElseIf typeStuffRight.Group = TypeGroup.Unknown Then
            exprRet = GetTypeFindFailure(expr.Right)
         ElseIf typeStuffLeft.Group = TypeGroup.Unknown Then
            exprRet = GetTypeFindFailure(expr.Left)
         Else
            Dim op As String
            If expr.Operator = CodeDom.CodeBinaryOperatorType.Subtract Then
               op = "-"
            Else
               op = "+"
            End If
            If typeStuffLeft.Type Is GetType(System.DateTime) And _
                        (typeStuffRight.Group = TypeGroup.Integer Or _
                         typeStuffRight.Group = TypeGroup.Float) Then
               exprRet = DateMath(expr, expr.Left, expr.Right)
               exprRet.UserData.Add("NETType", "System.DateTime")
               exprRet.UserData.Add("English", TranslateToEnglish(expr.Left) & _
                        op & TranslateToEnglish(expr.Right))
            ElseIf typeStuffRight.Type Is GetType(System.DateTime) And _
                        (typeStuffLeft.Group = TypeGroup.Integer Or _
                         typeStuffLeft.Group = TypeGroup.Float) Then
               exprRet = DateMath(expr, expr.Right, expr.Left)
               exprRet.UserData.Add("NETType", "System.DateTime")
               exprRet.UserData.Add("English", TranslateToEnglish(expr.Left) & _
                        op & TranslateToEnglish(expr.Right))
            Else
               If typeStuffRight.Precedence > typeStuffLeft.Precedence Then
                  expr.Right = New CodeDom.CodeCastExpression(typeStuffLeft.Type, expr.Right)
               Else
                  expr.Left = New CodeDom.CodeCastExpression(typeStuffRight.Type, expr.Left)
               End If
               exprRet = expr
            End If
         End If

         If TypeOf exprRet Is CodeDom.CodeBinaryOperatorExpression Then
            With CType(exprRet, CodeDom.CodeBinaryOperatorExpression)
               typeStuffLeft = SetType(.Left, xmlTokens, nsmgr)
               typeStuffRight = SetType(.Right, xmlTokens, nsmgr)
               If typeStuffRight.Precedence > typeStuffLeft.Precedence Then
                  exprRet.UserData.Add("NETType", typeStuffRight.Name)
               Else
                  exprRet.UserData.Add("NETType", typeStuffLeft.Name)
               End If
            End With
         End If

         Return exprRet
      End Function

      Private Shared Function SetType( _
                  ByVal expr As CodeDom.CodeExpression, _
                  ByVal xmlTokens As Xml.XmlDocument, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As TypeStuff
         Dim obj As System.Object = expr.UserData.Item("NETType")
         Dim s As String
         If Not obj Is Nothing Then
            ' We already calcucated the type
            s = obj.ToString
            For i As Int32 = 0 To mTypeStuff.GetUpperBound(0)
               If mTypeStuff(i).Name = s Then
                  Return mTypeStuff(i)
               End If
            Next
         ElseIf TypeOf expr Is CodeDom.CodeCastExpression Then
            s = CType(expr, CodeDom.CodeCastExpression).TargetType.BaseType
            For i As Int32 = 0 To mTypeStuff.GetUpperBound(0)
               If mTypeStuff(i).Name = s Then
                  Return mTypeStuff(i)
               End If
            Next
         ElseIf TypeOf expr Is CodeSQLExpression Then
            s = CType(expr, CodeSQLExpression).NETType
            For i As Int32 = 0 To mTypeStuff.GetUpperBound(0)
               If mTypeStuff(i).Name = s Then
                  Return mTypeStuff(i)
               End If
            Next
            ' If we get here, we didn't find it
            Dim node As Xml.XmlNode
            node = xmlTokens.SelectSingleNode("//orm:SpecialType[@Name='" & s & "']/orm:Property", nsmgr)
            If Not node Is Nothing Then
               s = Tools.GetAttributeOrEmpty(node, "AccessVia")
               For i As Int32 = 0 To mTypeStuff.GetUpperBound(0)
                  If mTypeStuff(i).Name = s Then
                     Return mTypeStuff(i)
                  End If
               Next
            End If
         ElseIf TypeOf expr Is CodeDom.CodePrimitiveExpression Then
            Dim type As System.Type
            type = CType(expr, CodeDom.CodePrimitiveExpression).Value.GetType
            For i As Int32 = 0 To mTypeStuff.GetUpperBound(0)
               If mTypeStuff(i).Type Is type Then
                  Return mTypeStuff(i)
               End If
            Next
         End If
         ' If we get here, we didn't find it
         Return mUnknownTypeStuff
      End Function

      Private Shared Function DateMath( _
                  ByVal expr As CodeDom.CodeBinaryOperatorExpression, _
                  ByVal exprDate As CodeDom.CodeExpression, _
                  ByVal exprInt As CodeDom.CodeExpression) _
                  As CodeDom.CodeExpression
         'Assume SQL won't let illegal stuff like 7 - getdate() through 
         Dim methodname As String
         If expr.Operator = CodeDom.CodeBinaryOperatorType.Add Then
            methodname = "Add"
         ElseIf expr.Operator = CodeDom.CodeBinaryOperatorType.Subtract Then
            methodname = "Subtract"
         End If
         If Not methodname Is Nothing Then
            Dim p0 As New CodeDom.CodePrimitiveExpression(0)
            ' Assume its days being added 
            Dim exprTS As New CodeDom.CodeObjectCreateExpression(GetType(System.TimeSpan), exprInt, p0, p0, p0)
            Return New CodeDom.CodeMethodInvokeExpression( _
                     New CodeDom.CodeMethodReferenceExpression(exprDate, methodname), _
                     exprTS)
         End If
      End Function

      Public Shared Function StoreLexical( _
                  ByVal expr As CodeDom.CodeExpression) _
                  As CodeDom.CodeExpression
         ' simplify simple conditions 

      End Function


      Public Shared Function MorphToCodeDOM( _
                  ByVal expr As CodeDom.CodeExpression) _
                  As CodeDom.CodeExpression
         Dim this As CodeDom.CodeThisReferenceExpression
         If TypeOf expr Is CodeUnaryOperatorExpression Then
            ' Change this to binary with a multiplicaton times negative one
            expr = New CodeDom.CodeBinaryOperatorExpression( _
                        New CodeDom.CodePrimitiveExpression(-1), _
                        CodeDom.CodeBinaryOperatorType.Multiply, _
                        MorphToCodeDOM( _
                              CType(expr, CodeUnaryOperatorExpression).Operand))
         ElseIf TypeOf expr Is CodeSQLColumnReferenceExpression Then
            With CType(expr, CodeSQLColumnReferenceExpression)
               expr = New CodeDom.CodePropertyReferenceExpression(this, .PropertyName)
               expr.UserData.Add("SQLName", .SQLName)
               expr.UserData.Add("NETType", .NETType)
            End With
         ElseIf TypeOf expr Is CodeSQLFunctionReferenceExpression Then
            With CType(expr, CodeSQLFunctionReferenceExpression)
               Dim ref As New CodeDom.CodeMethodReferenceExpression( _
                           New CodeDom.CodeTypeReferenceExpression(.NETClassName), _
                           .NETMethodName)
               ref.UserData.Add("SQLName", .SQLName)
               ref.UserData.Add("NETType", .NETType)
               If .parameters Is Nothing Then
                  expr = New CodeDom.CodeMethodInvokeExpression(ref)
               Else
                  expr = New CodeDom.CodeMethodInvokeExpression(ref, .parameters)
               End If
            End With
         ElseIf TypeOf expr Is CodeDom.CodeBinaryOperatorExpression Then
            With CType(expr, CodeDom.CodeBinaryOperatorExpression)
               .Left = MorphToCodeDOM(.Left)
               .Right = MorphToCodeDOM(.Right)
            End With
         ElseIf TypeOf expr Is CodeDom.CodeCastExpression Then
            With CType(expr, CodeDom.CodeCastExpression)
               .Expression = MorphToCodeDOM(.Expression)
            End With
         ElseIf TypeOf expr Is CodeDom.CodePrimitiveExpression Then
         ElseIf TypeOf expr Is CodeDom.CodeMethodInvokeExpression Then
            With CType(expr, CodeDom.CodeMethodInvokeExpression)
               .Method = CType(MorphToCodeDOM(.Method), CodeDom.CodeMethodReferenceExpression)
               For i As Int32 = 0 To .Parameters.Count - 1
                  .Parameters(i) = MorphToCodeDOM(.Parameters(i))
               Next
            End With
         ElseIf TypeOf expr Is CodeDom.CodeMethodReferenceExpression Then
            With CType(expr, CodeDom.CodeMethodReferenceExpression)
               .TargetObject = MorphToCodeDOM(.TargetObject)
            End With
         ElseIf TypeOf expr Is CodeDom.CodeObjectCreateExpression Then
            With CType(expr, CodeDom.CodeObjectCreateExpression)
               For i As Int32 = 0 To .Parameters.Count - 1
                  .Parameters(i) = MorphToCodeDOM(.Parameters(i))
               Next
            End With
         Else
            Throw New System.ApplicationException("Expression type " & expr.ToString & " not handled")
         End If
         Return expr
      End Function

      Public Shared Function TranslateToEnglish( _
                  ByVal expr As CodeDom.CodeExpression) _
                  As String
         Dim sb As New Text.StringBuilder
         Dim obj As System.Object
         If Not expr Is Nothing Then
            obj = expr.UserData("English")
            If Not obj Is Nothing Then
               Return CType(obj, String)
            ElseIf TypeOf expr Is System.CodeDom.CodeArgumentReferenceExpression Then
               With CType(expr, System.CodeDom.CodeArgumentReferenceExpression)
                  Return .ParameterName
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeArrayCreateExpression Then
               With CType(expr, System.CodeDom.CodeArrayCreateExpression)
                  Return "[Arrays not handled]"
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeArrayIndexerExpression Then
               With CType(expr, System.CodeDom.CodeArrayIndexerExpression)
                  Return "[Arrays not handled]"
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeBaseReferenceExpression Then
               With CType(expr, System.CodeDom.CodeBaseReferenceExpression)
                  Return "[BaseClass]"
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeBinaryOperatorExpression Then
               obj = expr.UserData("HasParens")
               Dim hasParens As Boolean
               If Not obj Is Nothing AndAlso TypeOf obj Is String Then
                  If CType(obj, String).ToLower = "true" Then
                     hasParens = True
                  End If
               End If
               If hasParens Then
                  sb.Append("(")
               End If
               With CType(expr, System.CodeDom.CodeBinaryOperatorExpression)
                  sb.Append(TranslateToEnglish(.Left))
                  sb.Append(TranslateOperatorToEnglish(.Operator))
                  sb.Append(TranslateToEnglish(.Right))
               End With
               If hasParens Then
                  sb.Append(")")
               End If
               Return sb.ToString
            ElseIf TypeOf expr Is System.CodeDom.CodeCastExpression Then
               With CType(expr, System.CodeDom.CodeCastExpression)
                  Return TranslateToEnglish(.Expression)
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeDelegateCreateExpression Then
               With CType(expr, System.CodeDom.CodeDelegateCreateExpression)
                  ' Do nothing
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeDelegateInvokeExpression Then
               With CType(expr, System.CodeDom.CodeDelegateInvokeExpression)
                  ' Do nothing
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeDirectionExpression Then
               With CType(expr, System.CodeDom.CodeDirectionExpression)
                  ' Do nothing
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeEventReferenceExpression Then
               With CType(expr, System.CodeDom.CodeEventReferenceExpression)
                  ' Do nothing
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeFieldReferenceExpression Then
               With CType(expr, System.CodeDom.CodeFieldReferenceExpression)
                  sb.Append(TranslateToEnglish(.TargetObject))
                  sb.Append(.FieldName)
                  Return sb.ToString
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeIndexerExpression Then
               With CType(expr, System.CodeDom.CodeIndexerExpression)
                  Return "[Arrays not handled]"
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeMethodInvokeExpression Then
               With CType(expr, System.CodeDom.CodeMethodInvokeExpression)
                  sb.Append(TranslateToEnglish(.Method))
                  If .Parameters.Count > 0 Then
                     sb.Append("(")
                     For i As Int32 = 0 To .Parameters.Count - 1
                        If i > 0 Then
                           sb.Append(", ")
                        End If
                        sb.Append(TranslateToEnglish(.Parameters(i)))
                     Next
                     sb.Append(")")
                  End If
                  Return sb.ToString
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeMethodReferenceExpression Then
               With CType(expr, System.CodeDom.CodeMethodReferenceExpression)
                  sb.Append(TranslateToEnglish(.TargetObject))
                  sb.Append(.MethodName)
                  Return sb.ToString
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeObjectCreateExpression Then
               With CType(expr, System.CodeDom.CodeObjectCreateExpression)
                  ' do nothing
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeParameterDeclarationExpression Then
               With CType(expr, System.CodeDom.CodeParameterDeclarationExpression)
                  ' do nothing
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodePrimitiveExpression Then
               With CType(expr, System.CodeDom.CodePrimitiveExpression)
                  If TypeOf .Value Is System.String Then
                     Return """" & CType(.Value, System.String) & """"
                  Else
                     Return .Value.ToString
                  End If
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodePropertyReferenceExpression Then
               With CType(expr, System.CodeDom.CodePropertyReferenceExpression)
                  sb.Append(TranslateToEnglish(.TargetObject))
                  sb.Append(.PropertyName)
                  Return sb.ToString
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodePropertySetValueReferenceExpression Then
               With CType(expr, System.CodeDom.CodePropertySetValueReferenceExpression)
                  Return "Value"
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeSnippetExpression Then
               With CType(expr, System.CodeDom.CodeSnippetExpression)
                  ' do nothing
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeThisReferenceExpression Then
               With CType(expr, System.CodeDom.CodeThisReferenceExpression)
                  ' In English, assume this
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeTypeOfExpression Then
               With CType(expr, System.CodeDom.CodeTypeOfExpression)
                  ' do nothing 
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeTypeReferenceExpression Then
               With CType(expr, System.CodeDom.CodeTypeReferenceExpression)
                  'sb.Append(.Type.BaseType)
               End With
            ElseIf TypeOf expr Is System.CodeDom.CodeVariableReferenceExpression Then
               With CType(expr, System.CodeDom.CodeVariableReferenceExpression)
                  Return .VariableName
               End With
            ElseIf TypeOf expr Is CodeSQLFunctionReferenceExpression Then
               With CType(expr, CodeSQLFunctionReferenceExpression)
                  Return .NETMethodName
               End With
            End If
         End If
      End Function

      Public Shared Function TranslateOperatorToEnglish( _
                  ByVal op As CodeDom.CodeBinaryOperatorType) _
                  As String
         For i As Int32 = 0 To mOperators.GetUpperBound(0)
            If mOperators(i).CodeDOMOperator = op Then
               Return " " & mOperators(i).EnglishText & " "
            End If
         Next
      End Function


      Private Shared Function GetTypeFindFailure(ByVal expr As CodeDom.CodeExpression) As CodeDom.CodeExpression
         ' Ooops, we have a failure and need to output something that will help the programmer solve it
         Dim smsg As String = "ERROR: TYPES DEFINED IN UTILITY.TRANSLATESQL.VB (or CS) DOES NOT HANDLE THE TYPE"
         If TypeOf expr Is CodeSQLExpression Then
            With CType(expr, CodeSQLExpression)
               smsg &= .NETType
            End With
         Else
            smsg &= " [UNKNOWN TYPE]"
         End If
         smsg &= ". ADD THIS TYPE TO THE ARRAY CREATED IN THE SetUpTypeStuff() METHOD."
         Return New CodeDom.CodePrimitiveExpression(smsg)
      End Function
   End Class


#End Region

  
End Class
