Option Strict On
Option Explicit On

Imports System

'! Class Summary: Create Hello World program using direct stream output

Public Class HelloWorldViaBruteForce

#Region "Public Methods and Properties"
   Public Shared Sub GenerateOutput(ByVal outputFileName As String)
      Dim writer As IO.StreamWriter

      Try
         writer = New IO.StreamWriter(outputFileName)
         writer.WriteLine("Option Strict On")
         writer.WriteLine("Option Explicit On")
         writer.WriteLine("")
         writer.WriteLine("Imports System")
         writer.WriteLine("")
         writer.WriteLine("'! Class Summary: Hello World target output ")
         writer.WriteLine("")
         writer.WriteLine("Public Class TargetOutput")
         writer.WriteLine("")
         writer.WriteLine("#Region " & Chr(34) & "Public Methods and Properties" & Chr(34))
         writer.WriteLine("   Public Shared Sub Main()")
         writer.WriteLine("      Console.WriteLine(" & Chr(34) & "Hello World" & Chr(34) & ")")
         writer.WriteLine("   End Sub")
         writer.WriteLine("#End Region")
         writer.WriteLine("")
         writer.WriteLine("End Class")
      Finally
         If Not writer Is Nothing Then
            writer.Flush()
            writer.Close()
         End If
      End Try
   End Sub
#End Region

End Class
