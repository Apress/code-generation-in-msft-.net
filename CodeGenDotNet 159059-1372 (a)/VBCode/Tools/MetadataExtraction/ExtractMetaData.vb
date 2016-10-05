' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Main class for extracting metadata from SQL-92 databases

Option Strict On
Option Explicit On 

Imports System
Imports System.Data
Imports System.Diagnostics
Imports KADGen

'! Class Summary: Metadata extracton based on the SQL-92 INFORMATION_SCHEMA standard. Tested only against SQL Server

Public MustInherit Class ExtractMetaData
   Implements IExtractMetaData

#Region "Class level declarations"
   Protected mServerName As String
   Private mUseVerboseNames As Boolean = False
   Private mUseProcContents As Boolean = False
   Protected mXMLDoc As Xml.XmlDocument
   Protected mDataSet As Data.DataSet
   Protected mSelectPatterns() As String
   Protected mSetSelectPatterns() As String
   Protected mRemovePrefix As String

   Private Enum ErrorResponse
      Continue
      Abort
   End Enum

#End Region

#Region "Constructors -empty"
#End Region

#Region "MustOverrides"
   Protected MustOverride Function DataTableFromSQL( _
                  ByVal SQLText As String, _
                  ByVal databaseName As String, _
                  ByVal tablename As String) _
                  As Data.DataTable

   Protected MustOverride Sub DataTableFromSQL( _
                  ByVal SQLText As String, _
                  ByVal tablename As String, _
                  ByVal dt As Data.DataTable)

   Protected MustOverride Function RunStoredProc( _
                  ByVal spname As String, _
                  ByVal nodeParams As Xml.XmlNode, _
                  ByVal databaseName As String) _
                  As Data.DataSet

#End Region

#Region "Public Methods and Properties"

   Public Property UseVerboseNames() _
               As Boolean _
               Implements IExtractMetaData.UseVerboseNames
      Get
         Return Me.mUseVerboseNames
      End Get
      Set(ByVal value As Boolean)
         Me.mUseVerboseNames = value
      End Set
   End Property

   Public Property UseProcContents() _
               As Boolean _
               Implements IExtractMetaData.UseProcContents
      Get
         Return Me.mUseProcContents
      End Get
      Set(ByVal value As Boolean)
         Me.mUseProcContents = value
      End Set
   End Property

   Public Property ServerName() _
               As String _
               Implements IExtractMetaData.ServerName
      Get
         Return Me.mServerName
      End Get
      Set(ByVal value As String)
         Me.mServerName = value
      End Set
   End Property

   Public Function CreateMetaData( _
               ByVal skipStoredProcs As Boolean, _
               ByVal selectPatterns As String, _
               ByVal setSelectPatterns As String, _
               ByVal removePrefix As String, _
               ByVal lookupPrefix As String, _
               ByVal ParamArray databaseNames() As String) _
               As Xml.XmlDocument _
               Implements IExtractMetaData.CreateMetaData
      mXMLDoc = New Xml.XmlDocument
      Dim nodeRoot As Xml.XmlElement = mXMLDoc.CreateElement("dbs", _
               "MetaDataRoot", "http://kadgen/DatabaseStructure")
      nodeRoot.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(mXMLDoc, "FreeForm", True))
      Dim nodeDatabase As Xml.XmlElement
      ' Retrieve database names if needed
      If (databaseNames.GetLength(0) = 0) Then
         Dim dtDatabases As Data.DataTable = GetDatabases()
         ReDim databaseNames(dtDatabases.Rows.Count - 1)
         For i As Int32 = 0 To dtDatabases.Rows.Count - 1
            databaseNames(i) = dtDatabases.Rows(i)("CATALOG_NAME").ToString
         Next
      End If

      Me.SetSelectPatterns(selectPatterns, setSelectPatterns)
      Me.mRemovePrefix = removePrefix

      mXMLDoc.AppendChild(mXMLDoc.CreateXmlDeclaration("1.0", "UTF-8", ""))
      mXMLDoc.AppendChild(nodeRoot)
      Dim node As Xml.XmlElement = Me.CreateElement("DataStructures")
      nodeRoot.AppendChild(node)
      For Each name As String In databaseNames
         mDataSet = Nothing
         nodeDatabase = Me.CreateElement("DataStructure", name)
         node.AppendChild(nodeDatabase)
         FillMetaDataSet(name)
         'Dim obj As New t
         TableInsertDeletePriority.appendTableInsertPriority(mDataSet)
         TableInsertDeletePriority.appendTableDeletePriority(mDataSet)
         nodeDatabase.AppendChild(Me.GetAllTablesXML(lookupPrefix))
         nodeDatabase.AppendChild(Me.GetAllUserDefinedTypesXML())
         If Not skipStoredProcs Then
            nodeDatabase.AppendChild(Me.GetAllStoredProcXML(name))
            nodeDatabase.AppendChild(Me.GetAllFunctionsXML())
         End If
         nodeDatabase.AppendChild(Me.GetAllViewsXML())
         GetProviderSpecificXMLNodes(nodeDatabase, _
               mDataSet.Tables("Database").Rows(0)("CATALOG_NAME").ToString, _
               mDataSet.Tables("Database").Rows(0)("SCHEMA_NAME").ToString)
         nodeDatabase.AppendChild(Me.GetHierarchyXML())
      Next
      Return mXMLDoc
   End Function

#End Region

