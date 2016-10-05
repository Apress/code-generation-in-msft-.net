' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Tools to facilitate working with files.

Option Strict On
Option Explicit On 

Imports system

Public Class FileTools
   Public Shared Function GetFileListXML( _
               ByVal startDir As String) _
               As IO.Stream
      Dim xmlDoc As New Xml.XmlDocument
      Dim node As Xml.XmlNode
      Dim dir As New IO.DirectoryInfo(startDir)
      Dim stream As New IO.MemoryStream
      xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"))
      'node = xmlDoc.CreateElement("fil:FileLists", "http://kadgen/filelist.xsd")
      'xmlDoc.AppendChild(node)
      node = xmlDoc.AppendChild(xmlHelpers.NewElement("fil", _
                     "http://kadgen/filelist.xsd", _
                     xmlDoc, "FileLists"))
      node = node.AppendChild(xmlHelpers.NewElement( _
                     "http://kadgen/filelist.xsd", _
                     xmlDoc, "fil:FileList"))
      node.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, "StartDir", _
                  startDir))
      node.AppendChild(FileListXML(dir, xmlDoc))
      xmlDoc.Save(stream)
      Return stream
   End Function

   Private Shared Function FileListXML( _
               ByVal dir As IO.DirectoryInfo, _
               ByVal xmlDoc As Xml.XmlDocument) _
               As Xml.XmlNode
      Dim node As Xml.XmlNode = xmlHelpers.NewElement( _
                     "http://kadgen/filelist.xsd", _
                     xmlDoc, "fil:Dir", dir.Name)
      node.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, "Name", _
                  dir.Name))
      Dim dirs As IO.DirectoryInfo()
      Dim files As IO.FileInfo()
      node.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, _
                   "FullName", dir.FullName))
      dirs = dir.GetDirectories
      files = dir.GetFiles()

      For Each d As IO.DirectoryInfo In dirs
         node.AppendChild(FileListXML(d, xmlDoc))
      Next

      For Each f As IO.FileInfo In files
         node.AppendChild(FileInfoXML(xmlDoc, f))
      Next
      Return node
   End Function

   Private Shared Function FileInfoXML( _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal f As IO.FileInfo) _
               As Xml.XmlNode
      Dim child As Xml.XmlNode
      child = xmlHelpers.NewElement( _
                     "http://kadgen/filelist.xsd", _
                     xmlDoc, "fil:File", f.Name)
      child.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, _
                   "Ext", f.Extension))
      child.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, _
                   "FullName", f.FullName))
      child.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, _
                   "Attributes", f.Attributes.ToString))
      child.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, _
                   "CreationUTC", f.CreationTimeUtc.ToString))
      child.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, _
                   "LastWriteUTC", f.LastWriteTimeUtc.ToString))
      child.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, _
                   "Length", f.Length.ToString))
      Return child
   End Function
End Class
