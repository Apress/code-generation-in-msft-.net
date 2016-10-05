// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Establishes the order of tables for deletion and insertion based on hierarchy

using System;

namespace KADGen.Metadata
{
	public class TableInsertDeletePriority
	{
		public static void appendTableInsertPriority( System.Data.DataSet metaData )
		{
			System.Data.DataTable Tables = metaData.Tables["Tables"];
			Tables.Columns.Add( "LOAD_PRIORITY" );
			
			System.Data.DataTable TableConstraints = metaData.Tables["TableConstraints"];
			System.Data.DataTable ReferentialConstraints = metaData.Tables["ReferentialConstraints"];
			for( int i=0; i<Tables.Rows.Count; i++ )
			{
				if( IsRoot( (string)Tables.Rows[i]["TABLE_NAME"], TableConstraints, ReferentialConstraints )  )
					assignInsertPriority( (string)Tables.Rows[i]["TABLE_NAME"], 0, Tables, TableConstraints, ReferentialConstraints );
			}
		}
		private static void assignInsertPriority( string TableName, int Priority, System.Data.DataTable Tables, System.Data.DataTable TableConstraints, System.Data.DataTable ReferentialConstraints )
		{
			System.Data.DataRow TableRow = Tables.Select( "TABLE_NAME = '" + TableName + "'" )[0];
			if( ( TableRow.IsNull( "LOAD_PRIORITY" ) ) || ( Priority > Convert.ToInt32( TableRow["LOAD_PRIORITY"] ) ) )
			{
				TableRow["LOAD_PRIORITY"] = Priority;
			}
			System.Data.DataRow[] TableConstraint = TableConstraints.Select( "CONSTRAINT_TYPE = 'PRIMARY KEY' and TABLE_NAME = '" + TableName + "'" );
			if( TableConstraint.Length == 0 )
				return;
			string UniqueConstraintName = (string)TableConstraint[0]["CONSTRAINT_NAME"];
			System.Data.DataRow[] ParentRelations = ReferentialConstraints.Select( "UNIQUE_CONSTRAINT_NAME = '" + UniqueConstraintName + "'" );
			for( int i=0; i<ParentRelations.Length; i++ )
			{
				System.Data.DataRow[] ChildRelations = TableConstraints.Select( "CONSTRAINT_TYPE = 'FOREIGN KEY' and CONSTRAINT_NAME = '" + ParentRelations[i]["CONSTRAINT_NAME"] + "'" );
				// Only in case of self reference?
				if( ChildRelations.Length == 0 )
					continue;
				// protect infinite recursion in case of self reference
				if( ((string)ChildRelations[0]["TABLE_NAME"]).Equals( TableName ) )
					continue;
				string ChildTableName = (string)ChildRelations[0]["TABLE_NAME"];
				assignInsertPriority( ChildTableName, Priority+1, Tables, TableConstraints, ReferentialConstraints );
			}
		}

		public static void appendTableDeletePriority( System.Data.DataSet metaData )
		{
			System.Data.DataTable Tables = metaData.Tables["Tables"];
			Tables.Columns.Add( "DELETE_PRIORITY" );
			
			System.Data.DataTable TableConstraints = metaData.Tables["TableConstraints"];
			System.Data.DataTable ReferentialConstraints = metaData.Tables["ReferentialConstraints"];
			for( int i=0; i<Tables.Rows.Count; i++ )
			{
				if( IsRoot( (string)Tables.Rows[i]["TABLE_NAME"], TableConstraints, ReferentialConstraints )  )
					assignInsertPriority( (string)Tables.Rows[i]["TABLE_NAME"], 0, Tables, TableConstraints, ReferentialConstraints );
			}
		}
		private static void assignDeletePriority( string TableName, int Priority, System.Data.DataTable Tables, System.Data.DataTable TableConstraints, System.Data.DataTable ReferentialConstraints )
		{
			System.Data.DataRow TableRow = Tables.Select( "TABLE_NAME = '" + TableName + "'" )[0];
			if( ( TableRow.IsNull( "DELETE_PRIORITY" ) ) || ( Priority < Convert.ToInt32( TableRow["DELETE_PRIORITY"] ) ) )
			{
				TableRow["DELETE_PRIORITY"] = Priority;
			}
			System.Data.DataRow[] TableConstraint = TableConstraints.Select( "CONSTRAINT_TYPE = 'PRIMARY KEY' and TABLE_NAME = '" + TableName + "'" );
			if( TableConstraint.Length == 0 )
				return;
			string UniqueConstraintName = (string)TableConstraint[0]["CONSTRAINT_NAME"];
			System.Data.DataRow[] ParentRelations = ReferentialConstraints.Select( "UNIQUE_CONSTRAINT_NAME = '" + UniqueConstraintName + "'" );
			for( int i=0; i<ParentRelations.Length; i++ )
			{
				System.Data.DataRow[] ChildRelations = TableConstraints.Select( "CONSTRAINT_TYPE = 'FOREIGN KEY' and CONSTRAINT_NAME = '" + ParentRelations[i]["CONSTRAINT_NAME"] + "'" );
				// Only in case of self reference?
				if( ChildRelations.Length == 0 )
					continue;
				// protect infinite recursion in case of self reference
				if( ((string)ChildRelations[0]["TABLE_NAME"]).Equals( TableName ) )
					continue;
				string ChildTableName = (string)ChildRelations[0]["TABLE_NAME"];
				assignInsertPriority( ChildTableName, Priority+1, Tables, TableConstraints, ReferentialConstraints );
			}
		}
		
		private static bool IsRoot( string TableName, System.Data.DataTable TableConstraints, System.Data.DataTable ReferentialConstraints )
		{
			System.Data.DataRow[] Constraints = TableConstraints.Select( "CONSTRAINT_TYPE = 'FOREIGN KEY' and TABLE_NAME = '"+TableName+"'" );
			if( Constraints.Length == 0 )
				return false;
			string ConstraintName = (string)Constraints[0]["CONSTRAINT_NAME"];
			System.Data.DataRow[] ChildRelations = ReferentialConstraints.Select( "CONSTRAINT_NAME = '" + ConstraintName + "'" );
			for( int i=0; i<ChildRelations.Length; i++ )
			{
				if( !((string)TableConstraints.Select( "CONSTRAINT_TYPE = 'PRIMARY KEY' and CONSTRAINT_NAME = '" + (string)ChildRelations[i]["UNIQUE_CONSTRAINT_NAME"] + "'" )[0]["TABLE_NAME"]).Equals( TableName ) )
					return false;
			}
			return true;
		}
	}
}
