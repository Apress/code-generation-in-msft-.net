' ***^^^***|||***^^^***' ' ' ' %%%###%%%74de7fe745d822db9f167e9ba2193031%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' Category.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
'
'
#End Region

Public Class CategoryCollection
	Inherits CollectionBase
	
#Region "Constructors"
	Protected Sub New()
      MyBase.New("CategoryCollection")
   End Sub
#End Region

#Region "Public and Friend Properties, Methods and Events"
   Public Overloads Sub Fill( _
		      ByVal CategoryID As int, _
            ByVal UserID As Int32)
		CategoryDataAccessor.Fill(Me, CategoryID, UserID)
   End Sub
#End Region

End Class

Public Class Category
   Inherits RowBase
	
#Region "Class Level Declarations"
   Protected mCollection As CategoryCollection
   Private Shared mNextPrimaryKey As Int32 = -1
   
			Private mCategoryIDAs int
			Private mCategoryNameAs string
			Private mDescriptionAs string
			Private mPictureAs System.Byte()
#End Region

#Region "Constructors"
   Friend Sub New(ByVal CategoryCollection As CategoryCollection)
      MyBase.new()
      mCollection = CategoryCollection
   End Sub
#End Region

#Region "Base Class Implementation"
   Friend Sub SetNewPrimaryKey()
		CategoryID = mNextPrimaryKey
      mNextPrimaryKey -= 1
   End Sub	
#End Region

#Region "Field access properties"
   

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
		

   Public Function CategoryNameColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "CategoryName"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NVarChar"
      columnInfo.Caption = "Category Name"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property CategoryName As string

      Get
         Return mCategoryName

      End Get
      Set(ByVal Value As string)
         mCategoryName = Value
      End Set
   End Property
		

   Public Function DescriptionColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Description"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NText"
      columnInfo.Caption = "Description"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Description As string

      Get
         Return mDescription

      End Get
      Set(ByVal Value As string)
         mDescription = Value
      End Set
   End Property
		

   Public Function PictureColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "Picture"
      columnInfo.FieldType = GetType(System.Byte())
      columnInfo.SQLType = "Image"
      columnInfo.Caption = "Picture"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property Picture As System.Byte()

      Get
         Return mPicture

      End Get
      Set(ByVal Value As System.Byte())
         mPicture = Value
      End Set
   End Property
		
#End Region

End Class	
