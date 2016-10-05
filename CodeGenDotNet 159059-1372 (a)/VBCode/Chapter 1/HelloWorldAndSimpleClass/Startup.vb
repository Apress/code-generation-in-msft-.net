Option Strict On
Option Explicit On

Imports System

Public Structure XSLTParam
   Public Name As String
   Public Value As String
   Public Sub New(ByVal Name As String, ByVal Value As String)
      Me.Name = Name
      Me.Value = Value
   End Sub
End Structure

'! Class Summary: 

Public Class Startup
   Private Const outputDir As String = "..\Test"
   Private Const xsltDir As String = ".."
   Private Const xmlDir As String = ".."
   Private Const tablename As String = "Customers"
   ' Output filenames are hard coded in this simple sample

#Region "Class level declarations"
#End Region

#Region "Constructors"
#End Region

   Public Shared Sub Main()
      ' Be sure output directory exists
      If Not IO.Directory.Exists(outputDir) Then
         IO.Directory.CreateDirectory(outputDir)
      End If

      Test()
      GenerateHelloWorld(outputDir)
      GenerateClassViaBruteForce(outputDir)
      GenerateClassViaCodeDOM(outputDir)
      GenerateClassViaXSLT(outputDir)

   End Sub

   Private Shared Sub GenerateHelloWorld(ByVal outputDir As String)

      ' Generate Hello World using Brute Force
      HelloWorldViaBruteForce.GenerateOutput(IO.Path.Combine(outputDir, "HelloWorldViaBruteForce.vb"))

      ' Generate Hello World in Visual Basic using the CodeDOM
      Dim compileUnit As CodeDom.CodeCompileUnit
      Dim provider As CodeDom.Compiler.CodeDomProvider
      compileUnit = HelloWorldViaCodeDOM.BuildGraph()
      provider = New Microsoft.VisualBasic.VBCodeProvider
      GenerateViaCodeDOM(IO.Path.Combine(outputDir, "HelloWorldViaCodeDOM.vb"), provider, compileUnit)
      ' Use same compile unit to generate C# Hello World
      provider = New Microsoft.CSharp.CSharpCodeProvider
      GenerateViaCodeDOM(IO.Path.Combine(outputDir, "HelloWorldViaCodeDOM.cs"), provider, compileUnit)

      ' Generate Hello World via XSLT Template
      GenerateViaXSLT(IO.Path.Combine(xsltDir, "HelloWorld.xslt"), Nothing, IO.Path.Combine(outputDir, "HelloWorldViaXSLT.vb"))

   End Sub

   Private Shared Sub GenerateClassViaBruteForce(ByVal outputDir As String)
      ' Open Metadata file
      Dim xmlMetaData As New Xml.XmlDocument
      xmlMetaData.Load(IO.Path.Combine(xmlDir, "Metadata.xml"))

      ClassViaBruteForce.GenerateOutput(IO.Path.Combine(outputDir, "ClassCustomersViaBruteForce.vb"), xmlMetaData, "Customers")
      ClassViaBruteForce.GenerateOutput(IO.Path.Combine(outputDir, "ClassOrdersViaBruteForce.vb"), xmlMetaData, "Orders")

   End Sub


   Private Shared Sub GenerateClassViaCodeDOM(ByVal outputDir As String)
      Dim compileUnit As CodeDom.CodeCompileUnit
      Dim provider As CodeDom.Compiler.CodeDomProvider

      ' Open Metadata file
      Dim xmlMetaData As New Xml.XmlDocument
      xmlMetaData.Load(IO.Path.Combine(xmlDir, "Metadata.xml"))

      ' Generate simple class for Cusotmers in Visual Basic using the CodeDOM
      compileUnit = ClassViaCodeDOM.BUildGraph(xmlMetaData, "Customers")
      provider = New Microsoft.VisualBasic.VBCodeProvider
      GenerateViaCodeDOM(IO.Path.Combine(outputDir, "ClassCustomersViaCodeDOM.vb"), provider, compileUnit)

      ' Generate simple class for Orders in Visual Basic using the CodeDOM
      compileUnit = ClassViaCodeDOM.BUildGraph(xmlMetaData, "Orders")
      provider = New Microsoft.VisualBasic.VBCodeProvider
      GenerateViaCodeDOM(IO.Path.Combine(outputDir, "ClassOrdersViaCodeDOM.vb"), provider, compileUnit)
   End Sub

   Private Shared Sub GenerateClassViaXSLT(ByVal outputDir As String)
      ' Open Metadata file
      Dim xmlMetaData As New Xml.XmlDocument
      xmlMetaData.Load(IO.Path.Combine(xmlDir, "Metadata.xml"))

      ' Generate Hello World via XSLT Template
      GenerateViaXSLT(IO.Path.Combine(xsltDir, "Class.xslt"), xmlMetaData, IO.Path.Combine(outputDir, "ClassCustomersViaXSLT.vb"), New XSLTParam("TableName", "Customers"))
      GenerateViaXSLT(IO.Path.Combine(xsltDir, "Class.xslt"), xmlMetaData, IO.Path.Combine(outputDir, "ClassOrdersViaXSLT.vb"), New XSLTParam("TableName", "Orders"))
   End Sub

   Private Shared Sub GenerateViaCodeDOM( _
               ByVal outputFileName As String, _
               ByVal provider As CodeDom.Compiler.CodeDomProvider, _
               ByVal compileunit As CodeDom.CodeCompileUnit)
      Dim gen As CodeDom.Compiler.ICodeGenerator = provider.CreateGenerator()
      Dim tw As CodeDom.Compiler.IndentedTextWriter

      tw = New CodeDom.Compiler.IndentedTextWriter(New IO.StreamWriter(outputFileName, False), "    ")
         gen.GenerateCodeFromCompileUnit(compileunit, tw, New CodeDom.Compiler.CodeGeneratorOptions)
         tw.Flush()
         tw.Close()

   End Sub


   Private Shared Sub GenerateViaXSLT( _
               ByVal xsltFileName As String, _
               ByVal xmlMetaData As Xml.XmlDocument, _
               ByVal outputFile As String, _
               ByVal ParamArray params() As XSLTParam)
      Dim xslt As New Xml.Xsl.XslTransform
      Dim xNav As Xml.XPath.XPathNavigator
      Dim streamWriter As IO.StreamWriter
      Dim args As New Xml.Xsl.XsltArgumentList
      Dim param As XSLTParam

      Try
         If xmlMetaData Is Nothing Then
            xmlMetaData = New Xml.XmlDocument
         End If

         For Each param In params
            args.AddParam(param.Name, "", param.Value)
         Next

         xNav = xmlMetaData.CreateNavigator()
         streamWriter = New IO.StreamWriter(outputFile)

         xslt.Load(xsltFileName)
         xslt.Transform(xNav, args, streamWriter, Nothing)

      Finally
         If Not streamWriter Is Nothing Then
            streamWriter.Flush()
            streamWriter.Close()
         End If
      End Try

   End Sub

   Private Shared Sub test()
      Debug.WriteLine("*" & CodeDom.Compiler.IndentedTextWriter.DefaultTabString & "*")
      Dim tw As New IO.StreamWriter("..\IndentTest")
      Dim itw As New CodeDom.Compiler.IndentedTextWriter(tw)
      itw.WriteLine("Fred")
      itw.Indent = 2
      itw.WriteLine("Fred")
      itw.Indent = 4
      itw.WriteLine("Fred")
      itw.Indent = itw.Indent + 1
      itw.WriteLine("Fred")
      itw.Indent = itw.Indent - 1
      itw.WriteLine("Fred")
      itw.Indent = itw.Indent - 1
      itw.WriteLine("Fred")
      itw.Indent = itw.Indent - 1
      itw.Flush()
      itw.Close()
   End Sub

End Class
