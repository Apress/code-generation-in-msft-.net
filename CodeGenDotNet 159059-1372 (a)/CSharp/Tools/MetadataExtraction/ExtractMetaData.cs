// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Main class for extracting metadata from SQL-92 databases

using System;
using System.Data;
using System.Diagnostics;
using System.Xml;
using KADGen;

namespace KADGen.Metadata
{
	/// <summary>
	/// Metadata extracton based on the SQL-92 INFORMATION_SCHEMA standard. Tested only against SQL Server
	/// </summary>
	public abstract class ExtractMetaData : IExtractMetaData
	{
		#region Class level declarations
		protected string mServerName;
		private bool mUseVerboseNames = false;
		private bool mUseProcContents = false;
		protected XmlDocument mXMLDoc;
		protected DataSet mDataSet;
		protected string[] mSelectPatterns;
		protected string[] mSetSelectPatterns;
		protected string mRemovePrefix;

		private enum ErrorResponse
		{
			Continue,
			Abort
		}
		#endregion

		#region Constructors -empty
		#endregion

		#region Abstracts
		protected abstract DataTable DataTableFromSQL( string SQLText, string databaseName, string tablename );

		protected abstract void DataTableFromSQL( string SQLText, string tablename, DataTable dt );

		protected abstract DataSet RunStoredProc( string spname, XmlNode nodeParams, string databaseName );
		#endregion

		#region Public Methods and Properties

		public bool UseVerboseNames
		{
			get
			{
				return this.mUseVerboseNames;
			}
			set
			{
				this.mUseVerboseNames = value;
			}
		}

		public bool UseProcContents
		{
			get
			{
				return this.mUseProcContents;
			}
			set
			{
				this.mUseProcContents = value;
			}
		}

		public string ServerName
		{
			get
			{
				return this.mServerName;
			}
			set
			{
				this.mServerName = value;
			}
		}

		public XmlDocument CreateMetaData(	bool skipStoredProcs, 
														string selectPatterns,
														string setSelectPatterns,
														string removePrefix,
														string lookupPrefix,
														params string[] databaseNames )
		{
			mXMLDoc = new XmlDocument();
			XmlElement nodeRoot = mXMLDoc.CreateElement( "dbs", "MetaDataRoot", "http://kadgen/DatabaseStructure" );
			nodeRoot.Attributes.Append( Utility.xmlHelpers.NewBoolAttribute( mXMLDoc, "FreeForm", true ) );
			XmlElement nodeDatabase;
			// Retrieve database names if needed
			if( databaseNames.GetLength(0) == 0 )
			{
				DataTable dtDatabases = GetDatabases();
				databaseNames = new string[dtDatabases.Rows.Count];
				for( int i=0; i<dtDatabases.Rows.Count; i++ )
				{
					databaseNames[i] = dtDatabases.Rows[i]["CATALOG_NAME"].ToString();
				}
			}

			this.SetSelectPatterns( selectPatterns, setSelectPatterns );
			this.mRemovePrefix = removePrefix;

			mXMLDoc.AppendChild( mXMLDoc.CreateXmlDeclaration( "1.0", "UTF-8", "" ) );
			mXMLDoc.AppendChild( nodeRoot );
			XmlElement node = this.CreateElement( "DataStructures" );
			nodeRoot.AppendChild( node );
			foreach( string name in databaseNames )
			{
				mDataSet = null;
				nodeDatabase = this.CreateElement( "DataStructure", name );
				node.AppendChild( nodeDatabase );
				FillMetaDataSet( name );
				TableInsertDeletePriority.appendTableInsertPriority( mDataSet );
				TableInsertDeletePriority.appendTableDeletePriority( mDataSet );
				nodeDatabase.AppendChild( this.GetAllTablesXML( lookupPrefix ) );
				nodeDatabase.AppendChild( this.GetAllUserDefinedTypesXML() );
				if( !skipStoredProcs )
				{
					nodeDatabase.AppendChild( this.GetAllStoredProcXML( name ) );
					nodeDatabase.AppendChild( this.GetAllFunctionsXML() );
				}
				nodeDatabase.AppendChild( this.GetAllViewsXML() );
				AddProviderSpecificXMLNodes( nodeDatabase, 
					mDataSet.Tables["Database"].Rows[0]["CATALOG_NAME"].ToString(), 
					mDataSet.Tables["Database"].Rows[0]["SCHEMA_NAME"].ToString() );
				nodeDatabase.AppendChild( this.GetHierarchyXML() );
			}
			return mXMLDoc;
		}

		#endregion

		#region Protected and Friend Methods and Properties
		protected virtual XmlNode GetAllTablesXML( string lookupPrefix )
		{
			XmlElement nodeTables = this.CreateElement( "Tables" );
			nodeTables.Prefix = "dbs";
			foreach( DataRow rowTable in mDataSet.Tables["Tables"].Rows )
			{
				// Each row represents a datatable
				nodeTables.AppendChild( GetTableXML( rowTable, "Table", lookupPrefix ) );
			}
			return nodeTables;
		}
		protected virtual XmlNode GetAllViewsXML()
		{
			XmlElement nodeTables = this.CreateElement( "Views" );
			foreach( DataRow rowTable in mDataSet.Tables["Views"].Rows )
			{
				// Each row represents a datatable
				nodeTables.AppendChild( GetTableXML( rowTable, "View", "" ) );
			}
			return nodeTables;
		}

