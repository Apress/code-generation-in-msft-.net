' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Interface for all metadata extraction tools

Option Strict On
Option Explicit On 

Imports System
Imports System.Data
Imports System.Diagnostics

'! Class Summary: Interface for metadata extracton 

Public Interface IExtractMetaData
   Property UseVerboseNames() As Boolean

   Property UseProcContents() As Boolean

   Property ServerName() As String

   Function CreateMetaData( _
               ByVal skipStoredProcs As Boolean, _
               ByVal setSelectPatterns As String, _
               ByVal selectPatterns As String, _
               ByVal removePrefix As String, _
               ByVal lookupPrefix As String, _
               ByVal ParamArray databaseNames() As String) _
               As Xml.XmlDocument


End Interface
