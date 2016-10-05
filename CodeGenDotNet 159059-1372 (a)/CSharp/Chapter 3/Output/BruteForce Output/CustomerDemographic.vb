' ***^^^***|||***^^^***' ' ' ' %%%###%%%a88cb8b8c0f0ed20148dbebb670d4f85%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// CustomerDemographic.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class CustomerDemographicCollection : CollectionBase
    #region Constructors
    protected CustomerDemographicCollection() : base( "CustomerDemographicCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    stringCustomerTypeID,
                    int UserID
    {
        CustomerDemographicDataAccessor.Fill( this, CustomerTypeID, UserID );
    }
    #endregion
}


public class CustomerDemographic : RowBase
    #region Class Level Declarations
    protected CustomerDemographic mCollection;
    private static int mNextPrimaryKey = -1;
    
    private string CustomerTypeID;
    private string CustomerDesc;
    #endregion
    #region Constructors
    internal CustomerDemographic( CustomerDemographicCollection CustomerDemographicCollection ) : base()
    {
        mCollection = CustomerDemographicCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
    public ColumnInfo CustomerTypeID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "CustomerTypeID"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NChar"
        columnInfo.Caption = "Customer Type ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string CustomerTypeID
    {
        get
        {
            return mCustomerTypeID;
        }
        set
        {
            mCustomerTypeID = value;
        }
    }
    public ColumnInfo CustomerDesc()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "CustomerDesc"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NText"
        columnInfo.Caption = "Customer Desc"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string CustomerDesc
    {
        get
        {
            return mCustomerDesc;
        }
        set
        {
            mCustomerDesc = value;
        }
    }
    #endregion
    End Class
