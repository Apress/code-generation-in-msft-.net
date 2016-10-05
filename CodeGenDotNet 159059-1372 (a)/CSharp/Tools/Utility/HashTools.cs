// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Tools for working with hash codes
//  Refactor: When a standard for hashing becomes available - swtich to it!


using System;

namespace KADGen.Utility
{
	public enum FileChanged
	{
		Unknown,
		FileDoesntExist,
		NotChanged,
		Changed,
		WhitespaceOnly
	}
	public class HashTools
	{
		
		private static string mHashMarker;
		private static string mHeaderMarker;

		public static FileChanged IsFileChanged( string fileName, string commentStart, string commentEnd, string headerMarker, string hashMarker )
		{
			System.IO.FileStream inStream;
			System.IO.StreamReader reader = null;
			System.IO.StreamWriter writer = null;

			try
			{
				if( !System.IO.File.Exists(fileName ) )
				{
					return FileChanged.FileDoesntExist;
				}
				else
				{
					inStream = new System.IO.FileStream( fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read );
					reader = new System.IO.StreamReader( inStream );
					writer = new System.IO.StreamWriter( new System.IO.MemoryStream() );
					return IsTextChanged( reader.ReadToEnd(), commentStart, commentEnd, headerMarker, hashMarker );
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				try
				{
					reader.Close();
					writer.Close();
				}
				catch
				{
				}
			}
		}

		public static FileChanged IsTextChanged( string s, string commentStart, string commentEnd, string headerMarker, string hashMarker )
		{
			string oldHashCode;
			string newHashCode;
			string fullHeaderMarker = commentStart + headerMarker + commentEnd;

			oldHashCode = GetHash( s, commentStart, commentEnd, hashMarker );
			if( oldHashCode.Length == 0 )
			{
				return FileChanged.Unknown;
			}
			else
			{
				s = StripHeader( s, fullHeaderMarker );
				newHashCode = CreateHash( s );

				if( oldHashCode == newHashCode.ToString() )
				{
					return FileChanged.NotChanged;
				}
				else
				{
					return FileChanged.Changed;
					//filechanged.whitspaceonly not yet supported
				}
			}
		}

		public static void ApplyHash( string fileName, string commentText, string commentStart, string commentEnd, string headerMarker, string hashMarker )
		{
			System.IO.FileStream inStream = new System.IO.FileStream( fileName, System.IO.FileMode.Open );
			System.IO.Stream outStream = ApplyHash( inStream, commentText, commentStart, commentEnd, headerMarker, hashMarker );
			inStream.Close();
			inStream = new System.IO.FileStream( fileName, System.IO.FileMode.Truncate );
			System.IO.StreamWriter writer = new System.IO.StreamWriter( inStream );
			writer.Write( outStream );
		}

		public static System.IO.Stream ApplyHash( System.IO.Stream inStream, string commentText, string commentStart, string commentEnd, string headerMarker, string hashMarker )
		{
			//string s;
			System.IO.StreamReader reader = new System.IO.StreamReader( inStream );
			System.IO.StreamWriter writer = new System.IO.StreamWriter( new System.IO.MemoryStream() );
			//string hashstring;
			string fullHeaderMarker = commentStart + headerMarker + commentEnd;

			inStream.Seek( 0, System.IO.SeekOrigin.Begin );
			writer.Write( ApplyHash(reader.ReadToEnd(), commentText, commentStart, commentEnd, true, headerMarker, hashMarker ));
			writer.Flush();
			writer.BaseStream.Seek( 0, System.IO.SeekOrigin.Begin );
			return writer.BaseStream;
		}

		public static string ApplyHash( string s, string commentText, string commentStart, string commentEnd, bool isString, string headerMarker, string hashMarker )
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			string hashstring;
			string fullHeaderMarker = commentStart + headerMarker + commentEnd;

			s = StripHeader( s, fullHeaderMarker );
			hashstring = CreateHash( s );

			sb.Append( fullHeaderMarker );
			sb.Append( commentStart + commentEnd );
			sb.Append( commentStart + commentText + commentEnd );
			sb.Append( commentStart + commentEnd );
			sb.Append( commentStart + hashMarker + hashstring + hashMarker + commentEnd );
			sb.Append( fullHeaderMarker );
			sb.Append( s );
			return sb.ToString();
		}

		private static string StripHeader( string s, string searchFor )
		{
			int iStart = s.IndexOf( searchFor );
			int iEnd;
			if( iStart >= 0 )
			{
				iEnd = s.Substring( iStart + searchFor.Length ).IndexOf( searchFor ) + iStart + searchFor.Length;
				if( iEnd >= 0 )
				{
					iEnd = iEnd + searchFor.Length + 2;
					if( iEnd <= s.Length )
					{
						return s.Substring( 0, iStart ) + s.Substring( iEnd );
					}
					else
					{
						return "";
					}
				}
				else
				{
					return s;
				}
			}
			else
			{
				return s;
			}
		}

		private static string GetHash( string s, string commentStart, string commentEnd, string hashMarker )
		{
			string searchForStart = commentStart + hashMarker;
			string searchForEnd = hashMarker + commentEnd;
			int iStart = s.IndexOf( searchForStart );
			int iEnd;
			int iLen;
			if( iStart >= 0 )
			{
				iStart += searchForStart.Length;
				iEnd = s.Substring( iStart ).IndexOf( searchForEnd ) + iStart - 1;
				if( iEnd >= 0 )
				{
					// Messy way to clean off the crlf
					iLen = iEnd - iStart + 1;
					return s.Substring( iStart, iLen );
				}
				else
				{
					return "";
				}
			}
			else
			{
				return "";
			}

		}

		private static string CreateHash( string s )
		{
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			//System.IO.StreamWriter writer = new System.IO.StreamWriter( new System.IO.MemoryStream )
			Byte[] hash;
			Byte[] input;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			s = s.Trim();
			input = new Byte[s.Length - 1];
			for( int i = 0; i <= input.GetUpperBound( 0 ); i++ )
			{
				input[i] = (byte)(s[i]);
			}
			//   writer.Write( s )
			//   writer.BaseStream.Seek( 0, System.IO.SeekOrigin.Begin )
			hash = md5.ComputeHash( input );
			for( int i = 0; i <= hash.GetUpperBound(0 ); i++ )
			{
				sb.Append( hash[i].ToString( "x2" ) );
			}
			//Int64 BitConcat
			//For i As int = 0 To 1
			//   BitConcat = 0
			//   For j As int = 0 To 7
			//      If ( j = 0 ) Then
			//         BitConcat = BitConcat | ( Convert.ToInt64(hash(j + 8 * i )))
			//      Else
			//         BitConcat = BitConcat | ( Convert.ToInt64(hash(j + 8 * i )) * Convert.ToInt64( 2 ^ (j * 8 - 1 )))
			//      End If
			//   Next
			//   sb.Append( Convert.ToString(BitConcat ))
			//Next
			return sb.ToString();
		}
	}
}
