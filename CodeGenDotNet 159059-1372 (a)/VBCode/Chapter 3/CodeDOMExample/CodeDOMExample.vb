Option Strict On
Option Explicit On 

Imports System
Imports System.CodeDom

'! Class Summary: 

Public Class CodeDOMExample

#Region "Class level declarations - empty"
#End Region

#Region "Constructors - empty"
#End Region

#Region "Public Methods and Properties"
   Public Shared Function BuildGraph(ByVal nodeSelect As Xml.XmlNode) As CodeCompileUnit
      ' Create the compile unit
      Dim compileUnit As New CodeCompileUnit
      Try

         ' Create some literals
         Dim exp As New CodePrimitiveExpression("Hello World")
         Dim exp2 As New CodePrimitiveExpression(42)
         Dim exp3 As New CodePrimitiveExpression(3)
         Dim s As String = "Sam"
         Dim exp4 As New CodePrimitiveExpression(s)
         Dim exp5 As New CodePrimitiveExpression(True)

         ' Declare some objects and create some object references
         Dim decl As New CodeVariableDeclarationStatement( _
                             GetType(System.Int32), "iSum")
         Dim decl2 As New CodeVariableDeclarationStatement( _
                              "System.Int32", "iValue", exp2)
         Dim decl3 As New CodeVariableDeclarationStatement( _
                               GetType(System.IO.Stream), "stream")
         Dim decl4 As New CodeVariableDeclarationStatement( _
                             GetType(System.String), "fileName", _
                             New CodePrimitiveExpression("Test.txt"))


         ' Create the CodeGraph structure and include declarations
         Dim nSpace As New CodeNamespace("CodeDOMTest")
         CompileUnit.Namespaces.Add(nSpace)
         ' Add an import/ CSharp using statment(just for demonstration)
         nSpace.Imports.Add(New CodeNamespaceImport("Fred2 = System"))
         ' Create a class to hold code
         Dim clsStartup As New CodeTypeDeclaration("Startup")
         nSpace.Types.Add(clsStartup)
         ' To run as an executable, you'll need a method that's an entry point
         Dim entry As New CodeEntryPointMethod
         entry.Name = "Main"
         clsStartup.Members.Add(entry)

         ' Add Option Strict and Option Explicit for Visual Basic .NET
         compileUnit.UserData.Add("AllowLateBound", False)
         compileUnit.UserData.Add("RequireVariableDeclaration", True)

         ' Output some code based on earlier declarations and show function usage
         entry.Statements.Add(decl)
         entry.Statements.Add(decl2)
         entry.Statements.Add(decl3)
         entry.Statements.Add(decl4)
         Dim rExpConsole As New CodeTypeReferenceExpression( _
                        GetType(System.Console))
         Dim stmt1 As New CodeExpressionStatement( _
                        New CodeMethodInvokeExpression(rExpConsole, _
                           "WriteLine", _
                           exp))
         entry.Statements.Add(stmt1)
         Dim stmt2 As New CodeAssignStatement( _
                        New CodeVariableReferenceExpression("iSum"), _
                        New CodeBinaryOperatorExpression(exp2, _
                           CodeBinaryOperatorType.Add, _
                           New CodePrimitiveExpression(23)))
         entry.Statements.Add(stmt2)

         ' Create an object and assign to a variable accessing an enum
         Dim enumValue As New CodeFieldReferenceExpression( _
                        New CodeTypeReferenceExpression( _
                              GetType(System.IO.FileMode)), _
                        "Create")
         Dim stmt3 As New CodeAssignStatement( _
                        New CodeVariableReferenceExpression("stream"), _
                           New CodeObjectCreateExpression( _
                              GetType(System.IO.FileStream), _
                              New CodeVariableReferenceExpression("fileName"), _
                              enumValue))
         entry.Statements.Add(stmt3)


         ' Declare an array
         entry.Statements.Add( _
                        New CodeVariableDeclarationStatement( _
                        GetType(System.Int32()), "aInts"))
         ' Shows option for type declaration
         entry.Statements.Add( _
                        New CodeVariableDeclarationStatement( _
                        New CodeTypeReference( _
                              New CodeTypeReference(GetType(System.Int32)), 1), _
                        "a2Ints"))
         Dim var2AInts As New CodeVariableReferenceExpression("aInts")
         entry.Statements.Add(New CodeAssignStatement( _
                        New CodeVariableReferenceExpression("a2Ints"), _
                        New CodeArrayCreateExpression("System.Int32", 10)))
         Dim varAInts As New CodeVariableReferenceExpression("aInts")
         entry.Statements.Add(New CodeAssignStatement( _
                        varAInts, _
                        New CodeArrayCreateExpression("System.Int32", _
                           New CodePrimitiveExpression(0), _
                           New CodePrimitiveExpression(1), _
                           New CodePrimitiveExpression(2), _
                           New CodePrimitiveExpression(3), _
                           New CodePrimitiveExpression(4), _
                           New CodePrimitiveExpression(5), _
                           New CodePrimitiveExpression(6), _
                           New CodePrimitiveExpression(7), _
                           New CodePrimitiveExpression(8), _
                           New CodePrimitiveExpression(9))))
         ' Assign an array value to a variable
         entry.Statements.Add( _
                        New CodeAssignStatement( _
                              New CodeVariableReferenceExpression("iValue"), _
                              New CodeArrayIndexerExpression( _
                                    varAInts, New CodePrimitiveExpression(3))))

         ' Assign move an item down by one in the array
         entry.Statements.Add( _
                        New CodeVariableDeclarationStatement( _
                              "System.Int32", "i", _
                              New CodePrimitiveExpression(0)))
         Dim varI As New CodeVariableReferenceExpression("i")
         entry.Statements.Add( _
                        New CodeAssignStatement( _
                              New CodeArrayIndexerExpression( _
                                    varAInts, varI), _
                              New CodeArrayIndexerExpression( _
                                    varAInts, _
                                    New CodeBinaryOperatorExpression( _
                                       varI, CodeBinaryOperatorType.Add, _
                                       New CodePrimitiveExpression(1)))))


         ' Conditional Statement
         Dim ifBlock As New CodeConditionStatement( _
                        New CodeBinaryOperatorExpression( _
                              varI, CodeBinaryOperatorType.GreaterThan, _
                              New CodePrimitiveExpression(6)))
         ifBlock.TrueStatements.Add(WriteLineExpression("True Executed"))
         ifBlock.FalseStatements.Add(WriteLineExpression("False Executed"))
         entry.Statements.Add(ifBlock)

         ' Loop using previously defined variable I
         Dim forLoop As New CodeIterationStatement
         forLoop.InitStatement = New CodeAssignStatement(varI, New CodePrimitiveExpression(0))
         forLoop.TestExpression = New CodeBinaryOperatorExpression( _
                                       varI, CodeBinaryOperatorType.LessThanOrEqual, _
                                       New CodePrimitiveExpression(9))
         forLoop.IncrementStatement = New CodeAssignStatement(varI, _
                                    New CodeBinaryOperatorExpression( _
                                       varI, CodeBinaryOperatorType.Add, _
                                       New CodePrimitiveExpression(1)))
         forLoop.Statements.Add( _
                        New CodeExpressionStatement( _
                              New CodeMethodInvokeExpression(rExpConsole, _
                                    "WriteLine", _
                                    New CodeArrayIndexerExpression(varAInts, varI))))
         entry.Statements.Add(forLoop)

         ' NOTE: The following code thows a Null Reference exception
         'Dim forLoop2 As New CodeIterationStatement
         'forLoop2.TestExpression = New CodeBinaryOperatorExpression( _
         '                              varI, CodeBinaryOperatorType.LessThanOrEqual, _
         '                              New CodePrimitiveExpression(9))
         'forLoop2.IncrementStatement = New CodeAssignStatement(varI, _
         '                           New CodeBinaryOperatorExpression( _
         '                              varI, CodeBinaryOperatorType.Add, _
         '                              New CodePrimitiveExpression(1)))
         'forLoop2.Statements.Add( _
         '               New CodeExpressionStatement( _
         '                     New CodeMethodInvokeExpression(rExpConsole, _
         '                           "WriteLine", _
         '                           New CodeArrayIndexerExpression(varAInts, varI))))
         'entry.Statements.Add(forLoop2)

         entry.Statements.Add( _
                       New CodeExpressionStatement( _
                          New CodeMethodInvokeExpression( _
                             New CodeMethodReferenceExpression( _
                                New CodeVariableReferenceExpression("obj"), _
                               "MethodA"), _
                             New CodeMethodInvokeExpression( _
                                New CodeMethodReferenceExpression( _
                                   New CodeVariableReferenceExpression("obj"), _
                                   "MethodC"), _
                                   New CodeVariableReferenceExpression("j")))))


         ' Display some weirdnesses
         entry.Statements.Add(New CodeMethodInvokeExpression(New CodeTypeReferenceExpression("Startup"), "EqualityDifference"))
         clsStartup.Members.Add(equalityDifferenceExample)
         Test6(nSpace)

      Catch ex As System.Exception
         Throw ex
      End Try

      Return CompileUnit
   End Function

   Public Shared Function equalityDifferenceExample() As CodeTypeMember
      Dim mbr As New CodeMemberMethod
      mbr.Name = "EqualityDifference"
      ' Use a line like  the following to create a function returning a value
      'mbr.ReturnType = New CodeTypeReference(GetType(Int32))
      mbr.Attributes = MemberAttributes.Static
      mbr.Statements.Add(New CodeVariableDeclarationStatement( _
                        GetType(System.Int32), "iValue"))

      Dim bexp As New CodeDom.CodeBinaryOperatorExpression( _
               New CodeDom.CodeVariableReferenceExpression("iValue"), _
                      CodeDom.CodeBinaryOperatorType.Assign, _
                      New CodeDom.CodeBinaryOperatorExpression( _
                            New CodeDom.CodePrimitiveExpression(42), _
                            CodeDom.CodeBinaryOperatorType.Add, _
                            New CodeDom.CodePrimitiveExpression(23)))
      Dim ifBlock As New CodeDom.CodeConditionStatement(bexp)
      ifBlock.TrueStatements.Add(WriteLineExpression("True Executed"))
      ifBlock.FalseStatements.Add(WriteLineExpression("False Executed"))
      mbr.Statements.Add(ifBlock)
      Return mbr
   End Function

   Public Shared Function Test() As CodeTypeMember
      Dim mbr As New CodeMemberMethod
      mbr.Name = "Test"
      mbr.Attributes = MemberAttributes.[New]
      Return mbr
   End Function

   Public Shared Function Test2() As CodeTypeMember
      Dim mbr As New CodeMemberMethod
      mbr.Name = "Test2"
      mbr.Attributes = MemberAttributes.FamilyAndAssembly
      Return mbr
   End Function

   Public Shared Function Test3() As CodeTypeMember
      Dim mbr As New CodeMemberMethod
      mbr.Name = "Test3"
      mbr.Attributes = MemberAttributes.Overloaded
      Return mbr
   End Function

   Public Shared Function Test4() As CodeTypeMember
      Dim mbr As New CodeMemberMethod
      mbr.Name = "Test4"
      mbr.Attributes = MemberAttributes.Assembly
      Return mbr
   End Function

   Shared Sub Test6(ByVal nspace As CodeNamespace)
      Dim cls As New CodeTypeDeclaration("TestClass")
      Dim comment As CodeDom.CodeCommentStatement
      cls.BaseTypes.Add("TestBase")
      cls.BaseTypes.Add("ITest")
      cls.BaseTypes.Add("ITest2")
      cls.BaseTypes.Add("ITest3")
      Dim mbr As New CodeMemberMethod
      mbr.ImplementationTypes.Add("ITest")
      mbr.ImplementationTypes.Add("ITest2")
      mbr.Name = "Foo"
      mbr.Attributes = MemberAttributes.Public
      cls.Members.Add(mbr)

      mbr = New CodeMemberMethod
      mbr.Name = "DefaultScope"
      comment = New CodeDom.CodeCommentStatement(New CodeDom.CodeComment(CInt(mbr.Attributes).ToString("X") & "-" & mbr.Attributes.ToString))
      mbr.Statements.Add(comment)
      cls.Members.Add(mbr)

      mbr = New CodeMemberMethod
      mbr.Name = "BarZ"
      mbr.Attributes = MemberAttributes.Private
      mbr.PrivateImplementationType = New CodeTypeReference("ITest3")
      comment = New CodeDom.CodeCommentStatement(New CodeDom.CodeComment(CInt(mbr.Attributes).ToString("X") & "-" & mbr.Attributes.ToString))
      mbr.Statements.Add(comment)
      cls.Members.Add(mbr)

      mbr = New CodeMemberMethod
      mbr.Name = "ScopeTest"
      comment = New CodeDom.CodeCommentStatement(New CodeDom.CodeComment(CInt(mbr.Attributes).ToString("X") & "-" & mbr.Attributes.ToString))
      mbr.Statements.Add(comment)
      cls.Members.Add(mbr)

      mbr = New CodeMemberMethod
      mbr.Name = "TestShadows"
      mbr.Attributes = MemberAttributes.[New] Or MemberAttributes.Public
      comment = New CodeDom.CodeCommentStatement(New CodeDom.CodeComment(CInt(mbr.Attributes).ToString("X") & "-" & mbr.Attributes.ToString))
      mbr.Statements.Add(comment)
      cls.Members.Add(mbr)

      mbr = New CodeMemberMethod
      mbr.Name = "TestOverride"
      mbr.Attributes = MemberAttributes.Override Or MemberAttributes.Public
      comment = New CodeDom.CodeCommentStatement(New CodeDom.CodeComment(CInt(mbr.Attributes).ToString("X") & "-" & mbr.Attributes.ToString))
      mbr.Statements.Add(comment)
      cls.Members.Add(mbr)

      mbr = New CodeMemberMethod
      mbr.Name = "TestOverloaded2"
      mbr.Attributes = MemberAttributes.Overloaded Or MemberAttributes.Public
      comment = New CodeDom.CodeCommentStatement(New CodeDom.CodeComment(CInt(mbr.Attributes).ToString("X") & "-" & mbr.Attributes.ToString))
      mbr.Statements.Add(comment)
      cls.Members.Add(mbr)

      mbr = New CodeMemberMethod
      mbr.Name = "TestFinal"
      mbr.Attributes = MemberAttributes.Final 'Or MemberAttributes.Public
      comment = New CodeDom.CodeCommentStatement(New CodeDom.CodeComment(CInt(mbr.Attributes).ToString("X") & "-" & mbr.Attributes.ToString))
      mbr.Statements.Add(comment)
      cls.Members.Add(mbr)
      nspace.Types.Add(cls)

      mbr = New CodeMemberMethod
      mbr.Name = "TestFinalAndOverrides"
      mbr.Attributes = MemberAttributes.Override Or MemberAttributes.Final 'Or MemberAttributes.Public
      comment = New CodeDom.CodeCommentStatement(New CodeDom.CodeComment(CInt(mbr.Attributes).ToString("X") & "-" & mbr.Attributes.ToString))
      mbr.Statements.Add(comment)
      cls.Members.Add(mbr)
      nspace.Types.Add(cls)

      cls = New CodeTypeDeclaration("TestBase")
      mbr = New CodeMemberMethod
      mbr.Name = "Test"
      mbr.Attributes = MemberAttributes.Public
      cls.Members.Add(mbr)

      mbr = New CodeMemberMethod
      mbr.Name = "Test2"
      mbr.Attributes = MemberAttributes.Family
      mbr.Attributes = MemberAttributes.Final
      cls.Members.Add(mbr)
      nspace.Types.Add(cls)

      cls = New CodeTypeDeclaration("TestMustInherit")
      cls.TypeAttributes = Reflection.TypeAttributes.Abstract Or Reflection.TypeAttributes.NotPublic
      mbr = New CodeMemberMethod
      mbr.Name = "Test"
      cls.Members.Add(mbr)
      nspace.Types.Add(cls)

      cls = New CodeTypeDeclaration("TestNotInheritable")
      cls.TypeAttributes = Reflection.TypeAttributes.Sealed Or Reflection.TypeAttributes.NestedFamORAssem
      mbr = New CodeMemberMethod
      mbr.Name = "Test"
      mbr.Attributes = MemberAttributes.Public
      cls.Members.Add(mbr)
      nspace.Types.Add(cls)

      cls = New CodeTypeDeclaration("TestNotPublic")
      cls.TypeAttributes = Reflection.TypeAttributes.NotPublic
      mbr = New CodeMemberMethod
      mbr.Name = "Test"
      mbr.Attributes = MemberAttributes.Public
      cls.Members.Add(mbr)
      nspace.Types.Add(cls)

      cls = New CodeTypeDeclaration("TestNested")
      cls.TypeAttributes = Reflection.TypeAttributes.NestedFamORAssem
      mbr = New CodeMemberMethod
      mbr.Name = "Test"
      mbr.Attributes = MemberAttributes.Public
      cls.Members.Add(mbr)
      nspace.Types.Add(cls)

      cls = New CodeTypeDeclaration("ITest")
      mbr = New CodeMemberMethod
      mbr.Name = "Foo"
      cls.Members.Add(mbr)
      cls.IsInterface = True
      nspace.Types.Add(cls)

      cls = New CodeTypeDeclaration("ITest2")
      cls.IsInterface = True
      mbr = New CodeMemberMethod
      mbr.Name = "Foo"
      cls.Members.Add(mbr)
      nspace.Types.Add(cls)

      cls = New CodeTypeDeclaration("BooleanTest")
      mbr = New CodeMemberMethod
      mbr.Name = "Bar"
      Dim exp As New CodeDom.CodeBinaryOperatorExpression(New CodePrimitiveExpression(1), CodeBinaryOperatorType.BitwiseAnd, New CodePrimitiveExpression(2))
      Dim exp2 As New CodeDom.CodeBinaryOperatorExpression(New CodePrimitiveExpression(3), CodeBinaryOperatorType.BitwiseOr, New CodePrimitiveExpression(4))
      Dim exp3 As New CodeDom.CodeBinaryOperatorExpression(New CodePrimitiveExpression(5), CodeBinaryOperatorType.BooleanAnd, New CodePrimitiveExpression(6))
      Dim exp4 As New CodeDom.CodeBinaryOperatorExpression(New CodePrimitiveExpression(7), CodeBinaryOperatorType.BooleanOr, New CodePrimitiveExpression(8))
      Dim exp5 As New CodeDom.CodeBinaryOperatorExpression(exp, CodeBinaryOperatorType.BooleanAnd, exp2)
      Dim exp6 As New CodeDom.CodeBinaryOperatorExpression(exp3, CodeBinaryOperatorType.BooleanAnd, exp4)
      Dim stmt As New CodeDom.CodeAssignStatement(New CodeDom.CodeVariableReferenceExpression("Sam"), _
                           New CodeDom.CodeBinaryOperatorExpression(exp5, CodeBinaryOperatorType.BooleanAnd, exp6))
      mbr.Statements.Add(New CodeDom.CodeVariableDeclarationStatement(GetType(Int32), "Sam"))
      mbr.Statements.Add(stmt)
      cls.Members.Add(mbr)

      Dim cls2 As New CodeDom.CodeTypeDeclaration

      cls = New CodeTypeDeclaration("Publc")
      cls.TypeAttributes = Reflection.TypeAttributes.NestedFamily
      cls2.Members.Add(cls)
      cls = New CodeTypeDeclaration("Frend")
      cls.TypeAttributes = Reflection.TypeAttributes.Public
      cls2.Members.Add(cls)
      cls = New CodeTypeDeclaration("Privte1")
      cls.TypeAttributes = Reflection.TypeAttributes.NotPublic
      cls2.Members.Add(cls)
      cls = New CodeTypeDeclaration("Privte2")
      cls.TypeAttributes = Reflection.TypeAttributes.VisibilityMask
      cls2.Members.Add(cls)
      cls = New CodeTypeDeclaration("Privte3")
      cls.TypeAttributes = Reflection.TypeAttributes.NestedAssembly
      cls2.Members.Add(cls)
      cls = New CodeTypeDeclaration("Privte4")
      cls.TypeAttributes = Reflection.TypeAttributes.NestedFamORAssem
      cls2.Members.Add(cls)
      cls = New CodeTypeDeclaration("Privte5")
      cls.TypeAttributes = Reflection.TypeAttributes.NestedPrivate
      cls2.Members.Add(cls)
      cls = New CodeTypeDeclaration("Privte6")
      cls.TypeAttributes = Reflection.TypeAttributes.NestedPublic
      cls2.Members.Add(cls)

      nspace.Types.Add(cls2)

   End Sub

   Public Shared Function WriteLineExpression(ByVal value As Object) As CodeDom.CodeStatement
      Dim refExp As New CodeDom.CodeTypeReferenceExpression(GetType(System.Console))
      Dim primExp As New CodeDom.CodePrimitiveExpression(value)
      Return New CodeDom.CodeExpressionStatement(New CodeDom.CodeMethodInvokeExpression(refExp, "WriteLine", primExp))
   End Function

#End Region

#Region "Protected and Friend Methods and Properties - empty"
#End Region

#Region "Private Methods and Properties - empty"
#End Region

End Class