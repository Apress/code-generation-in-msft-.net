' ***^^^***|||***^^^***' ' ' ' %%%###%%%2cb8c4ab0dd004b8b1d9a7bae73759d6%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Product.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class ProductCollection : CollectionBase
    #region Constructors
    protected ProductCollection() : base( "ProductCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intProductID,
                    int UserID
    {
        ProductDataAccessor.Fill( this, ProductID, UserID );
    }
    #endregion
}


public class Product : RowBase
    #region Class Level Declarations
    protected Product mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int ProductID;
    private string ProductName;
    private int SupplierID;
    private int CategoryID;
    private string QuantityPerUnit;
    private System.Decimal UnitPrice;
    private System.Int16 UnitsInStock;
    private System.Int16 UnitsOnOrder;
    private System.Int16 ReorderLevel;
    private bool Discontinued;
    #endregion
    #region Constructors
    internal Product( ProductCollection ProductCollection ) : base()
    {
        mCollection = ProductCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
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
    public ColumnInfo ProductName()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ProductName"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Product Name"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ProductName
    {
        get
        {
            return mProductName;
        }
        set
        {
            mProductName = value;
        }
    }
    public ColumnInfo SupplierID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "SupplierID"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Supplier ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int SupplierID
    {
        get
        {
            return mSupplierID;
        }
        set
        {
            mSupplierID = value;
        }
    }
    public ColumnInfo CategoryID()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "CategoryID"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Category ID"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int CategoryID
    {
        get
        {
            return mCategoryID;
        }
        set
        {
            mCategoryID = value;
        }
    }
    public ColumnInfo QuantityPerUnit()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "QuantityPerUnit"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Quantity Per Unit"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string QuantityPerUnit
    {
        get
        {
            return mQuantityPerUnit;
        }
        set
        {
            mQuantityPerUnit = value;
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
    public ColumnInfo UnitsInStock()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "UnitsInStock"
        columnInfo.FieldType = GetType(System.Int16)
        columnInfo.SQLType = "SmallInt"
        columnInfo.Caption = "Units In Stock"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Int16 UnitsInStock
    {
        get
        {
            return mUnitsInStock;
        }
        set
        {
            mUnitsInStock = value;
        }
    }
    public ColumnInfo UnitsOnOrder()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "UnitsOnOrder"
        columnInfo.FieldType = GetType(System.Int16)
        columnInfo.SQLType = "SmallInt"
        columnInfo.Caption = "Units On Order"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Int16 UnitsOnOrder
    {
        get
        {
            return mUnitsOnOrder;
        }
        set
        {
            mUnitsOnOrder = value;
        }
    }
    public ColumnInfo ReorderLevel()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ReorderLevel"
        columnInfo.FieldType = GetType(System.Int16)
        columnInfo.SQLType = "SmallInt"
        columnInfo.Caption = "Reorder Level"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Int16 ReorderLevel
    {
        get
        {
            return mReorderLevel;
        }
        set
        {
            mReorderLevel = value;
        }
    }
    public ColumnInfo Discontinued()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Discontinued"
        columnInfo.FieldType = GetType(bool)
        columnInfo.SQLType = "Bit"
        columnInfo.Caption = "Discontinued"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public bool Discontinued
    {
        get
        {
            return mDiscontinued;
        }
        set
        {
            mDiscontinued = value;
        }
    }
    #endregion
    End Class
