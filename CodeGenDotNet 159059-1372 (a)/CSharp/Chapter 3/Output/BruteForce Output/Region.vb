' ***^^^***|||***^^^***' ' ' ' %%%###%%%bb1321b37ea79b9b3af46740229c265a%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Region.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class RegionCollection : CollectionBase
    #region Constructors
    protected RegionCollection() : base( "RegionCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intRegionID,
                    int UserID
    {
        RegionDataAccessor.Fill( this, RegionID, UserID );
    }
    #endregion
}


public class Region : RowBase
    #region Class Level Declarations
    protected Region mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int RegionID;
    private string RegionDescription;
    #endregion
    #region Constructors
    internal Region( RegionCollection RegionCollection ) : base()
    {
        mCollection = RegionCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
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
    public ColumnInfo RegionDescription()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "RegionDescription"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NChar"
        columnInfo.Caption = "Region Description"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string RegionDescription
    {
        get
        {
            return mRegionDescription;
        }
        set
        {
            mRegionDescription = value;
        }
    }
    #endregion
    End Class
