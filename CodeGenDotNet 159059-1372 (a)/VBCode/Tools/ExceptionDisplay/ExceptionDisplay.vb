' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Displays exceptions in a manner friendly for the user

Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class ExceptionDisplay
    Inherits System.Windows.Forms.Form

   Private Enum exType
      Unknown
      Harness
      XSLT
      XML
   End Enum

#Region "Class level declarations"
   Private mExType As exType
   Private mDeepNode As Windows.Forms.TreeNode
   Private mStackArray As ExceptionDisplay.StackEntry()
   Private mStackArrayShort As ExceptionDisplay.StackEntry()

   Const vbcrlf As String = Microsoft.VisualBasic.ControlChars.CrLf
#End Region

#Region " Windows Form Designer generated code"

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
   Friend WithEvents Panel1 As System.Windows.Forms.Panel
   Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
   Friend WithEvents Panel2 As System.Windows.Forms.Panel
   Friend WithEvents tvExc As System.Windows.Forms.TreeView
   Friend WithEvents btnClose As System.Windows.Forms.Button
   Friend WithEvents Splitter3 As System.Windows.Forms.Splitter
   Friend WithEvents pnlMessage As System.Windows.Forms.Panel
   Friend WithEvents pnlStack As System.Windows.Forms.Panel
   Friend WithEvents Label2 As System.Windows.Forms.Label
   Friend WithEvents Label3 As System.Windows.Forms.Label
   Friend WithEvents txtStack As System.Windows.Forms.TextBox
   Friend WithEvents pnlStackGrid As System.Windows.Forms.TextBox
   Friend WithEvents Splitter4 As System.Windows.Forms.Splitter
   Friend WithEvents Panel3 As System.Windows.Forms.Panel
   Friend WithEvents gridStack As System.Windows.Forms.DataGrid
   Friend WithEvents txtMessage As System.Windows.Forms.TextBox
   Friend WithEvents Splitter5 As System.Windows.Forms.Splitter
   Friend WithEvents Label4 As System.Windows.Forms.Label
   Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
   Friend WithEvents pnlWarning As System.Windows.Forms.Panel
   Friend WithEvents txtWarning As System.Windows.Forms.TextBox
   Friend WithEvents Label1 As System.Windows.Forms.Label
   Friend WithEvents Label5 As System.Windows.Forms.Label
   Friend WithEvents tvCurrentNode As System.Windows.Forms.TreeView
   Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
   Friend WithEvents chkShowAllStack As System.Windows.Forms.CheckBox
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Me.Panel1 = New System.Windows.Forms.Panel
      Me.tvCurrentNode = New System.Windows.Forms.TreeView
      Me.Label4 = New System.Windows.Forms.Label
      Me.Splitter5 = New System.Windows.Forms.Splitter
      Me.btnClose = New System.Windows.Forms.Button
      Me.tvExc = New System.Windows.Forms.TreeView
      Me.Label5 = New System.Windows.Forms.Label
      Me.Splitter2 = New System.Windows.Forms.Splitter
      Me.pnlWarning = New System.Windows.Forms.Panel
      Me.txtWarning = New System.Windows.Forms.TextBox
      Me.Label1 = New System.Windows.Forms.Label
      Me.Splitter1 = New System.Windows.Forms.Splitter
      Me.Panel2 = New System.Windows.Forms.Panel
      Me.Panel3 = New System.Windows.Forms.Panel
      Me.chkShowAllStack = New System.Windows.Forms.CheckBox
      Me.gridStack = New System.Windows.Forms.DataGrid
      Me.Splitter4 = New System.Windows.Forms.Splitter
      Me.pnlStack = New System.Windows.Forms.Panel
      Me.txtStack = New System.Windows.Forms.TextBox
      Me.Label3 = New System.Windows.Forms.Label
      Me.Splitter3 = New System.Windows.Forms.Splitter
      Me.pnlMessage = New System.Windows.Forms.Panel
      Me.txtMessage = New System.Windows.Forms.TextBox
      Me.pnlStackGrid = New System.Windows.Forms.TextBox
      Me.Label2 = New System.Windows.Forms.Label
      Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
      Me.Panel1.SuspendLayout()
      Me.pnlWarning.SuspendLayout()
      Me.Panel2.SuspendLayout()
      Me.Panel3.SuspendLayout()
      CType(Me.gridStack, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.pnlStack.SuspendLayout()
      Me.pnlMessage.SuspendLayout()
      Me.SuspendLayout()
      '
      'Panel1
      '
      Me.Panel1.Controls.Add(Me.tvCurrentNode)
      Me.Panel1.Controls.Add(Me.Label4)
      Me.Panel1.Controls.Add(Me.Splitter5)
      Me.Panel1.Controls.Add(Me.btnClose)
      Me.Panel1.Controls.Add(Me.tvExc)
      Me.Panel1.Controls.Add(Me.Label5)
      Me.Panel1.Controls.Add(Me.Splitter2)
      Me.Panel1.Controls.Add(Me.pnlWarning)
      Me.Panel1.Controls.Add(Me.Label1)
      Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
      Me.Panel1.Location = New System.Drawing.Point(0, 0)
      Me.Panel1.Name = "Panel1"
      Me.Panel1.Size = New System.Drawing.Size(184, 549)
      Me.Panel1.TabIndex = 0
      '
      'tvCurrentNode
      '
      Me.tvCurrentNode.BackColor = System.Drawing.SystemColors.Control
      Me.tvCurrentNode.Dock = System.Windows.Forms.DockStyle.Fill
      Me.tvCurrentNode.ForeColor = System.Drawing.SystemColors.ControlText
      Me.tvCurrentNode.ImageIndex = -1
      Me.tvCurrentNode.Location = New System.Drawing.Point(0, 292)
      Me.tvCurrentNode.Name = "tvCurrentNode"
      Me.tvCurrentNode.SelectedImageIndex = -1
      Me.tvCurrentNode.Size = New System.Drawing.Size(184, 234)
      Me.tvCurrentNode.TabIndex = 10
      '
      'Label4
      '
      Me.Label4.BackColor = System.Drawing.SystemColors.ActiveCaption
      Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
      Me.Label4.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.Label4.Location = New System.Drawing.Point(0, 269)
      Me.Label4.Name = "Label4"
      Me.Label4.Size = New System.Drawing.Size(184, 23)
      Me.Label4.TabIndex = 3
      Me.Label4.Text = "Current Directive"
      '
      'Splitter5
      '
      Me.Splitter5.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter5.Location = New System.Drawing.Point(0, 266)
      Me.Splitter5.Name = "Splitter5"
      Me.Splitter5.Size = New System.Drawing.Size(184, 3)
      Me.Splitter5.TabIndex = 2
      Me.Splitter5.TabStop = False
      '
      'btnClose
      '
      Me.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom
      Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnClose.Location = New System.Drawing.Point(0, 526)
      Me.btnClose.Name = "btnClose"
      Me.btnClose.Size = New System.Drawing.Size(184, 23)
      Me.btnClose.TabIndex = 1
      Me.btnClose.Text = "Close"
      '
      'tvExc
      '
      Me.tvExc.Dock = System.Windows.Forms.DockStyle.Top
      Me.tvExc.ImageIndex = -1
      Me.tvExc.Location = New System.Drawing.Point(0, 162)
      Me.tvExc.Name = "tvExc"
      Me.tvExc.SelectedImageIndex = -1
      Me.tvExc.Size = New System.Drawing.Size(184, 104)
      Me.tvExc.TabIndex = 0
      '
      'Label5
      '
      Me.Label5.BackColor = System.Drawing.SystemColors.ActiveCaption
      Me.Label5.Dock = System.Windows.Forms.DockStyle.Top
      Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.Label5.Location = New System.Drawing.Point(0, 139)
      Me.Label5.Name = "Label5"
      Me.Label5.Size = New System.Drawing.Size(184, 23)
      Me.Label5.TabIndex = 9
      Me.Label5.Text = "Exception Tree"
      '
      'Splitter2
      '
      Me.Splitter2.Cursor = System.Windows.Forms.Cursors.HSplit
      Me.Splitter2.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter2.Location = New System.Drawing.Point(0, 136)
      Me.Splitter2.Name = "Splitter2"
      Me.Splitter2.Size = New System.Drawing.Size(184, 3)
      Me.Splitter2.TabIndex = 7
      Me.Splitter2.TabStop = False
      '
      'pnlWarning
      '
      Me.pnlWarning.AutoScroll = True
      Me.pnlWarning.BackColor = System.Drawing.SystemColors.Info
      Me.pnlWarning.Controls.Add(Me.txtWarning)
      Me.pnlWarning.Dock = System.Windows.Forms.DockStyle.Top
      Me.pnlWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.pnlWarning.ForeColor = System.Drawing.SystemColors.InfoText
      Me.pnlWarning.Location = New System.Drawing.Point(0, 24)
      Me.pnlWarning.Name = "pnlWarning"
      Me.pnlWarning.Size = New System.Drawing.Size(184, 112)
      Me.pnlWarning.TabIndex = 6
      '
      'txtWarning
      '
      Me.txtWarning.BackColor = System.Drawing.SystemColors.Info
      Me.txtWarning.Dock = System.Windows.Forms.DockStyle.Fill
      Me.txtWarning.ForeColor = System.Drawing.SystemColors.InfoText
      Me.txtWarning.Location = New System.Drawing.Point(0, 0)
      Me.txtWarning.Multiline = True
      Me.txtWarning.Name = "txtWarning"
      Me.txtWarning.ReadOnly = True
      Me.txtWarning.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
      Me.txtWarning.Size = New System.Drawing.Size(184, 112)
      Me.txtWarning.TabIndex = 0
      Me.txtWarning.Text = ""
      '
      'Label1
      '
      Me.Label1.BackColor = System.Drawing.SystemColors.ActiveCaption
      Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
      Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.Label1.Location = New System.Drawing.Point(0, 0)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(184, 24)
      Me.Label1.TabIndex = 8
      Me.Label1.Text = "Overall Problem"
      Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      '
      'Splitter1
      '
      Me.Splitter1.Location = New System.Drawing.Point(184, 0)
      Me.Splitter1.Name = "Splitter1"
      Me.Splitter1.Size = New System.Drawing.Size(3, 549)
      Me.Splitter1.TabIndex = 1
      Me.Splitter1.TabStop = False
      '
      'Panel2
      '
      Me.Panel2.Controls.Add(Me.Panel3)
      Me.Panel2.Controls.Add(Me.Splitter4)
      Me.Panel2.Controls.Add(Me.pnlStack)
      Me.Panel2.Controls.Add(Me.Label3)
      Me.Panel2.Controls.Add(Me.Splitter3)
      Me.Panel2.Controls.Add(Me.pnlMessage)
      Me.Panel2.Controls.Add(Me.Label2)
      Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
      Me.Panel2.Location = New System.Drawing.Point(187, 0)
      Me.Panel2.Name = "Panel2"
      Me.Panel2.Size = New System.Drawing.Size(581, 549)
      Me.Panel2.TabIndex = 2
      '
      'Panel3
      '
      Me.Panel3.Controls.Add(Me.chkShowAllStack)
      Me.Panel3.Controls.Add(Me.gridStack)
      Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
      Me.Panel3.Location = New System.Drawing.Point(0, 132)
      Me.Panel3.Name = "Panel3"
      Me.Panel3.Size = New System.Drawing.Size(581, 417)
      Me.Panel3.TabIndex = 9
      '
      'chkShowAllStack
      '
      Me.chkShowAllStack.BackColor = System.Drawing.SystemColors.ActiveCaption
      Me.chkShowAllStack.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.chkShowAllStack.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.chkShowAllStack.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.chkShowAllStack.Location = New System.Drawing.Point(472, 0)
      Me.chkShowAllStack.Name = "chkShowAllStack"
      Me.chkShowAllStack.Size = New System.Drawing.Size(104, 16)
      Me.chkShowAllStack.TabIndex = 1
      Me.chkShowAllStack.Text = "Show All"
      '
      'gridStack
      '
      Me.gridStack.CausesValidation = False
      Me.gridStack.DataMember = ""
      Me.gridStack.Dock = System.Windows.Forms.DockStyle.Fill
      Me.gridStack.HeaderForeColor = System.Drawing.SystemColors.ControlText
      Me.gridStack.Location = New System.Drawing.Point(0, 0)
      Me.gridStack.Name = "gridStack"
      Me.gridStack.PreferredColumnWidth = 150
      Me.gridStack.ReadOnly = True
      Me.gridStack.RowHeadersVisible = False
      Me.gridStack.Size = New System.Drawing.Size(581, 417)
      Me.gridStack.TabIndex = 0
      '
      'Splitter4
      '
      Me.Splitter4.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter4.Location = New System.Drawing.Point(0, 129)
      Me.Splitter4.Name = "Splitter4"
      Me.Splitter4.Size = New System.Drawing.Size(581, 3)
      Me.Splitter4.TabIndex = 8
      Me.Splitter4.TabStop = False
      '
      'pnlStack
      '
      Me.pnlStack.Controls.Add(Me.txtStack)
      Me.pnlStack.Dock = System.Windows.Forms.DockStyle.Top
      Me.pnlStack.Location = New System.Drawing.Point(0, 89)
      Me.pnlStack.Name = "pnlStack"
      Me.pnlStack.Size = New System.Drawing.Size(581, 40)
      Me.pnlStack.TabIndex = 4
      '
      'txtStack
      '
      Me.txtStack.BackColor = System.Drawing.SystemColors.Control
      Me.txtStack.Dock = System.Windows.Forms.DockStyle.Fill
      Me.txtStack.ForeColor = System.Drawing.SystemColors.ControlText
      Me.txtStack.Location = New System.Drawing.Point(0, 0)
      Me.txtStack.Multiline = True
      Me.txtStack.Name = "txtStack"
      Me.txtStack.ReadOnly = True
      Me.txtStack.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
      Me.txtStack.Size = New System.Drawing.Size(581, 40)
      Me.txtStack.TabIndex = 0
      Me.txtStack.Text = ""
      '
      'Label3
      '
      Me.Label3.BackColor = System.Drawing.SystemColors.ActiveCaption
      Me.Label3.Dock = System.Windows.Forms.DockStyle.Top
      Me.Label3.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.Label3.Location = New System.Drawing.Point(0, 66)
      Me.Label3.Name = "Label3"
      Me.Label3.Size = New System.Drawing.Size(581, 23)
      Me.Label3.TabIndex = 7
      Me.Label3.Text = "This Stack"
      '
      'Splitter3
      '
      Me.Splitter3.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter3.Location = New System.Drawing.Point(0, 63)
      Me.Splitter3.Name = "Splitter3"
      Me.Splitter3.Size = New System.Drawing.Size(581, 3)
      Me.Splitter3.TabIndex = 3
      Me.Splitter3.TabStop = False
      '
      'pnlMessage
      '
      Me.pnlMessage.AutoScroll = True
      Me.pnlMessage.Controls.Add(Me.txtMessage)
      Me.pnlMessage.Controls.Add(Me.pnlStackGrid)
      Me.pnlMessage.Dock = System.Windows.Forms.DockStyle.Top
      Me.pnlMessage.Location = New System.Drawing.Point(0, 23)
      Me.pnlMessage.Name = "pnlMessage"
      Me.pnlMessage.Size = New System.Drawing.Size(581, 40)
      Me.pnlMessage.TabIndex = 2
      '
      'txtMessage
      '
      Me.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill
      Me.txtMessage.Location = New System.Drawing.Point(0, 0)
      Me.txtMessage.Multiline = True
      Me.txtMessage.Name = "txtMessage"
      Me.txtMessage.ReadOnly = True
      Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
      Me.txtMessage.Size = New System.Drawing.Size(581, 40)
      Me.txtMessage.TabIndex = 1
      Me.txtMessage.Text = ""
      '
      'pnlStackGrid
      '
      Me.pnlStackGrid.BackColor = System.Drawing.SystemColors.Control
      Me.pnlStackGrid.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pnlStackGrid.ForeColor = System.Drawing.SystemColors.ControlText
      Me.pnlStackGrid.Location = New System.Drawing.Point(0, 0)
      Me.pnlStackGrid.Multiline = True
      Me.pnlStackGrid.Name = "pnlStackGrid"
      Me.pnlStackGrid.ReadOnly = True
      Me.pnlStackGrid.Size = New System.Drawing.Size(581, 40)
      Me.pnlStackGrid.TabIndex = 0
      Me.pnlStackGrid.Text = ""
      '
      'Label2
      '
      Me.Label2.BackColor = System.Drawing.SystemColors.ActiveCaption
      Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
      Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.Label2.Location = New System.Drawing.Point(0, 0)
      Me.Label2.Name = "Label2"
      Me.Label2.Size = New System.Drawing.Size(581, 23)
      Me.Label2.TabIndex = 6
      Me.Label2.Text = "This Exception Message"
      '
      'ExceptionDisplay
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(768, 549)
      Me.Controls.Add(Me.Panel2)
      Me.Controls.Add(Me.Splitter1)
      Me.Controls.Add(Me.Panel1)
      Me.Name = "ExceptionDisplay"
      Me.Text = "ExceptionDisplay"
      Me.Panel1.ResumeLayout(False)
      Me.pnlWarning.ResumeLayout(False)
      Me.Panel2.ResumeLayout(False)
      Me.Panel3.ResumeLayout(False)
      CType(Me.gridStack, System.ComponentModel.ISupportInitialize).EndInit()
      Me.pnlStack.ResumeLayout(False)
      Me.pnlMessage.ResumeLayout(False)
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region "Event Handlers"
   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
      SetTableStyle()
   End Sub

   Private Sub chkShowAllStack_CheckedChanged( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles chkShowAllStack.CheckedChanged
      Dim chk As Windows.Forms.CheckBox
      If TypeOf sender Is Windows.Forms.CheckBox Then
         chk = CType(sender, Windows.Forms.CheckBox)
         If chk.Checked Then
            gridStack.SetDataBinding(mStackArray, "")
         Else
            gridStack.SetDataBinding(mStackArrayShort, "")
         End If
      End If
   End Sub

   Private Sub tvExc_AfterSelect( _
                     ByVal sender As System.Object, _
                     ByVal e As System.Windows.Forms.TreeViewEventArgs) _
                     Handles tvExc.AfterSelect
      Dim ex As System.Exception
      Dim tnode As Windows.Forms.TreeNode
      Dim s As String
      Dim reader As IO.StringReader
      Dim ar As New System.Collections.ArrayList
      Dim arShort As New System.Collections.ArrayList
      Dim entry As StackEntry
      Dim i As Int32
      Try
         tnode = tvExc.SelectedNode
         If Not tnode Is Nothing Then
            If TypeOf tnode.Tag Is System.Exception Then
               ex = CType(tnode.Tag, System.Exception)
            End If
         End If
         If Not ex Is Nothing Then
            txtMessage.Text = ex.Message
            txtStack.Text = ex.StackTrace
         End If
         reader = New IO.StringReader(ex.StackTrace)
         s = reader.ReadLine
         Do While Not s Is Nothing
            i += 1
            entry = New StackEntry(s, i)
            ar.Add(entry)
            If Not entry.IsFramework Then
               arShort.Add(entry)
            End If
            s = reader.ReadLine
         Loop
         mStackArray = CType(ar.ToArray(GetType(StackEntry)), StackEntry())
         mStackArrayShort = CType(arShort.ToArray(GetType(StackEntry)), StackEntry())
         gridStack.SetDataBinding(mStackArrayShort, "")
      Catch exception As System.Exception
         Diagnostics.Debug.WriteLine(exception)
      End Try
   End Sub


   Private Sub gridStack_MouseMove( _
                     ByVal sender As Object, _
                     ByVal e As System.Windows.Forms.MouseEventArgs) _
                     Handles gridStack.MouseMove
      Dim hit As Windows.Forms.DataGrid.HitTestInfo
      hit = gridStack.HitTest(e.X, e.Y)
      Dim stackentry As stackentry = mStackArray(hit.Row)
      Dim memberName As String
      memberName = gridStack.TableStyles(0).GridColumnStyles(hit.Column).MappingName()
      Me.ToolTip.SetToolTip(gridStack, stackentry.GetText(memberName))
   End Sub

   Private Sub btnClose_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles btnClose.Click
      Me.Close()
   End Sub
#End Region

#Region "Public Methods and Properties"
   Public Overloads Sub Show( _
                     ByVal ex As System.Exception, _
                     ByVal xmlInfo As Xml.XmlNode, _
                     ByVal currentFile As String)
      Dim warning As String
      LoadTreeNode(ex, tvExc.Nodes)
      Select Case mExType
         Case exType.Harness
            warning = "An unexpected error occurred, the problem may lie in the generation harness itself"
         Case exType.Unknown
            warning = "An unknown error occurred"
         Case exType.XML
            warning = "An error ocurred in an XML document"
         Case exType.XSLT
            warning = "An error ocurred in an XSLT document"
      End Select
      If (Not currentFile Is Nothing) AndAlso (currentFile.Length >= 0) Then
         warning &= vbcrlf & vbcrlf & "The problem seems to have occurred with the file " & currentFile
      End If

      txtWarning.Text = warning
      tvExc.SelectedNode = mDeepNode
      If Not xmlInfo Is Nothing Then
         LoadTreeXML(xmlInfo, tvCurrentNode.Nodes)
      End If

      Me.ShowDialog()
   End Sub

#End Region

#Region "Protected and Friend Methods and Properties-empty"
#End Region

#Region "Protected Event Response Methods-empty"
#End Region

#Region "Private Methods and Properties"
   Private Sub InitForm()
   End Sub

   Private Sub LoadTreeNode( _
                     ByVal ex As System.Exception, _
                     ByVal t As Windows.Forms.TreeNodeCollection)
      Dim tnode As New Windows.Forms.TreeNode
      tnode.Text = ex.GetType.Name
      tnode.Tag = ex
      t.Add(tnode)
      ' This must occur before recursive call!
      mDeepNode = tnode
      If Not ex.InnerException Is Nothing Then
         LoadTreeNode(ex.InnerException, tnode.Nodes)
      End If
      If TypeOf ex Is System.Reflection.TargetInvocationException Then
         ' Leave as unknown
      ElseIf TypeOf ex Is System.Xml.XPath.XPathException And mExType < exType.XSLT Then
         mExType = exType.XSLT
      ElseIf TypeOf ex Is System.Xml.Xsl.XsltCompileException And mExType < exType.XSLT Then
         mExType = exType.XSLT
      ElseIf mExType < exType.XSLT Then
         mExType = exType.Harness
      End If
   End Sub

   Private Sub LoadTreeXML( _
                     ByVal xmlInfo As Xml.XmlNode, _
                     ByVal tv As Windows.Forms.TreeNodeCollection)
      Dim tnode As Windows.Forms.TreeNode
      Dim tAttr As Windows.Forms.TreeNode
      Select Case xmlInfo.NodeType
         Case Xml.XmlNodeType.Element
            tnode = New Windows.Forms.TreeNode
            tnode.Text = "<" & xmlInfo.Name & ">"
            For Each xnode As Xml.XmlNode In xmlInfo.ChildNodes
               LoadTreeXML(xnode, tnode.Nodes)
            Next
            For Each xAttr As Xml.XmlAttribute In xmlInfo.Attributes
               tAttr = New Windows.Forms.TreeNode
               tAttr.Text = "@" & xAttr.Name & " = " & xAttr.Value
               tnode.Nodes.Add(tAttr)
            Next
            tv.Add(tnode)
         Case Xml.XmlNodeType.Attribute
         Case Else
            ' just skip it
      End Select
   End Sub

   Private Sub SetTableStyle()
      Diagnostics.Debug.WriteLine(gridStack.TableStyles.Count)
      Dim ts As New Windows.Forms.DataGridTableStyle
      With ts
         '.GridColumnStyles.Clear()
         .RowHeadersVisible = False
         .MappingName = "StackEntry[]"
         .GridColumnStyles.Add(BuildCS("", "RowNum", 30))
         .GridColumnStyles.Add(BuildCS("Location", "MethodShort", 100))
         .GridColumnStyles.Add(BuildCS("Line Num", "Position", 40))
         .GridColumnStyles.Add(BuildCS("File", "File", 100))
         .GridColumnStyles.Add(BuildCS("Long Method Name", "Method", 200))
         .GridColumnStyles.Add(BuildCS("Long File Name", "FullFile", 200))
      End With
      gridStack.TableStyles.Add(ts)
      gridStack.Refresh()
   End Sub

   Public Function BuildCS( _
                     ByVal HeaderText As String, _
                     ByVal MappingName As String, _
                     ByVal Width As Int32) _
                     As Windows.Forms.DataGridColumnStyle
      Dim cs As New Windows.Forms.DataGridTextBoxColumn
      cs.HeaderText = HeaderText
      cs.MappingName = MappingName
      cs.Width = Width
      Return cs

   End Function
#End Region


   Public Class StackEntry
      Private mMethod As String
      Private mMethodShort As String
      Private mFile As String
      Private mFullFile As String
      Private mPosition As String
      Private mRowNum As String

      Public Sub New(ByVal line As String, ByVal rowNum As Int32)
         Dim arr As String()
         Dim iPos As Int32
         line = line.Trim
         If line.StartsWith("at ") Then
            line = line.Substring(3)
         End If
         arr = line.Split(")"c)
         mRowNum = CStr(rowNum)
         mMethod = arr(0)
         mMethodShort = SubstringAfter(SubstringBefore(arr(0), "("), ".")
         If arr.GetLength(0) > 1 Then
            line = arr(1).Trim
            If line.StartsWith("in ") Then
               line = line.Substring(3)
            End If
            mFullFile = line
            mPosition = SubstringAfter(line, ":").Trim
            If mPosition.StartsWith("line ") Then
               mPosition = mPosition.Substring(5)
            End If
            mFile = SubstringAfter(line, "\")
         End If
      End Sub

      Public Function GetText(ByVal memberName As String) As String
         ' We could use reflection, but this in intended for tooltips
         ' and this is faster, even if ugly. 
         Select Case memberName.ToLower
            Case "method"
               Return Method
            Case "methodshort"
               Return MethodShort
            Case "file"
               Return file
            Case "fullfile"
               Return FullFile
            Case "position"
               Return Position
            Case "rownum"
               Return RowNum
         End Select
      End Function

      Public ReadOnly Property IsFramework() As Boolean
         Get
            Return (mPosition.Trim.Length = 0)
         End Get
      End Property

      Public Property Method() As String
         Get
            Return mMethod
         End Get
         Set(ByVal Value As String)
            mMethod = Value
         End Set
      End Property

      Public Property RowNum() As String
         Get
            Return mRowNum
         End Get
         Set(ByVal Value As String)
            mRowNum = Value
         End Set
      End Property

      Public Property MethodShort() As String
         Get
            Return mMethodShort
         End Get
         Set(ByVal Value As String)
            mMethodShort = Value
         End Set
      End Property

      Public Property file() As String
         Get
            Return mFile
         End Get
         Set(ByVal Value As String)
            mFile = Value
         End Set
      End Property

      Public Property FullFile() As String
         Get
            Return mFullFile
         End Get
         Set(ByVal Value As String)
            mFullFile = Value
         End Set
      End Property

      Public Property Position() As String
         Get
            Return mPosition
         End Get
         Set(ByVal Value As String)
            mPosition = Value
         End Set
      End Property

      Private Function SubstringAfter( _
                        ByVal s As String, _
                        ByVal c As String) _
                        As String
         Dim ipos As Int32
         ipos = s.LastIndexOf(c)
         If ipos >= 0 Then
            Return s.Substring(ipos + 1)
         Else
            Return ""
         End If
      End Function

      Private Function SubstringBefore( _
                        ByVal s As String, _
                        ByVal c As String) _
                        As String
         Dim ipos As Int32
         ipos = s.LastIndexOf(c)
         If ipos >= 0 Then
            Return s.Substring(0, ipos)
         Else
            Return ""
         End If
      End Function

   End Class




End Class