#Region "Protected and Friend Methods and Properties"
   Protected Overridable Function GetAllTablesXML(ByVal lookupPrefix As String) _
                  As Xml.XmlNode
      Dim nodeTables As Xml.XmlElement = Me.CreateElement("Tables")
      nodeTables.Prefix = "dbs"
      For Each rowTable As Data.DataRow In mDataSet.Tables("Tables").Rows
         ' Each row represents a datatable
         nodeTables.AppendChild(GetTableXML(rowTable, "Table", lookupPrefix))
      Next
      Return nodeTables
   End Function

   Protected Overridable Function GetAllViewsXML() _
                  As Xml.XmlNode
      Dim nodeTables As Xml.XmlElement = Me.CreateElement("Views")
      For Each rowTable As Data.DataRow In mDataSet.Tables("Views").Rows
         ' Each row represents a datatable
         nodeTables.AppendChild(GetTableXML(rowTable, "View", ""))
      Next
      Return nodeTables
   End Function

   Protected Overridable Function GetAllUserDefinedTypesXML() _
                  As Xml.XmlNode
      Dim nodeUDTs As Xml.XmlElement = Me.CreateElement("UserDefinedTypes")
      For Each rowUDT As Data.DataRow In mDataSet.Tables("UserDefinedTypes").Rows
         ' Each row represents a dataUDT
         nodeUDTs.AppendChild(GetUDTXML(rowUDT))
      Next
      Return nodeUDTs
   End Function

   Protected Overridable Function GetAllStoredProcXML( _
                  ByVal databaseName As String) _
                  As Xml.XmlNode
      Dim nodeStoredProcs As Xml.XmlElement = Me.CreateElement("StoredProcs")
      For Each rowStoredProc As Data.DataRow In mDataSet.Tables("SProcs").Rows
         ' Each row represents a dataStoredProc
         Utility.xmlHelpers.AppendIfExists(nodeStoredProcs, _
                     GetStoredProcXML(rowStoredProc, _
                                "StoredProc", _
                                databaseName))
      Next
      Return nodeStoredProcs
   End Function

   Protected Overridable Function GetAllFunctionsXML() _
                  As Xml.XmlNode
      Dim nodeStoredProcs As Xml.XmlElement = Me.CreateElement("Functions")
      For Each rowStoredProc As Data.DataRow In mDataSet.Tables("Functions").Rows
         ' Each row represents a dataStoredProc
         nodeStoredProcs.AppendChild(GetStoredProcXML(rowStoredProc, "Function", ""))
      Next
      Return nodeStoredProcs
   End Function

   Protected Overridable Function GetHierarchyXML() _
                As Xml.XmlNode
      Dim nsmgr As Xml.XmlNamespaceManager = Me.GetNameSpaceManager(mXMLDoc, "dbs")
      Dim nodeHierarchy As Xml.XmlNode = Me.CreateElement("Hierarchy")
      Dim nodeParents As Xml.XmlNodeList
      Dim allNodes As Xml.XmlNodeList = mXMLDoc.SelectNodes("//dbs:Table", nsmgr)

      For Each node As Xml.XmlNode In allNodes
         nodeParents = node.SelectNodes( _
                  "dbs:TableConstraints/dbs:TableRelations/dbs:ParentTables/" & _
                  "dbs:ParentTable", nsmgr)
         If nodeParents.Count = 0 OrElse (nodeParents.Count = 1 And _
                  Utility.Tools.GetAttributeOrEmpty(nodeParents(0), "Name") = _
                           Utility.Tools.GetAttributeOrEmpty(node, "Name")) Then
            AddHierarchyNode(nodeHierarchy, node, nsmgr)
         End If
      Next
      Return nodeHierarchy
   End Function

   Protected Overridable Sub AddHierarchyNode( _
                     ByVal nodeParent As Xml.XmlNode, _
                     ByVal node As Xml.XmlNode, _
                     ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim nodeList As Xml.XmlNodeList
      Dim nodeTable As Xml.XmlNode
      Dim nodeNew As Xml.XmlNode
      Dim nodeCircularCheck As Xml.XmlNode
      nodeNew = Me.CreateElement("HTable", _
                  Utility.Tools.GetAttributeOrEmpty(node, "Name"))
      nodeParent.AppendChild(nodeNew)
      nodeList = node.SelectNodes( _
                  "dbs:TableConstraints/dbs:TableRelations/dbs:ChildTables/" & _
                  "dbs:ChildTable", nsmgr)
      For Each nodeChild As Xml.XmlNode In nodeList
         If nodeNew.SelectSingleNode( _
                     "ancestor-or-self::dbs:HTable[@Name='" & _
                     Utility.Tools.GetAttributeOrEmpty( _
                           nodeChild, "Name") & "']", nsmgr) Is Nothing Then
            nodeTable = node.SelectSingleNode( _
                     "ancestor::dbs:DataStructure/dbs:Tables/dbs:Table[@Name='" & _
                     Utility.Tools.GetAttributeOrEmpty(nodeChild, "Name") & "']", nsmgr)
            AddHierarchyNode(nodeNew, nodeTable, nsmgr)
         End If
      Next
   End Sub

   Protected Overridable Function GetTableXML( _
                  ByVal rowTable As Data.DataRow, _
                  ByVal modeName As String, _
                  ByVal lookupPrefix As String) _
                  As Xml.XmlNode
      Dim tableName As String = rowTable("TABLE_NAME").ToString
      Dim rowColumns() As Data.DataRow = mDataSet.Tables("Columns").Select( _
                           TableMatch(rowTable))
      Dim nodeColumns As Xml.XmlNode
      Dim originalName As String = rowTable("TABLE_NAME").ToString
      Dim isLookup As Boolean
      Dim name As String = Utility.Tools.FixName( _
                           originalName, mRemovePrefix)
      Dim singularName As String = Utility.Tools.GetSingular(name)
      Dim pluralName As String = Utility.Tools.GetPlural(name)
      ' Changed to singular 12/3/3 KAD
      Dim nodeTable As Xml.XmlNode = NewElementWithName(modeName, _
                           singularName, rowTable, "TABLE")
      nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "OriginalName", _
                           originalName))
      nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "Prefix", _
                           Utility.Tools.GetPrefix(originalName)))
      nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "SingularName", _
                           singularName))
      nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "PluralName", _
                           pluralName))
      nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "DisplayName", _
                           GetDisplayName(rowColumns, singularName)))
      nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "Caption", _
                           MakeCaption(singularName)))
      nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "PluralCaption", _
                           MakeCaption(pluralName)))
      If (lookupPrefix.Length > 0) AndAlso originalName.StartsWith(lookupPrefix) Then
         isLookup = True
         nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "IsLookup", _
                           "true"))
      End If

      If modeName = "Table" Then
         nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "DeletePriority", _
                           rowTable("DELETE_PRIORITY").ToString))
         nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           nodeTable.OwnerDocument, "InsertPriority", _
                           rowTable("LOAD_PRIORITY").ToString))
      End If
      nodeColumns = nodeTable.AppendChild(Me.CreateElement(modeName & "Columns"))
      For Each rowColumn As Data.DataRow In rowColumns
         nodeColumns.AppendChild(GetColumnXML(rowTable, rowColumn, modeName, isLookup))
      Next
      nodeTable.AppendChild(nodeColumns)
      nodeTable.AppendChild(GetPrivilegesXML(modeName, mDataSet.Tables( _
                           "TablePrivileges").Select(TableMatch(rowTable))))
      nodeTable.AppendChild(GetTableConstraintsXML(modeName, rowTable))
      GetProviderSpecificXMLNodes(nodeTable, rowTable("TABLE_CATALOG").ToString, _
                           rowTable("TABLE_SCHEMA").ToString, _
                           rowTable("TABLE_NAME").ToString, _
                           modeName)
      Return nodeTable
   End Function

   Protected Overridable Function GetColumnXML( _
                  ByVal rowtable As Data.DataRow, _
                  ByVal rowColumn As Data.DataRow, _
                  ByVal modeName As String, _
                  ByVal isLookup As Boolean) _
                  As Xml.XmlNode
      Dim columnName As String
      Dim isPrimaryKey As Boolean
      Dim nodeColumn As Xml.XmlNode = NewElementWithName(modeName & "Column", _
                        Utility.Tools.FixName(rowColumn("COLUMN_NAME").ToString, _
                                       Me.mRemovePrefix), _
                        rowColumn, "TABLE")
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        nodeColumn.OwnerDocument, "OriginalName", _
                        rowColumn("COLUMN_NAME").ToString))
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        nodeColumn.OwnerDocument, "Caption", _
                        MakeCaption(rowColumn("COLUMN_NAME").ToString)))
      Dim rows() As Data.DataRow
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute(mXMLDoc, _
                        "Ordinal", rowColumn("ORDINAL_POSITION").ToString))
      Dim def As String = rowColumn("COLUMN_DEFAULT").ToString
      If def.Length > 0 Then
         nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute(mXMLDoc, _
                        "Default", def))
      End If
      If rowColumn("IS_NULLABLE").ToString.ToLower = "yes" Then
         nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "AllowNulls", "true"))
      Else
         nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "AllowNulls", "false"))
      End If
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "SQLType", Utility.Tools.GetSQLTypeFromSQLType( _
                                       rowColumn("DATA_TYPE").ToString)))
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "NETType", Utility.Tools.GetNETTypeFromSQLType( _
                                       rowColumn("DATA_TYPE").ToString)))
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "MaxLength", _
                        rowColumn("CHARACTER_MAXIMUM_LENGTH").ToString))
      isPrimaryKey = Me.IsPrimaryKey(rowtable, rowColumn)
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "IsPrimaryKey", _
                        isPrimaryKey.ToString.ToLower))
      If isLookup And Not isPrimaryKey Then
         nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           mXMLDoc, "UseForDesc", _
                           "true"))
      End If

      If rowColumn("DOMAIN_NAME").ToString.Length > 0 Then
         nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "UDTCatalog", _
                        rowColumn("DOMAIN_CATALOG").ToString))
         nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "UDTOwner", rowColumn("DOMAIN_SCHEMA").ToString))
         nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "UDTName", rowColumn("DOMAIN_NAME").ToString))
      End If
      nodeColumn.AppendChild(GetPrivilegesXML(modeName & "Column", _
                        mDataSet.Tables("ColumnPrivileges").Select( _
                                    ColumnMatch(rowtable, rowColumn))))
      rows = mDataSet.Tables("ColumnConstraints").Select( _
                                    ColumnMatch(rowtable, rowColumn))
      If rows.GetLength(0) > 0 Then
         nodeColumn.AppendChild(GetCheckConstraintsXML("Check", rows))
      End If
      GetProviderSpecificXMLNodes(nodeColumn, rowtable("TABLE_CATALOG").ToString, _
                       rowtable("TABLE_SCHEMA").ToString, _
                       rowtable("TABLE_NAME").ToString, modeName, _
                       rowColumn("COLUMN_NAME").ToString)
      Return nodeColumn
   End Function

   Protected Overridable Function GetPrivilegesXML( _
               ByVal modeName As String, _
               ByVal rowPrivileges() As Data.DataRow) _
               As Xml.XmlNode
      Dim nodePrivileges As Xml.XmlNode = Me.CreateElement(modeName & "Privileges")
      Dim nodePrivilege As Xml.XmlNode
      For Each rowPrivilege As Data.DataRow In rowPrivileges
         nodePrivilege = Me.CreateElement(modeName & "Privilege")
         nodePrivilege.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                       mXMLDoc, "Grantor", rowPrivilege("GRANTOR").ToString))
         nodePrivilege.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                       mXMLDoc, "Grantee", rowPrivilege("GRANTEE").ToString))
         nodePrivilege.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                       mXMLDoc, "Type", rowPrivilege("PRIVILEGE_TYPE").ToString))
         nodePrivileges.AppendChild(nodePrivilege)
      Next
      Return nodePrivileges
   End Function

   Public Overridable Function GetCheckConstraintsXML( _
                  ByVal modeName As String, _
                  ByVal rowConstraints() As Data.DataRow) _
                  As Xml.XmlNode
      Dim nodeConstraints As Xml.XmlNode
      Dim nodeConstraint As Xml.XmlNode
      Dim rowChecks() As Data.DataRow
      nodeConstraints = Me.CreateElement(modeName & "Constraints")
      For Each rowConstraint As Data.DataRow In rowConstraints
         rowChecks = mDataSet.Tables("CheckConstraints").Select( _
                       ConstraintMatch(rowConstraint))
         If rowChecks.GetLength(0) > 0 Then
            nodeConstraint = Me.CreateElement(modeName & "Constraint")
            For Each rowCheck As Data.DataRow In rowChecks
               nodeConstraint.Attributes.Append( _
                     Utility.xmlHelpers.NewAttribute(mXMLDoc, "Clause", _
                     rowCheck("CHECK_CLAUSE").ToString))
            Next
            nodeConstraints.AppendChild(nodeConstraint)
         End If
      Next
      Return nodeConstraints
   End Function

   Protected Overridable Function GetTableConstraintsXML( _
                  ByVal modeName As String, _
                  ByVal rowTable As Data.DataRow) _
                  As Xml.XmlNode
      Dim nodeConstraints As Xml.XmlNode
      nodeConstraints = Me.CreateElement(modeName & "Constraints")
      AddPrimaryKeyXML(nodeConstraints, rowTable)
      AddRelationsXML(modeName, nodeConstraints, rowTable)
      AddUniqueConstraintXML(modeName, nodeConstraints, rowTable)
      AddTableCheckConstraintXML(nodeConstraints, rowTable)
      Return nodeConstraints
   End Function

   Protected Overridable Function GetUDTXML( _
                  ByVal rowUDT As Data.DataRow) _
                  As Xml.XmlNode
      Dim nodeUDT As Xml.XmlNode = NewElementWithName("UserDefinedType", _
                        rowUDT("DOMAIN_NAME").ToString, rowUDT, "DOMAIN")
      Dim rowConstraints() As Data.DataRow = mDataSet.Tables( _
                        "UDTConstraints").Select(UDTMatch(rowUDT))
      nodeUDT.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "Default", rowUDT("DOMAIN_DEFAULT").ToString))
      nodeUDT.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "Type", rowUDT("DATA_TYPE").ToString))
      nodeUDT.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "MaxLength", _
                        rowUDT("CHARACTER_MAXIMUM_LENGTH").ToString))
      If rowConstraints.GetLength(0) > 0 Then
         nodeUDT.AppendChild(GetCheckConstraintsXML("UDTCheck", rowConstraints))
      End If
      Return nodeUDT
   End Function

   Protected Overridable Function GetStoredProcXML( _
                  ByVal rowSProc As Data.DataRow, _
                  ByVal modeName As String, _
                  ByVal databaseName As String) _
                  As Xml.XmlNode
      Dim spName As String = rowSProc("ROUTINE_NAME").ToString
      Dim nodeSProc As Xml.XmlNode
      If Not spName.ToLower.StartsWith("dt_") Then
         nodeSProc = NewElementWithName(modeName, _
                           spName, rowSProc, "ROUTINE")
         Dim nodeParams As Xml.XmlNode = Me.CreateElement(modeName & "Parameters")
         Dim rowParams() As Data.DataRow = mDataSet.Tables("Parameters").Select( _
                           GenericMatch(rowSProc, "SPECIFIC_", "SPECIFIC_"))
         nodeSProc.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           mXMLDoc, "Created", rowSProc("CREATED").ToString))
         nodeSProc.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           mXMLDoc, "Modified", rowSProc("LAST_ALTERED").ToString))
         If mUseProcContents Then
            Dim nodeProcContents As Xml.XmlNode = Me.CreateElement("SourceCode")
            nodeProcContents.AppendChild(mXMLDoc.CreateTextNode( _
                           rowSProc("ROUTINE_DEFINITION").ToString))
            nodeSProc.AppendChild(nodeProcContents)
         End If
         nodeSProc.AppendChild(nodeParams)
         For Each rowparam As Data.DataRow In rowParams
            nodeParams.AppendChild(GetParameterXML(rowSProc, rowparam, modeName))
         Next
         If RetrieveRecordset(spName) Then
            nodeSProc.AppendChild(GetRecordsetXML(rowSProc, nodeParams, databaseName))
         End If
         GetProviderSpecificXMLNodes(nodeSProc, _
                           rowSProc("ROUTINE_CATALOG").ToString, _
                           rowSProc("ROUTINE_SCHEMA").ToString, _
                           rowSProc("ROUTINE_NAME").ToString, _
                           "StoredProc")
      End If
      Return nodeSProc
   End Function

   Protected Overridable Function GetRecordsetXML( _
                     ByVal rowSProc As Data.DataRow, _
                     ByVal nodeParams As Xml.XmlNode, _
                     ByVal databaseName As String) _
                     As Xml.XmlNode
      Dim nodeDataSet As Xml.XmlNode = Me.CreateElement("DataSet")
      Dim ds As Data.DataSet
      Dim nodeRecordset As Xml.XmlNode
      Dim nodeColumn As Xml.XmlNode
      Try
         ds = RunStoredProc(rowSProc("ROUTINE_NAME").ToString, nodeParams, databaseName)
         For Each dt As Data.DataTable In ds.Tables
            nodeRecordset = Me.CreateElement("Recordset", dt.TableName)
            For Each col As Data.DataColumn In dt.Columns
               nodeColumn = Me.CreateElement("Column", col.ColumnName)
               nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                         mXMLDoc, "NETType", col.DataType.ToString))
               nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                         mXMLDoc, "Caption", MakeCaption(col.Caption)))
               nodeColumn.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute( _
                         mXMLDoc, "AllowDBNull", CBool(col.AllowDBNull)))
               nodeColumn.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute( _
                         mXMLDoc, "AutoIncrement", CBool(col.AutoIncrement)))
               nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                         mXMLDoc, "DefaultValue", col.DefaultValue.ToString))
               nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                         mXMLDoc, "MaxLength", col.MaxLength.ToString))
               nodeColumn.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute( _
                         mXMLDoc, "Unique", CBool(col.Unique)))
               nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                         mXMLDoc, "Ordinal", col.Ordinal.ToString))
               nodeRecordset.AppendChild(nodeColumn)
            Next
            nodeDataSet.AppendChild(nodeRecordset)
         Next
      Catch ex As System.Exception
         ' Some stored procs will fail this attempt
         LogError(ex)
      End Try
      Return nodeDataSet
   End Function

   Protected Overridable Function GetParameterXML( _
                  ByVal rowSProc As Data.DataRow, _
                  ByVal rowParam As Data.DataRow, _
                  ByVal modeName As String) _
                  As Xml.XmlNode
      Dim nodeColumn As Xml.XmlNode
      Dim rows() As Data.DataRow
      nodeColumn = NewElementWithName(modeName & "Parameter", _
                        rowParam("PARAMETER_NAME").ToString, rowParam, "SPECIFIC")
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "Ordinal", rowParam("ORDINAL_POSITION").ToString))
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "Direction", rowParam("PARAMETER_MODE").ToString))
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "Type", rowParam("DATA_TYPE").ToString))
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "MaxLength", _
                        rowParam("CHARACTER_MAXIMUM_LENGTH").ToString))
      nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "Direction", rowParam("PARAMETER_MODE").ToString))
      GetProviderSpecificXMLNodes(nodeColumn, _
                        rowSProc("ROUTINE_CATALOG").ToString, _
                        rowSProc("ROUTINE_SCHEMA").ToString, _
                        rowSProc("ROUTINE_NAME").ToString, "StoredProc", _
                        rowParam("PARAMETER_NAME").ToString)
      Return nodeColumn
   End Function

   Protected Overridable Sub AddPrimaryKeyXML( _
                  ByVal nodeParent As Xml.XmlNode, _
                  ByVal rowTable As Data.DataRow)
      Dim rowPrimaryKey As Data.DataRow = GetPrimaryKeyConstraint(rowTable)
      If Not rowPrimaryKey Is Nothing Then
         Dim nodeConstraint As Xml.XmlNode = Me.CreateElement("PrimaryKey")
         AddKeyColumns(nodeParent.OwnerDocument, nodeConstraint, _
                        mDataSet.Tables("KeyColumns").Select( _
                                 ConstraintMatch(rowPrimaryKey)), "PKField")
         nodeParent.AppendChild(nodeConstraint)
      End If
   End Sub

   Protected Overridable Function GetPrimaryKeyConstraint( _
                  ByVal rowtable As Data.DataRow) _
                  As Data.DataRow
      Dim rowCOnstraints() As Data.DataRow
      rowCOnstraints = mDataSet.Tables("TableConstraints").Select( _
                     TableMatch(rowtable) & " AND CONSTRAINT_TYPE='PRIMARY KEY'")
      Select Case rowCOnstraints.GetLength(0)
         Case 0
            ' No problem, just return nothing
         Case 1
            Return rowCOnstraints(0)
         Case Else
            Throw New System.Exception( _
                     "More than one primary key found on table " & _
                     rowtable("TABLE_NAME").ToString)
      End Select
   End Function

   Protected Overridable Function IsPrimaryKey( _
                     ByVal rowTable As Data.DataRow, _
                     ByVal rowColumn As Data.DataRow) _
                     As Boolean
      Dim rowPrimaryKey As Data.DataRow = GetPrimaryKeyConstraint(rowTable)
      Dim rowKeyColumns() As Data.DataRow
      If Not rowPrimaryKey Is Nothing Then
         rowKeyColumns = mDataSet.Tables("KeyColumns").Select( _
                                    ConstraintMatch(rowPrimaryKey) & _
                                             " AND COLUMN_NAME='" & _
                                             rowColumn("COLUMN_NAME").ToString & "'")
         Return (rowKeyColumns.GetLength(0) > 0)
      End If
   End Function

   Protected Overridable Sub AddUniqueConstraintXML( _
                  ByVal modeName As String, _
                  ByVal nodeParent As Xml.XmlNode, _
                  ByVal rowTable As Data.DataRow)
      Dim rowConstraints() As Data.DataRow = mDataSet.Tables( _
                        "TableConstraints").Select(TableMatch(rowTable) & _
                        " AND CONSTRAINT_TYPE='UNIQUE'")
      For Each row As Data.DataRow In rowCOnstraints
         Dim nodeConstraint As Xml.XmlNode = Me.CreateElement("Unique")
         AddKeyColumns(nodeParent.OwnerDocument, nodeConstraint, _
                        mDataSet.Tables("KeyColumns").Select(ConstraintMatch( _
                              row)), "UniqueField")
         nodeParent.AppendChild(nodeConstraint)
      Next
   End Sub

   Protected Overridable Sub AddRelationsXML( _
                  ByVal modeName As String, _
                  ByVal nodeparent As Xml.XmlNode, _
                  ByVal rowtable As Data.DataRow)
      Dim xmlDoc As Xml.XmlDocument = nodeparent.OwnerDocument
      Dim nodeRelations As Xml.XmlNode = Me.CreateElement(modeName & "Relations")
      Dim nodeRelation As Xml.XmlNode
      Dim nodeRelated As Xml.XmlNode
      Dim rowKeys() As Data.DataRow
      Dim rowConstraints() As Data.DataRow
      Dim rowRefConstraints() As Data.DataRow
      Dim selectClause As String

      ' To create child relations, 
      ' -find the primary key, 
      ' -then find the referential constraints with this as unique side of the key
      ' -finally, get those relations keys
      Dim rowPrimaryKey As Data.DataRow = GetPrimaryKeyConstraint(rowtable)
      If Not rowPrimaryKey Is Nothing Then
         ' Child relations are possible
         rowRefConstraints = mDataSet.Tables("ReferentialConstraints").Select( _
                        ConstraintMatch(rowPrimaryKey, "", "UNIQUE_"))
         If rowRefConstraints.GetLength(0) > 0 Then
            nodeRelation = Me.CreateElement("ChildTables")
            For Each rowRefConstraint As Data.DataRow In rowRefConstraints
               rowKeys = mDataSet.Tables("KeyColumns").Select(ConstraintMatch( _
                           rowRefConstraint))
               ' Changed to singular 12/3/3 KAD
               nodeRelated = Me.CreateElement("ChildTable", Utility.Tools.FixName( _
                           Utility.Tools.GetSingular(rowKeys(0)("TABLE_NAME").ToString), _
                           Me.mRemovePrefix))
               AddKeyColumns(xmlDoc, nodeRelated, rowKeys, "ChildKeyField")
               nodeRelation.AppendChild(nodeRelated)
            Next
            nodeRelations.AppendChild(nodeRelation)
         End If
      End If

      ' To find all of the parent relations
      ' - Find all the foriegn key relations for this table
      ' - look up these referential constraints
      ' - get those relations keys
      rowConstraints = mDataSet.Tables("TableConstraints").Select( _
                     TableMatch(rowtable) & " AND CONSTRAINT_TYPE='FOREIGN KEY'")
      If rowConstraints.GetLength(0) > 0 Then
         nodeRelation = Me.CreateElement("ParentTables")
         For Each rowConstraint As Data.DataRow In rowConstraints
            rowRefConstraints = mDataSet.Tables("ReferentialConstraints").Select( _
                     ConstraintMatch(rowConstraint))
            For Each rowRefConstraint As Data.DataRow In rowRefConstraints
               rowKeys = mDataSet.Tables("KeyColumns").Select( _
                     ConstraintMatch(rowRefConstraint, "UNIQUE_", ""))
               nodeRelated = Me.CreateElement("ParentTable")
               nodeRelated.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                     xmlDoc, "Name", _
                     Utility.Tools.FixName(rowKeys(0)("TABLE_NAME").ToString, _
                              Me.mRemovePrefix)))
               AddKeyColumns(xmlDoc, nodeRelated, rowKeys, "ParentKeyField")
               nodeRelation.AppendChild(nodeRelated)

               rowKeys = mDataSet.Tables("KeyColumns").Select( _
                     ConstraintMatch(rowRefConstraint))
               AddKeyColumns(xmlDoc, nodeRelated, rowKeys, "ChildField")
            Next
         Next
         nodeRelations.AppendChild(nodeRelation)
      End If
      nodeparent.AppendChild(nodeRelations)
   End Sub

   Protected Overridable Sub AddTableCheckConstraintXML( _
                  ByVal nodeParent As Xml.XmlNode, _
                  ByVal rowtable As Data.DataRow)
      Dim xmlDoc As Xml.XmlDocument = nodeParent.OwnerDocument
      Dim rowCOnstraints() As Data.DataRow
      Dim nodeCheck As Xml.XmlNode
      Dim rowChecks() As Data.DataRow

      rowCOnstraints = mDataSet.Tables("TableConstraints").Select( _
                     TableMatch(rowtable) & " AND CONSTRAINT_TYPE='CHECK'")
      If rowCOnstraints.GetLength(0) > 0 Then
         For Each rowConstraint As Data.DataRow In rowCOnstraints
            rowChecks = mDataSet.Tables("CheckConstraints").Select( _
                     ConstraintMatch(rowConstraint))
            For Each row As Data.DataRow In rowChecks
               nodeCheck = Me.CreateElement("TableCheckConstraints")
               nodeCheck.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                     xmlDoc, "Clause", row("CHECK_CLAUSE").ToString))
               nodeParent.AppendChild(nodeCheck)
            Next
         Next
      End If
   End Sub

   Protected Overridable Function GetProviderSpecificXMLNodes( _
                  ByVal node As Xml.XmlNode, _
                  ByVal databaseName As String, _
                  ByVal schemaName As String) _
                  As Xml.XmlNode
      'NOTE: This follows a pattern of passing in the parent node, because I 
      '      don't know what types of changes you'll make - attributes, multiple 
      '      child nodes, etc.
      ' Override in child class to provide special behavior for database
   End Function

   Protected Overridable Function GetProviderSpecificXMLNodes( _
                  ByVal node As Xml.XmlNode, _
                  ByVal databaseName As String, _
                  ByVal schemaName As String, _
                  ByVal tableName As String, _
                  ByVal modeName As String) _
                  As Xml.XmlNode
      'NOTE: This follows a pattern of passing in the parent node, because I 
      '      don't know what types of changes you'll make - attributes, multiple 
      '      child nodes, etc.
      ' Override in child class to provide special behavior for table
   End Function

   Protected Overridable Function GetProviderSpecificXMLNodes( _
                  ByVal node As Xml.XmlNode, _
                  ByVal databaseName As String, _
                  ByVal schemaName As String, _
                  ByVal tableName As String, _
                  ByVal modeName As String, _
                  ByVal columnName As String) _
                  As Xml.XmlNode
      'NOTE: This follows a pattern of passing in the parent node, because I 
      '      don't know what types of changes you'll make - attributes, multiple 
      '      child nodes, etc.
      ' Override in child class to provide special behavior for column
   End Function
