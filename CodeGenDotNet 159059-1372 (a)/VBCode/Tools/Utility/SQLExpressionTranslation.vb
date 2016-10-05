Option Strict On
Option Explicit On 

Imports System
Imports System.Diagnostics

Public Class SQLExpressionTranslation
   Private Enum Op
      OpAnd
      OpOr
      OpEqual
      OpNotEqual
      OpLessThan
      OpGreaterThan
      OpLessThanOrEqual
      OpGreaterThanOrEqual
   End Enum


   Private MustInherit Class Operand
      Public operand As String
      Public prevOperand As operand
      Public nextOperator As Op
      Public nextOperand As operand
   End Class

   Private Class LogicalExpr
      Inherits operand
   End Class

   Private Class ComparisonExpr
      Inherits Operand
   End Class

   Private Class ArithExpr
      Inherits Operand
   End Class

   Private mStartSubExpr As Operand

   Private Sub New(ByVal s As String)
      mStartSubExpr = BuildOperands(s)
   End Sub

   Private Function BuildOperands(ByVal constraint As String) As Operand
      Dim a() As String
      Dim inner As String
      Dim after As String
      Dim before As String
      Dim logicalOps() As String = New String() {"and", "or"}
      Dim op As op
      If constraint.IndexOf("(") > 0 Then
         before = Tools.SubstringBefore(constraint, "(")
         Operand = BuildOperands(before)
         after = Tools.SubstringAfterLast(constraint, ")")
         Operand = BuildOperands(after)
         inner = constraint.Substring(before.Length + 1, constraint.Length - after.Length)
         Operand = BuildOperands(inner)
      ElseIf Tools.SubstringContains(constraint, logicalOps, False) Then
         before = Tools.SubstringBefore(constraint, logicalOps)
            Operand = BuildOperands(before)
            after = Tools.SubstringBefore(constraint, logicalOps)
            Operand = BuildOperands(after)
            op = GetOperator(Tools.SubstringContainsWhich(logicalOps))
      Else


      End If
   End Function

   Private Function GetOperator(ByVal opString As String) As Op
      Select Case opString.ToLower
         Case "and"
            Return Op.OpAnd
         Case "or"
            Return Op.OpOr
         Case "="
            Return Op.OpEqual
         Case "<>", "!="
            Return Op.OpNotEqual
         Case ">="
            Return Op.OpGreaterThanOrEqual
         Case "<="
            Return Op.OpLessThanOrEqual
         Case ">"
            Return Op.OpGreaterThan
         Case "<"
            Return Op.OpLessThan
      End Select
   End Function

   'Private Class Operand
   '   Public operarand
   'End Class

   'Private Class subexpression
   '   Public text As String
   '   Public toPrev As combineOp
   '   Public operand1 As operand
   '   Public operand2 As Operand
   '   Public operator
   'End Class

End Class
