using System;

public struct XSLTParam
{
	public string Name;
	public string Value;
	public XSLTParam( string Name, string Value )
	{
		this.Name = Name;
		this.Value = Value;
	}
}

/// <summary>
/// 
/// </summary>
public class Startup
{
	private const string outputDir = "..\\..\\Test";
	private const string xsltDir = "..\\..";
	private const string  xmlDir = "..\\..";
	private const string  tablename = "Customers";
	// Output file names are hard coded in this simple example
	
	#region Class level declarations
	#endregion

	#region Constructors
	#endregion

	public static void Main()
	{
		// Be sure output directory exists
		if( !System.IO.Directory.Exists( outputDir ) )
		{
			System.IO.Directory.CreateDirectory( outputDir );
		}

		GenerateHelloWorld(outputDir);
		GenerateClassViaBruteForce(outputDir);
		GenerateClassViaCodeDOM(outputDir);
		GenerateClassViaXSLT(outputDir);
	}

	private static void GenerateHelloWorld( string outputDir )
	{
		// Generate Hello World using Brute Force
		HelloWorldViaBruteForce.GenerateOutput( System.IO.Path.Combine( outputDir, "HelloWorldViaBruteForce.cs" ) );

		// Generate Hello World in Visual Basic using the CodeDOM
		System.CodeDom.CodeCompileUnit compileUnit;
		System.CodeDom.Compiler.CodeDomProvider provider;
		compileUnit = HelloWorldViaCodeDOM.BuildGraph();
		provider = new Microsoft.CSharp.CSharpCodeProvider();
		GenerateViaCodeDOM( System.IO.Path.Combine( outputDir, "HelloWorldViaCodeDOM.cs" ), provider, compileUnit );
		// Use same compile unit to generate VB Hello World
		provider = new Microsoft.VisualBasic.VBCodeProvider();
		GenerateViaCodeDOM( System.IO.Path.Combine( outputDir, "HelloWorldViaCodeDOM.vb" ), provider, compileUnit );

		// Generate Hello World via XSLT Template
		GenerateViaXSLT(	System.IO.Path.Combine( xsltDir, "HelloWorld.xslt" ), 
							null, 
							System.IO.Path.Combine( outputDir, "HelloWorldViaXSLT.cs" ) );
	}

	private static void GenerateClassViaBruteForce( string outputDir )
	{
		// Open Metadata file
		System.Xml.XmlDocument xmlMetaData = new System.Xml.XmlDocument();
		xmlMetaData.Load( System.IO.Path.Combine( xmlDir, "Metadata.xml" ) );

		ClassViaBruteForce.GenerateOutput( System.IO.Path.Combine( outputDir, "ClassCustomersViaBruteForce.cs" ), xmlMetaData, "Customers" );
		ClassViaBruteForce.GenerateOutput( System.IO.Path.Combine( outputDir, "ClassOrdersViaBruteForce.cs" ), xmlMetaData, "Orders" );
	}

	private static void GenerateClassViaCodeDOM( string outputDir )
	{
		System.CodeDom.CodeCompileUnit compileUnit;
		System.CodeDom.Compiler.CodeDomProvider provider;

		// Open Metadata file
		System.Xml.XmlDocument xmlMetaData = new System.Xml.XmlDocument();
		xmlMetaData.Load( System.IO.Path.Combine( xmlDir, "Metadata.xml" ) );

		// Generate simple class for Cusotmers in C# using the CodeDOM
		compileUnit = ClassViaCodeDOM.BuildGraph( xmlMetaData, "Customers" );
		provider = new Microsoft.CSharp.CSharpCodeProvider();
		GenerateViaCodeDOM( System.IO.Path.Combine( outputDir, "ClassCustomersViaCodeDOM.cs" ), provider, compileUnit );
		
		// Generate simple class for Orders in C# using the CodeDOM
		compileUnit = ClassViaCodeDOM.BuildGraph( xmlMetaData, "Orders" );
		GenerateViaCodeDOM( System.IO.Path.Combine( outputDir, "ClassOrdersViaCodeDOM.cs" ), provider, compileUnit );
	}

	private static void GenerateClassViaXSLT( string outputDir )
	{
		// Open Metadata file
		System.Xml.XmlDocument xmlMetaData = new System.Xml.XmlDocument();
		xmlMetaData.Load( System.IO.Path.Combine( xmlDir, "Metadata.xml" ) );

		// Generate Hello World via XSLT Template
		GenerateViaXSLT( System.IO.Path.Combine( xsltDir, "Class.xslt" ), xmlMetaData, System.IO.Path.Combine( outputDir, "ClassCustomersViaXSLT.cs" ), new XSLTParam( "TableName", "Customers" ) );
		GenerateViaXSLT( System.IO.Path.Combine( xsltDir, "Class.xslt" ), xmlMetaData, System.IO.Path.Combine( outputDir, "ClassOrdersViaXSLT.cs" ), new XSLTParam( "TableName", "Orders" ) );
	}

	private static void GenerateViaCodeDOM( string outputFileName, System.CodeDom.Compiler.CodeDomProvider provider, System.CodeDom.CodeCompileUnit compileUnit )
	{
		System.CodeDom.Compiler.ICodeGenerator gen = provider.CreateGenerator();
		System.CodeDom.Compiler.IndentedTextWriter tw;

		tw = new System.CodeDom.Compiler.IndentedTextWriter( new System.IO.StreamWriter( outputFileName, false ), "	" );
		System.CodeDom.Compiler.CodeGeneratorOptions options = new System.CodeDom.Compiler.CodeGeneratorOptions();
		options.BracingStyle = "C";
		gen.GenerateCodeFromCompileUnit( compileUnit, tw, options );
		tw.Flush();
		tw.Close();
	}

	private static void GenerateViaXSLT(	string xsltFileName,
											System.Xml.XmlDocument xmlMetaData,
											string outputFile,
											params XSLTParam[] @params )
	{
		System.Xml.Xsl.XslTransform xslt = new System.Xml.Xsl.XslTransform();
		System.Xml.XPath.XPathNavigator xNav;
		System.IO.StreamWriter streamWriter = null;
		System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();
		
		try
		{
			if( xmlMetaData == null )
			{
				xmlMetaData = new System.Xml.XmlDocument();
			}
			foreach( XSLTParam param in @params )
			{
				args.AddParam( param.Name, "", param.Value );
			}

			xNav = xmlMetaData.CreateNavigator();
			streamWriter = new System.IO.StreamWriter( outputFile );

			xslt.Load( xsltFileName );
			xslt.Transform( xNav, args, streamWriter, null );
		}
		finally
		{
			if( streamWriter != null )
			{
				streamWriter.Flush();
				streamWriter.Close();
			}
		}
	}
}