' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: An abstraction of the information needed for the menu items

Option Strict On
Option Explicit On 

Imports System
Imports KADGen.BusinessObjectSupport


Public Class ActionItem
   Protected mBusinessObjectType As System.Type
   Protected mEditUCType As System.Type
   Protected mSelectUCType As System.Type
   Private mCaption As String
   Private mName As String
   Private Shared mMDIParent As Windows.Forms.Form
   Private Const vbcrlf As String = Microsoft.VisualBasic.vbCrLf

   Friend Sub New( _
               ByVal BusinessObjectType As System.Type, _
               ByVal EditUCType As System.Type, _
               ByVal SelectUCType As System.Type, _
               ByVal MDIParent As Windows.Forms.Form)
      mBusinessObjectType = BusinessObjectType
      mEditUCType = EditUCType
      mSelectUCType = SelectUCType
      mMDIParent = MDIParent
      mCaption = CType(Utility.InvokeSharedPropertyGet(mBusinessObjectType, "Caption"), System.String)
      mName = CType(Utility.InvokeSharedPropertyGet(mBusinessObjectType, "ObjectName"), System.String)
   End Sub

   Protected Function NewSelectUserControl() As BaseSelectUserControl
      Return CType(Utility.CreateInstance(mSelectUCType), BaseSelectUserControl)
   End Function

   Protected Function NewEditUserControl() As BaseEditUserControl
      Return CType(Utility.CreateInstance(mEditUCType), BaseEditUserControl)
   End Function

   'Protected Function NewBusinessObject() As CSLA.BusinessBase
   '   Return CType(Me.CreateInstance(mBusinessObjectType), CSLA.BusinessBase)
   'End Function

   Friend Function CanCreate() As Boolean
      Return CType(Utility.InvokeSharedMethod(Me.mBusinessObjectType, "CanCreate"), Boolean)
   End Function

   Friend Function CanUpdate() As Boolean
      Return CType(Utility.InvokeSharedMethod(Me.mBusinessObjectType, "CanUpdate"), Boolean)
   End Function

   Friend Function CanDelete() As Boolean
      Return CType(Utility.InvokeSharedMethod(Me.mBusinessObjectType, "CanDelete"), Boolean)
   End Function

   Friend ReadOnly Property Caption() As String
      Get
         Return mCaption
      End Get
   End Property

   Friend ReadOnly Property BusinessObjectName() As String
      Get
         Return CType(Utility.InvokeSharedPropertyGet(mBusinessObjectType, "ObjectName"), System.String)
      End Get
   End Property

   Friend Sub Edit(ByVal e As System.EventArgs)
      Dim frm As New RootEditForm
      frm.Show(Me.NewEditUserControl, _
               Me.NewSelectUserControl, _
               Me.mBusinessObjectType, _
               mMDIParent, "", True)
      'mMDIParent)
   End Sub


   'Friend Sub Edit(ByVal sender As System.Object, _
   '         ByVal e 
   '   Dim dlg As BaseSelectForm = Me.NewSelectForm
   '   Dim caption As String = Me.Caption
   '   Dim obj As IBusinessObject
   '   Dim frm As New WinSupport.EditForm
   '   Dim result As Object

   '   dlg.Text = "Edit " & caption
   '   dlg.ShowDialog(Windows.Forms.Form.ActiveForm)

   '   result = dlg.Result
   '   If Not Utility.IsEmpty(result) Then
   '      Try
   '         Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
   '         obj = CType(Me.InvokeSharedMethod( _
   '                  Me.mBusinessObjectType, "Get" & Me.BusinessObjectName, result), _
   '                  IBusinessObject)

   '         frm.Show(Me.NewEditUserControl, _
   '                  Windows.Forms.Form.ActiveForm, _
   '                  obj)
   '         Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
   '      Catch ex As Exception
   '         Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
   '         Windows.Forms.MessageBox.Show("Error loading " & caption & vbcrlf & ex.ToString)
   '      End Try
   '   End If

   'End Sub

   'Friend Sub Remove(ByVal sender As System.Object, _
   '            ByVal e As System.EventArgs)
   '   Dim dlg As BaseSelectForm = Me.NewSelectForm
   '   Dim caption As String = Me.Caption
   '   Dim boname As String = Me.BusinessObjectName

   '   dlg.Text = "Remove " & caption
   '   dlg.ShowDialog(Utility.ParentForm(sender))

   '   Dim Result As Object = dlg.Result
   '   If Not Utility.IsEmpty(Result) Then
   '      If Windows.Forms.MessageBox.Show("Remove " & caption & Result.ToString, _
   '                  "Remove " & caption, _
   '                   Windows.Forms.MessageBoxButtons.YesNo) = _
   '                     Windows.Forms.DialogResult.Yes Then
   '         Try
   '            Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
   '            Singletons.SetStatus("Deleting " & caption & "...")

   '            Me.InvokeSharedMethod(Me.mBusinessObjectType, "Delete" & boname, Result)

   '            Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
   '            Windows.Forms.MessageBox.Show(caption & " deleted")

   '         Catch ex As Exception
   '            Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
   '            Windows.Forms.MessageBox.Show("Error deleting " & caption & vbcrlf & ex.ToString)

   '         Finally
   '            Singletons.SetStatus("")
   '         End Try
   '      End If
   '   End If

   'End Sub

   'Friend Sub Create(ByVal sender As System.Object, _
   '            ByVal e As System.EventArgs)
   '   Dim frm As New WinSupport.EditForm

   '   Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
   '   frm.Show(Me.NewEditUserControl, _
   '            Utility.ParentForm(sender), _
   '            CType(InvokeSharedMethod(mBusinessObjectType, "New" & Me.BusinessObjectName), _
   '                        IBusinessObject))
   '   Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
   'End Sub


End Class