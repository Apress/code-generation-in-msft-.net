
Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class Orders
   
#Region "Class level declarations"
   Private m_OrderID As Int32
   Private m_CustomerID As String
   Private m_EmployeeID As Int32
   Private m_OrderDate As DateTime
   Private m_RequiredDate As DateTime
   Private m_ShippedDate As DateTime
   Private m_ShipVia As Int32
   Private m_Freight As Decimal
   Private m_ShipName As String
   Private m_ShipAddress As String
   Private m_ShipCity As String
   Private m_ShipRegion As String
   Private m_ShipPostalCode As String
   Private m_ShipCountry As String
#End Region

#Region "Public Methods and Properties"

   Public Property OrderID() As Int32
      Get
         Return m_OrderID
      End Get
      Set(ByVal Value As Int32)
         m_OrderID = Value
      End Set
   End Property

   Public Property CustomerID() As String
      Get
         Return m_CustomerID
      End Get
      Set(ByVal Value As String)
         m_CustomerID = Value
      End Set
   End Property

   Public Property EmployeeID() As Int32
      Get
         Return m_EmployeeID
      End Get
      Set(ByVal Value As Int32)
         m_EmployeeID = Value
      End Set
   End Property

   Public Property OrderDate() As DateTime
      Get
         Return m_OrderDate
      End Get
      Set(ByVal Value As DateTime)
         m_OrderDate = Value
      End Set
   End Property

   Public Property RequiredDate() As DateTime
      Get
         Return m_RequiredDate
      End Get
      Set(ByVal Value As DateTime)
         m_RequiredDate = Value
      End Set
   End Property

   Public Property ShippedDate() As DateTime
      Get
         Return m_ShippedDate
      End Get
      Set(ByVal Value As DateTime)
         m_ShippedDate = Value
      End Set
   End Property

   Public Property ShipVia() As Int32
      Get
         Return m_ShipVia
      End Get
      Set(ByVal Value As Int32)
         m_ShipVia = Value
      End Set
   End Property

   Public Property Freight() As Decimal
      Get
         Return m_Freight
      End Get
      Set(ByVal Value As Decimal)
         m_Freight = Value
      End Set
   End Property

   Public Property ShipName() As String
      Get
         Return m_ShipName
      End Get
      Set(ByVal Value As String)
         m_ShipName = Value
      End Set
   End Property

   Public Property ShipAddress() As String
      Get
         Return m_ShipAddress
      End Get
      Set(ByVal Value As String)
         m_ShipAddress = Value
      End Set
   End Property

   Public Property ShipCity() As String
      Get
         Return m_ShipCity
      End Get
      Set(ByVal Value As String)
         m_ShipCity = Value
      End Set
   End Property

   Public Property ShipRegion() As String
      Get
         Return m_ShipRegion
      End Get
      Set(ByVal Value As String)
         m_ShipRegion = Value
      End Set
   End Property

   Public Property ShipPostalCode() As String
      Get
         Return m_ShipPostalCode
      End Get
      Set(ByVal Value As String)
         m_ShipPostalCode = Value
      End Set
   End Property

   Public Property ShipCountry() As String
      Get
         Return m_ShipCountry
      End Get
      Set(ByVal Value As String)
         m_ShipCountry = Value
      End Set
   End Property


#End Region

End Class
