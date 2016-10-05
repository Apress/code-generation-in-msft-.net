' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: The MDI Parent specific to the harness

Option Strict On
Option Explicit On 

Imports System

Public Class HarnessForm
   Inherits WinFormSupport.SimpleMDIParentBase

#Region "Class level declarations -empty"
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
      Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HarnessForm))
      '
      'HarnessForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(656, 521)
      Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
      Me.Name = "HarnessForm"
      Me.Text = "Harness"

   End Sub

#End Region

#Region "Event Handlers -empty"
#End Region

#Region "Public Methods and Properties -empty"
#End Region

#Region "Protected and Friend Methods and Properties"
   Protected Overrides Function FormForFileType( _
                  ByVal fileExtension As String) _
                  As System.Windows.Forms.Form
      Return New GenerationForm
   End Function

   Protected Overrides Sub NewFile()
      Dim xmlDoc As Xml.XmlDocument
      Dim node As Xml.XmlNode
      Dim attr As Xml.XmlAttribute
      Dim nodeChild As Xml.XmlNode
      Dim frm As GenerationForm
      Dim fileName As String
      Dim targetNameSpace As String
      Me.GetOpenFileDialog.Filter = "XML Schema files (*.xsd)|*.xsd"
      If Me.GetOpenFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
         fileName = Me.GetOpenFileDialog.FileName
         ' We have to open it to find the target namespace
         xmlDoc = New Xml.XmlDocument
         xmlDoc.Load(fileName)
         Dim nsmgr As Xml.XmlNamespaceManager = KADGen.Utility.Tools.BuildNameSpaceManager(xmlDoc, True)
         node = xmlDoc.SelectSingleNode("/xs:schema", nsmgr)
         targetNameSpace = KADGen.Utility.Tools.GetAttributeOrEmpty(node, "targetNamespace")
         xmlDoc = New Xml.XmlDocument
         node = xmlDoc.CreateXmlDeclaration("1.0", String.Empty, "yes")
         xmlDoc.AppendChild(node)
         node = xmlDoc.CreateElement("GenerationScript")
         attr = xmlDoc.CreateAttribute("xmlns:gs")
         attr.Value = targetNameSpace
         nodeChild = xmlDoc.CreateElement("Section")
         node.AppendChild(nodeChild)
         node.Attributes.Append(attr)
         xmlDoc.AppendChild(node)
         frm = New GenerationForm
         frm.MdiParent = Me
         frm.Show(xmlDoc, IO.Path.GetDirectoryName(fileName))
      End If
   End Sub
#End Region

#Region "Protected Event Response Methods -empty"
#End Region

#Region "Private Methods and Properties"
   Private Sub InitForm()
      mfilter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*"
   End Sub

#End Region



End Class
