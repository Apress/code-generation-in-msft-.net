' ***^^^***|||***^^^***' ' ' ' %%%###%%%1d92ee77c76a3b8ad62bc7bdc7749819%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// EmployeeTerritory.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class EmployeeTerritoryCollection : CollectionBase
    #region Constructors
    protected EmployeeTerritoryCollection() : base( "EmployeeTerritoryCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intEmployeeID,
                    int UserID
    {
        EmployeeTerritoryDataAccessor.Fill( this, EmployeeID, UserID );
    }
    #endregion
}


public class EmployeeTerritory : RowBase
    #region Class Level Declarations
    protected EmployeeTerritory mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int EmployeeID;
    private string TerritoryID;
    #endregion
    #region Constructors
    internal EmployeeTerritory( EmployeeTerritoryCollection EmployeeTerritoryCollection ) : base()
    {
        mCollection = EmployeeTerritoryCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
    public ColumnInfo EmployeeID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "EmployeeID"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Employee ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int EmployeeID
    {
        get
        {
            return mEmployeeID;
        }
        set
        {
            mEmployeeID = value;
        }
    }
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
    #endregion
    End Class
