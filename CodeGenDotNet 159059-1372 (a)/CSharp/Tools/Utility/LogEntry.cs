// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Encapsulates a log entry

using System;

namespace KADGen.Utility
{
	public class LogEntry
	{
		public enum logLevel
		{
			InfoOnly,
			Warning,
			SeriousError,
			CriticalError
		}
		
		private logLevel m_Level;
		private string m_Message;
		private string m_Source;

		public LogEntry( logLevel level, string message, string source )
		{
			this.Message = message;
			this.Level = level;
			this.Source = source;
		}

		public string Message
		{
			get
			{
				return this.m_Message;
			}
			set
			{
				this.m_Message = value;
			}
		}

		public string Source
		{
			get
			{
				return this.m_Source;
			}
			set
			{
				this.m_Source = value;
			}
		}

		public logLevel Level
		{
			get
			{
				return this.m_Level;
			}
			set
			{
				this.m_Level = value;
			}
		}
	}
}
