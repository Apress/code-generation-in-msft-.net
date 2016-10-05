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

Public Class LocalSettings
   Inherits SettingsBase
   'NOTE: I used get functions rather than readonly properties with 
   '      parameters because I planned to convert this code to C#
   Const localNspaceName As String = "http://kadgen.com/KADGenLocalSettings.xsd"

   Protected Friend Overrides Property Node() As System.Xml.XmlNode
      Get
         Return MyBase.Node
      End Get
      Set(ByVal Value As System.Xml.XmlNode)
         MyBase.Node = Value
         mNsmgr.AddNamespace("kl", localNspaceName)
      End Set
   End Property

   Protected Friend Function GetBasePath() As String
      If mnode Is Nothing Then
         Return ""
      Else
         Dim elem As Xml.XmlNode
         elem = mNode.SelectSingleNode("kl:BasePath", mNsmgr)
         Return Utility.Tools.GetAttributeOrEmpty(elem, "Path")
      End If
   End Function

End Class
