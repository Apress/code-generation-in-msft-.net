' ***^^^***|||***^^^***' ' ' ' %%%###%%%3fc3a1840b4ced40f92a738ea85b5015%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' EmployeeTerritory.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
'
'
#End Region

Public Class EmployeeTerritoryCollection
	Inherits CollectionBase
	
#Region "Constructors"
	Protected Sub New()
      MyBase.New("EmployeeTerritoryCollection")
   End Sub
#End Region

#Region "Public and Friend Properties, Methods and Events"
   Public Overloads Sub Fill( _
		      ByVal EmployeeID As int, _
            ByVal UserID As Int32)
		EmployeeTerritoryDataAccessor.Fill(Me, EmployeeID, UserID)
   End Sub
#End Region

End Class

Public Class EmployeeTerritory
   Inherits RowBase
	
#Region "Class Level Declarations"
   Protected mCollection As EmployeeTerritoryCollection
   Private Shared mNextPrimaryKey As Int32 = -1
   
			Private mEmployeeIDAs int
			Private mTerritoryIDAs string
#End Region

#Region "Constructors"
   Friend Sub New(ByVal EmployeeTerritoryCollection As EmployeeTerritoryCollection)
      MyBase.new()
      mCollection = EmployeeTerritoryCollection
   End Sub
#End Region

#Region "Base Class Implementation"
#End Region

#Region "Field access properties"
   

   Public Function EmployeeIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "EmployeeID"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Employee ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property EmployeeID As int

      Get
         Return mEmployeeID

      End Get
      Set(ByVal Value As int)
         mEmployeeID = Value
      End Set
   End Property
		

   Public Function TerritoryIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "TerritoryID"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Territory ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property TerritoryID As string

      Get
         Return mTerritoryID

      End Get
      Set(ByVal Value As string)
         mTerritoryID = Value
      End Set
   End Property
		
#End Region

End Class	
