' ***^^^***|||***^^^***' ' ' ' %%%###%%%c4b2ce3fab0f389dcaa682e55069b367%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Order.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class OrderCollection : CollectionBase
    #region Constructors
    protected OrderCollection() : base( "OrderCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intOrderID,
                    int UserID
    {
        OrderDataAccessor.Fill( this, OrderID, UserID );
    }
    #endregion
}


public class Order : RowBase
    #region Class Level Declarations
    protected Order mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int OrderID;
    private string CustomerID;
    private int EmployeeID;
    private System.DateTime OrderDate;
    private System.DateTime RequiredDate;
    private System.DateTime ShippedDate;
    private int ShipVia;
    private System.Decimal Freight;
    private string ShipName;
    private string ShipAddress;
    private string ShipCity;
    private string ShipRegion;
    private string ShipPostalCode;
    private string ShipCountry;
    #endregion
    #region Constructors
    internal Order( OrderCollection OrderCollection ) : base()
    {
        mCollection = OrderCollection
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
    public ColumnInfo OrderDate()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "OrderDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Order Date"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.DateTime OrderDate
    {
        get
        {
            return mOrderDate;
        }
        set
        {
            mOrderDate = value;
        }
    }
    public ColumnInfo RequiredDate()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "RequiredDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Required Date"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.DateTime RequiredDate
    {
        get
        {
            return mRequiredDate;
        }
        set
        {
            mRequiredDate = value;
        }
    }
    public ColumnInfo ShippedDate()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShippedDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Shipped Date"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.DateTime ShippedDate
    {
        get
        {
            return mShippedDate;
        }
        set
        {
            mShippedDate = value;
        }
    }
    public ColumnInfo ShipVia()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShipVia"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Ship Via"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int ShipVia
    {
        get
        {
            return mShipVia;
        }
        set
        {
            mShipVia = value;
        }
    }
    public ColumnInfo Freight()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Freight"
        columnInfo.FieldType = GetType(System.Decimal)
        columnInfo.SQLType = "Money"
        columnInfo.Caption = "Freight"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Decimal Freight
    {
        get
        {
            return mFreight;
        }
        set
        {
            mFreight = value;
        }
    }
    public ColumnInfo ShipName()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShipName"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Name"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ShipName
    {
        get
        {
            return mShipName;
        }
        set
        {
            mShipName = value;
        }
    }
    public ColumnInfo ShipAddress()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShipAddress"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Address"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ShipAddress
    {
        get
        {
            return mShipAddress;
        }
        set
        {
            mShipAddress = value;
        }
    }
    public ColumnInfo ShipCity()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShipCity"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship City"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ShipCity
    {
        get
        {
            return mShipCity;
        }
        set
        {
            mShipCity = value;
        }
    }
    public ColumnInfo ShipRegion()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShipRegion"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Region"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ShipRegion
    {
        get
        {
            return mShipRegion;
        }
        set
        {
            mShipRegion = value;
        }
    }
    public ColumnInfo ShipPostalCode()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShipPostalCode"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Postal Code"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ShipPostalCode
    {
        get
        {
            return mShipPostalCode;
        }
        set
        {
            mShipPostalCode = value;
        }
    }
    public ColumnInfo ShipCountry()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ShipCountry"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Ship Country"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ShipCountry
    {
        get
        {
            return mShipCountry;
        }
        set
        {
            mShipCountry = value;
        }
    }
    #endregion
    End Class
