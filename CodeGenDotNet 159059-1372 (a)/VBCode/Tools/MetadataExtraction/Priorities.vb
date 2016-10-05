' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Establishes the order of tables for deletion and insertion based on hierarchy

Option Strict On
Option Explicit On 

Imports System
Imports System.Diagnostics

Public Class TableInsertDeletePriority

   Public Shared Sub appendTableInsertPriority(ByVal metaData As System.Data.DataSet)

      Dim Tables As System.Data.DataTable = metaData.Tables("Tables")
      Tables.Columns.Add("LOAD_PRIORITY")

      Dim TableConstraints As System.Data.DataTable = metaData.Tables("TableConstraints")
      Dim ReferentialConstraints As System.Data.DataTable = metaData.Tables("ReferentialConstraints")
      For i As System.Int32 = 0 To Tables.Rows.Count - 1
         Diagnostics.Debug.WriteLine(Tables.Rows(i)("TABLE_NAME"))
         If IsRoot(CType(Tables.Rows(i)("TABLE_NAME"), String), TableConstraints, ReferentialConstraints) Then
            assignInsertPriority(CType(Tables.Rows(i)("TABLE_NAME"), String), 0, Tables, TableConstraints, ReferentialConstraints)
         End If
      Next
   End Sub

   Private Shared Sub assignInsertPriority( _
                     ByVal TableName As String, _
                     ByVal Priority As System.Int32, _
                     ByVal Tables As System.Data.DataTable, _
                     ByVal TableConstraints As System.Data.DataTable, _
                     ByVal ReferentialConstraints As System.Data.DataTable)
      Dim TableRow As System.Data.DataRow = Tables.Select("TABLE_NAME = '" + TableName + "'")(0)
      If ((TableRow.IsNull("LOAD_PRIORITY")) OrElse (Priority > Convert.ToInt32(TableRow("LOAD_PRIORITY")))) Then
         TableRow("LOAD_PRIORITY") = Priority
         Diagnostics.Debug.WriteLine(TableRow(0).ToString & ":" & TableRow(1).ToString & ":" & TableRow(2).ToString & ":" & TableRow(3).ToString & ":" & TableRow(4).ToString)
      End If
      Dim TableConstraint As System.Data.DataRow() = TableConstraints.Select("CONSTRAINT_TYPE = 'PRIMARY KEY' and TABLE_NAME = '" & TableName & "'")
      If TableConstraint.Length = 0 Then
      Else
         Dim UniqueConstraintName As String = CType(TableConstraint(0)("CONSTRAINT_NAME"), String)
         Dim ParentRelations As System.Data.DataRow() = ReferentialConstraints.Select("UNIQUE_CONSTRAINT_NAME = '" & UniqueConstraintName & "'")
         For i As System.Int32 = 0 To ParentRelations.Length - 1
            Dim ChildRelations As System.Data.DataRow() = TableConstraints.Select("CONSTRAINT_TYPE = 'FOREIGN KEY' and CONSTRAINT_NAME = '" & CType(ParentRelations(i)("CONSTRAINT_NAME"), String) & "'")
            ' Only in case of self reference?
            If ChildRelations.Length = 0 Then
               'continue;
            ElseIf (CType(ChildRelations(0)("TABLE_NAME"), String).Equals(TableName)) Then
               ' protect infinite recursion in case of self reference
               'continue;
            Else
               Dim ChildTableName As String = CType(ChildRelations(0)("TABLE_NAME"), String)
               assignInsertPriority(ChildTableName, Priority + 1, Tables, TableConstraints, ReferentialConstraints)
            End If
         Next
      End If
   End Sub

   Public Shared Sub appendTableDeletePriority(ByVal metaData As System.Data.DataSet)
      Dim Tables As System.Data.DataTable = metaData.Tables("Tables")
      Tables.Columns.Add("DELETE_PRIORITY")

      Dim TableConstraints As System.Data.DataTable = metaData.Tables("TableConstraints")
      Dim ReferentialConstraints As System.Data.DataTable = metaData.Tables("ReferentialConstraints")
      For i As Int32 = 0 To Tables.Rows.Count - 1
         If IsRoot(CType(Tables.Rows(i)("TABLE_NAME"), String), TableConstraints, ReferentialConstraints) Then
            assignInsertPriority(CType(Tables.Rows(i)("TABLE_NAME"), String), 0, Tables, TableConstraints, ReferentialConstraints)
         End If
      Next
   End Sub

   Private Shared Sub assignDeletePriority( _
                     ByVal TableName As String, _
                     ByVal Priority As System.Int32, _
                     ByVal Tables As System.Data.DataTable, _
                     ByVal TableConstraints As System.Data.DataTable, _
                     ByVal ReferentialConstraints As System.Data.DataTable)
      Dim TableRow As System.Data.DataRow = Tables.Select("TABLE_NAME = '" & TableName & "'")(0)
      If (TableRow.IsNull("DELETE_PRIORITY")) OrElse (Priority < Convert.ToInt32(TableRow("DELETE_PRIORITY"))) Then
         TableRow("DELETE_PRIORITY") = Priority
      End If
      Dim TableConstraint As System.Data.DataRow() = TableConstraints.Select("CONSTRAINT_TYPE = 'PRIMARY KEY' and TABLE_NAME = '" & TableName & "'")
      If TableConstraint.Length = 0 Then
         '		return;
      Else
         Dim UniqueConstraintName As String = CType(TableConstraint(0)("CONSTRAINT_NAME"), String)
         Dim ParentRelations As System.Data.DataRow() = ReferentialConstraints.Select("UNIQUE_CONSTRAINT_NAME = '" & UniqueConstraintName & "'")
         For i As Int32 = 0 To ParentRelations.Length - 1
            Dim ChildRelations As System.Data.DataRow() = TableConstraints.Select("CONSTRAINT_TYPE = 'FOREIGN KEY' and CONSTRAINT_NAME = '" & CType(ParentRelations(i)("CONSTRAINT_NAME"), String) & "'")
            '// Only in case of self reference?
            If ChildRelations.Length = 0 Then
               'continue;
               '// protect infinite recursion in case of self reference
            ElseIf (CType(ChildRelations(0)("TABLE_NAME"), String)).Equals(TableName) Then
               'continue;
            Else
               Dim ChildTableName As String = CType(ChildRelations(0)("TABLE_NAME"), String)
               assignInsertPriority(ChildTableName, Priority + 1, Tables, TableConstraints, ReferentialConstraints)
            End If
         Next
      End If
   End Sub

   Private Shared Function IsRoot( _
                     ByVal TableName As String, _
                     ByVal TableConstraints As System.Data.DataTable, _
                     ByVal ReferentialConstraints As System.Data.DataTable) _
                     As Boolean

      Dim Constraints As System.Data.DataRow() = TableConstraints.Select("CONSTRAINT_TYPE = 'FOREIGN KEY' and TABLE_NAME = '" & TableName & "'")
      If Constraints.Length = 0 Then
         Return True      ' CHanged
      Else
         Dim ConstraintName As String = CType(Constraints(0)("CONSTRAINT_NAME"), String)
         Dim ChildRelations As System.Data.DataRow() = ReferentialConstraints.Select("CONSTRAINT_NAME = '" & ConstraintName & "'")
         For i As Int32 = 0 To ChildRelations.Length - 1
            If Not (CType(TableConstraints.Select("CONSTRAINT_TYPE = 'PRIMARY KEY' and CONSTRAINT_NAME = '" & CType(ChildRelations(i)("UNIQUE_CONSTRAINT_NAME"), String) & "'")(0)("TABLE_NAME"), String)).Equals(TableName) Then
               Return False
            End If
         Next
         Return True
      End If
   End Function
End Class
