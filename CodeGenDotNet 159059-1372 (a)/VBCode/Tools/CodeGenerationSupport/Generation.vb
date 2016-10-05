' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Main file for managing the Generation process

Option Strict On
Option Explicit On 

Imports System

Public Structure OutputInfo
   Public StartTime As DateTime  'Fields aren't displayed by default in the grid
   Public EndTime As DateTime
   Private mSection As String
   Private mDirectiveName As String
   Private mDirectiveType As String
   Private mSelectFile As String
   Private mInputFile As String
   Private mTemplate As String
   Private mStatus As String
   Private mOutputRule As String
   Private mCountSelected As Int32
   Private mCountCreated As Int32
   Private mCountOutput As Int32

   Public Property Section() As String
      Get
         Return mSection
      End Get
      Set(ByVal Value As String)
         mSection = Value
      End Set
   End Property

   Public Property DirectiveName() As String
      Get
         Return mDirectiveName
      End Get
      Set(ByVal Value As String)
         mDirectiveName = Value
      End Set
   End Property

   Public Property DirectiveType() As String
      Get
         Return mDirectiveType
      End Get
      Set(ByVal Value As String)
         mDirectiveType = Value
      End Set
   End Property

   Public Property SelectFile() As String
      Get
         Return mSelectFile
      End Get
      Set(ByVal Value As String)
         mSelectFile = Value
      End Set
   End Property

   Public Property InputFile() As String
      Get
         Return mInputFile
      End Get
      Set(ByVal Value As String)
         mInputFile = Value
      End Set
   End Property

   Public Property Template() As String
      Get
         Return mTemplate
      End Get
      Set(ByVal Value As String)
         mTemplate = Value
      End Set
   End Property

   Public Property Status() As String
      Get
         Return mStatus
      End Get
      Set(ByVal Value As String)
         mStatus = Value
      End Set
   End Property

   Public Property OutputRule() As String
      Get
         Return mOutputRule
      End Get
      Set(ByVal Value As String)
         mOutputRule = Value
      End Set
   End Property

   Public Property CountSelected() As Int32
      Get
         Return mCountSelected
      End Get
      Set(ByVal Value As Int32)
         mCountSelected = Value
      End Set
   End Property

   Public Property CountCreated() As Int32
      Get
         Return mCountCreated
      End Get
      Set(ByVal Value As Int32)
         mCountCreated = Value
      End Set
   End Property

   Public Property CountOutput() As Int32
      Get
         Return mCountOutput
      End Get
      Set(ByVal Value As Int32)
         mCountOutput = Value
      End Set
   End Property

   Public ReadOnly Property Elapsed() As TimeSpan
      Get
         Return EndTime.Subtract(StartTime)
      End Get
   End Property

End Structure

'! Class Summary: 
Public Class Generation
#Region "Class level declarations"
   Private Delegate Function GenerateDelegate( _
               ByVal outputFileName As String, _
               ByVal nsmgr As Xml.XmlNamespaceManager, _
               ByVal node As Xml.XmlNode, _
               ByVal nodeSelect As Xml.XmlNode) _
               As IO.Stream
   '  Const vbcrlf As String = Microsoft.VisualBasic.ControlChars.CrLf
   Private mLogEntries As Collections.ArrayList
   Private mOutput As Collections.ArrayList
   Private mCurrentOutput As OutputInfo
   ' The following fields are used to avoid repeatedly loading XML files
   Private mNavMetadata As Xml.XPath.XPathNavigator
   Private mParams() As CodeGenerationSupport.Param
   Private mExtObjects() As CodeGenerationSupport.ExtObject
   Private mXSLTTransform As Xml.Xsl.XslTransform
   Private mConnection As Data.SqlClient.SqlConnection
   ' The following fields avoid repeated reflection look ups 
   Private mMethodInfo As Reflection.MethodInfo
   Private Shared mNodeFilePaths As Xml.XmlNode
   Private mNodeFilters As Xml.XmlNode
   Private mDocPath As String
   Private mProjectSettings As New ProjectSettings
   Private mLocalSettings As New LocalSettings
   Private mSourceControl As Utility.SourceBase
   Private mCallBack As Utility.IProgressCallback
   Private mXsdDoc As Xml.XmlDocument
   Private mCntDone As Int32
   Private mbCancel As Boolean

   'Private Const vbcr As String = Microsoft.VisualBasic.ControlChars.Cr
   'Private Const vblf As String = Microsoft.VisualBasic.ControlChars.Lf

#End Region

#Region "Public and Friend Methods and Properties"

   Friend Shared Function GetFilePathNode() As Xml.XmlNode
      Return mNodeFilePaths
   End Function

   Public Function RunGeneration( _
                  ByVal xmlDoc As Xml.XmlDocument, _
                  ByVal xsdDoc As Xml.XmlDocument, _
                  ByVal callBack As Utility.IProgressCallback, _
                  ByRef cntDone As Int32, _
                  ByRef output As Collections.ArrayList, _
                  ByVal xmlDocFileName As String) _
                  As Collections.ArrayList
      ' The sections just help order things
      ' XPathNavigators must be used to support sorting

      mLogEntries = New Collections.ArrayList

      Dim xpathNav As Xml.XPath.XPathNavigator = xmlDoc.CreateNavigator
      'Dim xpathExpr3 As Xml.XPath.XPathExpression
      Dim nodeItSection As Xml.XPath.XPathNodeIterator
      Dim nodeItDirective As Xml.XPath.XPathNodeIterator
      ' Dim xAttr As Xml.XmlNode
      Dim nsmgr As Xml.XmlNamespaceManager = GetNameSpaceManager(xmlDoc, "kg")
      Dim cntNodes As Int32 = CountNodes(xmlDoc, nsmgr)
      Dim percentDone As Int32
      Dim currentDirName As String
      Dim sectionName As String

      ' Push current directory info 
      currentDirName = Environment.CurrentDirectory
      Environment.CurrentDirectory = IO.Path.GetFullPath(IO.Path.GetDirectoryName(xmlDocFileName))

      mCallBack = callBack
      mXsdDoc = xsdDoc
      mCntDone = cntDone
      mOutput = output

      SetLocalSettings(xmlDoc, nsmgr)
      SetProjectSettings(xmlDoc, nsmgr)
      SetupSourceSafe()

      nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")

      mNodeFilePaths = xmlDoc.SelectSingleNode("kg:GenerationScript/kg:FilePaths", _
                           nsmgr)
      mNodeFilters = xmlDoc.SelectSingleNode("kg:GenerationScript/kg:Filters", _
                           nsmgr)
      mDocPath = IO.Path.GetDirectoryName(xmlDocFileName)

      nodeItSection = Utility.NodeItHelpers.GetNodeIt(xpathNav, _
                  "//kg:Section[kg:Standard/@Checked='true']", nsmgr)
      Do While nodeItSection.MoveNext
         sectionName = Utility.NodeItHelpers.GetChildAttribute(nodeItSection.Current, "kg:Standard", "Name", nsmgr)
         nodeItDirective = Utility.NodeItHelpers.GetSortedNodeIt(nodeItSection.Current, _
                        "./*", nsmgr, "Ordinal", True, False, True)
         Do While nodeItDirective.MoveNext
            mCurrentOutput = New OutputInfo
            mCurrentOutput.Section = sectionName
            mCurrentOutput.DirectiveName = Utility.NodeItHelpers.GetChildAttribute(nodeItDirective.Current, "kg:Standard", "Name", nsmgr)
            mCurrentOutput.DirectiveType = nodeItDirective.Current.LocalName
            mCurrentOutput.CountSelected = nodeItSection.Count
            If Utility.NodeItHelpers.GetChildAttribute(nodeItDirective.Current, "kg:Standard", "Checked", nsmgr) <> "false" _
                           Then
               mCntDone += 1
               Windows.Forms.Application.DoEvents()
               If callBack.GetCancel Or mbCancel Then
                  If Windows.Forms.MessageBox.Show( _
                           "Do you really want to cancel process?", _
                           "Confirm Cancel", _
                           Windows.Forms.MessageBoxButtons.YesNo) = _
                           Windows.Forms.DialogResult.Yes Then
                     Exit Do
                  Else
                     callBack.ResetCancel()
                  End If
               End If
               If cntNodes = 0 Then
                  callBack.UpdateProgress(100)
               Else
                  percentDone = CInt(100 * mCntDone / cntNodes)
                  If percentDone > 100 Then
                     percentDone = 100
                  End If
                  callBack.UpdateProgress(percentDone)
               End If
               callBack.UpdateCurrentNode(CType(nodeItDirective.Current, Xml.IHasXmlNode).GetNode)
               ' This is also updated in Multipas
               callBack.UpdateCurrentFile("")
               RunGenerationNode(nodeItDirective.Current, nsmgr, xsdDoc)
            Else
               mCurrentOutput.Status = "Not Checked"
            End If
            mOutput.Add(Me.mCurrentOutput)
         Loop
      Loop

      cntDone = mCntDone

      ' Pop current directory
      Environment.CurrentDirectory = currentDirName
      Return mLogEntries
   End Function
