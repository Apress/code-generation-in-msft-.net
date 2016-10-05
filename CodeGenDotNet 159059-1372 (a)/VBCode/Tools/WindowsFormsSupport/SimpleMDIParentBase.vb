' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Base class for MDI parent forms

Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class SimpleMDIParentBase
   Inherits System.Windows.Forms.Form
   Implements ISimpleMDIParent

   Protected mFilter As String

#Region "Class level declarations - empty"
   Const vbcrlf As String = Microsoft.VisualBasic.ControlChars.CrLf
#End Region

#Region " Windows Form Designer generated code"

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization inside the InitForm method
      InitForm()

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
   Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
   Friend WithEvents mnuFileOpen As System.Windows.Forms.MenuItem
   Friend WithEvents mnuFileNew As System.Windows.Forms.MenuItem
   Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
   Friend WithEvents mnuFileExit As System.Windows.Forms.MenuItem
   Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
   Friend WithEvents mnuFileClose As System.Windows.Forms.MenuItem
   Friend WithEvents mnuFileSaveAs As System.Windows.Forms.MenuItem
   Friend WithEvents toolBar As System.Windows.Forms.ToolBar
   Friend WithEvents imageList As System.Windows.Forms.ImageList
   Friend WithEvents mnuTools As System.Windows.Forms.MenuItem
   Friend WithEvents mnuToolsOptions As System.Windows.Forms.MenuItem
   Friend WithEvents MainMenu As System.Windows.Forms.MainMenu
   Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
   Friend WithEvents SaveFileDialog As System.Windows.Forms.SaveFileDialog
   Friend WithEvents mnuWindow As System.Windows.Forms.MenuItem
   Friend WithEvents mnuCascade As System.Windows.Forms.MenuItem
   Friend WithEvents mnuTileHorizontal As System.Windows.Forms.MenuItem
   Friend WithEvents mnuTileVertical As System.Windows.Forms.MenuItem
   Friend WithEvents mnuReload As System.Windows.Forms.MenuItem
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Me.MainMenu = New System.Windows.Forms.MainMenu
      Me.mnuFile = New System.Windows.Forms.MenuItem
      Me.mnuFileNew = New System.Windows.Forms.MenuItem
      Me.mnuFileOpen = New System.Windows.Forms.MenuItem
      Me.mnuFileClose = New System.Windows.Forms.MenuItem
      Me.mnuReload = New System.Windows.Forms.MenuItem
      Me.mnuFileSave = New System.Windows.Forms.MenuItem
      Me.mnuFileSaveAs = New System.Windows.Forms.MenuItem
      Me.MenuItem5 = New System.Windows.Forms.MenuItem
      Me.mnuFileExit = New System.Windows.Forms.MenuItem
      Me.mnuTools = New System.Windows.Forms.MenuItem
      Me.mnuToolsOptions = New System.Windows.Forms.MenuItem
      Me.mnuWindow = New System.Windows.Forms.MenuItem
      Me.mnuTileHorizontal = New System.Windows.Forms.MenuItem
      Me.mnuTileVertical = New System.Windows.Forms.MenuItem
      Me.mnuCascade = New System.Windows.Forms.MenuItem
      Me.toolBar = New System.Windows.Forms.ToolBar
      Me.imageList = New System.Windows.Forms.ImageList(Me.components)
      Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
      Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog
      Me.SuspendLayout()
      '
      'MainMenu
      '
      Me.MainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuTools, Me.mnuWindow})
      '
      'mnuFile
      '
      Me.mnuFile.Index = 0
      Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileNew, Me.mnuFileOpen, Me.mnuFileClose, Me.mnuReload, Me.mnuFileSave, Me.mnuFileSaveAs, Me.MenuItem5, Me.mnuFileExit})
      Me.mnuFile.Text = "File"
      '
      'mnuFileNew
      '
      Me.mnuFileNew.Index = 0
      Me.mnuFileNew.Text = "New"
      '
      'mnuFileOpen
      '
      Me.mnuFileOpen.Index = 1
      Me.mnuFileOpen.Text = "Open"
      '
      'mnuFileClose
      '
      Me.mnuFileClose.Index = 2
      Me.mnuFileClose.Text = "Close"
      '
      'mnuReload
      '
      Me.mnuReload.Index = 3
      Me.mnuReload.Text = "Reload"
      '
      'mnuFileSave
      '
      Me.mnuFileSave.Index = 4
      Me.mnuFileSave.Text = "Save"
      '
      'mnuFileSaveAs
      '
      Me.mnuFileSaveAs.Index = 5
      Me.mnuFileSaveAs.Text = "Save As"
      '
      'MenuItem5
      '
      Me.MenuItem5.Index = 6
      Me.MenuItem5.Text = "-"
      '
      'mnuFileExit
      '
      Me.mnuFileExit.Index = 7
      Me.mnuFileExit.Text = "Exit"
      '
      'mnuTools
      '
      Me.mnuTools.Index = 1
      Me.mnuTools.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuToolsOptions})
      Me.mnuTools.Text = "Tools"
      Me.mnuTools.Visible = False
      '
      'mnuToolsOptions
      '
      Me.mnuToolsOptions.Index = 0
      Me.mnuToolsOptions.MdiList = True
      Me.mnuToolsOptions.Text = "Options"
      '
      'mnuWindow
      '
      Me.mnuWindow.Index = 2
      Me.mnuWindow.MdiList = True
      Me.mnuWindow.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuTileHorizontal, Me.mnuTileVertical, Me.mnuCascade})
      Me.mnuWindow.Text = "Window"
      '
      'mnuTileHorizontal
      '
      Me.mnuTileHorizontal.Index = 0
      Me.mnuTileHorizontal.Text = "TIle Horizontal"
      '
      'mnuTileVertical
      '
      Me.mnuTileVertical.Index = 1
      Me.mnuTileVertical.Text = "Tile Vertical"
      '
      'mnuCascade
      '
      Me.mnuCascade.Index = 2
      Me.mnuCascade.Text = "Cascade"
      '
      'toolBar
      '
      Me.toolBar.DropDownArrows = True
      Me.toolBar.Location = New System.Drawing.Point(0, 0)
      Me.toolBar.Name = "toolBar"
      Me.toolBar.ShowToolTips = True
      Me.toolBar.Size = New System.Drawing.Size(656, 42)
      Me.toolBar.TabIndex = 1
      Me.toolBar.Visible = False
      '
      'imageList
      '
      Me.imageList.ImageSize = New System.Drawing.Size(16, 16)
      Me.imageList.TransparentColor = System.Drawing.Color.Transparent
      '
      'SimpleMDIParentBase
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(656, 521)
      Me.Controls.Add(Me.toolBar)
      Me.IsMdiContainer = True
      Me.Menu = Me.MainMenu
      Me.Name = "SimpleMDIParentBase"
      Me.Text = "SimpleMDIParentBase"
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region "Event Handlers"
   Protected Overrides Sub OnMdiChildActivate(ByVal e As System.EventArgs)
      EnableSave(False)
      EnableClose(False)
   End Sub

   Private Sub mnuFileNew_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuFileNew.Click
      NewFile()
   End Sub

   Private Sub mnuFileOpen_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuFileOpen.Click
      OpenFile()
   End Sub

   Private Sub mnuFileClose_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuFileClose.Click
      CloseFile()
   End Sub

   Private Sub mnuReload_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuReload.Click
      ReloadFile()
   End Sub

   Private Sub mnuFileSave_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuFileSave.Click
      SaveFile()
   End Sub

   Private Sub mnuFileSaveAs_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuFileSaveAs.Click
      SaveFileAs()
   End Sub

   Private Sub mnuFileExit_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuFileExit.Click
      Me.Close()
   End Sub


   Private Sub mnuOptions_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuToolsOptions.Click
      Dim frmOptions As New Options
      frmOptions.ShowDialog()
   End Sub

   Private Sub mnuTileHorizontal_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuTileHorizontal.Click
      Me.LayoutMdi(Windows.Forms.MdiLayout.TileHorizontal)
   End Sub


   Private Sub mnuTileVertical_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuTileVertical.Click
      Me.LayoutMdi(Windows.Forms.MdiLayout.TileVertical)
   End Sub


   Private Sub mnuCascade_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles mnuCascade.Click
      Me.LayoutMdi(Windows.Forms.MdiLayout.Cascade)
   End Sub

