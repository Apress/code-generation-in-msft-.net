' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Base class for tree based forms

Option Strict On
Option Explicit On

Imports System

Public Class SimpleTreeBase
   Inherits System.Windows.Forms.Form
   Implements WinFormSupport.ISimpleForm
   Implements Utility.IProgressCallback

   Protected mCancel As Boolean
   Const vbcrlf As String = Microsoft.VisualBasic.ControlChars.CrLf

   Public Enum XMLStyle
      unknown
      ' DataSet - not currently supported
      Free
   End Enum

   Protected Structure menuStuff
      Public Text As String
      Public XSDNode As Xml.XmlNode
      Public Sub New(ByVal XSDNode As Xml.XmlNode, ByVal Text As String)
         Me.Text = Text
         Me.XSDNode = XSDNode
      End Sub
   End Structure

#Region "Class level declarations"
   Const margin As Int32 = 2
   Const thisNamespace As String = "http://kadgen.com/KADGenDriving.xsd"
   Protected mXMLDoc As New Xml.XmlDocument
   Protected mXSDDoc As Xml.XmlDocument
   Protected mXMLFileName As String
   Protected mXMLStyle As XMLStyle
   Protected mHasChanges As Boolean
   Protected mBaseMenus() As menuStuff
   Protected mChildMenus As Collections.Hashtable
   Protected mChildAutos As Collections.Hashtable
   Protected mCurrentTreeNode As Windows.Forms.TreeNode
   Protected mCurrentProcessNode As Xml.XmlNode
   Protected mCurrentProcessFile As String
   Protected mTreeNodeLoadDepth As Int32 ' Only used by recursive treenode load
#End Region

