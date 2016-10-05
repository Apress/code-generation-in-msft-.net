' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Utilties for making project files and moving projects

Option Strict On
Option Explicit On 

Imports System

Public Class ProjectSupport
   Const vblf As String = Microsoft.VisualBasic.ControlChars.Lf

#Region "UpdateProjectFile with new source flies"
   Public Shared Function UpdateProjectFile( _
               ByVal projectFileName As String, _
               ByVal fileListXMLFileName As String, _
               ByVal directoryList As String) _
               As IO.Stream
      Dim prjXML As New Xml.XmlDataDocument
      Dim nodes As Xml.XmlNodeList
      Dim stream As New IO.MemoryStream
      Dim reloadDirs() As String
      Dim listXML As New Xml.XmlDocument
      Dim nodeInclude As Xml.XmlNode
      reloadDirs = SplitToArray(directoryList)
      listXML.Load(fileListXMLFileName)
      prjXML.Load(projectFileName)
      nodeInclude = prjXML.SelectSingleNode("//Files/Include")
      nodes = prjXML.SelectNodes("//Files/Include/*")

      ' Delete files in reload dirs
      For Each node As Xml.XmlNode In nodes
         For Each dirName As String In reloadDirs
            If Utility.Tools.GetAttributeOrEmpty(node, "RelPath"). _
                        StartsWith(dirName) Then
               nodeInclude.RemoveChild(node)
            End If
         Next
      Next

      ' Add back the files in these directories recursively
      ' TODO: Test the following change
      For Each dirName As String In reloadDirs
         nodes = listXML.SelectNodes( _
                        "//FileList//Dir[starts-with(@Name,'" & _
                        dirname & "')]/*")
         For Each node As Xml.XmlNode In nodes
            If Not Utility.Tools.GetAttributeOrEmpty(node, "Exclude") = _
                        "true" Then
               LoadVBFiles(node, nodeInclude, _
                        Utility.Tools.GetAttributeOrEmpty( _
                           Node.ParentNode, "Name") & "\")
            End If
         Next
      Next

      prjXML.Save(stream)
      Return stream
   End Function


   Private Shared Sub LoadVBFiles( _
            ByVal fileNode As Xml.XmlNode, _
            ByVal includeNode As Xml.XmlNode, _
            ByVal relDir As String)
      Dim dirNodes As Xml.XmlNodeList
      Dim fileNodes As Xml.XmlNodeList
      relDir &= Utility.Tools.GetAttributeOrEmpty(fileNode, "Name") & "\"
      dirNodes = fileNode.SelectNodes("Dir")
      fileNodes = fileNode.SelectNodes("File[@Ext='.vb' or @Ext='.cs']")
      For Each dNode As Xml.XmlNode In dirNodes
         LoadVBFiles(dNode, includeNode, relDir)
      Next
      For Each fNode As Xml.XmlNode In fileNodes
         includeNode.AppendChild( _
               MakeNewVBIncludeNode( _
                  includeNode.OwnerDocument, _
                  relDir & Utility.Tools.GetAttributeOrEmpty(fNode, "Name"), _
                  Utility.Tools.GetAttributeOrEmpty(fNode, "Ext")))
      Next
   End Sub

   Private Shared Function MakeNewVBIncludeNode( _
               ByVal xmldoc As Xml.XmlDocument, _
               ByVal fileName As String, _
               ByVal ext As String) _
               As Xml.XmlNode
      Dim node As Xml.XmlNode = xmldoc.CreateElement("File")
      Dim subType As String
      Dim buildAction As String
      node.Attributes.Append(Utility.xmlHelpers.NewAttribute(xmldoc, _
                  "RelPath", fileName))
      Select Case ext.ToUpper
         Case ".VB"
            subType = "Code"
            buildAction = "Compile"
      End Select
      Return node
      node.Attributes.Append(Utility.xmlHelpers.NewAttribute(xmldoc, _
                  "SubType", subType))
      node.Attributes.Append(Utility.xmlHelpers.NewAttribute(xmldoc, _
                  "BuildAction", buildAction))
   End Function


#End Region

#Region "Create New Solution from existing project"
   Public Shared Function CreateNewSolution( _
               ByVal nodeDirective As Xml.XmlNode, _
               ByVal rootSourceName As String, _
               ByVal slnFile As String, _
               ByVal SkipDirectories As String, _
               ByVal EmptyDirectories As String, _
               ByVal basePath As String) _
               As IO.Stream
      ' The memory stream will be returned empty as this is a self  
      ' contained process and nothing is output
      Dim stream As New IO.MemoryStream
      Dim rootTargetName As String
      Dim emptyDirArray() As String
      Dim nsmgr As New Xml.XmlNamespaceManager( _
                  nodeDirective.OwnerDocument.NameTable)
      Dim nodeFilePaths As Xml.XmlNode = Generation.GetFilePathNode
      nsmgr.AddNamespace("kg", "http://kadgen.com/KADGenDriving.xsd")
      Dim dirSourceRoot As IO.DirectoryInfo
      Dim dirTargetRoot As IO.DirectoryInfo
      rootTargetName = Utility.Tools.GetAttributeOrEmpty( _
                  nodeDirective, "kg:SinglePass", "OutputFile", nsmgr)
      rootTargetName = Utility.Tools.FixPath(rootTargetName, basePath, Nothing, nodeFilePaths)
      dirSourceRoot = New IO.DirectoryInfo(rootSourceName)
      dirTargetRoot = New IO.DirectoryInfo(rootTargetName)

      emptyDirArray = SplitToArray(EmptyDirectories)
      BuildDirectoryStructure(dirSourceRoot, _
                     dirTargetRoot, _
                     SplitToArray(SkipDirectories), _
                     emptyDirArray)
      If slnFile.IndexOf("\") < 0 Then
         slnFile = rootSourceName & "\" & slnFile
      End If
      UpdateProjectAndSolution(slnFile, rootSourceName, rootTargetName, _
                     emptyDirArray)

      Return stream
   End Function

   Private Shared Sub BuildDirectoryStructure( _
              ByVal dirSourceRoot As IO.DirectoryInfo, _
              ByVal dirTargetRoot As IO.DirectoryInfo, _
              ByVal skipDirectories As String(), _
              ByVal emptyDirectories As String())
      Dim dirs As IO.DirectoryInfo()
      Dim dirName As String
      Dim dirNew As IO.DirectoryInfo
      Dim childEmptyArray As String()
      dirs = dirSourceRoot.GetDirectories
      For Each dir As IO.DirectoryInfo In dirs
         If Array.IndexOf(skipDirectories, dir.Name) < 0 Then
            dirNew = dirTargetRoot.CreateSubdirectory(dir.Name)
            dirNew.Create()
            If Not (emptyDirectories(0) = "*" Or _
                           Array.IndexOf(emptyDirectories, dir.Name) >= 0) Then
               'Copy the files - there has to be a better way!
               Dim files As IO.FileInfo() = dir.GetFiles
               For Each file As IO.FileInfo In files
                  If file.Extension.ToLower = ".vbproj" Or _
                           file.Extension.ToLower = ".csproj" Then
                     ' Don't copy file now because you can't update project GUID
                  Else
                     file.CopyTo(dirNew.FullName & "\" & file.Name)
                  End If
               Next
               childEmptyArray = emptyDirectories
            Else
               childEmptyArray = New String() {"*"}
            End If
            BuildDirectoryStructure(dir, dirNew, skipDirectories, _
                           childEmptyArray)
         End If
      Next
   End Sub

   Private Shared Sub UpdateProjectAndSolution( _
                      ByVal slnFile As String, _
                      ByVal sourceDir As String, _
                      ByVal targetDir As String, _
                      ByVal emptyDirectories As String())
      ' NOTE: The logic in this routine counts on unique GUIDS
      ' Open the file and read project
      Dim reader As New IO.StreamReader(slnFile)
      Dim xmlDoc As New Xml.XmlDocument
      Dim nodes As Xml.XmlNodeList
      Dim writer As IO.StreamWriter
      Dim sln As String = reader.ReadToEnd
      Dim slnLines As String()
      Dim sAttr As String()
      Dim fileNames() As String
      Dim GUIDLookup As String()()
      Dim stream As IO.Stream
      Dim sProj As String
      reader.Close()

      slnLines = sln.Split(CChar(vblf))
      For Each s As String In slnLines
         If s.Trim.ToLower.StartsWith("project(") Then
            If GUIDLookup Is Nothing Then
               ReDim GUIDLookup(0)
               ReDim fileNames(0)
            Else
               ReDim Preserve GUIDLookup(GUIDLookup.GetUpperBound(0) + 1)
               ReDim Preserve fileNames(fileNames.GetUpperBound(0) + 1)
            End If
            sAttr = s.Split(","c)
            ' Second position is file, third is GUID  with junk after
            sAttr(2) = sAttr(2).Substring(sAttr(2).IndexOf("{") + 1)
            sAttr(2) = sAttr(2).Substring(0, sAttr(2).IndexOf("}"))
            fileNames(fileNames.GetUpperBound(0)) = _
                        sAttr(1).Replace("""", "").Trim
            GUIDLookup(GUIDLookup.GetUpperBound(0)) = _
                        New String() {New Guid(sAttr(2)).ToString, _
                        Guid.NewGuid.ToString}
         End If
      Next
      sln = ReplaceArray(sln, GUIDLookup)
      slnFile = IO.Path.GetFileName(slnFile)
      writer = New IO.StreamWriter(IO.Path.Combine(targetDir, slnFile))
      writer.Write(sln)
      writer.Flush()
      writer.Close()

      For Each fileName As String In fileNames
         reader = New IO.StreamReader(IO.Path.Combine(IO.Path.Combine( _
                        sourceDir, IO.Path.GetDirectoryName(slnFile)), fileName))
         sProj = reader.ReadToEnd
         reader.Close()
         sProj = ReplaceArray(sProj, GUIDLookup)
         writer = New IO.StreamWriter(IO.Path.Combine(IO.Path.Combine( _
                        targetDir, IO.Path.GetDirectoryName(slnFile)), fileName))
         writer.Write(sProj)
         writer.Flush()
         writer.Close()

         xmlDoc.Load(IO.Path.Combine(IO.Path.Combine(targetDir, _
                        IO.Path.GetDirectoryName(slnFile)), fileName))
         For Each exclude As String In emptyDirectories
            nodes = xmlDoc.SelectNodes( _
                        "//Files/Include/File[starts-with(@RelPath,'" & _
                        exclude & "')]")
            For Each node As Xml.XmlNode In nodes
               node.ParentNode.RemoveChild(node)
            Next
         Next
         xmlDoc.Save(IO.Path.Combine(IO.Path.Combine(targetDir, _
                        IO.Path.GetDirectoryName(slnFile)), fileName))
      Next
   End Sub

   Private Shared Sub ReplaceArray(ByVal sourceFile As String, _
                     ByVal targetFile As String, _
                     ByVal repArray As String()())
      Dim reader As New IO.StreamReader(sourceFile)
      Dim writer As New IO.StreamWriter(targetFile)
      Dim s As String = reader.ReadToEnd
      reader.Close()
      s = ReplaceArray(s, repArray)
      writer.Write(s)
      writer.Flush()
      writer.Close()
   End Sub

   Private Shared Function ReplaceArray(ByVal sln As String, _
                     ByVal repArray As String()()) _
                     As String
      For i As Int32 = 0 To repArray.GetUpperBound(0)
         sln = sln.Replace(repArray(i)(0), repArray(i)(1))
         sln = sln.Replace(repArray(i)(0).ToLower, repArray(i)(1).ToLower)
         sln = sln.Replace(repArray(i)(0).ToUpper, repArray(i)(1).ToUpper)
      Next
      Return sln
   End Function

   Private Shared Function SplitToArray(ByVal s As String) As String()
      Dim sarr As String()
      If s.StartsWith("{") Then
         s = s.Substring(1)
      End If
      If s.EndsWith("}") Then
         s = s.Substring(0, s.Length - 1)
      End If
      sarr = s.Split(","c)
      For i As Int32 = 0 To sarr.GetUpperBound(0)
         sarr(i) = sarr(i).Trim
      Next
      Return sarr
   End Function
#End Region

End Class
