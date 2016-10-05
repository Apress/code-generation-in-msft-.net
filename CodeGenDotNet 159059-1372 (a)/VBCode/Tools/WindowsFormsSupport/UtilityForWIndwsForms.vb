' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Tools used only within Windows.Forms.

Option Strict On
Option Explicit On 

Imports System
Imports System.Diagnostics
Imports KADGen


Public Class UtilityForWIndwsForms

#Region "Fill Tree View Code"
   Public Shared Sub FillTreeWithXML(ByVal TreeView As Windows.Forms.TreeView, ByVal Node As Xml.XmlNode)
      Dim treeNode As Windows.Forms.TreeNode = MakeTreeNode(TreeView, Node, True)
      For Each nodeChild As Xml.XmlNode In Node.ChildNodes
         FillChildNodes(treeNode, nodeChild, True)
      Next
   End Sub

   Public Shared Sub FillChildNodes(ByVal treeParent As Windows.Forms.TreeNode, ByVal node As Xml.XmlNode, ByVal DefaultToChecked As Boolean)
      Dim treeNode As Windows.Forms.TreeNode = MakeTreeNode(treeParent, node, True)
      For Each nodeChild As Xml.XmlNode In node.ChildNodes
         If Not nodeChild.NodeType = Xml.XmlNodeType.Text Then
            FillChildNodes(treeNode, nodeChild, DefaultToChecked)
         End If
      Next
   End Sub

   Private Shared Function MakeTreeNode(ByVal TreeView As Windows.Forms.TreeView, ByVal node As Xml.XmlNode, ByVal DefaultCheckedValue As Boolean) As Windows.Forms.TreeNode
      Dim treeNode As Windows.Forms.TreeNode = TreeView.Nodes.Add(Utility.Tools.SpaceAtCaps(node.Name))
      SetTreeDefaults(treeNode, node, DefaultCheckedValue)
      treeNode.Tag = node
      Return treeNode
   End Function

   Private Shared Function MakeTreeNode(ByVal TreeParent As Windows.Forms.TreeNode, ByVal node As Xml.XmlNode, ByVal DefaultCheckedValue As Boolean) As Windows.Forms.TreeNode
      Dim treeNode As Windows.Forms.TreeNode = TreeParent.Nodes.Add(Utility.Tools.SpaceAtCaps(node.Name))
      SetTreeDefaults(treeNode, node, DefaultCheckedValue)
      Return treeNode
      treeNode.Tag = node
   End Function

   Private Shared Sub SetTreeDefaults(ByVal treeNode As Windows.Forms.TreeNode, ByVal node As Xml.XmlNode, ByVal DefaultCheckedValue As Boolean)
      If node.Attributes.GetNamedItem("Checked") Is Nothing Then
         treeNode.Checked = True
      ElseIf node.Attributes.GetNamedItem("Checked").Value.ToLower = "true" Then
         treeNode.Checked = True
      Else
         treeNode.Checked = True
      End If
   End Sub
#End Region

   Public Shared Function FindControlByName(ByVal controls As Windows.Forms.Control.ControlCollection, ByVal name As String) As Windows.Forms.Control
      Dim ctlChild As Windows.Forms.Control
      For Each ctl As Windows.Forms.Control In controls
         Debug.WriteLine(ctl.Name)
         If ctl.Name = name Then
            Return ctl
         End If
         ctlChild = FindControlByName(ctl.Controls, name)
         If Not ctlChild Is Nothing Then
            Return ctlChild
         End If
         'For Each childCtl As Windows.Forms.Control In ctl.Controls
         '   ctlChild = FindControlByName(childCtl.Controls, name)
         '   If Not ctlChild Is Nothing Then
         '      Return ctlChild
         '   End If
         'Next
      Next
   End Function

   Public Shared Sub FillAllSameWidth(ByVal container As Windows.Forms.Control, ByVal margin As Int32)
      Dim ctl As Windows.Forms.Control
      Dim width As Int32
      Dim left As Int32 = margin
      Dim cnt As Int32
      Dim ctls As New Collections.SortedList
      For Each ctl In container.Controls
         If ctl.Visible Then
            ctls.Add("B" & ctl.Left.ToString("00000") & ctl.GetHashCode.ToString("00000000"), ctl)
            cnt += 1
         End If
      Next
      width = container.Width \ cnt
      For Each dict As Collections.DictionaryEntry In ctls
         ctl = CType(dict.Value, Windows.Forms.Control)
         ctl.Bounds = New Drawing.Rectangle(left, margin, width, container.Height - 2 * margin)
         left += margin + width
      Next
   End Sub
End Class
