' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Provides a specialized control for directives 

Option Explicit On 
Option Strict On

Imports System
Imports KADGen

Public Class GenerationDIrectiveUserControl
   Inherits KADGen.WinFormSupport.SimpleXMLNodeUserControl

   Public Overrides Sub MarkAsChecked(ByVal check As Boolean)
      For Each ctl As Windows.Forms.Control In Me.Controls
         If ctl.Text.ToUpper = "STANDARD" Then
            For Each ctl1 As Windows.Forms.Control In Me.Controls(0).Controls
               If ctl1.Name.ToUpper = "CHKCHECKED" Then
                  CType(ctl1, Windows.Forms.CheckBox).Checked = check
               End If
            Next
         End If
      Next
   End Sub

End Class
