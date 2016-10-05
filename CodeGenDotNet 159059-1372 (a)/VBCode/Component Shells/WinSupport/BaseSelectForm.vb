' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Base class for the selection form
'  Refactor: Change select form to better support user control

Public Class BaseSelectForm
   Inherits System.Windows.Forms.Form

#Region "Class Declarations"
   Private mList As CSLA.ReadOnlyCollectionBase
#End Region

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
   Friend WithEvents btnOK As System.Windows.Forms.Button
   Friend WithEvents btnCancel As System.Windows.Forms.Button
   Protected Friend WithEvents dgDisplay As System.Windows.Forms.DataGrid

   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.btnOK = New System.Windows.Forms.Button
      Me.btnCancel = New System.Windows.Forms.Button
      Me.dgDisplay = New System.Windows.Forms.DataGrid
      Me.SuspendLayout()
      '
      'btnOK
      '
      Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnOK.Location = New System.Drawing.Point(368, 8)
      Me.btnOK.Name = "btnOK"
      Me.btnOK.TabIndex = 0
      Me.btnOK.Text = "OK"
      '
      'btnCancel
      '
      Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnCancel.Location = New System.Drawing.Point(368, 40)
      Me.btnCancel.Name = "btnCancel"
      Me.btnCancel.TabIndex = 1
      Me.btnCancel.Text = "Cancel"
      '
      'dgDisplay
      '
      Me.dgDisplay.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
          Or System.Windows.Forms.AnchorStyles.Left) _
          Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.dgDisplay.DataSource = Nothing
      Me.dgDisplay.Location = New System.Drawing.Point(0, 0)
      Me.dgDisplay.Name = "dgDisplay"
      Me.dgDisplay.Size = New System.Drawing.Size(352, 232)
      Me.dgDisplay.TabIndex = 2
      '
      'ProjectSelect
      '
      Me.AcceptButton = Me.btnOK
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.CancelButton = Me.btnCancel
      Me.ClientSize = New System.Drawing.Size(456, 229)
      Me.Controls.Add(Me.dgDisplay)
      Me.Controls.Add(Me.btnCancel)
      Me.Controls.Add(Me.btnOK)
      Me.Name = "ProjectSelect"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
      Me.Text = "Project Select"
      Me.ResumeLayout(False)

   End Sub

#End Region

   Public ReadOnly Property Result() As Object
      Get
         Return GetResult
      End Get
   End Property

   Protected Overridable Function GetList() As CSLA.ReadOnlyCollectionBase
      Throw New System.Exception("Must Implement GetList in derived form")
   End Function

   Protected Overridable Sub SetResult()
      Throw New System.Exception("Must Implement GetResult in derived form")
   End Sub

   Protected Overridable Sub SetEmptyResult()
      Throw New System.Exception("Must Implement GetEmptyResult in derived form")
   End Sub

   Protected Overridable ReadOnly Property GetResult() As Object
      Get
         Throw New System.Exception("Must Implement GetResult in derived form")
      End Get
   End Property

   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
      mList = GetList()
      With dgDisplay
         .DataSource = mList
         .Focus()
      End With
   End Sub

   Private Sub btnOK_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles btnOK.Click, _
                     dgDisplay.DoubleClick
      SetResult()
      Hide()
   End Sub

   Private Sub btnCancel_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles btnCancel.Click
      SetEmptyResult()
      Hide()
   End Sub

End Class
