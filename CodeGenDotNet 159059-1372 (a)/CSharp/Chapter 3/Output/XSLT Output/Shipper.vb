' ***^^^***|||***^^^***' ' ' ' %%%###%%%dc32b55ab844725dad2db18a3afb8eb1%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' Shipper.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
'
'
#End Region

Public Class ShipperCollection
	Inherits CollectionBase
	
#Region "Constructors"
	Protected Sub New()
      MyBase.New("ShipperCollection")
   End Sub
#End Region

#Region "Public and Friend Properties, Methods and Events"
   Public Overloads Sub Fill( _
		      ByVal ShipperID As int, _
            ByVal UserID As Int32)
		ShipperDataAccessor.Fill(Me, ShipperID, UserID)
   End Sub
#End Region

End Class

Public Class Shipper
   Inherits RowBase
	
#Region "Class Level Declarations"
   Protected mCollection As ShipperCollection
   Private Shared mNextPrimaryKey As Int32 = -1
   
			Private mShipperIDAs int
			Private mCompanyNameAs string
			Private mPhoneAs string
#End Region

#Region "Constructors"
   Friend Sub New(ByVal ShipperCollection As ShipperCollection)
      MyBase.new()
      mCollection = ShipperCollection
   End Sub
#End Region

#Region "Base Class Implementation"
   Friend Sub SetNewPrimaryKey()
		ShipperID = mNextPrimaryKey
      mNextPrimaryKey -= 1
   End Sub	
#End Region

#Region "Field access properties"
   

   Public Function ShipperIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "ShipperID"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Shipper ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property ShipperID As int

      Get
         Return mShipperID

      End Get
      Set(ByVal Value As int)
         mShipperID = Value
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
		
#End Region

End Class	