#End Region

#Region "Public Methods and Properties"
   Public Sub EnableClose( _
                  ByVal enable As Boolean) Implements ISimpleMDIParent.EnableClose
      mnuFileClose.Enabled = enable
   End Sub

   Public Sub EnableSave( _
                  ByVal enable As Boolean) Implements ISimpleMDIParent.EnableSave
      mnuFileSave.Enabled = enable
      mnuFileSaveAs.Enabled = enable
   End Sub

   Public ReadOnly Property CurrentProcessNode() As Xml.XmlNode
      Get
         If Not Me.ActiveMdiChild Is Nothing Then
            If TypeOf Me.ActiveMdiChild Is SimpleTreeBase Then
               Return CType(Me.ActiveMdiChild, SimpleTreeBase).CurrentProcessNode
            End If
         End If
      End Get
   End Property

   Public ReadOnly Property CurrentProcessFile() As String
      Get
         If Not Me.ActiveMdiChild Is Nothing Then
            If TypeOf Me.ActiveMdiChild Is SimpleTreeBase Then
               Return CType(Me.ActiveMdiChild, SimpleTreeBase).CurrentProcessFile
            End If
         End If
      End Get
   End Property
#End Region

#Region "Protected and Friend Methods and Properties"
   Protected Overridable Function FormForFileType( _
                  ByVal fileExtension As String) _
                  As Windows.forms.Form
      ' If this isn't overridden, just intantiate the simple tree
      Dim frm As Windows.forms.Form = New SimpleTreeBase
      Return frm
   End Function

   Protected Function GetOpenFileDialog() _
                  As Windows.forms.OpenFileDialog
      Return Me.OpenFileDialog
   End Function

