Option Strict On
Option Explicit On 

Imports System

#Region "Description"
'Example file outputting the SimpleDataContainer class
#End Region

Public Class BruteForceExample
   Public Shared Function GetStream( _
                  ByVal Name As String, _
                  ByVal filename As String, _
                  ByVal gendatetime As String, _
                  ByVal nodeSelect As Xml.XmlNode) _
                  As IO.Stream
      Dim stream As New IO.MemoryStream
      Dim inwriter As New CodeDom.Compiler.IndentedTextWriter( _
                  New IO.StreamWriter(stream))
      Dim nodeList As Xml.XmlNodeList
      Dim nodeColumn As Xml.XmlNode
      Dim singularName As String = Utility.Tools.GetAttributeOrEmpty(nodeSelect, _
                  "SingularName")
      Dim nsmgr As Xml.XmlNamespaceManager = _
                  Utility.Tools.BuildNameSpaceManager( _
                        nodeSelect.OwnerDocument, "dbs", False)

      System.Console.WriteLine("Test VB")
      System.Console.ReadLine()
      Support.FileOpen(inwriter, "KADGen,System", filename, gendatetime)
      inwriter.WriteLine("")
      Support.WriteLineAndIndent(inwriter, "Public Class " & singularName & _
                  "Collection")
      inwriter.WriteLine("Inherits CollectionBase")
      CollectionConstructors(inwriter, nsmgr, nodeSelect)
      PublicAndFriend(inwriter, nsmgr, nodeSelect)
      Support.WriteLineAndOutdent(inwriter, "End Class")

      inwriter.WriteLine("")
      inwriter.WriteLine("")
      Support.WriteLineAndIndent(inwriter, "Public Class " & singularName)
      inwriter.WriteLine("Inherits RowBase")
      ClassLevelDeclarations(inwriter, nsmgr, nodeSelect)
      Constructors(inwriter, nsmgr, nodeSelect)
      BaseClassImplementation(inwriter, nsmgr, nodeSelect)
      Support.MakeRegion(inwriter, "Field access properties")
      nodeList = nodeSelect.SelectNodes("dbs:TableColumns/*", nsmgr)
      For Each nodeColumn In nodeList
         ColumnMethods(inwriter, nsmgr, nodeColumn)
      Next
      Support.EndRegion(inwriter)
      Support.WriteLineAndIndent(inwriter, "End Class")

      inwriter.Flush()
      Return stream
   End Function


   Private Shared Sub CollectionConstructors( _
                  ByVal inWriter As CodeDom.Compiler.IndentedTextWriter, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode)
      Support.MakeRegion(inWriter, "Constructors")
      Support.WriteLineAndIndent(inWriter, "Protected Sub New()")
      inWriter.WriteLine("MyBase.New(" & Support.DQ & _
                     Utility.Tools.GetAttributeOrEmpty(node, "SingularName") & _
                     "Collection" & Support.DQ & ")")
      Support.WriteLineAndOutdent(inWriter, "End Sub")
      Support.EndRegion(inWriter)
   End Sub


   Private Shared Sub PublicAndFriend( _
                  ByVal inWriter As CodeDom.Compiler.IndentedTextWriter, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode)
      Support.MakeRegion(inWriter, _
                  "Public and Friend Properties, Methods and Events")
      Dim nodeTemp As Xml.XmlNode = node.SelectSingleNode( _
                  "dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField", nsmgr)
      Dim primaryKeyName As String = ""
      Dim primaryKey As Xml.XmlNode
      If Not nodeTemp Is Nothing Then
         primaryKeyName = Utility.Tools.GetAttributeOrEmpty(nodeTemp, "Name")
      End If
      primaryKey = node.SelectSingleNode( _
                  "dbs:TableColumns/dbs:TableColumn[@Name='" & primaryKeyName & _
                  "']", nsmgr)
      Support.WriteLineAndIndent(inWriter, "Public Overloads Sub Fill( _")
      inWriter.Indent += 4
      If Not primaryKey Is Nothing Then
         inWriter.Write("ByVal " & primaryKeyName & " As ")
         inWriter.WriteLine(Utility.Tools.GetAttributeOrEmpty(primaryKey, _
                  "NETType") & ", _")
      End If
      inWriter.WriteLine("ByVal UserID As Int32)")
      inWriter.Indent -= 4
      inWriter.WriteLine("ByVal UserID As Int32)")
      inWriter.Write(Utility.Tools.GetAttributeOrEmpty(node, "SingularName") & _
                    "DataAccessor.Fill(Me")
      If Not primaryKey Is Nothing Then
         inWriter.Write(", " & primaryKeyName)
      End If
      inWriter.WriteLine(", UserID)")
      Support.WriteLineAndOutdent(inWriter, "End Sub")
      Support.EndRegion(inWriter)
   End Sub

   Private Shared Sub ClassLevelDeclarations( _
                  ByVal inWriter As CodeDom.Compiler.IndentedTextWriter, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode)
      Dim nodeList As Xml.XmlNodeList
      Dim pre As String = ""
      Support.MakeRegion(inWriter, "Class Level Declarations")
      inWriter.WriteLine("Protected mCollection As " & _
                  Utility.Tools.GetAttributeOrEmpty(node, "SingularName") & _
                  "Collection")
      inWriter.WriteLine("Private Shared mNextPrimaryKey As Int32 = -1")
      nodeList = node.SelectNodes("dbs:TableColumns/*", nsmgr)
      inWriter.WriteLine("")
      For Each nodeSub As Xml.XmlNode In nodeList
         If Utility.Tools.GetAttributeOrEmpty(nodeSub, "NETType").Length = 0 Then
            pre = "' "
         End If
         inWriter.WriteLine(pre & "Private m" & _
                  Utility.Tools.GetAttributeOrEmpty(nodeSub, "Name") & " As " & _
                  Utility.Tools.GetAttributeOrEmpty(nodeSub, "NETType"))
      Next
      Support.EndRegion(inWriter)
   End Sub

   Private Shared Sub Constructors( _
                  ByVal inWriter As CodeDom.Compiler.IndentedTextWriter, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode)
      Dim singularName As String = _
                  Utility.Tools.GetAttributeOrEmpty(node, "SingularName")
      Support.MakeRegion(inWriter, "Constructors")
      Support.WriteLineAndIndent(inWriter, "Friend Sub New(ByVal " & _
                  singularName & "Collection As " & singularName & "Collection)")
      inWriter.WriteLine("MyBase.new()")
      inWriter.WriteLine("mCollection = " & singularName & "Collection")
      Support.WriteLineAndOutdent(inWriter, "End Sub")
      Support.EndRegion(inWriter)

   End Sub

   Private Shared Sub BaseClassImplementation( _
                  ByVal inWriter As CodeDom.Compiler.IndentedTextWriter, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode)
      Support.MakeRegion(inWriter, "Base Class Implementation")
      Dim nodeTemp As Xml.XmlNode = node.SelectSingleNode( _
                  "dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField", nsmgr)
      Dim primaryKeyName As String = ""
      Dim primaryKey As Xml.XmlNode
      If Not nodeTemp Is Nothing Then
         primaryKeyName = Utility.Tools.GetAttributeOrEmpty(nodeTemp, "Name")
      End If
      primaryKey = node.SelectSingleNode( _
               "dbs:TableColumns/dbs:TableColumn[@Name='" & primaryKeyName & "']", _
               nsmgr)
      If Not primaryKey Is Nothing Then
         If Utility.Tools.GetAttributeOrEmpty(primaryKey, "IsAutoIncrement") = "1" _
                  Then
            Support.WriteLineAndIndent(inWriter, "Friend Sub SetNewPrimaryKey()")
            inWriter.WriteLine(primaryKeyName & " = mNextPrimaryKey")
            inWriter.WriteLine("mNextPrimaryKey -= 1")
            Support.WriteLineAndOutdent(inWriter, "End Sub")
         End If
      End If
      Support.EndRegion(inWriter)

   End Sub


   Private Shared Sub ColumnMethods( _
                  ByVal inWriter As CodeDom.Compiler.IndentedTextWriter, _
                  ByVal nsmgr As Xml.XmlNamespaceManager, _
                  ByVal node As Xml.XmlNode)
      Dim netTypeName As String = Utility.Tools.GetAttributeOrEmpty(node, _
                  "NETType")
      Dim name As String = Utility.Tools.GetAttributeOrEmpty(node, "Name")
      If netTypeName.Trim.Length = 0 Then
         inWriter.WriteLine("")
         inWriter.WriteLine("' Column " & name & _
                  " is not included because it uses")
         inWriter.WriteLine("' a SQLType (" & netTypeName & _
                           ") that is not yet supported")
         inWriter.WriteLine("")
      Else
         Support.WriteLineAndIndent(inWriter, "Public Function " & name & _
                  "ColumnInfo As ColumnInfo")
         inWriter.WriteLine("Dim columnInfo As New ColumnInfo")
         inWriter.WriteLine("columnInfo.FieldName = " & Support.DQ & name & _
                  Support.DQ)
         inWriter.WriteLine("columnInfo.FieldType = GetType(" & netTypeName & ")")
         inWriter.WriteLine("columnInfo.SQLType = " & Support.DQ & _
                  Utility.Tools.GetAttributeOrEmpty(node, "SQLType") & Support.DQ)
         inWriter.WriteLine("columnInfo.Caption = " & _
                  Support.DQ & _
                  Utility.Tools.GetAttributeOrEmpty(node, "Caption") & Support.DQ)
         inWriter.WriteLine("columnInfo.Desc = " & _
                  Support.DQ & Utility.Tools.GetAttributeOrEmpty(node, "Desc") & _
                  Support.DQ)
         inWriter.WriteLine("Return columnInfo")
         Support.WriteLineAndOutdent(inWriter, "End Function")

         inWriter.WriteLine("")
         Support.WriteLineAndIndent(inWriter, "Public Property " & name & _
                  " As " & netTypeName)
         Support.WriteLineAndIndent(inWriter, "Get")
         inWriter.WriteLine("Return m" & name)
         Support.WriteLineAndOutdent(inWriter, "End Get")
         Support.WriteLineAndIndent(inWriter, "Set(ByVal Value As " & _
                  netTypeName & ")")
         inWriter.WriteLine("m" & name & " = Value")
         Support.WriteLineAndOutdent(inWriter, "End Set")
         Support.WriteLineAndOutdent(inWriter, "End Property")
      End If

   End Sub
End Class

