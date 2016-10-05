// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Contains some general use functions 

using System;

namespace KADGen.Utility
{
	public abstract class SourceBase : IDisposable
	{
		public enum ItemStatus
		{
			CheckedOut = 1,
			CheckedOutToMe,
			NotCheckedOut,
			DoesntExist
		}
		
		public abstract SourceBase.ItemStatus CheckOut( string file, string basePath, string baseProject );

		public abstract Utility.SourceBase.ItemStatus CheckIn( string file, string basePath, string baseProject );

		public abstract void AddFile( string file, string basePath, string baseProject );

		public abstract void CleanUp();

		void System.IDisposable.Dispose()
		{
			CleanUp();
			GC.SuppressFinalize( this );
		}

		~SourceBase()
		{
			CleanUp();
		}

		public virtual string BuildSSPath( string file, string basePath, string baseProject )
		{
			file = System.IO.Path.GetFileName( file );
			return System.IO.Path.Combine( baseProject, file ).Replace( "\\", "/" );
		}

	}

	public class SourceControlException : System.ApplicationException
	{
		private string mFile;

		public SourceControlException( string message, string file ) : base( message )
		{
			mFile = file;
		}

		public SourceControlException( string message, System.Exception innerException, string file ) : base( message, innerException )
		{
			mFile = file;
		}

		public string File
		{
			get
			{
				return mFile;
			}
			set
			{
				mFile = value;
			}
		}
	}
}
