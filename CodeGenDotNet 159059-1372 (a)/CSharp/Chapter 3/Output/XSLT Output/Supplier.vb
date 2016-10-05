' ***^^^***|||***^^^***' ' ' ' %%%###%%%ac981f1c31f75d61290477c38971c374%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' Supplier.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
'
'
#End Region

Public Class SupplierCollection
	Inherits CollectionBase
	
#Region "Constructors"
	Protected Sub New()
      MyBase.New("SupplierCollection")
   End Sub
#End Region

#Region "Public and Friend Properties, Methods and Events"
   Public Overloads Sub Fill( _
		      ByVal SupplierID As int, _
            ByVal UserID As Int32)
		SupplierDataAccessor.Fill(Me, SupplierID, UserID)
   End Sub
#End Region

End Class

Public Class Supplier
   Inherits RowBase
	
#Region "Class Level Declarations"
   Protected mCollection As SupplierCollection
   Private Shared mNextPrimaryKey As Int32 = -1
   
			Private mSupplierIDAs int
			Private mCompanyNameAs string
			Private mContactNameAs string
			Private mContactTitleAs string
			Private mAddressAs string
			Private mCityAs string
			Private mRegionAs string
			Private mPostalCodeAs string
			Private mCountryAs string
			Private mPhoneAs string
			Private mFaxAs string
			Private mHomePageAs string
#End Region

#Region "Constructors"
   Friend Sub New(ByVal SupplierCollection As SupplierCollection)
      MyBase.new()
      mCollection = SupplierCollection
   End Sub
#End Region

#Region "Base Class Implementation"
   Friend Sub SetNewPrimaryKey()
		SupplierID = mNextPrimaryKey
      mNextPrimaryKey -= 1
   End Sub	
#End Region

#Region "Field access properties"
   

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
		

   Public Function CompanyNameColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "CompanyName"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Company Name"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property CompanyName As string

      Get
         Return mCompanyName

      End Get
      Set(ByVal Value As string)
         mCompanyName = Value
      End Set
   End Property
		

   Public Function ContactNameColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ContactName"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Contact Name"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ContactName As string

      Get
         Return mContactName

      End Get
      Set(ByVal Value As string)
         mContactName = Value
      End Set
   End Property
		

   Public Function ContactTitleColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ContactTitle"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Contact Title"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ContactTitle As string

      Get
         Return mContactTitle

      End Get
      Set(ByVal Value As string)
         mContactTitle = Value
      End Set
   End Property
		

   Public Function AddressColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Address"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Address"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Address As string

      Get
         Return mAddress

      End Get
      Set(ByVal Value As string)
         mAddress = Value
      End Set
   End Property
		

   Public Function CityColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "City"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "City"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property City As string

      Get
         Return mCity

      End Get
      Set(ByVal Value As string)
         mCity = Value
      End Set
   End Property
		

   Public Function RegionColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Region"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Region"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Region As string

      Get
         Return mRegion

      End Get
      Set(ByVal Value As string)
         mRegion = Value
      End Set
   End Property
		

   Public Function PostalCodeColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "PostalCode"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Postal Code"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property PostalCode As string

      Get
         Return mPostalCode

      End Get
      Set(ByVal Value As string)
         mPostalCode = Value
      End Set
   End Property
		

   Public Function CountryColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Country"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Country"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Country As string

      Get
         Return mCountry

      End Get
      Set(ByVal Value As string)
         mCountry = Value
      End Set
   End Property
		

   Public Function PhoneColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Phone"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Phone"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Phone As string

      Get
         Return mPhone

      End Get
      Set(ByVal Value As string)
         mPhone = Value
      End Set
   End Property
		

   Public Function FaxColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Fax"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Fax"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Fax As string

      Get
         Return mFax

      End Get
      Set(ByVal Value As string)
         mFax = Value
      End Set
   End Property
		

   Public Function HomePageColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "HomePage"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NText"
      columnInfo.Caption = "Home Page"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property HomePage As string

      Get
         Return mHomePage

      End Get
      Set(ByVal Value As string)
         mHomePage = Value
      End Set
   End Property
		
#End Region

End Class	
