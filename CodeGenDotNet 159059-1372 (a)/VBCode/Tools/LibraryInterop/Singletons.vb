' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Home for singleton methods

Option Strict On
Option Explicit On 

Imports System

Public Class Singletons
   Private Shared mXMLDoc As Xml.XmlDocument
   Public Shared NsMgr As Xml.XmlNamespaceManager

   Public Shared Property XMLDoc() As Xml.XmlDocument
      Get
         Return mXMLDoc
      End Get
      Set(ByVal Value As Xml.XmlDocument)
         mXMLDoc = Value
      End Set
   End Property
End Class
