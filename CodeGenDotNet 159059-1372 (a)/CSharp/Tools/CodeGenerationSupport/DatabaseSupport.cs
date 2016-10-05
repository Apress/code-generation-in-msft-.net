// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Utility files for database actions

using System;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace KADGen.CodeGenerationSupport
{
	/// <summary>
	/// 
	/// </summary>
	public class DatabaseSupport
	{
		#region Class level declarations - empty
		#endregion

		#region Constructors - empty
		#endregion

		#region Public Methods and Properties
		public static string GetConnectionString( string serverName, string databaseName )
		{
			return "workstation id=" + serverName + ";packet size=4096;integrated security=SSPI;data source=" + serverName + ";persist security info=False;initial catalog=" + databaseName;
		}

		public static void CreateStoredProcFromFile(	string fileName,
			string serverName,
			string databaseName,
			string spName,
			string executeUser )
		{
			System.IO.StreamReader streamReader = new System.IO.StreamReader( fileName );
			System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
			System.Data.SqlClient.SqlTransaction transaction = null;
			//string[] SQLStatements;
			cmd.Connection = new System.Data.SqlClient.SqlConnection( CodeGenerationSupport.DatabaseSupport.GetConnectionString( serverName, databaseName ) );
			cmd.CommandText = "sp_help '" + spName + "'";
			try
			{
				cmd.Connection.Open();
				transaction = cmd.Connection.BeginTransaction();
				cmd.Transaction = transaction;
				string[] statements = Regex.Split( streamReader.ReadToEnd(), "\\sgo\\s", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase );
				foreach( string statement in statements )
				{
					cmd.CommandText = statement;
					cmd.ExecuteNonQuery();
				}
				transaction.Commit();
			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex );
				transaction.Rollback();
				throw;
			}
			finally
			{
				try
				{
					cmd.Connection.Close();
				}
				catch
				{
				}
			}
		}
		#endregion
	}
}
