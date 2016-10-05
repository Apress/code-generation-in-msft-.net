' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Interface for BusinessObject Support 

Option Strict On
Option Explicit On 

Imports KADGen.BusinessObjectSupport

Public Interface IBusinessObject
   ReadOnly Property Caption() As String
   ReadOnly Property ObjectName() As String
   ReadOnly Property IsDirty() As Boolean
   ReadOnly Property IsValid() As Boolean
   Property UniqueKey() As String
   Property DisplayText() As String
   Sub CancelEdit()
   Sub ApplyEdit()
   Function Save() As IBusinessObject
   Function GetNew() As IBusinessObject
   Function CanUpdate() As Boolean
   Function CanCreate() As Boolean
   Function CanDelete() As Boolean
   Function CanRetrieve() As Boolean
End Interface