		protected virtual XmlNode GetAllUserDefinedTypesXML()
		{
			XmlElement nodeUDTs = this.CreateElement( "UserDefinedTypes" );
			foreach( DataRow rowUDT in mDataSet.Tables["UserDefinedTypes"].Rows )
			{
				// Each row represents a dataUDT
				nodeUDTs.AppendChild( GetUDTXML( rowUDT ) );
			}
			return nodeUDTs;
		}

		protected virtual XmlNode GetAllStoredProcXML( string databaseName )
		{
			XmlNode nodeStoredProcs = this.CreateElement( "StoredProcs" );
			foreach( DataRow rowStoredProc in mDataSet.Tables["SProcs"].Rows )
			{
				// Each row represents a dataStoredProc
				Utility.xmlHelpers.AppendIfExists(	nodeStoredProcs,
													GetStoredProcXML(	rowStoredProc,
																		"StoredProc",
																		databaseName ) );
			}
			return nodeStoredProcs;
		}

		protected virtual XmlNode GetAllFunctionsXML()
		{
			XmlNode nodeStoredProcs = this.CreateElement( "Functions" );
			foreach( DataRow rowStoredProc in mDataSet.Tables["Functions"].Rows )
			{
				// Each row represents a dataStoredProc
				nodeStoredProcs.AppendChild( GetStoredProcXML( rowStoredProc, "Function", "" ) );
			}
			return nodeStoredProcs;
		}

		protected virtual XmlNode GetHierarchyXML() 
		{
			XmlNamespaceManager nsmgr = this.GetNameSpaceManager( mXMLDoc, "dbs" );
			XmlNode nodeHierarchy = this.CreateElement( "Hierarchy" );
			XmlNodeList nodeParents;
			XmlNodeList allNodes = mXMLDoc.SelectNodes( "//dbs:Table", nsmgr );

			foreach( XmlNode node in allNodes )
			{
				nodeParents = node.SelectNodes( "dbs:TableConstraints/dbs:TableRelations/dbs:ParentTables/dbs:ParentTable", nsmgr );
				if( nodeParents.Count == 0 || ( nodeParents.Count == 1 && Utility.Tools.GetAttributeOrEmpty( nodeParents[0], "Name" ) == Utility.Tools.GetAttributeOrEmpty( node, "Name" ) ) )
				{
					AddHierarchyNode( nodeHierarchy, node, nsmgr );
				}
			}
			return nodeHierarchy;
		}

		protected virtual void AddHierarchyNode(	XmlNode nodeParent,
													XmlNode node,
													XmlNamespaceManager nsmgr )
		{
			XmlNodeList nodeList;
			XmlNode nodeTable;
			XmlNode nodeNew;
			nodeNew = this.CreateElement( "HTable",
				Utility.Tools.GetAttributeOrEmpty( node, "Name" ) );
			nodeParent.AppendChild( nodeNew );
			nodeList = node.SelectNodes( "dbs:TableConstraints/dbs:TableRelations/dbs:ChildTables/dbs:ChildTable", nsmgr );
			foreach( XmlNode nodeChild in nodeList )
			{
				if( nodeNew.SelectSingleNode( 
					"ancestor-or-self::dbs:HTable[@Name='" +
					Utility.Tools.GetAttributeOrEmpty( nodeChild, "Name") + "']", nsmgr) == null )
				{
					nodeTable = node.SelectSingleNode( 
						"ancestor::dbs:DataStructure/dbs:Tables/dbs:Table[@Name='" +
						Utility.Tools.GetAttributeOrEmpty( nodeChild, "Name" ) + "']", nsmgr );
					AddHierarchyNode( nodeNew, nodeTable, nsmgr );
				}
			}
		}

