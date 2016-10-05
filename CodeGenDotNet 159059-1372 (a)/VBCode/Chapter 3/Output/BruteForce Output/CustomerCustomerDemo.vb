' ***^^^***|||***^^^***' ' ' ' %%%###%%%5f1767184410026200b5d04b98cfc07d%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'CustomerCustomerDemo.vb
' Last Genned on Date: 1/28/2004 10:46:05 AM
'
'
#End Region

Public Class CustomerCustomerDemoCollection
    Inherits CollectionBase
    
#Region "Constructors"
    Protected Sub New()
        MyBase.New("CustomerCustomerDemoCollection")
    End Sub
#End Region
    
#Region "Public and Friend Properties, Methods and Events"
    Public Overloads Sub Fill( _
                        ByVal CustomerID As System.String, _
                        ByVal UserID As Int32)
        ByVal UserID As Int32)
        CustomerCustomerDemoDataAccessor.Fill(Me, CustomerID, UserID)
    End Sub
#End Region
End Class


Public Class CustomerCustomerDemo
    Inherits RowBase
    
#Region "Class Level Declarations"
    Protected mCollection As CustomerCustomerDemoCollection
    Private Shared mNextPrimaryKey As Int32 = -1
    
    Private mCustomerID As System.String
    Private mCustomerTypeID As System.String
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal CustomerCustomerDemoCollection As CustomerCustomerDemoCollection)
        MyBase.new()
        mCollection = CustomerCustomerDemoCollection
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
    Public Function CustomerTypeIDColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "CustomerTypeID"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NChar"
        columnInfo.Caption = "Customer Type ID"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property CustomerTypeID As System.String
        Get
            Return mCustomerTypeID
        End Get
        Set(ByVal Value As System.String)
            mCustomerTypeID = Value
        End Set
    End Property
#End Region
    End Class