#End Region

#Region "Protected Event Response Methods"

   Protected Overridable Sub NewFile()
      ' FUTURE: Figure out an intelligent response if this is not overridden
   End Sub

   Protected Overridable Sub OpenFile()

      OpenFileDialog.Filter = mFilter
      Do While OpenFileDialog.ShowDialog = Windows.Forms.DialogResult.OK
         Try
            LoadFile(OpenFileDialog.FileName)
            Exit Do
         Catch ex As System.Exception
            Windows.Forms.MessageBox.Show("Failure opening this file" _
                        & vbcrlf & vbcrlf & ex.Message)
         End Try
      Loop
   End Sub

   Protected Overridable Sub ReloadFile()
      Dim frm As WinFormSupport.ISimpleForm
      Dim abort As Boolean
      Try
         If TypeOf Me.ActiveMdiChild Is WinFormSupport.ISimpleForm Then
            frm = CType(Me.ActiveMdiChild, WinFormSupport.ISimpleForm)
            If frm.HasChanges Then
               abort = Windows.Forms.MessageBox.Show( _
                              "Overwrite current changes in loaded file?", _
                              "Confirmation", _
                              Windows.Forms.MessageBoxButtons.YesNo) _
                           = Windows.Forms.DialogResult.No
            End If
            If Not abort Then
               LoadFile(frm.FileName)
               CType(frm, Windows.Forms.Form).Close()
            End If
         End If
      Catch
         Windows.Forms.MessageBox.Show("Failure opening this file")
      End Try
   End Sub

   Protected Overridable Function LoadFile( _
                     ByVal fileName As String) _
                     As Boolean
      Dim frm As Windows.forms.Form
      ' NOTE: This routine EXPECTS THE CALLING CODE TO HAVE A MEANINGFUL TRY CATCH!!!
      frm = Me.FormForFileType(IO.Path.GetExtension(fileName))
      frm.MdiParent = Me
      If TypeOf frm Is WinFormSupport.ISimpleForm Then
         CType(frm, WinFormSupport.ISimpleForm).Show(fileName)
         mnuReload.Enabled = True
      Else
         frm.Show()
         mnuReload.Enabled = False
      End If
   End Function

   Protected Overridable Sub SaveFile()
      If TypeOf Me.ActiveMdiChild Is WinFormSupport.ISimpleForm Then
         CType(Me.ActiveMdiChild, WinFormSupport.ISimpleForm).Save()

      End If
   End Sub

   Protected Overridable Sub SaveFileAs()
      If TypeOf Me.ActiveMdiChild Is WinFormSupport.ISimpleForm Then
         SaveFileDialog.Filter = mFilter
         Do While SaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK
            Try
               CType(Me.ActiveMdiChild, WinFormSupport.ISimpleForm).Save(SaveFileDialog.FileName)
               Exit Do
            Catch ex As Exception
               Windows.Forms.MessageBox.Show(ex.ToString)  '"There was an error opening this file")
            End Try
         Loop
      End If
   End Sub

   Protected Overridable Sub CloseFile()
      If Not Me.ActiveMdiChild Is Nothing Then
         Me.ActiveMdiChild.Close()
      End If
   End Sub
#End Region

#Region "Private Methods and Properties"
   Private Sub InitForm()
   End Sub


#End Region

End Class
