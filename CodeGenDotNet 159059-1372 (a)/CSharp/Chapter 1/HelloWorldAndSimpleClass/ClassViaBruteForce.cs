using System;

/// <summary>
/// 
/// </summary>
public class ClassViaBruteForce
{
	#region Public Methods and Properties
	public static void GenerateOutput(	string outputFile, 
										System.Xml.XmlDocument xmlMetaData,
										string tableName)
	{
		System.CodeDom.Compiler.IndentedTextWriter writer = null;
		//System.Xml.XmlNode node;
		System.Xml.XmlNodeList nodeList;
		try
		{
			writer = new System.CodeDom.Compiler.IndentedTextWriter( new System.IO.StreamWriter( outputFile ) );
			nodeList = xmlMetaData.SelectNodes( "/DataSet/Table[@Name='" + tableName + "']/Column" );

			writer.WriteLine( "using System;" );
			writer.WriteLine( "" );
			writer.WriteLine( "/// <summary>" );
			writer.WriteLine( "/// " );
			writer.WriteLine( "/// </summar>" );
			writer.WriteLine( "public class TargetOutput" );
			writer.WriteLine( "{" );
			writer.Indent ++;
			writer.WriteLine( "#region Class level declarations" );
			foreach( System.Xml.XmlNode node in nodeList )
			{
				writer.WriteLine( "private " + node.Attributes["Type"].Value + " m_" + node.Attributes["Name"].Value + ";" );
			}
			writer.WriteLine( "#endregion" );
			writer.WriteLine( "" );
			writer.WriteLine( "#region Public Methods and Properties" );
			foreach( System.Xml.XmlNode node in nodeList )
			{
				writer.WriteLine( "public " + node.Attributes["Type"].Value + " " + node.Attributes["Name"].Value );
				writer.WriteLine( "{" );
				writer.Indent ++;
				writer.WriteLine( "get" );
				writer.WriteLine( "{" );
				writer.Indent ++;
				writer.WriteLine( "return m_" + node.Attributes["Name"].Value + ";" );
				writer.Indent --;
				writer.WriteLine( "}" );
				writer.WriteLine( "set" );
				writer.WriteLine( "{" );
				writer.Indent ++;
				writer.WriteLine( "m_" + node.Attributes["Name"].Value + " = value;" );
				writer.Indent --;
				writer.WriteLine( "}" );
				writer.Indent --;
				writer.WriteLine( "}" );
				writer.WriteLine( "" );
			}
			writer.WriteLine( "#endregion" );
			writer.WriteLine( "" );
			writer.Indent --;
			writer.WriteLine( "}" );
		}
		finally
		{
			if( writer != null )
			{
				writer.Flush();
				writer.Close();
			}
		}
	}
	#endregion
}