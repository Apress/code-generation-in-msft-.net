' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Displays exception details for programmers
'  Refactor: Please provide a friendlier display for your users.

Option Strict On
Option Explicit On

Imports System

Public Class ExceptionDisplay
   Inherits System.Windows.Forms.Form

   Private Enum exType
      Unknown
      Harness
      XSLT
      XML
   End Enum

#Region "Class level declarations"
   Private mStackArray As ExceptionDisplay.StackEntry()
   Private mStackArrayShort As ExceptionDisplay.StackEntry()
   Private mXMLHints As Xml.XmlDocument
   Private mExcFullName As String
   Private mExcShortName As String

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
   Friend WithEvents Splitter3 As System.Windows.Forms.Splitter
   Friend WithEvents pnlMessage As System.Windows.Forms.Panel
   Friend WithEvents pnlStack As System.Windows.Forms.Panel
   Friend WithEvents Label2 As System.Windows.Forms.Label
   Friend WithEvents Label3 As System.Windows.Forms.Label
   Friend WithEvents txtStack As System.Windows.Forms.TextBox
   Friend WithEvents pnlStackGrid As System.Windows.Forms.TextBox
   Friend WithEvents Splitter4 As System.Windows.Forms.Splitter
   Friend WithEvents gridStack As System.Windows.Forms.DataGrid
   Friend WithEvents txtMessage As System.Windows.Forms.TextBox
   Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
   Friend WithEvents pnlWarning As System.Windows.Forms.Panel
   Friend WithEvents txtWarning As System.Windows.Forms.TextBox
   Friend WithEvents Label1 As System.Windows.Forms.Label
   Friend WithEvents Label5 As System.Windows.Forms.Label
   Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
   Friend WithEvents chkShowAllStack As System.Windows.Forms.CheckBox
   Friend WithEvents btnAbort As System.Windows.Forms.Button
   Friend WithEvents btnClose As System.Windows.Forms.Button
   Friend WithEvents pnlButtons As System.Windows.Forms.Panel
   Friend WithEvents pnlGrid As System.Windows.Forms.Panel
   Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
   Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container
      Me.Panel1 = New System.Windows.Forms.Panel
      Me.pnlButtons = New System.Windows.Forms.Panel
      Me.btnClose = New System.Windows.Forms.Button
      Me.btnAbort = New System.Windows.Forms.Button
      Me.tvExc = New System.Windows.Forms.TreeView
      Me.Label5 = New System.Windows.Forms.Label
      Me.Splitter2 = New System.Windows.Forms.Splitter
      Me.pnlWarning = New System.Windows.Forms.Panel
      Me.txtWarning = New System.Windows.Forms.TextBox
      Me.Label1 = New System.Windows.Forms.Label
      Me.Splitter1 = New System.Windows.Forms.Splitter
      Me.Panel2 = New System.Windows.Forms.Panel
      Me.pnlGrid = New System.Windows.Forms.Panel
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
      Me.ComboBox1 = New System.Windows.Forms.ComboBox
      Me.TextBox1 = New System.Windows.Forms.TextBox
      Me.Panel1.SuspendLayout()
      Me.pnlButtons.SuspendLayout()
      Me.pnlWarning.SuspendLayout()
      Me.Panel2.SuspendLayout()
      Me.pnlGrid.SuspendLayout()
      CType(Me.gridStack, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.pnlStack.SuspendLayout()
      Me.pnlMessage.SuspendLayout()
      Me.SuspendLayout()
      '
      'Panel1
      '
      Me.Panel1.Controls.Add(Me.pnlButtons)
      Me.Panel1.Controls.Add(Me.tvExc)
      Me.Panel1.Controls.Add(Me.Label5)
      Me.Panel1.Controls.Add(Me.Splitter2)
      Me.Panel1.Controls.Add(Me.pnlWarning)
      Me.Panel1.Controls.Add(Me.Label1)
      Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
      Me.Panel1.Location = New System.Drawing.Point(0, 0)
      Me.Panel1.Name = "Panel1"
      Me.Panel1.Size = New System.Drawing.Size(288, 413)
      Me.Panel1.TabIndex = 0
      '
      'pnlButtons
      '
      Me.pnlButtons.Controls.Add(Me.btnClose)
      Me.pnlButtons.Controls.Add(Me.btnAbort)
      Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
      Me.pnlButtons.Location = New System.Drawing.Point(0, 379)
      Me.pnlButtons.Name = "pnlButtons"
      Me.pnlButtons.Size = New System.Drawing.Size(288, 34)
      Me.pnlButtons.TabIndex = 10
      '
      'btnClose
      '
      Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnClose.Location = New System.Drawing.Point(176, 0)
      Me.btnClose.Name = "btnClose"
      Me.btnClose.Size = New System.Drawing.Size(88, 32)
      Me.btnClose.TabIndex = 3
      Me.btnClose.Text = "Close"
      '
      'btnAbort
      '
      Me.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnAbort.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.btnAbort.Location = New System.Drawing.Point(72, 0)
      Me.btnAbort.Name = "btnAbort"
      Me.btnAbort.Size = New System.Drawing.Size(88, 32)
      Me.btnAbort.TabIndex = 2
      Me.btnAbort.Text = "Abort App"
      '
      'tvExc
      '
      Me.tvExc.Dock = System.Windows.Forms.DockStyle.Fill
      Me.tvExc.ImageIndex = -1
      Me.tvExc.Location = New System.Drawing.Point(0, 234)
      Me.tvExc.Name = "tvExc"
      Me.tvExc.SelectedImageIndex = -1
      Me.tvExc.Size = New System.Drawing.Size(288, 179)
      Me.tvExc.TabIndex = 0
      '
      'Label5
      '
      Me.Label5.BackColor = System.Drawing.SystemColors.ActiveCaption
      Me.Label5.Dock = System.Windows.Forms.DockStyle.Top
      Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.Label5.Location = New System.Drawing.Point(0, 211)
      Me.Label5.Name = "Label5"
      Me.Label5.Size = New System.Drawing.Size(288, 23)
      Me.Label5.TabIndex = 9
      Me.Label5.Text = "Exception Tree"
      '
      'Splitter2
      '
      Me.Splitter2.Cursor = System.Windows.Forms.Cursors.HSplit
      Me.Splitter2.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter2.Location = New System.Drawing.Point(0, 208)
      Me.Splitter2.Name = "Splitter2"
      Me.Splitter2.Size = New System.Drawing.Size(288, 3)
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
      Me.pnlWarning.Size = New System.Drawing.Size(288, 184)
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
      Me.txtWarning.Size = New System.Drawing.Size(288, 184)
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
      Me.Label1.Size = New System.Drawing.Size(288, 24)
      Me.Label1.TabIndex = 8
      Me.Label1.Text = "Overall Problem"
      Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      '
      'Splitter1
      '
      Me.Splitter1.Location = New System.Drawing.Point(288, 0)
      Me.Splitter1.Name = "Splitter1"
      Me.Splitter1.Size = New System.Drawing.Size(3, 413)
      Me.Splitter1.TabIndex = 1
      Me.Splitter1.TabStop = False
      '
      'Panel2
      '
      Me.Panel2.Controls.Add(Me.pnlGrid)
      Me.Panel2.Controls.Add(Me.Splitter4)
      Me.Panel2.Controls.Add(Me.pnlStack)
      Me.Panel2.Controls.Add(Me.Label3)
      Me.Panel2.Controls.Add(Me.Splitter3)
      Me.Panel2.Controls.Add(Me.pnlMessage)
      Me.Panel2.Controls.Add(Me.Label2)
      Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
      Me.Panel2.Location = New System.Drawing.Point(291, 0)
      Me.Panel2.Name = "Panel2"
      Me.Panel2.Size = New System.Drawing.Size(533, 413)
      Me.Panel2.TabIndex = 2
      '
      'pnlGrid
      '
      Me.pnlGrid.Controls.Add(Me.TextBox1)
      Me.pnlGrid.Controls.Add(Me.ComboBox1)
      Me.pnlGrid.Controls.Add(Me.chkShowAllStack)
      Me.pnlGrid.Controls.Add(Me.gridStack)
      Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pnlGrid.Location = New System.Drawing.Point(0, 188)
      Me.pnlGrid.Name = "pnlGrid"
      Me.pnlGrid.Size = New System.Drawing.Size(533, 225)
      Me.pnlGrid.TabIndex = 9
      '
      'chkShowAllStack
      '
      Me.chkShowAllStack.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.chkShowAllStack.BackColor = System.Drawing.SystemColors.ActiveCaption
      Me.chkShowAllStack.FlatStyle = System.Windows.Forms.FlatStyle.System
      Me.chkShowAllStack.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.chkShowAllStack.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
      Me.chkShowAllStack.Location = New System.Drawing.Point(424, 0)
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
      Me.gridStack.Size = New System.Drawing.Size(533, 225)
      Me.gridStack.TabIndex = 0
      '
      'Splitter4
      '
      Me.Splitter4.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter4.Location = New System.Drawing.Point(0, 185)
      Me.Splitter4.Name = "Splitter4"
      Me.Splitter4.Size = New System.Drawing.Size(533, 3)
      Me.Splitter4.TabIndex = 8
      Me.Splitter4.TabStop = False
      '
      'pnlStack
      '
      Me.pnlStack.Controls.Add(Me.txtStack)
      Me.pnlStack.Dock = System.Windows.Forms.DockStyle.Top
      Me.pnlStack.Location = New System.Drawing.Point(0, 106)
      Me.pnlStack.Name = "pnlStack"
      Me.pnlStack.Size = New System.Drawing.Size(533, 79)
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
      Me.txtStack.Size = New System.Drawing.Size(533, 79)
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
      Me.Label3.Location = New System.Drawing.Point(0, 83)
      Me.Label3.Name = "Label3"
      Me.Label3.Size = New System.Drawing.Size(533, 23)
      Me.Label3.TabIndex = 7
      Me.Label3.Text = "This Stack"
      '
      'Splitter3
      '
      Me.Splitter3.Dock = System.Windows.Forms.DockStyle.Top
      Me.Splitter3.Location = New System.Drawing.Point(0, 80)
      Me.Splitter3.Name = "Splitter3"
      Me.Splitter3.Size = New System.Drawing.Size(533, 3)
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
      Me.pnlMessage.Size = New System.Drawing.Size(533, 57)
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
      Me.txtMessage.Size = New System.Drawing.Size(533, 57)
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
      Me.pnlStackGrid.Size = New System.Drawing.Size(533, 57)
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
      Me.Label2.Size = New System.Drawing.Size(533, 23)
      Me.Label2.TabIndex = 6
      Me.Label2.Text = "This Exception Message"
      '
      'ComboBox1
      '
      Me.ComboBox1.Location = New System.Drawing.Point(256, 96)
      Me.ComboBox1.Name = "ComboBox1"
      Me.ComboBox1.Size = New System.Drawing.Size(121, 21)
      Me.ComboBox1.TabIndex = 2
      Me.ComboBox1.Text = "ComboBox1"
      '
      'TextBox1
      '
      Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      Me.TextBox1.Location = New System.Drawing.Point(328, 168)
      Me.TextBox1.Name = "TextBox1"
      Me.TextBox1.TabIndex = 3
      Me.TextBox1.Text = "TextBox1"
      '
      'ExceptionDisplay
      '
      Me.AcceptButton = Me.btnClose
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.CancelButton = Me.btnAbort
      Me.ClientSize = New System.Drawing.Size(824, 413)
      Me.Controls.Add(Me.Panel2)
      Me.Controls.Add(Me.Splitter1)
      Me.Controls.Add(Me.Panel1)
      Me.Name = "ExceptionDisplay"
      Me.Text = "ExceptionDisplay"
      Me.Panel1.ResumeLayout(False)
      Me.pnlButtons.ResumeLayout(False)
      Me.pnlWarning.ResumeLayout(False)
      Me.Panel2.ResumeLayout(False)
      Me.pnlGrid.ResumeLayout(False)
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
      Dim ar As New System.Collections.ArrayList
      Dim arShort As New System.Collections.ArrayList
      Dim entry As StackEntry
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
            Dim stack As String() = ex.StackTrace.Split(Microsoft.VisualBasic.ControlChars.Cr)
            For i As Int32 = 0 To stack.GetUpperBound(0)
               entry = New StackEntry(stack(i), i)
               ar.Add(entry)
               If Not entry.IsFramework Then
                  arShort.Add(entry)
               End If
            Next
         End If
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
      Dim stackentry As StackEntry = mStackArray(hit.Row)
      Dim memberName As String
      memberName = gridStack.TableStyles(0).GridColumnStyles(hit.Column).MappingName()
      Me.ToolTip.SetToolTip(gridStack, stackentry.GetText(memberName))
   End Sub

   Private Sub btnClose_Click( _
                  ByVal sender As System.Object, _
                  ByVal e As System.EventArgs) _
                  Handles btnClose.Click
      Me.DialogResult = DialogResult.OK
      Me.Close()
   End Sub

   Private Sub btnAbort_Click( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles btnAbort.Click
      Me.DialogResult = DialogResult.Abort
      Me.Close()
   End Sub

   Private Sub pnlButtons_Layout( _
                    ByVal sender As Object, _
                    ByVal e As System.Windows.Forms.LayoutEventArgs) _
                    Handles pnlButtons.Layout

      Dim width As Int32
      width = pnlButtons.Width \ 2
      btnAbort.Left = 0
      btnAbort.Width = width
      btnClose.Left = btnAbort.Right
      btnClose.Width = width
   End Sub

#End Region

#Region "Public Methods and Properties"
   Public Overloads Function Show( _
                     ByVal ex As System.Exception, _
                     ByVal ctl As Windows.Forms.Control, _
                     ByVal xmlHints As Xml.XmlDocument) _
                     As Windows.Forms.DialogResult
      Dim warning As String = FindWarning(ex, xmlHints)
      Dim nodeDeep As Windows.Forms.TreeNode
      nodeDeep = LoadTreeNode(ex, tvExc.Nodes)
      txtWarning.Text = warning
      tvExc.SelectedNode = nodeDeep
      mXMLHints = xmlHints
      Me.mExcFullName = ex.GetType.FullName
      Me.mExcShortName = ex.GetType.Name
      Return Me.ShowDialog()
   End Function

   Public Overloads Function Show( _
                  ByVal ex As System.Exception) _
                  As Windows.Forms.DialogResult
      Dim nodeDeep As Windows.Forms.TreeNode
      nodeDeep = LoadTreeNode(ex, tvExc.Nodes)
      tvExc.SelectedNode = nodeDeep
      Me.mExcFullName = ex.GetType.FullName
      Me.mExcShortName = ex.GetType.Name
      Return Me.ShowDialog()
   End Function
#End Region

#Region "Protected and Friend Methods and Properties-empty"
#End Region

#Region "Protected Event Response Methods-empty"
#End Region

#Region "Private Methods and Properties"
   Private Sub InitForm()
   End Sub

   Private Function LoadTreeNode( _
                     ByVal ex As System.Exception, _
                     ByVal t As Windows.Forms.TreeNodeCollection) _
                     As Windows.Forms.TreeNode
      Dim tnode As New Windows.Forms.TreeNode
      Dim nodeDeep As Windows.Forms.TreeNode
      tnode.Text = ex.GetType.Name
      tnode.Tag = ex
      t.Add(tnode)
      If ex.InnerException Is Nothing Then
         Return tnode
      Else
         Return LoadTreeNode(ex.InnerException, tnode.Nodes)
      End If
   End Function

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

   Private Function FindWarning( _
                     ByVal ex As System.Exception, _
                     ByVal xmlHints As Xml.XmlDocument) _
                     As String
      Dim nodeList As Xml.XmlNodeList
      Dim fullName As String
      Dim shortName As String
      Dim thisEx As System.Exception = ex
      Dim sRet As String = ""  ' Expecting only a few so no stringbuilder
      Do While Not thisEx Is Nothing
         fullName = thisEx.GetType.FullName
         shortName = thisEx.GetType.Name
         nodeList = xmlHints.SelectNodes("//Hint[@FullName='" & fullName & _
                           "' or @ShortName='" & shortName & "']/HintMessage")
         For Each node As Xml.XmlNode In nodeList
            sRet &= node.InnerText & Microsoft.VisualBasic.ControlChars.CrLf
         Next
         thisEx = thisEx.InnerException
      Loop
      Return sRet
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
         ParseLine(line)
      End Sub

      Public ReadOnly Property IsFramework() As Boolean
         Get
            Return (mPosition.Trim.Length = 0)
         End Get
      End Property

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

      Public Sub ParseLine(ByVal line As String)
         Dim arr As String()
         Dim iPos As Int32
         line = line.Trim
         If line.StartsWith("at ") Then
            line = line.Substring(3)
         End If
         arr = line.Split(")"c)
         mRowNum = CStr(RowNum)
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
