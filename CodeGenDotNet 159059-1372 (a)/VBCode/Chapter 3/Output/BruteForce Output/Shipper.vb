' ***^^^***|||***^^^***' ' ' ' %%%###%%%f69eff423792a859de2523cdd4096b8a%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'Shipper.vb
' Last Genned on Date: 1/28/2004 10:46:06 AM
'
'
#End Region

Public Class ShipperCollection
    Inherits CollectionBase
    
#Region "Constructors"
    Protected Sub New()
        MyBase.New("ShipperCollection")
    End Sub
#End Region
    
#Region "Public and Friend Properties, Methods and Events"
    Public Overloads Sub Fill( _
                        ByVal ShipperID As System.Int32, _
                        ByVal UserID As Int32)
        ByVal UserID As Int32)
        ShipperDataAccessor.Fill(Me, ShipperID, UserID)
    End Sub
#End Region
End Class


Public Class Shipper
    Inherits RowBase
    
#Region "Class Level Declarations"
    Protected mCollection As ShipperCollection
    Private Shared mNextPrimaryKey As Int32 = -1
    
    Private mShipperID As System.Int32
    Private mCompanyName As System.String
    Private mPhone As System.String
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal ShipperCollection As ShipperCollection)
        MyBase.new()
        mCollection = ShipperCollection
    End Sub
#End Region
    
#Region "Base Class Implementation"
#End Region
    
#Region "Field access properties"
    Public Function ShipperIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShipperID"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Shipper ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShipperID As System.Int32
        Get
            Return mShipperID
        End Get
        Set(ByVal Value As System.Int32)
            mShipperID = Value
        End Set
    End Property
    Public Function CompanyNameColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "CompanyName"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Company Name"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property CompanyName As System.String
        Get
            Return mCompanyName
        End Get
        Set(ByVal Value As System.String)
            mCompanyName = Value
        End Set
    End Property
    Public Function PhoneColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Phone"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Phone"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Phone As System.String
        Get
            Return mPhone
        End Get
        Set(ByVal Value As System.String)
            mPhone = Value
        End Set
    End Property
#End Region
    End Class
