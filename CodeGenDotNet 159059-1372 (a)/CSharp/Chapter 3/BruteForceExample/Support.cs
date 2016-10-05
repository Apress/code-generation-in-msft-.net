using System;

namespace KADGen.BruteForceExample
{
	/// <summary>
	/// Simple Utility files supporting code generation
	/// </summary>
	public class Support
	{
		public const string DQ = "\"";

		public static void FileOpen(	System.CodeDom.Compiler.IndentedTextWriter inWriter,
										string import,
										string fileName,
										string genDateTime )
		{
			inWriter.WriteLine( "using System;" );
			foreach( string s in import.Split( ',' ) )
			{
				inWriter.WriteLine( "using " + s.Trim() + ";" );
			}
			
			inWriter.WriteLine( "/// <summary>" );
			inWriter.WriteLine( "/// " + System.IO.Path.GetFileName( fileName ) );
			inWriter.WriteLine( "/// Last Generated on Date: " + genDateTime );
			inWriter.WriteLine( "/// </summary>" );
		}

		public static void MakeRegion( System.CodeDom.Compiler.IndentedTextWriter inWriter, string regionName )
		{
			inWriter.WriteLine( "#region " + regionName );
		}

		public static void EndRegion( System.CodeDom.Compiler.IndentedTextWriter inWriter )
		{
			inWriter.WriteLine( "#endregion" );
		}

		public static void WriteLineAndIndent( System.CodeDom.Compiler.IndentedTextWriter inWriter, string text )
		{
			inWriter.WriteLine( text );
			inWriter.Indent ++;
		}

		public static void WriteLineAndOutdent( System.CodeDom.Compiler.IndentedTextWriter inWriter, string text )
		{
			inWriter.Indent --;
			inWriter.WriteLine( text );
		}
	}
}