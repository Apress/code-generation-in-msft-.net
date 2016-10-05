' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Container form for editing control as a root
'  NOTE: Root and child are somewhat misnomers. The exact meaning 
'        is whether users will be able to add and remove records and
'        whether selectin will be allowed. 

Option Strict On
Option Explicit On 

Imports System
Imports System.Windows.Forms
Imports KADGen.BusinessObjectSupport

Public Class RootEditForm
   Inherits BaseEditForm


#Region "Class Declarations"
   Private mbIsLoaded As Boolean
   'Private mObject As IBusinessObject
   Private mCallingForm As Windows.Forms.Form
   Private mBusinessObjectType As System.Type
   Private WithEvents mSelectUserControl As BaseSelectUserControl
   'Private WithEvents mEditUserControl As BaseEditUserControl
   'Private mFormMode As FormMode
#End Region

#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
      mbtnlast = btnClose

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
   Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
   Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
   Friend WithEvents ToolBar As System.Windows.Forms.ToolBar
   Friend WithEvents ImageList As System.Windows.Forms.ImageList
   Friend WithEvents tbbSearch As System.Windows.Forms.ToolBarButton
   Friend WithEvents tbbGrid As System.Windows.Forms.ToolBarButton
   Friend WithEvents tbbSave As System.Windows.Forms.ToolBarButton
   Friend WithEvents tbbCancel As System.Windows.Forms.ToolBarButton
   Friend WithEvents pnlButtons As System.Windows.Forms.Panel
   Friend WithEvents lblSearch As System.Windows.Forms.Label
   Friend WithEvents btnCancel As System.Windows.Forms.Button
   Friend WithEvents btnSave As System.Windows.Forms.Button
   Friend WithEvents pnlBottom As System.Windows.Forms.Panel
   Friend WithEvents pnlEdit As System.Windows.Forms.Panel
   Friend WithEvents cboSelect As System.Windows.Forms.ComboBox
   Friend WithEvents btnNew As System.Windows.Forms.Button
   Friend WithEvents btnDelete As System.Windows.Forms.Button
   Friend WithEvents pnlSelect As System.Windows.Forms.Panel
   Friend WithEvents tbbDelete As System.Windows.Forms.ToolBarButton
   Friend WithEvents tbbNew As System.Windows.Forms.ToolBarButton
   Friend WithEvents btnClose As System.Windows.Forms.Button
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RootEditForm))
      Me.pnlSelect = New System.Windows.Forms.Panel
      Me.cboSelect = New System.Windows.Forms.ComboBox
      Me.lblSearch = New System.Windows.Forms.Label
      Me.Splitter1 = New System.Windows.Forms.Splitter
      Me.Splitter2 = New System.Windows.Forms.Splitter
      Me.pnlBottom = New System.Windows.Forms.Panel
      Me.pnlEdit = New System.Windows.Forms.Panel
      Me.pnlButtons = New System.Windows.Forms.Panel
      Me.btnClose = New System.Windows.Forms.Button
      Me.btnDelete = New System.Windows.Forms.Button
      Me.btnNew = New System.Windows.Forms.Button
      Me.btnCancel = New System.Windows.Forms.Button
      Me.btnSave = New System.Windows.Forms.Button
      Me.ToolBar = New System.Windows.Forms.ToolBar
      Me.tbbSearch = New System.Windows.Forms.ToolBarButton
      Me.tbbGrid = New System.Windows.Forms.ToolBarButton
      Me.tbbSave = New System.Windows.Forms.ToolBarButton
      Me.tbbCancel = New System.Windows.Forms.ToolBarButton
      Me.tbbDelete = New System.Windows.Forms.ToolBarButton
      Me.tbbNew = New System.Windows.Forms.ToolBarButton
      Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
      Me.pnlSelect.SuspendLayout()
      Me.pnlBottom.SuspendLayout()
      Me.pnlButtons.SuspendLayout()
      Me.SuspendLayout()
      '
      'pnlSelect
      '
      Me.pnlSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.pnlSelect.Controls.Add(Me.cboSelect)
      Me.pnlSelect.Controls.Add(Me.lblSearch)
      Me.pnlSelect.Dock = System.Windows.Forms.DockStyle.Top
      Me.pnlSelect.Location = New System.Drawing.Point(0, 28)
      Me.pnlSelect.Name = "pnlSelect"
      Me.pnlSelect.Size = New System.Drawing.Size(456, 36)
      Me.pnlSelect.TabIndex = 11
      '
      'cboSelect
      '
      Me.cboSelect.Location = New System.Drawing.Point(112, 8)
      Me.cboSelect.Name = "cboSelect"
      Me.cboSelect.Size = New System.Drawing.Size(168, 21)
      Me.cboSelect.TabIndex = 1
      Me.cboSelect.Text = "ComboBox1"
      '
      'lblSearch
      '
      Me.lblSearch.Location = New System.Drawing.Point(8, 8)
      Me.lblSearch.Name = "lblSearch"
      Me.lblSearch.TabIndex = 0
      Me.lblSearch.Text = "Select"
      '
      'Splitter1
      '
      Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter1.Location = New System.Drawing.Point(0, 64)
      Me.Splitter1.Name = "Splitter1"
      Me.Splitter1.Size = New System.Drawing.Size(456, 3)
      Me.Splitter1.TabIndex = 12
      Me.Splitter1.TabStop = False
      '
      'Splitter2
      '
      Me.Splitter2.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter2.Location = New System.Drawing.Point(0, 67)
      Me.Splitter2.Name = "Splitter2"
      Me.Splitter2.Size = New System.Drawing.Size(456, 3)
      Me.Splitter2.TabIndex = 14
      Me.Splitter2.TabStop = False
      '
      'pnlBottom
      '
      Me.pnlBottom.Controls.Add(Me.pnlEdit)
      Me.pnlBottom.Controls.Add(Me.pnlButtons)
      Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pnlBottom.Location = New System.Drawing.Point(0, 70)
      Me.pnlBottom.Name = "pnlBottom"
      Me.pnlBottom.Size = New System.Drawing.Size(456, 224)
      Me.pnlBottom.TabIndex = 15
      '
      'pnlEdit
      '
      Me.pnlEdit.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pnlEdit.Location = New System.Drawing.Point(0, 0)
      Me.pnlEdit.Name = "pnlEdit"
      Me.pnlEdit.Size = New System.Drawing.Size(376, 224)
      Me.pnlEdit.TabIndex = 13
      '
      'pnlButtons
      '
      Me.pnlButtons.Controls.Add(Me.btnClose)
      Me.pnlButtons.Controls.Add(Me.btnDelete)
      Me.pnlButtons.Controls.Add(Me.btnNew)
      Me.pnlButtons.Controls.Add(Me.btnCancel)
      Me.pnlButtons.Controls.Add(Me.btnSave)
      Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right
      Me.pnlButtons.Location = New System.Drawing.Point(376, 0)
      Me.pnlButtons.Name = "pnlButtons"
      Me.pnlButtons.Size = New System.Drawing.Size(80, 224)
      Me.pnlButtons.TabIndex = 14
      '
      'btnClose
      '
      Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnClose.Location = New System.Drawing.Point(0, 96)
      Me.btnClose.Name = "btnClose"
      Me.btnClose.TabIndex = 17
      Me.btnClose.Text = "Close"
      '
      'btnDelete
      '
      Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnDelete.Location = New System.Drawing.Point(0, 48)
      Me.btnDelete.Name = "btnDelete"
      Me.btnDelete.TabIndex = 16
      Me.btnDelete.Text = "&Delete"
      '
      'btnNew
      '
      Me.btnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnNew.Location = New System.Drawing.Point(0, 24)
      Me.btnNew.Name = "btnNew"
      Me.btnNew.TabIndex = 15
      Me.btnNew.Text = "&New"
      '
      'btnCancel
      '
      Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnCancel.Location = New System.Drawing.Point(0, 72)
      Me.btnCancel.Name = "btnCancel"
      Me.btnCancel.TabIndex = 14
      Me.btnCancel.Text = "&Cancel"
      '
      'btnSave
      '
      Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnSave.Location = New System.Drawing.Point(0, 0)
      Me.btnSave.Name = "btnSave"
      Me.btnSave.TabIndex = 13
      Me.btnSave.Text = "&Save"
      '
      'ToolBar
      '
      Me.ToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
      Me.ToolBar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbbSearch, Me.tbbGrid, Me.tbbSave, Me.tbbCancel, Me.tbbDelete, Me.tbbNew})
      Me.ToolBar.DropDownArrows = True
      Me.ToolBar.ImageList = Me.ImageList
      Me.ToolBar.Location = New System.Drawing.Point(0, 0)
      Me.ToolBar.Name = "ToolBar"
      Me.ToolBar.ShowToolTips = True
      Me.ToolBar.Size = New System.Drawing.Size(456, 28)
      Me.ToolBar.TabIndex = 16
      Me.ToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
      '
      'tbbSearch
      '
      Me.tbbSearch.ImageIndex = 2
      Me.tbbSearch.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
      Me.tbbSearch.Text = "Show Search"
      Me.tbbSearch.Visible = False
      '
      'tbbGrid
      '
      Me.tbbGrid.ImageIndex = 3
      Me.tbbGrid.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
      Me.tbbGrid.Text = "Show Grid"
      Me.tbbGrid.Visible = False
      '
      'tbbSave
      '
      Me.tbbSave.ImageIndex = 1
      Me.tbbSave.Text = "Save"
      '
      'tbbCancel
      '
      Me.tbbCancel.ImageIndex = 0
      Me.tbbCancel.Text = "Cancel"
      '
      'tbbDelete
      '
      Me.tbbDelete.ImageIndex = 4
      Me.tbbDelete.Text = "Delete"
      '
      'tbbNew
      '
      Me.tbbNew.ImageIndex = 5
      Me.tbbNew.Text = "New"
      '
      'ImageList
      '
      Me.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
      Me.ImageList.ImageSize = New System.Drawing.Size(16, 16)
      Me.ImageList.ImageStream = CType(resources.GetObject("ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
      Me.ImageList.TransparentColor = System.Drawing.Color.Silver
      '
      'RootEditForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(456, 294)
      Me.Controls.Add(Me.pnlBottom)
      Me.Controls.Add(Me.Splitter2)
      Me.Controls.Add(Me.Splitter1)
      Me.Controls.Add(Me.pnlSelect)
      Me.Controls.Add(Me.ToolBar)
      Me.Name = "RootEditForm"
      Me.Text = "EditForm"
      Me.pnlSelect.ResumeLayout(False)
      Me.pnlBottom.ResumeLayout(False)
      Me.pnlButtons.ResumeLayout(False)
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region " Public Properties and Methods "

#Region "Show and ShowDialog Overloads"

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessObjectType As System.Type, _
            ByVal parent As Windows.forms.Form)
      Show(editUserControl, selectUserControl, businessObjectType, _
               parent, True)
   End Sub

   Public Overloads Sub ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessobjecttype As System.Type, _
            ByVal parent As Windows.forms.Form)
      ShowDialog(editUserControl, selectUserControl, businessobjecttype, _
               parent, True)
   End Sub

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessObjectType As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String)
      Show(editUserControl, selectUserControl, businessObjectType, _
               parent, title, True)
   End Sub

   Public Overloads Sub ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessobjecttype As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String)
      ShowDialog(editUserControl, selectUserControl, businessobjecttype, _
               parent, title, True)
   End Sub

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessObjectType As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal showToolbar As Boolean)
      Show(editUserControl, selectUserControl, _
                  businessObjectType, parent, _
                  "", showToolbar)
   End Sub

   Public Overloads Sub ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessobjecttype As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal showToolbar As Boolean)
      ShowDialog(editUserControl, selectUserControl, _
                   businessobjecttype, parent, _
                   "", showToolbar)
   End Sub

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessObjectType As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal showToolbar As Boolean)
      Me.Show(editUserControl, selectUserControl, _
                  businessObjectType, parent, title, showToolbar, 0, 0)
   End Sub

   Public Overloads Sub ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessobjecttype As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal showToolbar As Boolean)
      Me.ShowDialog(editUserControl, selectUserControl, _
                  businessobjecttype, parent, title, showToolbar, 0, 0)
   End Sub

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessObjectType As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal showToolbar As Boolean, _
            ByVal openingWidth As Int32)
      Me.Show(editUserControl, selectUserControl, _
                  businessObjectType, parent, title, showToolbar, openingWidth, 0)
   End Sub

   Public Overloads Sub ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessobjecttype As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal showToolbar As Boolean, _
            ByVal openingWidth As Int32)
      Me.ShowDialog(editUserControl, selectUserControl, _
                  businessobjecttype, parent, title, showToolbar, openingWidth, 0)
   End Sub

   Public Overloads Sub Show( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessObjectType As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal showToolbar As Boolean, _
            ByVal openingWidth As Int32, _
            ByVal openingHeight As Int32)
      Me.SetupForm(editUserControl, selectUserControl, _
                  businessObjectType, parent, title, showToolbar, _
                  openingWidth, openingHeight)
      MyBase.Show()
   End Sub

   Public Overloads Sub ShowDialog( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessobjecttype As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal showToolbar As Boolean, _
            ByVal openingWidth As Int32, _
            ByVal openingHeight As Int32)
      Me.SetupForm(editUserControl, selectUserControl, _
                  businessobjecttype, parent, title, showToolbar, _
                  openingWidth, openingHeight)
      MyBase.ShowDialog(parent)
   End Sub

