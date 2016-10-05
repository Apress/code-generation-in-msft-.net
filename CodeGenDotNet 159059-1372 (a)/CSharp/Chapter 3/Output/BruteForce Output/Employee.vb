' ***^^^***|||***^^^***' ' ' ' %%%###%%%b977bba20fde9a956c6fecc0a593aea6%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Employee.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class EmployeeCollection : CollectionBase
    #region Constructors
    protected EmployeeCollection() : base( "EmployeeCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intEmployeeID,
                    int UserID
    {
        EmployeeDataAccessor.Fill( this, EmployeeID, UserID );
    }
    #endregion
}


public class Employee : RowBase
    #region Class Level Declarations
    protected Employee mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int EmployeeID;
    private string LastName;
    private string FirstName;
    private string Title;
    private string TitleOfCourtesy;
    private System.DateTime BirthDate;
    private System.DateTime HireDate;
    private string Address;
    private string City;
    private string Region;
    private string PostalCode;
    private string Country;
    private string HomePhone;
    private string Extension;
    private System.Byte() Photo;
    private string Notes;
    private int ReportsTo;
    private string PhotoPath;
    #endregion
    #region Constructors
    internal Employee( EmployeeCollection EmployeeCollection ) : base()
    {
        mCollection = EmployeeCollection
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
    public ColumnInfo LastName()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "LastName"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Last Name"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string LastName
    {
        get
        {
            return mLastName;
        }
        set
        {
            mLastName = value;
        }
    }
    public ColumnInfo FirstName()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "FirstName"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "First Name"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string FirstName
    {
        get
        {
            return mFirstName;
        }
        set
        {
            mFirstName = value;
        }
    }
    public ColumnInfo Title()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Title"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Title"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Title
    {
        get
        {
            return mTitle;
        }
        set
        {
            mTitle = value;
        }
    }
    public ColumnInfo TitleOfCourtesy()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "TitleOfCourtesy"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Title Of Courtesy"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string TitleOfCourtesy
    {
        get
        {
            return mTitleOfCourtesy;
        }
        set
        {
            mTitleOfCourtesy = value;
        }
    }
    public ColumnInfo BirthDate()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "BirthDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Birth Date"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.DateTime BirthDate
    {
        get
        {
            return mBirthDate;
        }
        set
        {
            mBirthDate = value;
        }
    }
    public ColumnInfo HireDate()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "HireDate"
        columnInfo.FieldType = GetType(System.DateTime)
        columnInfo.SQLType = "DateTime"
        columnInfo.Caption = "Hire Date"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.DateTime HireDate
    {
        get
        {
            return mHireDate;
        }
        set
        {
            mHireDate = value;
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
    public ColumnInfo HomePhone()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "HomePhone"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Home Phone"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string HomePhone
    {
        get
        {
            return mHomePhone;
        }
        set
        {
            mHomePhone = value;
        }
    }
    public ColumnInfo Extension()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Extension"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Extension"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Extension
    {
        get
        {
            return mExtension;
        }
        set
        {
            mExtension = value;
        }
    }
    public ColumnInfo Photo()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Photo"
        columnInfo.FieldType = GetType(System.Byte())
        columnInfo.SQLType = "Image"
        columnInfo.Caption = "Photo"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Byte() Photo
    {
        get
        {
            return mPhoto;
        }
        set
        {
            mPhoto = value;
        }
    }
    public ColumnInfo Notes()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Notes"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NText"
        columnInfo.Caption = "Notes"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Notes
    {
        get
        {
            return mNotes;
        }
        set
        {
            mNotes = value;
        }
    }
    public ColumnInfo ReportsTo()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "ReportsTo"
        columnInfo.FieldType = GetType(int)
        columnInfo.SQLType = "Int"
        columnInfo.Caption = "Reports To"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public int ReportsTo
    {
        get
        {
            return mReportsTo;
        }
        set
        {
            mReportsTo = value;
        }
    }
    public ColumnInfo PhotoPath()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "PhotoPath"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Photo Path"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string PhotoPath
    {
        get
        {
            return mPhotoPath;
        }
        set
        {
            mPhotoPath = value;
        }
    }
    #endregion
    End Class
