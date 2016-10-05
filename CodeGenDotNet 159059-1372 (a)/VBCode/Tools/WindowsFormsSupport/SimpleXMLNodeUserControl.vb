' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Base class for an XML/XSD user control.

Option Explicit On 
Option Strict On

Imports System
Imports KADGen

Public MustInherit Class SimpleXMLNodeUserControl
   Inherits System.Windows.Forms.UserControl
   Implements WinFormSupport.ISimpleUserControl


   ' Dim mNode As Xml.XmlNode
   Dim margin As Int32 = 2
   Dim mrequiredControls As New Collections.ArrayList

   Public MustOverride Sub MarkAsChecked( _
               ByVal Checked As Boolean) _
               Implements ISimpleUserControl.MarkAsChecked

#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
      InitForm()

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
   Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
   Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
      Me.ErrorProvider = New System.Windows.Forms.ErrorProvider
      '
      'ErrorProvider
      '
      Me.ErrorProvider.ContainerControl = Me
      '
      'SimpleXMLNodeUserControl
      '
      Me.AutoScroll = True
      Me.Name = "SimpleXMLNodeUserControl"

   End Sub

#End Region

   Private Sub InitForm()
      Me.Dock = Windows.Forms.DockStyle.Fill
   End Sub

   Private Sub ctl_Changed(ByVal sender As Object, ByVal e As EventArgs)
      If TypeOf Me.ParentForm Is ISimpleForm Then
         CType(Me.ParentForm, ISimpleForm).HasChanges = True
      End If
   End Sub

   Public Sub Gather() Implements ISimpleUserControl.Gather
      Gather(Me)
   End Sub

   Public Sub Gather(ByVal parentControl As Windows.Forms.Control)
      Dim attrName As String
      Dim node As Xml.XmlNode
      Dim attr As Xml.XmlAttribute

      If TypeOf parentControl.Tag Is Xml.XmlNode Then
         node = CType(parentControl.Tag, Xml.XmlNode)
         For Each ctl As Windows.Forms.Control In parentControl.Controls
            If TypeOf ctl Is Windows.Forms.GroupBox Then
               Gather(ctl)
            Else
               If ctl.Name.Length > 3 Then
                  attrName = ctl.Name.Substring(3) ' remove prefix
                  If ctl.Tag Is Nothing Then
                     attr = node.OwnerDocument.CreateAttribute(attrName)
                     node.Attributes.Append(attr)
                  Else
                     attr = CType(ctl.Tag, Xml.XmlAttribute)
                  End If
                  Select Case ctl.Name.Substring(0, 3)
                     Case "chk"
                        attr.Value = CType(ctl, Windows.Forms.CheckBox).Checked.ToString.ToLower
                     Case Else
                        attr.Value = ctl.Text
                  End Select
               End If
            End If
         Next
      End If
   End Sub

   Public Sub Scatter(ByVal logicalRow As Object) Implements ISimpleUserControl.Scatter
      If TypeOf logicalRow Is Xml.XmlNode Then
         Scatter(CType(logicalRow, Xml.XmlNode), Nothing)
      End If
   End Sub

   Public Function Scatter( _
                  ByVal xmlDataRow As Xml.XmlNode, _
                  ByVal xmlSchemaNode As Xml.XmlNode) _
                  As Int32
      Me.Tag = xmlDataRow
      Return Scatter(Me, 0, xmlDataRow, xmlSchemaNode)
   End Function

   Public Function Scatter( _
                  ByVal parentControl As Windows.Forms.Control, _
                  ByVal top As Int32, _
                  ByVal xmlDataRow As Xml.XmlNode, _
                  ByVal xmlSchemaNode As Xml.XmlNode) _
                  As Int32
      ' Dim nodeTmp As Xml.XmlNode
      Dim xsdNodeList As Xml.XmlNodeList
      Dim xTypeName As String
      Dim name As String
      Dim special As String
      Dim extra As Object
      Dim xpathString As String
      Dim ctl As Windows.Forms.Control
      Dim attrNode As Xml.XmlNode

      top += margin

      parentControl.Controls.Clear()
      If Not xmlDataRow Is Nothing Then
         If xmlSchemaNode Is Nothing Then
            For Each attr As Xml.XmlAttribute In xmlDataRow.Attributes
               If Not attr.Name.ToLower = "checked" Then
                  ctl = NewLabeledControl(parentControl, attr.Name, _
                         "String", "", "", False, top, attr.Value, Nothing)
                  ctl.Tag = attr
                  top = ctl.Bottom + margin
               End If
            Next
         Else
            Dim xsdNsmgr As Xml.XmlNamespaceManager = _
                        Utility.Tools.BuildNameSpaceManager(xmlSchemaNode, True)
            xsdNsmgr.AddNamespace("kd", "http://kadgen.com/KADGenDrivingx.xsd")
            ' At this point, we may be looking at either a schema or a type
            If xmlSchemaNode.LocalName = "element" Then
               Dim sType As String = Utility.Tools.GetAttributeOrEmpty( _
                        xmlSchemaNode, "type")
               If sType.Trim.Length = 0 Then
                  xpathString = "xs:complexType/xs:attribute"
               Else
                  xpathString = "//xs:complexType[@name='" & _
                                 sType & "']/xs:attribute"
               End If
            Else
               xpathString = "xs:attribute"
            End If
            xsdNodeList = xmlSchemaNode.SelectNodes(xpathString, xsdNsmgr)
            For Each node As Xml.XmlNode In xsdNodeList
               name = Utility.Tools.GetAttributeOrEmpty(node, "name")
               xTypeName = Utility.Tools.StripPrefix( _
                           Utility.Tools.GetAttributeOrEmpty(node, "type"))
               special = Utility.Tools.GetAnnotOrEmpty(node, "Special", xsdNsmgr)
               attrNode = xmlDataRow.Attributes.GetNamedItem(name)
               Select Case xTypeName.ToLower
                  Case "string", "int", "bool"
                  Case "filename"
                     special = "file"
                  Case "directory"
                     special = "directory"
                  Case Else
                     Dim comboList() As String
                     comboList = Utility.Tools.StringArrayFromNodeList( _
                              xmlSchemaNode.SelectNodes( _
                                    "//xs:simpleType[@name='" & xTypeName & _
                                    "']/xs:restriction/xs:enumeration", xsdNsmgr))
                     If comboList.GetLength(0) > 0 Then
                        special = "combobox"
                        extra = comboList
                     End If
               End Select
               ctl = NewLabeledControl( _
                     parentControl, _
                     name, _
                     xTypeName, _
                     special, _
                     Utility.Tools.GetAttributeOrEmpty(node, "", "kd:Desc", xsdNsmgr), _
                     (Utility.Tools.GetAnnotOrEmpty(node, "Required", xsdNsmgr) _
                              = "1"), _
                     top, _
                     Utility.Tools.GetAttributeOrEmpty(xmlDataRow, name), _
                     extra)
               'ctl = NewLabeledControl( _
               '      parentControl, _
               '      name, _
               '      xTypeName, _
               '      special, _
               '      Utility.Tools.GetAnnotOrEmpty(node, "Desc", xsdNsmgr), _
               '      (Utility.Tools.GetAnnotOrEmpty(node, "Required", xsdNsmgr) _
               '               = "1"), _
               '      top, _
               '      Utility.Tools.GetAttributeOrEmpty(xmlDataRow, name), _
               '      extra)
               top = ctl.Bottom + margin
               ctl.Tag = attrNode
            Next
            xsdNodeList = xmlSchemaNode.SelectNodes( _
                           "xs:complexType/xs:sequence/xs:element", xsdNsmgr)
            Dim dataNsmgr As New Xml.XmlNamespaceManager( _
                           xmlDataRow.OwnerDocument.NameTable)
            dataNsmgr.AddNamespace("kg", "http://kadgen.com/KADGenDriving.xsd")
            Dim grp As Windows.Forms.GroupBox
            Dim childXSDNode As Xml.XmlNode
            Dim childDataNode As Xml.XmlNode
            For Each xsdNode As Xml.XmlNode In xsdNodeList
               xTypeName = Utility.Tools.GetAttributeOrEmpty(xsdNode, "type")
               If xTypeName.Trim.Length > 0 Then
                  childXSDNode = xsdNode.SelectSingleNode( _
                           "//xs:complexType[@name='" & xTypeName & "']", xsdNsmgr)
                  childDataNode = xmlDataRow.SelectSingleNode("kg:" & xTypeName, _
                           dataNsmgr)
                  If Not childXSDNode Is Nothing Then
                     grp = New Windows.Forms.GroupBox
                     grp.Text = xTypeName
                     grp.Top = top + margin
                     grp.Width = parentControl.Width - (3 * margin)
                     grp.Tag = childDataNode
                     grp.Anchor = Windows.Forms.AnchorStyles.Top Or _
                           Windows.Forms.AnchorStyles.Left Or _
                           Windows.Forms.AnchorStyles.Right
                     parentControl.Controls.Add(grp)
                     top += Me.Scatter(grp, margin * 7, childDataNode, _
                           childXSDNode) + (3 * margin)
                  End If
               End If
            Next
         End If
      End If
      If top > 0 Then
         parentControl.Height = top + 2 * margin
      End If
      Return top
   End Function

   Private Function NewLabeledControl( _
                  ByVal parentControl As Windows.Forms.Control, _
                  ByVal name As String, _
                  ByVal type As String, _
                  ByVal special As String, _
                  ByVal desc As String, _
                  ByVal required As Boolean, _
                  ByVal top As Int32, _
                  ByVal value As String, _
                  ByVal extra As Object) As Windows.Forms.Control
      'The logic of this relies on three character prefixes
      Dim ctl As Windows.Forms.Control
      Dim lbl As Windows.Forms.Label
      Dim lblWidth As Int32 = 120
      Dim prefix As String
      Dim addLabel As Boolean
      Dim ctlLeft As Int32
      If Not special Is Nothing AndAlso special.Trim.Length > 0 Then
         Select Case special.ToLower
            Case "file"
               ctl = New FileTextBox
               CType(ctl, FileTextBox).LabelText = Utility.Tools.SpaceAtCaps(name)
               CType(ctl, FileTextBox).LabelWidth = lblWidth
               prefix = "ftb"
               addLabel = False
            Case "directory"
               ctl = New FolderTextBox
               CType(ctl, FolderTextBox).LabelText = _
                           Utility.Tools.SpaceAtCaps(name)
               CType(ctl, FolderTextBox).LabelWidth = lblWidth
               prefix = "ftb"
               addLabel = False
            Case "combobox"
               Dim entries() As String
               entries = CType(extra, String())
               Dim cbo As New Windows.Forms.ComboBox
               ctl = cbo
               For Each entry As String In entries
                  cbo.Items.Add(entry)
               Next
               prefix = "cbo"
               addLabel = True
               AddHandler cbo.SelectedIndexChanged, AddressOf ctl_Changed
            Case Else
               ctl = New Windows.Forms.TextBox
               prefix = "txt"
               addLabel = True
         End Select
      Else
         addLabel = True
         Select Case type.ToLower
            Case "int"
               ctl = New Windows.Forms.NumericUpDown
               prefix = "spn"
            Case "bool", "boolean"
               Dim chk As New Windows.Forms.CheckBox
               ctl = chk
               If value Is Nothing OrElse value.Length = 0 Then
                  CType(ctl, Windows.Forms.CheckBox).Checked = False
               Else
                  CType(ctl, Windows.Forms.CheckBox).Checked = Boolean.Parse(value)
               End If
               prefix = "chk"
               AddHandler chk.CheckedChanged, AddressOf ctl_Changed
            Case Else
               ctl = New Windows.Forms.TextBox
               prefix = "txt"
         End Select
      End If
      If addLabel Then
         lbl = New Windows.Forms.Label
         lbl.Text = Utility.Tools.SpaceAtCaps(name)
         lbl.Top = top
         lbl.Left = margin
         lbl.Width = lblWidth
         parentControl.Controls.Add(lbl)
         ctlLeft = lbl.Right
      Else
         ctlLeft = 2 * margin
      End If
      ToolTip.SetToolTip(ctl, desc)
      If required Then
         mrequiredControls.Add(ctl)
      End If
      ctl.Name = prefix & name
      If Not TypeOf ctl Is Windows.Forms.CheckBox Then
         ctl.Text = value
      End If
      ctl.Top = top
      ctl.Left = ctlLeft
      ctl.Width = parentControl.ClientRectangle.Width - ctl.Left - (3 * margin)
      ctl.Anchor = Windows.Forms.AnchorStyles.Top Or _
                           Windows.Forms.AnchorStyles.Left Or _
                           Windows.Forms.AnchorStyles.Right
      AddHandler ctl.TextChanged, AddressOf ctl_Changed
      AddHandler ctl.Leave, AddressOf ctl_Leave
      parentControl.Controls.Add(ctl)
      Return ctl
   End Function


   Private Sub ctl_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
      If mrequiredControls.Contains(sender) Then
         Dim ctl As Windows.Forms.Control = CType(sender, Windows.Forms.Control)
         If ctl.Text.Trim.Length = 0 Then
            ErrorProvider.SetError(ctl, "The field " & Utility.Tools.SpaceAtCaps(ctl.Name.Substring(3)) & " can not be empty")
         Else
            ErrorProvider.SetError(ctl, "")
         End If
      End If
   End Sub

End Class