		protected virtual XmlNode GetTableXML(	DataRow rowTable,
															string modeName,
															string lookupPrefix )
		{
			string tableName = rowTable["TABLE_NAME"].ToString();
			DataRow[] rowColumns = mDataSet.Tables["Columns"].Select( TableMatch( rowTable ) );
			XmlNode nodeColumns;
			string originalName = rowTable["TABLE_NAME"].ToString();
			bool isLookup = false;
			string name = Utility.Tools.FixName( originalName, mRemovePrefix );
			string singularName = Utility.Tools.GetSingular( name );
			string pluralName = Utility.Tools.GetPlural( name );
			// Changed to singular 12/3/3 KAD
			XmlNode nodeTable = NewElementWithName( modeName, singularName, rowTable, "TABLE" );
			nodeTable.Attributes.Append( Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "OriginalName", originalName ) );
			nodeTable.Attributes.Append( Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "Prefix", Utility.Tools.GetPrefix( originalName ) ) );
			nodeTable.Attributes.Append( Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "SingularName", singularName ) );
			nodeTable.Attributes.Append( Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "PluralName", pluralName ) );
			nodeTable.Attributes.Append( Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "DisplayName", GetDisplayName( rowColumns, singularName ) ) );
			nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "Caption", MakeCaption( singularName ) ) );
			nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "PluralCaption", MakeCaption( pluralName ) ) );
			if( lookupPrefix.Length > 0 && originalName.StartsWith( lookupPrefix ) )
			{
				isLookup = true;
				nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "IsLookup", "true" ) );
			}

			if( modeName == "Table" )
			{
				nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "DeletePriority", rowTable["DELETE_PRIORITY"].ToString() ) );
				nodeTable.Attributes.Append(Utility.xmlHelpers.NewAttribute( nodeTable.OwnerDocument, "InsertPriority", rowTable["LOAD_PRIORITY"].ToString() ) );
			}
			nodeColumns = nodeTable.AppendChild( this.CreateElement( modeName + "Columns" ) );
			foreach( DataRow rowColumn in rowColumns )
			{
				nodeColumns.AppendChild( GetColumnXML( rowTable, rowColumn, modeName, isLookup ) );
			}
			nodeTable.AppendChild( nodeColumns );
			nodeTable.AppendChild( GetPrivilegesXML( modeName, mDataSet.Tables["TablePrivileges"].Select( TableMatch( rowTable ) ) ) );
			nodeTable.AppendChild( GetTableConstraintsXML( modeName, rowTable ) );
			AddProviderSpecificXMLNodes( nodeTable, rowTable["TABLE_CATALOG"].ToString(), rowTable["TABLE_SCHEMA"].ToString(), rowTable["TABLE_NAME"].ToString(), modeName );
			return nodeTable;
		}

		protected virtual XmlNode GetColumnXML(	DataRow rowtable, 
															DataRow rowColumn, 
															string modeName, 
															bool isLookup)
		{
			//string columnName;
			bool isPrimaryKey;
			XmlNode nodeColumn = NewElementWithName( modeName + "Column", Utility.Tools.FixName( rowColumn["COLUMN_NAME"].ToString(), this.mRemovePrefix ), rowColumn, "TABLE" );
			nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( nodeColumn.OwnerDocument, "OriginalName", rowColumn["COLUMN_NAME"].ToString() ) );
			nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( nodeColumn.OwnerDocument, "Caption", MakeCaption( rowColumn["COLUMN_NAME"].ToString() ) ) );
			DataRow[] rows;
			nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "Ordinal", rowColumn["ORDINAL_POSITION"].ToString() ) );
			string def = rowColumn["COLUMN_DEFAULT"].ToString();
			if( def.Length > 0 )
			{
				nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Default", def ) );
			}
			if( rowColumn["IS_NULLABLE"].ToString().ToLower() == "yes" )
			{
				nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "AllowNulls", "true" ) );
      
				nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "AllowNulls", "false" ) );
			}
			nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "SQLType", Utility.Tools.GetSQLTypeFromSQLType( rowColumn["DATA_TYPE"].ToString() ) ) );
			nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "NETType", Utility.Tools.GetNETTypeFromSQLType( rowColumn["DATA_TYPE"].ToString() ) ) );
			nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "MaxLength", rowColumn["CHARACTER_MAXIMUM_LENGTH"].ToString() ) );
			isPrimaryKey = this.IsPrimaryKey( rowtable, rowColumn );
			nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "IsPrimaryKey", isPrimaryKey.ToString().ToLower() ) );
			if( isLookup && !isPrimaryKey )
			{
				nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "UseForDesc", "true" ) );
			}

			if( rowColumn["DOMAIN_NAME"].ToString().Length > 0 )
			{
				nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "UDTCatalog", rowColumn["DOMAIN_CATALOG"].ToString() ) );
				nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "UDTOwner", rowColumn["DOMAIN_SCHEMA"].ToString() ) );
				nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "UDTName", rowColumn["DOMAIN_NAME"].ToString() ) );
			}
			nodeColumn.AppendChild( GetPrivilegesXML( modeName + "Column", mDataSet.Tables["ColumnPrivileges"].Select( ColumnMatch( rowtable, rowColumn ) ) ) );
			rows = mDataSet.Tables["ColumnConstraints"].Select( ColumnMatch( rowtable, rowColumn ) );
			if( rows.GetLength(0) > 0 )
			{
				nodeColumn.AppendChild( GetCheckConstraintsXML( "Check", rows ) );
			}
			AddProviderSpecificXMLNodes( nodeColumn, rowtable["TABLE_CATALOG"].ToString(), rowtable["TABLE_SCHEMA"].ToString(), rowtable["TABLE_NAME"].ToString(), modeName, rowColumn["COLUMN_NAME"].ToString() );
			return nodeColumn;
		}

		protected virtual XmlNode GetPrivilegesXML( string modeName, DataRow[] rowPrivileges ) 
		{
			XmlNode nodePrivileges = this.CreateElement( modeName + "Privileges" );
			XmlNode nodePrivilege;
			foreach( DataRow rowPrivilege in rowPrivileges )
			{
				nodePrivilege = this.CreateElement( modeName + "Privilege" );
				nodePrivilege.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Grantor", rowPrivilege["GRANTOR"].ToString() ) );
				nodePrivilege.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Grantee", rowPrivilege["GRANTEE"].ToString() ) );
				nodePrivilege.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Type", rowPrivilege["PRIVILEGE_TYPE"].ToString() ) );
				nodePrivileges.AppendChild( nodePrivilege );
			}
			return nodePrivileges;
		}

		protected virtual XmlNode GetCheckConstraintsXML( string modeName, DataRow[] rowConstraints )
		{
			XmlNode nodeConstraints;
			XmlNode nodeConstraint;
			DataRow[] rowChecks;
			nodeConstraints = this.CreateElement( modeName + "Constraints" );
			foreach( DataRow rowConstraint in rowConstraints )
			{
				rowChecks = mDataSet.Tables["CheckConstraints"].Select( ConstraintMatch( rowConstraint ) );
				if( rowChecks.GetLength(0) > 0 )
				{
					nodeConstraint = this.CreateElement( modeName + "Constraint" );
					foreach( DataRow rowCheck in rowChecks )
					{
						nodeConstraint.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Clause", rowCheck["CHECK_CLAUSE"].ToString() ) );
					}
					nodeConstraints.AppendChild( nodeConstraint );
				}
			}
			return nodeConstraints;
		}

		protected virtual XmlNode GetTableConstraintsXML( string modeName, DataRow rowTable )
		{
			XmlNode nodeConstraints;
			nodeConstraints = this.CreateElement( modeName + "Constraints" );
			AddPrimaryKeyXML( nodeConstraints, rowTable );
			AddRelationsXML( modeName, nodeConstraints, rowTable );
			AddUniqueConstraintXML( modeName, nodeConstraints, rowTable );
			AddTableCheckConstraintXML( nodeConstraints, rowTable );
			return nodeConstraints;
		}

		protected virtual XmlNode GetUDTXML( DataRow rowUDT )
		{
			XmlNode nodeUDT = NewElementWithName( "UserDefinedType", rowUDT["DOMAIN_NAME"].ToString(), rowUDT, "DOMAIN" );
			DataRow[] rowConstraints = mDataSet.Tables["UDTConstraints"].Select( UDTMatch( rowUDT ) );
			nodeUDT.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Default", rowUDT["DOMAIN_DEFAULT"].ToString() ) );
			nodeUDT.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Type", rowUDT["DATA_TYPE"].ToString() ) );
			nodeUDT.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "MaxLength", rowUDT["CHARACTER_MAXIMUM_LENGTH"].ToString() ) );
			if( rowConstraints.GetLength(0) > 0 )
			{
				nodeUDT.AppendChild( GetCheckConstraintsXML( "UDTCheck", rowConstraints ) );
			}
			return nodeUDT;
		}

		protected virtual XmlNode GetStoredProcXML( DataRow rowSProc, string modeName, string databaseName)
		{
			string spName = rowSProc["ROUTINE_NAME"].ToString();
			XmlNode nodeSProc = null;
			if( !spName.ToLower().StartsWith( "dt_" ) )
			{
				nodeSProc = NewElementWithName( modeName, spName, rowSProc, "ROUTINE" );
				XmlNode nodeParams = this.CreateElement( modeName + "Parameters" );
				DataRow[] rowParams = mDataSet.Tables["Parameters"].Select( GenericMatch(rowSProc, "SPECIFIC_", "SPECIFIC_" ) );
				nodeSProc.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Created", rowSProc["CREATED"].ToString() ) );
				nodeSProc.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Modified", rowSProc["LAST_ALTERED"].ToString() ) );
				if( mUseProcContents )
				{
					XmlNode nodeProcContents = this.CreateElement( "SourceCode" );
					nodeProcContents.AppendChild( mXMLDoc.CreateTextNode( rowSProc["ROUTINE_DEFINITION"].ToString() ) );
					nodeSProc.AppendChild( nodeProcContents );
				}
				nodeSProc.AppendChild( nodeParams );
				foreach( DataRow rowparam in rowParams )
				{
					nodeParams.AppendChild( GetParameterXML( rowSProc, rowparam, modeName ) );
				}
				if( RetrieveRecordset( spName ) )
				{
					nodeSProc.AppendChild( GetRecordsetXML( rowSProc, nodeParams, databaseName ) );
				}
				AddProviderSpecificXMLNodes(	nodeSProc, 
					rowSProc["ROUTINE_CATALOG"].ToString(),
					rowSProc["ROUTINE_SCHEMA"].ToString(),
					rowSProc["ROUTINE_NAME"].ToString(),
					"StoredProc" );
			}
			return nodeSProc;
		}

		protected virtual XmlNode GetRecordsetXML( DataRow rowSProc, XmlNode nodeParams, string databaseName )
		{
			XmlNode nodeDataSet = this.CreateElement( "DataSet" );
			DataSet ds;
			XmlNode nodeRecordset;
			XmlNode nodeColumn;
			try
			{
				ds = RunStoredProc( rowSProc["ROUTINE_NAME"].ToString(), nodeParams, databaseName );
				foreach( DataTable dt in ds.Tables )
				{
					nodeRecordset = this.CreateElement( "Recordset", dt.TableName );
					foreach( DataColumn col in dt.Columns )
					{
						nodeColumn = this.CreateElement( "Column", col.ColumnName );
						nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "NETType", col.DataType.ToString() ) );
						nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "Caption", MakeCaption( col.Caption ) ) );
						nodeColumn.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute( mXMLDoc, "AllowDBNull", (bool)col.AllowDBNull ) );
						nodeColumn.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute( mXMLDoc, "AutoIncrement", (bool)col.AutoIncrement ) );
						nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "DefaultValue", col.DefaultValue.ToString() ) );
						nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "MaxLength", col.MaxLength.ToString() ) );
						nodeColumn.Attributes.Append(Utility.xmlHelpers.NewBoolAttribute( mXMLDoc, "Unique", (bool)col.Unique ) );
						nodeColumn.Attributes.Append(Utility.xmlHelpers.NewAttribute( mXMLDoc, "Ordinal", col.Ordinal.ToString() ) );
						nodeRecordset.AppendChild( nodeColumn );
					}
					nodeDataSet.AppendChild( nodeRecordset );
				}
			}
			catch( System.Exception ex )
			{
				// Some stored procs will fail this attempt
				LogError(ex);
			}
			return nodeDataSet;
		}

		protected virtual XmlNode GetParameterXML( DataRow rowSProc, DataRow rowParam, string modeName )
		{
			XmlNode nodeColumn;
			nodeColumn = NewElementWithName( modeName + "Parameter", rowParam["PARAMETER_NAME"].ToString(), rowParam, "SPECIFIC" );
			nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Ordinal", rowParam["ORDINAL_POSITION"].ToString() ) );
			nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Direction", rowParam["PARAMETER_MODE"].ToString() ) );
			nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Type", rowParam["DATA_TYPE"].ToString() ) );
			nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "MaxLength", rowParam["CHARACTER_MAXIMUM_LENGTH"].ToString() ) );
			nodeColumn.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Direction", rowParam["PARAMETER_MODE"].ToString() ) );
			AddProviderSpecificXMLNodes( nodeColumn, rowSProc["ROUTINE_CATALOG"].ToString(), rowSProc["ROUTINE_SCHEMA"].ToString(), rowSProc["ROUTINE_NAME"].ToString(), "StoredProc", rowParam["PARAMETER_NAME"].ToString() );
			return nodeColumn;
		}

		protected virtual void AddPrimaryKeyXML( XmlNode nodeParent, DataRow rowTable )
		{
			DataRow rowPrimaryKey = GetPrimaryKeyConstraint( rowTable );
			if( rowPrimaryKey != null )
			{
				XmlNode nodeConstraint = this.CreateElement( "PrimaryKey" );
				AddKeyColumns( nodeParent.OwnerDocument, nodeConstraint, mDataSet.Tables["KeyColumns"].Select( ConstraintMatch( rowPrimaryKey ) ), "PKField" );
				nodeParent.AppendChild(nodeConstraint);
			}
		}

		protected virtual DataRow GetPrimaryKeyConstraint( DataRow rowtable )
		{
			DataRow[] rowConstraints = mDataSet.Tables["TableConstraints"].Select( TableMatch(rowtable) + " AND CONSTRAINT_TYPE='PRIMARY KEY'" );
			switch( rowConstraints.GetLength(0) )
			{
				case 0:
					// No problem, just return nothing
					return null;
				case 1:
					return rowConstraints[0];
				default:
					throw new System.Exception( "More than one primary key found on table " + rowtable["TABLE_NAME"].ToString() );
			}
		}

		protected virtual bool IsPrimaryKey( DataRow rowTable, DataRow rowColumn )
		{
			DataRow rowPrimaryKey = GetPrimaryKeyConstraint( rowTable );
			DataRow[] rowKeyColumns;
			if( rowPrimaryKey != null )
			{
				rowKeyColumns = mDataSet.Tables["KeyColumns"].Select( ConstraintMatch(rowPrimaryKey) + " AND COLUMN_NAME='" + rowColumn["COLUMN_NAME"].ToString() + "'" );
				return (rowKeyColumns.GetLength(0) > 0);
			}
			return false;
		}

		protected virtual void AddUniqueConstraintXML( string modeName, XmlNode nodeParent, DataRow rowTable )
		{
			DataRow[] rowConstraints = mDataSet.Tables["TableConstraints"].Select( TableMatch(rowTable) + " AND CONSTRAINT_TYPE='UNIQUE'" );
			foreach( DataRow row in rowConstraints )
			{
				XmlNode nodeConstraint = this.CreateElement( "Unique" );
				AddKeyColumns( nodeParent.OwnerDocument, nodeConstraint, mDataSet.Tables["KeyColumns"].Select( ConstraintMatch( row ) ), "UniqueField" );
				nodeParent.AppendChild( nodeConstraint );
			}
		}

		protected virtual void AddRelationsXML( string modeName, XmlNode nodeparent, DataRow rowtable )
		{
			XmlDocument xmlDoc = nodeparent.OwnerDocument;
			XmlNode nodeRelations = this.CreateElement( modeName + "Relations" );
			XmlNode nodeRelation;
			XmlNode nodeRelated;
			DataRow[] rowKeys;
			DataRow[] rowConstraints;
			DataRow[] rowRefConstraints;
			//string selectClause;

			// To create child relations, 
			// -find the primary key, 
			// -then find the referential constraints with this as unique side of the key
			// -finally, get those relations keys
			DataRow rowPrimaryKey = GetPrimaryKeyConstraint( rowtable );
			if( rowPrimaryKey != null )
			{
				// Child relations are possible
				rowRefConstraints = mDataSet.Tables["ReferentialConstraints"].Select( ConstraintMatch( rowPrimaryKey, "", "UNIQUE_" ) );
				if( rowRefConstraints.GetLength(0) > 0 )
				{
					nodeRelation = this.CreateElement( "ChildTables" );
					foreach( DataRow rowRefConstraint in rowRefConstraints )
					{
						rowKeys = mDataSet.Tables["KeyColumns"].Select(ConstraintMatch( rowRefConstraint ) );
						// Changed to singular 12/3/3 KAD
						nodeRelated = this.CreateElement( "ChildTable", Utility.Tools.FixName( Utility.Tools.GetSingular( rowKeys[0]["TABLE_NAME"].ToString()), this.mRemovePrefix ) );
						AddKeyColumns( xmlDoc, nodeRelated, rowKeys, "ChildKeyField" );
						nodeRelation.AppendChild( nodeRelated );
					}
					nodeRelations.AppendChild( nodeRelation );
				}
			}

			// To find all of the parent relations
			// - Find all the foriegn key relations for this table
			// - look up these referential constraints
			// - get those relations keys
			rowConstraints = mDataSet.Tables["TableConstraints"].Select( TableMatch(rowtable) + " AND CONSTRAINT_TYPE='FOREIGN KEY'" );
			if( rowConstraints.GetLength(0) > 0 )
			{
				nodeRelation = this.CreateElement( "ParentTables" );
				foreach( DataRow rowConstraint in rowConstraints )
				{
					rowRefConstraints = mDataSet.Tables["ReferentialConstraints"].Select( ConstraintMatch( rowConstraint ) );
					foreach( DataRow rowRefConstraint in rowRefConstraints )
					{
						rowKeys = mDataSet.Tables["KeyColumns"].Select( ConstraintMatch(rowRefConstraint, "UNIQUE_", "" ) );
						nodeRelated = this.CreateElement( "ParentTable" );
						nodeRelated.Attributes.Append(Utility.xmlHelpers.NewAttribute( xmlDoc, "Name", Utility.Tools.FixName( rowKeys[0]["TABLE_NAME"].ToString(), this.mRemovePrefix ) ) );
						AddKeyColumns( xmlDoc, nodeRelated, rowKeys, "ParentKeyField" );
						nodeRelation.AppendChild( nodeRelated );

						rowKeys = mDataSet.Tables["KeyColumns"].Select( ConstraintMatch( rowRefConstraint ) );
						AddKeyColumns( xmlDoc, nodeRelated, rowKeys, "ChildField" );
					}
				}
				nodeRelations.AppendChild( nodeRelation );
			}
			nodeparent.AppendChild( nodeRelations );
		}

		protected virtual void AddTableCheckConstraintXML( XmlNode nodeParent, DataRow rowtable )
		{
			XmlDocument xmlDoc = nodeParent.OwnerDocument;
			DataRow[] rowConstraints;
			XmlNode nodeCheck;
			DataRow[] rowChecks;

			rowConstraints = mDataSet.Tables["TableConstraints"].Select( TableMatch(rowtable) + " AND CONSTRAINT_TYPE='CHECK'" );
			if( rowConstraints.GetLength(0) > 0 )
			{
				foreach( DataRow rowConstraint in rowConstraints )
				{
					rowChecks = mDataSet.Tables["CheckConstraints"].Select( ConstraintMatch( rowConstraint ) );
					foreach( DataRow row in rowChecks )
					{
						nodeCheck = this.CreateElement( "TableCheckConstraints" );
						nodeCheck.Attributes.Append( Utility.xmlHelpers.NewAttribute( xmlDoc, "Clause", row["CHECK_CLAUSE"].ToString() ) );
						nodeParent.AppendChild( nodeCheck );
					}
				}
			}
		}

		protected virtual void AddProviderSpecificXMLNodes( XmlNode node, string databaseName, string schemaName )
		{
			//NOTE: This follows a pattern of passing in the parent node, because I 
			//      don't know what types of changes you'll make - attributes, multiple 
			//      child nodes, etc.
			// Override in child class to provide special behavior for database
		}

		protected virtual void AddProviderSpecificXMLNodes(	XmlNode node,
															string databaseName, 
															string schemaName, 
															string tableName, 
															string modeName )
		{
			//NOTE: This follows a pattern of passing in the parent node, because I 
			//      don't know what types of changes you'll make - attributes, multiple 
			//      child nodes, etc.
			// Override in child class to provide special behavior for table
		}

		protected virtual void AddProviderSpecificXMLNodes(	XmlNode node,
															string databaseName,
															string schemaName,
															string tableName,
															string modeName,
															string columnName )
		{
			//NOTE: This follows a pattern of passing in the parent node, because I 
			//      don't know what types of changes you'll make - attributes, multiple 
			//      child nodes, etc.
			// Override in child class to provide special behavior for column
		}
		#endregion

		#region Protected Overridable = Get Info from database
		protected virtual void FillMetaDataSet( string name )
		{
			mDataSet = new DataSet();
			mDataSet.Tables.Add( GetDatabase( name ) );
			mDataSet.Tables.Add( GetTables( name ) );
			mDataSet.Tables.Add( GetColumns( name ) );
			mDataSet.Tables.Add( GetTableprivileges( name ) );
			mDataSet.Tables.Add( GetTableConstraints( name ) );
			mDataSet.Tables.Add( GetColumnprivileges( name ) );
			mDataSet.Tables.Add( GetColumnConstraints( name ) );
			mDataSet.Tables.Add( GetCheckConstraints( name ) );
			mDataSet.Tables.Add( GetKeyColumns( name ) );
			mDataSet.Tables.Add( GetReferentialConstraints( name ) );
			mDataSet.Tables.Add( GetUDTs( name ) );
			mDataSet.Tables.Add( GetUDTConstraints( name ) );
			mDataSet.Tables.Add( GetStoredProcs( name ) );
			mDataSet.Tables.Add( GetFunctions( name ) );
			mDataSet.Tables.Add( GetParameters( name ) );
			mDataSet.Tables.Add( GetViews( name ) );
		}

		protected virtual DataTable GetDatabases()
		{
			// TODO: Ask Don whether logging onto Master makes sense here
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.SCHEMATA", "Master", "Databases" );
		}

		protected virtual DataTable GetDatabase( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE CATALOG_NAME='" + databaseName + "'", databaseName, "Database" );
		}

		protected virtual DataTable GetTables( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_CATALOG='" + databaseName + "' AND TABLE_TYPE='BASE TABLE'", databaseName, "Tables" );
		}

		protected virtual DataTable GetColumns( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG='" + databaseName + "' ORDER BY TABLE_NAME, ORDINAL_POSITION", databaseName, "Columns" );
		}

		protected virtual DataTable GetTableprivileges( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.TABLE_PRIVILEGES WHERE TABLE_CATALOG='" + databaseName + "'", databaseName, "TablePrivileges" );
		}

		protected virtual DataTable GetTableConstraints( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_CATALOG='" + databaseName + "'", databaseName, "TableConstraints" );
		}

		protected virtual DataTable GetColumnprivileges( string databaseName ) 
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.COLUMN_PRIVILEGES " + "WHERE TABLE_CATALOG='" + databaseName + "'", databaseName, "ColumnPrivileges" );
		}

		protected virtual DataTable GetColumnConstraints( string databaseName ) 
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE TABLE_CATALOG='" + databaseName + "'", databaseName, "ColumnConstraints" );
		}

		protected virtual DataTable GetCheckConstraints( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS WHERE CONSTRAINT_CATALOG='" + databaseName + "'", databaseName, "CheckConstraints" );
		}

		protected virtual DataTable GetKeyColumns( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_CATALOG='" + databaseName + "'", databaseName, "KeyColumns" );
		}

		protected virtual DataTable GetReferentialConstraints( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_CATALOG='" + databaseName + "'", databaseName, "ReferentialConstraints" );
		}

		protected virtual DataTable GetViews( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_CATALOG='" + databaseName + "' AND TABLE_TYPE='VIEW'", databaseName, "Views" );
		}

		protected virtual DataTable GetUDTs( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.DOMAINS WHERE DOMAIN_CATALOG='" + databaseName + "'", databaseName, "UserDefinedTypes" );
		}

		protected virtual DataTable GetUDTConstraints( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.DOMAIN_CONSTRAINTS WHERE DOMAIN_CATALOG='" + databaseName + "'", databaseName, "UDTConstraints" );
		}

		protected virtual DataTable GetStoredProcs( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_CATALOG='" + databaseName + "' AND ROUTINE_TYPE='PROCEDURE'", databaseName, "SProcs" );
		}

		protected virtual DataTable GetParameters( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_CATALOG='" + databaseName + "'", databaseName, "Parameters" );
		}

		protected virtual DataTable GetFunctions( string databaseName )
		{
			return DataTableFromSQL( "SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_CATALOG='" + databaseName + "' AND ROUTINE_TYPE='FUNCTION'", databaseName, "Functions" );
		}
		#endregion

		#region Protected Overridable Class Specific Utility Routines
		protected virtual void SetSelectPatterns( string selectPatterns, string setSelectPatterns )
		{
			if( selectPatterns.Trim().Length == 0 )
			{
				mSelectPatterns = new string[]{ @"\w*Select\w*" };
			}
			else
			{
				mSelectPatterns = selectPatterns.Split( '~' );
			}
			if( setSelectPatterns.Trim().Length == 0 )
			{
				mSetSelectPatterns = new string[]{ @"\w*SetSetSelect\w*" };
			}
			else
			{
				mSetSelectPatterns = setSelectPatterns.Split( '~' );
			}
		}

		protected virtual bool RetrieveRecordset( string spName )
		{
			foreach( string s in mSelectPatterns )
			{
				if( System.Text.RegularExpressions.Regex.IsMatch(spName, s) )
				{
					return true;
				}
			}
			foreach( string s  in mSetSelectPatterns )
			{
				if( System.Text.RegularExpressions.Regex.IsMatch(spName, s) )
				{
					return true;
				}
			}
			return false;
		}

		protected virtual XmlElement CreateElement( string ElementName ) 
		{
			return mXMLDoc.CreateElement( "dbs", ElementName, "http://kadgen/DatabaseStructure" );
		}

		protected virtual XmlElement CreateElement( string ElementName, string name )
		{
			XmlElement elem;
			elem = mXMLDoc.CreateElement( "dbs", ElementName, "http://kadgen/DatabaseStructure" );
			elem.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "Name", name ) );
			return elem;
		}


		protected virtual string GenericMatch( DataRow row, string targetPrefix, string sourcePrefix )
		{
			return string.Format(	"{0}CATALOG='{1}' AND {0}SCHEMA='{2}' AND {0}NAME='{3}'", 
									targetPrefix, 
									row[sourcePrefix + "CATALOG"].ToString(),
									row[sourcePrefix + "SCHEMA"].ToString(),
									row[sourcePrefix + "NAME"].ToString() );
		}

		protected virtual string TableMatch( DataRow rowTable )
		{
			return GenericMatch( rowTable, "TABLE_", "TABLE_" );
		}

		protected virtual string ColumnMatch( DataRow rowTable, DataRow rowColumn )
		{
			return TableMatch( rowTable ) + " AND COLUMN_NAME='" + rowColumn["COLUMN_NAME"].ToString() + "'";
		}

		protected virtual string UDTMatch( DataRow rowUDT )
		{
			return GenericMatch( rowUDT, "DOMAIN_", "DOMAIN_" );
		}

		protected virtual string SProcMatch( DataRow rowSProc )
		{
			return GenericMatch( rowSProc, "SPROC_", "SPROC_" );
		}

		protected virtual string ConstraintMatch( DataRow rowConstraint )
		{
			return ConstraintMatch( rowConstraint, "", "" );
		}

		protected virtual string ConstraintMatch( DataRow rowConstraint, string sourcePrefix, string targetPrefix)
		{
			return GenericMatch( rowConstraint, targetPrefix + "CONSTRAINT_", sourcePrefix + "CONSTRAINT_" );
		}

		protected virtual XmlElement NewElementWithName( string elementName, string name, DataRow row, params string[] prefixes )
		{
			string dbName;
			string owner;
			string partName;
			string fullName = null;
			XmlElement nodeElement = this.CreateElement( elementName, name );
			if( mUseVerboseNames && row != null )
			{
				foreach( string prefix in prefixes )
				{
					dbName = row[prefix + "_CATALOG"].ToString();
					owner = row[prefix + "_SCHEMA"].ToString();
					partName = row[prefix + "_NAME"].ToString();
					nodeElement.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, prefix + "Name", partName ) );
					nodeElement.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, prefix + "Database", dbName ) );
					nodeElement.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, prefix + "Owner", owner ) );
					fullName += "[" + dbName + "].[" + owner + "].[" + partName + "]";
					if( fullName.Length > 0 )
					{
						fullName += "!";
					}
				}
				nodeElement.Attributes.Append( Utility.xmlHelpers.NewAttribute( mXMLDoc, "FullName", fullName ) );
			}
			return nodeElement;
		}

		protected virtual void AddKeyColumns( XmlDocument xmldoc, XmlNode nodeParent, DataRow[] rowKeys, string fieldName )
		{
			XmlNode nodeKey;
			foreach( DataRow row in rowKeys )
			{
				nodeKey = this.CreateElement( fieldName, Utility.Tools.FixName( row["COLUMN_NAME"].ToString(), this.mRemovePrefix ) );
				nodeKey.Attributes.Append(Utility.xmlHelpers.NewAttribute( xmldoc, "Ordinal", row["ORDINAL_POSITION"].ToString() ) );
				nodeParent.AppendChild( nodeKey );
			}
		}

		private XmlNamespaceManager GetNameSpaceManager( XmlDocument xmlDoc, string prefix )
		{
			XmlNamespaceManager nsmgr = new XmlNamespaceManager( xmlDoc.NameTable );
			string ns = null; 
			switch( prefix )
			{
				case "kg":
					ns = "http://kadgen.com/KADGenDriving.xsd";
					break;
				case "dbs":
					ns = "http://kadgen/DatabaseStructure";
					break;
			}
			if( ns != null )
			{
				nsmgr.AddNamespace( prefix, ns );
				return nsmgr;
			}
			return null;
		}

		protected virtual string MakeCaption( string s )
		{
			return Utility.Tools.SpaceAtCaps( s );
		}

		protected virtual string GetDisplayName( DataRow[] rows, string tableName )
		{
			string firstString = null;
			string columnName = null;
			string simpleName = null;
			string tableTableName = null;
			string anyName = null;
			string anyDesc = null;
			// First see if we lack strings
			foreach( DataRow row in rows )
			{
				string datatype = row["DATA_TYPE"].ToString();
				if( datatype == "char" || datatype == "varchar" || datatype == "nchar" || datatype == "nvarchar" )
				{
					columnName = Utility.Tools.FixName( row["COLUMN_NAME"].ToString(), this.mRemovePrefix );
					if( firstString == null )
					{
						firstString = columnName;
					}
					if( columnName == Utility.Tools.GetSingular( tableName ) + "Name" )
					{
						tableTableName = columnName;
					}
					else if( columnName == Utility.Tools.GetPlural(tableName) + "Name" )
					{
						tableTableName = columnName;
					}
					else if( columnName == "Name" )
					{
						simpleName = columnName;
					}
					else if( anyName == null && columnName.IndexOf( "Name" ) >= 0 )
					{
						anyName = columnName;
					}
					else if( anyDesc == null && columnName.IndexOf( "Desc" ) >= 0 )
					{
						anyDesc = columnName;
					}
				}
			}

			if( tableTableName != null )
			{
				return tableTableName;
			}
			else if( simpleName != null )
			{
				return simpleName;
			}
			else if( anyName != null )
			{
				return anyName;
			}
			else if( anyDesc != null )
			{
				return anyDesc;
			}
			else if( firstString != null )
			{
				return firstString;
			}
			else
			{
				return Utility.Tools.FixName( rows[0]["COLUMN_NAME"].ToString(), this.mRemovePrefix );
			}
		}
		#endregion

		#region Private Log Methods
		private ErrorResponse LogError( System.Exception ex )
		{
			return new ErrorResponse();
		}
		#endregion
	}
}
