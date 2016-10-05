' ***^^^***|||***^^^***' ' ' ' %%%###%%%b1644388dc533723e023482a9f86c53f%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'Employee.vb
' Last Genned on Date: 1/28/2004 10:46:05 AM
'
'
#End Region

Public Class EmployeeCollection
    Inherits CollectionBase
    
#Region "Constructors"
    Protected Sub New()
        MyBase.New("EmployeeCollection")
    End Sub
#End Region
    
#Region "Public and Friend Properties, Methods and Events"
    Public Overloads Sub Fill( _
                        ByVal EmployeeID As System.Int32, _
                        ByVal UserID As Int32)
        ByVal UserID As Int32)
        EmployeeDataAccessor.Fill(Me, EmployeeID, UserID)
    End Sub
#End Region
End Class


Public Class Employee
    Inherits RowBase
    
#Region "Class Level Declarations"
    Protected mCollection As EmployeeCollection
    Private Shared mNextPrimaryKey As Int32 = -1
    
    Private mEmployeeID As System.Int32
    Private mLastName As System.String
    Private mFirstName As System.String
    Private mTitle As System.String
    Private mTitleOfCourtesy As System.String
    Private mBirthDate As System.DateTime
    Private mHireDate As System.DateTime
    Private mAddress As System.String
    Private mCity As System.String
    Private mRegion As System.String
    Private mPostalCode As System.String
    Private mCountry As System.String
    Private mHomePhone As System.String
    Private mExtension As System.String
    Private mPhoto As System.Byte()
    Private mNotes As System.String
    Private mReportsTo As System.Int32
    Private mPhotoPath As System.String
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal EmployeeCollection As EmployeeCollection)
        MyBase.new()
        mCollection = EmployeeCollection
    End Sub
#End Region
    
#Region "Base Class Implementation"
#End Region
    
#Region "Field access properties"
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
    Public Function LastNameColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "LastName"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Last Name"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property LastName As System.String
        Get
            Return mLastName
        End Get
        Set(ByVal Value As System.String)
            mLastName = Value
        End Set
    End Property
    Public Function FirstNameColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "FirstName"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "First Name"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property FirstName As System.String
        Get
            Return mFirstName
        End Get
        Set(ByVal Value As System.String)
            mFirstName = Value
        End Set
    End Property
    Public Function TitleColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Title"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Title"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Title As System.String
        Get
            Return mTitle
        End Get
        Set(ByVal Value As System.String)
            mTitle = Value
        End Set
    End Property
    Public Function TitleOfCourtesyColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "TitleOfCourtesy"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Title Of Courtesy"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property TitleOfCourtesy As System.String
        Get
            Return mTitleOfCourtesy
        End Get
        Set(ByVal Value As System.String)
            mTitleOfCourtesy = Value
        End Set
    End Property
    Public Function BirthDateColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "BirthDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Birth Date"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property BirthDate As System.DateTime
        Get
            Return mBirthDate
        End Get
        Set(ByVal Value As System.DateTime)
            mBirthDate = Value
        End Set
    End Property
    Public Function HireDateColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "HireDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Hire Date"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property HireDate As System.DateTime
        Get
            Return mHireDate
        End Get
        Set(ByVal Value As System.DateTime)
            mHireDate = Value
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
    Public Function HomePhoneColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "HomePhone"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Home Phone"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property HomePhone As System.String
        Get
            Return mHomePhone
        End Get
        Set(ByVal Value As System.String)
            mHomePhone = Value
        End Set
    End Property
    Public Function ExtensionColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Extension"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Extension"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Extension As System.String
        Get
            Return mExtension
        End Get
        Set(ByVal Value As System.String)
            mExtension = Value
        End Set
    End Property
    Public Function PhotoColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Photo"
        columnInfo.FieldType = GetType(System.Byte())
        columnInfo.SQLType = "Image"
        columnInfo.Caption = "Photo"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Photo As System.Byte()
        Get
            Return mPhoto
        End Get
        Set(ByVal Value As System.Byte())
            mPhoto = Value
        End Set
    End Property
    Public Function NotesColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Notes"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NText"
        columnInfo.Caption = "Notes"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Notes As System.String
        Get
            Return mNotes
        End Get
        Set(ByVal Value As System.String)
            mNotes = Value
        End Set
    End Property
    Public Function ReportsToColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "ReportsTo"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Reports To"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property ReportsTo As System.Int32
        Get
            Return mReportsTo
        End Get
        Set(ByVal Value As System.Int32)
            mReportsTo = Value
        End Set
    End Property
    Public Function PhotoPathColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "PhotoPath"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Photo Path"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property PhotoPath As System.String
        Get
            Return mPhotoPath
        End Get
        Set(ByVal Value As System.String)
            mPhotoPath = Value
        End Set
    End Property
#End Region
    End Class
