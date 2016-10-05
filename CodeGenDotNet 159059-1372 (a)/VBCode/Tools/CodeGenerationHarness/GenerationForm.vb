' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: The MDI child form that displays the tree and directive details 

Option Strict On
Option Explicit On 

Imports System
Imports KADGEn.WinFormSupport

'! Class Summary: 

Public Class GenerationForm
   Inherits WinFormSupport.SimpleTreeBase

#Region "Class level declarations"
   Private Delegate Function GenerateDelegate( _
                  ByVal MetadataFileName As String, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode, _
                  ByVal nodeSelect As Xml.XmlNode) _
                  As IO.Stream
#End Region

#Region " Windows Form Designer generated code "

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
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      '
      'Generation
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(608, 422)
      Me.Name = "Generation"
      Me.Text = "Generation"

   End Sub

#End Region

#Region "Event Handlers"
   Protected Overrides Sub OnLoad( _
                  ByVal e As System.EventArgs)
      Dim ctl As Windows.forms.Control = WinFormSupport.UtilityForWIndwsForms.FindControlByName(Me.Controls, "pnlTreeButtons")
      If Not ctl Is Nothing Then
         Dim newBtn As New Windows.Forms.Button
         newBtn.Text = "Run"
         newBtn.Name = "btnRun"
         newBtn.Left = 500
         AddHandler newBtn.Click, AddressOf btnRun_Click
         ctl.Controls.Add(newBtn)
         newBtn = New Windows.Forms.Button
         newBtn.Text = "Cancel"
         newBtn.Name = "btnCancel"
         newBtn.Left = 500
         AddHandler newBtn.Click, AddressOf btnCancel_Click
         ctl.Controls.Add(newBtn)
      End If
   End Sub

   Private Sub btnRun_Click( _
                  ByVal sender As System.Object, _
                  ByVal e As System.EventArgs)
      Dim gen As New KADGen.CodeGenerationSupport.Generation
      Dim cntDone As Int32
      Dim output As New Collections.ArrayList
      Dim sMsg As String
      Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
      If Me.HasChanges Then
         Me.Save()
      End If
      Dim logEntries As Collections.ArrayList = gen.RunGeneration(mxmldoc, mxsddoc, Me, cntDone, output, mxmlfilename)
      Dim frm As New OutputDisplay
      If logEntries.Count > 0 Then
         sMsg = "There were problems with the generation. Please review the 'Results' tab"
      ElseIf cntDone = 0 Then
         sMsg = "Nothing to do"
      Else
         sMsg = "Generation was successful"
      End If
      frm.Show(Me, output, sMsg)
      Me.GetDataGrid.DataSource = logEntries
      ' FUTURE: Store log to file
      Me.UpdateProgress(0)
      Cursor.Current = System.Windows.Forms.Cursors.Default
   End Sub

   Private Sub btnCancel_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs)
      mCancel = True
   End Sub

#End Region

#Region "Public Methods and Properties -empty"
#End Region

#Region "Protected and Friend Methods and Properties"
   Protected Overrides Function IsChecked( _
                  ByVal node As Xml.XmlNode) _
                  As Boolean
      Dim nsmgr As Xml.XmlNamespaceManager = GetNameSpaceManager(node.OwnerDocument, "kg")
      Dim nodeStandard As Xml.XmlNode = node.SelectSingleNode("kg:Standard", nsmgr)
      If nodeStandard Is Nothing Then
         Return False
      Else
         Dim attr As Xml.XmlNode = nodeStandard.Attributes.GetNamedItem("Checked")
         If attr Is Nothing Then
            attr = nodeStandard.Attributes.GetNamedItem("checked")
         End If
         If attr Is Nothing Then
            attr = nodeStandard.Attributes.GetNamedItem("CHECKED")
         End If
         If Not attr Is Nothing Then
            Return attr.Value = "true"
         End If
      End If
   End Function

   Protected Overrides Function TypeForTag(ByVal obj As Object) As System.Type
      Return GetType(GenerationDIrectiveUserControl)
   End Function

   Protected Overrides Function MakeName( _
                  ByVal node As Xml.XmlNode) _
                  As String
      Dim nsmgr As Xml.XmlNamespaceManager = GetNameSpaceManager(node.OwnerDocument, "kg")
      Dim nodeStandard As Xml.XmlNode = node.SelectSingleNode("kg:Standard", nsmgr)
      Dim nodename As String = Utility.Tools.SpaceAtCaps(Utility.Tools.StripNamespacePrefix(node.Name))
      Dim name As String
      If Not nodeStandard Is Nothing Then
         name = Utility.Tools.GetAttributeOrEmpty(nodeStandard, "Name")
         If name.Trim.Length > 0 Then
            nodename &= "  (" & Utility.Tools.SpaceAtCaps(name) & ")"
         End If
      End If
      If name Is Nothing OrElse name.Trim.Length = 0 Then
         nodename = Utility.Tools.StripNamespacePrefix(MyBase.MakeName(node))
      End If
      Return nodename
   End Function

   Protected Overrides Sub CheckForChecked( _
                  ByVal treenode As Windows.Forms.TreeNode, _
                  ByVal node As Xml.XmlNode)
      Dim nsmgr As Xml.XmlNamespaceManager = GetNameSpaceManager(node.OwnerDocument, "kg")
      Dim nodeStandard As Xml.XmlNode = node.SelectSingleNode("kg:Standard", nsmgr)
      If Not nodeStandard Is Nothing Then
         Dim attr As Xml.XmlAttribute = CType(nodeStandard.Attributes.GetNamedItem("Checked"), Xml.XmlAttribute)
         If Not attr Is Nothing Then
            If treenode.Checked Then
               attr.Value = "true"
            Else
               attr.Value = "false"
            End If
         End If
      End If
   End Sub

#End Region

#Region "Protected Event Response Methods - empty"
#End Region

#Region "Private Methods and Properties"
   Private Sub InitForm()
      mXMLStyle = WinFormSupport.SimpleTreeBase.XMLStyle.Free
   End Sub

   Private Shared Function GetNameSpaceManager( _
                  ByVal xmlDoc As Xml.XmlDocument, _
                  ByVal prefix As String) _
                  As Xml.XmlNamespaceManager
      Dim nsmgr As New Xml.XmlNamespaceManager(xmlDoc.NameTable)
      Dim ns As String
      Select Case prefix
         Case "kg"
            ns = "http://kadgen.com/KADGenDriving.xsd"
      End Select
      If Not ns Is Nothing Then
         nsmgr.AddNamespace(prefix, ns)
         Return nsmgr
      End If
   End Function
#End Region

End Class
