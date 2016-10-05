' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: A root class for the Child and Root edit forms
'  Refactor: Move additional code from child and root into this form.

Option Strict On
Option Explicit On 

Imports System
Imports System.Windows.Forms
Imports KADGen.BusinessObjectSupport



Public Class BaseEditForm
   Inherits System.Windows.Forms.Form

   Protected Enum FormMode
      Root
      Child
   End Enum

   'Protected mMargin As Int32 = 5
   Protected mBtnLast As Control
   Protected mObject As IBusinessObject
   Protected WithEvents mEditUserControl As BaseEditUserControl
   Protected mFormMode As FormMode

   Protected Sub ResizeForm( _
               ByVal toolBar As Windows.Forms.ToolBar, _
               ByVal ucSelect As BaseSelectUserControl, _
               ByVal ucEdit As BaseEditUserControl, _
               ByVal pnlBottom As Windows.Forms.Panel, _
               ByVal pnlButtons As Windows.Forms.Panel, _
               ByVal pnlSelect As Windows.forms.Panel)
      Dim height As Int32 = 0
      Dim top As Int32 = 0
      Dim width As Int32 = Me.ClientSize.Width
      Dim vMargin As Int32 = CType(ucEdit, IEditUserControl).VerticalMargin
      ucEdit.Width = Me.ClientSize.Width - pnlButtons.Width
      If Not toolBar Is Nothing AndAlso toolBar.Visible Then
         height += toolBar.Height + vMargin
         top += toolBar.Bottom + vMargin
      End If
      If Not ucSelect Is Nothing AndAlso ucSelect.Visible Then
         height += ucSelect.Height + vMargin
         ucSelect.Width = width
         ucSelect.Top = top
         top += ucSelect.Bottom + vMargin
      End If
      If Not pnlSelect Is Nothing AndAlso pnlSelect.Visible Then
         height += pnlSelect.Height + vMargin
         pnlSelect.Width = width
         pnlSelect.Top = top
         top += pnlSelect.Bottom + vMargin
      End If
      pnlBottom.Top = top
      pnlBottom.Width = width
      height = ucEdit.Height
      If height < mBtnLast.Bottom + vMargin Then
         height = mBtnLast.Bottom + vMargin
      Else
         height += 2 * vMargin
      End If
      pnlBottom.Height = height
      top += pnlBottom.Bottom + vMargin

      Me.ClientSize = New Drawing.Size(Me.ClientSize.Width, height)
   End Sub

   Protected Overrides Sub OnClosing( _
                     ByVal e As System.ComponentModel.CancelEventArgs)
      If Me.mFormMode = FormMode.Root Then
         CType(mEditUserControl, WinSupport.IEditUserControl).OnClosing(e)
      End If
      'If Not mObject Is Nothing Then
      '   mObject.CancelEdit()
      'End If
   End Sub

   'Protected Sub ResizeForm( _
   '            ByVal toolBar As Windows.Forms.ToolBar, _
   '            ByVal ucSelect As BaseSelectUserControl, _
   '            ByVal ucEdit As BaseEditUserControl, _
   '            ByVal pnlButtons As Windows.Forms.Panel, _
   '            ByVal pnlSelect As Windows.forms.Panel)
   '   Dim height As Int32
   '   Dim vMargin As Int32 = CType(ucEdit, IEditUserControl).VerticalMargin
   '   ucEdit.Width = Me.ClientSize.Width - pnlButtons.Width
   '   height = ucEdit.Height
   '   If height < mBtnLast.Bottom + vMargin Then
   '      height = mBtnLast.Bottom + vMargin
   '   Else
   '      height += 2 * vMargin
   '   End If
   '   If Not toolBar Is Nothing AndAlso toolBar.Visible Then
   '      height += toolBar.Height + vMargin
   '      pnlSelect.Top = toolBar.Bottom + vMargin
   '   Else
   '      pnlSelect.Top = 0
   '   End If
   '   If Not ucSelect Is Nothing AndAlso ucSelect.Visible Then
   '      height += ucSelect.Height + vMargin
   '   End If
   '   If Not pnlSelect Is Nothing AndAlso pnlSelect.Visible Then
   '      height += pnlSelect.Height + vMargin
   '      pnlSelect.Width = Me.ClientSize.Width
   '      pnlBottom.top = pnlSelect.Bottom
   '   End If
   '   Me.ClientSize = New Drawing.Size(Me.ClientSize.Width, height)
   'End Sub

   '#Region "Class Declarations"
   '   Protected mbIsLoaded As Boolean
   '   Protected mObject As IBusinessObject
   '   Protected mCallingForm As Windows.Forms.Form
   '   Protected mBusinessObjectType As System.Type
   '   Protected WithEvents mSelectUserControl As BaseSelectUserControl
   '   Protected WithEvents mEditUserControl As BaseEditUserControl
   '#End Region

   '#Region " Windows Form Designer generated code "

   '   Public Sub New()
   '      MyBase.New()

   '      'This call is required by the Windows Form Designer.
   '      InitializeComponent()

   '      'Add any initialization after the InitializeComponent() call

   '   End Sub

   '   'Form overrides dispose to clean up the component list.
   '   Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
   '      If disposing Then
   '         If Not (components Is Nothing) Then
   '            components.Dispose()
   '         End If
   '      End If
   '      MyBase.Dispose(disposing)
   '   End Sub

   '   'Required by the Windows Form Designer
   '   Private components As System.ComponentModel.IContainer

   '   'NOTE: The following procedure is required by the Windows Form Designer
   '   'It can be modified using the Windows Form Designer.  
   '   'Do not modify it using the code editor.
   '   Friend WithEvents pnlSearch As System.Windows.Forms.Panel
   '   Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
   '   Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
   '   Friend WithEvents ToolBar As System.Windows.Forms.ToolBar
   '   Friend WithEvents ImageList As System.Windows.Forms.ImageList
   '   Friend WithEvents tbbSearch As System.Windows.Forms.ToolBarButton
   '   Friend WithEvents tbbGrid As System.Windows.Forms.ToolBarButton
   '   Friend WithEvents tbbSave As System.Windows.Forms.ToolBarButton
   '   Friend WithEvents tbbCancel As System.Windows.Forms.ToolBarButton
   '   Friend WithEvents pnlButtons As System.Windows.Forms.Panel
   '   Friend WithEvents lblSearch As System.Windows.Forms.Label
   '   Friend WithEvents btnCancel As System.Windows.Forms.Button
   '   Friend WithEvents btnSave As System.Windows.Forms.Button
   '   Friend WithEvents pnlBottom As System.Windows.Forms.Panel
   '   Friend WithEvents pnlEdit As System.Windows.Forms.Panel
   '   Friend WithEvents cboSelect As System.Windows.Forms.ComboBox
   '   Friend WithEvents pnlGrid As System.Windows.Forms.Panel
   '   Friend WithEvents btnNew As System.Windows.Forms.Button
   '   Friend WithEvents btnDelete As System.Windows.Forms.Button
   '   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
   '      Me.components = New System.ComponentModel.Container
   '      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RootEditForm))
   '      Me.pnlSearch = New System.Windows.Forms.Panel
   '      Me.cboSelect = New System.Windows.Forms.ComboBox
   '      Me.lblSearch = New System.Windows.Forms.Label
   '      Me.Splitter1 = New System.Windows.Forms.Splitter
   '      Me.pnlGrid = New System.Windows.Forms.Panel
   '      Me.Splitter2 = New System.Windows.Forms.Splitter
   '      Me.pnlBottom = New System.Windows.Forms.Panel
   '      Me.pnlButtons = New System.Windows.Forms.Panel
   '      Me.btnDelete = New System.Windows.Forms.Button
   '      Me.btnNew = New System.Windows.Forms.Button
   '      Me.btnCancel = New System.Windows.Forms.Button
   '      Me.btnSave = New System.Windows.Forms.Button
   '      Me.pnlEdit = New System.Windows.Forms.Panel
   '      Me.ToolBar = New System.Windows.Forms.ToolBar
   '      Me.tbbSearch = New System.Windows.Forms.ToolBarButton
   '      Me.tbbGrid = New System.Windows.Forms.ToolBarButton
   '      Me.tbbSave = New System.Windows.Forms.ToolBarButton
   '      Me.tbbCancel = New System.Windows.Forms.ToolBarButton
   '      Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
   '      Me.pnlSearch.SuspendLayout()
   '      Me.pnlBottom.SuspendLayout()
   '      Me.pnlButtons.SuspendLayout()
   '      Me.SuspendLayout()
   '      '
   '      'pnlSearch
   '      '
   '      Me.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
   '      Me.pnlSearch.Controls.Add(Me.cboSelect)
   '      Me.pnlSearch.Controls.Add(Me.lblSearch)
   '      Me.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top
   '      Me.pnlSearch.Location = New System.Drawing.Point(0, 28)
   '      Me.pnlSearch.Name = "pnlSearch"
   '      Me.pnlSearch.Size = New System.Drawing.Size(840, 36)
   '      Me.pnlSearch.TabIndex = 11
   '      '
   '      'cboSelect
   '      '
   '      Me.cboSelect.Location = New System.Drawing.Point(112, 8)
   '      Me.cboSelect.Name = "cboSelect"
   '      Me.cboSelect.Size = New System.Drawing.Size(168, 21)
   '      Me.cboSelect.TabIndex = 1
   '      Me.cboSelect.Text = "ComboBox1"
   '      '
   '      'lblSearch
   '      '
   '      Me.lblSearch.Location = New System.Drawing.Point(8, 8)
   '      Me.lblSearch.Name = "lblSearch"
   '      Me.lblSearch.TabIndex = 0
   '      Me.lblSearch.Text = "Search"
   '      '
   '      'Splitter1
   '      '
   '      Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
   '      Me.Splitter1.Location = New System.Drawing.Point(0, 64)
   '      Me.Splitter1.Name = "Splitter1"
   '      Me.Splitter1.Size = New System.Drawing.Size(840, 3)
   '      Me.Splitter1.TabIndex = 12
   '      Me.Splitter1.TabStop = False
   '      '
   '      'pnlGrid
   '      '
   '      Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Top
   '      Me.pnlGrid.Location = New System.Drawing.Point(0, 67)
   '      Me.pnlGrid.Name = "pnlGrid"
   '      Me.pnlGrid.Size = New System.Drawing.Size(840, 85)
   '      Me.pnlGrid.TabIndex = 13
   '      '
   '      'Splitter2
   '      '
   '      Me.Splitter2.Dock = System.Windows.Forms.DockStyle.Top
   '      Me.Splitter2.Location = New System.Drawing.Point(0, 152)
   '      Me.Splitter2.Name = "Splitter2"
   '      Me.Splitter2.Size = New System.Drawing.Size(840, 3)
   '      Me.Splitter2.TabIndex = 14
   '      Me.Splitter2.TabStop = False
   '      '
   '      'pnlBottom
   '      '
   '      Me.pnlBottom.Controls.Add(Me.pnlButtons)
   '      Me.pnlBottom.Controls.Add(Me.pnlEdit)
   '      Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill
   '      Me.pnlBottom.Location = New System.Drawing.Point(0, 155)
   '      Me.pnlBottom.Name = "pnlBottom"
   '      Me.pnlBottom.Size = New System.Drawing.Size(840, 291)
   '      Me.pnlBottom.TabIndex = 15
   '      '
   '      'pnlButtons
   '      '
   '      Me.pnlButtons.Controls.Add(Me.btnDelete)
   '      Me.pnlButtons.Controls.Add(Me.btnNew)
   '      Me.pnlButtons.Controls.Add(Me.btnCancel)
   '      Me.pnlButtons.Controls.Add(Me.btnSave)
   '      Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right
   '      Me.pnlButtons.Location = New System.Drawing.Point(752, 0)
   '      Me.pnlButtons.Name = "pnlButtons"
   '      Me.pnlButtons.Size = New System.Drawing.Size(88, 291)
   '      Me.pnlButtons.TabIndex = 14
   '      '
   '      'btnDelete
   '      '
   '      Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
   '      Me.btnDelete.Location = New System.Drawing.Point(8, 72)
   '      Me.btnDelete.Name = "btnDelete"
   '      Me.btnDelete.TabIndex = 16
   '      Me.btnDelete.Text = "&Delete"
   '      '
   '      'btnNew
   '      '
   '      Me.btnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
   '      Me.btnNew.Location = New System.Drawing.Point(8, 40)
   '      Me.btnNew.Name = "btnNew"
   '      Me.btnNew.TabIndex = 15
   '      Me.btnNew.Text = "&New"
   '      '
   '      'btnCancel
   '      '
   '      Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
   '      Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
   '      Me.btnCancel.Location = New System.Drawing.Point(8, 104)
   '      Me.btnCancel.Name = "btnCancel"
   '      Me.btnCancel.TabIndex = 14
   '      Me.btnCancel.Text = "&Cancel"
   '      '
   '      'btnSave
   '      '
   '      Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
   '      Me.btnSave.Location = New System.Drawing.Point(8, 8)
   '      Me.btnSave.Name = "btnSave"
   '      Me.btnSave.TabIndex = 13
   '      Me.btnSave.Text = "&Save"
   '      '
   '      'pnlEdit
   '      '
   '      Me.pnlEdit.Dock = System.Windows.Forms.DockStyle.Fill
   '      Me.pnlEdit.Location = New System.Drawing.Point(0, 0)
   '      Me.pnlEdit.Name = "pnlEdit"
   '      Me.pnlEdit.Size = New System.Drawing.Size(840, 291)
   '      Me.pnlEdit.TabIndex = 13
   '      '
   '      'ToolBar
   '      '
   '      Me.ToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
   '      Me.ToolBar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbbSearch, Me.tbbGrid, Me.tbbSave, Me.tbbCancel})
   '      Me.ToolBar.DropDownArrows = True
   '      Me.ToolBar.ImageList = Me.ImageList
   '      Me.ToolBar.Location = New System.Drawing.Point(0, 0)
   '      Me.ToolBar.Name = "ToolBar"
   '      Me.ToolBar.ShowToolTips = True
   '      Me.ToolBar.Size = New System.Drawing.Size(840, 28)
   '      Me.ToolBar.TabIndex = 16
   '      Me.ToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
   '      '
   '      'tbbSearch
   '      '
   '      Me.tbbSearch.ImageIndex = 2
   '      Me.tbbSearch.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
   '      Me.tbbSearch.Text = "Show Search"
   '      '
   '      'tbbGrid
   '      '
   '      Me.tbbGrid.ImageIndex = 3
   '      Me.tbbGrid.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
   '      Me.tbbGrid.Text = "Show Grid"
   '      '
   '      'tbbSave
   '      '
   '      Me.tbbSave.ImageIndex = 1
   '      Me.tbbSave.Text = "Save"
   '      '
   '      'tbbCancel
   '      '
   '      Me.tbbCancel.ImageIndex = 0
   '      Me.tbbCancel.Text = "Cancel"
   '      '
   '      'ImageList
   '      '
   '      Me.ImageList.ImageSize = New System.Drawing.Size(16, 16)
   '      Me.ImageList.ImageStream = CType(resources.GetObject("ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
   '      Me.ImageList.TransparentColor = System.Drawing.Color.Transparent
   '      '
   '      'RootEditForm
   '      '
   '      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
   '      Me.ClientSize = New System.Drawing.Size(840, 446)
   '      Me.Controls.Add(Me.pnlBottom)
   '      Me.Controls.Add(Me.Splitter2)
   '      Me.Controls.Add(Me.pnlGrid)
   '      Me.Controls.Add(Me.Splitter1)
   '      Me.Controls.Add(Me.pnlSearch)
   '      Me.Controls.Add(Me.ToolBar)
   '      Me.Name = "RootEditForm"
   '      Me.Text = "EditForm"
   '      Me.pnlSearch.ResumeLayout(False)
   '      Me.pnlBottom.ResumeLayout(False)
   '      Me.pnlButtons.ResumeLayout(False)
   '      Me.ResumeLayout(False)

   '   End Sub

   '#End Region

   '#Region "Constructors"
   '#End Region

   '#Region " Public Properties and Methods "

   '   Public Property BusinessObject() As IBusinessObject
   '      Get
   '         Return mObject
   '      End Get
   '      Set(ByVal Value As IBusinessObject)
   '         mObject = Value
   '      End Set
   '   End Property

   '   Public Property CallingForm() As Windows.Forms.Form
   '      Get
   '         Return mCallingForm
   '      End Get
   '      Set(ByVal Value As Windows.Forms.Form)
   '         mCallingForm = Value
   '      End Set
   '   End Property

   '#End Region

   '#Region " Event Handlers "
   '   Private Sub btnCancel_Click( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs) _
   '               Handles btnCancel.Click
   '      OnBtnCancelClick(sender, e)
   '   End Sub

   '   Private Sub btnSave_Click( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs) _
   '               Handles btnSave.Click
   '      OnBtnSaveClick(sender, e)
   '   End Sub

   '   Private Sub btnNew_Click( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs) _
   '               Handles btnNew.Click
   '      OnBtnNewClick(sender, e)
   '   End Sub

   '   Private Sub btnDelete_Click( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs) _
   '               Handles btnDelete.Click
   '      OnBtnDeleteClick(sender, e)
   '   End Sub

   '   Private Sub ToolBar_ButtonClick( _
   '            ByVal sender As System.Object, _
   '            ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) _
   '            Handles ToolBar.ButtonClick
   '      onToolBarButtonClick(sender, e)
   '   End Sub

   '   Private Sub selectUC_SelectionMade( _
   '               ByVal sender As Object, _
   '               ByVal e As SelectionMadeEventArgs) _
   '               Handles mSelectUserControl.SelectionMade
   '      OnUCSelectionMade(sender, e)
   '   End Sub

   '   Private Sub cboSelect_SelectedValueChanged( _
   '            ByVal sender As Object, _
   '            ByVal e As System.EventArgs) _
   '            Handles cboSelect.SelectedValueChanged
   '      OnCboSelectionmade(sender, e)
   '   End Sub

   '#End Region

   '#Region "Protected Event Response"
   '   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
   '      MyBase.OnLoad(e)
   '      If Me.CallingForm Is Nothing Then
   '         btnSave.Text = "Save"
   '      ElseIf TypeOf Me.CallingForm Is RootEditForm Then
   '         btnSave.Text = "OK"
   '      Else
   '         btnSave.Text = "Save"
   '      End If

   '      For Each ctl As Windows.Forms.Control In Me.Controls
   '         If TypeOf ctl Is IEditUserControl Then
   '            CType(ctl, IEditUserControl).SetupControl(e)
   '         End If
   '      Next
   '      mbIsLoaded = True

   '      ResetDatasource(Nothing)
   '      Me.tbbSearch.Pushed = True
   '      Me.tbbGrid.Pushed = False
   '      Me.pnlSearch.Visible = tbbSearch.Pushed
   '      Me.pnlGrid.Visible = tbbGrid.Pushed

   '   End Sub

   '   Protected Overrides Sub OnClosing( _
   '                     ByVal e As System.ComponentModel.CancelEventArgs)
   '      If Not mObject Is Nothing Then
   '         mObject.CancelEdit()
   '      End If
   '   End Sub

   '   Protected Overridable Sub OnBtnNewClick( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs)
   '      If Not mObject Is Nothing Then
   '         Me.BindingContext(mObject).EndCurrentEdit()
   '      End If
   '      Dim bo As IBusinessObject
   '      Try
   '         Cursor.Current = Cursors.WaitCursor
   '         cboSelect.SelectedIndex = -1
   '         bo = CType(Utility.InvokeSharedMethod( _
   '                    Me.mBusinessObjectType, _
   '                    "New" & Me.mBusinessObjectType.Name), _
   '                 IBusinessObject)
   '         mObject = bo
   '         With CType(Me.mEditUserControl, IEditUserControl)
   '            .BusinessObject = bo
   '            .SetupControl(e)
   '         End With
   '         Cursor.Current = Cursors.Default
   '      Catch ex As Exception
   '         Cursor.Current = Cursors.Default
   '         MessageBox.Show(ex.ToString)
   '      End Try
   '   End Sub

   '   Protected Overridable Sub OnBtnDeleteClick( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs)
   '      If MessageBox.Show( _
   '               "Do you really want to delete " & Me.cboSelect.Text & "?", _
   '               "Please confirm deletion", _
   '               MessageBoxButtons.YesNo) = DialogResult.Yes Then
   '         CType(mEditUserControl, IEditUserControl).Delete()
   '         ResetDatasource(Nothing)
   '      End If

   '   End Sub

   '   Protected Overridable Sub OnBtnSaveClick( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs)
   '      Me.BindingContext(mObject).EndCurrentEdit()
   '      If btnSave.Text = "Save" Then
   '         Try
   '            Cursor.Current = Cursors.WaitCursor
   '            mObject = CType(mEditUserControl, IEditUserControl).Save()
   '            ResetDatasource(mObject)
   '            Cursor.Current = Cursors.Default
   '         Catch ex As Exception
   '            Cursor.Current = Cursors.Default
   '            MessageBox.Show(ex.ToString)
   '         End Try
   '      End If
   '   End Sub

   '   Protected Overridable Sub OnBtnCancelClick( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs)
   '      mObject.CancelEdit()
   '   End Sub

   '   Protected Overridable Sub onToolBarButtonClick( _
   '               ByVal sender As System.Object, _
   '               ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs)
   '      If e.Button Is Me.tbbSearch Then
   '         If tbbGrid.Pushed Then
   '            tbbGrid.Pushed = False
   '         End If
   '         SetPanels()
   '      ElseIf e.Button Is Me.tbbGrid Then
   '         If tbbSearch.Pushed Then
   '            tbbSearch.Pushed = False
   '         End If
   '         SetPanels()
   '      ElseIf e.Button Is Me.tbbSave Then
   '         btnSave.PerformClick()
   '      ElseIf e.Button Is Me.tbbCancel Then
   '         btnCancel.PerformLayout()
   '      End If
   '   End Sub

   '   Protected Overridable Sub OnUCSelectionMade( _
   '               ByVal sender As System.Object, _
   '               ByVal e As SelectionMadeEventArgs)
   '      DisplayForEdits(e, e.primaryKey)

   '   End Sub

   '   Protected Overridable Sub OnCboSelectionmade( _
   '              ByVal sender As System.Object, _
   '               ByVal e As System.EventArgs)
   '      If mbIsLoaded Then
   '         Dim obj As Object
   '         obj = cboSelect.SelectedItem
   '         If Not obj Is Nothing Then
   '            obj = CType(obj, BusinessObjectSupport.IListInfo).GetPrimaryKey
   '            DisplayForEdits(e, obj)
   '         End If
   '      End If
   '   End Sub
   '#End Region

   '#Region "Protected Properties and Methods"
   '   Protected Overridable Sub DataBindButtons()
   '      If CType(mEditUserControl, IEditUserControl).CanCreate() Then
   '         Utility.BindField(btnSave, "Enabled", mObject, "IsValid")
   '      Else
   '         btnSave.Enabled = False
   '      End If
   '   End Sub

   '   Private Sub ResetDatasource(ByVal ibo As IBusinessObject)
   '      Dim objKey As String
   '      If Not ibo Is Nothing Then
   '         objKey = ibo.UniqueKey
   '      End If
   '      Me.cboSelect.DataSource = mSelectUserControl.GetList
   '      If Not ibo Is Nothing Then
   '         Me.cboSelect.SelectedValue = objKey
   '      ElseIf Me.cboSelect.Items.Count > 0 Then
   '         Me.cboSelect.SelectedIndex = 0
   '      Else
   '         Me.cboSelect.SelectedIndex = -1
   '      End If
   '      ' For some reason, cboSelectedValueChanged, not fired here, so I am explicitly calling routine 12/4/03 kad
   '      Me.OnCboSelectionmade(Me.cboSelect, New System.EventArgs)
   '   End Sub

   '   Private Sub SetPanels()
   '      Me.pnlSearch.Visible = tbbSearch.Pushed
   '      Me.pnlGrid.Visible = tbbGrid.Pushed
   '   End Sub

   '   Private Sub DisplayForEdits(ByVal e As System.EventArgs, ByVal pk As Object)
   '      Dim bo As IBusinessObject
   '      Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
   '      bo = CType(Utility.InvokeSharedMethod( _
   '                  Me.mBusinessObjectType, _
   '                  "Get" & Me.mBusinessObjectType.Name, _
   '                  pk), _
   '               IBusinessObject)
   '      mObject = bo
   '      With CType(Me.mEditUserControl, IEditUserControl)
   '         .BusinessObject = CType(bo, BusinessObjectSupport.IBusinessObject)
   '         .SetupControl(e)
   '      End With
   '      Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
   '   End Sub
   '#End Region

   '#Region "Private Properties and Methods"
   '   'Private Sub SelectFromShow( _
   '   '         ByVal editUserControl As WinSupport.BaseEditUserControl, _
   '   '         ByVal selectUserControl As WinSupport.BaseSelectUserControl, _
   '   '         ByVal businessobjecttype As System.Type, _
   '   '         ByVal parent As Windows.forms.Form, _
   '   '         ByVal title As String, _
   '   '         ByVal showToolbar As Boolean)
   '   '   If Not selectUserControl Is Nothing Then
   '   '      Me.pnlGrid.Controls.Add(selectUserControl)
   '   '      selectUserControl.Dock = DockStyle.Fill
   '   '   End If
   '   '   Me.pnlEdit.Controls.Add(editUserControl)
   '   '   Me.ToolBar.Visible = showToolbar
   '   '   Me.ClientSize = New System.Drawing.Size( _
   '   '                  editUserControl.Width + pnlButtons.Width, _
   '   '                  editUserControl.Height + pnlBottom.Top)
   '   '   editUserControl.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom
   '   '   'editUserControl.Dock = DockStyle.Fill
   '   '   Me.mEditUserControl = editUserControl
   '   '   Me.mSelectUserControl = selectUserControl
   '   '   Me.mBusinessObjectType = businessobjecttype
   '   '   Me.Text = title
   '   '   Me.cboSelect.ValueMember = "UniqueKey"
   '   '   Me.cboSelect.DisplayMember = "DisplayText"
   '   '   Me.MdiParent = parent
   '   'End Sub

   '   'Private Sub ResetDatasource(ByVal ibo As IBusinessObject)
   '   '   Dim objKey As String
   '   '   If Not ibo Is Nothing Then
   '   '      objKey = ibo.UniqueKey
   '   '   End If
   '   '   Me.cboSelect.DataSource = mSelectUserControl.GetList
   '   '   If Not ibo Is Nothing Then
   '   '      Me.cboSelect.SelectedValue = objKey
   '   '   ElseIf Me.cboSelect.Items.Count > 0 Then
   '   '      Me.cboSelect.SelectedIndex = 0
   '   '   Else
   '   '      Me.cboSelect.SelectedIndex = -1
   '   '   End If
   '   '   ' For some reason, cboSelectedValueChanged, not fired here, so I am explicitly calling routine 12/4/03 kad
   '   '   Me.OnCboSelectionmade(Me.cboSelect, New System.EventArgs)
   '   'End Sub

   '   'Private Sub SetPanels()
   '   '   Me.pnlSearch.Visible = tbbSearch.Pushed
   '   '   Me.pnlGrid.Visible = tbbGrid.Pushed
   '   'End Sub

   '   'Private Sub DisplayForEdits(ByVal e As System.EventArgs, ByVal pk As Object)
   '   '   Dim bo As IBusinessObject
   '   '   Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
   '   '   bo = CType(Utility.InvokeSharedMethod( _
   '   '               Me.mBusinessObjectType, _
   '   '               "Get" & Me.mBusinessObjectType.Name, _
   '   '               pk), _
   '   '            IBusinessObject)
   '   '   mObject = bo
   '   '   With CType(Me.mEditUserControl, IEditUserControl)
   '   '      .BusinessObject = CType(bo, BusinessObjectSupport.IBusinessObject)
   '   '      .SetupControl(e)
   '   '   End With
   '   '   Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
   '   'End Sub

   '#End Region



End Class
