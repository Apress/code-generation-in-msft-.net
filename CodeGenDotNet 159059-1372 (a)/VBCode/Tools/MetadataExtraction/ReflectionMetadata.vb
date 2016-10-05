' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Extracts metadata from classes.
'  NOTE: This is prelimiary code with very little testing

Option Explicit On 
Option Strict On

Imports System
Imports System.Diagnostics

Public Class ReflectionMetadata
   Public Shared Function AssemblyMetadata( _
                  ByVal assemblyFileName As String, _
                  ByVal nodeParent As Xml.XmlNode) _
                  As Xml.XmlNode
      Dim assm As Reflection.Assembly
      assm.LoadFile(assemblyFileName)
      Return AssemblyMetadata(assm, nodeParent)
   End Function

   Public Shared Function AssemblyMetadata( _
                  ByVal assm As Reflection.Assembly, _
                  ByVal nodeParent As Xml.XmlNode) _
                  As Xml.XmlNode
      Dim node As Xml.XmlNode = Utility.xmlHelpers.NewElement(nodeParent.OwnerDocument, "Assembly", assm.FullName)
      For Each type As System.Type In assm.GetTypes
         node.AppendChild(MakeTypeNode(type, nodeParent.OwnerDocument))
      Next
      Return node
   End Function

   Private Shared Function MakeTypeNode(ByVal type As System.Type, ByVal ownerdoc As Xml.XmlDocument) As Xml.XmlNode
      Dim nodeCategory As Xml.XmlNode
      Dim nodeType As Xml.XmlNode
      nodeType = Utility.xmlHelpers.NewElement(ownerdoc, "Type", type.Name)
      nodeCategory = nodeType.AppendChild(Utility.xmlHelpers.NewElement(ownerdoc, "Category", "Constructors"))
      For Each meminfo As Reflection.ConstructorInfo In type.GetConstructors
         Utility.xmlHelpers.AppendIfExists(nodeCategory, MakeMethodNode(meminfo, ownerdoc, False))
      Next
      nodeCategory = nodeType.AppendChild(Utility.xmlHelpers.NewElement(ownerdoc, "Category", "Methods"))
      For Each meminfo As Reflection.MethodInfo In type.GetMethods
         Utility.xmlHelpers.AppendIfExists(nodeCategory, MakeMethodNode(meminfo, ownerdoc, True))
      Next
      nodeCategory = nodeType.AppendChild(Utility.xmlHelpers.NewElement(ownerdoc, "Category", "Properties"))
      For Each meminfo As Reflection.PropertyInfo In type.GetProperties
         Utility.xmlHelpers.AppendIfExists(nodeCategory, MakePropertyNode(meminfo, ownerdoc))
      Next
      nodeCategory = nodeType.AppendChild(Utility.xmlHelpers.NewElement(ownerdoc, "Category", "NestedTypes"))
      For Each innerType As System.Type In type.GetNestedTypes
         Utility.xmlHelpers.AppendIfExists(nodeCategory, MakeTypeNode(innerType, ownerdoc))
      Next
      nodeCategory = nodeType.AppendChild(Utility.xmlHelpers.NewElement(ownerdoc, "Category", "Fields"))
      For Each memberInfo As Reflection.FieldInfo In type.GetFields
         Utility.xmlHelpers.AppendIfExists(nodeCategory, MakeFieldNode(memberInfo, ownerdoc))
      Next
      nodeCategory = nodeType.AppendChild(Utility.xmlHelpers.NewElement(ownerdoc, "Category", "Events"))
      For Each innerType As System.Type In type.GetNestedTypes
         Utility.xmlHelpers.AppendIfExists(nodeCategory, MakeTypeNode(innerType, ownerdoc))
      Next
      Return nodeType
   End Function

   Private Shared Function MakeMethodNode( _
                  ByVal memberInfo As Reflection.MethodBase, _
                  ByVal ownerdoc As Xml.XmlDocument, _
                  ByVal SkipSpecialName As Boolean) _
                  As Xml.XmlNode
      Dim nodeMember As Xml.XmlNode
      If Not memberInfo.IsPrivate And Not (memberInfo.IsSpecialName And SkipSpecialName) Then
         nodeMember = Utility.xmlHelpers.NewElement(ownerdoc, memberInfo.MemberType.ToString, memberInfo.Name)
         nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerdoc, "MustInherit", memberInfo.IsAbstract))
         nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerdoc, "NotOverridable", memberInfo.IsFinal))
         nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerdoc, "ShadowOverload", memberInfo.IsHideBySig))
         nodeMember.Attributes.Append(Utility.xmlHelpers.NewAttribute(ownerdoc, "Scope", _
                     GetScope(memberInfo.IsPrivate, memberInfo.IsPublic, memberInfo.IsAssembly, memberInfo.IsFamily, memberInfo.IsFamilyOrAssembly)))
         nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerdoc, "Shared", memberInfo.IsStatic))
         nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerdoc, "Overridable", memberInfo.IsVirtual))
         If TypeOf memberInfo Is Reflection.MethodInfo Then
            nodeMember.Attributes.Append(Utility.xmlHelpers.NewAttribute(ownerdoc, "ReturnType", CType(memberInfo, Reflection.MethodInfo).ReturnType.ToString))
         End If
         MakeParameterNodes(nodeMember, memberInfo.GetParameters())
         Return nodeMember
      End If
   End Function

   Private Shared Function MakePropertyNode( _
                  ByVal memberInfo As Reflection.PropertyInfo, _
                  ByVal ownerdoc As Xml.XmlDocument) _
                  As Xml.XmlNode
      Dim nodeMember As Xml.XmlNode
      nodeMember = Utility.xmlHelpers.NewElement(ownerdoc, "Property", memberInfo.Name)
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewAttribute(ownerdoc, "Type", memberInfo.PropertyType.ToString))
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerdoc, "CanRead", memberInfo.CanRead))
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerdoc, "CanWrite", memberInfo.CanWrite))
      MakeParameterNodes(nodeMember, memberInfo.GetIndexParameters())
      If Not memberInfo.GetGetMethod Is Nothing Then
         nodeMember.AppendChild(MakeMethodNode(memberInfo.GetGetMethod, ownerdoc, False))
      End If
      If Not memberInfo.GetSetMethod Is Nothing Then
         nodeMember.AppendChild(MakeMethodNode(memberInfo.GetSetMethod, ownerdoc, False))
      End If
      Return nodeMember
   End Function

   Private Shared Function MakeFieldNode( _
                  ByVal fieldInfo As Reflection.FieldInfo, _
                  ByVal ownerDoc As Xml.XmlDocument) _
                  As Xml.XmlNode
      Dim nodeMember As Xml.XmlNode
      nodeMember = Utility.xmlHelpers.NewElement(ownerDoc, "Property", fieldInfo.Name)
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewAttribute(ownerDoc, "Type", fieldInfo.FieldType.ToString))
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewAttribute(ownerDoc, "Scope", _
                     GetScope(fieldInfo.IsPrivate, fieldInfo.IsPublic, fieldInfo.IsAssembly, fieldInfo.IsFamily, fieldInfo.IsFamilyOrAssembly)))
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerDoc, "ReadOnly", fieldInfo.IsInitOnly))
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerDoc, "Constant", fieldInfo.IsLiteral))
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerDoc, "Shared", fieldInfo.IsStatic))
      Return nodeMember
   End Function

   Private Shared Function MakeEventNode( _
                  ByVal eventInfo As Reflection.EventInfo, _
                  ByVal ownerDoc As Xml.XmlDocument) _
                  As Xml.XmlNode
      Dim nodeMember As Xml.XmlNode
      nodeMember = Utility.xmlHelpers.NewElement(ownerDoc, "Property", eventInfo.Name)
      nodeMember.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerDoc, "Mulitcast", eventInfo.IsMulticast))
      Return nodeMember
   End Function

   Private Shared Sub MakeParameterNodes(ByVal nodeParent As Xml.XmlNode, ByVal params() As Reflection.ParameterInfo)
      Dim nodeParam As Xml.XmlNode
      Dim ownerDoc As Xml.XmlDocument = nodeParent.OwnerDocument
      For Each param As Reflection.ParameterInfo In params
         nodeParam = nodeParent.AppendChild(Utility.xmlHelpers.NewElement(ownerDoc, "Param", param.Name))
         nodeParam.Attributes.Append(Utility.xmlHelpers.NewAttribute(ownerDoc, "Type", param.ParameterType.ToString))
         nodeParam.Attributes.Append(Utility.xmlHelpers.NewAttribute(ownerDoc, "Default", param.DefaultValue.ToString))
         nodeParam.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerDoc, "Optional", param.IsOptional))
         nodeParam.Attributes.Append(Utility.xmlHelpers.NewAttribute(ownerDoc, "Position", param.Position.ToString))
         nodeParam.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute(ownerDoc, "ReturnValue", param.IsRetval))
      Next
   End Sub


   Private Shared Function GetScope( _
                  ByVal isPrivate As Boolean, _
                  ByVal isPublic As Boolean, _
                  ByVal isAssembly As Boolean, _
                  ByVal isFamily As Boolean, _
                  ByVal isFamilyOrAssembly As Boolean) _
                  As String
      If isPrivate Then
         Return "Private"
      ElseIf isPublic Then
         Return "Public"
      ElseIf isAssembly Then
         Return "Friend"
      ElseIf isFamily Then
         Return "Protected"
      ElseIf isFamilyOrAssembly Then
         Return "ProtectedFriend"
      End If

   End Function

End Class
