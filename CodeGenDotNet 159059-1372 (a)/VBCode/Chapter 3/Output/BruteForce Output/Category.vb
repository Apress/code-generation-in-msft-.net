' ***^^^***|||***^^^***' ' ' ' %%%###%%%fdf0ce06d0e1a07e9d98288fef96f519%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'Category.vb
' Last Genned on Date: 1/28/2004 10:46:05 AM
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
                        ByVal CategoryID As System.Int32, _
                        ByVal UserID As Int32)
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
    
    Private mCategoryID As System.Int32
    Private mCategoryName As System.String
    Private mDescription As System.String
    Private mPicture As System.Byte()
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal CategoryCollection As CategoryCollection)
        MyBase.new()
        mCollection = CategoryCollection
    End Sub
#End Region
    
#Region "Base Class Implementation"
#End Region
    
#Region "Field access properties"
    Public Function CategoryIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "CategoryID"
        columnInfo.FieldType = GetType(System.Int32)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Category ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property CategoryID As System.Int32
        Get
            Return mCategoryID
        End Get
        Set(ByVal Value As System.Int32)
            mCategoryID = Value
        End Set
    End Property
    Public Function CategoryNameColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "CategoryName"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Category Name"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property CategoryName As System.String
        Get
            Return mCategoryName
        End Get
        Set(ByVal Value As System.String)
            mCategoryName = Value
        End Set
    End Property
    Public Function DescriptionColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "Description"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NText"
        columnInfo.Caption = "Description"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property Description As System.String
        Get
            Return mDescription
        End Get
        Set(ByVal Value As System.String)
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
