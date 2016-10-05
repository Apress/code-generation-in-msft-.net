// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Provides support for running XSLT templates
//  Refactor: There are too many translation methods

using System;

namespace KADGen.CodeGenerationSupport
{
	public struct Param
	{
		public string Name;
		public object Value;
		public Param( string Name, object value )
		{
			this.Name = Name;
			this.Value = value;
		}
		public Param( string Name )
		{
			this.Name = Name;
			this.Value = null;
		}
	}
	public struct ExtObject
	{
		public string NameSpaceURI;
		public object value;
		public ExtObject( string NameSpaceURI, object value )
		{
			this.NameSpaceURI = NameSpaceURI;
			this.value = value;
		}
		public ExtObject( string NameSpaceURI )
		{
			this.NameSpaceURI = NameSpaceURI;
			this.value = null;
		}
	}
	public class XSLTSupport
	{
		
		#region Class level declarations - empty
		#endregion

		#region constructors - empty
		#endregion

		#region public Methods and Properties
		public static void GenerateViaXSLT( string xsltFileName, string MetaDataFileName, string outputFile, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			xmlDoc.Load( MetaDataFileName );
			KADGen.LibraryInterop.Singletons.XMLDoc = xmlDoc;
			KADGen.LibraryInterop.Singletons.NsMgr = KADGen.Utility.xmlHelpers.BuildNamespacesManagerForDoc( xmlDoc );
			GenerateViaXSLT( xsltFileName, xmlDoc, outputFile, outputType, extObjects, @params );
		}


		public static void GenerateViaXSLT( string xsltFileName, System.Xml.XmlDocument xmlMetaData, string outputFile, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.IO.FileStream stream = null;
			try
			{
				stream = new System.IO.FileStream( outputFile, System.IO.FileMode.Create );
				GenerateXSLTToStream( xsltFileName, xmlMetaData, stream, outputType, extObjects, @params );
			}
			finally
			{
				stream.Close();
			}

		}

		public static System.IO.Stream GenerateViaXSLT( string xsltFileName, System.Xml.XmlDocument xmlMetaData, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.IO.MemoryStream stream;
			stream = new System.IO.MemoryStream();
			GenerateXSLTToStream( xsltFileName, xmlMetaData, stream, outputType, extObjects, @params );
			stream.Flush();
			stream.Seek( 0, System.IO.SeekOrigin.Begin );
			return stream;
		}

		public static System.IO.Stream GenerateViaXSLT( System.Xml.Xsl.XslTransform xslTransform, System.Xml.XPath.XPathNavigator xNavMetaData, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.IO.MemoryStream stream;
			stream = new System.IO.MemoryStream();
			GenerateXSLTToStream( xslTransform, xNavMetaData, stream, outputType, extObjects, @params );
			stream.Flush();
			stream.Seek( 0, System.IO.SeekOrigin.Begin );
			return stream;
		}

		public static System.IO.Stream GenerateViaXSLT( string xsltFileName, string MetaDataFileName, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			xmlDoc.Load( MetaDataFileName );
			KADGen.LibraryInterop.Singletons.XMLDoc = xmlDoc;
			KADGen.LibraryInterop.Singletons.NsMgr = KADGen.Utility.xmlHelpers.BuildNamespacesManagerForDoc( xmlDoc );
			return GenerateViaXSLT( xsltFileName, xmlDoc, outputType, extObjects, @params );
		}

		public static void GenerateXMLViaXSLT( string xsltFileName, System.Xml.XmlDocument xmlMetaData, string outputFile, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.Xml.Xsl.XslTransform xslt = new System.Xml.Xsl.XslTransform();
			System.Xml.XPath.XPathNavigator xNav;
			System.Xml.XmlTextWriter XMLWriter = null;
			System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();

			try
			{
				if( xmlMetaData == null )
				{
					xmlMetaData = new System.Xml.XmlDocument();
				}

				foreach( Param param in @params )
				{
					args.AddParam( param.Name, "", param.Value );
				}

				if( extObjects == null )
				{
					// No problem, just skip
				}
				else
				{
					foreach( ExtObject extObject in extObjects )
					{
						System.Reflection.ConstructorInfo constructorInfo = ((System.Type)(extObject.value)).GetConstructor( null );
						object obj = constructorInfo.Invoke( null );
						args.AddExtensionObject( extObject.NameSpaceURI, obj );
					}
				}
				AddStandardExtension( args );

				xNav = xmlMetaData.CreateNavigator();
				XMLWriter = new System.Xml.XmlTextWriter( outputFile, System.Text.Encoding.UTF8 );

				xslt.Load( xsltFileName );
				xslt.Transform( xNav, args, XMLWriter, null );

			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex );
				throw;

			}
			finally
			{
				if( XMLWriter != null )
				{
					XMLWriter.Flush();
					XMLWriter.Close();
				}
			}

		}

