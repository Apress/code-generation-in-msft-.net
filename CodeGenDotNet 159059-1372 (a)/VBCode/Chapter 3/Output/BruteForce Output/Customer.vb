' ***^^^***|||***^^^***' ' ' ' %%%###%%%4a586f316d04a371cfeff2083059d3f9%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'Customer.vb
' Last Genned on Date: 1/28/2004 10:46:05 AM
'
'
#End Region

Public Class CustomerCollection
    Inherits CollectionBase
    
#Region "Constructors"
    Protected Sub New()
        MyBase.New("CustomerCollection")
    End Sub
#End Region
    
#Region "Public and Friend Properties, Methods and Events"
    Public Overloads Sub Fill( _
                        ByVal CustomerID As System.String, _
                        ByVal UserID As Int32)
        ByVal UserID As Int32)
        CustomerDataAccessor.Fill(Me, CustomerID, UserID)
    End Sub
#End Region
End Class


Public Class Customer
    Inherits RowBase
    
#Region "Class Level Declarations"
    Protected mCollection As CustomerCollection
    Private Shared mNextPrimaryKey As Int32 = -1
    
    Private mCustomerID As System.String
    Private mCompanyName As System.String
    Private mContactName As System.String
    Private mContactTitle As System.String
    Private mAddress As System.String
    Private mCity As System.String
    Private mRegion As System.String
    Private mPostalCode As System.String
    Private mCountry As System.String
    Private mPhone As System.String
    Private mFax As System.String
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal CustomerCollection As CustomerCollection)
        MyBase.new()
        mCollection = CustomerCollection
    End Sub
#End Region
    
#Region "Base Class Implementation"
#End Region
    
#Region "Field access properties"
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
    Public Function ContactNameColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ContactName"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Contact Name"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ContactName As System.String
        Get
            Return mContactName
        End Get
        Set(ByVal Value As System.String)
            mContactName = Value
        End Set
    End Property
    Public Function ContactTitleColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ContactTitle"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Contact Title"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ContactTitle As System.String
        Get
            Return mContactTitle
        End Get
        Set(ByVal Value As System.String)
            mContactTitle = Value
        End Set
    End Property
    Public Function AddressColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Address"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Address"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Address As System.String
        Get
            Return mAddress
        End Get
        Set(ByVal Value As System.String)
            mAddress = Value
        End Set
    End Property
    Public Function CityColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "City"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "City"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property City As System.String
        Get
            Return mCity
        End Get
        Set(ByVal Value As System.String)
            mCity = Value
        End Set
    End Property
    Public Function RegionColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Region"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Region"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Region As System.String
        Get
            Return mRegion
        End Get
        Set(ByVal Value As System.String)
            mRegion = Value
        End Set
    End Property
    Public Function PostalCodeColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "PostalCode"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Postal Code"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property PostalCode As System.String
        Get
            Return mPostalCode
        End Get
        Set(ByVal Value As System.String)
            mPostalCode = Value
        End Set
    End Property
    Public Function CountryColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Country"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Country"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Country As System.String
        Get
            Return mCountry
        End Get
        Set(ByVal Value As System.String)
            mCountry = Value
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
    Public Function FaxColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Fax"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Fax"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Fax As System.String
        Get
            Return mFax
        End Get
        Set(ByVal Value As System.String)
            mFax = Value
        End Set
    End Property
#End Region
    End Class
