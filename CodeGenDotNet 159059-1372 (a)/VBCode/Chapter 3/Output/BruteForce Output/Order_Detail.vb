' ***^^^***|||***^^^***' ' ' ' %%%###%%%0c5e58eda2c92695b21ef2520410523b%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'Order_Detail.vb
' Last Genned on Date: 1/28/2004 10:46:06 AM
'
'
#End Region

Public Class Order_DetailCollection
    Inherits CollectionBase
    
#Region "Constructors"
    Protected Sub New()
        MyBase.New("Order_DetailCollection")
    End Sub
#End Region
    
#Region "Public and Friend Properties, Methods and Events"
    Public Overloads Sub Fill( _
                        ByVal OrderID As System.Int32, _
                        ByVal UserID As Int32)
        ByVal UserID As Int32)
        Order_DetailDataAccessor.Fill(Me, OrderID, UserID)
    End Sub
#End Region
End Class


Public Class Order_Detail
    Inherits RowBase
    
#Region "Class Level Declarations"
    Protected mCollection As Order_DetailCollection
    Private Shared mNextPrimaryKey As Int32 = -1
    
    Private mOrderID As System.Int32
    Private mProductID As System.Int32
    Private mUnitPrice As System.Decimal
    Private mQuantity As System.Int16
    Private mDiscount As System.Single
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal Order_DetailCollection As Order_DetailCollection)
        MyBase.new()
        mCollection = Order_DetailCollection
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
    Public Function ProductIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ProductID"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Product ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ProductID As System.Int32
        Get
            Return mProductID
        End Get
        Set(ByVal Value As System.Int32)
            mProductID = Value
        End Set
    End Property
    Public Function UnitPriceColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "UnitPrice"
        columnInfo.FieldType = GetType(System.Decimal)
        columnInfo.SQLType = "Money"
        columnInfo.Caption = "Unit Price"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property UnitPrice As System.Decimal
        Get
            Return mUnitPrice
        End Get
        Set(ByVal Value As System.Decimal)
            mUnitPrice = Value
        End Set
    End Property
    Public Function QuantityColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Quantity"
        columnInfo.FieldType = GetType(System.Int16)
        columnInfo.SQLType = "SmallInt"
        columnInfo.Caption = "Quantity"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Quantity As System.Int16
        Get
            Return mQuantity
        End Get
        Set(ByVal Value As System.Int16)
            mQuantity = Value
        End Set
    End Property
    Public Function DiscountColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Discount"
        columnInfo.FieldType = GetType(System.Single)
        columnInfo.SQLType = "Real"
        columnInfo.Caption = "Discount"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Discount As System.Single
        Get
            Return mDiscount
        End Get
        Set(ByVal Value As System.Single)
            mDiscount = Value
        End Set
    End Property
#End Region
    End Class
