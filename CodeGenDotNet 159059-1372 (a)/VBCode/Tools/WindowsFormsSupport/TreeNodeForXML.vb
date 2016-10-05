' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Specialized treenode that contains referneces to XML and XSD nodes

Option Explicit On 
Option Strict On

Imports System

Public Class TreeNodeForXML
   Inherits Windows.forms.TreeNode

   Private mXMLSchemaNode As Xml.XmlNode
   Private mXMLDataNode As Xml.XmlNode
   Private mCanAdd As Boolean

   Public Sub New( _
                  ByVal text As String, _
                  ByVal XMLDataNode As Xml.XmlNode, _
                  ByVal XMLSchemaNode As Xml.XmlNode)
      MyBase.New(text)
      Me.XMLDataNode = XMLDataNode
      Me.XMLSchemaNode = XMLSchemaNode
   End Sub

   Public Sub New(ByVal text As String)
      MyBase.New(text)
   End Sub

   Public Sub New()
      MyBase.new()
   End Sub

   Public Property XMLDataNode() As Xml.XmlNode
      Get
         Return Me.mXMLDataNode
      End Get
      Set(ByVal value As Xml.XmlNode)
         Me.mXMLDataNode = value
      End Set
   End Property

   Public Property XMLSchemaNode() _
                  As Xml.XmlNode
      Get
         Return Me.mXMLSchemaNode
      End Get
      Set(ByVal value As Xml.XmlNode)
         Me.mXMLSchemaNode = value
      End Set
   End Property

   Public Property CanAdd() As Boolean
      Get
         Return Me.mCanAdd
      End Get
      Set(ByVal value As Boolean)
         Me.mCanAdd = value
      End Set
   End Property

End Class
