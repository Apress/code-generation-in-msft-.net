' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Support for CodeDOM templates

Option Strict On
Option Explicit On

Imports System
Imports KADGen
Imports System.Diagnostics

'! Class Summary: 

Public Class CodeDOMSupport

#Region "Class level declarations - empty"
#End Region

#Region "Constructors - empty"
#End Region

#Region "Public Methods and Properties"
   Public Shared Function GenerateViaCodeDOM( _
               ByVal outputType As Utility.OutputType, _
               ByVal compileunit As CodeDom.CodeCompileUnit) _
               As IO.Stream
      Dim provider As CodeDom.Compiler.CodeDomProvider
      Dim gen As CodeDom.Compiler.ICodeGenerator
      Dim tw As CodeDom.Compiler.IndentedTextWriter
      Dim stream As IO.MemoryStream
      Dim writer As IO.StreamWriter

      Select Case outputType
         Case Utility.OutputType.VB
            provider = New Microsoft.VisualBasic.VBCodeProvider
         Case Utility.OutputType.CSharp
            provider = New Microsoft.CSharp.CSharpCodeProvider
      End Select
      gen = provider.CreateGenerator()

      Try
         stream = New IO.MemoryStream
         writer = New IO.StreamWriter(stream)
         tw = New CodeDom.Compiler.IndentedTextWriter(writer, "   ")
         gen.GenerateCodeFromCompileUnit(compileunit, tw, New CodeDom.Compiler.CodeGeneratorOptions)
         tw.Flush()
         stream.Seek(0, IO.SeekOrigin.Begin)
      Catch ex As System.Exception
         Debug.WriteLine(ex)
         tw.Flush()
         tw.Close()
         Throw
      End Try
      Return stream
   End Function

   Public Shared Sub GenerateViaCodeDOM( _
               ByVal outputFileName As String, _
               ByVal provider As CodeDom.Compiler.CodeDomProvider, _
               ByVal compileunit As CodeDom.CodeCompileUnit)
      Dim gen As CodeDom.Compiler.ICodeGenerator = provider.CreateGenerator()
      Dim tw As CodeDom.Compiler.IndentedTextWriter

      Try
         tw = New CodeDom.Compiler.IndentedTextWriter(New IO.StreamWriter(outputFileName, False), "    ")
         gen.GenerateCodeFromCompileUnit(compileunit, tw, New CodeDom.Compiler.CodeGeneratorOptions)
         tw.Flush()
         tw.Close()
      Catch ex As System.Exception
         Debug.WriteLine(ex)
         Throw
      End Try

   End Sub
#End Region

#Region "Protected and Friend Methods and Properties - empty"
#End Region

#Region "Private Methods and Properties - empty"
#End Region

End Class
