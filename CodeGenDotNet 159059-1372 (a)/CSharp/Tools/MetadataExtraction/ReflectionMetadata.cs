// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Extracts metadata from classes.
//  NOTE: This is prelimiary code with very little testing

using System;
using System.Diagnostics;

namespace KADGen.Metadata
{
	/// <summary>
	/// Summary description for ReflectionMetadata.
	/// </summary>
	public class ReflectionMetadata
	{
		public static System.Xml.XmlNode AssemblyMetadata(	string assemblyFileName, 
															System.Xml.XmlNode nodeParent )
		{
			System.Reflection.Assembly assm;
			assm = System.Reflection.Assembly.LoadFile( assemblyFileName );
			return AssemblyMetadata( assm, nodeParent );
		}

		public static System.Xml.XmlNode AssemblyMetadata(	System.Reflection.Assembly assm,
			System.Xml.XmlNode nodeParent )
		{
			System.Xml.XmlNode node = Utility.xmlHelpers.NewElement( nodeParent.OwnerDocument, "Assembly", assm.FullName );
			foreach( System.Type type in assm.GetTypes() )
			{
				node.AppendChild( MakeTypeNode( type, nodeParent.OwnerDocument ) );
			}
			return node;
		}

		private static System.Xml.XmlNode MakeTypeNode( System.Type type, System.Xml.XmlDocument ownerdoc )
		{
			System.Xml.XmlNode nodeCategory;
			System.Xml.XmlNode nodeType;
			nodeType = Utility.xmlHelpers.NewElement( ownerdoc, "Type", type.Name );
			nodeCategory = nodeType.AppendChild( Utility.xmlHelpers.NewElement( ownerdoc, "Category", "Constructors" ) );
			foreach( System.Reflection.ConstructorInfo meminfo in type.GetConstructors() )
			{
				Utility.xmlHelpers.AppendIfExists( nodeCategory, MakeMethodNode( meminfo, ownerdoc, false ) );
			}
			nodeCategory = nodeType.AppendChild( Utility.xmlHelpers.NewElement( ownerdoc, "Category", "Methods" ) );
			foreach( System.Reflection.MethodInfo meminfo in type.GetMethods() )
			{
				Utility.xmlHelpers.AppendIfExists( nodeCategory, MakeMethodNode( meminfo, ownerdoc, true ) );
			}
			nodeCategory = nodeType.AppendChild( Utility.xmlHelpers.NewElement( ownerdoc, "Category", "Properties" ) );
			foreach( System.Reflection.PropertyInfo meminfo in type.GetProperties() )
			{
				Utility.xmlHelpers.AppendIfExists( nodeCategory, MakePropertyNode( meminfo, ownerdoc ) );
			}
			nodeCategory = nodeType.AppendChild( Utility.xmlHelpers.NewElement( ownerdoc, "Category", "NestedTypes" ) );
			foreach( System.Type innerType in type.GetNestedTypes() )
			{
				Utility.xmlHelpers.AppendIfExists( nodeCategory, MakeTypeNode( innerType, ownerdoc ) );
			}
			nodeCategory = nodeType.AppendChild( Utility.xmlHelpers.NewElement( ownerdoc, "Category", "Fields" ) );
			foreach( System.Reflection.FieldInfo memberInfo in type.GetFields() )
			{
				Utility.xmlHelpers.AppendIfExists( nodeCategory, MakeFieldNode( memberInfo, ownerdoc ) );
			}
			nodeCategory = nodeType.AppendChild(Utility.xmlHelpers.NewElement( ownerdoc, "Category", "Events" ) );
			foreach( System.Type innerType in type.GetNestedTypes() )
			{
				Utility.xmlHelpers.AppendIfExists( nodeCategory, MakeTypeNode( innerType, ownerdoc ) );
			}
			return nodeType;
		}

