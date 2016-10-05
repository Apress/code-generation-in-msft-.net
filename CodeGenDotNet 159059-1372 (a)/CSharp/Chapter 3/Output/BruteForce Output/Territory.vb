' ***^^^***|||***^^^***' ' ' ' %%%###%%%1298748be01dde1289ef442775ad0c8e%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Territory.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class TerritoryCollection : CollectionBase
    #region Constructors
    protected TerritoryCollection() : base( "TerritoryCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    stringTerritoryID,
                    int UserID
    {
        TerritoryDataAccessor.Fill( this, TerritoryID, UserID );
    }
    #endregion
}


public class Territory : RowBase
    #region Class Level Declarations
    protected Territory mCollection;
    private static int mNextPrimaryKey = -1;
    
    private string TerritoryID;
    private string TerritoryDescription;
    private int RegionID;
    #endregion
    #region Constructors
    internal Territory( TerritoryCollection TerritoryCollection ) : base()
    {
        mCollection = TerritoryCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
    public ColumnInfo TerritoryID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "TerritoryID"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Territory ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string TerritoryID
    {
        get
        {
            return mTerritoryID;
        }
        set
        {
            mTerritoryID = value;
        }
    }
    public ColumnInfo TerritoryDescription()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "TerritoryDescription"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NChar"
        columnInfo.Caption = "Territory Description"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string TerritoryDescription
    {
        get
        {
            return mTerritoryDescription;
        }
        set
        {
            mTerritoryDescription = value;
        }
    }
    public ColumnInfo RegionID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "RegionID"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Region ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int RegionID
    {
        get
        {
            return mRegionID;
        }
        set
        {
            mRegionID = value;
        }
    }
    #endregion
    End Class
