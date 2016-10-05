' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Container form for editing control as a child
'  NOTE: Root and child are somewhat misnomers. The exact meaning 
'        is whether users will be able to add and remove records and
'        whether selectin will be allowed. 

Option Strict On
Option Explicit On 

Imports System
Imports System.Windows.Forms
Imports KADGen.BusinessObjectSupport

Public Class ChildEditForm
   Inherits BaseEditForm

 
#Region "Class Declarations"
   Private mbIsLoaded As Boolean
   'Private mObject As IBusinessObject
   Private mCallingForm As Windows.Forms.Form
   Private mBusinessObjectType As System.Type
   'Private WithEvents mEditUserControl As BaseEditUserControl
   'Private mFormMode As FormMode
   Private mDlgResult As Windows.Forms.DialogResult
#End Region

#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
      mBtnLast = Me.btnClose
   End Sub

   'Form overrides dispose to clean up the component list.
   Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
         If Not (components Is Nothing) Then
            components.Dispose()
         End If
      End If
      MyBase.Dispose(disposing)
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   Friend WithEvents Panel1 As System.Windows.Forms.Panel
   Friend WithEvents pnlButtons As System.Windows.Forms.Panel
   Friend WithEvents btnClose As System.Windows.Forms.Button
   Friend WithEvents btnCancel As System.Windows.Forms.Button
   Friend WithEvents btnSave As System.Windows.Forms.Button
   Friend WithEvents pnlShim As System.Windows.Forms.Panel
   Friend WithEvents pnlback As System.Windows.Forms.Panel
   Friend WithEvents pnlEdit As System.Windows.Forms.Panel
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.Panel1 = New System.Windows.Forms.Panel
      Me.pnlback = New System.Windows.Forms.Panel
      Me.pnlEdit = New System.Windows.Forms.Panel
      Me.pnlShim = New System.Windows.Forms.Panel
      Me.pnlButtons = New System.Windows.Forms.Panel
      Me.btnClose = New System.Windows.Forms.Button
      Me.btnCancel = New System.Windows.Forms.Button
      Me.btnSave = New System.Windows.Forms.Button
      Me.Panel1.SuspendLayout()
      Me.pnlback.SuspendLayout()
      Me.pnlButtons.SuspendLayout()
      Me.SuspendLayout()
      '
      'Panel1
      '
      Me.Panel1.Controls.Add(Me.pnlback)
      Me.Panel1.Controls.Add(Me.pnlButtons)
      Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
      Me.Panel1.Location = New System.Drawing.Point(0, 0)
      Me.Panel1.Name = "Panel1"
      Me.Panel1.Size = New System.Drawing.Size(352, 198)
      Me.Panel1.TabIndex = 19
      '
      'pnlback
      '
      Me.pnlback.Controls.Add(Me.pnlEdit)
      Me.pnlback.Controls.Add(Me.pnlShim)
      Me.pnlback.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pnlback.Location = New System.Drawing.Point(0, 0)
      Me.pnlback.Name = "pnlback"
      Me.pnlback.Size = New System.Drawing.Size(272, 198)
      Me.pnlback.TabIndex = 20
      '
      'pnlEdit
      '
      Me.pnlEdit.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pnlEdit.Location = New System.Drawing.Point(0, 8)
      Me.pnlEdit.Name = "pnlEdit"
      Me.pnlEdit.Size = New System.Drawing.Size(272, 190)
      Me.pnlEdit.TabIndex = 1
      '
      'pnlShim
      '
      Me.pnlShim.Dock = System.Windows.Forms.DockStyle.Top
      Me.pnlShim.Location = New System.Drawing.Point(0, 0)
      Me.pnlShim.Name = "pnlShim"
      Me.pnlShim.Size = New System.Drawing.Size(272, 8)
      Me.pnlShim.TabIndex = 0
      '
      'pnlButtons
      '
      Me.pnlButtons.Controls.Add(Me.btnClose)
      Me.pnlButtons.Controls.Add(Me.btnCancel)
      Me.pnlButtons.Controls.Add(Me.btnSave)
      Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right
      Me.pnlButtons.Location = New System.Drawing.Point(272, 0)
      Me.pnlButtons.Name = "pnlButtons"
      Me.pnlButtons.Size = New System.Drawing.Size(80, 198)
      Me.pnlButtons.TabIndex = 19
      '
      'btnClose
      '
      Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnClose.Location = New System.Drawing.Point(0, 56)
      Me.btnClose.Name = "btnClose"
      Me.btnClose.TabIndex = 15
      Me.btnClose.Text = "Close"
      '
      'btnCancel
      '
      Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnCancel.Location = New System.Drawing.Point(0, 32)
      Me.btnCancel.Name = "btnCancel"
      Me.btnCancel.TabIndex = 14
      Me.btnCancel.Text = "&Cancel"
      '
      'btnSave
      '
      Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnSave.Location = New System.Drawing.Point(0, 8)
      Me.btnSave.Name = "btnSave"
      Me.btnSave.TabIndex = 13
      Me.btnSave.Text = "&Save"
      '
      'ChildEditForm
      '
      Me.AcceptButton = Me.btnSave
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.CancelButton = Me.btnCancel
      Me.ClientSize = New System.Drawing.Size(352, 198)
      Me.Controls.Add(Me.Panel1)
      Me.Name = "ChildEditForm"
      Me.Text = "EditForm"
      Me.Panel1.ResumeLayout(False)
      Me.pnlback.ResumeLayout(False)
      Me.pnlButtons.ResumeLayout(False)
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region "Constructors"
#End Region

