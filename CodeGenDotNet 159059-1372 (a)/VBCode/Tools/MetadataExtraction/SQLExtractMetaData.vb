' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Specialized class for extracting metadata from SQL Server

Option Strict On
Option Explicit On 

Imports System
Imports System.Data
Imports System.Diagnostics

' Class Summary: Metadata extraction for SQL Server. 
'                Assumes Windows authentication

Public Class SQLExtractMetaData
   Inherits ExtractMetaData


#Region "Class level declarations"
   Dim mdtExtendedColumns As New Collections.Hashtable     ' Also hold parameters
   Dim mdtColumnInfo As New Collections.Hashtable     ' Also hold parameters
   Dim mdtStoredProcPrivileges As New Data.DataTable
#End Region

#Region "Constructors"
   Public Sub New(ByVal serverName As String)
      MyBase.New()
      mServerName = serverName
   End Sub
#End Region

#Region "Public Methods and Properties-empty"
#End Region

#Region "Protected and Friend Methods and Properties"
   Protected Function GetConnection( _
                  ByVal databaseName As String) _
                  As SqlClient.SqlConnection
      Return New SqlClient.SqlConnection( _
               "workstation id=" & mServerName & _
               ";packet size=4096;integrated security=SSPI" & _
               ";data source=" & mServerName & _
               ";persist security info=False;initial catalog=" & databaseName)

   End Function

   Protected Overloads Overrides Function DataTableFromSQL( _
                  ByVal SQLText As String, _
                  ByVal databaseName As String, _
                  ByVal tablename As String) _
                  As Data.DataTable
      Dim da As New SqlClient.SqlDataAdapter(SQLText, GetConnection(databaseName))
      Dim dt As New Data.DataTable
      da.Fill(dt)
      dt.TableName = tablename
      Return dt
   End Function

   Protected Overloads Overrides Sub DataTableFromSQL( _
                  ByVal SQLText As String, _
                  ByVal databaseName As String, _
                  ByVal dt As Data.DataTable)
      Dim da As New SqlClient.SqlDataAdapter(SQLText, GetConnection(databaseName))
      da.Fill(dt)
   End Sub

   Protected Overrides Function RunStoredProc( _
                     ByVal spname As String, _
                     ByVal nodeParams As System.Xml.XmlNode, _
                     ByVal databaseName As String) _
                     As System.Data.DataSet
      Dim ds As New Data.DataSet
      Dim da As New SqlClient.SqlDataAdapter
      Dim cmd As New SqlClient.SqlCommand
      Dim def As Object
      Dim name As String
      Dim tr As SqlClient.SqlTransaction

      cmd.Connection = GetConnection(databaseName)
      cmd.CommandType = CommandType.StoredProcedure
      cmd.CommandText = spname
      For Each node As Xml.XmlNode In nodeParams
         Select Case CStr(Utility.Tools.GetAttributeOrEmpty(node, "Type")).ToLower
            Case "char", "varchar", "nchar", "nvarchar"
               def = ""
            Case "uniqueidentifier"
               def = DBNull.Value
            Case "datetime"
               def = "1/1/1873"
            Case Else
               def = 0
         End Select
         name = Utility.Tools.GetAttributeOrEmpty(node, "Name")
         cmd.Parameters.Add(New SqlClient.SqlParameter(name, def))
      Next
      da.SelectCommand = cmd
      Try
         cmd.Connection.Open()
         cmd.CommandTimeout = 5
         tr = cmd.Connection.BeginTransaction(IsolationLevel.Serializable)
         cmd.Transaction = tr
         da.Fill(ds)
         tr.Rollback()
      Finally
         cmd.Connection.Close()
      End Try
      Return ds
   End Function

   Protected Overrides Sub FillMetaDataSet(ByVal name As String)
      mdtExtendedColumns = New Collections.Hashtable
      MyBase.FillMetaDataSet(name)
      mdataset.Tables.Add(DataTableFromSQL( _
                 "SELECT * FROM ::fn_listextendedproperty(null, null, " & _
                 "null, null, null, null, null)", name, "ExtDatabase"))
      mdataset.Tables.Add(DataTableFromSQL( _
                 "SELECT * FROM ::fn_listextendedproperty(null, 'User', " & _
                 "'dbo', 'Table', null, null, null)", name, "ExtTable"))
      mdataset.Tables.Add(DataTableFromSQL( _
                 "SELECT * FROM ::fn_listextendedproperty(null, 'User', " & _
                 "'dbo', 'Procedure', null, null, null)", name, "ExtStoredProc"))
      mdataset.Tables.Add(DataTableFromSQL( _
                 "SELECT * FROM ::fn_listextendedproperty(null, 'User', " & _
                 "'dbo', 'View', null, null, null)", name, "ExtView"))
      mdataset.Tables.Add(DataTableFromSQL( _
                 "SELECT * FROM ::fn_listextendedproperty(null, 'User', " & _
                 "'dbo', 'Function', null, null, null)", name, "ExtFunction"))
      mdtStoredProcPrivileges = DataTableFromSQL("sp_helprotect", name, "StoredProcPrivileges")
   End Sub

   Protected Overloads Overrides Function GetProviderSpecificXMLNodes( _
                  ByVal node As System.Xml.XmlNode, _
                  ByVal databaseName As String, _
                  ByVal schemaName As String) _
                  As System.Xml.XmlNode

      Dim dt As DataTable = mdataset.Tables("ExtDatabase")
      Dim nodeProps As Xml.XmlNode
      Dim nodeProp As Xml.XmlNode
      Dim xmlDoc As Xml.XmlDocument = node.OwnerDocument
      If dt.Rows.Count > 0 Then
         nodeProps = node.AppendChild(Me.CreateElement("ExtendedProperties"))
         For Each row As Data.DataRow In dt.Rows
            nodeProp = nodeProps.AppendChild(Me.CreateElement( _
                  "ExtendedProperty", row("name").ToString))
            nodeProp.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                  mxmlDoc, "Value", row("value").ToString))
         Next
      End If

   End Function

   Protected Overloads Overrides Function GetProviderSpecificXMLNodes( _
                  ByVal node As System.Xml.XmlNode, _
                  ByVal databaseName As String, _
                  ByVal schemaName As String, _
                  ByVal tableName As String, _
                  ByVal modeName As String) _
                  As System.Xml.XmlNode
      If modeName = "StoredProc" Then
         GetExtraStoredProcInfo(node, databaseName, schemaName, tableName, modeName)
      Else
         GetExtraTableInfo(node, databaseName, schemaName, tableName, modeName)
      End If
   End Function

   Protected Overloads Overrides Function GetProviderSpecificXMLNodes( _
                  ByVal node As System.Xml.XmlNode, _
                  ByVal databaseName As String, _
                  ByVal schemaName As String, _
                  ByVal tableName As String, _
                  ByVal modeName As String, _
                  ByVal columnName As String) _
                  As System.Xml.XmlNode
      Dim dt As Data.DataTable
      Dim nodeProps As Xml.XmlNode
      Dim nodeProp As Xml.XmlNode
      Dim xmlDoc As Xml.XmlDocument = node.OwnerDocument
      Dim rows() As Data.DataRow
      Dim extSQL As String

      If mdtColumnInfo.Contains(tableName) Then
         dt = CType(mdtColumnInfo(tableName), Data.DataTable)
      Else
         dt = DataTableFromSQL("sp_columns [" & tableName & "]", databaseName, _
                  "columns")
         mdtColumnInfo.Add(tableName, dt)
      End If
      For Each row As Data.DataRow In dt.Rows
         If CStr(row("COLUMN_NAME")).tolower = columnName.ToLower Then
            If CStr(row("TYPE_NAME")).IndexOf("identity") >= 0 Then
               node.Attributes.Append( _
                       Utility.xmlHelpers.NewAttribute( _
                          node.OwnerDocument, "IsAutoIncrement", "true"))
            End If
         End If
      Next

      If mdtExtendedColumns.Contains(tableName) Then
         dt = CType(mdtExtendedColumns(tableName), Data.DataTable)
      Else
         Select Case modeName
            Case "Table"
               extSQL = "SELECT * FROM ::fn_listextendedproperty(null, " & _
                        "'User', 'dbo', 'Table', '" & tableName & _
                        "', 'Column', null)"
            Case "View"
               extSQL = "SELECT * FROM ::fn_listextendedproperty(null, " & _
                        "'User', 'dbo', 'View', '" & tableName & _
                        "', 'Column', null)"
            Case "StoredProc"
               extSQL = "SELECT * FROM ::fn_listextendedproperty(null, " & _
                        "'User','dbo', 'Procedure', '" & tableName & _
                        "', 'Parameter', null)"
            Case "Function"
               extSQL = "SELECT * FROM ::fn_listextendedproperty(null, " & _
                        "'User', 'dbo', 'Function', '" & tableName & _
                        "', 'Parameter', null)"
         End Select
         dt = DataTableFromSQL(extSQL, databaseName, "ExtProp")
         mdtExtendedColumns.Add(tableName, dt)
      End If
      rows = dt.Select("objname='" & columnName & "'")
      If rows.GetLength(0) > 0 Then
         For Each row As Data.DataRow In rows
            node.Attributes.Append( _
                     Utility.xmlHelpers.NewAttribute( _
                           node.OwnerDocument, _
                           row("name").ToString, row("value").ToString))
         Next
      End If
   End Function

   Protected Overridable Function GetExtraTableInfo( _
               ByVal node As System.Xml.XmlNode, _
               ByVal databaseName As String, _
               ByVal schemaName As String, _
               ByVal tableName As String, _
               ByVal modeName As String) _
               As System.Xml.XmlNode

      Dim extTableName As String = "Ext" & node.LocalName
      Dim dt As Data.DataTable = mdataset.Tables(extTableName)
      Dim nodeProps As Xml.XmlNode
      Dim nodeProp As Xml.XmlNode
      Dim xmlDoc As Xml.XmlDocument = node.OwnerDocument
      Dim rows() As Data.DataRow = dt.Select("objname='" & tableName & "'")
      If dt.Rows.Count > 0 Then
         nodeProps = node.AppendChild(Me.CreateElement("ExtendedProperties"))
         For Each row As Data.DataRow In rows
            nodeProp = nodeProps.AppendChild(Me.CreateElement( _
                  "ExtendedProperty", row("name").ToString))
            nodeProp.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                  mxmlDoc, "Value", row("value").ToString))
         Next
      End If
   End Function

   Protected Overridable Function GetExtraStoredProcInfo( _
               ByVal node As System.Xml.XmlNode, _
               ByVal databaseName As String, _
               ByVal schemaName As String, _
               ByVal tableName As String, _
               ByVal modeName As String) _
               As System.Xml.XmlNode
      ' Get Stored Proc Privileges
      Dim rowPrivileges As Data.DataRow()
      rowPrivileges = mdtStoredProcPrivileges.Select("Object = '" & tableName & "'")
      Dim nodePrivileges As Xml.XmlNode = Me.CreateElement(modeName & "Privileges")
      Dim nodePrivilege As Xml.XmlNode
      For Each rowPrivilege As Data.DataRow In rowPrivileges
         nodePrivilege = Me.CreateElement(modeName & "Privilege")
         nodePrivilege.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                       mXMLDoc, "Grantor", rowPrivilege("Grantor").ToString))
         nodePrivilege.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                       mXMLDoc, "Grantee", rowPrivilege("Grantee").ToString))
         nodePrivilege.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                       mXMLDoc, "Type", rowPrivilege("Action").ToString))
         nodePrivilege.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                       mXMLDoc, "ProtectType", rowPrivilege("ProtectType").ToString))
         nodePrivileges.AppendChild(nodePrivilege)
         ' NOTE: We are not yet supporting column privileges or Deny access!!!
      Next
      node.AppendChild(nodePrivileges)

   End Function

   Protected Overrides Function GetTables( _
                        ByVal databaseName As String) _
                        As System.Data.DataTable
      Dim dta As Data.DataTable
      dta = MyBase.GetTables(databaseName)
      ' This is an ugly hack because the data diagram table dtProperties
      ' is marked by SQL as a user table
      Dim rows() As Data.DataRow
      rows = dta.Select("TABLE_NAME = 'dtproperties'")
      If rows.GetLength(0) > 0 Then
         dta.Rows.Remove(rows(0))
      End If
      Return dta
   End Function


#End Region

#Region "Private Methods and Properties-empty"
#End Region

End Class
