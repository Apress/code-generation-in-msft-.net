' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Tools to facilitate working with XML. 
'  Note: Additional tools are in the Tools class

Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class xmlHelpers

   Private Class nspaceLookup
      Public nspace As String
      Public prefix As String
   End Class


#Region "Class level declarations - empty"
#End Region

#Region "Constructors - empty"
#End Region

#Region "Public Methods and Properties"

   Public Shared Function BuildNamespacesManagerForDoc( _
               ByVal xmlDoc As Xml.XmlDocument) _
               As Xml.XmlNamespaceManager
      Return BuildNamespacesManagerForDoc(xmlDoc, "zzz")
   End Function
   Public Shared Function BuildNamespacesManagerForDoc( _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal defaultNamespacePrefix As String) _
               As Xml.XmlNamespaceManager
      Dim nodes As Xml.XmlNodeList
      Dim nsmgrXML As New Xml.XmlNamespaceManager(xmlDoc.NameTable)
      Dim nSpace As nspaceLookup
      Dim collection As New Collections.Hashtable
      Diagnostics.Debug.WriteLine(DateTime.Now)
      nodes = xmlDoc.SelectNodes("//*")
      For Each node As Xml.XmlNode In nodes
         nSpace = New nspaceLookup
         nSpace.nspace = node.NamespaceURI
         nSpace.prefix = node.Prefix
         If Not collection.Contains(nSpace.prefix & ":" & nSpace.nspace) Then
            collection.Add(nSpace.prefix & ":" & nSpace.nspace, nSpace)
         End If
      Next
      Diagnostics.Debug.WriteLine(DateTime.Now)

      For Each d As Collections.DictionaryEntry In collection
         nSpace = CType(d.Value, nspaceLookup)
         If nSpace.prefix.Trim.Length = 0 Then
            nSpace.prefix = defaultNamespacePrefix
         End If
         nsmgrXML.AddNamespace(nSpace.prefix, nSpace.nspace)
      Next
      nsmgrXML.AddNamespace("kg", "http://kadgen.com/KADGenDriving.xsd")

      For Each prefix As String In nsmgrXML
         Console.WriteLine("Prefix={0}, Namespace={1}", prefix, nsmgrXML.LookupNamespace(prefix))
      Next

      Return nsmgrXML


   End Function


   Public Shared Function NewElement( _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal elementName As String, _
               ByVal name As String) _
               As Xml.XmlElement
      Dim nodeElement As Xml.XmlElement
      nodeElement = xmlDoc.CreateElement(elementName)
      nodeElement.Attributes.Append(xmlHelpers.NewAttribute( _
               xmlDoc, "Name", name))
      Return nodeElement
   End Function

   Public Shared Function NewElement( _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal elementName As String) _
               As Xml.XmlElement
      Dim nodeElement As Xml.XmlElement
      nodeElement = xmlDoc.CreateElement(elementName)
      Return nodeElement
   End Function

   Public Shared Function NewElement( _
               ByVal nSpace As String, _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal elementName As String) _
               As Xml.XmlElement
      Dim nodeElement As Xml.XmlElement
      nodeElement = xmlDoc.CreateElement(elementName, nSpace)
      Return nodeElement
   End Function

   Public Shared Function NewElement( _
               ByVal nSpace As String, _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal elementName As String, _
               ByVal name As String) _
               As Xml.XmlElement
      Dim nodeElement As Xml.XmlElement
      nodeElement = xmlDoc.CreateElement(elementName, nSpace)
      nodeElement.Attributes.Append(xmlHelpers.NewAttribute( _
               xmlDoc, "Name", name))
      Return nodeElement
   End Function

   Public Shared Function NewElement( _
               ByVal prefix As String, _
               ByVal nSpace As String, _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal elementName As String) _
               As Xml.XmlElement
      Dim nodeElement As Xml.XmlElement
      nodeElement = xmlDoc.CreateElement(prefix, elementName, nSpace)
      Return nodeElement
   End Function

   Public Shared Function NewAttribute( _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal name As String, _
               ByVal value As String) _
               As Xml.XmlAttribute
      Dim nodeAttribute As Xml.XmlAttribute
      nodeAttribute = xmlDoc.CreateAttribute(name)
      nodeAttribute.Value = value
      Return nodeAttribute
   End Function

   Public Shared Function NewBoolAttribute( _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal name As String, _
               ByVal value As Boolean) _
               As Xml.XmlAttribute
      Dim nodeAttribute As Xml.XmlAttribute
      nodeAttribute = xmlDoc.CreateAttribute(name)
      If value Then
         nodeAttribute.Value = "true"
      Else
         nodeAttribute.Value = "false"
      End If
      Return nodeAttribute
   End Function

   Public Shared Sub AppendParts( _
               ByVal xmlOut As Xml.XmlDocument, _
               ByVal node As Xml.XmlNode, _
               ByVal name As String, _
               ByVal partString As String)
      Dim parts() As String

      parts = partString.Split(","c)
      For Each s As String In parts
         node.AppendChild(xmlHelpers.NewElement(xmlOut, name, s))
      Next
   End Sub

   Public Shared Sub AppendIfExists(ByVal node As Xml.XmlNode, ByVal nodeChild As Xml.XmlNode)
      If Not nodeChild Is Nothing Then
         node.AppendChild(nodeChild)
      End If
   End Sub

   Public Shared Function GetSchemaForNode( _
                  ByVal nodeName As String, _
                  ByVal xsdDoc As Xml.XmlDocument) _
                  As Xml.XmlNode
      If Not xsdDoc Is Nothing Then
         Dim namespaceManager As New Xml.XmlNamespaceManager( _
                        New System.Xml.NameTable)
         namespaceManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")
         Return xsdDoc.SelectSingleNode("//xs:element[@name='" & nodeName & "']", _
                        namespaceManager)
      End If
   End Function

   Public Shared Function LoadFile( _
            ByVal xmlFileName As String, _
            ByVal xsdDoc As Xml.XmlDocument) _
            As Xml.XmlDocument
      Dim xmlDoc As New Xml.XmlDocument
      Dim textReader As Xml.XmlTextReader
      Dim validReader As Xml.XmlValidatingReader
      xsdDoc = Nothing
      Try
         xmlFileName = xmlFileName
         textReader = New Xml.XmlTextReader(xmlFileName)
         validReader = New Xml.XmlValidatingReader(textReader)
         validReader.ValidationType = Xml.ValidationType.Schema
         xmlDoc.Load(validReader)
      Finally
         textReader.Close()
      End Try
      Return xmlDoc
   End Function

#End Region

#Region "Protected and Friend Methods and Properties - empty"
#End Region

#Region "Private Methods and Properties - empty"
#End Region

End Class