		private static System.Xml.XmlNode MakeMethodNode(	System.Reflection.MethodBase memberInfo,
															System.Xml.XmlDocument ownerdoc,
															bool SkipSpecialName )
		{
			System.Xml.XmlNode nodeMember;
			if( !memberInfo.IsPrivate && !( memberInfo.IsSpecialName && SkipSpecialName ) )
			{
				nodeMember = Utility.xmlHelpers.NewElement( ownerdoc, memberInfo.MemberType.ToString(), memberInfo.Name );
				nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerdoc, "MustInherit", memberInfo.IsAbstract ) );
				nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerdoc, "NotOverridable", memberInfo.IsFinal ) );
				nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerdoc, "ShadowOverload", memberInfo.IsHideBySig ) );
				nodeMember.Attributes.Append( Utility.xmlHelpers.NewAttribute( ownerdoc, "Scope", 
					GetScope( memberInfo.IsPrivate, memberInfo.IsPublic, memberInfo.IsAssembly, memberInfo.IsFamily, memberInfo.IsFamilyOrAssembly ) ) );
				nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerdoc, "Shared", memberInfo.IsStatic ) );
				nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerdoc, "Overridable", memberInfo.IsVirtual ) );
				if( memberInfo is System.Reflection.MethodInfo )
				{
					nodeMember.Attributes.Append( Utility.xmlHelpers.NewAttribute( ownerdoc, "ReturnType", ((System.Reflection.MethodInfo)memberInfo).ReturnType.ToString() ) );
				}
				MakeParameterNodes( nodeMember, memberInfo.GetParameters() );
				return nodeMember;
			}
			return null;
		}

		private static System.Xml.XmlNode MakePropertyNode(	System.Reflection.PropertyInfo memberInfo,
															System.Xml.XmlDocument ownerdoc )
		{
			System.Xml.XmlNode nodeMember;
			nodeMember = Utility.xmlHelpers.NewElement( ownerdoc, "Property", memberInfo.Name );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewAttribute( ownerdoc, "Type", memberInfo.PropertyType.ToString() ) );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerdoc, "CanRead", memberInfo.CanRead ) );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerdoc, "CanWrite", memberInfo.CanWrite ) );
			MakeParameterNodes( nodeMember, memberInfo.GetIndexParameters() );
			if( memberInfo.GetGetMethod() != null )
			{
				nodeMember.AppendChild( MakeMethodNode( memberInfo.GetGetMethod(), ownerdoc, false ) );
			}
			if( memberInfo.GetSetMethod() != null )
			{
				nodeMember.AppendChild( MakeMethodNode( memberInfo.GetSetMethod(), ownerdoc, false ) );
			}
			return nodeMember;
		}

		private static System.Xml.XmlNode MakeFieldNode(	System.Reflection.FieldInfo fieldInfo,
															System.Xml.XmlDocument ownerDoc )
		{
			System.Xml.XmlNode nodeMember;
			nodeMember = Utility.xmlHelpers.NewElement( ownerDoc, "Property", fieldInfo.Name );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewAttribute( ownerDoc, "Type", fieldInfo.FieldType.ToString() ) );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewAttribute( ownerDoc, "Scope", GetScope( fieldInfo.IsPrivate, fieldInfo.IsPublic, fieldInfo.IsAssembly, fieldInfo.IsFamily, fieldInfo.IsFamilyOrAssembly ) ) );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerDoc, "ReadOnly", fieldInfo.IsInitOnly ) );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerDoc, "Constant", fieldInfo.IsLiteral ) );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerDoc, "Shared", fieldInfo.IsStatic ) );
			return nodeMember;
		}

		private static System.Xml.XmlNode MakeEventNode( System.Reflection.EventInfo eventInfo, System.Xml.XmlDocument ownerDoc )
		{
			System.Xml.XmlNode nodeMember;
			nodeMember = Utility.xmlHelpers.NewElement( ownerDoc, "Property", eventInfo.Name );
			nodeMember.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerDoc, "Mulitcast", eventInfo.IsMulticast ) );
			return nodeMember;
		}

		private static void MakeParameterNodes( System.Xml.XmlNode nodeParent, System.Reflection.ParameterInfo[] @params )
		{
			System.Xml.XmlNode nodeParam;
			System.Xml.XmlDocument ownerDoc = nodeParent.OwnerDocument;
			foreach( System.Reflection.ParameterInfo param in @params )
			{
				nodeParam = nodeParent.AppendChild( Utility.xmlHelpers.NewElement( ownerDoc, "Param", param.Name ) );
				nodeParam.Attributes.Append( Utility.xmlHelpers.NewAttribute( ownerDoc, "Type", param.ParameterType.ToString() ) );
				nodeParam.Attributes.Append( Utility.xmlHelpers.NewAttribute( ownerDoc, "Default", param.DefaultValue.ToString() ) );
				nodeParam.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerDoc, "Optional", param.IsOptional ) );
				nodeParam.Attributes.Append( Utility.xmlHelpers.NewAttribute( ownerDoc, "Position", param.Position.ToString() ) );
				nodeParam.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( ownerDoc, "ReturnValue", param.IsRetval ) );
			}
		}


		private static string GetScope(	bool isPrivate,
										bool isPublic,
										bool isAssembly,
										bool isFamily,
										bool isFamilyOrAssembly )
		{
			if( isPrivate )
			{
				return "Private";
			}
			else if( isPublic )
			{
				return "Public";
			}
			else if( isAssembly )
			{
				return "Friend";
			}
			else if( isFamily )
			{
				return "Protected";
			}
			else if( isFamilyOrAssembly )
			{
				return "ProtectedFriend";
			}
			else
			{
				return null;
			}

		}
	}
}