#End Region

#Region "Protected Overridable = Get Info from database"
   Protected Overridable Sub FillMetaDataSet( _
                  ByVal name As String)
      mDataSet = New Data.DataSet
      mDataSet.Tables.Add(GetDatabase(name))
      mDataSet.Tables.Add(GetTables(name))
      mDataSet.Tables.Add(GetColumns(name))
      mDataSet.Tables.Add(GetTableprivileges(name))
      mDataSet.Tables.Add(GetTableConstraints(name))
      mDataSet.Tables.Add(GetColumnprivileges(name))
      mDataSet.Tables.Add(GetColumnConstraints(name))
      mDataSet.Tables.Add(GetCheckConstraints(name))
      mDataSet.Tables.Add(GetKeyColumns(name))
      mDataSet.Tables.Add(GetReferentialConstraints(name))
      mDataSet.Tables.Add(GetUDTs(name))
      mDataSet.Tables.Add(GetUDTConstraints(name))
      mDataSet.Tables.Add(GetStoredProcs(name))
      mDataSet.Tables.Add(GetFunctions(name))
      mDataSet.Tables.Add(GetParameters(name))
      mDataSet.Tables.Add(GetViews(name))
   End Sub

   Protected Overridable Function GetDatabases() As Data.DataTable
      ' TODO: Ask Don whether logging onto Master makes sense here
      Return DataTableFromSQL("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA", _
                     "Master", "Databases")
   End Function

   Protected Overridable Function GetDatabase( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE CATALOG_NAME='" & _
              databaseName & "'", databaseName, "Database")
   End Function

   Protected Overridable Function GetTables( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_CATALOG='" & _
              databaseName & "' AND TABLE_TYPE='BASE TABLE'", databaseName, _
              "Tables")
   End Function

   Protected Overridable Function GetColumns( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG='" & _
              databaseName & "' ORDER BY TABLE_NAME, ORDINAL_POSITION", _
              databaseName, "Columns")
   End Function

   Protected Overridable Function GetTableprivileges( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.TABLE_PRIVILEGES " & _
              "WHERE TABLE_CATALOG='" & databaseName & "'", databaseName, _
              "TablePrivileges")
   End Function

   Protected Overridable Function GetTableConstraints( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS " & _
              "WHERE TABLE_CATALOG='" & databaseName & "'", databaseName, _
              "TableConstraints")
   End Function

   Protected Overridable Function GetColumnprivileges( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.COLUMN_PRIVILEGES " & _
              "WHERE TABLE_CATALOG='" & databaseName & "'", databaseName, _
              "ColumnPrivileges")
   End Function

   Protected Overridable Function GetColumnConstraints( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE " & _
              "WHERE TABLE_CATALOG='" & databaseName & "'", databaseName, _
              "ColumnConstraints")
   End Function

   Protected Overridable Function GetCheckConstraints( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS " & _
              "WHERE CONSTRAINT_CATALOG='" & databaseName & "'", _
              databaseName, "CheckConstraints")
   End Function

   Protected Overridable Function GetKeyColumns( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " & _
              "WHERE TABLE_CATALOG='" & databaseName & "'", _
              databaseName, "KeyColumns")
   End Function

   Protected Overridable Function GetReferentialConstraints( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS " & _
              "WHERE CONSTRAINT_CATALOG='" & databaseName & "'", _
              databaseName, "ReferentialConstraints")
   End Function

   Protected Overridable Function GetViews( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_CATALOG='" & _
              databaseName & "' AND TABLE_TYPE='VIEW'", databaseName, "Views")
   End Function

   Protected Overridable Function GetUDTs( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.DOMAINS " & _
              "WHERE DOMAIN_CATALOG='" & databaseName & "'", _
              databaseName, "UserDefinedTypes")
   End Function

   Protected Overridable Function GetUDTConstraints( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.DOMAIN_CONSTRAINTS " & _
              "WHERE DOMAIN_CATALOG='" & databaseName & "'", _
              databaseName, "UDTConstraints")
   End Function

   Protected Overridable Function GetStoredProcs( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.ROUTINES " & _
              "WHERE ROUTINE_CATALOG='" & databaseName & _
              "' AND ROUTINE_TYPE='PROCEDURE'", databaseName, "SProcs")
   End Function

   Protected Overridable Function GetParameters( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.PARAMETERS " & _
              "WHERE SPECIFIC_CATALOG='" & databaseName & "'", _
              databaseName, "Parameters")
   End Function

   Protected Overridable Function GetFunctions( _
                  ByVal databaseName As String) _
                  As Data.DataTable
      Return DataTableFromSQL( _
              "SELECT * FROM INFORMATION_SCHEMA.ROUTINES " & _
              "WHERE ROUTINE_CATALOG='" & databaseName & _
              "' AND ROUTINE_TYPE='FUNCTION'", databaseName, "Functions")
   End Function

