' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Specialized text box that allows browsing to find a folder

Option Strict On
Option Explicit On 

Imports System
Imports System.Windows

Public Class FolderTextBox
   Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call

   End Sub

   'UserControl overrides dispose to clean up the component list.
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
   Friend WithEvents btnOpenFileDialog As System.Windows.Forms.Button
   Friend WithEvents Label As System.Windows.Forms.Label
   Friend WithEvents TextBox As System.Windows.Forms.TextBox
   Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
   Friend WithEvents FolderBrowserDialog As System.Windows.Forms.FolderBrowserDialog
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Me.Label = New System.Windows.Forms.Label
      Me.TextBox = New System.Windows.Forms.TextBox
      Me.btnOpenFileDialog = New System.Windows.Forms.Button
      Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
      Me.FolderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog
      Me.SuspendLayout()
      '
      'Label
      '
      Me.Label.Dock = System.Windows.Forms.DockStyle.Left
      Me.Label.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.Label.Location = New System.Drawing.Point(0, 0)
      Me.Label.Name = "Label"
      Me.Label.Size = New System.Drawing.Size(100, 24)
      Me.Label.TabIndex = 0
      '
      'TextBox
      '
      Me.TextBox.Dock = System.Windows.Forms.DockStyle.Fill
      Me.TextBox.Location = New System.Drawing.Point(100, 0)
      Me.TextBox.Name = "TextBox"
      Me.TextBox.Size = New System.Drawing.Size(140, 20)
      Me.TextBox.TabIndex = 1
      Me.TextBox.Text = ""
      Me.ToolTip.SetToolTip(Me.TextBox, "Fred")
      '
      'btnOpenFileDialog
      '
      Me.btnOpenFileDialog.Dock = System.Windows.Forms.DockStyle.Right
      Me.btnOpenFileDialog.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnOpenFileDialog.Location = New System.Drawing.Point(208, 0)
      Me.btnOpenFileDialog.Name = "btnOpenFileDialog"
      Me.btnOpenFileDialog.Size = New System.Drawing.Size(32, 24)
      Me.btnOpenFileDialog.TabIndex = 2
      Me.btnOpenFileDialog.Text = "..."
      '
      'DirectoryTextBox
      '
      Me.Controls.Add(Me.btnOpenFileDialog)
      Me.Controls.Add(Me.TextBox)
      Me.Controls.Add(Me.Label)
      Me.Name = "DirectoryTextBox"
      Me.Size = New System.Drawing.Size(240, 24)
      Me.ResumeLayout(False)

   End Sub

#End Region

   Public Property LabelText() As String
      Get
         Return Label.Text
      End Get
      Set(ByVal Value As String)
         Label.Text = Value
      End Set
   End Property

   Public Property LabelWidth() As Int32
      Get
         Return Label.Width
      End Get
      Set(ByVal Value As Int32)
         Label.Width = Value
      End Set
   End Property

   Public Overrides Property Text() As String
      Get
         Return TextBox.Text
      End Get
      Set(ByVal Value As String)
         TextBox.Text = Value
      End Set
   End Property

   Public Property FolderName() As String
      Get
         Return TextBox.Text
      End Get
      Set(ByVal Value As String)
         TextBox.Text = Value
      End Set
   End Property

   Private Sub btnOpenFileDialog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenFileDialog.Click
      If FolderName.Length > 0 Then
         FolderBrowserDialog.SelectedPath = FolderName
      End If
      If FolderBrowserDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
         TextBox.Text = FolderBrowserDialog.SelectedPath
      End If
   End Sub

   Private Sub TextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox.TextChanged
      MyBase.OnTextChanged(e)
      Me.ToolTip.SetToolTip(Me.TextBox, TextBox.Text)
   End Sub

End Class