#End Region


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

   Private Sub btnClose_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles btnClose.Click
      OnBtnCloseClick(sender, e)
   End Sub

   Private Sub btnSave_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles btnSave.Click
      OnBtnSaveClick(sender, e)
   End Sub

   Private Sub btnNew_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles btnNew.Click
      OnBtnNewClick(sender, e)
   End Sub

   Private Sub btnDelete_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles btnDelete.Click
      OnBtnDeleteClick(sender, e)
   End Sub

   Private Sub ToolBar_ButtonClick( _
            ByVal sender As System.Object, _
            ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) _
            Handles ToolBar.ButtonClick
      onToolBarButtonClick(sender, e)
   End Sub

   Private Sub selectUC_SelectionMade( _
               ByVal sender As Object, _
               ByVal e As SelectionMadeEventArgs) _
               Handles mSelectUserControl.SelectionMade
      OnUCSelectionMade(sender, e)
   End Sub

   Private Sub cboSelect_SelectedValueChanged( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs) _
            Handles cboSelect.SelectedValueChanged
      OnCboSelectionmade(sender, e)
   End Sub

   Private Sub uc_DataChanged( _
            ByVal sender As Object, _
            ByVal e As System.EventArgs) _
            Handles mEditUserControl.DataChanged
      Diagnostics.Debug.WriteLine("Here we are again")
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

      With CType(Me.mEditUserControl, IEditUserControl)
         Me.cboSelect.Left = .ControlLeft
         'Me.cboSelect.Left = .LabelWidth + .HorizontalMargin
         Me.cboSelect.Width = .ControlWidth
         btnSave.Top = .ControlTop
         btnNew.Top = btnSave.Bottom
         btnDelete.Top = btnNew.Bottom
         btnCancel.Top = btnDelete.Bottom
         btnClose.Top = btnCancel.Bottom
      End With

      For Each ctl As Windows.Forms.Control In Me.Controls
         If TypeOf ctl Is IEditUserControl Then
            CType(ctl, IEditUserControl).SetupControl(mObject)
         End If
      Next

      ResetDatasource(Nothing)
      Me.tbbSearch.Pushed = True
      Me.tbbGrid.Pushed = False
      Me.pnlSelect.Visible = tbbSearch.Pushed

      Me.pnlSelect.Dock = DockStyle.Top
      Me.pnlBottom.Dock = DockStyle.Fill
      Me.pnlButtons.Dock = DockStyle.Right
      Me.pnlEdit.Dock = DockStyle.Fill
      Me.mEditUserControl.Dock = DockStyle.Fill
      Me.cboSelect.Width = CType(Me.mEditUserControl, IEditUserControl).ControlWidth
      Me.cboSelect.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

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
      Me.ClientSize = New Drawing.Size(width, pnlBottom.Top + height)


      mbIsLoaded = True
      If Me.cboSelect.Items.Count > 0 Then
         Me.cboSelect.SelectedIndex = -1
         Me.cboSelect.SelectedIndex = 0
      Else
         ' added 12/13/03 to avoid problems with new forms. 
         'Me.OnBtnNewClick(Me, e)
      End If

   End Sub

   'Protected Overrides Sub OnLayout(ByVal levent As System.Windows.Forms.LayoutEventArgs)
   'pnlBottom.Width = Me.ClientSize.Width
   'ResizeForm(ToolBar, Me.mSelectUserControl, _
   '            Me.mEditUserControl, Me.pnlBottom, Me.pnlButtons, _
   '            Me.pnlSelect)
   ''pnlBottom.Height = Me.mEditUserControl.Height
   'End Sub

   'Protected Overrides Sub OnClosing( _
   '                  ByVal e As System.ComponentModel.CancelEventArgs)
   '   If Not mObject Is Nothing Then
   '      mObject.CancelEdit()
   '   End If
   'End Sub

   Protected Overridable Sub OnBtnNewClick( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs)
      'If Not mObject Is Nothing Then
      '   Me.BindingContext(mObject).EndCurrentEdit()
      'End If
      Dim bo As IBusinessObject
      Try
         Cursor.Current = Cursors.WaitCursor
         ' KD - The second time you added a new record wasn't clearing the combobox until there were 
         '     two calls to set the SelectIndex
         cboSelect.SelectedIndex = -1
         cboSelect.SelectedIndex = -1
         bo = CType(Utility.InvokeSharedMethod( _
                    Me.mBusinessObjectType, _
                    "New" & Me.mBusinessObjectType.Name), _
                 IBusinessObject)
         mObject = bo
         With CType(Me.mEditUserControl, IEditUserControl)
            '.BusinessObject = bo
            .SetupControl(bo)
         End With
         Cursor.Current = Cursors.Default
      Catch ex As Exception
         Cursor.Current = Cursors.Default
         MessageBox.Show(ex.ToString)
      End Try
      SetState()
      Me.mEditUserControl.Focus()
   End Sub

   Protected Overridable Sub OnBtnDeleteClick( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs)
      If MessageBox.Show( _
               "Do you really want to delete " & Me.cboSelect.Text & "?", _
               "Please confirm deletion", _
               MessageBoxButtons.YesNo) = DialogResult.Yes Then
         CType(mEditUserControl, IEditUserControl).Delete()
         ResetDatasource(Nothing)
      End If
      SetState()
   End Sub

   Protected Overridable Sub OnBtnSaveClick( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs)
      Me.BindingContext(mObject).EndCurrentEdit()
      If btnSave.Text = "Save" Then
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
               ResetDatasource(mObject)
               Me.mEditUserControl.Height = pnlEdit.Height
               Cursor.Current = Cursors.Default
            End If
         Catch ex As Exception
            Cursor.Current = Cursors.Default
            Throw
         End Try
      End If
      SetState()
   End Sub

   Protected Overridable Sub OnBtnCancelClick( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs)
      With CType(Me.mEditUserControl, IEditUserControl)
         Dim isNew As Boolean = ((.EditMode And EditMode.IsNew) > 0)
         .CancelEdit()
         Me.mEditUserControl.VisitControls()
         If isNew And cboSelect.Items.Count = 0 Then
            'Me.OnBtnNewClick(Me, e)
            SetState()
         ElseIf isNew Then
            cboSelect.SelectedIndex = 0
         Else
            SetState()
         End If
      End With
   End Sub

   Protected Overridable Sub OnBtnCloseClick( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs)
      Me.Close()
   End Sub

   Protected Overridable Sub onToolBarButtonClick( _
               ByVal sender As System.Object, _
               ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs)
      If e.Button Is Me.tbbSearch Then
         If tbbGrid.Pushed Then
            tbbGrid.Pushed = False
         End If
         SetPanels()
      ElseIf e.Button Is Me.tbbGrid Then
         If tbbSearch.Pushed Then
            tbbSearch.Pushed = False
         End If
         SetPanels()
      ElseIf e.Button Is Me.tbbSave Then
         btnSave.PerformClick()
      ElseIf e.Button Is Me.tbbNew Then
         btnNew.PerformClick()
      ElseIf e.Button Is Me.tbbDelete Then
         btnDelete.PerformClick()
      ElseIf e.Button Is Me.tbbCancel Then
         btnCancel.PerformLayout()
      End If
      SetState()
   End Sub

   Protected Overridable Sub OnUCSelectionMade( _
               ByVal sender As System.Object, _
               ByVal e As SelectionMadeEventArgs)
      DisplayForEdits(e, e.primaryKey)
      SetState()
   End Sub

   Protected Overridable Sub OnCboSelectionmade( _
              ByVal sender As System.Object, _
               ByVal e As System.EventArgs)
      If mbIsLoaded Then
         Dim obj As Object
         obj = cboSelect.SelectedItem
         If Not obj Is Nothing Then
            obj = CType(obj, BusinessObjectSupport.IListInfo).GetPrimaryKey
            DisplayForEdits(e, obj)
         End If
      End If
      SetState()
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
#End Region

