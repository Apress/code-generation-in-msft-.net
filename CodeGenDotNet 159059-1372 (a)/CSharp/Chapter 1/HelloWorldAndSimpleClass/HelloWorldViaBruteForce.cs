using System;

/// <summary>
/// Create Hello World program using direct stream output
/// </summary>
public class HelloWorldViaBruteForce
{
	#region Public Methods and Properties
	public static void GenerateOutput( string outputFileName )
	{
		System.IO.StreamWriter writer = null;
		try
		{
			writer = new System.IO.StreamWriter( outputFileName );
			writer.WriteLine( "using System;" );
			writer.WriteLine( "" );
			writer.WriteLine( "/// <summary>" );
			writer.WriteLine( "/// Hello World target output" );
			writer.WriteLine( "/// </summary>" );
			writer.WriteLine( "public class TargetHelloWorld" );
			writer.WriteLine( "{" );
			writer.WriteLine( "	#region Public Methods and Properties" );
			writer.WriteLine( "	public static void Main()" );
			writer.WriteLine( "	{" );
			writer.WriteLine( "		Console.WriteLine( \"Hello World\" );" );
			writer.WriteLine( "	}" );
			writer.WriteLine( "	#endregion" );
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
