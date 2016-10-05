// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Tools to facilitate working with XML. 
//  Note: Additional tools are in the Tools class

using System;

namespace KADGen.Utility
{
	/// <summary>
	/// Summary description for xmlHelpers.
	/// </summary>
	public class xmlHelpers
	{
		public class nspaceLookup
		{
			public string nspace;
			public string prefix;
		}
		

		#region Class level declarations - empty
		#endregion

		#region constructors - empty
		#endregion

		#region public Methods and Properties

		public static System.Xml.XmlNamespaceManager BuildNamespacesManagerForDoc( System.Xml.XmlDocument xmlDoc )
		{
			return BuildNamespacesManagerForDoc( xmlDoc, "zzz" );
		}
		public static System.Xml.XmlNamespaceManager BuildNamespacesManagerForDoc( System.Xml.XmlDocument xmlDoc, string defaultNamespacePrefix )
		{
			System.Xml.XmlNodeList nodes;
			System.Xml.XmlNamespaceManager nsmgrXML = new System.Xml.XmlNamespaceManager( xmlDoc.NameTable );
			nspaceLookup nSpace;
			System.Collections.Hashtable collection = new System.Collections.Hashtable();
			System.Diagnostics.Debug.WriteLine( DateTime.Now );
			nodes = xmlDoc.SelectNodes( "//*" );
			foreach( System.Xml.XmlNode node in nodes )
			{
				nSpace = new nspaceLookup();
				nSpace.nspace = node.NamespaceURI;
				nSpace.prefix = node.Prefix;
				if( !collection.Contains(nSpace.prefix + ":" + nSpace.nspace ) )
				{
					collection.Add( nSpace.prefix + ":" + nSpace.nspace, nSpace );
				}
			}
			System.Diagnostics.Debug.WriteLine( DateTime.Now );

			foreach( System.Collections.DictionaryEntry d in collection )
			{
				nSpace = ((nspaceLookup)(d.Value));
				if( nSpace.prefix.Trim().Length == 0 )
				{
					nSpace.prefix = defaultNamespacePrefix;
				}
				nsmgrXML.AddNamespace( nSpace.prefix, nSpace.nspace );
			}
			nsmgrXML.AddNamespace( "kg", "http://kadgen.com/KADGenDriving.xsd" );

			foreach( string prefix in nsmgrXML )
			{
				Console.WriteLine( "Prefix={0}, Namespace={1}", prefix, nsmgrXML.LookupNamespace( prefix ) );
			}

			return nsmgrXML;


		}


		public static System.Xml.XmlElement NewElement( System.Xml.XmlDocument xmlDoc, string elementName, string name )
		{
			System.Xml.XmlElement nodeElement;
			nodeElement = xmlDoc.CreateElement( elementName );
			nodeElement.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "Name", name ));
			return nodeElement;
		}

		public static System.Xml.XmlElement NewElement( System.Xml.XmlDocument xmlDoc, string elementName )
		{
			System.Xml.XmlElement nodeElement;
			nodeElement = xmlDoc.CreateElement( elementName );
			return nodeElement;
		}

		public static System.Xml.XmlElement NewElement( string nSpace, System.Xml.XmlDocument xmlDoc, string elementName )
		{
			System.Xml.XmlElement nodeElement;
			nodeElement = xmlDoc.CreateElement( elementName, nSpace );
			return nodeElement;
		}

		public static System.Xml.XmlElement NewElement( string nSpace, System.Xml.XmlDocument xmlDoc, string elementName, string name )
		{
			System.Xml.XmlElement nodeElement;
			nodeElement = xmlDoc.CreateElement( elementName, nSpace );
			nodeElement.Attributes.Append( xmlHelpers.NewAttribute( xmlDoc, "Name", name ));
			return nodeElement;
		}

		public static System.Xml.XmlElement NewElement( string prefix, string nSpace, System.Xml.XmlDocument xmlDoc, string elementName )
		{
			System.Xml.XmlElement nodeElement;
			nodeElement = xmlDoc.CreateElement( prefix, elementName, nSpace );
			return nodeElement;
		}

		public static System.Xml.XmlAttribute NewAttribute( System.Xml.XmlDocument xmlDoc, string name, string value )
		{
			System.Xml.XmlAttribute nodeAttribute;
			nodeAttribute = xmlDoc.CreateAttribute( name );
			nodeAttribute.Value = value;
			return nodeAttribute;
		}

		public static System.Xml.XmlAttribute NewBoolAttribute( System.Xml.XmlDocument xmlDoc, string name, bool value )
		{
			System.Xml.XmlAttribute nodeAttribute;
			nodeAttribute = xmlDoc.CreateAttribute( name );
			if( value )
			{
				nodeAttribute.Value = "true";
			}
			else
			{
				nodeAttribute.Value = "false";
			}
			return nodeAttribute;
		}

		public static void AppendParts( System.Xml.XmlDocument xmlOut, System.Xml.XmlNode node, string name, string partString )
		{
			string[] parts;

			parts = partString.Split( ',' );
			foreach( string s in parts )
			{
				node.AppendChild( xmlHelpers.NewElement(xmlOut, name, s ));
			}
		}

		public static void AppendIfExists( System.Xml.XmlNode node, System.Xml.XmlNode nodeChild )
		{
			if( nodeChild != null )
			{
				node.AppendChild( nodeChild );
			}
		}

		public static System.Xml.XmlNode GetSchemaForNode( string nodeName, System.Xml.XmlDocument xsdDoc )
		{
			if( xsdDoc != null )
			{
				System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager( new System.Xml.NameTable() );
				namespaceManager.AddNamespace( "xs", "http://www.w3.org/2001/XMLSchema" );
				return xsdDoc.SelectSingleNode( "//xs:element[@name='" + nodeName + "']", namespaceManager );
			}
			return null;
		}

		public static System.Xml.XmlDocument LoadFile( string xmlFileName, System.Xml.XmlDocument xsdDoc )
		{
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			System.Xml.XmlTextReader textReader = null;
			System.Xml.XmlValidatingReader validReader;
			xsdDoc = null;
			try
			{
				xmlFileName = xmlFileName;
				textReader = new System.Xml.XmlTextReader( xmlFileName );
				validReader = new System.Xml.XmlValidatingReader( textReader );
				validReader.ValidationType = System.Xml.ValidationType.Schema;
				xmlDoc.Load( validReader );
			}
			finally
			{
				textReader.Close();
			}
			return xmlDoc;
		}

		#endregion

		#region protected and internal Methods and Properties - empty
		#endregion

		#region private Methods and Properties - empty
		#endregion

	}
}
