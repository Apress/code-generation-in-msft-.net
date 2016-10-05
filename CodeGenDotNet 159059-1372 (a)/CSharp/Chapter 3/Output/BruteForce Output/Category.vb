' ***^^^***|||***^^^***' ' ' ' %%%###%%%93552a6acd01714f027f861785e820ee%%%###%%%' ***^^^***|||***^^^***using System;
using KADGen;
using System;
/// <summary>
/// Category.vb
/// Last Generated on Date: 1/28/2004 9:07:37 AM
/// </summary>

public class CategoryCollection : CollectionBase
    #region Constructors
    protected CategoryCollection() : base( "CategoryCollection" )
    {
    }
    #endregion
    #region Public and Friend Properties, Methods and Events
    public overloads void Fill( 
                    intCategoryID,
                    int UserID
    {
        CategoryDataAccessor.Fill( this, CategoryID, UserID );
    }
    #endregion
}


public class Category : RowBase
    #region Class Level Declarations
    protected Category mCollection;
    private static int mNextPrimaryKey = -1;
    
    private int CategoryID;
    private string CategoryName;
    private string Description;
    private System.Byte() Picture;
    #endregion
    #region Constructors
    internal Category( CategoryCollection CategoryCollection ) : base()
    {
        mCollection = CategoryCollection
    }
    #endregion
    #region Base Class Implementation
    #endregion
    #region Field access properties
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
    public ColumnInfo CategoryName()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "CategoryName"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NVarChar"
        columnInfo.Caption = "Category Name"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string CategoryName
    {
        get
        {
            return mCategoryName;
        }
        set
        {
            mCategoryName = value;
        }
    }
    public ColumnInfo Description()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Description"
        columnInfo.FieldType = GetType(string)
        columnInfo.SQLType = "NText"
        columnInfo.Caption = "Description"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public string Description
    {
        get
        {
            return mDescription;
        }
        set
        {
            mDescription = value;
        }
    }
    public ColumnInfo Picture()
    {
        ColumnInfo columnInfo = new ColumnInfo
        columnInfo.FieldName = "Picture"
        columnInfo.FieldType = GetType(System.Byte())
        columnInfo.SQLType = "Image"
        columnInfo.Caption = "Picture"
        columnInfo.Desc = ""
        return columnInfo
    }
    
    public System.Byte() Picture
    {
        get
        {
            return mPicture;
        }
        set
        {
            mPicture = value;
        }
    }
    #endregion
    End Class