#Region " Public Properties and Methods "

#Region "Show and ShowDialog Overloads"

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal businessObject As CSLA.BusinessBase , _
            ByVal parent As Windows.forms.Form)
      Me.Show(editUserControl, businessObject, _
               parent, "")
   End Sub

   Public Overloads Function ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal businessObject As CSLA.BusinessBase, _
            ByVal parentPrimaryKey As String, _
            ByVal parent As Windows.forms.Form) _
            As Windows.Forms.DialogResult
      Me.ShowDialog(editUserControl, businessObject, parentPrimaryKey, _
               parent, "")
      Return Me.mDlgResult
   End Function

   Public Overloads Sub Show( _
         ByVal editUserControl As WinSupport.BaseEditUserControl, _
         ByVal businessObject As CSLA.BusinessBase, _
         ByVal parent As Windows.forms.Form, _
         ByVal title As String)
      Me.Show(editUserControl, businessObject, _
                  parent, title, 0, 0)
   End Sub

   Public Overloads Function ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal businessObject As CSLA.BusinessBase, _
            ByVal parentPrimaryKey As String, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String) _
            As Windows.Forms.DialogResult
      Me.ShowDialog(editUserControl, businessObject, _
                  parentPrimaryKey, parent, title, 0, 0)
      Return Me.mDlgResult
   End Function

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal businessObject As CSLA.BusinessBase, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal openingWidth As Int32)
      Me.Show(editUserControl, businessObject, _
                  parent, title, openingWidth, 0)
   End Sub

   Public Overloads Function ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal businessObject As CSLA.BusinessBase, _
            ByVal parentPrimaryKey As String, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal openingWidth As Int32) _
            As Windows.Forms.DialogResult
      Me.ShowDialog(editUserControl, businessObject, _
                  parentPrimaryKey, parent, title, openingWidth, 0)
      Return Me.mDlgResult
   End Function

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal businessObject As CSLA.BusinessBase, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal openingWidth As Int32, _
            ByVal openingHeight As Int32)
      Me.SetupForm(editUserControl, _
                  businessObject, "", parent, title, _
                  openingWidth, openingHeight, False)
      MyBase.Show()
   End Sub

   Public Overloads Function ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal businessObject As CSLA.BusinessBase, _
            ByVal parentPrimaryKey As String, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal openingWidth As Int32, _
            ByVal openingHeight As Int32) _
            As Windows.Forms.DialogResult
      Me.SetupForm(editUserControl, _
                  businessObject, parentPrimaryKey, parent, title, _
                  openingWidth, openingHeight, True)
      MyBase.ShowDialog(parent)
      Return Me.mDlgResult
   End Function

