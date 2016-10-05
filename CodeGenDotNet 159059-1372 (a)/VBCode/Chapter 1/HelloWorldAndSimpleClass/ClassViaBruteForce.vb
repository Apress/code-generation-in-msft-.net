Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class ClassViaBruteForce

#Region "Public Methods and Properties"
   Public Shared Sub GenerateOutput( _
               ByVal outputFile As String, _
               ByVal xmlMetaData As Xml.XmlDocument, _
               ByVal tableName As String)
      Dim writer As CodeDom.Compiler.IndentedTextWriter
      Dim node As Xml.XmlNode
      Dim nodeLIst As Xml.XmlNodeList
      Try
         writer = New CodeDom.Compiler.IndentedTextWriter(New IO.StreamWriter(outputFile))
         nodeLIst = xmlMetaData.SelectNodes("/DataSet/Table[@Name='" & tableName & "']/Column")

         writer.WriteLine("Option Strict On")
         writer.WriteLine("Option Explicit On")
         writer.WriteLine("")
         writer.WriteLine("Imports System")
         writer.WriteLine("")
         writer.WriteLine("'! Class Summary: ")
         writer.WriteLine("")
         writer.WriteLine("Public Class TargetOutput")
         writer.WriteLine("")
         writer.WriteLine("#Region " & Chr(34) & "Class level declarations" & Chr(34))
         writer.Indent += 1
         For Each node In nodeLIst
            writer.WriteLine("Private m_" & node.Attributes("Name").Value & " As " & node.Attributes("Type").Value)
         Next
         writer.Indent -= 1
         writer.WriteLine("#End Region")
         writer.WriteLine("")
         writer.WriteLine("#Region " & Chr(34) & "Public Methods and Properties" & Chr(34))
         writer.Indent += 1
         For Each node In nodeLIst
            writer.WriteLine("Public Property " & node.Attributes("Name").Value & "() As " & node.Attributes("Type").Value)
            writer.Indent += 1
            writer.WriteLine("Get")
            writer.Indent += 1
            writer.WriteLine("Return m_" & node.Attributes("Name").Value)
            writer.Indent -= 1
            writer.WriteLine("End Get")
            writer.WriteLine("Set(ByVal Value As " & node.Attributes("Type").Value & ")")
            writer.Indent += 1
            writer.WriteLine("m_" & node.Attributes("Name").Value & " = Value")
            writer.Indent -= 1
            writer.WriteLine("End Set")
            writer.Indent -= 1
            writer.WriteLine("End Property")
            writer.WriteLine("")
         Next
         writer.Indent -= 1
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
