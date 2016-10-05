// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Support for CodeDOM templates

using System;
using KADGen;
using System.Diagnostics;

namespace KADGen.CodeGenerationSupport
{
	/// <summary>
	/// 
	/// </summary>
	public class CodeDOMSupport
	{

		#region Class level declarations - empty
		#endregion

		#region Constructors - empty
		#endregion

		#region Public Methods and Properties
		public static System.IO.Stream GenerateViaCodeDOM( Utility.OutputType outputType, System.CodeDom.CodeCompileUnit compileunit )
		{
			System.CodeDom.Compiler.CodeDomProvider provider = null;
			System.CodeDom.Compiler.ICodeGenerator gen;
			System.CodeDom.Compiler.IndentedTextWriter tw = null;
			System.IO.MemoryStream stream;
			System.IO.StreamWriter writer;

			switch( outputType )
			{
				case Utility.OutputType.VB:
					provider = new Microsoft.VisualBasic.VBCodeProvider();
					break;
				case Utility.OutputType.CSharp:
					provider = new Microsoft.CSharp.CSharpCodeProvider();
					break;
			}
			gen = provider.CreateGenerator();

			try
			{
				stream = new System.IO.MemoryStream();
				writer = new System.IO.StreamWriter( stream );
				tw = new System.CodeDom.Compiler.IndentedTextWriter( writer, "   " );
				gen.GenerateCodeFromCompileUnit( compileunit, tw, new System.CodeDom.Compiler.CodeGeneratorOptions() );
				tw.Flush();
				stream.Seek( 0, System.IO.SeekOrigin.Begin );
			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex );
				if( tw != null )
				{
					tw.Flush();
					tw.Close();
				}
				throw;
			}
			return stream;
		}

		public static void GenerateViaCodeDOM(	string outputFileName,
												System.CodeDom.Compiler.CodeDomProvider provider,
												System.CodeDom.CodeCompileUnit compileunit )
		{
			System.CodeDom.Compiler.ICodeGenerator gen = provider.CreateGenerator();
			System.CodeDom.Compiler.IndentedTextWriter tw;

			try
			{
				tw = new System.CodeDom.Compiler.IndentedTextWriter( new System.IO.StreamWriter( outputFileName, false ), "    " );
				gen.GenerateCodeFromCompileUnit( compileunit, tw, new System.CodeDom.Compiler.CodeGeneratorOptions() );
				tw.Flush();
				tw.Close();
			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Debug.WriteLine(ex);
				throw;
			}

		}
		#endregion

		#region Protected and Friend Methods and Properties - empty
		#endregion

		#region Private Methods and Properties - empty
		#endregion

	}
}
