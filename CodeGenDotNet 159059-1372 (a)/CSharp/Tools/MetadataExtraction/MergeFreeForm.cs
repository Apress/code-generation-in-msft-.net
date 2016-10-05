// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Merge metadata.This is an intimate merge as described in Chapter 2 of Code Generation in Microsoft .NET

using System;
using KADGen.Utility;

namespace KADGen.Metadata
{
	public class MergeFreeForm
	{
		public static void Merge( System.Xml.XmlDocument outDoc, string[] fileNames )
		{
			System.Xml.XmlDocument mergeDoc  = new System.Xml.XmlDocument();
			foreach( string filename in fileNames )
			{
				mergeDoc.Load( filename );
				MergeRoot( outDoc, mergeDoc );
			}
		}

		public static void Merge( string baseFileName, string mergeFileName, string outputFileName )
		{
			System.Xml.XmlDocument outDoc = new System.Xml.XmlDocument();
			System.Xml.XmlDocument mergeDoc = new System.Xml.XmlDocument();
			outDoc.Load( baseFileName );
			mergeDoc.Load( mergeFileName );
			MergeRoot( outDoc, mergeDoc );
         Utility.Tools.MakePathIfNeeded(outputFileName);
			outDoc.Save( outputFileName );
		}

		private static void MergeRoot( System.Xml.XmlDocument outDoc, System.Xml.XmlDocument mergeDoc )
		{
			System.Xml.XmlNode rootNode = null;
			System.Xml.XmlNode outRoot = null;
			foreach( System.Xml.XmlNode node in outDoc.ChildNodes )
			{
				if( node.NodeType == System.Xml.XmlNodeType.Element )
				{
					outRoot = node;
				}
			}
			foreach( System.Xml.XmlNode node in mergeDoc.ChildNodes )
			{
				if( node.NodeType == System.Xml.XmlNodeType.Element )
				{
					rootNode = node;
				}
			}
			if( Tools.GetAttributeOrEmpty( rootNode, "FreeForm" ) == "true" )
			{
				// If its a freeform file, regardless of the root element name, 
				// attempt to merge all child nodes of root
				foreach( System.Xml.XmlNode node in rootNode.ChildNodes )
				{
					MergeNode( outRoot, node, null );
				}
			}
			else
			{
				MergeNode( outRoot, rootNode, null );
			}
		}

		private static void MergeNode( System.Xml.XmlNode outParent, System.Xml.XmlNode node, System.Xml.XmlAttribute nameAttrib )
		{
			System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager( outParent.OwnerDocument.NameTable );
			string predicate = "";
			nsmgr.AddNamespace( node.Prefix, node.NamespaceURI );
			if( nameAttrib != null )
			{
				predicate = "[@Name='" + nameAttrib.Value + "']";
			}
			System.Xml.XmlNode testNode = outParent.SelectSingleNode( node.Name + predicate, nsmgr );
			if( testNode == null )
			{
				AddChild( outParent, node );
			}
			else
			{
				if( Utility.Tools.GetAttributeOrEmpty( node, "MergeRemoveExisting").ToLower() == "true" )
				{
					testNode.ParentNode.RemoveChild( testNode );
				}
				else if( Utility.Tools.GetAttributeOrEmpty(node, "MergeReplaceExisting").ToLower() == "true" )
				{
					testNode.ParentNode.RemoveChild( testNode );
					AddChild( outParent, node );
				}
				else
				{
					// Node exists, add attributes, then children
					foreach( System.Xml.XmlAttribute attrib in node.Attributes )
					{
						if( testNode.Attributes[attrib.Name] == null )
						{
							testNode.Attributes.Append( xmlHelpers.NewAttribute( testNode.OwnerDocument, attrib.Name, attrib.Value ) );
						}
						else
						{
							testNode.Attributes[attrib.Name].Value = attrib.Value;
						}
					}
					foreach( System.Xml.XmlNode childNode in node.ChildNodes )
					{
						MergeNode( testNode, childNode, childNode.Attributes["Name"] );
					}
				}
			}
		}

		private static void AddChild( System.Xml.XmlNode outParent , System.Xml.XmlNode node )
		{
			outParent.AppendChild( outParent.OwnerDocument.ImportNode( node, true ) );
		}
	}
}
