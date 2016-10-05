' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Merge metadata.This is an intimate merge as described in Chapter 2 of Code Generation in Microsoft .NET

Option Strict On
Option Explicit On 

Imports System
Imports KADGen.Utility

Public Class MergeFreeForm
   Public Shared Sub Merge( _
                  ByVal outDoc As Xml.XmlDocument, _
                  ByVal fileNames() _
                  As String)
      Dim mergeDoc As New Xml.XmlDocument
      For Each filename As String In fileNames
         mergeDoc.Load(filename)
         MergeRoot(outDoc, mergeDoc)
      Next
   End Sub

   Public Shared Sub Merge( _
                  ByVal baseFileName As String, _
                  ByVal mergeFileName As String, _
                  ByVal outputFileName As String)
      Dim outDoc As New Xml.XmlDocument
      Dim mergeDoc As New Xml.XmlDocument
      outDoc.Load(baseFileName)
      mergeDoc.Load(mergeFileName)
      MergeRoot(outDoc, mergeDoc)
      'BEN I changed this!
      Utility.Tools.MakePathIfNeeded(outputFileName)
      outDoc.Save(outputFileName)
   End Sub

   Private Shared Sub MergeRoot( _
                  ByVal outDoc As Xml.XmlDocument, _
                  ByVal mergeDoc As Xml.XmlDocument)
      Dim rootNode As Xml.XmlNode
      Dim outRoot As Xml.XmlNode
      For Each node As Xml.XmlNode In outDoc.ChildNodes
         If node.NodeType = Xml.XmlNodeType.Element Then
            outRoot = node
         End If
      Next
      For Each node As Xml.XmlNode In mergeDoc.ChildNodes
         If node.NodeType = Xml.XmlNodeType.Element Then
            rootNode = node
         End If
      Next
      'rootNode = mergeDoc.ChildNodes(1)
      If Tools.GetAttributeOrEmpty(rootNode, "FreeForm") = "true" Then
         ' If its a freeform file, regardless of the root element name, 
         ' attempt to merge all child nodes of root
         For Each node As Xml.XmlNode In rootNode.ChildNodes
            MergeNode(outRoot, node, Nothing)
         Next
      Else
         MergeNode(outRoot, rootNode, Nothing)
      End If
   End Sub

   Private Shared Sub MergeNode( _
                  ByVal outParent As Xml.XmlNode, _
                  ByVal node As Xml.XmlNode, _
                  ByVal nameAttrib As Xml.XmlAttribute)
      ' If node.NodeType <> Xml.XmlNodeType.Comment Then
      Dim nsmgr As New Xml.XmlNamespaceManager(outParent.OwnerDocument.NameTable)
      Dim predicate As String = ""
      nsmgr.AddNamespace(node.Prefix, node.NamespaceURI)
      If Not nameAttrib Is Nothing Then
         predicate = "[@Name='" & nameAttrib.Value & "']"
      End If
      Dim testNode As Xml.XmlNode = outParent.SelectSingleNode( _
                        node.Name & predicate, nsmgr)
      If testNode Is Nothing Then
         AddChild(outParent, node)
      Else
         If Utility.Tools.GetAttributeOrEmpty(node, "MergeRemoveExisting").ToLower = "true" Then
            testNode.ParentNode.RemoveChild(testNode)
         ElseIf Utility.Tools.GetAttributeOrEmpty(node, "MergeReplaceExisting").ToLower = "true" Then
            testNode.ParentNode.RemoveChild(testNode)
            AddChild(outParent, node)
         Else
            ' Node exists, add attributes, then children
            For Each attrib As Xml.XmlAttribute In node.Attributes
               If testNode.Attributes(attrib.Name) Is Nothing Then
                  testNode.Attributes.Append(xmlHelpers.NewAttribute( _
                           testNode.OwnerDocument, attrib.Name, attrib.Value))
               Else
                  testNode.Attributes(attrib.Name).Value = attrib.Value
               End If
            Next
            For Each childNode As Xml.XmlNode In node.ChildNodes
               MergeNode(testNode, childNode, childNode.Attributes("Name"))
            Next
         End If
      End If
      ' End If
   End Sub

   Private Shared Sub AddChild(ByVal outParent As Xml.XmlNode, ByVal node As Xml.XmlNode)
      outParent.AppendChild(outParent.OwnerDocument.ImportNode(node, True))
   End Sub

End Class
