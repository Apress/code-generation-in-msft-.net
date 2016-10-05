' ***^^^***|||***^^^***' ' ' ' %%%###%%%ad649326b958426e16761682c9671ce2%%%###%%%' ***^^^***|||***^^^***

Option Strict On
Option Explicit On

Imports System

Imports KADGen
Imports System

#Region "Description"
'
' Region.vb
'
' Last Genned On Date: 1/28/2004 9:07:38 AM
'
'
#End Region

Public Class RegionCollection
	Inherits CollectionBase
	
#Region "Constructors"
	Protected Sub New()
      MyBase.New("RegionCollection")
   End Sub
#End Region

#Region "Public and Friend Properties, Methods and Events"
   Public Overloads Sub Fill( _
		      ByVal RegionID As int, _
            ByVal UserID As Int32)
		RegionDataAccessor.Fill(Me, RegionID, UserID)
   End Sub
#End Region

End Class

Public Class Region
   Inherits RowBase
	
#Region "Class Level Declarations"
   Protected mCollection As RegionCollection
   Private Shared mNextPrimaryKey As Int32 = -1
   
			Private mRegionIDAs int
			Private mRegionDescriptionAs string
#End Region

#Region "Constructors"
   Friend Sub New(ByVal RegionCollection As RegionCollection)
      MyBase.new()
      mCollection = RegionCollection
   End Sub
#End Region

#Region "Base Class Implementation"
#End Region

#Region "Field access properties"
   

   Public Function RegionIDColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "RegionID"
      columnInfo.FieldType = GetType(int)
      columnInfo.SQLType = "Int"
      columnInfo.Caption = "Region ID"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property RegionID As int

      Get
         Return mRegionID

      End Get
      Set(ByVal Value As int)
         mRegionID = Value
      End Set
   End Property
		

   Public Function RegionDescriptionColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "RegionDescription"
      columnInfo.FieldType = GetType(string)
      columnInfo.SQLType = "NChar"
      columnInfo.Caption = "Region Description"
      columnInfo.Desc = ""
      Return columnInfo
   End Function
	
   Public Property RegionDescription As string

      Get
         Return mRegionDescription

      End Get
      Set(ByVal Value As string)
         mRegionDescription = Value
      End Set
   End Property
		
#End Region

End Class	
