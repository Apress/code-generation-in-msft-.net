' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Very simple user control to support null handling of dates

Option Strict On
Option Explicit On 

Imports System

Public Class DateTimeUC
   Inherits System.Windows.Forms.UserControl

   Private mOriginalForeColor As System.Drawing.Color
   Private mOriginalFormat As Windows.Forms.DateTimePickerFormat
   Private mOriginalCustomFormat As String
   Private mReadOnly As Boolean

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
   Friend WithEvents chkEmpty As System.Windows.Forms.CheckBox
   Friend WithEvents dtsValue As System.Windows.Forms.DateTimePicker
   Friend WithEvents txtReadOnlyValue As System.Windows.Forms.TextBox
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.chkEmpty = New System.Windows.Forms.CheckBox
      Me.txtReadOnlyValue = New System.Windows.Forms.TextBox
      Me.dtsValue = New System.Windows.Forms.DateTimePicker
      Me.SuspendLayout()
      '
      'chkEmpty
      '
      Me.chkEmpty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
      Me.chkEmpty.Location = New System.Drawing.Point(0, 0)
      Me.chkEmpty.Name = "chkEmpty"
      Me.chkEmpty.Size = New System.Drawing.Size(56, 20)
      Me.chkEmpty.TabIndex = 0
      Me.chkEmpty.Text = "Empty"
      Me.chkEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
      '
      'txtReadOnlyValue
      '
      Me.txtReadOnlyValue.Enabled = False
      Me.txtReadOnlyValue.Location = New System.Drawing.Point(0, 0)
      Me.txtReadOnlyValue.Name = "txtReadOnlyValue"
      Me.txtReadOnlyValue.ReadOnly = True
      Me.txtReadOnlyValue.Size = New System.Drawing.Size(264, 20)
      Me.txtReadOnlyValue.TabIndex = 2
      Me.txtReadOnlyValue.Text = ""
      Me.txtReadOnlyValue.Visible = False
      '
      'dtsValue
      '
      Me.dtsValue.Format = System.Windows.Forms.DateTimePickerFormat.Short
      Me.dtsValue.Location = New System.Drawing.Point(88, 0)
      Me.dtsValue.Name = "dtsValue"
      Me.dtsValue.Size = New System.Drawing.Size(176, 20)
      Me.dtsValue.TabIndex = 3
      '
      'DateTimeUC
      '
      Me.Controls.Add(Me.dtsValue)
      Me.Controls.Add(Me.chkEmpty)
      Me.Controls.Add(Me.txtReadOnlyValue)
      Me.Name = "DateTimeUC"
      Me.Size = New System.Drawing.Size(264, 20)
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region "Public Properties and Methods"
   Public Overrides Property Text() As String
      Get
         If chkEmpty.Checked Then
            Return ""
         Else
            Return dtsValue.Text
         End If
      End Get
      Set(ByVal Value As String)
         If Value = "" OrElse Value = "1/1/1900" Then
            chkEmpty.Checked = True
            dtsValue.Width = dtsValue.Height
            txtReadOnlyValue.Text = ""
         Else
            chkEmpty.Checked = False
            dtsValue.Width = dtsValue.Height
            dtsValue.Text = Value
            txtReadOnlyValue.Text = Value
         End If
      End Set
   End Property

   Public Property EmptyText() As String
      Get
         Return chkEmpty.Text
      End Get
      Set(ByVal Value As String)
         chkEmpty.Text = Value
      End Set
   End Property

   Public Property CheckAppearance() As System.Windows.Forms.Appearance
      Get
         Return chkEmpty.Appearance
      End Get
      Set(ByVal Value As System.Windows.Forms.Appearance)
         chkEmpty.Appearance = Value
      End Set
   End Property

   Public Property Format() As Windows.Forms.DateTimePickerFormat
      Get
         Return dtsValue.Format
      End Get
      Set(ByVal Value As Windows.Forms.DateTimePickerFormat)
         dtsValue.Format = Value
      End Set
   End Property

   Public Property CustomFormat() As String
      Get
         Return dtsValue.CustomFormat
      End Get
      Set(ByVal Value As String)
         dtsValue.CustomFormat = Value
      End Set
   End Property

   Public Property MinDate() As System.DateTime
      Get
         Return dtsValue.MinDate
      End Get
      Set(ByVal Value As System.DateTime)
         dtsValue.MinDate = Value
      End Set
   End Property

   Public Property MaxDate() As System.DateTime
      Get
         Return dtsValue.MaxDate
      End Get
      Set(ByVal Value As System.DateTime)
         dtsValue.MaxDate = Value
      End Set
   End Property

   Public Property [ReadOnly]() As Boolean
      Get
         Return mReadOnly
      End Get
      Set(ByVal Value As Boolean)
         mReadOnly = Value
         chkEmpty.Visible = Not Value
         dtsValue.Visible = Not Value
         txtReadOnlyValue.Visible = Value
      End Set
   End Property

#End Region

#Region "Event Handlers"
   Private Sub chkEmpty_CheckedChanged( _
               ByVal sender As Object, _
               ByVal e As System.EventArgs) _
               Handles chkEmpty.CheckedChanged
      'If chkEmpty.Checked Then
      '   dtsValue.Checked = True
      '   'dtsValue.Text = ""
      '   ' set dts empty
      'Else
      '   dtsValue.Checked = True
      '   ' enable dts
      'End If
      If Not Me.ActiveControl Is dtsValue Then
         Me.PerformLayout()
      End If
   End Sub

   Private Sub dtsValue_TextChanged( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles dtsValue.TextChanged
      chkEmpty.Checked = False
      onTextChanged(e)
   End Sub

   'Private Sub dtsValue_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtsValue.Validated
   '   onvalidated(e)
   'End Sub

   Private Sub dtsValue_CloseUp( _
               ByVal sender As Object, _
               ByVal e As System.EventArgs) _
               Handles dtsValue.CloseUp
      Me.PerformLayout()
   End Sub

   'Private Sub dtsValue_ValueChanged( _
   '            ByVal sender As System.Object, _
   '            ByVal e As System.EventArgs) '_
   '            Handles dtsValue.ValueChanged
   '   chkEmpty.Checked = (dtsValue.Text <> "")
   'End Sub

   Protected Overrides Sub OnLayout( _
               ByVal levent As System.Windows.Forms.LayoutEventArgs)
      If mReadOnly Then
         dtsValue.Left = 0
         dtsValue.Width = Me.Width
      Else
         dtsValue.Left = chkEmpty.Right
         dtsValue.Width = Me.Width - chkEmpty.Right
      End If
      If Not Me.DesignMode Then
         If Not Me.mOriginalFormat = 0 Then
            If chkEmpty.Checked Then
               dtsValue.ForeColor = System.Drawing.Color.AntiqueWhite
               dtsValue.Format = Windows.Forms.DateTimePickerFormat.Custom
               dtsValue.CustomFormat = "''"
               ' dtsValue.Text()
            Else
               dtsValue.Format = Me.mOriginalFormat
               dtsValue.CustomFormat = Me.mOriginalCustomFormat
               dtsValue.ForeColor = mOriginalForeColor
            End If
         End If
      End If
   End Sub

   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
      Me.mOriginalForeColor = Me.ForeColor
      Me.mOriginalCustomFormat = Me.CustomFormat
      Me.mOriginalFormat = Me.Format
   End Sub

#End Region


End Class