' ***^^^***|||***^^^***' ' ' ' %%%###%%%28f1f1a305619ce55b971271562a0f44%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Supplier.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class SupplierCollection : CollectionBase
    #region Constructors
    protected SupplierCollection() : base( "SupplierCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intSupplierID,
                    int UserID
    {
        SupplierDataAccessor.Fill( this, SupplierID, UserID );
    }
    #endregion
}


public class Supplier : RowBase
    #region Class Level Declarations
    protected Supplier mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int SupplierID;
    private string CompanyName;
    private string ContactName;
    private string ContactTitle;
    private string Address;
    private string City;
    private string Region;
    private string PostalCode;
    private string Country;
    private string Phone;
    private string Fax;
    private string HomePage;
    #endregion
    #region Constructors
    internal Supplier( SupplierCollection SupplierCollection ) : base()
    {
        mCollection = SupplierCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
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
    public ColumnInfo ContactName()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ContactName"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Contact Name"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ContactName
    {
        get
        {
            return mContactName;
        }
        set
        {
            mContactName = value;
        }
    }
    public ColumnInfo ContactTitle()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ContactTitle"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Contact Title"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string ContactTitle
    {
        get
        {
            return mContactTitle;
        }
        set
        {
            mContactTitle = value;
        }
    }
    public ColumnInfo Address()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Address"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Address"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Address
    {
        get
        {
            return mAddress;
        }
        set
        {
            mAddress = value;
        }
    }
    public ColumnInfo City()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "City"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "City"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string City
    {
        get
        {
            return mCity;
        }
        set
        {
            mCity = value;
        }
    }
    public ColumnInfo Region()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Region"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Region"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Region
    {
        get
        {
            return mRegion;
        }
        set
        {
            mRegion = value;
        }
    }
    public ColumnInfo PostalCode()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "PostalCode"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Postal Code"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string PostalCode
    {
        get
        {
            return mPostalCode;
        }
        set
        {
            mPostalCode = value;
        }
    }
    public ColumnInfo Country()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Country"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Country"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Country
    {
        get
        {
            return mCountry;
        }
        set
        {
            mCountry = value;
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
    public ColumnInfo Fax()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Fax"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Fax"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Fax
    {
        get
        {
            return mFax;
        }
        set
        {
            mFax = value;
        }
    }
    public ColumnInfo HomePage()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "HomePage"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NText"
        columnInfo.Caption = "Home Page"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string HomePage
    {
        get
        {
            return mHomePage;
        }
        set
        {
            mHomePage = value;
        }
    }
    #endregion
    End Class
