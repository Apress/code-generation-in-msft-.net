// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Specialized class for extracting metadata from SQL Server

using System;
using System.Data;

namespace KADGen.Metadata
{
	/// <summary>
	/// Summary description for SQLExtractMetaData.
	/// </summary>
	public class SQLExtractMetaData : ExtractMetaData
	{
		#region Class level declarations
		System.Collections.Hashtable mdtExtendedColumns = new System.Collections.Hashtable();      // Also hold parameters
		System.Collections.Hashtable mdtColumnInfo = new System.Collections.Hashtable();     // Also hold parameters
		System.Data.DataTable mdtStoredProcPrivileges = new System.Data.DataTable();
		#endregion

		#region Constructors
		public SQLExtractMetaData( string serverName ) : base()
		{
			mServerName = serverName;
		}
		#endregion

		#region Public Methods and Properties-empty
		#endregion

		#region Protected and Friend Methods and Properties
		protected System.Data.SqlClient.SqlConnection GetConnection( string databaseName )
		{
			return new System.Data.SqlClient.SqlConnection(
				"workstation id=" + mServerName +
				";packet size=4096;integrated security=SSPI" +
				";data source=" + mServerName +
				";persist security info=False;initial catalog=" + databaseName);

		}

		protected override System.Data.DataTable DataTableFromSQL(	string SQLText,
																	string databaseName,
																	string tablename )
		{
			System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter( SQLText, GetConnection( databaseName ) );
			System.Data.DataTable dt = new System.Data.DataTable();
			da.Fill( dt );
			dt.TableName = tablename;
			return dt;
		}

		protected override void DataTableFromSQL(	string SQLText,
													string databaseName,
													System.Data.DataTable dt )
		{
			System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter( SQLText, GetConnection( databaseName ) );
			da.Fill( dt );
		}

		protected override System.Data.DataSet RunStoredProc(	string spname,
																System.Xml.XmlNode nodeParams,
																string databaseName )
		{
			System.Data.DataSet ds = new System.Data.DataSet();
			System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
			System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
			object def;
			string name;
			System.Data.SqlClient.SqlTransaction tr;

			cmd.Connection = GetConnection( databaseName );
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = spname;
			foreach( System.Xml.XmlNode node in nodeParams )
			{
				string s = Utility.Tools.GetAttributeOrEmpty(node, "Type").ToLower();
				if( s == "char" || s == "varchar" || s == "nchar" || s == "nvarchar" )
				{
					def = "";
				}
				else if( s == "uniqueidentifier" )
				{
					def = DBNull.Value;
				}
				else if( s == "datetime" )
				{
					def = "1/1/1873";
				}
				else
					def = 0;
				name = Utility.Tools.GetAttributeOrEmpty( node, "Name" );
				cmd.Parameters.Add( new System.Data.SqlClient.SqlParameter( name, def ) );
			}
			da.SelectCommand = cmd;
			cmd.Connection.Open();
			cmd.CommandTimeout = 5;
			tr = cmd.Connection.BeginTransaction( IsolationLevel.Serializable );
			cmd.Transaction = tr;
			da.Fill( ds );
			tr.Rollback();
			cmd.Connection.Close();
			return ds;
		}

		protected override void FillMetaDataSet( string name )
		{
			mdtExtendedColumns = new System.Collections.Hashtable();
			base.FillMetaDataSet( name );
			mDataSet.Tables.Add(DataTableFromSQL( "SELECT * FROM ::fn_listextendedproperty(null, null, null, null, null, null, null)", name, "ExtDatabase" ) );
			mDataSet.Tables.Add(DataTableFromSQL( "SELECT * FROM ::fn_listextendedproperty(null, 'User', 'dbo', 'Table', null, null, null)", name, "ExtTable" ) );
			mDataSet.Tables.Add(DataTableFromSQL( "SELECT * FROM ::fn_listextendedproperty(null, 'User', 'dbo', 'Procedure', null, null, null)", name, "ExtStoredProc" ) );
			mDataSet.Tables.Add(DataTableFromSQL( "SELECT * FROM ::fn_listextendedproperty(null, 'User', 'dbo', 'View', null, null, null)", name, "ExtView" ) );
			mDataSet.Tables.Add(DataTableFromSQL( "SELECT * FROM ::fn_listextendedproperty(null, 'User', 'dbo', 'Function', null, null, null)", name, "ExtFunction" ) );
			mdtStoredProcPrivileges = DataTableFromSQL( "sp_helprotect", name, "StoredProcPrivileges" );
		}