		public static Param[] GetXSLTParams( string xsltFileName, string basePath )
		{
			System.IO.Stream xsltStream = null;
			System.Xml.XmlDocument xmlInput = new System.Xml.XmlDocument();
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			string[] paramNames;
			Param[] @params = null;
			//System.Xml.XmlNode attr;
			//string value;

			try
			{
				xsltStream = Utility.Tools.GetStreamFromStringResource( typeof(XSLTSupport), "RetrieveParams.xslt" );
				xmlInput.Load( xsltFileName );
				GenerateXSLTToStream( xsltStream, xmlInput, stream, "", null );
				stream.Seek( 0, System.IO.SeekOrigin.Begin );
				System.IO.StreamReader reader = new System.IO.StreamReader( stream );
				paramNames = reader.ReadToEnd().Split( '|' );
				if( paramNames.GetLength(0 ) == 1 && paramNames[0].Length == 0 )
				{
					// there are no parameters
				}
				else
				{
					@params = new Param[paramNames.GetLength( 0 )];
					for( int i = 0; i <= paramNames.GetUpperBound(0 ); i++ )
					{
						if( paramNames[i].Trim().Length > 0 )
						{
							@params[i] = new Param( paramNames[i], "");
						}
					}
				}
			}
			finally
			{
				try
				{
					stream.Close();
				}
				catch
				{
				}
				try
				{
					xsltStream.Close();
				}
				catch
				{
				}
			}
			return @params;
		}



		public static void FillParams( Param[] @params, System.Xml.XPath.XPathNavigator nav )
		{
			for( int i=0; i<@params.Length; i++ )
			{
				@params[i].Value = nav.GetAttribute( @params[i].Name, "" );
			}
		}

		#endregion

		#region protected and internal Methods and Properties - empty
		#endregion

		#region private Methods and Properties
		private static void GenerateXSLTToStream( string xsltFileName, System.Xml.XmlDocument xmlMetaData, System.IO.Stream stream, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.Xml.Xsl.XslTransform xslt = new System.Xml.Xsl.XslTransform();
			System.Xml.XPath.XPathNavigator xNav;
			System.IO.StreamWriter streamWriter = new System.IO.StreamWriter( stream );
			System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();

			try
			{
				if( xmlMetaData == null )
				{
					xmlMetaData = new System.Xml.XmlDocument();
				}

				xNav = xmlMetaData.CreateNavigator();

				xslt.Load( xsltFileName );
				GenerateXSLTToStream( xslt, xNav, stream, outputType, extObjects, @params );

			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex );
				throw;

			}
			finally
			{
				if( streamWriter != null )
				{
					streamWriter.Flush();
				}
			}
		}

		private static void GenerateXSLTToStream( System.IO.Stream xsltStream, System.Xml.XmlDocument xmlMetaData, System.IO.Stream stream, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.Xml.Xsl.XslTransform xslt = new System.Xml.Xsl.XslTransform();
			System.Xml.XPath.XPathNavigator xNav;
			System.IO.StreamWriter streamWriter = new System.IO.StreamWriter( stream );
			System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();
			System.Xml.XmlTextReader xmlReader;

			try
			{
				if( xmlMetaData == null )
				{
					xmlMetaData = new System.Xml.XmlDocument();
				}

				xNav = xmlMetaData.CreateNavigator();

				xmlReader = new System.Xml.XmlTextReader( xsltStream );
				xslt.Load( xmlReader, null, new System.Security.Policy.Evidence() );
				GenerateXSLTToStream( xslt, xNav, stream, outputType, extObjects, @params );

			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex );
				throw ex;

			}
			finally
			{
				if( streamWriter != null )
				{
					streamWriter.Flush();
				}
			}
		}

		private static void GenerateXSLTToStream( System.Xml.Xsl.XslTransform xsltTransform, System.Xml.XPath.XPathNavigator xNavMetaData, System.IO.Stream stream, string outputType, ExtObject[] extObjects, params Param[] @params )
		{
			System.IO.StreamWriter streamWriter = null;
			System.Xml.XmlTextWriter xmlWriter = null;
			System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();

			try
			{
				if( @params != null )
				{
					foreach( Param param in @params )
					{
						args.AddParam( param.Name, "", param.Value );
					}
				}

				if( extObjects == null )
				{
					// No problem, just skip
				}
				else
				{
					foreach( ExtObject extObject in extObjects )
					{
						System.Reflection.ConstructorInfo constructorInfo = ((System.Type)(extObject.value)).GetConstructor( Type.EmptyTypes );
						object obj = constructorInfo.Invoke( null );
						args.AddExtensionObject( extObject.NameSpaceURI, obj );
					}
				}
				AddStandardExtension( args );

				streamWriter = new System.IO.StreamWriter( stream );
				xsltTransform.Transform( xNavMetaData, args, streamWriter, null );

			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex );
				throw;

			}
			finally
			{
				if( streamWriter != null )
				{
					streamWriter.Flush();
				}
				if( xmlWriter != null )
				{
					xmlWriter.Flush();
				}
			}
		}


		private static void AddStandardExtension( System.Xml.Xsl.XsltArgumentList args )
		{
			args.AddExtensionObject( "http://kadgen.com/StandardNETSupport.xsd", new XSLTSupport.StandardSupport() );
		}
		#endregion

		private class StandardSupport
		{
			
			private int mIndent = 2;
			private int mIndentSize = 2;

			public void SetIndentSize( int value )
			{
				mIndentSize = value;
			}

			public int GetIndent(   )
			{
				return mIndent;
			}

			public void SetIndent( int value )
			{
				mIndent = value;
			}

			public void Indent(   )
			{
				mIndent += 1;
			}

			public void Outdent(   )
			{
				mIndent -= 1;
			}

			public string InsertIndent(   )
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				int repeatCount = mIndentSize * mIndent;
				if( repeatCount > 0 )
				{
					sb.Append( ' ', mIndentSize * mIndent );
				}
				return sb.ToString();
			}

			public string InsertNLIndent()
			{
				return '\n' + InsertIndent();
			}
		}
	}
}
