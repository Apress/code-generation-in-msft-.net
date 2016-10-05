' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Base class for WinForms editing user controls

Option Strict On
Option Explicit On 

Imports System

Public Class BaseEditUserControl
   Inherits System.Windows.Forms.UserControl
   ' Implements IEditUserControl

   Public Event DataChanged( _
                  ByVal sender As Object, _
                  ByVal e As EventArgs)


#Region "Class Level Declaration"
   Private mSelectUserControl As BaseSelectUserControl
   Protected mBusinessObject As BusinessObjectSupport.IBusinessObject
   Protected mMargin As Int32 = 5
   Protected mbIsDirty As Boolean
   Protected mCaption As String
   ' Protected mMinimumSize As System.Drawing.Size
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
   Protected Friend WithEvents errProvider As System.Windows.Forms.ErrorProvider
   Protected Friend WithEvents toolTip As System.Windows.Forms.ToolTip
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.errProvider = New System.Windows.Forms.ErrorProvider
      Me.toolTip = New System.Windows.Forms.ToolTip
      '
      'errProvider
      '
      Me.errProvider.ContainerControl = Me
      '
      'toolTip
      '
      '
      'BaseEditUserControl
      '
      Me.Name = "BaseEditUserControl"

   End Sub

#End Region

#Region "Public Properties and Methods"
   Public ReadOnly Property SelectUserControl() As BaseSelectUserControl
      Get
         Return mSelectUserControl
      End Get
   End Property

   Public Overridable Function GetList() As CSLA.ReadOnlyCollectionBase
      Throw New System.ApplicationException("The GetList method must be overridden")
   End Function

   Public Sub VisitControls()
      VisitControls(Me)
   End Sub
   Public Sub VisitControls(ByVal ctl As Windows.Forms.Control)
      'Visit every control on the form to set error provider
      'By doing this as a complete first step, all error flags are set
      Dim childCtl As Windows.Forms.Control
      For Each childCtl In ctl.Controls
         childCtl.Focus()
         If childCtl.Controls.Count > 0 Then
            VisitControls(childCtl)
         End If
      Next
   End Sub

   Public Function IsFormValid() As Boolean
      Return IsFormValid(Me, Me.errProvider)
   End Function
   Public Function IsFormValid( _
               ByVal ctl As Windows.Forms.Control, _
               ByVal errProv As Windows.Forms.ErrorProvider) As Boolean
      Dim bValid As Boolean = True
      Dim childCtl As Windows.Forms.Control
      For Each childCtl In ctl.Controls
         'childCtrl.Focus()
         If errProv.GetError(childCtl) <> "" Then
            bValid = False
            Exit For
         End If
         If childCtl.Controls.Count > 0 Then
            bValid = IsFormValid(childCtl, errProv)
            If Not bValid Then
               Exit For
            End If
         End If
      Next
      Return bValid
   End Function

#End Region

#Region "Protected Properties and Methods"
   Protected Overridable Sub ResizeUC()
      Dim maxHeight As Integer
      For Each ctl As Windows.Forms.Control In Me.Controls
         If ctl.Visible Then
            If ctl.Bottom > maxHeight Then
               maxHeight = ctl.Bottom
            End If
         End If
      Next
      Me.ClientSize = New Drawing.Size(Me.ClientSize.Width, maxHeight + mMargin)
   End Sub

   Protected Overridable Sub OnDataChanged(ByVal sender As Object, ByVal e As EventArgs)
      RaiseEvent DataChanged(sender, e)
   End Sub

   Protected Sub RemoveControl( _
               ByVal ctl As Windows.Forms.Control, _
               ByVal sFieldName As String)
      Dim height As Int32
      If sFieldName.Trim.Length > 0 Then
         If ctl.Name.Length > 3 Then
            If ctl.Name.Substring(3) = sFieldName Then
               ' remove later controls and exit
               ctl.Visible = False
               If ctl.Name.Substring(0, 3).ToLower <> "lbl" Then
                  height = ctl.Height + CType(Me, IEditUserControl).VerticalMargin
                  For Each ctlOther As Windows.Forms.Control In ctl.Parent.Controls
                     If ctlOther.Top > ctl.Bottom Then
                        ctlOther.Top -= height
                     End If
                  Next
               End If
            End If
         End If
         For Each ctlChild As Windows.Forms.Control In ctl.Controls
            RemoveControl(ctlChild, sFieldName)
         Next
      End If
   End Sub

   Protected Overridable Sub BindField( _
               ByVal control As Windows.forms.Control, _
               ByVal propertyName As String, _
               ByVal dataSource As Object, _
               ByVal dataMember As String, _
               ByVal netType As System.Type)

      Dim bd As Windows.Forms.Binding
      Dim index As Integer

      For index = control.DataBindings.Count - 1 To 0 Step -1
         bd = control.DataBindings.Item(index)
         If bd.PropertyName = propertyName Then
            control.DataBindings.Remove(bd)
         End If
      Next
      control.DataBindings.Add(propertyName, dataSource, dataMember)

      BindEvents(control, propertyName, dataSource, dataMember, netType)

   End Sub

   Protected Overridable Sub BindEvents( _
               ByVal control As Windows.forms.Control, _
               ByVal controlProperty As String, _
               ByVal source As Object, _
               ByVal sourceProperty As String, _
               ByVal netType As System.Type)
      If netType Is GetType(System.Decimal) Then
         WinSupport.Utility.BindEvents(control, controlProperty, source, sourceProperty, _
                  AddressOf FormatCurrency, AddressOf ParseCurrency)
      End If
   End Sub


#Region "Format Event Handlers"
   Protected Overridable Sub FormatCurrency( _
                  ByVal sender As Object, _
                  ByVal e As Windows.Forms.ConvertEventArgs)
      e.Value = CType(e.Value, System.Decimal).ToString("C")
   End Sub

   Protected Overridable Sub ParseCurrency( _
               ByVal sender As Object, _
               ByVal e As Windows.Forms.ConvertEventArgs)
      Dim val As String = e.Value.ToString
      val = val.Replace("$", "")
      e.Value = System.Decimal.Parse(val)
   End Sub

#End Region

   Public ReadOnly Property Caption() As String
      Get
         Return mCaption
      End Get
   End Property

#End Region

End Class
