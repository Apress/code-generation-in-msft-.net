' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Entry point for application 

Option Strict On
Option Explicit On 

Imports System
Imports System.Windows.forms

Public Class Startup
   Private Shared frmHarness As New HarnessForm
   Const vbcrlf As String = Microsoft.VisualBasic.ControlChars.CrLf

   Public Shared Sub main()
      Dim exceptionHandler As New GlobalExceptionHandler
      ' Adds the event handler to to the event.
      AddHandler Application.ThreadException, AddressOf exceptionHandler.OnThreadException
      Windows.Forms.Application.Run(frmHarness)
   End Sub

   Public Class GlobalExceptionHandler

      ' Handles the exception event.
      Public Sub OnThreadException(ByVal sender As Object, ByVal t As System.threading.ThreadExceptionEventArgs)
         Dim result As DialogResult = System.Windows.Forms.DialogResult.Cancel
         Try
            result = Me.ShowThreadExceptionDialog(t.Exception)
         Catch exLocal As System.Exception
            Try
               MessageBox.Show("Fatal Error" & vbcrlf & vbcrlf & t.Exception.ToString, "Fatal Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop)
            Finally
               Application.Exit()
            End Try
         End Try

         ' Exits the program when the user clicks Abort.
         If (result = System.Windows.Forms.DialogResult.Abort) Then
            Application.Exit()
         End If
      End Sub

      ' Creates the error message and displays it.
      Private Function ShowThreadExceptionDialog(ByVal ex As Exception) As DialogResult
         Dim frm As New ExceptionDisplay.ExceptionDisplay
         frm.Show(ex, Startup.frmHarness.CurrentProcessNode, Startup.frmHarness.currentprocessfile)
      End Function
   End Class

End Class
