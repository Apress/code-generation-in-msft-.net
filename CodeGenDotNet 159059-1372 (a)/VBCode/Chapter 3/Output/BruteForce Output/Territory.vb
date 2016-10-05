' ***^^^***|||***^^^***' ' ' ' %%%###%%%04b5a5e8cd6258ac05a673197a9bfb95%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'Territory.vb
' Last Genned on Date: 1/28/2004 10:46:06 AM
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
                        ByVal TerritoryID As System.String, _
                        ByVal UserID As Int32)
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
    
    Private mTerritoryID As System.String
    Private mTerritoryDescription As System.String
    Private mRegionID As System.Int32
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
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Territory ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property TerritoryID As System.String
        Get
            Return mTerritoryID
        End Get
        Set(ByVal Value As System.String)
            mTerritoryID = Value
        End Set
    End Property
    Public Function TerritoryDescriptionColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "TerritoryDescription"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NChar"
        columnInfo.Caption = "Territory Description"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property TerritoryDescription As System.String
        Get
            Return mTerritoryDescription
        End Get
        Set(ByVal Value As System.String)
            mTerritoryDescription = Value
        End Set
    End Property
    Public Function RegionIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "RegionID"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Region ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property RegionID As System.Int32
        Get
            Return mRegionID
        End Get
        Set(ByVal Value As System.Int32)
            mRegionID = Value
        End Set
    End Property
#End Region
    End Class
