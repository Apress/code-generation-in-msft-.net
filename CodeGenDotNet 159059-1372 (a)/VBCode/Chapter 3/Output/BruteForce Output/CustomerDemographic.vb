' ***^^^***|||***^^^***' ' ' ' %%%###%%%081cee31f344557669253ebfd2286c4e%%%###%%%' ***^^^***|||***^^^***Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System

#Region "Description"
'
'CustomerDemographic.vb
' Last Genned on Date: 1/28/2004 10:46:05 AM
'
'
#End Region

Public Class CustomerDemographicCollection
    Inherits CollectionBase
    
#Region "Constructors"
    Protected Sub New()
        MyBase.New("CustomerDemographicCollection")
    End Sub
#End Region
    
#Region "Public and Friend Properties, Methods and Events"
    Public Overloads Sub Fill( _
                        ByVal CustomerTypeID As System.String, _
                        ByVal UserID As Int32)
        ByVal UserID As Int32)
        CustomerDemographicDataAccessor.Fill(Me, CustomerTypeID, UserID)
    End Sub
#End Region
End Class


Public Class CustomerDemographic
    Inherits RowBase
    
#Region "Class Level Declarations"
    Protected mCollection As CustomerDemographicCollection
    Private Shared mNextPrimaryKey As Int32 = -1
    
    Private mCustomerTypeID As System.String
    Private mCustomerDesc As System.String
#End Region
    
#Region "Constructors"
    Friend Sub New(ByVal CustomerDemographicCollection As CustomerDemographicCollection)
        MyBase.new()
        mCollection = CustomerDemographicCollection
    End Sub
#End Region
    
#Region "Base Class Implementation"
#End Region
    
#Region "Field access properties"
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
    Public Function CustomerDescColumnInfo As ColumnInfo
        Dim columnInfo As New ColumnInfo
        columnInfo.FieldName = "CustomerDesc"
        columnInfo.FieldType = GetType(System.String)
        columnInfo.SQLType = "NText"
        columnInfo.Caption = "Customer Desc"
        columnInfo.Desc = ""
        Return columnInfo
    End Function
    
    Public Property CustomerDesc As System.String
        Get
            Return mCustomerDesc
        End Get
        Set(ByVal Value As System.String)
            mCustomerDesc = Value
        End Set
    End Property
#End Region
    End Class
