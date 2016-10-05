' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Initial work on an options dialog. Not meaningfully functional

Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class Options
    Inherits System.Windows.Forms.Form

#Region "Class level declarations -empty"
#End Region

#Region " Windows Form Designer generated code"

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
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
   Friend WithEvents propGridOptions As System.Windows.Forms.PropertyGrid
   Friend WithEvents Panel1 As System.Windows.Forms.Panel
   Friend WithEvents btnOK As System.Windows.Forms.Button
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.propGridOptions = New System.Windows.Forms.PropertyGrid
      Me.Panel1 = New System.Windows.Forms.Panel
      Me.btnOK = New System.Windows.Forms.Button
      Me.Panel1.SuspendLayout()
      Me.SuspendLayout()
      '
      'propGridOptions
      '
      Me.propGridOptions.CommandsVisibleIfAvailable = True
      Me.propGridOptions.Dock = System.Windows.Forms.DockStyle.Fill
      Me.propGridOptions.LargeButtons = False
      Me.propGridOptions.LineColor = System.Drawing.SystemColors.ScrollBar
      Me.propGridOptions.Location = New System.Drawing.Point(0, 0)
      Me.propGridOptions.Name = "propGridOptions"
      Me.propGridOptions.Size = New System.Drawing.Size(292, 273)
      Me.propGridOptions.TabIndex = 0
      Me.propGridOptions.Text = "PropertyGrid1"
      Me.propGridOptions.ViewBackColor = System.Drawing.SystemColors.Window
      Me.propGridOptions.ViewForeColor = System.Drawing.SystemColors.WindowText
      '
      'Panel1
      '
      Me.Panel1.Controls.Add(Me.btnOK)
      Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
      Me.Panel1.Location = New System.Drawing.Point(0, 241)
      Me.Panel1.Name = "Panel1"
      Me.Panel1.Size = New System.Drawing.Size(292, 32)
      Me.Panel1.TabIndex = 1
      '
      'btnOK
      '
      Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnOK.Location = New System.Drawing.Point(120, 8)
      Me.btnOK.Name = "btnOK"
      Me.btnOK.TabIndex = 0
      Me.btnOK.Text = "OK"
      '
      'Options
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(292, 273)
      Me.Controls.Add(Me.Panel1)
      Me.Controls.Add(Me.propGridOptions)
      Me.Name = "Options"
      Me.Text = "Options"
      Me.Panel1.ResumeLayout(False)
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region "Event Handlers"
   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
      Dim opt As New Options
      propGridOptions.SelectedObject = opt
      propGridOptions.Select()
   End Sub
#End Region

#Region "Public Methods and Properties-empty"
#End Region

#Region "Protected and Friend Methods and Properties-empty"
#End Region

#Region "Protected Event Response Methods-empty"
#End Region

#Region "Private Methods and Properties"
   Private Sub InitForm()
   End Sub
#End Region


   Private Class Options
      Private mFont As Drawing.Font

      Public Property Font() As Drawing.Font
         Get
            Return mFont
         End Get
         Set(ByVal Value As Drawing.Font)
            mFont = Value
         End Set
      End Property
   End Class
End Class