#End Region
   'Public Overloads Function ShowDialog( _
   '          ByVal editUserControl As WinSupport.BaseEditUserControl, _
   '          ByVal busObject As CSLA.BusinessBase, _
   '          ByVal parent As Windows.forms.Form) _
   '          As Windows.Forms.DialogResult
   '   Dim dlgResult As Windows.Forms.DialogResult
   '   SetupForm(editUserControl, busObject, "", parent)
   '   Me.ShowDialog()
   '   Return mDlgResult
   'End Function

   'Public Overloads Function ShowDialog( _
   '         ByVal editUserControl As WinSupport.BaseEditUserControl, _
   '         ByVal busObject As CSLA.BusinessBase, _
   '         ByVal parentPrimaryKey As String, _
   '         ByVal parent As Windows.forms.Form) _
   '         As Windows.Forms.DialogResult
   '   Dim dlgResult As Windows.Forms.DialogResult
   '   SetupForm(editUserControl, busObject, parentPrimaryKey, parent)
   '   Me.ShowDialog()
   '   Return mDlgResult
   'End Function

   'Public Overloads Sub Show( _
   '         ByVal editUserControl As WinSupport.BaseEditUserControl, _
   '         ByVal busObject As CSLA.BusinessBase, _
   '         ByVal parent As Windows.forms.Form)
   '   If parent.IsMdiContainer Then
   '      Me.MdiParent = parent
   '   Else
   '      If Not parent.MdiParent Is Nothing Then
   '         Me.MdiParent = parent.MdiParent
   '      Else
   '         Me.Parent = parent
   '      End If
   '   End If
   '   SetupForm(editUserControl, busObject, "", parent)
   '   Me.Show()
   'End Sub

   Public Property BusinessObject() As IBusinessObject
      Get
         Return mObject
      End Get
      Set(ByVal Value As IBusinessObject)
         mObject = Value
      End Set
   End Property

   Public Property CallingForm() As Windows.Forms.Form
      Get
         Return mCallingForm
      End Get
      Set(ByVal Value As Windows.Forms.Form)
         mCallingForm = Value
      End Set
   End Property

#End Region

