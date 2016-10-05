
Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class Customers
   
#Region "Class level declarations"
   Private m_CustomerID As String
   Private m_CompanyName As String
   Private m_ContactName As String
   Private m_ContactTitle As String
   Private m_Address As String
   Private m_City As String
   Private m_Region As String
   Private m_PostalCode As String
   Private m_Country As String
   Private m_Phone As String
   Private m_Fax As String
#End Region

#Region "Public Methods and Properties"

   Public Property CustomerID() As String
      Get
         Return m_CustomerID
      End Get
      Set(ByVal Value As String)
         m_CustomerID = Value
      End Set
   End Property

   Public Property CompanyName() As String
      Get
         Return m_CompanyName
      End Get
      Set(ByVal Value As String)
         m_CompanyName = Value
      End Set
   End Property

   Public Property ContactName() As String
      Get
         Return m_ContactName
      End Get
      Set(ByVal Value As String)
         m_ContactName = Value
      End Set
   End Property

   Public Property ContactTitle() As String
      Get
         Return m_ContactTitle
      End Get
      Set(ByVal Value As String)
         m_ContactTitle = Value
      End Set
   End Property

   Public Property Address() As String
      Get
         Return m_Address
      End Get
      Set(ByVal Value As String)
         m_Address = Value
      End Set
   End Property

   Public Property City() As String
      Get
         Return m_City
      End Get
      Set(ByVal Value As String)
         m_City = Value
      End Set
   End Property

   Public Property Region() As String
      Get
         Return m_Region
      End Get
      Set(ByVal Value As String)
         m_Region = Value
      End Set
   End Property

   Public Property PostalCode() As String
      Get
         Return m_PostalCode
      End Get
      Set(ByVal Value As String)
         m_PostalCode = Value
      End Set
   End Property

   Public Property Country() As String
      Get
         Return m_Country
      End Get
      Set(ByVal Value As String)
         m_Country = Value
      End Set
   End Property

   Public Property Phone() As String
      Get
         Return m_Phone
      End Get
      Set(ByVal Value As String)
         m_Phone = Value
      End Set
   End Property

   Public Property Fax() As String
      Get
         Return m_Fax
      End Get
      Set(ByVal Value As String)
         m_Fax = Value
      End Set
   End Property


#End Region

End Class
