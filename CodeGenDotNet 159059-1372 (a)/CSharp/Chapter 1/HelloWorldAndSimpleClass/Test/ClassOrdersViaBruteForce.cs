using System;

/// <summary>
/// 
/// </summar>
public class TargetOutput
{
    #region Class level declarations
    private Int32 m_OrderID;
    private String m_CustomerID;
    private Int32 m_EmployeeID;
    private DateTime m_OrderDate;
    private DateTime m_RequiredDate;
    private DateTime m_ShippedDate;
    private Int32 m_ShipVia;
    private Decimal m_Freight;
    private String m_ShipName;
    private String m_ShipAddress;
    private String m_ShipCity;
    private String m_ShipRegion;
    private String m_ShipPostalCode;
    private String m_ShipCountry;
    #endregion
    
    #region Public Methods and Properties
    public Int32 OrderID
    {
        get
        {
            return m_OrderID;
        }
        set
        {
            m_OrderID = value;
        }
    }
    
    public String CustomerID
    {
        get
        {
            return m_CustomerID;
        }
        set
        {
            m_CustomerID = value;
        }
    }
    
    public Int32 EmployeeID
    {
        get
        {
            return m_EmployeeID;
        }
        set
        {
            m_EmployeeID = value;
        }
    }
    
    public DateTime OrderDate
    {
        get
        {
            return m_OrderDate;
        }
        set
        {
            m_OrderDate = value;
        }
    }
    
    public DateTime RequiredDate
    {
        get
        {
            return m_RequiredDate;
        }
        set
        {
            m_RequiredDate = value;
        }
    }
    
    public DateTime ShippedDate
    {
        get
        {
            return m_ShippedDate;
        }
        set
        {
            m_ShippedDate = value;
        }
    }
    
    public Int32 ShipVia
    {
        get
        {
            return m_ShipVia;
        }
        set
        {
            m_ShipVia = value;
        }
    }
    
    public Decimal Freight
    {
        get
        {
            return m_Freight;
        }
        set
        {
            m_Freight = value;
        }
    }
    
    public String ShipName
    {
        get
        {
            return m_ShipName;
        }
        set
        {
            m_ShipName = value;
        }
    }
    
    public String ShipAddress
    {
        get
        {
            return m_ShipAddress;
        }
        set
        {
            m_ShipAddress = value;
        }
    }
    
    public String ShipCity
    {
        get
        {
            return m_ShipCity;
        }
        set
        {
            m_ShipCity = value;
        }
    }
    
    public String ShipRegion
    {
        get
        {
            return m_ShipRegion;
        }
        set
        {
            m_ShipRegion = value;
        }
    }
    
    public String ShipPostalCode
    {
        get
        {
            return m_ShipPostalCode;
        }
        set
        {
            m_ShipPostalCode = value;
        }
    }
    
    public String ShipCountry
    {
        get
        {
            return m_ShipCountry;
        }
        set
        {
            m_ShipCountry = value;
        }
    }
    
    #endregion
    
}
