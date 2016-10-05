' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Action Menu Items are used in dynamic menuing

Option Explicit On 
Option Strict On

Imports System

Public Class ActionMenuItem
   Inherits Windows.forms.MenuItem

   Private mActionItem As ActionItem

   Private Const vbcrlf As String = Microsoft.VisualBasic.vbCrLf

   Public Sub New( _
               ByVal BusinessObjectType As System.Type, _
               ByVal EditUCType As System.Type, _
               ByVal SelectFormType As System.Type, _
               ByVal MDIParent As Windows.Forms.Form)
      MyBase.New("")
      mActionItem = New ActionItem(BusinessObjectType, EditUCType, SelectFormType, MDIParent)
      Me.Text = mActionItem.Caption
      'If mActionItem.CanCreate Then
      '   Me.MenuItems.Add("New " & mActionItem.Caption, AddressOf OnCreate)
      'End If
      'If mActionItem.CanUpdate Then
      '   Me.MenuItems.Add("Update " & mActionItem.Caption, AddressOf OnEdit)
      'End If
      'If mActionItem.CanDelete Then
      '   Me.MenuItems.Add("Delete " & mActionItem.Caption, AddressOf OnRemove)
      'End If
   End Sub

   Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
      mActionItem.Edit(e)
   End Sub

   'Private Sub OnCreate(ByVal sender As System.Object, _
   '            ByVal e As System.EventArgs)
   '   mActionItem.Create(sender, e)
   'End Sub

   'Private Sub OnRemove(ByVal sender As System.Object, _
   '            ByVal e As System.EventArgs)
   '   mActionItem.Remove(sender, e)
   'End Sub

   'Private Sub OnEdit(ByVal sender As System.Object, _
   '            ByVal e As System.EventArgs)
   '   mActionItem.Edit(sender, e)
   'End Sub


End Class
