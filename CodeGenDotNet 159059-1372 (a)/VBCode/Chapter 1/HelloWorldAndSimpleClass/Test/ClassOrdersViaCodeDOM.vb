'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.573
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace ClassViaCodeDOM
    
    Public Class Orders
        
        Private m_OrderID As Int32
        
        Private m_CustomerID As [String]
        
        Private m_EmployeeID As Int32
        
        Private m_OrderDate As DateTime
        
        Private m_RequiredDate As DateTime
        
        Private m_ShippedDate As DateTime
        
        Private m_ShipVia As Int32
        
        Private m_Freight As [Decimal]
        
        Private m_ShipName As [String]
        
        Private m_ShipAddress As [String]
        
        Private m_ShipCity As [String]
        
        Private m_ShipRegion As [String]
        
        Private m_ShipPostalCode As [String]
        
        Private m_ShipCountry As [String]
        
        Public Overridable Property OrderID As Int32
            Get
                Return Me.m_OrderID
            End Get
            Set
                Me.m_OrderID = value
            End Set
        End Property
        
        Public Overridable Property CustomerID As [String]
            Get
                Return Me.m_CustomerID
            End Get
            Set
                Me.m_CustomerID = value
            End Set
        End Property
        
        Public Overridable Property EmployeeID As Int32
            Get
                Return Me.m_EmployeeID
            End Get
            Set
                Me.m_EmployeeID = value
            End Set
        End Property
        
        Public Overridable Property OrderDate As DateTime
            Get
                Return Me.m_OrderDate
            End Get
            Set
                Me.m_OrderDate = value
            End Set
        End Property
        
        Public Overridable Property RequiredDate As DateTime
            Get
                Return Me.m_RequiredDate
            End Get
            Set
                Me.m_RequiredDate = value
            End Set
        End Property
        
        Public Overridable Property ShippedDate As DateTime
            Get
                Return Me.m_ShippedDate
            End Get
            Set
                Me.m_ShippedDate = value
            End Set
        End Property
        
        Public Overridable Property ShipVia As Int32
            Get
                Return Me.m_ShipVia
            End Get
            Set
                Me.m_ShipVia = value
            End Set
        End Property
        
        Public Overridable Property Freight As [Decimal]
            Get
                Return Me.m_Freight
            End Get
            Set
                Me.m_Freight = value
            End Set
        End Property
        
        Public Overridable Property ShipName As [String]
            Get
                Return Me.m_ShipName
            End Get
            Set
                Me.m_ShipName = value
            End Set
        End Property
        
        Public Overridable Property ShipAddress As [String]
            Get
                Return Me.m_ShipAddress
            End Get
            Set
                Me.m_ShipAddress = value
            End Set
        End Property
        
        Public Overridable Property ShipCity As [String]
            Get
                Return Me.m_ShipCity
            End Get
            Set
                Me.m_ShipCity = value
            End Set
        End Property
        
        Public Overridable Property ShipRegion As [String]
            Get
                Return Me.m_ShipRegion
            End Get
            Set
                Me.m_ShipRegion = value
            End Set
        End Property
        
        Public Overridable Property ShipPostalCode As [String]
            Get
                Return Me.m_ShipPostalCode
            End Get
            Set
                Me.m_ShipPostalCode = value
            End Set
        End Property
        
        Public Overridable Property ShipCountry As [String]
            Get
                Return Me.m_ShipCountry
            End Get
            Set
                Me.m_ShipCountry = value
            End Set
        End Property
    End Class
End Namespace