		protected override void AddProviderSpecificXMLNodes(	System.Xml.XmlNode node,
																			string databaseName,
																			string schemaName )
		{

			System.Data.DataTable dt = mDataSet.Tables["ExtDatabase"];
			System.Xml.XmlNode nodeProps;
			System.Xml.XmlNode nodeProp;
			System.Xml.XmlDocument xmlDoc = node.OwnerDocument;
			if( dt.Rows.Count > 0 )
			{
				nodeProps = node.AppendChild( this.CreateElement( "ExtendedProperties" ) );
				foreach( System.Data.DataRow row in dt.Rows )
				{
					nodeProp = nodeProps.AppendChild( this.CreateElement( "ExtendedProperty", row["name"].ToString() ) );
					nodeProp.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "Value", row["value"].ToString() ) );
				}
			}

		}

		protected override void AddProviderSpecificXMLNodes(	System.Xml.XmlNode node,
																			string databaseName,
																			string schemaName,
																			string tableName,
																			string modeName )
		{
			if( modeName == "StoredProc" )
			{
				AddExtraStoredProcInfo( node, databaseName, schemaName, tableName, modeName );
			}
			else
			{
				AddExtraTableInfo( node, databaseName, schemaName, tableName, modeName );
			}
		}

		protected override void AddProviderSpecificXMLNodes(	System.Xml.XmlNode node,
																			string databaseName,
																			string schemaName,
																			string tableName,
																			string modeName,
																			string columnName )
		{
			System.Data.DataTable dt;
			//System.Xml.XmlNode nodeProps;
			//System.Xml.XmlNode nodeProp;
			System.Xml.XmlDocument xmlDoc = node.OwnerDocument;
			System.Data.DataRow[] rows;
			string extSQL = null;

			if( mdtColumnInfo.Contains( tableName ) )
			{
				dt = (System.Data.DataTable)mdtColumnInfo[tableName];
			}
			else
			{
				dt = DataTableFromSQL( "sp_columns [" + tableName + "]", databaseName, "columns" );
				mdtColumnInfo.Add( tableName, dt );
			}
			foreach( System.Data.DataRow row in dt.Rows )
			{
				if( ((string)row["COLUMN_NAME"]).ToLower() == columnName.ToLower() )
				{
					if( ((string)row["TYPE_NAME"]).IndexOf( "identity" ) >= 0 )
					{
						node.Attributes.Append( Utility.xmlHelpers.NewAttribute( node.OwnerDocument, "IsAutoIncrement", "true" ) );
					}
				}
			}

			if( mdtExtendedColumns.Contains( tableName ) )
			{
				dt = (System.Data.DataTable)mdtExtendedColumns[tableName];
			}
			else
			{
				switch( modeName )
				{
					case "Table":
						extSQL = "SELECT * FROM ::fn_listextendedproperty(null, " +
							"'User', 'dbo', 'Table', '" + tableName +
							"', 'Column', null)";
						break;
					case "View":
						extSQL = "SELECT * FROM ::fn_listextendedproperty(null, " +
							"'User', 'dbo', 'View', '" + tableName +
							"', 'Column', null)";
						break;
					case "StoredProc":
						extSQL = "SELECT * FROM ::fn_listextendedproperty(null, " +
							"'User','dbo', 'Procedure', '" + tableName +
							"', 'Parameter', null)";
						break;
					case "Function":
						extSQL = "SELECT * FROM ::fn_listextendedproperty(null, " +
							"'User', 'dbo', 'Function', '" + tableName +
							"', 'Parameter', null)";
						break;
				}
				dt = DataTableFromSQL( extSQL, databaseName, "ExtProp" );
				mdtExtendedColumns.Add( tableName, dt );
			}
			rows = dt.Select( "objname='" + columnName + "'" );
			if( rows.GetLength(0) > 0 )
			{
				foreach( System.Data.DataRow row in rows )
				{
					node.Attributes.Append( Utility.xmlHelpers.NewAttribute( node.OwnerDocument, row["name"].ToString(), row["value"].ToString() ) );
				}
			}
		}

		protected virtual void AddExtraTableInfo(	System.Xml.XmlNode node,
													string databaseName,
													string schemaName,
													string tableName,
													string modeName )
		{
			string extTableName = "Ext" + node.LocalName;
			System.Data.DataTable dt = mDataSet.Tables[extTableName];
			System.Xml.XmlNode nodeProps;
			System.Xml.XmlNode nodeProp;
			System.Xml.XmlDocument xmlDoc = node.OwnerDocument;
			System.Data.DataRow[] rows = dt.Select( "objname='" + tableName + "'" );
			if( dt.Rows.Count > 0 )
			{
				nodeProps = node.AppendChild( this.CreateElement( "ExtendedProperties" ) );
				foreach( System.Data.DataRow row in rows )
				{
					nodeProp = nodeProps.AppendChild( this.CreateElement( "ExtendedProperty", row["name"].ToString() ) );
					nodeProp.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Value", row["value"].ToString() ) );
				}
			}
		}

		protected virtual void AddExtraStoredProcInfo(	System.Xml.XmlNode node,
														string databaseName,
														string schemaName,
														string tableName,
														string modeName )
		{
			// Get Stored Proc Privileges
			System.Data.DataRow[] rowPrivileges;
			rowPrivileges = mdtStoredProcPrivileges.Select( "Object = '" + tableName + "'" );
			System.Xml.XmlNode nodePrivileges = this.CreateElement( modeName + "Privileges" );
			System.Xml.XmlNode nodePrivilege;
			foreach( System.Data.DataRow rowPrivilege in rowPrivileges )
			{
				nodePrivilege = this.CreateElement( modeName + "Privilege" );
				nodePrivilege.Attributes.Append( Utility.xmlHelpers.NewAttribute(
					mXMLDoc, "Grantor", rowPrivilege["Grantor"].ToString() ) );
				nodePrivilege.Attributes.Append( Utility.xmlHelpers.NewAttribute(
					mXMLDoc, "Grantee", rowPrivilege["Grantee"].ToString() ) );
				nodePrivilege.Attributes.Append( Utility.xmlHelpers.NewAttribute(
					mXMLDoc, "Type", rowPrivilege["Action"].ToString() ) );
				nodePrivilege.Attributes.Append( Utility.xmlHelpers.NewAttribute(
					mXMLDoc, "ProtectType", rowPrivilege["ProtectType"].ToString() ) );
				nodePrivileges.AppendChild( nodePrivilege );
				// NOTE: We are not yet supporting column privileges or Deny access!!!
			}
			node.AppendChild( nodePrivileges );
		}

		protected override System.Data.DataTable GetTables( string databaseName )
		{
			System.Data.DataTable dta;
			dta = base.GetTables( databaseName );
			// This is an ugly hack because the data diagram table dtProperties
			// is marked by SQL as a user table
			System.Data.DataRow[] rows;
			rows = dta.Select( "TABLE_NAME = 'dtproperties'" );
			if( rows.GetLength(0) > 0 )
			{
				dta.Rows.Remove( rows[0] );
			}
			return dta;
		}
		#endregion
		
		#region Private Methods and Properties-empty
		#endregion
	}
}
