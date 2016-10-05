' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Extension of menu items to hold xsd information 

Option Explicit On 
Option Strict On

Imports System

Public Class MenuItemWithXSD
   Inherits Windows.Forms.MenuItem

   Private mXSDNode As Xml.XmlNode

   Public Sub New( _
                  ByVal text As String, _
                  ByVal xsdNode As Xml.XmlNode)
      MyBase.New(text)
      Me.XSDNode = xsdNode
   End Sub

   Public Sub New()
      MyBase.new()
   End Sub

   Public Sub New(ByVal text As String)
      MyBase.New(text)
   End Sub

   Public Property XSDNode() As Xml.XmlNode
      Get
         Return Me.mXSDNode
      End Get
      Set(ByVal value As Xml.XmlNode)
         Me.mXSDNode = value
      End Set
   End Property

End Class
