' ***^^^***|||***^^^***' ' ' ' %%%###%%%39b1097e8fd0519b7fb095fa2a2db52f%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' Employee.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
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
		      ByVal EmployeeID As int, _
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
   
			Private mEmployeeIDAs int
			Private mLastNameAs string
			Private mFirstNameAs string
			Private mTitleAs string
			Private mTitleOfCourtesyAs string
			Private mBirthDateAs System.DateTime
			Private mHireDateAs System.DateTime
			Private mAddressAs string
			Private mCityAs string
			Private mRegionAs string
			Private mPostalCodeAs string
			Private mCountryAs string
			Private mHomePhoneAs string
			Private mExtensionAs string
			Private mPhotoAs System.Byte()
			Private mNotesAs string
			Private mReportsToAs int
			Private mPhotoPathAs string
#End Region

#Region "Constructors"
   Friend Sub New(ByVal EmployeeCollection As EmployeeCollection)
      MyBase.new()
      mCollection = EmployeeCollection
   End Sub
#End Region

#Region "Base Class Implementation"
   Friend Sub SetNewPrimaryKey()
		EmployeeID = mNextPrimaryKey
      mNextPrimaryKey -= 1
   End Sub	
#End Region

#Region "Field access properties"
   

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
		

   Public Function LastNameColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "LastName"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Last Name"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property LastName As string

      Get
         Return mLastName

      End Get
      Set(ByVal Value As string)
         mLastName = Value
      End Set
   End Property
		

   Public Function FirstNameColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "FirstName"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "First Name"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property FirstName As string

      Get
         Return mFirstName

      End Get
      Set(ByVal Value As string)
         mFirstName = Value
      End Set
   End Property
		

   Public Function TitleColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Title"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Title"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Title As string

      Get
         Return mTitle

      End Get
      Set(ByVal Value As string)
         mTitle = Value
      End Set
   End Property
		

   Public Function TitleOfCourtesyColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "TitleOfCourtesy"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Title Of Courtesy"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property TitleOfCourtesy As string

      Get
         Return mTitleOfCourtesy

      End Get
      Set(ByVal Value As string)
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
		

   Public Function HomePhoneColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "HomePhone"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Home Phone"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property HomePhone As string

      Get
         Return mHomePhone

      End Get
      Set(ByVal Value As string)
         mHomePhone = Value
      End Set
   End Property
		

   Public Function ExtensionColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Extension"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Extension"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Extension As string

      Get
         Return mExtension

      End Get
      Set(ByVal Value As string)
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
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NText"
      columnInfo.Caption = "Notes"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Notes As string

      Get
         Return mNotes

      End Get
      Set(ByVal Value As string)
         mNotes = Value
      End Set
   End Property
		

   Public Function ReportsToColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ReportsTo"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Reports To"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ReportsTo As int

      Get
         Return mReportsTo

      End Get
      Set(ByVal Value As int)
         mReportsTo = Value
      End Set
   End Property
		

   Public Function PhotoPathColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "PhotoPath"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Photo Path"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property PhotoPath As string

      Get
         Return mPhotoPath

      End Get
      Set(ByVal Value As string)
         mPhotoPath = Value
      End Set
   End Property
		
#End Region

End Class	
