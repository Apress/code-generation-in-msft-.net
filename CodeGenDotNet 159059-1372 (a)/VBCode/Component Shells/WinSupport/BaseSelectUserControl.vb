' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Base for selection user controls - WinForms 

Option Strict On
Option Explicit On 

Imports System

Public Class SelectionMadeEventArgs
   Inherits System.EventArgs
   Public primaryKey As System.Object
   Public Sub New(ByVal pk As Object)
      primaryKey = pk
   End Sub
End Class

Public Class BaseSelectUserControl

   Inherits System.Windows.Forms.UserControl

   Public Event SelectionMade( _
            ByVal sender As Object, _
            ByVal e As SelectionMadeEventArgs)


#Region "Class Declarations"
   Private mList As CSLA.ReadOnlyCollectionBase
   Protected mCaption As String
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
   Protected Friend WithEvents dgDisplay As System.Windows.Forms.DataGrid

   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.dgDisplay = New System.Windows.Forms.DataGrid
      Me.SuspendLayout()
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
      Me.ClientSize = New System.Drawing.Size(456, 229)
      Me.Controls.Add(Me.dgDisplay)
      Me.Name = "ProjectSelect"
      Me.Text = ""
      Me.ResumeLayout(False)

   End Sub

#End Region

   Public ReadOnly Property Result() As Object
      Get
         Return GetResult
      End Get
   End Property

   Public Overridable Function GetList() As CSLA.ReadOnlyCollectionBase
      Throw New System.Exception("Must Implement GetList in derived form")
   End Function

   Protected Overridable Sub BuildColumns()
      Throw New System.Exception("Must Implement BuildColumns in derived form")
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

   Private Sub dgDisplay_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgDisplay.CurrentCellChanged
      RaiseEvent SelectionMade(Me, New SelectionMadeEventArgs(Me.Result))
   End Sub

   Public ReadOnly Property Caption() As String
      Get
         Return mCaption
      End Get
   End Property
End Class