#Region "Private Properties and Methods"
   Protected Overridable Sub SetupForm( _
            ByVal editUserControl As WinSupport.BaseEditUserControl, _
            ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
            ByVal businessobjecttype As System.Type, _
            ByVal parent As Windows.forms.Form, _
            ByVal title As String, _
            ByVal showToolbar As Boolean, _
            ByVal openingWidth As Int32, _
            ByVal openingHeight As Int32)
      Dim showSelectUC As Boolean = False
      Dim iWidth As Int32
      Dim iHeight As Int32
      Me.mEditUserControl = editUserControl
      Me.mEditUserControl.BringToFront()
      Me.mSelectUserControl = selectUserControl
      Me.mBusinessObjectType = businessobjecttype
      'If Not selectUserControl Is Nothing Then
      '   Me.pnlGrid.Controls.Add(selectUserControl)
      '   selectUserControl.Dock = DockStyle.Fill
      'End If
      selectUserControl.Visible = False
      Me.pnlEdit.Controls.Add(editUserControl)
      'editUserControl.Dock = DockStyle.Fill
      If title Is Nothing OrElse title.Trim.Length = 0 Then
         Me.Text = selectUserControl.Caption
      Else
         Me.Text = title
      End If
      Me.cboSelect.ValueMember = "UniqueKey"
      Me.cboSelect.DisplayMember = "DisplayText"
      Me.MdiParent = parent
      Me.ToolBar.Visible = showToolbar
      'Me.pnlSelect.Dock = DockStyle.Top
      'Me.pnlBottom.Dock = DockStyle.Fill
      'Me.pnlButtons.Dock = DockStyle.Right
      'Me.pnlEdit.Dock = DockStyle.Fill
      'Me.mEditUserControl.Dock = DockStyle.Fill
      ''ResizeForm(ToolBar, Me.mSelectUserControl, _
      ''Me.mEditUserControl, Me.pnlBottom, Me.pnlButtons, _
      ''Me.pnlSelect)
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
      If iHeight < Me.MdiParent.ClientSize.Height Then
         iHeight = Me.MdiParent.ClientSize.Height
      End If
      Me.ClientSize = New Drawing.Size(iWidth, iHeight)
      SetState()
      'editUserControl.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom
   End Sub



   Private Sub SetState()
      With CType(Me.mEditUserControl, IEditUserControl)
         Dim editMode As EditMode = .EditMode
         Me.btnDelete.Enabled = (editMode = editMode.IsClean Or _
                                 editMode = editMode.IsDirty)
         Me.btnSave.Enabled = ((editMode And editMode.IsDirty) > 0)
         Me.btnNew.Enabled = (editMode = editMode.IsClean Or _
                                 editMode = editMode.IsEmpty)
         Me.btnCancel.Enabled = ((editMode And editMode.IsDirty) > 0) Or _
                                 ((editMode And editMode.IsNew) > 0)
         Me.tbbDelete.Enabled = Me.btnDelete.Enabled
         Me.tbbSave.Enabled = Me.btnSave.Enabled
         Me.tbbNew.Enabled = Me.btnNew.Enabled
         Me.tbbCancel.Enabled = Me.btnCancel.Enabled
      End With
   End Sub

   Private Sub ResetDatasource(ByVal ibo As IBusinessObject)
      Dim objKey As String
      If Not ibo Is Nothing Then
         objKey = ibo.UniqueKey
      End If
      Me.cboSelect.DataSource = mSelectUserControl.GetList
      If Not ibo Is Nothing Then
         Me.cboSelect.SelectedValue = objKey
      ElseIf Me.cboSelect.Items.Count > 0 Then
         Me.cboSelect.SelectedIndex = 0
      Else
         Me.cboSelect.SelectedIndex = -1
         'Me.OnBtnNewClick(Me, New System.EventArgs)
      End If
      ' For some reason, cboSelectedValueChanged, not fired here, so I am explicitly calling routine 12/4/03 kad
      Me.OnCboSelectionmade(Me.cboSelect, New System.EventArgs)
   End Sub

   Private Sub SetPanels()
      Me.pnlSelect.Visible = tbbSearch.Pushed
   End Sub

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

#End Region

 
End Class
