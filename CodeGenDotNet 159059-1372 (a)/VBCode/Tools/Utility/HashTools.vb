' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Tools for working with hash codes
'  Refactor: When a standard for hashing becomes available - swtich to it!

Option Strict On
Option Explicit On 

Imports system

Public Enum FileChanged
   Unknown
   FileDoesntExist
   NotChanged
   Changed
   WhitespaceOnly
End Enum

Public Class HashTools
   Private Shared mHashMarker As String
   Private Shared mHeaderMarker As String

   Public Shared Function IsFileChanged( _
                  ByVal fileName As String, _
                  ByVal commentStart As String, _
                  ByVal commentEnd As String, _
                  ByVal headerMarker As String, _
                  ByVal hashMarker As String) _
                  As FileChanged
      Dim inStream As IO.FileStream
      Dim reader As IO.StreamReader
      Dim writer As IO.StreamWriter

      Try
         If Not IO.File.Exists(fileName) Then
            Return FileChanged.FileDoesntExist
         Else
            inStream = New IO.FileStream(fileName, IO.FileMode.Open, IO.FileAccess.Read)
            reader = New IO.StreamReader(inStream)
            writer = New IO.StreamWriter(New IO.MemoryStream)
            Return IsTextChanged(reader.ReadToEnd, commentStart, commentEnd, headerMarker, hashMarker)
         End If
      Catch ex As System.Exception
         Throw
      Finally
         Try
            reader.Close()
            writer.Close()
         Catch
         End Try
      End Try
   End Function

   Public Shared Function IsTextChanged( _
                  ByVal s As String, _
                  ByVal commentStart As String, _
                  ByVal commentEnd As String, _
                  ByVal headerMarker As String, _
                  ByVal hashMarker As String) As FileChanged
      Dim oldHashCode As String
      Dim newHashCode As String
      Dim fullHeaderMarker As String = commentStart & headerMarker & commentEnd

      oldHashCode = GetHash(s, commentStart, commentEnd, hashMarker)
      If oldHashCode.Length = 0 Then
         Return FileChanged.Unknown
      Else
         s = StripHeader(s, fullHeaderMarker)
         newHashCode = CreateHash(s)

         If oldHashCode = newHashCode.ToString Then
            Return FileChanged.NotChanged
         Else
            Return FileChanged.Changed
            'filechanged.whitspaceonly not yet supported
         End If
      End If
   End Function

   Public Shared Sub ApplyHash( _
                  ByVal fileName As String, _
                  ByVal commentText As String, _
                  ByVal commentStart As String, _
                  ByVal commentEnd As String, _
                  ByVal headerMarker As String, _
                  ByVal hashMarker As String)
      Dim inStream As New IO.FileStream(fileName, IO.FileMode.Open)
      Dim outStream As IO.Stream = ApplyHash(inStream, commentText, _
                        commentStart, commentEnd, headerMarker, hashMarker)
      inStream.Close()
      inStream = New IO.FileStream(fileName, IO.FileMode.Truncate)
      Dim writer As New IO.StreamWriter(inStream)
      writer.Write(outStream)
   End Sub

   Public Shared Function ApplyHash( _
                  ByVal inStream As IO.Stream, _
                  ByVal commentText As String, _
                  ByVal commentStart As String, _
                  ByVal commentEnd As String, _
                  ByVal headerMarker As String, _
                  ByVal hashMarker As String) _
                  As IO.Stream
      Dim s As String
      Dim reader As New IO.StreamReader(inStream)
      Dim writer As New IO.StreamWriter(New IO.MemoryStream)
      Dim hashstring As String
      Dim fullHeaderMarker As String = commentStart & headerMarker & commentEnd

      inStream.Seek(0, IO.SeekOrigin.Begin)
      writer.Write(ApplyHash(reader.ReadToEnd, commentText, commentStart, _
                  commentEnd, True, headerMarker, hashMarker))
      writer.Flush()
      writer.BaseStream.Seek(0, IO.SeekOrigin.Begin)
      Return writer.BaseStream
   End Function

   Public Shared Function ApplyHash( _
                  ByVal s As String, _
                  ByVal commentText As String, _
                  ByVal commentStart As String, _
                  ByVal commentEnd As String, _
                  ByVal isString As Boolean, _
                  ByVal headerMarker As String, _
                  ByVal hashMarker As String) _
                  As String
      Dim sb As New Text.StringBuilder
      Dim hashstring As String
      Dim fullHeaderMarker As String = commentStart & headerMarker & commentEnd

      s = StripHeader(s, fullHeaderMarker)
      hashstring = CreateHash(s)

      sb.Append(fullHeaderMarker)
      sb.Append(commentStart & commentEnd)
      sb.Append(commentStart & commentText & commentEnd)
      sb.Append(commentStart & commentEnd)
      sb.Append(commentStart & hashMarker & hashstring & hashMarker & commentEnd)
      sb.Append(fullHeaderMarker)
      sb.Append(s)
      Return sb.ToString
   End Function

   Private Shared Function StripHeader(ByVal s As String, ByVal searchFor As String) As String
      Dim iStart As Int32 = s.IndexOf(searchFor)
      Dim iEnd As Int32
      If iStart >= 0 Then
         iEnd = s.Substring(iStart + searchFor.Length).IndexOf(searchFor) + iStart + searchFor.Length
         If iEnd >= 0 Then
            iEnd = iEnd + searchFor.Length + 2
            If iEnd <= s.Length() Then
               Return s.Substring(0, iStart) & s.Substring(iEnd)
            Else
               Return ""
            End If
         Else
            Return s
         End If
      Else
         Return s
      End If
   End Function

   Private Shared Function GetHash( _
                  ByVal s As String, _
                  ByVal commentStart As String, _
                  ByVal commentEnd As String, _
                  ByVal hashMarker As String) _
                  As String
      Dim searchForStart As String = commentStart & hashMarker
      Dim searchForEnd As String = hashMarker & commentEnd
      Dim iStart As Int32 = s.IndexOf(searchForStart)
      Dim iEnd As Int32
      Dim iLen As Int32
      If iStart >= 0 Then
         iStart += searchForStart.Length
         iEnd = s.Substring(iStart).IndexOf(searchForEnd) + iStart - 1
         If iEnd >= 0 Then
            ' Messy way to clean off the crlf
            iLen = iEnd - iStart + 1
            Return s.Substring(iStart, iLen)
         Else
            Return ""
         End If
      Else
         Return ""
      End If

   End Function

   Private Shared Function CreateHash(ByVal s As String) As String
      Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
      'Dim writer As New IO.StreamWriter(New IO.MemoryStream)
      Dim hash() As Byte
      Dim input() As Byte
      Dim sb As New Text.StringBuilder
      s = s.Trim
      ReDim input(s.Length - 1)
      For i As Int32 = 0 To input.GetUpperBound(0)
         input(i) = CByte(Microsoft.VisualBasic.Asc(s.Chars(i)))
      Next
      '   writer.Write(s)
      '   writer.BaseStream.Seek(0, IO.SeekOrigin.Begin)
      hash = md5.ComputeHash(input)
      For i As Int32 = 0 To hash.GetUpperBound(0)
         sb.Append(hash(i).ToString("x2"))
      Next
      'Dim BitConcat As Int64
      'For i As Int32 = 0 To 1
      '   BitConcat = 0
      '   For j As Int32 = 0 To 7
      '      If (j = 0) Then
      '         BitConcat = BitConcat Or (Convert.ToInt64(hash(j + 8 * i)))
      '      Else
      '         BitConcat = BitConcat Or (Convert.ToInt64(hash(j + 8 * i)) * Convert.ToInt64(2 ^ (j * 8 - 1)))
      '      End If
      '   Next
      '   sb.Append(Convert.ToString(BitConcat))
      'Next
      Return sb.ToString
   End Function

End Class
