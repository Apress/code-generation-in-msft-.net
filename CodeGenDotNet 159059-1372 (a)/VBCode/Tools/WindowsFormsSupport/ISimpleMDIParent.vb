' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Interface for MDI parent

Option Explicit On 
Option Strict On

Imports System

Public Interface ISimpleMDIParent
   Sub EnableSave(ByVal enable As Boolean)
   Sub EnableClose(ByVal enable As Boolean)
End Interface
