' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: The modal form displayed when generation is complete
'  Refactor: Move to a tab of the generation form, rather than the modal dialog.

Option Strict On
Option Explicit On 

Imports System

Public Class OutputDisplay
   Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call

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
   Friend WithEvents dbOutput As System.Windows.Forms.DataGrid
   Friend WithEvents btnClose As System.Windows.Forms.Button
   Friend WithEvents lblMsg As System.Windows.Forms.Label
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.dbOutput = New System.Windows.Forms.DataGrid
      Me.btnClose = New System.Windows.Forms.Button
      Me.lblMsg = New System.Windows.Forms.Label
      CType(Me.dbOutput, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'dbOutput
      '
      Me.dbOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                  Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.dbOutput.DataMember = ""
      Me.dbOutput.HeaderForeColor = System.Drawing.SystemColors.ControlText
      Me.dbOutput.Location = New System.Drawing.Point(0, 0)
      Me.dbOutput.Name = "dbOutput"
      Me.dbOutput.Size = New System.Drawing.Size(616, 232)
      Me.dbOutput.TabIndex = 0
      '
      'btnClose
      '
      Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnClose.Location = New System.Drawing.Point(528, 240)
      Me.btnClose.Name = "btnClose"
      Me.btnClose.TabIndex = 1
      Me.btnClose.Text = "Close"
      '
      'lblMsg
      '
      Me.lblMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.lblMsg.Location = New System.Drawing.Point(0, 240)
      Me.lblMsg.Name = "lblMsg"
      Me.lblMsg.Size = New System.Drawing.Size(488, 16)
      Me.lblMsg.TabIndex = 2
      '
      'OutputDisplay
      '
      Me.AcceptButton = Me.btnClose
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.CancelButton = Me.btnClose
      Me.ClientSize = New System.Drawing.Size(616, 266)
      Me.Controls.Add(Me.lblMsg)
      Me.Controls.Add(Me.btnClose)
      Me.Controls.Add(Me.dbOutput)
      Me.Name = "OutputDisplay"
      Me.Text = "OutputDisplay"
      CType(Me.dbOutput, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

   End Sub

#End Region

   Public Overloads Sub Show( _
               ByVal parent As Windows.Forms.Form, _
               ByVal output As System.Collections.ArrayList, _
               ByVal sMsg As String)
      dbOutput.DataSource = output
      lblMsg.Text = sMsg
      Me.ShowDialog(parent)
   End Sub

End Class
