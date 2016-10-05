' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Interface for lists  -->

Option Explicit On 
Option Strict On

Imports System

Public Interface IListInfo
   Function GetPrimaryKey() As Object
   Property UniqueKey() As String
   Property DisplayText() As String
End Interface
