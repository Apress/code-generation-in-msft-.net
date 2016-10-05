' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: The main utility class with miscellaneous tools

Option Strict On
Option Explicit On 

Imports System
Imports System.Diagnostics

Public Enum OutputType
   None
   VB
   CSharp
   XML
   StoredProc
End Enum

Public Enum GenType
   None
   Once
   UntilEdited
   Always
   Overwrite
End Enum



Public Class Tools
   Const vbcrlf As String = Microsoft.VisualBasic.ControlChars.CrLf
   Const vbTab As String = Microsoft.VisualBasic.ControlChars.Tab

   Private Shared mSBForLog As New Text.StringBuilder
   Private Shared mStartTimeForLog As System.DateTime = System.DateTime.Now

   Public Shared Function HelloWorld() As String
      Return "Hello World"
   End Function

   Public Function HelloWorld2() As String
      Return "Hello World 2"
   End Function

   Public Function ToUpper(ByVal s As String) As String
      Return s.ToUpper
   End Function

   Public Shared Function SpaceAtCaps(ByVal s As String) As String
      Dim chars() As Char = s.ToCharArray
      Dim sb As New Text.StringBuilder

      For i As Int32 = 0 To chars.GetUpperBound(0)

         If (i > 0) AndAlso (IsSpaceChar(chars(i)) And Not IsSpaceChar(chars(i - 1))) Then
            sb.Append(" ")
         End If
         sb.Append(chars(i))
      Next
      Return sb.ToString
   End Function
   Private Shared Function IsSpaceChar(ByVal c As Char) As Boolean
      Return System.Char.IsUpper(c) Or System.Char.IsNumber(c)
   End Function

   Public Shared Function GetAttributeOrEmpty( _
                  ByVal node As Xml.XmlNode, _
                  ByVal attributeName As String) _
                  As String
      Dim ret As String = ""
      If Not node Is Nothing Then
         If Not node.Attributes Is Nothing Then
            Dim attr As Xml.XmlNode = node.Attributes.GetNamedItem(attributeName)
            If Not attr Is Nothing Then
               ret = attr.Value
            End If
         End If
      End If
      Return ret
   End Function

   Public Shared Function FixPath( _
               ByVal sPath As String, _
               ByVal basePath As String, _
               ByVal docPath As String, _
               ByVal nodeFilePath As Xml.XmlNode) _
               As String
      ' Assume any square bracketed things should be in a path def in the XML
      Dim sStart As String
      Dim sEnd As String
      Dim sRet As String = sPath
      Dim sSearch As String = ""
      Dim sNew As String = ""
      Dim iStart As Int32
      Dim iEnd As Int32
      Dim node As Xml.XmlNode
      Dim nsmgr As Xml.XmlNamespaceManager = _
               Tools.BuildNameSpaceManager(nodeFilePath, "kg", False)
      Do While sRet.IndexOf("[") >= 0
         iStart = sRet.IndexOf("[")
         iEnd = sRet.IndexOf("]")
         sEnd = sRet.Substring(iEnd + 1)
         sSearch = sRet.Substring(iStart + 1, iEnd - iStart - 1)
         sStart = sRet.Substring(0, iStart)
         If Not nodeFilePath Is Nothing Then
            node = nodeFilePath.SelectSingleNode( _
                        "kg:FilePath[@Name='" & sSearch & "']", nsmgr)
         End If
         If Not node Is Nothing Then
            sNew = System.IO.Path.Combine( _
                        Utility.Tools.GetAttributeOrEmpty(node, "Path"), _
                        Utility.Tools.GetAttributeOrEmpty(node, "File"))
         Else
            sNew = basePath
         End If
         If sNew.StartsWith("/") Or sNew.StartsWith("\") Then
            sNew = sNew.Substring(1)
         End If
         If sEnd.StartsWith("/") Or sEnd.StartsWith("\") Then
            sEnd = sEnd.Substring(1)
         End If
         sRet = IO.Path.Combine(IO.Path.Combine(sStart, sNew), sEnd)
         If Not docPath Is Nothing Then
            IO.Path.Combine(docPath, sRet)
         End If
      Loop
      'If Not nodeFilePath Is Nothing Then
      '   node = nodeFilePath.SelectSingleNode( _
      '               "kg:FilePath[@Name='" & s & "']", nsmgr)
      'End If
      'If Not node Is Nothing Then
      '   sNew = Utility.Tools.GetAttributeOrEmpty(node, "File")
      'End If
      'If Not sRet Is Nothing And Not sNew Is Nothing Then
      '   sRet = IO.Path.Combine(sRet, sNew)
      'End If
      Return sRet
   End Function

   Public Shared Function FixFilter( _
               ByVal sPath As String, _
               ByVal nodeFilter As Xml.XmlNode) _
               As String
      ' Assume any square bracketed things should be in a path def in the XML
      Dim sStart As String
      Dim sEnd As String
      Dim sRet As String = sPath
      Dim sSearch As String = ""
      Dim iStart As Int32
      Dim iEnd As Int32
      Dim node As Xml.XmlNode
      Dim nsmgr As Xml.XmlNamespaceManager = _
               Tools.BuildNameSpaceManager(nodeFilter, "kg", False)
      ' This does NOT support nesting!
      If sRet.IndexOf("[") >= 0 Then
         iStart = sRet.IndexOf("[")
         iEnd = sRet.IndexOf("]")
         sEnd = sRet.Substring(iEnd + 1)
         sSearch = sRet.Substring(iStart + 1, iEnd - iStart - 1)
         sStart = sRet.Substring(0, iStart)
         If Not nodeFilter Is Nothing Then
            node = nodeFilter.SelectSingleNode( _
                        "kg:Filter[@Name=" & """" & sSearch & """" & "]", nsmgr)
         End If
         If Not node Is Nothing Then
            sRet = Utility.Tools.GetAttributeOrEmpty(node, "Filter")
         End If
      End If
      Return sRet
   End Function

   'Public Shared Function FixPath( _
   '               ByVal sPath As String, _
   '               ByVal basePath As String, _
   '               ByVal docPath As String, _
   '               ByVal nodeFilePath As Xml.XmlNode) _
   '               As String
   '   ' Assume any square bracketed things should be in a path def in the XML
   '   Dim s As String = ""
   '   Dim sRet As String = sPath
   '   Dim sFragment As String = sPath
   '   Dim sNew As String = ""
   '   Dim iStart As Int32
   '   Dim iEnd As Int32
   '   Dim node As Xml.XmlNode
   '   Dim nsmgr As Xml.XmlNamespaceManager = _
   '            Tools.BuildNameSpaceManager(nodeFilePath, "kg", False)
   '   Do While sRet.IndexOf("[") >= 0
   '      iStart = sRet.IndexOf("[")
   '      iEnd = sRet.IndexOf("]")
   '      sFragment = sRet.Substring(iEnd + 1)
   '      s = sRet.Substring(iStart + 1, iEnd - iStart - 1)
   '      sRet = sRet.Substring(0, iStart)
   '      If Not nodeFilePath Is Nothing Then
   '         node = nodeFilePath.SelectSingleNode( _
   '                     "kg:FilePath[@Name='" & s & "']", nsmgr)
   '      End If
   '      If Not node Is Nothing Then
   '         sNew = System.IO.Path.Combine( _
   '                     Utility.Tools.GetAttributeOrEmpty(node, "Path"), _
   '                     Utility.Tools.GetAttributeOrEmpty(node, "File"))
   '      Else
   '         sNew = basePath
   '      End If
   '      If Not sNew Is Nothing Then
   '         If sNew.StartsWith("/") Or sNew.StartsWith("\") Then
   '            sNew = sNew.Substring(1)
   '         End If
   '         If sFragment.StartsWith("/") Or sFragment.StartsWith("\") Then
   '            sFragment = sFragment.Substring(1)
   '         End If
   '         sRet = IO.Path.Combine(IO.Path.Combine(sRet, sNew), sFragment)
   '         If Not docPath Is Nothing Then
   '            IO.Path.Combine(docPath, sRet)
   '         End If
   '      End If
   '   Loop
   '   'If Not nodeFilePath Is Nothing Then
   '   '   node = nodeFilePath.SelectSingleNode( _
   '   '               "kg:FilePath[@Name='" & s & "']", nsmgr)
   '   'End If
   '   'If Not node Is Nothing Then
   '   '   sNew = Utility.Tools.GetAttributeOrEmpty(node, "File")
   '   'End If
   '   'If Not sRet Is Nothing And Not sNew Is Nothing Then
   '   '   sRet = IO.Path.Combine(sRet, sNew)
   '   'End If
   '   Return sRet
   'End Function



   Public Shared Function GetAttributeOrEmpty( _
                  ByVal node As Xml.XmlNode, _
                  ByVal ElementName As String, _
                  ByVal attributeName As String, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As String
      Dim elem As Xml.XmlNode
      If ElementName.Trim.Length = 0 Then
         Return GetAttributeOrEmpty(node, attributeName)
      Else
         elem = node.SelectSingleNode(ElementName, nsmgr)
         If elem Is Nothing Then
            Return ""
         Else
            Return GetAttributeOrEmpty(elem, attributeName)
         End If
      End If
   End Function

   Public Shared Function GetAnnotOrEmpty( _
                  ByVal node As Xml.XmlNode, _
                  ByVal attributeName As String) _
                  As String
      Dim nodeTmp As Xml.XmlNode
      nodeTmp = node.SelectSingleNode("annotation[@" & attributeName & "]")
      If Not nodeTmp Is Nothing Then
         Return Tools.GetAttributeOrEmpty(nodeTmp, attributeName)
      Else
         Return ""
      End If
   End Function

   Public Shared Function GetAnnotOrEmpty( _
                  ByVal node As Xml.XmlNode, _
                  ByVal attributeName As String, _
                  ByVal nsmgr As Xml.XmlNamespaceManager) _
                  As String
      Dim nodeTmp As Xml.XmlNode
      nodeTmp = node.SelectSingleNode("xs:annotation[@" & attributeName & "]", nsmgr)
      If Not nodeTmp Is Nothing Then
         Return Tools.GetAttributeOrEmpty(nodeTmp, attributeName)
      Else
         Return ""
      End If
   End Function

   Public Shared Function GetNamespace( _
                  ByVal xmlDoc As Xml.XmlDocument, _
                  ByVal prefix As String) _
                  As String
      Dim node As Xml.XmlNode = xmlDoc.ChildNodes(1)
      If prefix.Trim.Length = 0 Then
         Return GetAttributeOrEmpty(node, "xmlns")
      Else
         Return GetAttributeOrEmpty(node, "xmlns:" & prefix)
      End If
   End Function


   Public Shared Function BuildNameSpaceManager( _
               ByVal xmlDoc As Xml.XmlDocument, _
               ByVal node As Xml.XmlNode, _
               ByVal elemName As String, _
               ByVal nsmgr As Xml.XmlNamespaceManager, _
               ByVal prefix As String) _
               As Xml.XmlNamespaceManager
      Dim namespaceName As String = GetAttributeOrEmpty(node, elemName, prefix & "Namespace", nsmgr)
      Dim NSPrefix As String = GetAttributeOrEmpty(node, elemName, prefix & "NSPrefix", nsmgr)
      Dim newNsmgr As New Xml.XmlNamespaceManager(xmlDoc.NameTable)
      newNsmgr.AddNamespace(NSPrefix, namespaceName)
      Return newNsmgr
   End Function


   Public Shared Function BuildNamespaceManager( _
               ByVal node As Xml.XmlNode, _
               ByVal includeXSD As Boolean) _
               As Xml.XmlNamespaceManager
      Return BuildNamespaceManager(node, "", includeXSD)
   End Function

   Public Shared Function BuildNamespaceManager( _
               ByVal node As Xml.XmlNode, _
               ByVal prefix As String, _
               ByVal includeXSD As Boolean) _
               As Xml.XmlNamespaceManager
      Dim ownerDocument As Xml.XmlDocument
      If node Is Nothing Then
         Debug.WriteLine("oops")
      Else
         If TypeOf node Is Xml.XmlDocument Then
            ownerDocument = CType(node, Xml.XmlDocument)
         Else
            ownerDocument = node.OwnerDocument
         End If
         Return BuildNamespaceManager(ownerDocument, prefix, includeXSD)
      End If
   End Function

   Public Shared Function BuildNamespaceManager( _
                  ByVal doc As Xml.XmlDocument, _
                  ByVal prefix As String, _
                  ByVal includeXSD As Boolean) _
                  As Xml.XmlNamespaceManager
      If doc Is Nothing Then
         Debug.WriteLine("oops")
      Else
         Dim nsmgr As Xml.XmlNamespaceManager = New Xml.XmlNamespaceManager(doc.NameTable)
         nsmgr.AddNamespace(prefix, Tools.GetNamespace(doc, prefix))
         If includeXSD Then
            nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")
         End If
         Return nsmgr
      End If
   End Function

   Public Shared Function GetMethodInfo( _
                   ByVal node As Xml.XmlNode, _
                   ByVal callingType As System.Type, _
                   ByVal nsmgr As Xml.XmlNamespaceManager, _
                   ByVal basePath As String, _
                   ByVal docPath As String, _
                   ByVal nodeFilePath As Xml.XmlNode) _
                   As Reflection.MethodInfo
      Return GetMethodInfo(node, callingType, nsmgr, "", basePath, docPath, nodeFilePath)
   End Function

   Public Shared Function GetMethodInfo( _
                  ByVal node As Xml.XmlNode, _
                  ByVal callingType As System.Type, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal elem As String, _
                  ByVal basePath As String, _
                  ByVal docPath As String, _
                  ByVal nodeFilePath As Xml.XmlNode) _
                  As Reflection.MethodInfo
      Dim name As String
      Dim asm As Reflection.Assembly
      Dim type As System.Type
      Dim methodInfo As Reflection.MethodInfo
      If node Is Nothing Then
      Else
         type = GetSpecifiedType(node, callingType, nsmgr, elem, basePath, docPath, nodeFilePath)
         name = Tools.GetAttributeOrEmpty(node, elem, "MethodName", nsmgr)
         If name.Trim.Length = 0 Then
            name = StripNamespacePrefix(Tools.GetAttributeOrEmpty(node, elem, "name", nsmgr))
         End If
         methodInfo = type.GetMethod(name, Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Static Or Reflection.BindingFlags.IgnoreCase Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic)
         Return methodInfo
      End If
   End Function

   Public Shared Function GetSpecifiedType( _
                  ByVal node As Xml.XmlNode, _
                  ByVal callingType As System.Type, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal elem As String, _
                  ByVal basePath As String, _
                  ByVal docPath As String, _
                  ByVal nodeFilePath As Xml.XmlNode) _
                  As System.Type
      Dim name As String
      Dim asm As System.Reflection.Assembly
      Dim type As System.Type
      Try
         name = Tools.FixPath(Tools.GetAttributeOrEmpty(node, elem, "AssemblyFileName", nsmgr), basePath, docPath, nodeFilePath)
         If name.Trim.Length = 0 Then
            name = Tools.GetAttributeOrEmpty(node, elem, "AssemblyName", nsmgr)
            If name.Trim.Length = 0 Then
               asm = callingType.Assembly
            Else
               asm = Reflection.Assembly.Load(name)
            End If
         Else
            asm = Reflection.Assembly.LoadFrom(name)
         End If

         name = Tools.GetAttributeOrEmpty(node, elem, "TypeName", nsmgr)
         If name.Trim.Length = 0 Then
            type = callingType
         Else
            ' Kind of ugly but it works
            For Each testType As System.Type In asm.GetTypes
               If testType.Name = name Then
                  type = testType
               End If
            Next
         End If
         Return type
      Catch ex As System.Exception
         Throw
      End Try
   End Function

   Public Shared Function StripNamespacePrefix( _
                  ByVal name As String) _
                  As String
      If name.IndexOf(":") >= 0 Then
         Return name.Substring(name.IndexOf(":") + 1)
      Else
         Return name
      End If
   End Function

   Public Shared Function StringArrayFromNodeList( _
                  ByVal nodeList As Xml.XmlNodeList) _
                  As String()
      Return StringArrayFromNodeList(nodeList, "value")
   End Function
   Public Shared Function StringArrayFromNodeList( _
                  ByVal nodeList As Xml.XmlNodeList, _
                  ByVal attrName As String) _
                  As String()
      Dim ret() As String
      ReDim ret(nodeList.Count - 1)
      For i As Int32 = 0 To ret.GetUpperBound(0)
         ret(i) = GetAttributeOrEmpty(nodeList(i), attrName)
      Next
      Return ret
   End Function

   Public Shared Function StripPrefix(ByVal s As String) As String
      If s.IndexOf(":") >= 0 Then
         Return s.Substring(s.IndexOf(":") + 1)
      Else
         Return s
      End If
   End Function

   Public Shared Function GetSQLTypeFromXSDType(ByVal XSDTypeName As String) As String
      Dim localType As String = GetLocalPart(XSDTypeName)

      Select Case localType.ToLower
         Case "string"
            Return "char"
         Case "integer"
            Return "int"
         Case "boolean"
            Return "bit"
         Case Else
            Return localType.ToLower
      End Select
   End Function

   Public Shared Function GetSQLTypeFromSQLType( _
                     ByVal SQLTypeName As String) _
                     As String
      ' This is kind of ugly, and currently we don't store the 
      ' original type, trusting this synonum and case insensitity
      Select Case SQLTypeName.ToLower
         Case "numeric"
            SQLTypeName = "decimal"
      End Select
      ' This is to fix the case for C#
      SQLTypeName = [Enum].Parse(GetType(Data.SqlDbType), SQLTypeName, True).ToString
      Return SQLTypeName
   End Function

   Public Shared Function GetNETTypeFromSQLType( _
                     ByVal SQLTypeName As String) _
                     As String
      Select Case SQLTypeName.ToLower
         Case "int"
            Return "System.Int32"
         Case "smallint"
            Return "System.Int16"
         Case "bigint"
            Return "System.Int64"
         Case "decimal"
            Return "System.Decimal"
         Case "base64Binary"
            Return "System.Byte()"
         Case "boolean"
            Return "System.Boolean"
         Case "dateTime"
            Return "System.DateTime"
         Case "float"
            Return "System.Double"
         Case "real"
            Return "System.Single"
         Case "unsignedByte"
            Return "System.Byte"
         Case "char", "nchar", "varchar", "nvarchar", "ntext", "text"
            Return "System.String"
         Case "datetime", "smalldatetime"
            Return "System.DateTime"
         Case "money", "smallmoney", "numeric"
            Return "System.Decimal"
         Case "bit"
            Return "System.Boolean"
         Case "tinyint"
            Return "System.Byte"
         Case "timestamp"
            Return "System.Byte()"
         Case "uniqueidentifier"
            Return "System.Guid"
         Case "image", "binary", "varbinary"
            Return "System.Byte()" ' This is an issue as it is VB or C# specific
         Case Else
            Debug.WriteLine("type not found - {0}", SQLTypeName)
      End Select
   End Function

   Public Shared Function GetPrefix(ByVal name As String) As String
      Dim ret As String = name

      For i As Int32 = 0 To name.Length - 1
         If name.Substring(i, 1) < "a" Then
            Return name.Substring(0, i)
         End If
      Next
      Return name
   End Function

   Public Shared Function FixName( _
            ByVal name As String) As String
      Return FixName(name, False)
   End Function


   Public Shared Function FixName( _
               ByVal name As String, _
               ByVal removePrefix As String) As String
      Return FixName(name, (removePrefix.Trim.ToLower = "true"))
   End Function

   Public Shared Function FixName( _
               ByVal name As String, _
               ByVal removePrefix As Boolean) As String
      Dim ret As String = name
      Dim keywords As String
      ' remove any blanks
      If removePrefix Then
         ret = ret.Substring(GetPrefix(ret).Length)
         'For i As Int32 = 0 To ret.Length - 1
         '   If ret.Substring(i, 1) < "a" Then
         '      ret = ret.Substring(i)
         '      Exit For
         '   End If
         'Next
      End If
      ret = ret.Replace(" ", "_")

      ' replace c# keywords(C# is case sensitive)
      keywords = " abstract as base bool break byte case catch char checked" & _
                 " class const continue decimal default delegate do double" & _
                 " else enum event explicit extern false finally fixed float" & _
                 " for foreach goto if implicit in int interface internal is" & _
                 " lock long namespace new null object operator out override" & _
                 " params private protected public readonly ref return sbyte" & _
                 " sealed short sizeof stackalloc static string struct switch" & _
                 " this throw true try typeof uint ulong unchecked unsafe" & _
                 " ushort using virtual void volatile while "
      If keywords.IndexOf(" " & ret & " ") > 0 Then
         ret = "_" & ret
      End If

      ' replace vb keywords (vb is not case sensitive)
      keywords = " addhandler addressof andalso alias and ansi as assembly" & _
                 " auto boolean byref byte byval call case catch cbool cbyte" & _
                 " cchar cdate cdec cdbl char cint class clng cobj const" & _
                 " cshort csng cstr ctype date decimal declare default" & _
                 " delegate dim directcast do double each else elseif end" & _
                 " enum erase error event exit false finally for friend" & _
                 " function get gettype gosub goto handles if implements" & _
                 " imports in inherits integer interface is let lib like" & _
                 " long loop me mod module mustinherit mustoverride mybase" & _
                 " myclass namespace new next not nothing notinheritable" & _
                 " notoverridable object on option optional or orelse" & _
                 " overloads overridable overrides paramarray preserve" & _
                 " private property protected public raiseevent readonly" & _
                 " redim rem removehandler resume return select set shadows" & _
                 " shared short single static step stop string structure sub" & _
                 " synclock then throw to true try typeof unicode until" & _
                 " variant when while with withevents writeonly xor"
      If keywords.ToLower.IndexOf(" " & ret & " ") > 0 Then
         ret = "_" & ret
      End If
      Return ret
   End Function

   Public Shared Function GetSingular(ByVal name As String) As String
      If name.ToLower.EndsWith("ss") Or Not name.ToLower.EndsWith("s") Then
         ' This is generally not a plural    
         Return name
      ElseIf name.ToLower.EndsWith("us") Then
         ' This is generally not a plural
         Return name
      ElseIf name.ToLower.EndsWith("sses") Then
         Return name.Substring(0, name.Length - 2)
      ElseIf name.ToLower.EndsWith("uses") Then
         Return name.Substring(0, name.Length - 2)
      ElseIf name.ToLower.EndsWith("ies") Then
         ' NOTE: You can't tell from here if it used to be ey or y. You must
         '       add the specific cases important to you.
         Return name.Substring(0, name.Length - 3) & "y"
      ElseIf name.ToLower.EndsWith("s") Then
         Return name.Substring(0, name.Length - 1)
      Else
         Return name ' We shouldn't be able to get here
      End If
   End Function


   Public Shared Function GetPlural(ByVal name As String) As String
      ' Simple rules, you may need to expand these. Create new Case entries 
      ' above the Else for any exceptions important to you
      Dim testName As String = GetSingular(name)
      Select Case testName
         Case Else
            If testName.EndsWith("ey") Then
               Return testName.Substring(0, testName.Length - 2) & "ies"
            ElseIf testName.EndsWith("ay") Then
               Return testName.Substring(0, testName.Length) & "s"
            ElseIf testName.EndsWith("y") Then
               Return testName.Substring(0, testName.Length - 1) & "ies"
            ElseIf testName.EndsWith("ss") Then
               Return testName.Substring(0, testName.Length) & "es"
            ElseIf testName.EndsWith("us") Then
               Return testName.Substring(0, testName.Length) & "es"
            ElseIf testName.EndsWith("s") Then
               Return testName.Substring(0, testName.Length - 1) & "es"
            Else
               Return testName & "s"
            End If
      End Select
   End Function

   Public Shared Function IsPlural(ByVal s As String) As Boolean
      If s.EndsWith("s") Then
         ' it might be plural. Here is a simple test we can expand as needed
         s = s.Substring(0, s.Length - 1)
         If s.EndsWith("s") Then
            Return False
         ElseIf s.EndsWith("u") Then
            Return False
         Else
            Return True
         End If
      End If
   End Function

   Public Shared Function GetLocalPart(ByVal s As String) As String
      Return s.Substring(s.IndexOf(":") + 1)
   End Function

   Public Shared Function MakeNewNodeFromXSDNode( _
               ByVal xmldoc As Xml.XmlDocument, _
               ByVal namespaceURI As String, _
               ByVal prefix As String, _
               ByVal xsdElementNode As Xml.XmlNode) _
               As Xml.XmlNode
      Dim name As String = Utility.Tools.GetAttributeOrEmpty( _
                           xsdElementNode, "name")
      Dim attrName As String
      Dim dataNode As Xml.XmlElement = xmldoc.CreateElement( _
                           prefix & ":" & name, namespaceURI)
      Dim xsdAttrNodeList As Xml.XmlNodeList
      Dim attrValue As String
      Dim xsdNodeList As Xml.XmlNodeList
      Dim xsdTypeNode As Xml.XmlNode
      Dim xsdAttrTypeNode As Xml.XmlNode
      Dim nsmgr As New Xml.XmlNamespaceManager(xmldoc.NameTable)
      Dim typeName As String = Utility.Tools.GetAttributeOrEmpty( _
                           xsdElementNode, "type")
      nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema")

      If xsdElementNode.Attributes.GetNamedItem("type") Is Nothing Then
         ' This is an anonymous type
         xsdTypeNode = xsdElementNode.SelectSingleNode("xs:complexType", nsmgr)
      Else
         ' This is a named type
         ' The following is an XPath union
         xsdTypeNode = xsdElementNode.SelectSingleNode( _
                           "/xs:schema/xs:complexType[@name='" & typeName & "']", _
                           nsmgr)
      End If

      If Not xsdTypeNode Is Nothing Then
         xsdAttrNodeList = xsdTypeNode.SelectNodes("xs:attribute", nsmgr)
         For Each xsdAttrNode As Xml.XmlNode In xsdAttrNodeList
            attrName = Utility.Tools.GetAttributeOrEmpty(xsdAttrNode, "name")
            typeName = Utility.Tools.GetAttributeOrEmpty(xsdAttrNode, "type")
            ' Cheat on namespace handling
            attrValue = ""
            If typeName.StartsWith("xs:") Then
               Select Case typeName
                  Case "xs:int"
                     attrValue = "0"
                  Case "xs:boolean"
                     attrValue = "false"
               End Select
            Else
               ' A simple type, assume its a simple type of enumeratio, grab first
               xsdAttrTypeNode = xsdElementNode.SelectSingleNode( _
                           "/xs:schema/xs:simpleType[@name='" & typeName & _
                           "']/xs:restriction/xs:enumeration", nsmgr)
               If Not xsdAttrTypeNode Is Nothing Then
                  attrValue = GetAttributeOrEmpty(xsdAttrTypeNode, "value")
               End If
            End If
            dataNode.Attributes.Append(Utility.xmlHelpers.NewAttribute( _
                           xmldoc, attrName, attrValue))
         Next
         xsdNodeList = xsdTypeNode.SelectNodes( _
                           "xs:sequence/xs:element|xs:all/xs:element", nsmgr)
         ' I'm not currently supporting choice because I am not sure what you'd do
         For Each xsdChildNode As Xml.XmlNode In xsdNodeList
            dataNode.AppendChild(MakeNewNodeFromXSDNode( _
                           xmldoc, namespaceURI, prefix, xsdChildNode))
         Next
      End If
      Return dataNode
   End Function

   Public Shared Sub MakePathIfNeeded(ByVal fileName As String)
      Dim path As String = IO.Path.GetDirectoryName(fileName)
      If Not IO.Directory.Exists(path) Then
         IO.Directory.CreateDirectory(path)
      End If
   End Sub

   Public Shared Function GetStreamFromStringResource( _
                     ByVal type As System.Type, _
                     ByVal name As String) _
                     As IO.Stream
      Dim ass As Reflection.Assembly
      Dim stream As IO.Stream
      ass = type.Assembly
      stream = ass.GetManifestResourceStream(type, name)
      Return stream
   End Function

   Public Shared Function ShowNamespaceManager( _
                     ByVal nsmgr As Xml.XmlNamespaceManager) _
                     As String
      Dim s As String
      Dim sThis As String
      For Each ns As Object In nsmgr
         sThis = ns.GetType.Name & vbTab
         If sThis Is Nothing Then
            sThis &= "Nothing" & vbcrlf
         Else
            sThis &= s.ToString & vbcrlf
         End If
         s &= sThis
      Next
      Return s
   End Function

   Public Shared Function SubstringBefore( _
               ByVal searchIn As String, _
               ByVal searchFor As String) _
               As String
      Return SubstringBefore(searchIn, searchFor, True)
   End Function

   Public Shared Function SubstringAfter( _
               ByVal searchIn As String, _
               ByVal searchFor As String) _
               As String
      Return SubstringAfter(searchIn, searchFor, True)
   End Function

   Public Shared Function SubstringAfterLast( _
               ByVal searchIn As String, _
               ByVal searchFor As String) _
               As String
      Return SubstringAfterLast(searchIn, searchFor, True)
   End Function

   Public Shared Function SubstringBefore( _
               ByVal searchIn As String, _
               ByVal searchFor As String, _
               ByVal IsCaseSensitive As Boolean) _
               As String
      If Not IsCaseSensitive Then
         searchIn = searchIn.ToLower
         searchFor = searchFor.ToLower
      End If
      Dim ipos As Int32 = searchIn.IndexOf(searchFor)
      Return searchIn.Substring(0, ipos)
   End Function

   Public Shared Function SubstringAfter( _
               ByVal searchIn As String, _
               ByVal searchFor As String, _
               ByVal IsCaseSensitive As Boolean) _
               As String
      If Not IsCaseSensitive Then
         searchIn = searchIn.ToLower
         searchFor = searchFor.ToLower
      End If
      Dim ipos As Int32 = searchIn.IndexOf(searchFor)
      Return searchIn.Substring(ipos + searchFor.Length)
   End Function

   Public Shared Function SubstringAfterLast( _
               ByVal searchIn As String, _
               ByVal searchFor As String, _
               ByVal IsCaseSensitive As Boolean) _
               As String
      If Not IsCaseSensitive Then
         searchIn = searchIn.ToLower
         searchFor = searchFor.ToLower
      End If
      Dim ipos As Int32 = searchIn.LastIndexOf(searchFor)
      Return searchIn.Substring(ipos + searchFor.Length)
   End Function

   Public Shared Function SubstringBefore( _
               ByVal searchIn As String, _
               ByVal searchFor() As String) As String
      Return SubstringBefore(searchIn, searchFor, True)
   End Function

   Public Shared Function SubstringAfter( _
               ByVal searchIn As String, _
               ByVal searchFor() As String) As String
      Return SubstringAfter(searchIn, searchFor, True)
   End Function

   Public Shared Function SubstringAfterLast( _
               ByVal searchIn As String, _
               ByVal searchFor() As String) As String
      Return SubstringAfterLast(searchIn, searchFor, True)
   End Function

   Public Shared Function SubstringContains( _
               ByVal searchIn As String, _
               ByVal searchFor() As String) As Boolean
      Return SubstringContains(searchIn, searchFor, True)
   End Function

   Public Shared Function SubstringContainsWhich( _
               ByVal searchIn As String, _
               ByVal searchFor() As String) As String
      Return SubstringContainsWhich(searchIn, searchFor, True)
   End Function

   Public Shared Function SubstringBefore( _
               ByVal searchIn As String, _
               ByVal searchFor() As String, _
               ByVal IsCaseSensitive As Boolean) As String
      Dim find As String
      If Not IsCaseSensitive Then
         searchIn = searchIn.ToLower
      End If
      For i As Int32 = 0 To searchFor.GetLength(0)
         If IsCaseSensitive Then
            find = searchFor(i)
         Else
            find = searchFor(i).ToLower
         End If
         If searchIn.IndexOf(find) > 0 Then
            Dim ipos As Int32 = searchIn.IndexOf(find)
            Return searchIn.Substring(0, ipos + 1)
         End If
      Next
   End Function

   Public Shared Function SubstringAfter( _
               ByVal searchIn As String, _
               ByVal searchFor() As String, _
               ByVal IsCaseSensitive As Boolean) As String
      Dim find As String
      If Not IsCaseSensitive Then
         searchIn = searchIn.ToLower
      End If
      For i As Int32 = 0 To searchFor.GetLength(0)
         If IsCaseSensitive Then
            find = searchFor(i)
         Else
            find = searchFor(i).ToLower
         End If
         If searchIn.IndexOf(find) > 0 Then
            Dim ipos As Int32 = searchIn.IndexOf(find)
            Return searchIn.Substring(ipos + searchFor.Length)
         End If
      Next
   End Function

   Public Shared Function SubstringAfterLast( _
               ByVal searchIn As String, _
               ByVal searchFor() As String, _
               ByVal IsCaseSensitive As Boolean) As String
      Dim find As String
      If Not IsCaseSensitive Then
         searchIn = searchIn.ToLower
      End If
      For i As Int32 = 0 To searchFor.GetLength(0)
         If IsCaseSensitive Then
            find = searchFor(i)
         Else
            find = searchFor(i).ToLower
         End If
         If searchIn.IndexOf(find) > 0 Then
            Dim ipos As Int32 = searchIn.LastIndexOf(find)
            Return searchIn.Substring(ipos + searchFor.Length)
         End If
      Next
   End Function

   Public Shared Function SubstringContains( _
               ByVal searchIn As String, _
               ByVal searchFor() As String, _
               ByVal IsCaseSensitive As Boolean) As Boolean
      Dim find As String
      If Not IsCaseSensitive Then
         searchIn = searchIn.ToLower
      End If
      For i As Int32 = 0 To searchFor.GetLength(0)
         If IsCaseSensitive Then
            find = searchFor(i)
         Else
            find = searchFor(i).ToLower
         End If
         If searchIn.IndexOf(find) > 0 Then
            Return True
         End If
      Next
   End Function

   Public Shared Function SubstringContainsWhich( _
               ByVal searchIn As String, _
               ByVal searchFor() As String, _
               ByVal IsCaseSensitive As Boolean) As String
      Dim find As String
      Dim index As Int32
      Dim maxIndex As Int32
      Dim pos As Int32 = -1
      If Not IsCaseSensitive Then
         searchIn = searchIn.ToLower
      End If
      For i As Int32 = 0 To searchFor.GetLength(0)
         If IsCaseSensitive Then
            find = searchFor(i)
         Else
            find = searchFor(i).ToLower
         End If
         index = searchIn.IndexOf(find)
         If index > 0 Then
            If index < maxIndex Then
               pos = i
            End If
         End If
      Next
      Return searchFor(pos)
   End Function

   Public Shared Function GetMatchingParenPosition( _
               ByVal s As String) _
               As Int32
      Return GetMatchingPunctuation(s, "("c, ")"c)
   End Function

   Public Shared Function GetMatchingPunctuation( _
               ByVal s As String, _
               ByVal punc1 As Char, _
               ByVal punc2 As Char) _
               As Int32
      Dim iDepth As Int32
      Dim chars() As Char = s.ToCharArray
      If punc1 = punc2 Then
         Throw New System.ApplicationException("The GetMatchingPunctuation method doesn't work with matching open and close characters")
      End If
      If s.IndexOf(punc1) >= 0 Then
         For i As Int32 = 0 To s.Length - 1
            If chars(i) = punc1 Then
               iDepth += 1
            ElseIf chars(i) = punc2 Then
               iDepth -= 1
            End If
            If iDepth = 0 Then
               Return i
            End If
         Next
         Throw New System.ApplicationException("Improperly nested parentheses")
      Else
         Return -1
      End If

   End Function

   Public Shared Function CharArrayToString(ByVal chars() As Char, _
                 ByVal startpos As Int32) _
                 As String
      Return CharArrayToString(chars, startpos, chars.GetLength(0))
   End Function

   Public Shared Function CharArrayToString(ByVal chars() As Char) _
              As String
      Return CharArrayToString(chars, 0, chars.GetLength(0))
   End Function

   Public Shared Function CharArrayToString(ByVal chars() _
                  As Char, _
                  ByVal startpos As Int32, _
                  ByVal length As Int32) As String
      Dim sb As New Text.StringBuilder
      For i As Int32 = startpos To length - 1
         sb.Append(chars(i))
      Next
      Return sb.ToString
   End Function

   Public Shared Function Test(ByVal node As Xml.XmlNode) As String
      Return "George"
   End Function

   Public Shared Function Test2(ByVal node As Xml.XPath.XPathNavigator) As String
      Return "Ron"
   End Function

   Public Shared Function Test3(ByVal node As Xml.XPath.XPathNodeIterator) As String
      Return "Ginny"
   End Function

   Public Shared Function TranslateSQLExpression( _
                  ByVal constraint As String, _
                  ByVal currentObject As String, _
                  ByVal currentProperty As String) _
                  As Xml.XmlNode
      Dim xmlDoc As Xml.XmlDocument
      Dim nsmgr As Xml.XmlNamespaceManager
      Dim oTranslate As New TranslateSQL
      xmlDoc = KADGen.LibraryInterop.Singletons.XMLDoc
      nsmgr = KADGen.LibraryInterop.Singletons.NsMgr
      Dim node As Xml.XmlNode = xmlDoc.CreateElement("CheckConstraint")
      node.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, "Fred", "True"))
      node.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, "OriginalClause", constraint))
      node.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, "VBClause", oTranslate.TranslateSQLToVB(constraint, currentObject, currentProperty, xmlDoc, nsmgr, True)))
      node.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, "CSharpClause", oTranslate.TranslateSQLToVB(constraint, currentObject, currentProperty, xmlDoc, nsmgr, True)))
      node.Attributes.Append(xmlHelpers.NewAttribute(xmlDoc, "EnglishClause", oTranslate.TranslateSQLToEnglish(constraint, currentObject, currentProperty, xmlDoc, nsmgr)))
      Return node

   End Function

   Public Shared Sub LogEntry(ByVal sMessage As String)
      'mSBForLog.Append(DateTime.Now.Ticks.ToString("N") & "-" & sMessage & vbcrlf)
      Dim dt As DateTime = DateTime.Now
      Dim tickcount As Int64
      Static ticks As Int64
      If ticks = 0 Then
         ticks = DateTime.Now.Ticks
      End If
      tickcount = (DateTime.Now.Ticks - ticks)
      If tickcount > 160000 Then
         mSBForLog.Append("*** ")
      Else
         mSBForLog.Append("    ")
      End If
      mSBForLog.Append(dt.ToString("T") & ":" & dt.Millisecond.ToString("000") & _
                        "(" & (tickcount \ 10000).ToString("000,000") & _
                        ") -" & sMessage & vbcrlf)
      ticks = DateTime.Now.Ticks
      If DateTime.Now.Subtract(mStartTimeForLog).Seconds > 10 Then
         mStartTimeForLog = DateTime.Now
      End If
      If False Then
         mSBForLog = New Text.StringBuilder
         Throw New System.Exception
      End If
   End Sub

   Public Shared Sub WriteLogAndClear(ByVal sFileName As String)
      If mSBForLog.Length > 0 Then
         Dim writer As New IO.StreamWriter(sFileName)
         writer.Write(mSBForLog.ToString)
         writer.Flush()
         writer.Close()
         mSBForLog = New Text.StringBuilder
      End If
   End Sub
End Class
