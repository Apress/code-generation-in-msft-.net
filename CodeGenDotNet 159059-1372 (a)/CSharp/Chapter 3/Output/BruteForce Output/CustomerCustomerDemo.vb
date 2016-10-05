' ***^^^***|||***^^^***' ' ' ' %%%###%%%d9b0d1b13e72a764f6f26f71646eb473%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// CustomerCustomerDemo.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class CustomerCustomerDemoCollection : CollectionBase
    #region Constructors
    protected CustomerCustomerDemoCollection() : base( "CustomerCustomerDemoCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    stringCustomerID,
                    int UserID
    {
        CustomerCustomerDemoDataAccessor.Fill( this, CustomerID, UserID );
    }
    #endregion
}


public class CustomerCustomerDemo : RowBase
    #region Class Level Declarations
    protected CustomerCustomerDemo mCollection;
    private static int mNextPrimaryKey = -1;
    
    private string CustomerID;
    private string CustomerTypeID;
    #endregion
    #region Constructors
    internal CustomerCustomerDemo( CustomerCustomerDemoCollection CustomerCustomerDemoCollection ) : base()
    {
        mCollection = CustomerCustomerDemoCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
    public ColumnInfo CustomerID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "CustomerID"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NChar"
        columnInfo.Caption = "Customer ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string CustomerID
    {
        get
        {
            return mCustomerID;
        }
        set
        {
            mCustomerID = value;
        }
    }
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
    #endregion
    End Class