#End Region

#Region "Protected Overridable Class Specific Utility Routines"
   Protected Overridable Sub SetSelectPatterns( _
            ByVal selectPatterns As String, _
            ByVal setSelectPatterns As String)
      If selectPatterns.Trim.Length = 0 Then
         '   mSelectPatterns = New String() {"*Select*"}
         mSelectPatterns = New String() {"\w*Select\w*"}
      Else
         mSelectPatterns = selectPatterns.Split("~"c)
      End If
      If setSelectPatterns.Trim.Length = 0 Then
         '   mSetSelectPatterns = New String() {"*SetSetSelect*"}
         mSetSelectPatterns = New String() {"\w*SetSetSelect\w*"}
      Else
         mSetSelectPatterns = setSelectPatterns.Split("~"c)
      End If
   End Sub

   Protected Overridable Function RetrieveRecordset(ByVal spName As String) As Boolean
      For Each s As String In mSelectPatterns
         If Text.RegularExpressions.Regex.IsMatch(spName, s) Then
            Return True
         End If
      Next
      For Each s As String In mSetSelectPatterns
         If Text.RegularExpressions.Regex.IsMatch(spName, s) Then
            Return True
         End If
      Next
   End Function

   Protected Overridable Function CreateElement( _
                  ByVal ElementName As String) _
                  As Xml.XmlElement
      Return mXMLDoc.CreateElement("dbs", ElementName, _
                     "http://kadgen/DatabaseStructure")
   End Function

   Protected Overridable Function CreateElement( _
                  ByVal ElementName As String, _
                  ByVal name As String) _
                  As Xml.XmlElement
      Dim elem As Xml.XmlElement
      elem = mXMLDoc.CreateElement("dbs", ElementName, _
                     "http://kadgen/DatabaseStructure")
      elem.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                     mXMLDoc, "Name", name))
      Return elem
   End Function


   Protected Overridable Function GenericMatch( _
                  ByVal row As Data.DataRow, _
                  ByVal targetPrefix As String, _
                  ByVal sourcePrefix As String) _
                  As String
      Return targetPrefix & "CATALOG='" & row(sourcePrefix & "CATALOG").ToString _
               & "' AND " & _
               targetPrefix & "SCHEMA='" & row(sourcePrefix & "SCHEMA").ToString _
               & "' AND " & _
               targetPrefix & "NAME='" & row(sourcePrefix & "NAME").ToString & "'"
   End Function


   Protected Overridable Function TableMatch( _
                  ByVal rowTable As Data.DataRow) _
                  As String
      Return GenericMatch(rowTable, "TABLE_", "TABLE_")
   End Function

   Protected Overridable Function ColumnMatch( _
                  ByVal rowTable As Data.DataRow, _
                  ByVal rowColumn As Data.DataRow) _
                  As String
      Return TableMatch(rowTable) & " AND COLUMN_NAME='" & _
                     rowColumn("COLUMN_NAME").ToString & "'"
   End Function

   Protected Overridable Function UDTMatch( _
                  ByVal rowUDT As Data.DataRow) _
                  As String
      Return GenericMatch(rowUDT, "DOMAIN_", "DOMAIN_")
   End Function

   Protected Overridable Function SProcMatch( _
               ByVal rowSProc As Data.DataRow) _
               As String
      Return GenericMatch(rowSProc, "SPROC_", "SPROC_")
   End Function


   Protected Overridable Function ConstraintMatch( _
                  ByVal rowConstraint As Data.DataRow) _
                  As String
      Return ConstraintMatch(rowConstraint, "", "")
   End Function

   Protected Overridable Function ConstraintMatch( _
                  ByVal rowConstraint As Data.DataRow, _
                  ByVal sourcePrefix As String, _
                  ByVal targetPrefix As String) _
                  As String
      Return GenericMatch(rowConstraint, targetPrefix & _
                     "CONSTRAINT_", sourcePrefix & "CONSTRAINT_")
   End Function

   Protected Overridable Function NewElementWithName( _
                  ByVal elementName As String, _
                  ByVal name As String, _
                  ByVal row As Data.DataRow, _
                  ByVal ParamArray prefixes() As String) _
                  As Xml.XmlElement
      Dim dbName As String
      Dim owner As String
      Dim partName As String
      Dim fullName As String
      Dim nodeElement As Xml.XmlElement = Me.CreateElement(elementName, name)
      If mUseVerboseNames And Not row Is Nothing Then
         For Each prefix As String In prefixes
            dbName = row(prefix & "_CATALOG").ToString
            owner = row(prefix & "_SCHEMA").ToString
            partName = row(prefix & "_NAME").ToString
            nodeElement.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, prefix & "Name", partName))
            nodeElement.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, prefix & "Database", dbName))
            nodeElement.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, prefix & "Owner", owner))
            fullName &= "[" & dbName & "].[" & owner & "].[" & partName & "]"
            If fullName.Length > 0 Then
               fullName &= "!"
            End If
         Next
         nodeElement.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                        mXMLDoc, "FullName", fullName))
      End If
      Return nodeElement
   End Function

   Protected Overridable Sub AddKeyColumns( _
                  ByVal xmldoc As Xml.XmlDocument, _
                  ByVal nodeParent As Xml.XmlNode, _
                  ByVal rowKeys() As Data.DataRow, _
                  ByVal fieldName As String)
      Dim nodeKey As Xml.XmlNode
      For Each row As Data.DataRow In rowKeys
         nodeKey = Me.CreateElement(fieldName, _
                     Utility.Tools.FixName(row("COLUMN_NAME").ToString, _
                                 Me.mRemovePrefix))
         nodeKey.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                     xmldoc, "Ordinal", row("ORDINAL_POSITION").ToString))
         nodeParent.AppendChild( _
                        nodeKey)
      Next
   End Sub

   Private Function GetNameSpaceManager( _
                  ByVal xmlDoc As Xml.XmlDocument, _
                  ByVal prefix As String) _
                  As Xml.XmlNamespaceManager
      Dim nsmgr As New Xml.XmlNamespaceManager(xmlDoc.NameTable)
      Dim ns As String
      Select Case prefix
         Case "kg"
            ns = "http://kadgen.com/KADGenDriving.xsd"
         Case "dbs"
            ns = "http://kadgen/DatabaseStructure"
      End Select
      If Not ns Is Nothing Then
         nsmgr.AddNamespace(prefix, ns)
         Return nsmgr
      End If
   End Function

   Protected Overridable Function MakeCaption( _
                     ByVal s As String) _
                     As String
      Return Utility.Tools.SpaceAtCaps(s)
   End Function

   Protected Overridable Function GetDisplayName( _
                     ByVal rows() As Data.DataRow, _
                     ByVal tableName As String) _
                     As String
      Dim firstString As String
      Dim columnName As String
      Dim simpleName As String
      Dim tableTableName As String
      Dim anyName As String
      Dim anyDesc As String
      ' First see if we lack strings
      For Each row As Data.DataRow In rows
         Select Case row("DATA_TYPE")
            Case "char", "varchar", "nchar", "nvarchar"
               columnName = Utility.Tools.FixName(row("COLUMN_NAME").ToString, _
                           Me.mRemovePrefix)
               If firstString Is Nothing Then
                  firstString = columnName
               End If
               If columnName = Utility.Tools.GetSingular(tableName) & "Name" Then
                  tableTableName = columnName
               ElseIf columnName = Utility.Tools.GetPlural(tableName) & "Name" Then
                  tableTableName = columnName
               ElseIf columnName = "Name" Then
                  simpleName = columnName
               ElseIf anyName Is Nothing AndAlso columnName.IndexOf("Name") >= 0 Then
                  anyName = columnName
               ElseIf anyDesc Is Nothing AndAlso columnName.IndexOf("Desc") >= 0 Then
                  anyDesc = columnName
               End If
         End Select
      Next

      If Not tableTableName Is Nothing Then
         Return tableTableName
      ElseIf Not simpleName Is Nothing Then
         Return simpleName
      ElseIf Not anyName Is Nothing Then
         Return anyName
      ElseIf Not anyDesc Is Nothing Then
         Return anyDesc
      ElseIf Not firstString Is Nothing Then
         Return firstString
      Else
         Return Utility.Tools.FixName(rows(0)("COLUMN_NAME").ToString, _
                     Me.mRemovePrefix)
      End If
   End Function

   'Protected Overridable Function GetDisplayName( _
   '                  ByVal row As Data.DataRow) _
   '                  As String
   '   Dim hasStrings As Boolean
   '   Dim firstString As String
   '   Dim tablename As String = row.Table.TableName
   '   ' First see if we lack strings
   '   For Each col As Data.DataColumn In row.Table.Columns
   '      If col.DataType Is GetType(System.String) Then
   '         If firstString Is Nothing Then
   '            firstString = col.ColumnName
   '         End If
   '         hasStrings = True
   '         Exit For
   '      End If
   '   Next

   '   If hasStrings Then
   '      ' See if anything is Table + Name   or just Name
   '      If Not row.Table.Columns(Utility.Tools.GetSingular(tablename) & "Name") Is Nothing Then
   '         Return Utility.Tools.GetSingular(tablename) & "Name"
   '      ElseIf Not row.Table.Columns(Utility.Tools.GetPlural(tablename) & "Name") Is Nothing Then
   '         Return Utility.Tools.GetPlural(tablename) & "Name"
   '      ElseIf Not row.Table.Columns("Name") Is Nothing Then
   '         Return "Name"
   '      Else
   '         ' Then see if anything contains Name
   '         For Each col As Data.DataColumn In row.Table.Columns
   '            If col.ColumnName.IndexOf("Name") > -1 Then
   '               Return col.ColumnName
   '            End If
   '         Next
   '         ' OK, give up and return the first string
   '         Return firstString
   '      End If
   '   Else
   '      ' Bummer, return the first column
   '      Return row.Table.Columns(0).ColumnName
   '   End If
   'End Function
#End Region

#Region "Private Log Methods"
   Private Function LogError(ByVal ex As System.Exception) As ErrorResponse

   End Function

#End Region

End Class
