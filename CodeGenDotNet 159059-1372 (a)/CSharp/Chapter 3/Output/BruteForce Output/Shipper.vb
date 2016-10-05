' ***^^^***|||***^^^***' ' ' ' %%%###%%%2766d5237f9c9186ceef314de72ed352%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Shipper.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class ShipperCollection : CollectionBase
    #region Constructors
    protected ShipperCollection() : base( "ShipperCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intShipperID,
                    int UserID
    {
        ShipperDataAccessor.Fill( this, ShipperID, UserID );
    }
    #endregion
}


public class Shipper : RowBase
    #region Class Level Declarations
    protected Shipper mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int ShipperID;
    private string CompanyName;
    private string Phone;
    #endregion
    #region Constructors
    internal Shipper( ShipperCollection ShipperCollection ) : base()
    {
        mCollection = ShipperCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
    public ColumnInfo ShipperID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShipperID"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Shipper ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int ShipperID
    {
        get
        {
            return mShipperID;
        }
        set
        {
            mShipperID = value;
        }
    }
    public ColumnInfo CompanyName()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "CompanyName"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Company Name"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string CompanyName
    {
        get
        {
            return mCompanyName;
        }
        set
        {
            mCompanyName = value;
        }
    }
    public ColumnInfo Phone()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Phone"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Phone"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Phone
    {
        get
        {
            return mPhone;
        }
        set
        {
            mPhone = value;
        }
    }
    #endregion
    End Class
