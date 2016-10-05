' ***^^^***|||***^^^***' ' ' ' %%%###%%%716ec884423c6c1bd16056a5af9d922b%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' Order.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
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
		      ByVal OrderID As int, _
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
   
			Private mOrderIDAs int
			Private mCustomerIDAs string
			Private mEmployeeIDAs int
			Private mOrderDateAs System.DateTime
			Private mRequiredDateAs System.DateTime
			Private mShippedDateAs System.DateTime
			Private mShipViaAs int
			Private mFreightAs System.Decimal
			Private mShipNameAs string
			Private mShipAddressAs string
			Private mShipCityAs string
			Private mShipRegionAs string
			Private mShipPostalCodeAs string
			Private mShipCountryAs string
#End Region

#Region "Constructors"
   Friend Sub New(ByVal OrderCollection As OrderCollection)
      MyBase.new()
      mCollection = OrderCollection
   End Sub
#End Region

#Region "Base Class Implementation"
   Friend Sub SetNewPrimaryKey()
		OrderID = mNextPrimaryKey
      mNextPrimaryKey -= 1
   End Sub	
#End Region

#Region "Field access properties"
   

   Public Function OrderIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "OrderID"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Order ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property OrderID As int

      Get
         Return mOrderID

      End Get
      Set(ByVal Value As int)
         mOrderID = Value
      End Set
   End Property
		

   Public Function CustomerIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "CustomerID"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NChar"
      columnInfo.Caption = "Customer ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property CustomerID As string

      Get
         Return mCustomerID

      End Get
      Set(ByVal Value As string)
         mCustomerID = Value
      End Set
   End Property
		

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
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Ship Via"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ShipVia As int

      Get
         Return mShipVia

      End Get
      Set(ByVal Value As int)
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
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Ship Name"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ShipName As string

      Get
         Return mShipName

      End Get
      Set(ByVal Value As string)
         mShipName = Value
      End Set
   End Property
		

   Public Function ShipAddressColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ShipAddress"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Ship Address"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ShipAddress As string

      Get
         Return mShipAddress

      End Get
      Set(ByVal Value As string)
         mShipAddress = Value
      End Set
   End Property
		

   Public Function ShipCityColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ShipCity"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Ship City"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ShipCity As string

      Get
         Return mShipCity

      End Get
      Set(ByVal Value As string)
         mShipCity = Value
      End Set
   End Property
		

   Public Function ShipRegionColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ShipRegion"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Ship Region"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ShipRegion As string

      Get
         Return mShipRegion

      End Get
      Set(ByVal Value As string)
         mShipRegion = Value
      End Set
   End Property
		

   Public Function ShipPostalCodeColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ShipPostalCode"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Ship Postal Code"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ShipPostalCode As string

      Get
         Return mShipPostalCode

      End Get
      Set(ByVal Value As string)
         mShipPostalCode = Value
      End Set
   End Property
		

   Public Function ShipCountryColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ShipCountry"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Ship Country"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ShipCountry As string

      Get
         Return mShipCountry

      End Get
      Set(ByVal Value As string)
         mShipCountry = Value
      End Set
   End Property
		
#End Region

End Class	
