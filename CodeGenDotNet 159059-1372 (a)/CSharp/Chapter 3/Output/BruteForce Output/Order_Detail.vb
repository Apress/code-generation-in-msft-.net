' ***^^^***|||***^^^***' ' ' ' %%%###%%%76aa03ffa6f5a9feb243f39f8985579f%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Order_Detail.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class Order_DetailCollection : CollectionBase
    #region Constructors
    protected Order_DetailCollection() : base( "Order_DetailCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intOrderID,
                    int UserID
    {
        Order_DetailDataAccessor.Fill( this, OrderID, UserID );
    }
    #endregion
}


public class Order_Detail : RowBase
    #region Class Level Declarations
    protected Order_Detail mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int OrderID;
    private int ProductID;
    private System.Decimal UnitPrice;
    private System.Int16 Quantity;
    private System.Single Discount;
    #endregion
    #region Constructors
    internal Order_Detail( Order_DetailCollection Order_DetailCollection ) : base()
    {
        mCollection = Order_DetailCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
    public ColumnInfo OrderID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "OrderID"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Order ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int OrderID
    {
        get
        {
            return mOrderID;
        }
        set
        {
            mOrderID = value;
        }
    }
    public ColumnInfo ProductID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ProductID"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Product ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int ProductID
    {
        get
        {
            return mProductID;
        }
        set
        {
            mProductID = value;
        }
    }
    public ColumnInfo UnitPrice()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "UnitPrice"
        columnInfo.FieldType = GetType(System.Decimal)
        columnInfo.SQLType = "Money"
        columnInfo.Caption = "Unit Price"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Decimal UnitPrice
    {
        get
        {
            return mUnitPrice;
        }
        set
        {
            mUnitPrice = value;
        }
    }
    public ColumnInfo Quantity()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Quantity"
        columnInfo.FieldType = GetType(System.Int16)
        columnInfo.SQLType = "SmallInt"
        columnInfo.Caption = "Quantity"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Int16 Quantity
    {
        get
        {
            return mQuantity;
        }
        set
        {
            mQuantity = value;
        }
    }
    public ColumnInfo Discount()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Discount"
        columnInfo.FieldType = GetType(System.Single)
        columnInfo.SQLType = "Real"
        columnInfo.Caption = "Discount"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Single Discount
    {
        get
        {
            return mDiscount;
        }
        set
        {
            mDiscount = value;
        }
    }
    #endregion
    End Class
