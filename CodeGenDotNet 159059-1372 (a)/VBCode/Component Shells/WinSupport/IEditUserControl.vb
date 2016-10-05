' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Interface for editing user control

Option Strict On
Option Explicit On 

Imports KADGen.BusinessObjectSupport

Public Interface IEditUserControl
   Sub SetupControl(ByVal businessObject As IBusinessObject)
   Sub SetupControl(ByVal businessObject As IBusinessObject, ByVal parentPrimaryKey As String)
   ReadOnly Property BusinessObject() As IBusinessObject
   ReadOnly Property MinimumSize() As System.Drawing.Size
   ReadOnly Property LabelWidth() As System.Int32
   ReadOnly Property ControlWidth() As System.Int32
   ReadOnly Property ControlTop() As System.Int32
   ReadOnly Property ControlLeft() As System.Int32
   ReadOnly Property HorizontalMargin() As System.Int32
   ReadOnly Property VerticalMargin() As System.Int32
   ReadOnly Property IdealHeight() As System.Int32
   ReadOnly Property IdealWidth() As System.Int32
   ReadOnly Property EditMode() As EditMode
   Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
   Function Save() As IBusinessObject
   Sub Delete()
   Sub CancelEdit()
   Function GetList() As CSLA.ReadOnlyCollectionBase
   ' Function GetNew() As IBusinessObject
   Function CanCreate() As Boolean
   Function CanRetrieve() As Boolean
   Function CanUpdate() As Boolean
   Function CanDelete() As Boolean
End Interface
