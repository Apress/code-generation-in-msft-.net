' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Provides support for running XSLT templates
'  Refactor: There are too many translation methods

Option Strict On
Option Explicit On

Imports System
Imports System.Diagnostics

'! Class Summary: 

Public Structure Param
   Public Name As String
   Public Value As Object
   Public Sub New(ByVal Name As String, ByVal Value As Object)
      Me.Name = Name
      Me.Value = Value
   End Sub
   Public Sub New(ByVal Name As String)
      Me.Name = Name
   End Sub
End Structure

Public Structure ExtObject
   Public NameSpaceURI As String
   Public Value As Object
   Public Sub New(ByVal NameSpaceURI As String, ByVal Value As Object)
      Me.NameSpaceURI = NameSpaceURI
      Me.Value = Value
   End Sub
   Public Sub New(ByVal NameSpaceURI As String)
      Me.NameSpaceURI = NameSpaceURI
   End Sub
End Structure

Public Class XSLTSupport

#Region "Class level declarations - empty"
#End Region

#Region "Constructors - empty"
#End Region

#Region "Public Methods and Properties"
   Public Shared Sub GenerateViaXSLT( _
               ByVal xsltFileName As String, _
               ByVal MetaDataFileName As String, _
               ByVal outputFile As String, _
               ByVal outputType As String, _
               ByVal extObjects() As ExtObject, _
               ByVal ParamArray params() As Param)
      Dim xmlDoc As New Xml.XmlDocument
      xmlDoc.Load(MetaDataFileName)
      KADGen.LibraryInterop.Singletons.XMLDoc = xmlDoc
      KADGen.LibraryInterop.Singletons.NsMgr = KADGen.Utility.xmlHelpers.BuildNamespacesManagerForDoc(xmlDoc)
      GenerateViaXSLT(xsltFileName, xmlDoc, outputFile, outputType, extObjects, params)
   End Sub


   Public Shared Sub GenerateViaXSLT( _
               ByVal xsltFileName As String, _
               ByVal xmlMetaData As Xml.XmlDocument, _
               ByVal outputFile As String, _
               ByVal outputType As String, _
                ByVal extObjects() As ExtObject, _
               ByVal ParamArray params() As Param)
      Dim stream As IO.FileStream
      Try
         stream = New IO.FileStream(outputFile, IO.FileMode.Create)
         GenerateXSLTToStream(xsltFileName, xmlMetaData, stream, outputType, extObjects, params)
      Finally
         stream.Close()
      End Try

   End Sub

   Public Shared Function GenerateViaXSLT( _
               ByVal xsltFileName As String, _
               ByVal xmlMetaData As Xml.XmlDocument, _
               ByVal outputType As String, _
               ByVal extObjects() As ExtObject, _
               ByVal ParamArray params() As Param) _
               As IO.Stream
      Dim stream As IO.MemoryStream
      stream = New IO.MemoryStream
      GenerateXSLTToStream(xsltFileName, xmlMetaData, stream, outputType, extObjects, params)
      stream.Flush()
      stream.Seek(0, IO.SeekOrigin.Begin)
      Return stream
   End Function

   Public Shared Function GenerateViaXSLT( _
               ByVal xslTransform As Xml.Xsl.XslTransform, _
               ByVal xNavMetaData As Xml.XPath.XPathNavigator, _
               ByVal outputType As String, _
               ByVal extObjects() As ExtObject, _
               ByVal ParamArray params() As Param) _
               As IO.Stream
      Dim stream As IO.MemoryStream
      stream = New IO.MemoryStream
      GenerateXSLTToStream(xslTransform, xNavMetaData, stream, outputType, extObjects, params)
      stream.Flush()
      stream.Seek(0, IO.SeekOrigin.Begin)
      Return stream
   End Function

   Public Shared Function GenerateViaXSLT( _
               ByVal xsltFileName As String, _
               ByVal MetaDataFileName As String, _
               ByVal outputType As String, _
               ByVal extObjects() As ExtObject, _
               ByVal ParamArray params() As Param) _
               As IO.Stream
      Dim xmlDoc As New Xml.XmlDocument
      xmlDoc.Load(MetaDataFileName)
      KADGen.LibraryInterop.Singletons.XMLDoc = xmlDoc
      KADGen.LibraryInterop.Singletons.NsMgr = KADGen.Utility.xmlHelpers.BuildNamespacesManagerForDoc(xmlDoc)
      Return GenerateViaXSLT(xsltFileName, xmlDoc, outputType, extObjects, params)
   End Function

   Public Shared Sub GenerateXMLViaXSLT( _
               ByVal xsltFileName As String, _
               ByVal xmlMetaData As Xml.XmlDocument, _
               ByVal outputFile As String, _
               ByVal outputType As String, _
               ByVal extObjects() As ExtObject, _
               ByVal ParamArray params() As Param)
      Dim xslt As New Xml.Xsl.XslTransform
      Dim xNav As Xml.XPath.XPathNavigator
      Dim XMLWriter As Xml.XmlTextWriter
      Dim args As New Xml.Xsl.XsltArgumentList
      Dim param As param

      Try
         If xmlMetaData Is Nothing Then
            xmlMetaData = New Xml.XmlDocument
         End If

         For Each param In params
            args.AddParam(param.Name, "", param.Value)
         Next

         If extObjects Is Nothing Then
            ' No problem, just skip
         Else
            For Each extObject As extObject In extObjects
               Dim constructorInfo As Reflection.ConstructorInfo = CType(extObject.Value, System.Type).GetConstructor(Nothing)
               Dim obj As Object = constructorInfo.Invoke(Nothing)
               args.AddExtensionObject(extObject.NameSpaceURI, obj)
            Next
         End If
         AddStandardExtension(args)

         xNav = xmlMetaData.CreateNavigator()
         XMLWriter = New Xml.XmlTextWriter(outputFile, Text.Encoding.UTF8)

         xslt.Load(xsltFileName)
         xslt.Transform(xNav, args, XMLWriter, Nothing)

      Catch ex As System.Exception
         Debug.WriteLine(ex)
         Throw

      Finally
         If Not XMLWriter Is Nothing Then
            XMLWriter.Flush()
            XMLWriter.Close()
         End If
      End Try

   End Sub

   Public Shared Function GetXSLTParams( _
            ByVal xsltFileName As String, _
            ByVal basePath As String) _
            As Param()
      Dim xsltStream As IO.Stream
      Dim xmlInput As New Xml.XmlDocument
      Dim stream As New IO.MemoryStream
      Dim paramNames() As String
      Dim params() As Param
      Dim attr As Xml.XmlNode
      Dim value As String

      Try
         xsltStream = Utility.Tools.GetStreamFromStringResource( _
                     GetType(XSLTSupport), "RetrieveParams.xslt")
         xmlInput.Load(xsltFileName)
         GenerateXSLTToStream(xsltStream, xmlInput, stream, "", Nothing)
         stream.Seek(0, IO.SeekOrigin.Begin)
         Dim reader As New IO.StreamReader(stream)
         paramNames = reader.ReadToEnd.Split("|"c)
         If paramNames.GetLength(0) = 1 AndAlso paramNames(0).Length = 0 Then
            ' there are no parameters
         Else
            ReDim params(paramNames.GetUpperBound(0))
            For i As Int32 = 0 To paramNames.GetUpperBound(0)
               If paramNames(i).Trim.Length > 0 Then
                  params(i) = New Param(paramNames(i), "")
               End If
            Next
         End If
      Finally
         Try
            stream.Close()
         Catch
         End Try
         Try
            xsltStream.Close()
         Catch
         End Try
      End Try
      Return params
   End Function



   Public Shared Sub FillParams( _
                  ByVal params() As Param, _
                  ByVal nav As Xml.XPath.XPathNavigator)
      For Each param As param In params
         param.Value = nav.GetAttribute(param.Name, "")
      Next
   End Sub

