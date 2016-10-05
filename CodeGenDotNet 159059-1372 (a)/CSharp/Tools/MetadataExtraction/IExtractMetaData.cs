// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Interface for all metadata extraction tools

using System;
using System.Data;
using System.Diagnostics;

namespace KADGen.Metadata
{
	/// <summary>
	/// Interface for metadata extracton
	/// </summary>
	public interface IExtractMetaData
	{
		bool UseVerboseNames
		{
			get;
			set;
		}

		bool UseProcContents
		{
			get;
			set;
		}

		string ServerName
		{
			get;
			set;
		}

		System.Xml.XmlDocument CreateMetaData(	bool skipStoredProcs, 
			string setSelectPatterns,
			string selectPatterns,
			string removePrefix,
			string lookupPrefix,
			params string[] databaseNames );
	}
}
