' ***^^^***|||***^^^***' ' ' ' %%%###%%%65a216b2907a88ac0f07789a69b24601%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'Product.vb
' Last Genned on Date: 1/28/2004 10:46:06 AM
'
'
#End Region

Public Class ProductCollection
    Inherits CollectionBase
    
#Region "Constructors"
    Protected Sub New()
        MyBase.New("ProductCollection")
    End Sub
#End Region
    
#Region "Public and Friend Properties, Methods and Events"
    Public Overloads Sub Fill( _
                        ByVal ProductID As System.Int32, _
                        ByVal UserID As Int32)
        ByVal UserID As Int32)
        ProductDataAccessor.Fill(Me, ProductID, UserID)
    End Sub
#End Region
End Class


Public Class Product
    Inherits RowBase
    
#Region "Class Level Declarations"
    Protected mCollection As ProductCollection
    Private Shared mNextPrimaryKey As Int32 = -1
    
    Private mProductID As System.Int32
    Private mProductName As System.String
    Private mSupplierID As System.Int32
    Private mCategoryID As System.Int32
    Private mQuantityPerUnit As System.String
    Private mUnitPrice As System.Decimal
    Private mUnitsInStock As System.Int16
    Private mUnitsOnOrder As System.Int16
    Private mReorderLevel As System.Int16
    Private mDiscontinued As System.Boolean
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal ProductCollection As ProductCollection)
        MyBase.new()
        mCollection = ProductCollection
    End Sub
#End Region
    
#Region "Base Class Implementation"
#End Region
    
#Region "Field access properties"
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
    Public Function ProductNameColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ProductName"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Product Name"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ProductName As System.String
        Get
            Return mProductName
        End Get
        Set(ByVal Value As System.String)
            mProductName = Value
        End Set
    End Property
    Public Function SupplierIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "SupplierID"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Supplier ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property SupplierID As System.Int32
        Get
            Return mSupplierID
        End Get
        Set(ByVal Value As System.Int32)
            mSupplierID = Value
        End Set
    End Property
    Public Function CategoryIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "CategoryID"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Category ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property CategoryID As System.Int32
        Get
            Return mCategoryID
        End Get
        Set(ByVal Value As System.Int32)
            mCategoryID = Value
        End Set
    End Property
    Public Function QuantityPerUnitColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "QuantityPerUnit"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Quantity Per Unit"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property QuantityPerUnit As System.String
        Get
            Return mQuantityPerUnit
        End Get
        Set(ByVal Value As System.String)
            mQuantityPerUnit = Value
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
    Public Function UnitsInStockColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "UnitsInStock"
        columnInfo.FieldType = GetType(System.Int16)
        columnInfo.SQLType = "SmallInt"
        columnInfo.Caption = "Units In Stock"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property UnitsInStock As System.Int16
        Get
            Return mUnitsInStock
        End Get
        Set(ByVal Value As System.Int16)
            mUnitsInStock = Value
        End Set
    End Property
    Public Function UnitsOnOrderColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "UnitsOnOrder"
        columnInfo.FieldType = GetType(System.Int16)
        columnInfo.SQLType = "SmallInt"
        columnInfo.Caption = "Units On Order"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property UnitsOnOrder As System.Int16
        Get
            Return mUnitsOnOrder
        End Get
        Set(ByVal Value As System.Int16)
            mUnitsOnOrder = Value
        End Set
    End Property
    Public Function ReorderLevelColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ReorderLevel"
        columnInfo.FieldType = GetType(System.Int16)
        columnInfo.SQLType = "SmallInt"
        columnInfo.Caption = "Reorder Level"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ReorderLevel As System.Int16
        Get
            Return mReorderLevel
        End Get
        Set(ByVal Value As System.Int16)
            mReorderLevel = Value
        End Set
    End Property
    Public Function DiscontinuedColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Discontinued"
        columnInfo.FieldType = GetType(System.Boolean)
        columnInfo.SQLType = "Bit"
        columnInfo.Caption = "Discontinued"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Discontinued As System.Boolean
        Get
            Return mDiscontinued
        End Get
        Set(ByVal Value As System.Boolean)
            mDiscontinued = Value
        End Set
    End Property
#End Region
    End Class
