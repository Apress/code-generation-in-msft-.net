' ***^^^***|||***^^^***' ' ' ' %%%###%%%56473c773274122ede6ef7f99e6d7aa3%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' Product.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
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
		      ByVal ProductID As int, _
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
   
			Private mProductIDAs int
			Private mProductNameAs string
			Private mSupplierIDAs int
			Private mCategoryIDAs int
			Private mQuantityPerUnitAs string
			Private mUnitPriceAs System.Decimal
			Private mUnitsInStockAs System.Int16
			Private mUnitsOnOrderAs System.Int16
			Private mReorderLevelAs System.Int16
			Private mDiscontinuedAs bool
#End Region

#Region "Constructors"
   Friend Sub New(ByVal ProductCollection As ProductCollection)
      MyBase.new()
      mCollection = ProductCollection
   End Sub
#End Region

#Region "Base Class Implementation"
   Friend Sub SetNewPrimaryKey()
		ProductID = mNextPrimaryKey
      mNextPrimaryKey -= 1
   End Sub	
#End Region

#Region "Field access properties"
   

   Public Function ProductIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ProductID"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Product ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ProductID As int

      Get
         Return mProductID

      End Get
      Set(ByVal Value As int)
         mProductID = Value
      End Set
   End Property
		

   Public Function ProductNameColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ProductName"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Product Name"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ProductName As string

      Get
         Return mProductName

      End Get
      Set(ByVal Value As string)
         mProductName = Value
      End Set
   End Property
		

   Public Function SupplierIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "SupplierID"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Supplier ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property SupplierID As int

      Get
         Return mSupplierID

      End Get
      Set(ByVal Value As int)
         mSupplierID = Value
      End Set
   End Property
		

   Public Function CategoryIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "CategoryID"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Category ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property CategoryID As int

      Get
         Return mCategoryID

      End Get
      Set(ByVal Value As int)
         mCategoryID = Value
      End Set
   End Property
		

   Public Function QuantityPerUnitColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "QuantityPerUnit"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Quantity Per Unit"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property QuantityPerUnit As string

      Get
         Return mQuantityPerUnit

      End Get
      Set(ByVal Value As string)
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
      columnInfo.FieldType = GetType(bool)
      columnInfo.SQLType = "Bit"
      columnInfo.Caption = "Discontinued"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Discontinued As bool

      Get
         Return mDiscontinued

      End Get
      Set(ByVal Value As bool)
         mDiscontinued = Value
      End Set
   End Property
		
#End Region

End Class	
