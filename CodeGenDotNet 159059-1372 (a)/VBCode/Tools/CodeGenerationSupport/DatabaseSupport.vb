' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Utility files for database actions

Option Strict On
Option Explicit On

Imports System
Imports System.Text.RegularExpressions
Imports System.Diagnostics

'! Class Summary: 

Public Class DatabaseSupport
   
#Region "Class level declarations - empty"
#End Region

#Region "Constructors - empty"
#End Region

#Region "Public Methods and Properties"
   Public Shared Function GetConnectionString( _
               ByVal serverName As String, _
               ByVal databaseName As String) _
               As String
      Return "workstation id=" & serverName & ";packet size=4096;integrated security=SSPI;data source=" & serverName & ";persist security info=False;initial catalog=" & databaseName
   End Function

   Public Shared Sub CreateStoredProcFromFile( _
            ByVal fileName As String, _
            ByVal serverName As String, _
            ByVal databaseName As String, _
            ByVal spName As String, _
            ByVal executeUser As String)
      Dim streamReader As New IO.StreamReader(fileName)
      Dim cmd As New Data.SqlClient.SqlCommand
      Dim transaction As Data.SqlClient.SqlTransaction
      Dim SQLStatements() As String
      cmd.Connection = New Data.SqlClient.SqlConnection(CodeGenerationSupport.DatabaseSupport.GetConnectionString(serverName, databaseName))
      cmd.CommandText = "sp_help '" & spName & "'"
      Try
         cmd.Connection.Open()
         transaction = cmd.Connection.BeginTransaction
         cmd.Transaction = transaction
         Dim statements() As String = Regex.Split(streamReader.ReadToEnd, "\sgo\s", RegexOptions.Compiled Or RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase)
         For Each statement As String In statements
            cmd.CommandText = statement
            cmd.ExecuteNonQuery()
         Next
         transaction.Commit()
      Catch ex As System.Exception
         Debug.WriteLine(ex)
         transaction.Rollback()
         Throw
      Finally
         Try
            cmd.Connection.Close()
         Catch
         End Try
      End Try
   End Sub

   'Public Shared Sub CreateStoredProcFromFile( _
   '            ByVal fileName As String, _
   '            ByVal serverName As String, _
   '            ByVal databaseName As String, _
   '            ByVal spName As String, _
   '            ByVal executeUser As String)
   '   Dim streamReader As New IO.StreamReader(fileName)
   '   Dim cmd As New Data.SqlClient.SqlCommand
   '   cmd.Connection = New Data.SqlClient.SqlConnection(CodeGenerationSupport.DatabaseSupport.GetConnectionString(serverName, databaseName))
   '   cmd.CommandText = "sp_help '" & spName & "'"
   '   Try
   '      cmd.Connection.Open()
   '      Try
   '         cmd.ExecuteNonQuery()
   '         cmd.CommandText = "drop procedure [dbo].[" & spName & "]"
   '         cmd.ExecuteNonQuery()
   '      Catch ex As Exception
   '         ' Benign, the stored proc doesn't exist
   '      End Try
   '      cmd.CommandText = streamReader.ReadToEnd
   '      cmd.ExecuteNonQuery()
   '      cmd.CommandText = "GRANT  EXECUTE  ON [dbo].[" & spName & "]  TO [" & executeUser & "]"
   '      cmd.ExecuteNonQuery()
   '   Finally
   '      Try
   '         cmd.Connection.Close()
   '      Catch
   '      End Try
   '   End Try
   'End Sub

#End Region

#Region "Protected and Friend Methods and Properties - empty"
#End Region

#Region "Private Methods and Properties - empty"
#End Region

End Class