#End Region

#Region "Protected and Friend Methods and Properties -empty"
#End Region

#Region "Protected Event Response Methods -empty"
#End Region

#Region "Private Methods and Properties"

   Private Sub SetLocalSettings( _
                     ByVal xmlDoc As Xml.XmlDocument, _
                     ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim sLocalFile As String
      Dim elem As Xml.XmlNode
      Dim xmlLocalDoc As New Xml.XmlDocument
      elem = xmlDoc.SelectSingleNode("kg:GenerationScript", nsmgr)
      nsmgr.AddNamespace("kl", "http://kadgen.com/KADGenLocalSettings.xsd")
      sLocalFile = Utility.Tools.GetAttributeOrEmpty(elem, "LocalSettings")
      If IO.File.Exists(sLocalFile) Then
         xmlLocalDoc.Load(sLocalFile)
         mLocalSettings.Node = xmlLocalDoc.SelectSingleNode("//kl:LocalSettings", nsmgr)
      End If
   End Sub

   Private Sub SetProjectSettings( _
                     ByVal xmlDoc As Xml.XmlDocument, _
                     ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim sProjectFile As String
      Dim elem As Xml.XmlNode
      Dim xmlLocalDoc As New Xml.XmlDocument
      elem = xmlDoc.SelectSingleNode("kg:GenerationScript", nsmgr)
      sProjectFile = Utility.Tools.GetAttributeOrEmpty(elem, "ProjectSettings")
      If IO.File.Exists(sProjectFile) Then
         xmlLocalDoc.Load(sProjectFile)
         nsmgr.AddNamespace("kp", "http://kadgen.com/KADGenProjectSettings.xsd")
         mProjectSettings.Node = xmlLocalDoc.SelectSingleNode("//kp:ProjectSettings", nsmgr)
      End If
   End Sub

   Private Sub SetupSourceSafe()
      Try
         ' NOTE: change the following for alternate sourcecontrol tools
         Try
            ' I am opening this via reflection so people that don't have SourceSafe
            ' can run this tool
            Dim asm As Reflection.Assembly
            Dim path As String = System.Windows.Forms.Application.ExecutablePath
            path = IO.Path.GetDirectoryName(path)
            path = IO.Path.Combine(path, "SourceSafeTools.dll")
            asm = Reflection.Assembly.LoadFile(path)
            If Not asm Is Nothing Then
               mSourceControl = CType(asm.CreateInstance("KADGen.SourceSafeTools.SourceSafe"), Utility.SourceBase)
            End If
            'Simple instatiation blows a compile error where people don't have sourcesafe
            'If you Then 've got SourceSafe, reference the SourceSafeTools directory and 
            'uncomment the following
            'mSourceControl = New SourceSafeTools.SourceSafe("")
         Catch ex As System.Exception
            Diagnostics.Debug.WriteLine(ex)
            ' TODO: Determine whether missing source control is an error
            ' It probably won't be an error here, as you might not have any
            ' files marked for Checkout. OTOH, this is an earlier location to 
            ' get an exception.
         End Try

      Catch ex As Exception
         Diagnostics.Debug.WriteLine("No SourceSafe")
      End Try
   End Sub

   Private Sub RunGenerationNode( _
                  ByVal nav As Xml.xpath.XPathNavigator, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal xsdDoc As Xml.XmlDocument)
      Try
         Dim node As Xml.XmlNode = CType(nav, Xml.IHasXmlNode).GetNode
         Dim nodeXSD As Xml.XmlNode = Utility.xmlHelpers.GetSchemaForNode( _
                              node.LocalName, xsdDoc)
         Dim methodInfo As Reflection.MethodInfo
         If node.LocalName <> "Standard" Then
            methodInfo = Utility.Tools.GetMethodInfo(nodeXSD, GetType(Generation), _
                                 nsmgr, mLocalSettings.GetBasePath(), mDocPath, _
                                 mNodeFilePaths)

         End If
         If Not methodInfo Is Nothing Then
            methodInfo.Invoke(Me, New Object() {node, nsmgr})
         End If
      Catch ex As System.Exception
         Diagnostics.Debug.WriteLine(ex.ToString)
         Throw
      End Try
   End Sub

   Private Sub XSLTProcess( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)

      Dim basePath As String = mLocalSettings.GetBasePath()
      Dim outputFile As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:SinglePass", "OutputFile", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim inputXML As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:XSLTFiles", "InputFileName", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim XSLTFileName As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:XSLTFiles", "XSLTFileName", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim extObjects() As ExtObject = GetExtObjects(node, nsmgr)
      Dim stream As IO.Stream
      Dim madeFile As Boolean
      Dim countOutput As Int32
      Dim outputType As String = Utility.Tools.GetAttributeOrEmpty(node, _
                  "kg:OutputRules", "OutputFileType", nsmgr)
      CurrentOutputStart("None", inputXML, XSLTFileName, 1, Me.GetOutType(node).ToString)
      Try
         If ShouldRun(outputFile, node) Then
            If SSCheckOutFile(node, basePath, outputFile) Then
               Try
                  stream = CodeGenerationSupport.XSLTSupport.GenerateViaXSLT( _
                                 XSLTFileName, inputXML, outputType, extObjects)
                  madeFile = MarkWriteAndClose(stream, node, outputFile)
               Finally
                  SSCheckInFile(node, basePath, outputFile, madeFile)
               End Try
            End If
         End If
         If madeFile Then
            countOutput = 1
         End If
         CurrentOutputEnd("Done", 1, countOutput)

      Catch ex As System.Exception
         mCurrentOutput.Status = "Error"
         Throw
      End Try
   End Sub


   Private Sub XSLTCheck( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)

      Dim basePath As String = mLocalSettings.GetBasePath()
      Dim bAbortOnOutput As Boolean = (Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:SinglePass", "OutputFile", nsmgr) = "true")
      Dim outputFile As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:SinglePass", "OutputFile", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim inputXML As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:XSLTFiles", "InputFileName", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim XSLTFileName As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:XSLTFiles", "XSLTFileName", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim extObjects() As ExtObject = GetExtObjects(node, nsmgr)
      Dim stream As IO.Stream
      Dim madeFile As Boolean
      Dim countOutput As Int32
      Dim outputType As String = Utility.Tools.GetAttributeOrEmpty(node, _
                  "kg:OutputRules", "OutputFileType", nsmgr)
      CurrentOutputStart("None", inputXML, XSLTFileName, 1, Me.GetOutType(node).ToString)
      Try
         If ShouldRun(outputFile, node) Then
            If SSCheckOutFile(node, basePath, outputFile) Then
               Try
                  stream = CodeGenerationSupport.XSLTSupport.GenerateViaXSLT( _
                                 XSLTFileName, inputXML, outputType, extObjects)
                  madeFile = MarkWriteAndClose(stream, node, outputFile)
                  If madeFile And bAbortOnOutput Then
                     mbCancel = True
                  End If
               Finally
                  SSCheckInFile(node, basePath, outputFile, madeFile)
               End Try
            End If
         End If
         If madeFile Then
            countOutput = 1
         End If
         CurrentOutputEnd("Done", 1, countOutput)

      Catch ex As System.Exception
         mCurrentOutput.Status = "Error"
         Throw
      End Try
   End Sub


   Private Sub RunProcess( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim basePath As String = mLocalSettings.GetBasePath()
      Dim outputFile As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:SinglePass", "OutputFile", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim stream As IO.Stream
      Dim objs() As Object
      Dim madeFile As Boolean
      Dim countOutput As Int32
      SetProcess(node, nsmgr)
      objs = SetProcessParameters(node, nsmgr)
      CurrentOutputStart("None", mMethodInfo.Name, Me.GetProcessName(node, nsmgr), 1, _
               Me.GetOutType(node).ToString)
      Try
         If ShouldRun(outputFile, node) Then
            If SSCheckOutFile(node, basePath, outputFile) Then
               Try
                  stream = CType(mMethodInfo.Invoke(Me, objs), IO.Stream)
                  SSCheckOutFile(node, basePath, outputFile)
                  madeFile = MarkWriteAndClose(stream, node, outputFile)
               Finally
                  SSCheckInFile(node, basePath, outputFile, madeFile)
               End Try
            End If
         End If
         If madeFile Then
            countOutput = 1
         End If
         CurrentOutputEnd("Done", 1, countOutput)
      Catch ex As System.Exception
         CurrentOutputEnd("Error", 1, countOutput)
         Throw
      End Try
   End Sub

   Private Sub CreateMetadata( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim basePath As String = mLocalSettings.GetBasePath()
      Dim server As String = Utility.Tools.GetAttributeOrEmpty(node, "Server")
      Dim mappingFileName As String = Utility.Tools.FixPath( _
                        Utility.Tools.GetAttributeOrEmpty( _
                              node, "MappingFileName"), _
                        basePath, mDocPath, mNodeFilePaths)
      Dim outputFile As String = Utility.Tools.FixPath( _
                        Utility.Tools.GetAttributeOrEmpty( _
                              node, "kg:SinglePass", "OutputFile", nsmgr), _
                        basePath, mDocPath, mNodeFilePaths)
      Dim databaseName As String = Utility.Tools.GetAttributeOrEmpty( _
                        node, "Database")
      databaseName = Utility.Tools.FixPath(databaseName, basePath, mDocPath, mNodeFilePaths)
      Dim SQLServerMetaData As New Metadata.SQLExtractMetaData(server)
      Dim xmlDoc As Xml.XmlDocument
      Dim skipStoredProcs As String = Utility.Tools.GetAttributeOrEmpty( _
                        node, "SkipStoredProcs")
      Dim selectPatterns As String = Utility.Tools.GetAttributeOrEmpty( _
                        node, "SelectPatterns")
      Dim setSelectPatterns As String = Utility.Tools.GetAttributeOrEmpty( _
                        node, "SetSelectPatterns")
      Dim removePrefix As String = Utility.Tools.GetAttributeOrEmpty( _
                        node, "RemovePrefix")
      Dim lookupPrefix As String = Utility.Tools.GetAttributeOrEmpty( _
                        node, "LookupPrefix")
      CurrentOutputStart("None", "N/A", "N/A", 1, Me.GetOutType(node).ToString)
      Try
         If ShouldRun(outputFile, node) Then
            If SSCheckOutFile(node, basePath, outputFile) Then
               Try
                  xmlDoc = SQLServerMetaData.CreateMetaData( _
                                   (skipStoredProcs = "true"), _
                                   selectPatterns, setSelectPatterns, _
                                   removePrefix, lookupPrefix, databaseName)
                  Utility.Tools.MakePathIfNeeded(outputFile)
                  xmlDoc.Save(outputFile)
               Finally
                  SSCheckInFile(node, basePath, outputFile, True)
               End Try
            End If
         End If

         CurrentOutputEnd("Done", 1, 1)
      Catch ex As System.Exception
         CurrentOutputEnd("Error", 1, 0)
         Throw
      End Try
   End Sub

   Private Sub MergeMetadata( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim baseXMLFileName As String
      Dim mergeXMLFileName As String
      Dim outputXMLFileName As String
      Dim basePath As String = mLocalSettings.GetBasePath()
      CurrentOutputStart("None", baseXMLFileName, mergeXMLFileName, 1, Me.GetOutType(node).ToString)
      Try
         ' TODO: Add overwriting protection
         baseXMLFileName = Utility.Tools.FixPath( _
                              Utility.Tools.GetAttributeOrEmpty(node, "BaseXMLFileName"), _
                              basePath, mDocPath, mNodeFilePaths)
         mergeXMLFileName = Utility.Tools.FixPath( _
                              Utility.Tools.GetAttributeOrEmpty(node, "MergingXMLFileName"), _
                              basePath, mDocPath, mNodeFilePaths)
         outputXMLFileName = Utility.Tools.FixPath( _
                              Utility.Tools.GetAttributeOrEmpty(node, "OutputXMLFileName"), _
                              basePath, mDocPath, mNodeFilePaths)
         ' TODO: Consider adding check out for these files, but generally they would not be checked in and out as they are transitory
         Metadata.MergeFreeForm.Merge(baseXMLFileName, mergeXMLFileName, outputXMLFileName)
         CurrentOutputEnd("Done", 1, 1)
      Catch ex As Exception
         CurrentOutputEnd("Error", 1, 0)
         Throw
      End Try
   End Sub

   Private Sub XSLTGeneration( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim basePath As String = mLocalSettings.GetBasePath()
      Dim metadataFileName As String = Utility.Tools.FixPath( _
                  Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:XSLTFiles", "InputFileName", nsmgr), _
                  basePath, mDocPath, mNodeFilePaths)
      Dim xmlDoc As New Xml.XmlDocument
      Dim xsltFile As String = Utility.Tools.FixPath( _
                  Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:XSLTFiles", "XSLTFileName", nsmgr), _
                  basePath, mDocPath, mNodeFilePaths)
      CurrentOutputStart(xsltFile)
      Try
         xmlDoc.Load(metadataFileName)
         KADGen.LibraryInterop.Singletons.XMLDoc = xmlDoc
         KADGen.LibraryInterop.Singletons.NsMgr = KADGen.Utility.xmlHelpers.BuildNamespacesManagerForDoc(xmlDoc)
         mNavMetadata = xmlDoc.CreateNavigator

         mParams = CodeGenerationSupport.XSLTSupport.GetXSLTParams(xsltFile, _
                     basePath)
         mExtObjects = GetExtObjects(node, nsmgr)
         mXSLTTransform = New Xml.Xsl.XslTransform
         mXSLTTransform.Load(xsltFile)
         MultiPass(node, nsmgr, AddressOf XSLTGenerationCallBack)
         mNavMetadata = Nothing
         mParams = Nothing
         mXSLTTransform = Nothing
         CurrentOutputEnd("Done")
      Catch ex As System.Exception
         CurrentOutputEnd("Error")
         Throw
      End Try
   End Sub


   Private Sub BruteForceGeneration( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      CurrentOutputStart(Me.GetProcessName(node, nsmgr))
      Try
         SetProcess(node, nsmgr)
         MultiPass(node, nsmgr, AddressOf BruteForceGenerationCallBack)
         mMethodInfo = Nothing
         mParams = Nothing
         CurrentOutputEnd("Done")
      Catch ex As System.Exception
         CurrentOutputEnd("Error")
         Throw
      End Try
   End Sub

   Private Sub CodeDOMGeneration( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      CurrentOutputStart(Me.GetProcessName(node, nsmgr))
      Try
         SetProcess(node, nsmgr)
         MultiPass(node, nsmgr, AddressOf CodeDOMGenerationCallBack)
         mMethodInfo = Nothing
         mParams = Nothing
         CurrentOutputEnd("Done")
      Catch ex As System.Exception
         CurrentOutputEnd("Error")
         Throw
      End Try
   End Sub

   Private Sub RunSQLScripts( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim basePath As String = mLocalSettings.GetBasePath()
      Dim serverName As String = Utility.Tools.GetAttributeOrEmpty(node, "Server")
      Dim databaseName As String = Utility.Tools.GetAttributeOrEmpty(node, "Database")
      databaseName = Utility.Tools.FixPath(databaseName, basePath, mDocPath, mNodeFilePaths)
      CurrentOutputStart(serverName & ":" & databaseName)
      Try
         mConnection = New Data.SqlClient.SqlConnection( _
               "workstation id=" & serverName & _
               ";packet size=4096;integrated security=SSPI" & _
               ";data source=" & serverName & _
               ";persist security info=False;initial catalog=" & databaseName)
         MultiPass(node, nsmgr, AddressOf RunSQLScriptsCallBack)
         CurrentOutputEnd("Done")
      Catch
         CurrentOutputEnd("Error")
         Throw
      Finally
         mConnection.Close()
      End Try
   End Sub

   Private Sub CopyFiles( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim sourceFileName As String = Utility.Tools.GetAttributeOrEmpty( _
                           node, "SourceFileName")
      Dim targetFileName As String = Utility.Tools.GetAttributeOrEmpty( _
                           node, "TargetFileName")
      sourceFileName = Utility.Tools.FixPath(sourceFileName, "", "", mNodeFilePaths)
      targetFileName = Utility.Tools.FixPath(targetFileName, "", "", mNodeFilePaths)
      CurrentOutputStart("None", sourceFileName, "None", 1, "")
      ' TODO: COnsider Source Control adn whether to allow overwriting files
      If sourceFileName.Trim.Length > 0 And targetFileName.Trim.Length > 0 Then
         If IO.File.Exists(sourceFileName) Then
            IO.File.Copy(sourceFileName, targetFileName)
         ElseIf IO.Directory.Exists(sourceFileName) Then
            Dim dirTarget As IO.DirectoryInfo = IO.Directory.CreateDirectory(targetFileName)
            Dim dirSource As New IO.DirectoryInfo(sourceFileName)
            CopyContainedFiles(dirSource, dirTarget)
         Else
            Throw New System.Exception("File " & sourceFileName & " Not Found")
         End If
      End If
         CurrentOutputEnd("Done", 1, 1)
   End Sub

   Private Sub NestedScript( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim xmlDoc As Xml.XmlDocument
      Dim xmlFileName As String
      ' TODO: Incorporate nested scripts in scrollbar, logging and output
      CurrentOutputStart("None", xmlFileName, "None", -1, "")
      xmlFileName = Utility.Tools.GetAttributeOrEmpty(node, "ScriptName")
      xmlFileName = Utility.Tools.FixPath(xmlFileName, "", "", mNodeFilePaths)
      xmlDoc = Utility.xmlHelpers.LoadFile(xmlFileName, mXsdDoc)
      RunGeneration(xmlDoc, mXsdDoc, mCallBack, mCntDone, mOutput, xmlFileName)
      CurrentOutputEnd("Done", 1, 1)
      'Windows.Forms.MessageBox.Show("Nested Script not yet supported")
   End Sub

   Private Sub MultiPass( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal delegGenerate As GenerateDelegate)
      Dim basePath As String = mLocalSettings.GetBasePath()
      Dim xmlDoc As New Xml.XmlDocument
      Dim nodeList As Xml.XmlNodeList
      Dim outputFileName As String
      Dim attr As Xml.XmlNode
      Dim stream As IO.Stream

      Dim outputDir As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:MultiPass", "OutputDir", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim outputFilePattern As String = Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:MultiPass", "OutputFilePattern", nsmgr)
      Dim selectXPath As String = Utility.Tools.FixFilter( _
                           Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:MultiPass", "SelectXPath", nsmgr), _
                           mNodeFilters)
      Dim selectNSPrefix As String = Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:MultiPass", "SelectNSPrefix", nsmgr)
      Dim selectNamespace As String = Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:MultiPass", "SelectNamespace", nsmgr)
      Dim selectFile As String = Utility.Tools.FixPath( _
                           Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:MultiPass", "SelectFile", nsmgr), _
                           basePath, mDocPath, mNodeFilePaths)
      Dim madeFile As Boolean
      If selectFile.Trim.Length > 0 Then
         If (Not Me.mCurrentOutput.InputFile Is Nothing) AndAlso _
                     Me.mCurrentOutput.InputFile.Trim.Length = 0 Then
            Me.mCurrentOutput.InputFile = selectFile
         End If
         xmlDoc.Load(selectFile)
         Dim nsmgrMeta As Xml.XmlNamespaceManager = _
                        Utility.Tools.BuildNameSpaceManager( _
                              xmlDoc, node, "kg:MultiPass", nsmgr, "Select")
         '  nsmgrMeta.AddNamespace("orm", "http://kadgen.com/KADORM.xsd")
         nsmgrMeta.AddNamespace("ffu", "http://kadgen/FreeFormForUI.xsd")
         nsmgrMeta.AddNamespace("ui", "http://kadgen.com/UserInterface.xsd")
         '   nsmgrMeta.AddNamespace(selectNSPrefix, selectNamespace)
         nodeList = xmlDoc.SelectNodes(selectXPath, nsmgrMeta)
         Me.mCurrentOutput.CountSelected = nodeList.Count
         For Each nodeSelect As Xml.XmlNode In nodeList
            madeFile = False
            outputFileName = IO.Path.Combine(outputDir, _
                        ParseFilenamePattern(outputFilePattern, nodeSelect, _
                        nsmgrMeta))
            mCallBack.UpdateCurrentFile(outputFileName)
            Utility.Tools.MakePathIfNeeded(outputFileName)
            If ShouldRun(outputFileName, node) Then
               If SSCheckOutFile(node, basePath, outputFileName) Then
                  Try
                     stream = delegGenerate.Invoke(outputFileName, nsmgr, node, _
                                        nodeSelect)
                     If Not stream Is Nothing Then
                        madeFile = MarkWriteAndClose(stream, node, outputFileName)
                     End If
                     Me.mCurrentOutput.CountCreated += 1
                     If madeFile Then
                        Me.mCurrentOutput.CountOutput += 1
                     End If
                  Finally
                     SSCheckInFile(node, basePath, outputFileName, madeFile)
                  End Try
               End If
            End If
         Next
      End If
   End Sub


   Private Function XSLTGenerationCallBack( _
                  ByVal outputFileName As String, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode, _
                  ByVal nodeSelect As Xml.XmlNode) _
                  As IO.Stream
      SetParameters(nodeSelect, outputFileName)
      Dim outputType As String = Utility.Tools.GetAttributeOrEmpty(node, _
                  "kg:OutputRules", "OutputFileType", nsmgr)
      Return CodeGenerationSupport.XSLTSupport.GenerateViaXSLT( _
                           mXSLTTransform, mNavMetadata, outputType, mExtObjects, mParams)
   End Function

   Private Function BruteForceGenerationCallBack( _
                  ByVal outputFileName As String, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode, _
                  ByVal nodeSelect As Xml.XmlNode) _
                  As IO.Stream
      SetParameters(nodeSelect, outputFileName)
      Return CType(mMethodInfo.Invoke(Me, BuildParamArray()), IO.Stream)
   End Function

   Private Function CodeDOMGenerationCallBack( _
                  ByVal outputFileName As String, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode, _
                  ByVal nodeSelect As Xml.XmlNode) _
                  As IO.Stream
      Dim outType As Utility.OutputType = CType( _
                           System.Enum.Parse(GetType(Utility.OutputType), _
                              Utility.Tools.GetAttributeOrEmpty( _
                                 node, "TargetLanguage")), Utility.OutputType)
      Dim compileUnit As CodeDom.CodeCompileUnit
      SetParameters(nodeSelect, outputFileName)
      Dim params As Object() = Me.BuildParamArray()
      If params Is Nothing Then
         compileUnit = CType(mMethodInfo.Invoke( _
                              Me, params), CodeDom.CodeCompileUnit)
      Else
         compileUnit = CType(mMethodInfo.Invoke( _
                              Me, BuildParamArray()), CodeDom.CodeCompileUnit)
      End If
      Return CodeGenerationSupport.CodeDOMSupport.GenerateViaCodeDOM( _
                           outType, compileUnit)
   End Function

   Private Function RunSQLScriptsCallBack( _
                  ByVal outputFileName As String, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode, _
                  ByVal nodeSelect As Xml.XmlNode) _
                  As IO.Stream
      RunScript(node, outputFileName)
      Return Nothing
   End Function

   Private Function ShouldRun( _
                  ByVal outputfile As String, _
                  ByVal node As Xml.XmlNode) _
                  As Boolean
      Dim outType As Utility.OutputType
      Dim fileChanged As Utility.FileChanged
      If node.Name = "kg:RunSQLScripts" Then
         ' Should Run is handled later
         Return True
      Else
         Dim nsmgr As Xml.XmlNamespaceManager = GetNameSpaceManager( _
                              node.OwnerDocument, "kg")
         Dim nodeStandard As Xml.XmlNode = node.SelectSingleNode( _
                              "kg:OutputRules", nsmgr)
         outType = Me.GetOutType(node)
         fileChanged = Utility.HashTools.IsFileChanged(outputfile, _
                              mProjectSettings.GetCommentStart(outType), _
                              mProjectSettings.GetCommentEnd(outType), _
                              mProjectSettings.GetHeaderMarker(), _
                              mProjectSettings.GetHashMarker())
         Return ShouldRun(Me.GetGenType(node), fileChanged, outputfile, False)
      End If
   End Function

   Private Function ShouldRun( _
                     ByVal genType As Utility.GenType, _
                     ByVal fileChanged As Utility.FileChanged, _
                     ByVal name As String, _
                     ByVal isRunSQLScript As Boolean) _
                     As Boolean
      Dim sMode As String = ""
      If isRunSQLScript Then
         sMode = "(Running SQL Script) "
      End If
      'If Not isRunSQLScript And _
      '         (IO.File.Exists(name) AndAlso (IO.File.GetAttributes(name) And _
      '                   IO.FileAttributes.ReadOnly) <> 0) Then
      '   LogError(name & " cannot output because file's ReadOnly - probably because of SourceSafe issues", name)
      'Else
      Select Case genType
         Case genType.Overwrite
            Return True
         Case genType.Always
            Select Case fileChanged
               Case fileChanged.Unknown
                  LogError("ERROR: " & sMode & "Autogenerated hash could not be recovered. " & _
                           "Generation of " & name & " aborted", name)
               Case fileChanged.Unknown, Utility.FileChanged.Changed
                  LogError("ERROR: " & sMode & "Autogenerated file was manually edited. " & _
                           "Generation of " & name & " aborted", name)
               Case Else
                  Return True
            End Select
         Case genType.None
            ' I have no idea why we'd be here
         Case genType.Once
            Select Case fileChanged
               Case Utility.FileChanged.FileDoesntExist
                  Return True
               Case Else
                  LogInfo("Information Only: " & sMode & name & _
                           " not generated because it already existed. " & _
                           "This could be a normal condition.", name)
            End Select
         Case genType.UntilEdited
            Select Case fileChanged
               Case Utility.FileChanged.FileDoesntExist, _
                           Utility.FileChanged.NotChanged
                  Return True
               Case Utility.FileChanged.Unknown
                  LogError("ERROR: " & sMode & "Autogenerated hash could not be recovered. " & _
                           "Generation of " & name & " aborted", name)
               Case Utility.FileChanged.Changed
                  LogInfo("Information Only: " & sMode & name & " not generated " & _
                          "because it existed and had been edited. This could " & _
                          "be a normal condition.", name)
            End Select
      End Select
      'End If
   End Function

   Private Function GetNameSpaceManager( _
                  ByVal xmlDoc As Xml.XmlDocument, _
                  ByVal prefix As String) _
                  As Xml.XmlNamespaceManager
      Dim nsmgr As New Xml.XmlNamespaceManager(xmlDoc.NameTable)
      Dim ns As String
      Select Case prefix
         Case "kg"
            ns = "http://kadgen.com/KADGenDriving.xsd"
         Case "dbs"
            ns = "http://kadgen/DatabaseStructure"
      End Select
      If Not ns Is Nothing Then
         nsmgr.AddNamespace(prefix, ns)
         Return nsmgr
      End If
   End Function

   Private Function GetOutType( _
                  ByVal node As Xml.XmlNode) _
                  As Utility.OutputType
      Dim outTypeString As String
      Dim nsmgr As Xml.XmlNamespaceManager = GetNameSpaceManager( _
                           node.OwnerDocument, _
                           "kg")
      Dim nodeStandard As Xml.XmlNode = node.SelectSingleNode( _
                           "kg:OutputRules", nsmgr)
      outTypeString = Utility.Tools.GetAttributeOrEmpty( _
                           nodeStandard, "OutputFileType")
      If outTypeString.Trim.Length = 0 Then
         Return Utility.OutputType.None
      Else
         Return CType(System.Enum.Parse(GetType(Utility.OutputType), _
                           outTypeString), Utility.OutputType)
      End If
   End Function

   Private Function GetApplyHash(ByVal node As Xml.XmlNode) As String
      Dim nsmgr As Xml.XmlNamespaceManager = Utility.Tools.BuildNameSpaceManager( _
                     node, "kg", False)
      Return Utility.Tools.GetAttributeOrEmpty(node, "kg:OutputRules", _
                           "HashOutput", nsmgr)
   End Function

   Private Function GetGenType( _
                  ByVal node As Xml.XmlNode) _
                  As Utility.GenType
      Dim GenTypeString As String
      Dim nsmgr As Xml.XmlNamespaceManager = _
                           GetNameSpaceManager(node.OwnerDocument, _
      "kg")
      Dim nodeStandard As Xml.XmlNode = node.SelectSingleNode( _
                           "kg:OutputRules", nsmgr)
      GenTypeString = Utility.Tools.GetAttributeOrEmpty(nodeStandard, _
                           "OutputGenType")
      Return CType(System.Enum.Parse(GetType(Utility.GenType), GenTypeString), _
                           Utility.GenType)
   End Function

   Private Function MarkWriteAndClose( _
                  ByVal stream As IO.Stream, _
                  ByVal node As Xml.XmlNode, _
                  ByVal outputFile As String) _
                  As Boolean
      Dim writer As IO.StreamWriter
      Dim reader As IO.StreamReader
      Dim outType As Utility.OutputType = Me.GetOutType(node)
      Dim genType As Utility.GenType = Me.GetGenType(node)
      Dim applyHash As String = Me.GetApplyHash(node)
      Dim madeFile As Boolean = stream.Length > 0
      If madeFile Then
         If applyHash = "true" Then
            stream = Utility.HashTools.ApplyHash(stream, _
                              mProjectSettings.GetCommentText(genType), _
                              mProjectSettings.GetCommentStart(outType), _
                              mProjectSettings.GetCommentEnd(outType), _
                              mProjectSettings.GetHeaderMarker(), _
                              mProjectSettings.GetHashMarker())
         End If
         Utility.Tools.MakePathIfNeeded(outputFile)
         writer = New IO.StreamWriter(outputFile)
         reader = New IO.StreamReader(stream)
         stream.Seek(0, IO.SeekOrigin.Begin)
         writer.Write(reader.ReadToEnd)
         writer.Flush()
         writer.Close()
         reader.Close()
      End If
      stream.Close()
      Return madeFile
   End Function

   Protected Overridable Function SSCheckOutFile( _
                     ByVal node As Xml.XmlNode, _
                     ByVal workingPath As String, _
                     ByVal file As String) As Boolean

      Dim status As Utility.SourceBase.ItemStatus
      Dim checkoutProject As String = GetSSCheckOutProject(node)
      Dim ret As Boolean
      If GetSSCheckOut(node) Then
         If (Not checkoutProject Is Nothing) AndAlso _
                     checkoutProject.Trim.Length > 0 Then
            checkoutProject = Utility.Tools.FixPath(checkoutProject, _
                     mLocalSettings.GetBasePath(), mDocPath, mNodeFilePaths)
            If mSourceControl Is Nothing Then
               ' TODO: Figure out whether this is an error
               Return True
            Else
               status = mSourceControl.CheckOut(file, workingPath, checkoutProject)
               ret = (status = Utility.SourceBase.ItemStatus.CheckedOutToMe Or _
                           status = Utility.SourceBase.ItemStatus.DoesntExist)
               If Not ret Then
                  LogError("Couldn't check file out of SourceSafe", file)
               End If
               Return ret
            End If
         End If
      Else
         Return True
      End If
   End Function

   Protected Overridable Sub SSCheckInFile( _
                     ByVal node As Xml.XmlNode, _
                     ByVal workingPath As String, _
                     ByVal file As String, _
                     ByVal addFile As Boolean)
      Dim status As Utility.SourceBase.ItemStatus
      Dim checkoutProject As String = GetSSCheckOutProject(node)
      If (Not checkoutProject Is Nothing) AndAlso _
                  checkoutProject.Trim.Length > 0 Then
         checkoutProject = Utility.Tools.FixPath(checkoutProject, _
                  mLocalSettings.GetBasePath(), mDocPath, mNodeFilePaths)
         If mSourceControl Is Nothing Then
            ' TODO: Figure out whether this is an error
         Else
            status = mSourceControl.CheckIn(file, workingPath, checkoutProject)
            If status = Utility.SourceBase.ItemStatus.DoesntExist Then
               If addFile Then
                  SSAddFile(node, workingPath, checkoutProject, file)
               End If
            End If
         End If
      End If
   End Sub

   Protected Overridable Sub SSAddFile( _
                     ByVal node As Xml.XmlNode, _
                     ByVal workingPath As String, _
                     ByVal checkoutProject As String, _
                     ByVal file As String)
      If checkoutProject.Trim.Length > 0 Then
         If mSourceControl Is Nothing Then
            ' TODO: Figure out whether this is an error
         Else
            mSourceControl.AddFile(file, workingPath, checkoutProject)
         End If
      End If

   End Sub

   'Protected Overridable Sub SSCheckOutFiles( _
   '                  ByVal node As Xml.XmlNode, _
   '                  ByVal workingPath As String)
   '   SSFileAction(node, workingPath, "CheckOut")
   'End Sub

   'Protected Overridable Sub SSCheckInFiles( _
   '                  ByVal node As Xml.XmlNode, _
   '                  ByVal workingPath As String)
   '   SSFileAction(node, workingPath, "CheckIn")
   '   SSAddFiles(node, workingPath)
   'End Sub

   'Protected Overridable Sub SSFileAction( _
   '                  ByVal node As Xml.XmlNode, _
   '                  ByVal workingPath As String, _
   '                  ByVal action As String)
   '   Dim exeString As String
   '   Dim ssEXE As String
   '   Dim checkoutProject As String = GetSSCheckOutProject(node)
   '   If (Not checkoutProject Is Nothing) AndAlso _
   '               checkoutProject.Trim.Length > 0 Then
   '      checkoutProject = Utility.Tools.FixPath(checkoutProject, _
   '               mLocalSettings.GetBasePath(), mDocPath, mnodefilepaths)
   '      ssEXE = GetSSSourceSafe(checkoutProject)
   '      If (Not ssEXE Is Nothing) AndAlso _
   '               ssEXE.Trim.Length > 0 Then
   '         exeString &= ssEXE
   '         exeString &= " " & action
   '         exeString &= " -O- -GL""" & workingPath & """ """ & _
   '                  checkoutProject & """"
   '         Microsoft.VisualBasic.Shell(exeString, , True, 2000)
   '      End If
   '   End If
   'End Sub

   'Protected Overridable Sub SSAddFiles( _
   '                  ByVal node As Xml.XmlNode, _
   '                  ByVal workingPath As String)
   '   Dim exeString As String
   '   Dim checkoutProject As String = GetSSCheckOutProject(node)
   '   If (Not checkoutProject Is Nothing) AndAlso _
   '               checkoutProject.Trim.Length > 0 Then
   '      checkoutProject = Utility.Tools.FixPath( _
   '               checkoutProject, mLocalSettings.GetBasePath(), mDocPath, mnodefilepaths)
   '      exeString = GetSSSourceSafe(checkoutProject)
   '      exeString &= " CP """ & checkoutProject & """"
   '      Microsoft.VisualBasic.Shell(exeString, , True, 100)
   '      exeString = GetSSSourceSafe(checkoutProject)
   '      exeString &= " Add -C- "
   '      exeString &= """" & workingPath & "/*.*"""
   '      Microsoft.VisualBasic.Shell(exeString, , True, 100)
   '   End If
   'End Sub

   Protected Overridable Function GetSSCheckOutProject(ByVal node As Xml.XmlNode) As String
      Dim nsmgr As New Xml.XmlNamespaceManager(node.OwnerDocument.NameTable)
      nsmgr.AddNamespace("kg", "http://kadgen.com/KADGenDriving.xsd")

      If Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:OutputRules", "CheckOut", nsmgr) = "true" Then
         Return Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:OutputRules", "CheckOutProject", nsmgr)
      End If
   End Function

   Protected Overridable Function GetSSCheckOut(ByVal node As Xml.XmlNode) As Boolean
      Dim nsmgr As New Xml.XmlNamespaceManager(node.OwnerDocument.NameTable)
      nsmgr.AddNamespace("kg", "http://kadgen.com/KADGenDriving.xsd")

      Return Utility.Tools.GetAttributeOrEmpty( _
                           node, "kg:OutputRules", "CheckOut", nsmgr) = "true"
   End Function

   Private Sub LogError( _
                 ByVal message As String, _
                 ByVal source As String)
      mLogEntries.Add(New Utility.LogEntry( _
                           Utility.LogEntry.logLevel.SeriousError, _
                           message, source))
   End Sub
   Private Sub LogCritical( _
                  ByVal message As String, _
                  ByVal source As String)
      mLogEntries.Add(New Utility.LogEntry( _
                           Utility.LogEntry.logLevel.CriticalError, message, _
                           source))
   End Sub
   Private Sub LogInfo( _
                  ByVal message As String, _
                  ByVal source As String)
      mLogEntries.Add(New Utility.LogEntry( _
                           Utility.LogEntry.logLevel.InfoOnly, message, source))
   End Sub
   Private Sub LogWarning( _
                  ByVal message As String, _
                  ByVal source As String)
      mLogEntries.Add(New Utility.LogEntry( _
                           Utility.LogEntry.logLevel.Warning, message, source))
   End Sub

   Private Function CountNodes( _
                  ByVal xmlDoc As Xml.XmlDocument, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As Int32
      Dim nodelist As Xml.XmlNodeList
      nodelist = xmlDoc.SelectNodes( _
                           "//kg:Section[kg:Standard/@Checked='true']" & _
                           "/*[kg:Standard/@Checked='true']", _
                           nsmgr)
      Return nodelist.Count + 1
   End Function

   Private Sub SetProcess( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager)
      Dim paramInfos() As Reflection.ParameterInfo
      mMethodInfo = Utility.Tools.GetMethodInfo(node, _
                  GetType(Generation), _
                  nsmgr, _
                  "kg:Process", _
                  mLocalSettings.GetBasePath(), _
                  mDocPath, _
                  mNodeFilePaths)
      If Not mMethodInfo Is Nothing Then
         paramInfos = mMethodInfo.GetParameters
         ReDim mParams(paramInfos.GetUpperBound(0))
         For i As Int32 = 0 To paramInfos.GetUpperBound(0)
            mParams(i) = New CodeGenerationSupport.Param(paramInfos(i).Name)
         Next
      End If
   End Sub

   Private Sub SetParameters( _
                  ByVal nodeSelect As Xml.XmlNode, _
                  ByVal outputFileName As String)
      If Not mParams Is Nothing Then
         For i As Int32 = 0 To mParams.GetUpperBound(0)
            Select Case mParams(i).Name.ToLower
               Case "gendatetime"
                  mParams(i).Value = CStr(System.DateTime.Now())
               Case "filename"
                  mParams(i).Value = outputFileName
               Case "database"
                  Dim nsmgr As New Xml.XmlNamespaceManager(nodeSelect.OwnerDocument.NameTable)
                  nsmgr.AddNamespace("dbs", nodeSelect.NamespaceURI)
                  Dim nodeTemp As Xml.XmlNode = nodeSelect.SelectSingleNode( _
                           "ancestor::dbs:DataStructure", nsmgr)
                  mParams(i).Value = Utility.Tools.GetAttributeOrEmpty( _
                           nodeTemp, "Name")
               Case "nodeselect"
                  mParams(i).Value = nodeSelect
               Case Else
                  Dim nodeAttr As Xml.XmlNode
                  Dim nodeTemp As Xml.XmlNode
                  Dim nsmgr As New Xml.XmlNamespaceManager(nodeSelect.OwnerDocument.NameTable)
                  nsmgr.AddNamespace("dbs", nodeSelect.NamespaceURI)
                  nodeAttr = nodeSelect.Attributes.GetNamedItem(mParams(i).Name)
                  If nodeAttr Is Nothing Then
                     nodetemp = nodeSelect.SelectSingleNode( _
                           "ancestor::dbs:" & mParams(i).Name, nsmgr)
                     If nodetemp Is Nothing Then
                        Diagnostics.Debug.WriteLine("Could not find xpath:")
                     Else
                        mParams(i).Value = Utility.Tools.GetAttributeOrEmpty( _
                              nodetemp, "Name")
                     End If
                  Else
                     mParams(i).Value = nodeAttr.Value
                  End If
            End Select
         Next
      End If
   End Sub

   ' CHANGE: SetProcessParameters was very complex, so I rewrote it changing from commas to obscure delmiters 10/11/2003
   Private Function SetProcessParameters( _
            ByVal node As Xml.XmlNode, _
            ByVal nsmgr As Xml.XmlNamespaceManager) _
            As Object()
      ' TODO: Rewrite to a recursive algorithm
      Dim ret() As Object
      Dim paramString As String = Utility.Tools.GetAttributeOrEmpty( _
                                 node, "kg:Parameter", "Parameter", nsmgr)
      ReDim ret(mParams.GetUpperBound(0))
      Dim params() As String = paramString.Split("|"c)
      Dim arrayvals() As String
      Dim iPos As Int32

      ' If the directive node is passed, it must be the first parameter
      If mParams(0).Name.ToLower = "nodedirective" Then
         ret(0) = node
         iPos += 1
      End If

      For i As Int32 = 0 To params.GetUpperBound(0)
         params(i) = params(i).Trim
         ret(iPos) = Utility.Tools.FixPath(params(i).Trim, _
                     mLocalSettings.GetBasePath(), _
                     mDocPath, mNodeFilePaths)
         iPos += 1
      Next
      Return ret
   End Function

   'Private Function SetProcessParameters( _
   '            ByVal node As Xml.XmlNode, _
   '            ByVal nsmgr As Xml.XmlNamespaceManager) _
   '            As Object()
   '   ' TODO: Rewrite to a recursive algorithm
   '   Dim ret() As Object
   '   Dim params As String = Utility.Tools.GetAttributeOrEmpty( _
   '                              node, "kg:Parameter", "Parameter", nsmgr)
   '   ReDim ret(mParams.GetUpperBound(0))
   '   Dim nextParam As String
   '   Dim iPos As Int32
   '   Dim basePath As String = mLocalSettings.GetBasePath()
   '   ' If the directive node is passed, it must be the first parameter
   '   If mParams(iPos).Name.ToLower = "nodedirective" Then
   '      ret(iPos) = node
   '      iPos += 1
   '   End If
   '   For i As Int32 = 0 To params.Length - 1
   '      Select Case params.Substring(i, 1)
   '         Case "{", ","
   '            If params.Substring(i, 1) = "{" Then
   '               Dim temp As String
   '               temp = params.Substring(i)
   '               nextParam = temp.Substring(0, temp.IndexOf("}") + 1)
   '               i += nextParam.Length
   '            End If
   '            ret(iPos) = Utility.Tools.FixPath(nextParam.Trim, basePath, _
   '                        mDocPath, mnodefilepaths)
   '            nextParam = ""
   '            iPos += 1
   '            If iPos > ret.GetUpperBound(0) Or i > params.Length - 1 Then
   '               Exit For
   '            End If
   '         Case ","
   '            ret(iPos) = Utility.Tools.FixPath(nextParam.Trim, basePath, _
   '                        mDocPath, mnodefilepaths)
   '            nextParam = ""
   '            iPos += 1
   '            If iPos > ret.GetUpperBound(0) Then
   '               Exit For
   '            End If
   '         Case "{"
   '            Dim temp As String
   '            temp = params.Substring(i)
   '            nextParam = temp.Substring(0, temp.IndexOf("}") + 1)
   '            ret(iPos) = Utility.Tools.FixPath(nextParam.Trim, _
   '                        basePath, mDocPath, mnodefilepaths)
   '            nextParam = ""
   '            iPos += 1
   '            i += temp.Length
   '            If iPos > ret.GetUpperBound(0) Or i > params.Length - 1 Then
   '               Exit For
   '            End If
   '         Case vbcr, vblf
   '            ' skip because this is probably a wrap for insignificant whitespace.
   '         Case Else
   '            nextParam &= params.Substring(i, 1)
   '      End Select
   '   Next
   '   Return ret
   'End Function

   Private Function BuildParamArray() As Object()
      Dim ret() As Object = {}
      If Not mParams Is Nothing Then
         ReDim ret(mParams.GetUpperBound(0))
         For i As Int32 = 0 To mParams.GetUpperBound(0)
            ret(i) = mParams(i).Value
         Next
      End If
      Return ret
   End Function

   Private Function GetExtObjects( _
                  ByVal node As Xml.XmlNode, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As CodeGenerationSupport.ExtObject()
      Dim type As System.Type = Utility.Tools.GetSpecifiedType(node, _
                     Me.GetType, nsmgr, "kg:XSLTFiles", _
                     mLocalSettings.GetBasePath(), mDocPath, mNodeFilePaths)
      If Not type Is Nothing Then
         Dim extObjects() As ExtObject
         Dim nSpace As String = Utility.Tools.GetAttributeOrEmpty(node, _
                     "kg:XSLTFiles", "NamespaceURI", nsmgr)
         If nSpace.TrimEnd.Length > 0 Then
            ReDim extObjects(0)
            extObjects(0).NameSpaceURI = nSpace
            extObjects(0).Value = type
            Return extObjects
         End If
      End If
   End Function

   Private Function ParseFilenamePattern( _
                     ByVal fileName As String, _
                     ByVal node As Xml.XmlNode, _
                     ByVal nsmgr As Xml.XmlNamespaceManager) _
                     As String
      Dim xPath As String
      Dim iStart As Int32
      Dim iEnd As Int32
      Dim sEnd As String
      Dim fragNode As Xml.XmlNode
      Do While fileName.IndexOf("<") >= 0
         iStart = fileName.IndexOf("<")
         iEnd = fileName.IndexOf(">")
         xPath = fileName.Substring(iStart + 1, iEnd - iStart - 1)
         If xPath.IndexOf("<") >= 0 Then
            xPath = ParseFilenamePattern(xPath, node, nsmgr)
         End If
         ' *Changed to pass nsmgr 7/27/03 KD
         'Dim nsmgr As Xml.XmlNamespaceManager = GetNameSpaceManager(node.OwnerDocument, "dbs")
         fragNode = node.SelectSingleNode(xPath, nsmgr)
         sEnd = fileName.Substring(iEnd + 1)
         fileName = fileName.Substring(0, iStart)
         If Not fragNode Is Nothing Then
            fileName &= fragNode.Value
         End If
         fileName &= sEnd
      Loop
      Return fileName
   End Function

   Public Sub RunScript( _
                  ByVal node As Xml.XmlNode, _
                  ByVal filename As String)
      Dim trans As Data.sqlClient.SqlTransaction
      Dim cmd As New Data.SqlClient.SqlCommand
      Dim reader As IO.StreamReader
      Dim fileChanged As Utility.FileChanged
      Dim outType As Utility.OutputType
      Dim genType As Utility.GenType
      Dim sProcOldCode As String
      Dim sProcNewCode As String
      Dim sProcName As String
      Dim sServerName As String
      Dim iProcStatementPos As Int32
      outType = Me.GetOutType(node)
      genType = Me.GetGenType(node)
      If IO.File.Exists(filename) Then
         Try
            reader = New IO.StreamReader(filename)
            Dim statements() As String = Text.RegularExpressions.Regex.Split( _
                     reader.ReadToEnd, "\sgo\s", _
                     Text.RegularExpressions.RegexOptions.Compiled Or _
                     Text.RegularExpressions.RegexOptions.CultureInvariant Or _
                     Text.RegularExpressions.RegexOptions.IgnoreCase)
            iProcStatementPos = GetProcStatementPosition(statements)
            sProcName = RetrieveStoredProcName(statements, iProcStatementPos)
            sProcOldCode = RetrieveStoredProcContents(sProcName, node)
            If sProcOldCode Is Nothing Then
               fileChanged = Utility.FileChanged.FileDoesntExist
            Else
               fileChanged = Utility.HashTools.IsTextChanged(sProcOldCode, _
                        mProjectSettings.GetCommentStart(outType), _
                        mProjectSettings.GetCommentEnd(outType), _
                        mProjectSettings.GetHeaderMarker(), _
                        mProjectSettings.GetHashMarker())
            End If
            If ShouldRun(genType, fileChanged, _
                     IO.Path.GetFileName(filename), True) Then
               Try
                  mConnection.Open()
                  trans = mConnection.BeginTransaction
                  cmd.Connection = mConnection
                  cmd.Transaction = trans
                  cmd.CommandType = Data.CommandType.Text

                  For i As Int32 = 0 To statements.Length - 1
                     If i = iProcStatementPos Then
                        sProcNewCode = Utility.HashTools.ApplyHash(statements(i), _
                                 mProjectSettings.GetCommentText(genType), _
                                 mProjectSettings.GetCommentStart(outType), _
                                 mProjectSettings.GetCommentEnd(outType), _
                                 True, mProjectSettings.GetHeaderMarker(), _
                                 mProjectSettings.GetHashMarker())
                        cmd.CommandText = sProcNewCode
                     Else
                        cmd.CommandText = statements(i)
                     End If
                     cmd.ExecuteNonQuery()
                  Next
                  trans.Commit()

               Catch ex As Exception
                  Console.WriteLine(ex)
                  Throw
               Finally
                  cmd.Connection.Close()
               End Try
            End If
         Catch ex As System.Exception
            Throw
         Finally
            reader.Close()
         End Try
      End If

   End Sub

   Private Function GetProcStatementPosition( _
                     ByVal statements() As String) _
                     As Int32
      Dim sprocName As String
      Dim iPos As String
      ' TODO: Replace this with Regex that avoids finding CreateProcedure in a comment
      '       and in other ways might need to be more sophisticated
      For i As Int32 = 0 To statements.GetUpperBound(0)
         Dim s() As String = _
               Text.RegularExpressions.Regex.Split(statements(i), "\W+create\W+procedure\W+", _
               Text.RegularExpressions.RegexOptions.Compiled Or _
               Text.RegularExpressions.RegexOptions.CultureInvariant Or _
               Text.RegularExpressions.RegexOptions.IgnoreCase)
         If s.GetLength(0) > 1 Then
            Return i
         End If
      Next
   End Function

   Private Function RetrieveStoredProcName( _
                     ByVal statements() As String, _
                     ByVal iProcPos As Int32) _
                     As String
      Dim sprocName As String
      Dim iPos As String
      ' TODO: Replace this with Regex that avoids finding CreateProcedure in a comment
      '       and in other ways might need to be more sophisticated

      Dim s() As String = _
            Text.RegularExpressions.Regex.Split(statements(iProcPos), "\W+create\W+procedure\W+", _
            Text.RegularExpressions.RegexOptions.Compiled Or _
            Text.RegularExpressions.RegexOptions.CultureInvariant Or _
            Text.RegularExpressions.RegexOptions.IgnoreCase)
      If s.GetLength(0) > 1 Then
         sprocName = s(1).Trim
         ' Separate into individual words
         Dim sWords() As String = _
               Text.RegularExpressions.Regex.Split(sprocName, "\W+\w+\W+", _
               Text.RegularExpressions.RegexOptions.Compiled Or _
               Text.RegularExpressions.RegexOptions.CultureInvariant Or _
               Text.RegularExpressions.RegexOptions.IgnoreCase)
         If sWords.GetLength(0) > 0 Then
            Return sWords(0)
         End If
      End If
   End Function

   Private Function RetrieveStoredProcContents( _
                     ByVal procName As String, _
                     ByVal node As Xml.XmlNode) _
                     As String
      Dim databaseName As String = Utility.Tools.GetAttributeOrEmpty(node, "Database")
      Dim sqlText As String = "SELECT * FROM INFORMATION_SCHEMA.ROUTINES " & _
              " WHERE ROUTINE_CATALOG='" & databaseName & _
              "' AND ROUTINE_TYPE='PROCEDURE'" & _
              " AND ROUTINE_NAME='" & procName & "'"
      Dim da As New Data.SqlClient.SqlDataAdapter(sqlText, mConnection)
      Dim dt As New Data.DataTable
      da.Fill(dt)
      If dt.Rows.Count > 0 Then
         Return dt.Rows(0)("ROUTINE_DEFINITION").ToString
      End If
   End Function

   Private Sub CurrentOutputStart( _
               ByVal selectFile As String, _
               ByVal inputFile As String, _
               ByVal template As String, _
               ByVal countSelected As Int32, _
               ByVal outputRule As String)
      mCurrentOutput.StartTime = DateTime.Now
      mCurrentOutput.SelectFile = selectFile
      mCurrentOutput.InputFile = inputFile
      mCurrentOutput.Template = IO.Path.GetFileName(template)
      mCurrentOutput.CountSelected = countSelected
   End Sub
   Private Sub CurrentOutputStart( _
               ByVal template As String)
      mCurrentOutput.StartTime = DateTime.Now
      mCurrentOutput.Template = IO.Path.GetFileName(template)
   End Sub

   Private Sub CurrentOutputEnd( _
               ByVal status As String, _
               ByVal countCreated As Int32, _
               ByVal countOutput As Int32)
      mCurrentOutput.EndTime = DateTime.Now
      mCurrentOutput.Status = status
      mCurrentOutput.CountCreated = countCreated
      mCurrentOutput.CountOutput = countOutput
   End Sub
   Private Sub CurrentOutputEnd( _
            ByVal status As String)
      mCurrentOutput.EndTime = DateTime.Now
      mCurrentOutput.Status = status
   End Sub

   Private Function GetProcessName(ByVal node As Xml.XmlNode, ByVal nsmgr As Xml.XmlNamespaceManager) As String
      Return (Utility.Tools.GetAttributeOrEmpty(node, "kg:Process", "TypeName", nsmgr) & ":" & _
               Utility.Tools.GetAttributeOrEmpty(node, "kg:Process", "MethodName", nsmgr))
   End Function

   Private Sub CopyContainedFiles(ByVal sourceDir As IO.DirectoryInfo, ByVal targetDir As IO.DirectoryInfo)
      Dim dirTarget As IO.DirectoryInfo
      Dim dirSource As IO.DirectoryInfo
      For Each f As IO.FileInfo In sourceDir.GetFiles
         f.CopyTo(IO.Path.Combine(targetDir.FullName, f.Name))
      Next
      For Each d As IO.DirectoryInfo In sourceDir.GetDirectories
         dirTarget = IO.Directory.CreateDirectory(IO.Path.Combine(targetDir.FullName, d.Name))
         dirSource = New IO.DirectoryInfo(d.FullName)
         CopyContainedFiles(dirSource, dirTarget)
      Next

   End Sub

#End Region
End Class
