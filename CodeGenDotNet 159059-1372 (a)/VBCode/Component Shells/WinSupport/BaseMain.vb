' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Base class for the main MDI parent

Option Strict On
Option Explicit On 

Imports System
Imports System.Configuration
Imports System.Security.Principal
Imports CSLA.Security
Imports System.Threading
'Imports CSLA.BatchQueue
Imports KADGen.BusinessObjectSupport
Imports System.Windows.Forms

Public Class BaseMain
   Inherits System.Windows.Forms.Form
   Private Const vbcrlf As String = Microsoft.VisualBasic.vbCrLf
   Private Shared mMain As BaseMain


#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
      InitializeForm()
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
   Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
   Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
   Friend WithEvents mnuFileLogin As System.Windows.Forms.MenuItem
   Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
   Friend WithEvents mnuFileExit As System.Windows.Forms.MenuItem
   Protected Friend WithEvents mnuAction As System.Windows.Forms.MenuItem
   Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
   Friend WithEvents pnlStatus As System.Windows.Forms.StatusBarPanel
   Friend WithEvents pnlUser As System.Windows.Forms.StatusBarPanel
   Protected Friend WithEvents mnuOtherTools As System.Windows.Forms.MenuItem
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.MainMenu1 = New System.Windows.Forms.MainMenu
      Me.MenuItem1 = New System.Windows.Forms.MenuItem
      Me.mnuFileLogin = New System.Windows.Forms.MenuItem
      Me.MenuItem3 = New System.Windows.Forms.MenuItem
      Me.mnuFileExit = New System.Windows.Forms.MenuItem
      Me.mnuAction = New System.Windows.Forms.MenuItem
      Me.StatusBar1 = New System.Windows.Forms.StatusBar
      Me.pnlStatus = New System.Windows.Forms.StatusBarPanel
      Me.pnlUser = New System.Windows.Forms.StatusBarPanel
      Me.mnuOtherTools = New System.Windows.Forms.MenuItem
      CType(Me.pnlStatus, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.pnlUser, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'MainMenu1
      '
      Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.mnuAction, Me.mnuOtherTools})
      '
      'MenuItem1
      '
      Me.MenuItem1.Index = 0
      Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileLogin, Me.MenuItem3, Me.mnuFileExit})
      Me.MenuItem1.Text = "&File"
      '
      'mnuFileLogin
      '
      Me.mnuFileLogin.Index = 0
      Me.mnuFileLogin.Text = "&Login"
      '
      'MenuItem3
      '
      Me.MenuItem3.Index = 1
      Me.MenuItem3.Text = "-"
      '
      'mnuFileExit
      '
      Me.mnuFileExit.Index = 2
      Me.mnuFileExit.Text = "E&xit"
      '
      'mnuAction
      '
      Me.mnuAction.Index = 1
      Me.mnuAction.Text = "&Action"
      '
      'StatusBar1
      '
      Me.StatusBar1.Location = New System.Drawing.Point(0, 384)
      Me.StatusBar1.Name = "StatusBar1"
      Me.StatusBar1.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.pnlStatus, Me.pnlUser})
      Me.StatusBar1.ShowPanels = True
      Me.StatusBar1.Size = New System.Drawing.Size(720, 22)
      Me.StatusBar1.TabIndex = 0
      Me.StatusBar1.Text = "StatusBar1"
      '
      'pnlStatus
      '
      Me.pnlStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
      Me.pnlStatus.Width = 604
      '
      'mnuOtherTools
      '
      Me.mnuOtherTools.Index = 2
      Me.mnuOtherTools.Text = "Other Tools"
      Me.mnuOtherTools.Visible = False
      '
      'BaseMain
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(720, 406)
      Me.Controls.Add(Me.StatusBar1)
      Me.IsMdiContainer = True
      Me.Menu = Me.MainMenu1
      Me.Name = "BaseMain"
      Me.Text = "Project Tracker"
      Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
      CType(Me.pnlStatus, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.pnlUser, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region " Load and Exit "

   Private Sub InitializeForm()
      mMain = Me
   End Sub

   Private Sub mnuFileExit_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles mnuFileExit.Click
      Close()
   End Sub

   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
      MyBase.OnLoad(e)
      If ConfigurationSettings.AppSettings("Authentication") = "Windows" Then
         mnuFileLogin.Visible = False
         AppDomain.CurrentDomain.SetPrincipalPolicy( _
                     PrincipalPolicy.WindowsPrincipal)
         BuildMenus()
      Else
         DoLogin()
      End If
   End Sub


   Protected Overridable Sub BuildMenus()
      Dim type As System.Type
      Dim asms As Reflection.Assembly() = BusinessObjectAssemblyList()
      Dim asm As Reflection.Assembly
      Dim types() As System.Type
      Dim attrs() As System.Attribute
      Dim bo As IBusinessObject
      Dim mnu As WinSupport.ActionMenuItem
      Dim frmSelect As System.Type
      Dim ucEdit As System.Type
      Dim selectUCType As System.Type
      Dim editUCType As System.Type
      Dim meType As System.Type = Me.GetType

      mnuAction.MenuItems.Clear()
      For i As Int32 = 0 To asms.GetUpperBound(0)
         asm = asms(i)
         types = asm.GetTypes
         For Each type In types
            If Not type.IsAbstract Then
               attrs = System.Attribute.GetCustomAttributes(type)
               For Each attr As System.Attribute In attrs
                  If TypeOf attr Is RootAttribute Then
                     ' type belongs in menu
                     selectUCType = meType.Assembly.GetType( _
                                 meType.Namespace & "." & type.Name & "SelectUC")
                     editUCType = meType.Assembly.GetType( _
                                 meType.Namespace & "." & type.Name & "Edit")
                     If Not selectUCType Is Nothing And _
                                 Not editUCType Is Nothing Then
                        mnu = New WinSupport.ActionMenuItem(type, _
                                 editUCType, selectUCType, Me)
                        mnuAction.MenuItems.Add(mnu)
                     End If
                  End If
               Next
            End If
         Next
      Next
      mnuAction.Enabled = True


   End Sub
   'Protected Sub BuildMenus()
   '   Dim type As System.Type
   '   Dim asms As Reflection.Assembly() = BusinessObjectAssemblyList()
   '   Dim asm As Reflection.Assembly
   '   Dim types() As System.Type
   '   Dim attrs() As System.Attribute
   '   Dim bo As IBusinessObject
   '   Dim mnu As WinSupport.ActionMenuItem
   '   Dim frmSelect As System.Type
   '   Dim ucEdit As System.Type
   '   Dim selectFormType As System.Type
   '   Dim editUCType As System.Type
   '   Dim meType As System.Type = Me.GetType

   '   mnuAction.MenuItems.Clear()
   '   For i As Int32 = 0 To asms.GetUpperBound(0)
   '      asm = asms(i)
   '      types = asm.GetTypes
   '      For Each type In types
   '         If Not type.IsAbstract Then
   '            attrs = System.Attribute.GetCustomAttributes(type)
   '            For Each attr As System.Attribute In attrs
   '               If TypeOf attr Is RootAttribute Then
   '                  ' type belongs in menu
   '                  selectFormType = meType.Assembly.GetType( _
   '                              meType.Namespace & "." & type.Name & "Select")
   '                  editUCType = meType.Assembly.GetType( _
   '                              meType.Namespace & "." & type.Name & "Edit")
   '                  If Not selectFormType Is Nothing And _
   '                              Not editUCType Is Nothing Then
   '                     mnu = New WinSupport.ActionMenuItem(type, _
   '                              editUCType, selectFormType)
   '                     mnuAction.MenuItems.Add(mnu)
   '                  End If
   '               End If
   '            Next
   '         End If
   '      Next
   '   Next
   '   mnuAction.Enabled = True


   'End Sub


   Protected Overridable ReadOnly Property BusinessObjectAssemblyList() As Reflection.Assembly()
      Get
         Throw New System.ApplicationException("You must overrride this method")
      End Get
   End Property

#End Region

#Region " Login/Logout/Authorization "

   Private Sub mnuFileLogin_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles mnuFileLogin.Click

      DoLogin()

   End Sub

   Private Sub DoLogin()

      Dim dlg As New Login

      dlg.ShowDialog(Me)
      If dlg.Login Then
         Cursor = Cursors.WaitCursor
         Me.SetStatus("Verifying user...")
         BusinessPrincipal.Login(dlg.Username, dlg.Password)
         Me.SetStatus("")
         Cursor = Cursors.Default

         If Thread.CurrentPrincipal.Identity.IsAuthenticated Then
            pnlUser.Text = Thread.CurrentPrincipal.Identity.Name
            BuildMenus()
         Else
            DoLogout()
            MessageBox.Show("The username and password were not valid", _
                     "Incorrect Password", MessageBoxButtons.OK, _
                     MessageBoxIcon.Exclamation)
         End If

      Else
         DoLogout()
      End If

   End Sub

   Private Sub DoLogout()
      'Can't set Thread.CurrentPrincipal to Nothing; it does nothing.
      'Instead, create an unauthenticated identity and principal
      ' Change from Rocky's book
      Dim identity As New GenericIdentity("", "")
      Dim principal As New GenericPrincipal(identity, New String() {})
      Thread.CurrentPrincipal = principal

      pnlUser.Text = ""

      BuildMenus()

      Try
         '    GetMenuItem("&Logout").Text = "&Login"
      Catch
         '
      End Try

   End Sub

#End Region

#Region " Status "


   Public Shared Sub SetStatus(ByVal Text As String)
      mMain.pnlStatus.Text = Text
   End Sub


#End Region

End Class