#Region " Event Handlers "
   Private Sub btnCancel_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles btnCancel.Click
      OnBtnCancelClick(sender, e)
   End Sub

   Private Sub btnSave_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles btnSave.Click
      OnBtnSaveClick(sender, e)
   End Sub

   Private Sub btnClose_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles btnClose.Click
      OnBtnCloseClick(sender, e)
   End Sub

   Private Sub uc_DataChanged( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles mEditUserControl.DataChanged
      Diagnostics.Debug.WriteLine("Here we are")
      Me.SetState()
   End Sub

#End Region

#Region "Protected Event Response"
   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
      Dim vMargin As Int32 = CType(Me.mEditUserControl, IEditUserControl).VerticalMargin
      Dim height As Int32
      Dim width As Int32

      MyBase.OnLoad(e)
      If Me.CallingForm Is Nothing Then
         mFormMode = FormMode.Root
      ElseIf TypeOf Me.CallingForm Is BaseEditForm Then
         mFormMode = FormMode.Child
      Else
         mFormMode = FormMode.Root
      End If
      If mFormMode = FormMode.Child Then
         btnSave.Text = "OK"
      Else
         btnSave.Text = "Save"
      End If

      For Each ctl As Windows.Forms.Control In Me.Controls
         If TypeOf ctl Is IEditUserControl Then
            CType(ctl, IEditUserControl).SetupControl(mObject)
         End If
      Next

      '  Me.pnlButtons.BringToFront()
      Me.pnlButtons.Dock = DockStyle.Right
      Me.pnlEdit.Dock = DockStyle.Fill
      Me.mEditUserControl.Dock = DockStyle.None
      Me.mEditUserControl.Dock = DockStyle.Fill

      'Height = CType(Me.mEditUserControl, IEditUserControl).IdealHeight
      With CType(Me.mEditUserControl, IEditUserControl)
         If .IdealHeight > 0 Then
            height = .IdealHeight
         Else
            height = mEditUserControl.Height
         End If
         If .IdealWidth > 0 Then
            width = .IdealWidth + Me.pnlButtons.Width
         Else
            width = Me.ClientSize.Width
         End If
      End With
      If height < mBtnLast.Bottom + vMargin Then
         height = mBtnLast.Bottom + vMargin
      Else
         height += 2 * vMargin
      End If
      Me.ClientSize = New Drawing.Size(width, height)

      mbIsLoaded = True

   End Sub

   'Protected Overrides Sub OnLayout(ByVal levent As System.Windows.Forms.LayoutEventArgs)
   '   ResizeForm(Nothing, Nothing, _
   '               Me.mEditUserControl, Me.pnlEdit, Me.pnlButtons, _
   '               Nothing)
   'End Sub

   Protected Overrides Sub OnActivated( _
                     ByVal e As System.EventArgs)
      SetState()
   End Sub

   'Protected Overrides Sub OnClosing( _
   '                  ByVal e As System.ComponentModel.CancelEventArgs)
   '   If Not mObject Is Nothing Then
   '      mObject.CancelEdit()
   '   End If
   'End Sub

   Protected Overridable Sub OnBtnSaveClick( _
              ByVal sender As System.Object, _
              ByVal e As System.EventArgs)
      Me.BindingContext(mObject).EndCurrentEdit()
      mDlgResult = Windows.Forms.DialogResult.OK
      If mFormMode = FormMode.Root Then
         Try
            Cursor.Current = Cursors.WaitCursor
            Me.mEditUserControl.VisitControls()
            Dim bValid As Boolean = Me.mEditUserControl.IsFormValid()
            Me.btnSave.Focus()
            If Not bValid Then
               Windows.Forms.MessageBox.Show( _
                           "Please correct the errors on this form. You can hover the mouse over the red symbol next to each field to find out what the problem is.", _
                           "Input Errors", _
                           Windows.Forms.MessageBoxButtons.OK, _
                           MessageBoxIcon.Error)
            Else
               mObject = CType(mEditUserControl, IEditUserControl).Save()
               Cursor.Current = Cursors.Default
            End If
         Catch ex As Exception
            Cursor.Current = Cursors.Default
            Throw
         End Try
      Else
         Me.Hide()
      End If
      SetState()
   End Sub

   Protected Overridable Sub OnBtnCancelClick( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs)
      mDlgResult = Windows.Forms.DialogResult.Cancel
      mObject.CancelEdit()
      If mFormMode = FormMode.Child Then
         Me.Hide()
      Else
         With CType(Me.mEditUserControl, IEditUserControl)
            '.BusinessObject = mObject
            .SetupControl(mObject)
         End With
         Me.mEditUserControl.VisitControls()
         Me.SetState()
      End If
   End Sub

   Protected Overridable Sub OnBtnCloseClick( _
            ByVal sender As System.Object, _
            ByVal e As System.EventArgs)
      Me.Hide()
   End Sub

#End Region

#Region "Protected Properties and Methods"
   Protected Overridable Sub DataBindButtons()
      If CType(mEditUserControl, IEditUserControl).CanCreate() Then
         Utility.BindField(btnSave, "Enabled", mObject, "IsValid")
      Else
         btnSave.Enabled = False
      End If
   End Sub

   Protected Sub SetupForm( _
               ByVal editUserControl As BaseEditUserControl, _
               ByVal busObject As CSLA.BusinessBase, _
               ByVal parentPrimaryKey As String, _
               ByVal parent As Windows.Forms.Form, _
               ByVal title As String, _
               ByVal openingWidth As Int32, _
               ByVal openingHeight As Int32, _
               ByVal isModal As Boolean)
      Dim iWidth As Int32
      Dim iHeight As Int32
      Me.mEditUserControl = editUserControl
      Me.mEditUserControl.BringToFront()
      Me.pnlEdit.Controls.Add(editUserControl)
      mObject = CType(busObject, BusinessObjectSupport.IBusinessObject)
      If title Is Nothing OrElse title.Trim.Length = 0 Then
         Me.Text = editUserControl.Caption
      Else
         Me.Text = title
      End If
      If Not isModal AndAlso Not parent Is Nothing AndAlso parent.IsMdiContainer Then
         Me.MdiParent = parent
      End If
      With CType(Me.mEditUserControl, IEditUserControl)
         '.BusinessObject = mObject
         .SetupControl(mObject, parentPrimaryKey)
      End With
      mCallingForm = parent
      ' editUserControl.Width = Me.ClientSize.Width - pnlButtons.Width
      'Me.Height = Me.pnlEdit.Top + editUserControl.Height + Me.Height - Me.ClientSize.Height
      'Me.pnlEdit.Controls.Add(editUserControl)
      'ResizeForm(Nothing, Nothing, Me.mEditUserControl, pnlEdit, Me.pnlButtons, Nothing)
      If openingWidth = 0 Then
         iWidth = Me.ClientSize.Width + 1
      Else
         iWidth = openingWidth
      End If
      If openingHeight = 0 Then
         iHeight = Me.ClientSize.Height + 1
      Else
         iHeight = openingHeight
      End If
      If iHeight < Me.CallingForm.ClientSize.Height Then
         iHeight = Me.CallingForm.ClientSize.Height
      End If
      Me.ClientSize = New Drawing.Size(iWidth, iHeight)
      SetState()
      'editUserControl.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
   End Sub
#End Region

#Region "Private Properties and Methods"
   'Private Sub SelectFromShow( _
   '         ByVal editUserControl As WinSupport.BaseEditUserControl, _
   '         ByVal businessobjecttype As System.Type, _
   '         ByVal parent As Windows.forms.Form, _
   '         ByVal title As String, _
   '         ByVal showToolbar As Boolean)
   '   Me.pnlEdit.Controls.Add(editUserControl)
   '   Me.ClientSize = New System.Drawing.Size( _
   '                  editUserControl.Width + pnlButtons.Width, _
   '                  editUserControl.Height)
   '   editUserControl.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom
   '   'editUserControl.Dock = DockStyle.Fill
   '   Me.mEditUserControl = editUserControl
   '   Me.mBusinessObjectType = businessobjecttype
   '   Me.Text = title
   '   Me.MdiParent = parent
   'End Sub

   Private Sub DisplayForEdits(ByVal e As System.EventArgs, ByVal pk As Object)
      Dim bo As IBusinessObject
      Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
      bo = CType(Utility.InvokeSharedMethod( _
                  Me.mBusinessObjectType, _
                  "Get" & Me.mBusinessObjectType.Name, _
                  pk), _
               IBusinessObject)
      mObject = bo
      With CType(Me.mEditUserControl, IEditUserControl)
         '.BusinessObject = CType(bo, BusinessObjectSupport.IBusinessObject)
         .SetupControl(CType(bo, BusinessObjectSupport.IBusinessObject))
      End With
      Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
   End Sub

   'Private Sub ResizeForm()
   '   Dim height As Int32
   '   Dim vMargin As Int32 = CType(Me.mEditUserControl, IEditUserControl).VerticalMargin
   '   mEditUserControl.Width = Me.ClientSize.Width - pnlButtons.Width
   '   height = mEditUserControl.Height
   '   If height < mBtnLast.Bottom + vMargin Then
   '      height = mbtnlast.Bottom + vMargin
   '   Else
   '      height += 2 * vMargin
   '   End If
   '   height += vMargin
   '   Me.ClientSize = New Drawing.Size(Me.Width, height)
   'End Sub

   Private Sub SetState()
      With CType(Me.mEditUserControl, IEditUserControl)
         Dim editmode As EditMode = .EditMode
         Me.btnSave.Enabled = ((editmode And editmode.IsDirty) > 0)
         Me.btnCancel.Enabled = ((editmode And editmode.IsDirty) > 0) Or _
                                 ((editmode And editmode.IsNew) > 0)
         Me.btnClose.Enabled = Not btnCancel.Enabled
      End With
   End Sub
#End Region


End Class
