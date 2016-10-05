' ***^^^***|||***^^^***' ' ' ' %%%###%%%173cf188d5ca0cbb49f19e5dcc0a4879%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' Territory.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
'
'
#End Region

Public Class TerritoryCollection
	Inherits CollectionBase
	
#Region "Constructors"
	Protected Sub New()
      MyBase.New("TerritoryCollection")
   End Sub
#End Region

#Region "Public and Friend Properties, Methods and Events"
   Public Overloads Sub Fill( _
		      ByVal TerritoryID As string, _
            ByVal UserID As Int32)
		TerritoryDataAccessor.Fill(Me, TerritoryID, UserID)
   End Sub
#End Region

End Class

Public Class Territory
   Inherits RowBase
	
#Region "Class Level Declarations"
   Protected mCollection As TerritoryCollection
   Private Shared mNextPrimaryKey As Int32 = -1
   
			Private mTerritoryIDAs string
			Private mTerritoryDescriptionAs string
			Private mRegionIDAs int
#End Region

#Region "Constructors"
   Friend Sub New(ByVal TerritoryCollection As TerritoryCollection)
      MyBase.new()
      mCollection = TerritoryCollection
   End Sub
#End Region

#Region "Base Class Implementation"
#End Region

#Region "Field access properties"
   

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
		

   Public Function TerritoryDescriptionColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "TerritoryDescription"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NChar"
      columnInfo.Caption = "Territory Description"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property TerritoryDescription As string

      Get
         Return mTerritoryDescription

      End Get
      Set(ByVal Value As string)
         mTerritoryDescription = Value
      End Set
   End Property
		

   Public Function RegionIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "RegionID"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Region ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property RegionID As int

      Get
         Return mRegionID

      End Get
      Set(ByVal Value As int)
         mRegionID = Value
      End Set
   End Property
		
#End Region

End Class	
