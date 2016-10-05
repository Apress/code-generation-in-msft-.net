<?xml version="1.0" encoding="UTF-8" ?>
<!--
  ====================================================================
   Copyright ©2004, Kathleen Dollard, All Rights Reserved.
  ====================================================================
   I'm distributing this code so you'll be able to use it to see code
   generation in action and I hope it will be useful and you'll enjoy 
   using it. This code is provided "AS IS" without warranty, either 
   expressed or implied, including implied warranties of merchantability 
   and/or fitness for a particular purpose. 
  ====================================================================
  Summary:  Generates plumbing class for lists!-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" >
<xsl:import href="CSLASupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$Name]//orm:SetSelectSP" 
								mode="SetSelectSP"/>
</xsl:template>

<xsl:template match="orm:SetSelectSP" mode="SetSelectSP">
using System;
using System.Data;
using System.Data.SqlClient;
using CSLA.Data;
using CSLA;
using KADGen.BusinessObjectSupport;

namespace KADGen.BusinessObjects
{
   [Serializable()]
   public class gen<xsl:value-of select="$Name"/>List : ReadOnlyCollectionBase
   {
	   private static <xsl:value-of select="$Name"/> m<xsl:value-of select="$Name"/>;
	   <xsl:call-template name="DataStructure"/>
	   <xsl:call-template name="BusinessPropertiesAndMethods"/>
	   <xsl:call-template name="SharedMethods"/>
	   <xsl:call-template name="Criteria"/>
	   <xsl:call-template name="Constructors"/>
	   <xsl:call-template name="DataAccess"/>
   }
}
   </xsl:template>

   <xsl:template name="DataStructure">
	   #region Data Structure

	   [Serializable()]
	   public class <xsl:value-of select="$Name"/>Info : KADGen.BusinessObjectSupport.IListInfo 
	   {
		   // this has private members, public properties because
		   // ASP.NET can't databind to public members of a structure...
		   <xsl:for-each select="orm:SPRecordSet[1]/orm:Column">
		   private <xsl:value-of select="@NETType"/> m<xsl:value-of select="@Name"/>;
		   </xsl:for-each>

		   <xsl:for-each select="orm:SPRecordSet[1]/orm:Column">
		   public <xsl:value-of select="@NETType"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
		   {
			   get
			   {
				   return m<xsl:value-of select="@Name"/>;
			   }
			   set
			   {
				   m<xsl:value-of select="@Name"/> = value;
			   }
		   }
		   </xsl:for-each> 

		   <xsl:if test="count(orm:SPRecordSet)" > 
		   public bool Equals( <xsl:value-of select="$Name"/>Info info )
		   {
			   return <xsl:text/>
			   <xsl:for-each select="orm:SPRecordSet[1]/orm:Column">m<xsl:text/>
				   <xsl:value-of select="@Name"/>.Equals(info.<xsl:text/>
				   <xsl:value-of select="@Name"/>)<xsl:text/>
				   <xsl:if test="last() != position()"> &amp;&amp; 
				   </xsl:if>
			   </xsl:for-each>;
		   }
		   </xsl:if>

		   public string UniqueKey
		   {
			   get
			   {
				   return <xsl:text/>
				   <!-- THe followign assumes all primary keys are in set select -->
				   <xsl:for-each select="../orm:Property[@IsPrimaryKey='true']">
					   <xsl:text/>m<xsl:value-of select="@Name"/>.ToString()<xsl:if test="position()!=last()"> + </xsl:if>
				   </xsl:for-each>;
			   }
			   set
			   {
				   throw new System.Exception( "Unexpected call to setUniqueKey" );
			   }
		   }

		   public string DisplayText
		   {
			   get
			   {
				   return ToString();
			   }
			   set
			   {
				   throw new System.Exception( "Unexpected call to DisplayText" );
			   }
		   }

		   public int GetHashCode()
		   {
			   return UniqueKey.GetHashCode();
		   }
   		
		   public void AssignFromDataReader( SafeDataReader dr )
		   {
			   <xsl:apply-templates select="orm:SPRecordSet[1]/orm:Column" mode="RetrieveFromReader">
				   <xsl:with-param name="forcefield" select="'true'" />
				   <xsl:with-param name="useme" select="'false'" />
			   </xsl:apply-templates>
		   }
   		
		   <xsl:if test="count(..//orm:Property[@UseForDesc='true'])>0">
		   public string ToString()
		   {
				// FRED<xsl:value-of select="name()"/>
				// FRED<xsl:value-of select="@Name"/>
			   return <xsl:text />
			   <xsl:for-each select="..//orm:Property[@UseForDesc='true']">
				   <xsl:sort select="@UseForDescOrdinal" data-type="number" />
				   <xsl:text/>m<xsl:value-of select="@Name"/>.ToString()<xsl:text/>
				   <xsl:if test="position()!=last()"> 
					   <xsl:text/> + "<xsl:text/>
					   <xsl:choose>
						   <xsl:when test="@UseForDescDelimiter">
							   <xsl:value-of select="@UseForDescDelimiter"  />
						   </xsl:when>
						   <xsl:otherwise>, </xsl:otherwise>
					   </xsl:choose>" +
				   </xsl:if>
			   </xsl:for-each>;<xsl:text/>
			   <!-- <xsl:for-each select="..//orm:Property[@UseForDesc='true']">
			   <xsl:text/>m<xsl:value-of select="@Name"/>.ToString<xsl:if test="position()!=last()"> &amp; ", " &amp; </xsl:if>
			   </xsl:for-each> -->
		   }
		   </xsl:if>
   	 
		   public object GetPrimaryKey()
		   {
			   <xsl:choose>
				   <xsl:when test="count(..//orm:Property[@IsPrimaryKey='true'])>1">
					   throw new System.ApplicationException( "This function doesn't work for multi-column primary keys" );
				   </xsl:when>
				   <xsl:otherwise>
					   <xsl:for-each select="..//orm:Property[@IsPrimaryKey='true']">
						   return <xsl:value-of select="@Name"/>;
					   </xsl:for-each>
				   </xsl:otherwise>
			   </xsl:choose>
		   }

	   }

	   #endregion

   </xsl:template>


   <xsl:template name="BusinessPropertiesAndMethods">
	   #region Business Properties and Methods

	   public <xsl:value-of select="$Name"/>Info this[int Index]
	   {
		   get
		   {
			   return (<xsl:value-of select="$Name"/>Info)List[Index];
		   }
	   }
   	  
	   public <xsl:value-of select="$Name"/>Info Get<xsl:value-of select="$Name"/>Info( <xsl:text/>
	   <xsl:if test="count(../orm:Property[@IsPrimaryKey='true'])">
		   <xsl:for-each select="../orm:Property[@IsPrimaryKey='true']"><xsl:text>
				   </xsl:text>
			   <xsl:value-of select="@NETType" /><xsl:text> </xsl:text>
			   <xsl:value-of select="@Name"/>
			   <xsl:if test="last()!=position()">, </xsl:if>
		   </xsl:for-each>
	   </xsl:if> )
	   {
		   <xsl:if test="count(../orm:Property[@IsPrimaryKey='true'])"> 
		   for( int i = 0; i&lt;this.Count; i++ )
		   {
			   if( <xsl:text/>
				   <xsl:for-each select="../orm:Property[@IsPrimaryKey='true']">
					   <xsl:call-template name="Comparison">
						   <xsl:with-param name="first" select="concat('this[i].',@Name)"/>
						   <xsl:with-param name="second" select="@Name"/>
						   <xsl:with-param name="operand" select="'='"/>
						   <xsl:with-param name="type" select="@NETType"/>
					   </xsl:call-template> 
					   <xsl:if test="position()!=last()"> &amp; </xsl:if>
				   </xsl:for-each> )
				   return this[i];
		   }
			   </xsl:if> 
		   return null;
	   }


	   #endregion

	   #region Contains

	   public bool Contains( <xsl:value-of select="$Name"/>Info item )
	   {
		   foreach( <xsl:value-of select="$Name"/>Info child in List )
		   {
			   if( child.Equals( item ) )
				   return true;
		   }
		   return false;
	   }

	   #endregion

   </xsl:template>

   <xsl:template name="SharedMethods">
	   #region Static Methods

	   public static <xsl:value-of select="$Name"/>List Get<xsl:value-of select="$Name"/>List()
	   {
		   return (<xsl:value-of select="$Name"/>List)DataPortal.Fetch( new <xsl:value-of select="$Name"/>List.Criteria() );
	   }

	   #endregion

   </xsl:template>


   <xsl:template name="Criteria">
	   #region Criteria

	   [Serializable()]
	   public class Criteria : CSLA.Server.CriteriaBase
	   {
		   // no criteria - we retrieve all <xsl:value-of select="$Name"/>
       
		   public Criteria() : base( typeof( <xsl:value-of select="$Name"/>List ) )
		   {
		   }
	   }

	   #endregion

   </xsl:template>


   <xsl:template name="Constructors">
	   #region Constructors

	   protected gen<xsl:value-of select="$Name"/>List()
	   {
			   // prevent direct creation
	   }

	   #endregion

   </xsl:template>


   <xsl:template name="DataAccess">
	   #region Data Access

	   protected void DataPortal_Fetch( object Criteria )
	   {
		   SqlConnection cn = new SqlConnection( DB( "<xsl:value-of select="../@MapDataStructure"/>" ) );
		   SqlCommand cm = new SqlCommand();

		   cn.Open();
		   try
		   {
			   cm.Connection = cn;
			   cm.CommandType = CommandType.StoredProcedure;
			   cm.CommandText = "<xsl:value-of select="@Name"/>";

			   SafeDataReader dr = new SafeDataReader( cm.ExecuteReader() );
			   try
			   {
				   while( dr.Read() )
				   {
					   <xsl:value-of select="$Name"/>Info info = new <xsl:value-of select="$Name"/>Info();
					   info.AssignFromDataReader( dr );
					   InnerList.Add( info );
				   }
			   }
			   finally
			   {
				   dr.Close();
			   }
		   }
		   finally
		   {
			   cn.Close();
		   }
	   }

	   #endregion

   </xsl:template>

</xsl:stylesheet> 
  
