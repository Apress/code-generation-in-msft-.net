' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Provides a home for singleton methods 

Option Strict On
Option Explicit On 

Imports System

Public Class Singletons
   Public Shared Sub SetStatus(ByVal StatusMessage As String)
      Main.SetStatus(StatusMessage)
   End Sub

   Public Shared Function GetBusinessObjectAssembly() As Reflection.Assembly
      Return GetType(KADGen.BusinessObjects.Singletons).Assembly
   End Function
End Class
