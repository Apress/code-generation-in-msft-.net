Option Strict On
Option Explicit On 

Imports System

#Region "Description"
'Simple Utility files supporting code generation
#End Region

Public Class Support
   Public Const DQ As String = """"

   Public Shared Sub FileOpen( _
                  ByVal inWriter As CodeDom.Compiler.IndentedTextWriter, _
                  ByVal import As String, _
                  ByVal filename As String, _
                  ByVal gendatetime As String)
      inWriter.WriteLine("Option Strict On")
      inWriter.WriteLine("Option Explicit On")
      inWriter.WriteLine("")
      inWriter.WriteLine("Imports System")
      For Each s As String In import.Split(","c)
         inWriter.WriteLine("Imports " & s)
      Next
      MakeRegion(inWriter, "Description")
      inWriter.WriteLine("'")
      inWriter.WriteLine("'" & IO.Path.GetFileName(filename))
      inWriter.WriteLine("' Last Genned on Date: " & gendatetime)
      inWriter.WriteLine("'")
      inWriter.WriteLine("'")
      EndRegion(inWriter)
   End Sub

   Public Shared Sub MakeRegion( _
                  ByVal inWriter As CodeDom.Compiler.IndentedTextWriter, _
                  ByVal regionName As String)
      inWriter.WriteLine("")
      inWriter.WriteLineNoTabs("#Region " & DQ & regionName & DQ)
   End Sub

   Public Shared Function EndRegion(ByVal inwriter As CodeDom.Compiler.IndentedTextWriter) As String
      inwriter.WriteLineNoTabs("#End Region")
   End Function

   Public Shared Sub WriteLineAndIndent(ByVal inwriter As CodeDom.Compiler.IndentedTextWriter, ByVal text As String)
      inwriter.WriteLine(text)
      inwriter.Indent += 1
   End Sub

   Public Shared Sub WriteLineAndOutdent(ByVal inwriter As CodeDom.Compiler.IndentedTextWriter, ByVal text As String)
      inwriter.Indent -= 1
      inwriter.WriteLine(text)
   End Sub

End Class