#Region " Windows Form Designer generated code"

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization inside the InitForm method
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
   Friend WithEvents Panel1 As System.Windows.Forms.Panel
   Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
   Friend WithEvents cmnuTreeView As System.Windows.Forms.ContextMenu
   Friend WithEvents pnlUserControl As System.Windows.Forms.Panel
   Friend WithEvents btnDelete As System.Windows.Forms.Button
   Friend WithEvents btnNew As System.Windows.Forms.Button
   Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
   Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
   Friend WithEvents cmnuTreeViewAdd As System.Windows.Forms.ContextMenu
   Friend WithEvents mnuTreeViewAdd As System.Windows.Forms.MenuItem
   Friend WithEvents mnuTreeViewDelete As System.Windows.Forms.MenuItem
   Friend WithEvents pnlTreeButtons As System.Windows.Forms.Panel
   Friend WithEvents TabControl As System.Windows.Forms.TabControl
   Friend WithEvents tbpInstructions As System.Windows.Forms.TabPage
   Friend WithEvents tbpResults As System.Windows.Forms.TabPage
   Friend WithEvents dataGridResults As System.Windows.Forms.DataGrid
   Friend WithEvents progressBar As System.Windows.Forms.ProgressBar
   Friend WithEvents Panel2 As System.Windows.Forms.Panel
   Friend WithEvents TreeView As System.Windows.Forms.TreeView
   Friend WithEvents btnCopy As System.Windows.Forms.Button
   Friend WithEvents treeImages As System.Windows.Forms.ImageList
   Friend WithEvents btnPaste As System.Windows.Forms.Button
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SimpleTreeBase))
      Me.Panel1 = New System.Windows.Forms.Panel
      Me.Panel2 = New System.Windows.Forms.Panel
      Me.TreeView = New System.Windows.Forms.TreeView
      Me.cmnuTreeView = New System.Windows.Forms.ContextMenu
      Me.mnuTreeViewAdd = New System.Windows.Forms.MenuItem
      Me.mnuTreeViewDelete = New System.Windows.Forms.MenuItem
      Me.treeImages = New System.Windows.Forms.ImageList(Me.components)
      Me.progressBar = New System.Windows.Forms.ProgressBar
      Me.pnlTreeButtons = New System.Windows.Forms.Panel
      Me.btnPaste = New System.Windows.Forms.Button
      Me.btnCopy = New System.Windows.Forms.Button
      Me.btnDelete = New System.Windows.Forms.Button
      Me.btnNew = New System.Windows.Forms.Button
      Me.cmnuTreeViewAdd = New System.Windows.Forms.ContextMenu
      Me.Splitter1 = New System.Windows.Forms.Splitter
      Me.pnlUserControl = New System.Windows.Forms.Panel
      Me.TabControl = New System.Windows.Forms.TabControl
      Me.tbpInstructions = New System.Windows.Forms.TabPage
      Me.tbpResults = New System.Windows.Forms.TabPage
      Me.dataGridResults = New System.Windows.Forms.DataGrid
      Me.MenuItem3 = New System.Windows.Forms.MenuItem
      Me.MenuItem4 = New System.Windows.Forms.MenuItem
      Me.Panel1.SuspendLayout()
      Me.Panel2.SuspendLayout()
      Me.pnlTreeButtons.SuspendLayout()
      Me.pnlUserControl.SuspendLayout()
      Me.TabControl.SuspendLayout()
      Me.tbpResults.SuspendLayout()
      CType(Me.dataGridResults, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'Panel1
      '
      Me.Panel1.Controls.Add(Me.Panel2)
      Me.Panel1.Controls.Add(Me.progressBar)
      Me.Panel1.Controls.Add(Me.pnlTreeButtons)
      Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
      Me.Panel1.Location = New System.Drawing.Point(0, 0)
      Me.Panel1.Name = "Panel1"
      Me.Panel1.Size = New System.Drawing.Size(225, 326)
      Me.Panel1.TabIndex = 0
      '
      'Panel2
      '
      Me.Panel2.Controls.Add(Me.TreeView)
      Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
      Me.Panel2.Location = New System.Drawing.Point(0, 0)
      Me.Panel2.Name = "Panel2"
      Me.Panel2.Size = New System.Drawing.Size(225, 286)
      Me.Panel2.TabIndex = 5
      '
      'TreeView
      '
      Me.TreeView.ContextMenu = Me.cmnuTreeView
      Me.TreeView.Dock = System.Windows.Forms.DockStyle.Fill
      Me.TreeView.FullRowSelect = True
      Me.TreeView.HideSelection = False
      Me.TreeView.ImageList = Me.treeImages
      Me.TreeView.Location = New System.Drawing.Point(0, 0)
      Me.TreeView.Name = "TreeView"
      Me.TreeView.Size = New System.Drawing.Size(225, 286)
      Me.TreeView.TabIndex = 1
      '
      'cmnuTreeView
      '
      Me.cmnuTreeView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuTreeViewAdd, Me.mnuTreeViewDelete})
      '
      'mnuTreeViewAdd
      '
      Me.mnuTreeViewAdd.Index = 0
      Me.mnuTreeViewAdd.Text = "Add"
      '
      'mnuTreeViewDelete
      '
      Me.mnuTreeViewDelete.Index = 1
      Me.mnuTreeViewDelete.Text = "Delete"
      '
      'treeImages
      '
      Me.treeImages.ImageSize = New System.Drawing.Size(16, 16)
      Me.treeImages.ImageStream = CType(resources.GetObject("treeImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
      Me.treeImages.TransparentColor = System.Drawing.Color.Transparent
      '
      'progressBar
      '
      Me.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom
      Me.progressBar.Location = New System.Drawing.Point(0, 286)
      Me.progressBar.Name = "progressBar"
      Me.progressBar.Size = New System.Drawing.Size(225, 16)
      Me.progressBar.TabIndex = 4
      '
      'pnlTreeButtons
      '
      Me.pnlTreeButtons.Controls.Add(Me.btnPaste)
      Me.pnlTreeButtons.Controls.Add(Me.btnCopy)
      Me.pnlTreeButtons.Controls.Add(Me.btnDelete)
      Me.pnlTreeButtons.Controls.Add(Me.btnNew)
      Me.pnlTreeButtons.Dock = System.Windows.Forms.DockStyle.Bottom
      Me.pnlTreeButtons.Location = New System.Drawing.Point(0, 302)
      Me.pnlTreeButtons.Name = "pnlTreeButtons"
      Me.pnlTreeButtons.Size = New System.Drawing.Size(225, 24)
      Me.pnlTreeButtons.TabIndex = 2
      '
      'btnPaste
      '
      Me.btnPaste.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnPaste.Location = New System.Drawing.Point(168, 0)
      Me.btnPaste.Name = "btnPaste"
      Me.btnPaste.Size = New System.Drawing.Size(56, 24)
      Me.btnPaste.TabIndex = 3
      Me.btnPaste.Text = "Paste"
      Me.btnPaste.Visible = False
      '
      'btnCopy
      '
      Me.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnCopy.Location = New System.Drawing.Point(112, 0)
      Me.btnCopy.Name = "btnCopy"
      Me.btnCopy.Size = New System.Drawing.Size(56, 24)
      Me.btnCopy.TabIndex = 2
      Me.btnCopy.Text = "Copy"
      Me.btnCopy.Visible = False
      '
      'btnDelete
      '
      Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnDelete.Location = New System.Drawing.Point(56, 0)
      Me.btnDelete.Name = "btnDelete"
      Me.btnDelete.Size = New System.Drawing.Size(56, 24)
      Me.btnDelete.TabIndex = 1
      Me.btnDelete.Text = "Delete"
      '
      'btnNew
      '
      Me.btnNew.ContextMenu = Me.cmnuTreeViewAdd
      Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnNew.Location = New System.Drawing.Point(0, 0)
      Me.btnNew.Name = "btnNew"
      Me.btnNew.Size = New System.Drawing.Size(56, 24)
      Me.btnNew.TabIndex = 0
      Me.btnNew.Text = "New >"
      '
      'cmnuTreeViewAdd
      '
      '
      'Splitter1
      '
      Me.Splitter1.Location = New System.Drawing.Point(225, 0)
      Me.Splitter1.Name = "Splitter1"
      Me.Splitter1.Size = New System.Drawing.Size(3, 326)
      Me.Splitter1.TabIndex = 1
      Me.Splitter1.TabStop = False
      '
      'pnlUserControl
      '
      Me.pnlUserControl.ContextMenu = Me.cmnuTreeViewAdd
      Me.pnlUserControl.Controls.Add(Me.TabControl)
      Me.pnlUserControl.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pnlUserControl.Location = New System.Drawing.Point(228, 0)
      Me.pnlUserControl.Name = "pnlUserControl"
      Me.pnlUserControl.Size = New System.Drawing.Size(252, 326)
      Me.pnlUserControl.TabIndex = 2
      '
      'TabControl
      '
      Me.TabControl.Controls.Add(Me.tbpInstructions)
      Me.TabControl.Controls.Add(Me.tbpResults)
      Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
      Me.TabControl.Location = New System.Drawing.Point(0, 0)
      Me.TabControl.Name = "TabControl"
      Me.TabControl.SelectedIndex = 0
      Me.TabControl.Size = New System.Drawing.Size(252, 326)
      Me.TabControl.TabIndex = 1
      '
      'tbpInstructions
      '
      Me.tbpInstructions.Location = New System.Drawing.Point(4, 22)
      Me.tbpInstructions.Name = "tbpInstructions"
      Me.tbpInstructions.Size = New System.Drawing.Size(244, 300)
      Me.tbpInstructions.TabIndex = 0
      Me.tbpInstructions.Text = "Instructions"
      '
      'tbpResults
      '
      Me.tbpResults.Controls.Add(Me.dataGridResults)
      Me.tbpResults.Location = New System.Drawing.Point(4, 22)
      Me.tbpResults.Name = "tbpResults"
      Me.tbpResults.Size = New System.Drawing.Size(244, 300)
      Me.tbpResults.TabIndex = 1
      Me.tbpResults.Text = "Results"
      '
      'dataGridResults
      '
      Me.dataGridResults.DataMember = ""
      Me.dataGridResults.Dock = System.Windows.Forms.DockStyle.Fill
      Me.dataGridResults.HeaderForeColor = System.Drawing.SystemColors.ControlText
      Me.dataGridResults.Location = New System.Drawing.Point(0, 0)
      Me.dataGridResults.Name = "dataGridResults"
      Me.dataGridResults.Size = New System.Drawing.Size(244, 300)
      Me.dataGridResults.TabIndex = 1
      '
      'MenuItem3
      '
      Me.MenuItem3.Index = -1
      Me.MenuItem3.Text = ""
      '
      'MenuItem4
      '
      Me.MenuItem4.Index = -1
      Me.MenuItem4.Text = ""
      '
      'SimpleTreeBase
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(480, 326)
      Me.Controls.Add(Me.pnlUserControl)
      Me.Controls.Add(Me.Splitter1)
      Me.Controls.Add(Me.Panel1)
      Me.Name = "SimpleTreeBase"
      Me.Text = "SimpleTreeBase"
      Me.Panel1.ResumeLayout(False)
      Me.Panel2.ResumeLayout(False)
      Me.pnlTreeButtons.ResumeLayout(False)
      Me.pnlUserControl.ResumeLayout(False)
      Me.TabControl.ResumeLayout(False)
      Me.tbpResults.ResumeLayout(False)
      CType(Me.dataGridResults, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region "Event Handlers"
   Protected Overrides Sub OnActivated( _
                  ByVal e As System.EventArgs)
      If TypeOf Me.MdiParent Is ISimpleMDIParent Then
         CType(Me.MdiParent, ISimpleMDIParent).EnableSave(HasChanges)
         CType(Me.MdiParent, ISimpleMDIParent).EnableClose(True)
      End If
   End Sub

   Private Sub btnNew_Click( _
                  ByVal sender As System.Object, _
                  ByVal e As System.EventArgs) _
                  Handles btnNew.Click
      OnButtonNewClick(sender, e)
   End Sub

   Private Sub btnCopy_Click( _
                  ByVal sender As System.Object, _
                  ByVal e As System.EventArgs) _
                  Handles btnCopy.Click
      'OnButtonCopyClick(sender, e)
      OnNodeCopy()
   End Sub

   Private Sub btnPaste_Click( _
                  ByVal sender As System.Object, _
                  ByVal e As System.EventArgs) _
                  Handles btnPaste.Click
      OnNodePaste()
   End Sub

   Private Sub btnDelete_Click( _
                  ByVal sender As System.Object, _
                  ByVal e As System.EventArgs) _
                  Handles btnDelete.Click
      DeleteTreeNode(CType(TreeView.SelectedNode, TreeNodeForXML))
   End Sub

   Private Sub pnlTreeButtons_Layout( _
                  ByVal sender As Object, _
                  ByVal e As System.Windows.Forms.LayoutEventArgs) _
                  Handles pnlTreeButtons.Layout
      OnPanelTreeViewButtonsLayout(sender, e)
   End Sub

   Private Sub TreeView_AfterSelect( _
                     ByVal sender As System.Object, _
                     ByVal e As System.Windows.Forms.TreeViewEventArgs) _
                     Handles TreeView.AfterSelect
      OnTreeViewAfterSelect(sender, e)
   End Sub

   Private Sub TreeView_MouseDown( _
                    ByVal sender As System.Object, _
                    ByVal e As System.Windows.Forms.MouseEventArgs) _
                    Handles TreeView.MouseDown
      ' When the user puts mousedown on this node for any reason, 
      ' save it for possible use by a context menu
      Dim treeNode As Windows.Forms.TreeNode
      treeNode = TreeView.GetNodeAt(e.X, e.Y)
      If Not treeNode Is Nothing Then
         mCurrentTreeNode = treeNode
      End If
   End Sub

   Private Sub cmnuTreeViewAdd_Popup( _
                  ByVal sender As Object, _
                  ByVal e As System.EventArgs) _
                  Handles cmnuTreeViewAdd.Popup, mnuTreeViewAdd.Popup
      OnTreeViewAddPopup(sender, e)
   End Sub

   Private Sub cmnuTreeView_Click( _
                  ByVal sender As Object, _
                  ByVal e As System.EventArgs) _
                  Handles mnuTreeViewDelete.Click
      DeleteTreeNode(CType(mCurrentTreeNode, TreeNodeForXML))
   End Sub

   Private Sub TreeView_KeyUp( _
                  ByVal sender As Object, _
                  ByVal e As System.Windows.Forms.KeyEventArgs) _
                  Handles TreeView.KeyUp
      If e.Control Then
         Select Case e.KeyCode
            Case Windows.Forms.Keys.C
               OnNodeCopy()
            Case Windows.Forms.Keys.V
               OnNodePaste()
         End Select
      End If
   End Sub

   Private Sub TreeView_Click( _
                  ByVal sender As Object, _
                  ByVal e As System.EventArgs) _
                  Handles TreeView.Click
      Dim pt As Drawing.Point = TreeView.PointToClient(Me.MousePosition)
      Dim treeNode As Windows.Forms.TreeNode = TreeView.GetNodeAt(pt)
      If Not treeNode Is Nothing Then
         ' We should always get here.
         If pt.X < treeNode.Bounds.X And pt.X >= treeNode.Bounds.X - 16 Then
            Dim xmlNode As Xml.XmlNode = CType(treeNode, TreeNodeForXML).XMLDataNode
            Dim nodeSchema As Xml.XmlNode = CType(treeNode, TreeNodeForXML).XMLSchemaNode
            Dim attr As Xml.XmlNode = xmlNode.ChildNodes(0).Attributes.GetNamedItem("Checked")
            Dim nsmgr As New Xml.XmlNamespaceManager(New Xml.NameTable)
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")
            nsmgr.AddNamespace("kd", "http://kadgen.com/KADGenDrivingx.xsd")
            If Not attr Is Nothing Then
               If attr.Value = "true" Then
                  attr.Value = "false"
               Else
                  attr.Value = "true"
               End If
               If treeNode Is TreeView.SelectedNode Then
                  If Me.pnlUserControl.Controls.Count > 1 Then
                     Throw New Exception("There is more than one control in the right panel")
                  Else
                     Dim tabControl As Windows.Forms.TabControl = CType(Me.pnlUserControl.Controls(0), Windows.Forms.TabControl)
                     If TypeOf tabControl.TabPages(0).Controls(0) Is SimpleXMLNodeUserControl Then
                        CType(tabControl.TabPages(0).Controls(0), SimpleXMLNodeUserControl).MarkAsChecked(attr.Value = "true")
                     End If
                  End If
               End If
               'type(treenode,TreeNodeForXML).
               MarkChecked(treeNode, nodeSchema, xmlNode, nsmgr)
               HasChanges = True
            End If
         End If
      End If
   End Sub

#End Region

#Region "Public Methods and Properties"
   Public Overloads Sub Show( _
                  ByVal xmlFileName As String) _
                  Implements WinFormSupport.ISimpleForm.Show
      LoadFile(xmlFileName)
      Me.Text = "Generation Script - " & IO.Path.GetFileName(xmlFileName)
      HasChanges = False
      mCurrentTreeNode = TreeView.Nodes(0)
   End Sub

   Public Overloads Sub Show( _
                  ByVal xmlDoc As Xml.XmlDocument, _
                  ByVal path As String) _
                  Implements WinFormSupport.ISimpleForm.Show
      LoadFile(xmlDoc, "", "", path)
   End Sub

   Public ReadOnly Property FileName() As String Implements ISimpleForm.FileName
      Get
         Return mXMLFileName
      End Get
   End Property

   Public Sub Save() Implements ISimpleForm.Save
      SaveFile()
   End Sub

   Public Sub Save( _
                  ByVal fileName As String) Implements ISimpleForm.Save
      SaveFile(fileName)
   End Sub

   Public Sub UpdateProgress( _
               ByVal complete As Int32) _
               Implements Utility.IProgressCallback.UpdateProgress
      progressBar.Value = complete
   End Sub

   Public Sub UpdateCurrentNode( _
               ByVal node As System.Xml.XmlNode) _
               Implements Utility.IProgressCallback.UpdateCurrentNode
      mCurrentProcessNode = node
   End Sub

   Public Function GetCancel() As Boolean _
               Implements Utility.IProgressCallback.GetCancel
      Return mCancel
   End Function

   Public Sub ResetCancel() Implements Utility.IProgressCallback.ResetCancel
      mCancel = False
   End Sub

   Public ReadOnly Property CurrentProcessNode() As Xml.XmlNode
      Get
         Return mCurrentProcessNode
      End Get
   End Property

   Public Sub UpdateCurrentFile(ByVal file As String) Implements Utility.IProgressCallback.UpdateCurrentFile
      mCurrentProcessFile = file
   End Sub

   Public ReadOnly Property CurrentProcessFile() As String
      Get
         Return mCurrentProcessFile
      End Get
   End Property

#End Region

#Region "Protected and Friend Methods and Properties"
   Protected Function GetDataGrid() As System.Windows.Forms.DataGrid
      Return dataGridResults
   End Function

   Protected Overridable Function TypeForTag( _
                  ByVal obj As Object) _
                  As System.Type
      If TypeOf obj Is Data.DataTable Then
         ' Display a listview
      ElseIf TypeOf obj Is Data.DataRow Then
         ' See if there is a user control by this name and load it if 
         ' available using reflection
         Dim dt As Data.DataTable
         Dim row As Data.DataRow = CType(obj, Data.DataRow)
         Dim type As System.Type

         dt = row.Table
         type = Me.GetType.Assembly.GetType(Me.GetType.Namespace & "." & _
                           dt.TableName & "Control")
         Return type
      ElseIf TypeOf obj Is Xml.XmlNode Then
         Dim node As Xml.XmlNode = CType(obj, Xml.XmlNode)
         Return GetType(SimpleXMLNodeUserControl)
      End If
   End Function

   Protected Overridable Function ControlForTag( _
                  ByVal obj As Object) _
                  As Object
      Dim type As System.Type
      Dim constructorInfo As Reflection.ConstructorInfo
      Dim retObj As Object
      type = TypeForTag(obj)
      If Not type Is Nothing Then
         ' We need to instantiate it here
         constructorInfo = type.GetConstructor(System.Type.EmptyTypes)
         retObj = constructorInfo.Invoke(New Object() {})
      End If
      Return retObj
   End Function

   Protected Sub GatherAndRemoveExistingUserControl()
      If tbpInstructions.Controls.Count > 0 Then
         Me.Gather()
         tbpInstructions.Controls.RemoveAt(0)
      End If
   End Sub

   Protected Overridable Sub LoadFile( _
                  ByVal xmlFileName As String)
      Dim xmlDoc As New Xml.XmlDocument
      Dim textReader As Xml.XmlTextReader
      Dim validReader As Xml.XmlValidatingReader
      mXSDDoc = Nothing
      Try
         mXMLFileName = xmlFileName
         textReader = New Xml.XmlTextReader(xmlFileName)
         validReader = New Xml.XmlValidatingReader(textReader)
         validReader.ValidationType = Xml.ValidationType.Schema
         Select Case mXMLStyle
            Case XMLStyle.Free
               Try
                  xmlDoc.Load(validReader)
               Catch ex As System.Exception
                  Throw New System.Exception("Could not validate Harness XML file" & _
                                 vbcrlf & vbcrlf & "More Information: " & _
                                 vbcrlf & vbcrlf & ex.message)
               End Try
               'xmlDoc.Load(xmlFileName)
               LoadFile(xmlDoc, "", "", IO.Path.GetDirectoryName(xmlFileName))
            Case Else
               Return
         End Select

      Catch ex As System.Exception
         Throw
      Finally
         textReader.Close()
      End Try

   End Sub

   Protected Overridable Sub LoadFile( _
                  ByVal xmlDoc As Xml.XmlDocument, _
                  ByVal xFilepath As String, _
                  ByVal basePath As String, _
                  ByVal path As String)
      Dim node As Xml.XmlNode
      Dim fileName As String
      Dim fullFileName As String
      Dim xfileFullPath As String
      Dim nsmgrXSD As New Xml.XmlNamespaceManager(New Xml.NameTable)
      nsmgrXSD.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")
      nsmgrXSD.AddNamespace("kd", "http://kadgen.com/KADGenDrivingx.xsd")
      Dim nsmgrXML As Xml.XmlNamespaceManager = _
                  Utility.xmlHelpers.BuildNamespacesManagerForDoc(xmlDoc)
      mXMLDoc = xmlDoc
      node = mXMLDoc.SelectSingleNode("kg:GenerationScript", nsmgrXML)
      If mXMLDoc.ChildNodes.Count > 1 Then
         If Not node.Attributes.GetNamedItem("schemaLocation", _
                        "http://www.w3.org/2001/XMLSchema-instance") _
                        Is Nothing Then
            Dim temp() As String
            temp = node.Attributes.GetNamedItem("schemaLocation", _
                        "http://www.w3.org/2001/XMLSchema-instance"). _
                              Value.Split(" "c)
            fullFileName = temp(1)
            'ElseIf Not node.Attributes.GetNamedItem("xmlns" & prefix) Is Nothing Then
            '   fileName = node.Attributes.GetNamedItem("xmlns" & prefix).Value
            '   ' Look first in local directory, then in the XFile directory
            '   If IO.Path.GetExtension(fileName).ToLower = ".xsd" Then
            '      fullFileName = IO.Path.Combine(path, IO.Path.GetFileName(fileName))
            '   End If
         End If
         If Not fullFileName Is Nothing Then
            If IO.File.Exists(fullFileName) Then
               OpenXSD(fullFileName)
            Else
               xfileFullPath = Utility.Tools.FixPath(xFilepath, _
                     basePath, Nothing, Nothing)
               fullFileName = IO.Path.Combine(xfileFullPath, _
                     IO.Path.GetFileName(fullFileName))
               If IO.File.Exists(fullFileName) Then
                  OpenXSD(fullFileName)
               End If
            End If
         End If
         LoadFreeXML(nsmgrXSD)
      End If

   End Sub

   Private Sub OpenXSD(ByVal filename As String)
      Try
         mXSDDoc = New Xml.XmlDocument
         mXSDDoc.Load(filename)
      Catch ex As Exception
         ' Only throw exception if file exists and we can not open it
         Throw
      End Try
   End Sub

   Protected Overridable Sub LoadFreeXML(ByVal nsmgr As Xml.XmlNamespaceManager)
      LoadFreeXMLData(nsmgr)
      LoadTreeMenuXSD(nsmgr)
      Me.Show()
   End Sub

   Protected Sub LoadFreeXMLData(ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim node As Xml.XmlNode

      mTreeNodeLoadDepth = 0
      For Each node In mXMLDoc.ChildNodes
         If node.NodeType = Xml.XmlNodeType.Element Then
            Exit For
         End If
      Next
      If Not node Is Nothing Then
         LoadFreeXMLDataNode(node, TreeView.Nodes, nsmgr)
      End If
   End Sub

   Protected Sub LoadFreeXMLDataNode( _
                     ByVal node As Xml.XmlNode, _
                     ByVal treenodelist As Windows.Forms.TreeNodeCollection, _
                     ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim treenode As TreeNodeForXML
      Me.mTreeNodeLoadDepth += 1
      treenode = MakeTreeNode(treenodelist, node, mTreeNodeLoadDepth <= 2, nsmgr)
      Dim xmlSchemaNode As Xml.XmlNode = _
                  Utility.xmlHelpers.GetSchemaForNode( _
                        node.LocalName, mXSDDoc)
      If Utility.Tools.GetAttributeOrEmpty(xmlSchemaNode, "", "kd:NoChildrenInTree", nsmgr) <> "true" Then
         For Each childNode As Xml.XmlNode In node.ChildNodes
            If childNode.NodeType = Xml.XmlNodeType.Element Then
               LoadFreeXMLDataNode(childNode, treenode.Nodes, nsmgr)
            End If
         Next
      End If
      Me.mTreeNodeLoadDepth -= 1
   End Sub


   Protected Sub LoadTreeMenuXSD(ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim baseNode As Xml.XmlNode

      If Not mXSDDoc Is Nothing Then
         mChildMenus = New Collections.Hashtable
         mChildAutos = New Collections.Hashtable
         baseNode = mXSDDoc.ChildNodes(1).ChildNodes(0)
         mBaseMenus = GetMenuInfo(baseNode, nsmgr)
         'GetAutoInfo(baseNode, nsmgr)
         'add a dummy entry so the menu will open
         mnuTreeViewAdd.MenuItems.Add("Fred")
      End If
   End Sub

   Protected Sub Gather()
      If tbpInstructions.Controls.Count > 0 Then
         If TypeOf tbpInstructions.Controls(0) Is ISimpleUserControl Then
            CType(tbpInstructions.Controls(0), _
                           WinFormSupport.ISimpleUserControl).Gather()
         End If
      End If
   End Sub

   Protected Overridable Sub SaveFile()
      Me.Gather()
      Me.SaveFile(mXMLFileName)
   End Sub

   Protected Overridable Sub SaveFile( _
                  ByVal fileName As String)
      Select Case mXMLStyle
         Case XMLStyle.Free
            mXMLDoc.Save(fileName)
            HasChanges = False
      End Select
   End Sub

   Protected Overridable Property HasChanges() _
                  As Boolean Implements ISimpleForm.HasChanges
      Get
         Return mHasChanges
      End Get
      Set(ByVal Value As Boolean)
         mHasChanges = Value
         Me.EnableSave(Value)
      End Set
   End Property

   Protected Sub EnableSave( _
                  ByVal enable As Boolean) _
                  Implements ISimpleForm.EnableSave
      If TypeOf Me.MdiParent Is ISimpleMDIParent Then
         CType(Me.MdiParent, ISimpleMDIParent).EnableSave(enable)
      End If
   End Sub

   Protected Sub EnableClose( _
                  ByVal enable As Boolean)
      If TypeOf Me.MdiParent Is ISimpleMDIParent Then
         CType(Me.MdiParent, ISimpleMDIParent).EnableClose(enable)
      End If
   End Sub

#End Region

#Region "Protected Event Response Methods"
   Protected Overridable Sub OnPanelTreeViewButtonsLayout( _
                  ByVal sender As Object, _
                  ByVal e As System.Windows.Forms.LayoutEventArgs)
      ' If we let this happen in design mode it is hard to add child buttons
      ' because parent forms are instantiated duing child form design
      If Not Me.DesignMode Then
         UtilityForWIndwsForms.FillAllSameWidth(pnlTreeButtons, 0)
      End If
   End Sub

   Protected Overridable Sub OnTreeViewAfterSelect( _
                  ByVal sender As Object, _
                  ByVal e As System.Windows.Forms.TreeViewEventArgs)
      Dim obj As Object
      Dim nodeData As Xml.XmlNode
      Dim nodeSchema As Xml.XmlNode
      If TypeOf e.Node Is TreeNodeForXML Then
         obj = ControlForTag(CType(e.Node, TreeNodeForXML).XMLDataNode)
         nodeData = CType(e.Node, TreeNodeForXML).XMLDataNode
         nodeSchema = CType(e.Node, TreeNodeForXML).XMLSchemaNode
      Else
         obj = ControlForTag(e.Node.Tag)
         If TypeOf e.Node.Tag Is Xml.XmlNode Then
            nodeData = CType(e.Node.Tag, Xml.XmlNode)
         End If
      End If
      If TypeOf obj Is Windows.Forms.Control Then
         If TypeOf obj Is SimpleXMLNodeUserControl Then
            CType(obj, SimpleXMLNodeUserControl).Scatter(nodeData, nodeSchema)
         ElseIf TypeOf obj Is WinFormSupport.ISimpleUserControl Then
            CType(obj, WinFormSupport.ISimpleUserControl).Scatter(nodeData)
         End If
         GatherAndRemoveExistingUserControl()
         tbpInstructions.Controls.Add(CType(obj, Windows.Forms.UserControl))
      End If
   End Sub

   Protected Overridable Sub OnButtonNewClick( _
                  ByVal sender As Object, _
                  ByVal e As System.EventArgs)
      FillAddMenu(TreeView.SelectedNode, cmnuTreeViewAdd)
      cmnuTreeViewAdd.Show(btnNew, New Drawing.Point(btnNew.Width, 0))
   End Sub


   Protected Overridable Sub OnButtonCopyClick( _
                  ByVal sender As Object, _
                  ByVal e As System.EventArgs)
      Dim tx As TreeNodeForXML
      Dim nodeNew As Xml.XmlNode
      ' TODO: Add check that its legal to copy this node based on an XSD attribute
      If Not TreeView.SelectedNode Is Nothing Then
         If TypeOf TreeView.SelectedNode Is TreeNodeForXML Then
            tx = CType(TreeView.SelectedNode, TreeNodeForXML)
            If Not tx.XMLDataNode Is Nothing Then
               nodeNew = tx.XMLDataNode.Clone
               tx.XMLDataNode.ParentNode.InsertAfter(nodeNew, tx.XMLDataNode)
               Reset(nodeNew)
            End If
         End If
      End If
   End Sub

   Protected Overridable Sub OnNodeCopy()
      Dim tx As TreeNodeForXML
      Dim nodeNew As Xml.XmlNode
      Dim s As String
      ' TODO: Add check that its legal to copy this node based on an XSD attribute
      If Not TreeView.SelectedNode Is Nothing Then
         If TypeOf TreeView.SelectedNode Is TreeNodeForXML Then
            tx = CType(TreeView.SelectedNode, TreeNodeForXML)
            If Not tx.XMLDataNode Is Nothing Then
               Windows.Forms.Clipboard.SetDataObject(tx.XMLDataNode.OuterXml)
            End If
         End If
      End If
   End Sub

   Protected Overridable Sub OnNodePaste()
      Dim s As String = CStr(Windows.Forms.Clipboard.GetDataObject().GetData(GetType(System.String)))
      If Not TreeView.SelectedNode Is Nothing Then
         If TypeOf TreeView.SelectedNode Is TreeNodeForXML Then
            Dim tx As TreeNodeForXML = CType(TreeView.SelectedNode, TreeNodeForXML)
            Dim xmlNode As Xml.XmlNode = tx.XMLDataNode
            Dim reader As New Xml.XmlTextReader(s, Xml.XmlNodeType.Element, Nothing)

            Dim newNode As Xml.XmlNode = mXMLDoc.ReadNode(reader)
            xmlNode.ParentNode.InsertAfter(newNode, xmlNode)
            Reset(newNode)
            Me.HasChanges = True
         End If
      End If
   End Sub

   Protected Overridable Sub OnTreeViewAddPopup( _
                  ByVal sender As Object, _
                  ByVal e As System.EventArgs)
      FillAddMenu(mCurrentTreeNode, mnuTreeViewAdd)
   End Sub

   Protected Overridable Sub CheckForChecked( _
                  ByVal treenode As Windows.Forms.TreeNode, _
                  ByVal node As Xml.XmlNode)
      Dim attr As Xml.XmlAttribute = CType(node.Attributes.GetNamedItem("Checked"), Xml.XmlAttribute)
      If Not attr Is Nothing Then
         If treenode.Checked Then
            attr.Value = "true"
         Else
            attr.Value = "false"
         End If
      End If
   End Sub

   Protected Overridable Function IsChecked( _
                  ByVal xmlNode As Xml.XmlNode) _
                  As Boolean
      Dim attr As Xml.XmlNode = xmlNode.Attributes.GetNamedItem("Checked")
      If attr Is Nothing Then
         attr = xmlNode.Attributes.GetNamedItem("checked")
      End If
      If attr Is Nothing Then
         attr = xmlNode.Attributes.GetNamedItem("CHECKED")
      End If
      If Not attr Is Nothing Then
         Return attr.Value = "true"
      End If
   End Function

   Protected Overridable Function CanCheck( _
                  ByVal xsdNode As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As Boolean
      Return Utility.Tools.GetAttributeOrEmpty(xsdNode, "", "kd:CanCheck", nsmgr) = "true"
   End Function

   Protected Overridable Function MakeName( _
                  ByVal node As Xml.XmlNode) _
                  As String
      Dim name As String
      name = Utility.Tools.GetAttributeOrEmpty(node, "Name")
      If name.Trim.Length = 0 Then
         name = Utility.Tools.GetAttributeOrEmpty(node, "name")
      End If
      If name.Trim.Length > 0 Then
         Return Utility.Tools.SpaceAtCaps(node.Name) & "  (" & Utility.Tools.SpaceAtCaps(name) & ")"
      Else
         Return Utility.Tools.SpaceAtCaps(node.Name)
      End If
   End Function
#End Region

#Region "Private Methods and Properties"
   Private Sub InitForm()
   End Sub

   Private Sub Reset(ByVal node As Xml.XmlNode)
      Dim tnx As TreeNodeForXML
      TreeView.Nodes.Clear()
      LoadFile(mXMLDoc, "", "", mXMLFileName)
      If Not node Is Nothing Then
         tnx = FindXMLNode(TreeView.Nodes, node)
         If Not tnx Is Nothing Then
            TreeView.SelectedNode = tnx
         End If
      End If
   End Sub

   Private Sub Reset()
      ' OK, this is really ugly
      Dim node As Xml.XmlNode
      If TypeOf TreeView.SelectedNode Is TreeNodeForXML Then
         node = CType(TreeView.SelectedNode, TreeNodeForXML).XMLDataNode
      End If
      Reset(node)
   End Sub

   Private Function FindXMLNode( _
                     ByVal nodes As Windows.Forms.TreeNodeCollection, _
                     ByVal searchFor As Xml.XmlNode) _
                     As TreeNodeForXML
      Dim tnx As TreeNodeForXML
      For Each tn As Windows.Forms.TreeNode In nodes
         If TypeOf tn Is TreeNodeForXML Then
            tnx = CType(tn, TreeNodeForXML)
            If tnx.XMLDataNode Is searchFor Then
               Return tnx
            End If
         End If
         tnx = FindXMLNode(tn.Nodes, searchFor)
         If Not tnx Is Nothing Then
            Return tnx
         End If
      Next
   End Function

   Private Sub AddItemClick( _
                 ByVal sender As Object, _
                 ByVal e As System.EventArgs)
      Dim treeNode As TreeNodeForXML
      Dim mnu As MenuItemWithXSD = CType(sender, MenuItemWithXSD)
      Dim xmlDataNode As Xml.XmlNode
      Dim xsdNode As Xml.XmlNode = mnu.XSDNode
      Dim canAdd As Boolean
      If TypeOf sender Is MenuItemWithXSD Then
         If Not mXSDDoc Is Nothing Then

            treeNode = CType(mCurrentTreeNode, TreeNodeForXML)
            Do While Not treeNode.CanAdd
               treeNode = CType(mCurrentTreeNode.Parent, TreeNodeForXML)
            Loop

            If Not treeNode Is Nothing Then
               ' Infer add status
               If treeNode.Nodes.Count > 0 Then
                  canAdd = CType(treeNode.Nodes(0), TreeNodeForXML).CanAdd
               Else
                  canAdd = False
               End If

               ' Now we know where to stick the new treenode, make new xmldatanode
               Dim nsmgr As New Xml.XmlNamespaceManager(New Xml.NameTable)
               nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")
               nsmgr.AddNamespace("kd", "http://kadgen.com/KADGenDrivingx.xsd")
               MakeNewNode(mnu.XSDNode, treeNode, canAdd, nsmgr)
               'xmlDataNode = Utility.Tools.MakeNewNodeFromXSDNode( _
               '            mXMLDoc, thisNamespace, "kg", mnu.XSDNode)
               '' Need a dummy name
               'Dim attrName As Xml.XmlAttribute = CType( _
               '            xmlDataNode.Attributes.GetNamedItem("Name"), _
               '            Xml.XmlAttribute)
               'If Not attrName Is Nothing Then
               '   attrName.Value = Utility.Tools.SpaceAtCaps( _
               '            Utility.Tools.StripNamespacePrefix(xmlDataNode.Name))
               'End If
               'treeNode.XMLDataNode.AppendChild(xmlDataNode)
               'MakeTreeNode(treeNode.Nodes, xmlDataNode, canAdd)
            End If
         End If
      End If
   End Sub

   Private Sub MakeNewNode( _
                     ByVal xsdNode As Xml.XmlNode, _
                     ByVal parentTreeNode As TreeNodeForXML, _
                     ByVal canAdd As Boolean, _
                     ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim xmlDataNode As Xml.XmlNode = _
                  Utility.Tools.MakeNewNodeFromXSDNode( _
                  mXMLDoc, thisNamespace, "kg", xsdNode)
      ' Need a dummy name
      Dim attrName As Xml.XmlAttribute = CType( _
                  xmlDataNode.Attributes.GetNamedItem("Name"), _
                  Xml.XmlAttribute)
      Dim xsdNodes() As Xml.XmlNode
      Dim newTreeNode As TreeNodeForXML
      If Not attrName Is Nothing Then
         attrName.Value = Utility.Tools.SpaceAtCaps( _
                  Utility.Tools.StripNamespacePrefix(xmlDataNode.Name))
      End If

      parentTreeNode.XMLDataNode.AppendChild(xmlDataNode)
      newTreeNode = MakeTreeNode(parentTreeNode.Nodes, xmlDataNode, canAdd, nsmgr)

      xsdNodes = CType(mChildAutos(Utility.Tools.StripNamespacePrefix(xmlDataNode.Name)), Xml.XmlNode())
      If Not xsdNodes Is Nothing Then
         For Each xsdAutoNode As Xml.XmlNode In xsdNodes
            Dim newXmlDataNode As Xml.XmlNode = _
                        Utility.Tools.MakeNewNodeFromXSDNode( _
                        mXMLDoc, thisNamespace, "kg", xsdAutoNode)
            MakeTreeNode(newTreeNode.Nodes, newXmlDataNode, canAdd, nsmgr)
         Next
      End If
   End Sub

   Private Function MakeTreeNode( _
                 ByVal nodeCollection As Windows.Forms.TreeNodeCollection, _
                 ByVal node As Xml.XmlNode, _
                 ByVal canAdd As Boolean, _
                 ByVal nsmgr As Xml.XmlNamespaceManager) _
                 As TreeNodeForXML
      Dim xmlSchemaNode As Xml.XmlNode = Utility.xmlHelpers.GetSchemaForNode(node.LocalName, _
                     mXSDDoc)
      Dim treenode As New TreeNodeForXML(MakeName(node), node, xmlSchemaNode)
      nodeCollection.Add(treenode)
      MarkChecked(treenode, xmlSchemaNode, node, nsmgr)
      'KD1 treenode.Checked = Me.IsChecked(node)
      treenode.CanAdd = canAdd
      Return treenode
   End Function

   Private Sub AddMenuStuff( _
                  ByVal menu As Windows.Forms.Menu, _
                  ByVal obj As Object)
      Dim mnu As MenuItemWithXSD
      Dim menuStuffs() As menuStuff
      menuStuffs = CType(obj, menuStuff())
      menu.MenuItems.Clear()
      For Each menuStuff As menuStuff In menuStuffs
         If (Not menuStuff.Text Is Nothing) AndAlso menuStuff.Text.Trim.Length > 0 Then
            mnu = New MenuItemWithXSD( _
                           menuStuff.Text, menuStuff.XSDNode)
            'menuStuff.XSDNode.ParentNode.ParentNode.ParentNode)
            AddHandler mnu.Click, AddressOf AddItemClick
            menu.MenuItems.Add(mnu)
         End If
      Next
   End Sub

   Protected Overridable Sub DeleteTreeNode( _
                  ByVal XMLTreeNode As TreeNodeForXML)
      If Windows.Forms.MessageBox.Show("Do you want to delete " & _
                        mCurrentTreeNode.Text & "?", "", _
                        Windows.Forms.MessageBoxButtons.YesNo) = _
                        Windows.Forms.DialogResult.Yes Then
         ' Remove xml node and treenode
         XMLTreeNode.XMLDataNode.ParentNode.RemoveChild(XMLTreeNode.XMLDataNode)
         XMLTreeNode.Remove()
         EnableSave(True)
      End If
   End Sub

   Private Sub FillAddMenu( _
                  ByVal treeNode As Windows.Forms.TreeNode, _
                  ByVal menu As Windows.Forms.Menu)
      Dim name As String
      Dim obj As Object
      Dim treeNodeXML As TreeNodeForXML = CType(treeNode, TreeNodeForXML)
      name = Utility.Tools.GetAttributeOrEmpty(treeNodeXML.XMLSchemaNode, "name")
      obj = mChildMenus(name)
      If Not obj Is Nothing Then
         AddMenuStuff(menu, obj)
      Else
         ' Try the next one up
         If Not treeNode.Parent Is Nothing Then
            treeNodeXML = CType(treeNode.Parent, TreeNodeForXML)
            name = Utility.Tools.GetAttributeOrEmpty(treeNodeXML.XMLSchemaNode, "name")
            obj = mChildMenus(name)
            If Not obj Is Nothing Then
               AddMenuStuff(menu, obj)
            Else
               AddMenuStuff(menu, mBaseMenus)
            End If
         Else
            AddMenuStuff(menu, mBaseMenus)
         End If
      End If
   End Sub

   Private Function GetMenuInfo( _
               ByVal node As Xml.XmlNode, _
               ByVal nsmgr As Xml.XmlNamespaceManager) _
               As menuStuff()
      Dim childNodeList As Xml.XmlNodeList
      Dim childNode As Xml.XmlNode
      Dim menuStuffItems() As menuStuff
      Dim menuStuff As menuStuff
      Dim autoStuff() As Xml.XmlNode
      ' The following selects elements with at least one child
      childNodeList = node.SelectNodes( _
              "xs:complexType/*/xs:element[*]", nsmgr)
      If childNodeList.Count > 0 Then
         ReDim menuStuffItems(childNodeList.Count - 1)
         For i As Int32 = 0 To menuStuffItems.GetUpperBound(0)
            childNode = childNodeList(i)
            If Utility.Tools.GetAttributeOrEmpty(childNode, "", "kd:auto", nsmgr) <> "true" Then
               menuStuff = New menuStuff(childNode, Utility.Tools.SpaceAtCaps( _
                        Utility.Tools.GetAttributeOrEmpty(childNode, "name")))
               menuStuffItems(i) = menuStuff
               mChildMenus.Add(Utility.Tools.GetAttributeOrEmpty(childNode, "name"), _
                           GetMenuInfo(childNode, nsmgr))
            End If
         Next
         childNodeList = node.SelectNodes( _
                  "xs:complexType/*/xs:element[@kd:auto='true']", nsmgr)
         If childNodeList.Count > 0 Then
            ReDim autoStuff(childNodeList.Count - 1)
            For i As Int32 = 0 To childNodeList.Count - 1
               autoStuff(i) = childNodeList(i)
            Next
            mChildAutos.Add(Utility.Tools.GetAttributeOrEmpty(node, "name"), autoStuff)
         End If
      End If

      Return menuStuffItems
   End Function

   Private Function GetAutoInfo( _
            ByVal node As Xml.XmlNode, _
            ByVal nsmgr As Xml.XmlNamespaceManager) _
            As menuStuff()
      Dim childNodeList As Xml.XmlNodeList
      Dim childNode As Xml.XmlNode
      Dim menuStuffItems() As menuStuff
      Dim menuStuff As menuStuff
      childNodeList = node.SelectNodes( _
               "xs:complexType/*/xs:element[@kd:auto='true']", nsmgr)
      If childNodeList.Count > 0 Then
         ReDim menuStuffItems(childNodeList.Count - 1)
         For i As Int32 = 0 To menuStuffItems.GetUpperBound(0)
            childNode = childNodeList(i)
            menuStuff = New menuStuff(childNode, Utility.Tools.SpaceAtCaps( _
                     Utility.Tools.GetAttributeOrEmpty(childNode, "name")))
            menuStuffItems(i) = menuStuff
            mChildAutos.Add(Utility.Tools.GetAttributeOrEmpty(childNode, "name"), _
                        GetAutoInfo(childNode, nsmgr))
         Next
      End If

      Return menuStuffItems
   End Function

   Private Sub MarkChecked( _
               ByVal treenode As Windows.Forms.TreeNode, _
               ByVal xmlSchemaNode As Xml.XmlNode, _
               ByVal xmlNode As Xml.XmlNode, _
               ByVal nsmgr As Xml.XmlNamespaceManager)
      Const noCheckImage As Int32 = 0
      Const checkImage As Int32 = 1
      Const uncheckImage As Int32 = 2
      If Not Me.CanCheck(xmlSchemaNode, nsmgr) Then
         treenode.ImageIndex = noCheckImage
         treenode.SelectedImageIndex = noCheckImage
      ElseIf Me.IsChecked(xmlNode) Then
         treenode.ImageIndex = checkImage
         treenode.SelectedImageIndex = checkImage
      Else
         treenode.ImageIndex = uncheckImage
         treenode.SelectedImageIndex = uncheckImage
         'treenode.BackColor = Drawing.Color.LightYellow
      End If
   End Sub

#End Region



End Class
