' ***^^^***|||***^^^***' ' ' ' %%%###%%%588dae956cdb6c3f06f9023cf31449ed%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'Order.vb
' Last Genned on Date: 1/28/2004 10:46:06 AM
'
'
#End Region

Public Class OrderCollection
    Inherits CollectionBase
    
#Region "Constructors"
    Protected Sub New()
        MyBase.New("OrderCollection")
    End Sub
#End Region
    
#Region "Public and Friend Properties, Methods and Events"
    Public Overloads Sub Fill( _
                        ByVal OrderID As System.Int32, _
                        ByVal UserID As Int32)
        ByVal UserID As Int32)
        OrderDataAccessor.Fill(Me, OrderID, UserID)
    End Sub
#End Region
End Class


Public Class Order
    Inherits RowBase
    
#Region "Class Level Declarations"
    Protected mCollection As OrderCollection
    Private Shared mNextPrimaryKey As Int32 = -1
    
    Private mOrderID As System.Int32
    Private mCustomerID As System.String
    Private mEmployeeID As System.Int32
    Private mOrderDate As System.DateTime
    Private mRequiredDate As System.DateTime
    Private mShippedDate As System.DateTime
    Private mShipVia As System.Int32
    Private mFreight As System.Decimal
    Private mShipName As System.String
    Private mShipAddress As System.String
    Private mShipCity As System.String
    Private mShipRegion As System.String
    Private mShipPostalCode As System.String
    Private mShipCountry As System.String
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal OrderCollection As OrderCollection)
        MyBase.new()
        mCollection = OrderCollection
    End Sub
#End Region
    
#Region "Base Class Implementation"
#End Region
    
#Region "Field access properties"
    Public Function OrderIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "OrderID"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Order ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property OrderID As System.Int32
        Get
            Return mOrderID
        End Get
        Set(ByVal Value As System.Int32)
            mOrderID = Value
        End Set
    End Property
    Public Function CustomerIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "CustomerID"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NChar"
        columnInfo.Caption = "Customer ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property CustomerID As System.String
        Get
            Return mCustomerID
        End Get
        Set(ByVal Value As System.String)
            mCustomerID = Value
        End Set
    End Property
    Public Function EmployeeIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "EmployeeID"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Employee ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property EmployeeID As System.Int32
        Get
            Return mEmployeeID
        End Get
        Set(ByVal Value As System.Int32)
            mEmployeeID = Value
        End Set
    End Property
    Public Function OrderDateColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "OrderDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Order Date"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property OrderDate As System.DateTime
        Get
            Return mOrderDate
        End Get
        Set(ByVal Value As System.DateTime)
            mOrderDate = Value
        End Set
    End Property
    Public Function RequiredDateColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "RequiredDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Required Date"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property RequiredDate As System.DateTime
        Get
            Return mRequiredDate
        End Get
        Set(ByVal Value As System.DateTime)
            mRequiredDate = Value
        End Set
    End Property
    Public Function ShippedDateColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShippedDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Shipped Date"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShippedDate As System.DateTime
        Get
            Return mShippedDate
        End Get
        Set(ByVal Value As System.DateTime)
            mShippedDate = Value
        End Set
    End Property
    Public Function ShipViaColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShipVia"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Ship Via"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShipVia As System.Int32
        Get
            Return mShipVia
        End Get
        Set(ByVal Value As System.Int32)
            mShipVia = Value
        End Set
    End Property
    Public Function FreightColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Freight"
        columnInfo.FieldType = GetType(System.Decimal)
        columnInfo.SQLType = "Money"
        columnInfo.Caption = "Freight"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Freight As System.Decimal
        Get
            Return mFreight
        End Get
        Set(ByVal Value As System.Decimal)
            mFreight = Value
        End Set
    End Property
    Public Function ShipNameColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShipName"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Name"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShipName As System.String
        Get
            Return mShipName
        End Get
        Set(ByVal Value As System.String)
            mShipName = Value
        End Set
    End Property
    Public Function ShipAddressColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShipAddress"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Address"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShipAddress As System.String
        Get
            Return mShipAddress
        End Get
        Set(ByVal Value As System.String)
            mShipAddress = Value
        End Set
    End Property
    Public Function ShipCityColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShipCity"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship City"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShipCity As System.String
        Get
            Return mShipCity
        End Get
        Set(ByVal Value As System.String)
            mShipCity = Value
        End Set
    End Property
    Public Function ShipRegionColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShipRegion"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Region"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShipRegion As System.String
        Get
            Return mShipRegion
        End Get
        Set(ByVal Value As System.String)
            mShipRegion = Value
        End Set
    End Property
    Public Function ShipPostalCodeColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShipPostalCode"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Postal Code"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShipPostalCode As System.String
        Get
            Return mShipPostalCode
        End Get
        Set(ByVal Value As System.String)
            mShipPostalCode = Value
        End Set
    End Property
    Public Function ShipCountryColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ShipCountry"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Country"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ShipCountry As System.String
        Get
            Return mShipCountry
        End Get
        Set(ByVal Value As System.String)
            mShipCountry = Value
        End Set
    End Property
#End Region
    End Class