#End Region

#Region "Protected and Friend Methods and Properties - empty"
#End Region

#Region "Private Methods and Properties"
   Private Shared Sub GenerateXSLTToStream( _
               ByVal xsltFileName As String, _
               ByVal xmlMetaData As Xml.XmlDocument, _
               ByVal stream As IO.Stream, _
               ByVal outputType As String, _
               ByVal extObjects() As ExtObject, _
               ByVal ParamArray params() As Param)
      Dim xslt As New Xml.Xsl.XslTransform
      Dim xNav As Xml.XPath.XPathNavigator
      Dim streamWriter As New IO.StreamWriter(stream)
      Dim args As New Xml.Xsl.XsltArgumentList
      Dim param As param

      Try
         If xmlMetaData Is Nothing Then
            xmlMetaData = New Xml.XmlDocument
         End If

         xNav = xmlMetaData.CreateNavigator()

         xslt.Load(xsltFileName)
         GenerateXSLTToStream(xslt, xNav, stream, outputType, extObjects, params)

      Catch ex As System.Exception
         Debug.WriteLine(ex)
         Throw

      Finally
         If Not streamWriter Is Nothing Then
            streamWriter.Flush()
         End If
      End Try
   End Sub

   Private Shared Sub GenerateXSLTToStream( _
            ByVal xsltStream As IO.Stream, _
            ByVal xmlMetaData As Xml.XmlDocument, _
            ByVal stream As IO.Stream, _
               ByVal outputType As String, _
             ByVal extObjects() As ExtObject, _
            ByVal ParamArray params() As Param)
      Dim xslt As New Xml.Xsl.XslTransform
      Dim xNav As Xml.XPath.XPathNavigator
      Dim streamWriter As New IO.StreamWriter(stream)
      Dim args As New Xml.Xsl.XsltArgumentList
      Dim param As param
      Dim xmlReader As Xml.XmlTextReader

      Try
         If xmlMetaData Is Nothing Then
            xmlMetaData = New Xml.XmlDocument
         End If

         xNav = xmlMetaData.CreateNavigator()

         xmlReader = New Xml.XmlTextReader(xsltStream)
         xslt.Load(xmlReader, Nothing, New Security.Policy.Evidence)
         GenerateXSLTToStream(xslt, xNav, stream, outputType, extObjects, params)

      Catch ex As System.Exception
         Debug.WriteLine(ex)
         Throw

      Finally
         If Not streamWriter Is Nothing Then
            streamWriter.Flush()
         End If
      End Try
   End Sub

   'Private Shared Sub GenerateXSLTToStream( _
   '            ByVal xsltFileName As String, _
   '            ByVal xmlMetaData As Xml.XmlDocument, _
   '            ByVal stream As IO.Stream, _
   '            ByVal ParamArray params() As XSLTParam)
   '   Dim xslt As New Xml.Xsl.XslTransform
   '   Dim xNav As Xml.XPath.XPathNavigator
   '   Dim streamWriter As New IO.StreamWriter(stream)
   '   Dim args As New Xml.Xsl.XsltArgumentList
   '   Dim param As XSLTParam

   '   Try
   '      If xmlMetaData Is Nothing Then
   '         xmlMetaData = New Xml.XmlDocument
   '      End If

   '      For Each param In params
   '         args.AddParam(param.Name, "", param.Value)
   '      Next

   '      xNav = xmlMetaData.CreateNavigator()

   '      xslt.Load(xsltFileName)
   '      xslt.Transform(xNav, args, streamWriter, Nothing)

   '   Catch ex As System.Exception
   '      Debug.WriteLine(ex)
   '      Throw

   '   Finally
   '      If Not streamWriter Is Nothing Then
   '         streamWriter.Flush()
   '      End If
   '   End Try
   'End Sub

   Private Shared Sub GenerateXSLTToStream( _
               ByVal xsltTransform As Xml.Xsl.XslTransform, _
               ByVal xNavMetaData As Xml.XPath.XPathNavigator, _
               ByVal stream As IO.Stream, _
               ByVal outputType As String, _
               ByVal extObjects() As ExtObject, _
               ByVal ParamArray params() As Param)
      Dim streamWriter As IO.StreamWriter
      Dim xmlWriter As Xml.XmlTextWriter
      Dim args As New Xml.Xsl.XsltArgumentList
      Dim param As param

      Try
         If Not params Is Nothing Then
            For Each param In params
               args.AddParam(param.Name, "", param.Value)
            Next
         End If

         If extObjects Is Nothing Then
            ' No problem, just skip
         Else
            For Each extObject As extObject In extObjects
               Dim constructorInfo As Reflection.ConstructorInfo = CType(extObject.Value, System.Type).GetConstructor(Type.EmptyTypes)
               Dim obj As Object = constructorInfo.Invoke(Nothing)
               args.AddExtensionObject(extObject.NameSpaceURI, obj)
            Next
         End If
         AddStandardExtension(args)

         'If outputType.Trim.ToUpper = "XML" Then
         '   xmlWriter = New Xml.XmlTextWriter(stream, Text.Encoding.UTF8)
         '   xsltTransform.Transform(xNavMetaData, args, xmlWriter, Nothing)
         'Else
            streamWriter = New IO.StreamWriter(stream)
            xsltTransform.Transform(xNavMetaData, args, streamWriter, Nothing)
         'End If

      Catch ex As System.Exception
         Debug.WriteLine(ex)
         Throw

      Finally
         If Not streamWriter Is Nothing Then
            streamWriter.Flush()
         End If
         If Not xmlWriter Is Nothing Then
            xmlWriter.Flush()
         End If
      End Try
   End Sub


   Private Shared Sub AddStandardExtension(ByVal args As Xml.Xsl.XsltArgumentList)
      args.AddExtensionObject("http://kadgen.com/StandardNETSupport.xsd", _
            New XSLTSupport.StandardSupport)

   End Sub

#End Region

   Private Class StandardSupport
      Private mIndent As Int32 = 2
      Private mIndentSize As Int32 = 2

      Public Sub SetIndentSize(ByVal value As Int32)
         mIndentSize = value
      End Sub

      Public Function GetIndent() As Int32
         Return mIndent
      End Function

      Public Sub SetIndent(ByVal Value As Int32)
         mIndent = Value
      End Sub

      Public Sub Indent()
         mIndent += 1
      End Sub

      Public Sub Outdent()
         mIndent -= 1
      End Sub

      Public Function InsertIndent() As String
         Dim sb As New Text.StringBuilder
         Dim repeatCount As Int32 = mIndentSize * mIndent
         If repeatcount > 0 Then
            sb.Append(" "c, mIndentSize * mIndent)
         End If
         Return sb.ToString
      End Function

      Public Function InsertNLIndent() As String
         Return Microsoft.VisualBasic.ControlChars.CrLf & InsertIndent()
      End Function
   End Class

End Class
