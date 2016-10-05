' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Currently unused code that could be reused to isolate project settings. This proved to complex for most scenarios in testing.

Option Strict On
Option Explicit On 

Imports System

Public Class SettingsBase
   'NOTE: I used get functions rather than readonly properties with 
   '      parameters because I planned to convert this code to C#

   Protected mNode As Xml.XmlNode
   Protected mNsmgr As Xml.XmlNamespaceManager
   Const nspaceName As String = "http://kadgen.com/KADGenDriving.xsd"

   Protected Friend Overridable Property Node() As Xml.XmlNode
      Get
         Return mNode
      End Get
      Set(ByVal Value As Xml.XmlNode)
         mNode = Value
         mNsmgr = New Xml.XmlNamespaceManager(mNode.OwnerDocument.NameTable)
         mNsmgr.AddNamespace("kg", nspaceName)
      End Set
   End Property

End Class
