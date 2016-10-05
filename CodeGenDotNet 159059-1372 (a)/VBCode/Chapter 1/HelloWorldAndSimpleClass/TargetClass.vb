Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class Customers

#Region "Class level declarations"
   Private m_CustomerID As System.String
   Private m_CompanyName As System.String
   Private m_ContactName As System.String
   Private m_ContactTitle As System.String
   Private m_Address As System.String
   Private m_City As System.String
   Private m_Region As System.String
   Private m_PostalCode As System.String
   Private m_Country As System.String
   Private m_Phone As System.String
   Private m_Fax As System.String
#End Region

#Region "Constructors"
#End Region

#Region "Public Methods and Properties"
   Public Property CustomerID() As System.String
      Get
         Return m_CustomerID
      End Get
      Set(ByVal Value As System.String)
         m_CustomerID = Value
      End Set
   End Property

   Public Property CompanyName() As System.String
      Get
         Return m_CompanyName
      End Get
      Set(ByVal Value As System.String)
         m_CompanyName = Value
      End Set
   End Property

   Public Property ContactName() As System.String
      Get
         Return m_ContactName
      End Get
      Set(ByVal Value As System.String)
         m_ContactName = Value
      End Set
   End Property

   Public Property ContactTitle() As System.String
      Get
         Return m_ContactTitle
      End Get
      Set(ByVal Value As System.String)
         m_ContactTitle = Value
      End Set
   End Property

   Public Property Address() As System.String
      Get
         Return m_Address
      End Get
      Set(ByVal Value As System.String)
         m_Address = Value
      End Set
   End Property

   Public Property City() As System.String
      Get
         Return m_City
      End Get
      Set(ByVal Value As System.String)
         m_City = Value
      End Set
   End Property

   Public Property Region() As System.String
      Get
         Return m_Region
      End Get
      Set(ByVal Value As System.String)
         m_Region = Value
      End Set
   End Property

   Public Property PostalCode() As System.String
      Get
         Return m_PostalCode
      End Get
      Set(ByVal Value As System.String)
         m_PostalCode = Value
      End Set
   End Property

   Public Property Country() As System.String
      Get
         Return m_Country
      End Get
      Set(ByVal Value As System.String)
         m_Country = Value
      End Set
   End Property

   Public Property Phone() As System.String
      Get
         Return m_Phone
      End Get
      Set(ByVal Value As System.String)
         m_Phone = Value
      End Set
   End Property

   Public Property Fax() As System.String
      Get
         Return m_Fax
      End Get
      Set(ByVal Value As System.String)
         m_Fax = Value
      End Set
   End Property

#End Region

#Region "Protected and Friend Methods and Properties"
#End Region

#Region "Private Methods and Properties"
#End Region

End Class
